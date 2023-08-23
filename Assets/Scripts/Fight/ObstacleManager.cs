using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
	public NativeList<int2> obstacles;

	void Awake() {
		obstacles = new NativeList<int2>(Allocator.Persistent);
		GameObject[] collider = GameObject.FindGameObjectsWithTag("Obstacle");

		for (int x = 0; x < 24; x++) 
			for(int y = 0; y < 24; y++) {

			}
	}
}
