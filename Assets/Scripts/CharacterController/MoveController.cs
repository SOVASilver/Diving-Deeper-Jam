using System;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour {
	private NativeList<int2> _path;
	private float _speed = 12;
	private Vector2 _target;
	[SerializeField] private PathFinding _pathFinding;
	[SerializeField] private Rigidbody2D _rb;
	[SerializeField] private CharacterManager _characterManager;

	private void OnMouseDown() {
		_characterManager.unit = gameObject;
	}

	private void Awake() {
		_path = new (Allocator.Persistent);
	}

	private void FixedUpdate() {
		if (_characterManager.unit == gameObject) {
			if (Input.GetMouseButtonDown(1)) {
				float startTime = Time.realtimeSinceStartup;
				_path.CopyFrom(_pathFinding.path);
				Debug.Log((Time.realtimeSinceStartup - startTime) * 1000);
			}
		}

		if(!_path.IsEmpty) {
			_target = new Vector2(Mathf.MoveTowards(transform.position.x, _path[_path.Length - 1].x, _speed * Time.fixedDeltaTime), Mathf.MoveTowards(transform.position.y, _path[_path.Length - 1].y, _speed * Time.fixedDeltaTime));
			_rb.MovePosition(_target);

			if (_rb.position.x == _path[_path.Length - 1].x && _rb.position.y == _path[_path.Length - 1].y)
				_path.RemoveAt(_path.Length - 1);
		}
	}

	private void OnDisable() {
		_path.Dispose();
	}

	private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
	private int2 GetRoundToIntPosition(Vector2 position) => new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
}
