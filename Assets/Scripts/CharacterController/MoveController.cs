using System;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour {
	private NativeList<int2> _path;
	private float _speed = 8;
	private Vector2 _target;
	private bool _isMoving = false;
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

	private void FixedUpdate() {
		if (Input.GetMouseButtonDown(1)) {
			float startTime = Time.realtimeSinceStartup;
			_pathFinding.FindPath(new int2(GetRoundToIntPosition(_rb.position)), new int2(GetRoundToIntPosition(GetMousePosition())), _path);
			Debug.Log((Time.realtimeSinceStartup - startTime) * 1000);
		}

		if (Input.GetKeyDown(KeyCode.W))
			_isMoving = !_isMoving;

		if (_isMoving && !_path.IsEmpty) {
			_target = new Vector2(Mathf.MoveTowards(transform.position.x, _path[_path.Length - 1].x, _speed * Time.fixedDeltaTime), Mathf.MoveTowards(transform.position.y, _path[_path.Length - 1].y, _speed * Time.fixedDeltaTime));
			_rb.MovePosition(_target);

			if (_rb.position.x == _path[_path.Length - 1].x && _rb.position.y == _path[_path.Length - 1].y)
				_path.RemoveAt(_path.Length - 1);
		}

		if(Input.GetKeyDown(KeyCode.S))
			foreach(int2 node in _path)
				Debug.Log(node);

		if (Input.GetKeyDown(KeyCode.D))
			Debug.Log(GetRoundToIntPosition(GetMousePosition()));
	}

	private void OnDisable() {
		_path.Dispose();
	}

	private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
	private int2 GetRoundToIntPosition(Vector2 position) => new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
}
