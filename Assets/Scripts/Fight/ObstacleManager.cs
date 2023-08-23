using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
	public static NativeList<int2> obstacles;

	private void Awake() {
		obstacles = new NativeList<int2>(Allocator.Persistent);
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Obstacle");

		for (int x = 0; x < 25; x++) {
			for (int y = 0; y < 25; y++) {
				foreach (GameObject gameObject in gameObjects) {
					if (gameObject.GetComponent<BoxCollider2D>().bounds.Contains(new Vector2(x, y)))
						obstacles.Add(new int2(x, y));
				}
			}
		}
	}

	private void OnDisable() {
		obstacles.Dispose();
	}
}
