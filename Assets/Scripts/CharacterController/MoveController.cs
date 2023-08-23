using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour {
	private NativeList<int2> _path;
	[SerializeField] private PathFinding _pathFinding;
	[SerializeField] private Rigidbody2D _rb;
	[SerializeField] private CharacterManager _characterManager;

	private void OnMouseDown() {
		Debug.Log("I'm Working!");
		_characterManager.unit = gameObject;
	}

	private void Awake() {
		_path = new (Allocator.Persistent);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(1)) {
			try {
				float startTime = Time.realtimeSinceStartup;
				_pathFinding.FindPath(new int2(GetRoundToIntPosition(_rb.position)), new int2(GetRoundToIntPosition(GetMousePosition())), _path);
				Debug.Log((Time.realtimeSinceStartup - startTime) * 1000);
			}
			catch (Exception exception) {
				Debug.LogError(exception.Message);
			}
		}

		if(Input.GetKeyDown(KeyCode.W))
			foreach(int2 node in _path)
				Debug.Log(node);
	}

	private void OnDisable() {
		_path.Dispose();
	}

	private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
	private int2 GetRoundToIntPosition(Vector2 position) => new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
}
