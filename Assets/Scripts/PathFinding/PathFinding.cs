using System;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding : MonoBehaviour {
	public NativeList<int2> path;

	public void Awake() {
		path = new NativeList<int2>(Allocator.Persistent);
	}

	public void OnDisable() {
		path.Dispose();
	}

	public void FindPath(int2 startPosition, int2 endPosition) {
		FindPathJob findPathJob = new FindPathJob {
			startPosition = startPosition,
			endPosition = endPosition,
			obstacles = ObstacleManager.obstacles,
			path = path
		};
		findPathJob.Run();
	}

	[BurstCompile]
	private struct FindPathJob : IJob {
		public int2 startPosition;
		public int2 endPosition;
		public NativeArray<int2> obstacles;
		public NativeList<int2> path;

		public void Execute() {
			path.Clear();

			NativeArray<PathNode> pathNodeArray = new(25 * 25, Allocator.Temp);

			for (int x = 0; x < 25; x++) {
				for (int y = 0; y < 25; y++) {
					PathNode pathNode = new();
					pathNode.position = new(x, y);
					pathNode.index = CalculateIndex(pathNode.position, 25);

					pathNode.gCost = int.MaxValue;
					pathNode.hCost = CalculateDistanceCost(pathNode.position, endPosition);
					pathNode.CalculateFCost();
					pathNode.isWalkable = !obstacles.Contains(new int2(x, y));
					pathNode.cameFromIndex = -1;

					pathNodeArray[pathNode.index] = pathNode;
				}
			}

			int endNodeIndex = CalculateIndex(endPosition, 25);
			PathNode startNode = pathNodeArray[CalculateIndex(startPosition, 25)];
			startNode.gCost = 0;
			startNode.CalculateFCost();
			pathNodeArray[startNode.index] = startNode;

			NativeList<int> openList = new(Allocator.Temp);
			NativeList<int> closedList = new(Allocator.Temp);
			NativeArray<int2> neighbourOffsetArray = new(4, Allocator.Temp);
			neighbourOffsetArray[0] = new int2(-1, 0);
			neighbourOffsetArray[1] = new int2(1, 0);
			neighbourOffsetArray[2] = new int2(0, -1);
			neighbourOffsetArray[3] = new int2(0, 1);
			/*neighbourOffsetArray[4] = new int2(-1, -1);
			neighbourOffsetArray[5] = new int2(-1, 1);
			neighbourOffsetArray[6] = new int2(1, -1);
			neighbourOffsetArray[7] = new int2(1, 1);*/

			openList.Add(startNode.index);
			while (openList.Length > 0) {
				int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
				PathNode currentNode = pathNodeArray[currentNodeIndex];

				if (currentNodeIndex == endNodeIndex) {
					break;
				}

				for (int i = 0; i < openList.Length; i++) {
					if (openList[i] == currentNodeIndex) {
						openList.RemoveAtSwapBack(i);
						break;
					}
				}

				closedList.Add(currentNodeIndex);

				for (int i = 0; i < neighbourOffsetArray.Length; i++) {
					int2 neighbourOffset = neighbourOffsetArray[i];
					int2 neighbourPosition = new(currentNode.position.x + neighbourOffset.x, currentNode.position.y + neighbourOffset.y);

					if (!IsPositionInsideGrid(neighbourPosition, new int2(25, 25))) {
						continue;
					}

					int neighbourNodeIndex = CalculateIndex(neighbourPosition, 25);
					if (closedList.Contains(neighbourNodeIndex)) {
						continue;
					}

					PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
					if (!neighbourNode.isWalkable) {
						continue;
					}

					int2 currentNodePosition = new(currentNode.position);
					int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
					if (tentativeGCost < neighbourNode.gCost) {
						neighbourNode.cameFromIndex = currentNodeIndex;
						neighbourNode.gCost = tentativeGCost;
						neighbourNode.CalculateFCost();
						pathNodeArray[neighbourNodeIndex] = neighbourNode;

						if (!openList.Contains(neighbourNode.index))
							openList.Add(neighbourNode.index);
					}
				}
			}

			PathNode endNode = pathNodeArray[endNodeIndex];
			if (endNode.cameFromIndex == -1) {

			}
			else {
				CalculatePath(pathNodeArray, endNode);
			}

			pathNodeArray.Dispose();
			openList.Dispose();
			closedList.Dispose();
			neighbourOffsetArray.Dispose();
		}
		private bool IsPositionInsideGrid(int2 position, int2 gridSize) {
			return position.x >= 0 && position.y >= 0 &&
				   position.x < gridSize.x && position.y < gridSize.y;
		}

		private int CalculateDistanceCost(int2 firstPosition, int2 secondPosition) {
			int2 distance = new(math.abs(firstPosition.x - secondPosition.x),
								 math.abs(firstPosition.y - secondPosition.y));
			int remaining = math.abs(distance.x - distance.y);
			return 14 * math.min(distance.x, distance.y) + 10 * remaining;
		}

		private int GetLowestCostFNodeIndex(NativeArray<int> openList, NativeArray<PathNode> pathNodeArray) {
			PathNode lowestCostPathNode = pathNodeArray[openList[0]];
			for (int i = 1; i < openList.Length; i++) {
				PathNode testPathNode = pathNodeArray[openList[i]];
				if (testPathNode.fCost < lowestCostPathNode.fCost) {
					lowestCostPathNode = testPathNode;
				}
			}

			return lowestCostPathNode.index;
		}

		private void CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode) {
			if (endNode.cameFromIndex == -1) {
				path.Clear();
			}
			else {
				path.Add(new int2(endNode.position));

				PathNode currentNode = endNode;
				while (currentNode.cameFromIndex != -1) {
					PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
					path.Add(new int2(cameFromNode.position));
					currentNode = cameFromNode;
				}
			}
		}

		public int CalculateIndex(int2 position, int gridWidth) {
			return position.x + position.y * gridWidth;
		}

		private struct PathNode {
			public int2 position;
			public int index;
			public int cameFromIndex;

			public int gCost, hCost, fCost;
			public bool isWalkable;

			public void CalculateFCost() {
				fCost = gCost + hCost;
			}
		}

	}
}
