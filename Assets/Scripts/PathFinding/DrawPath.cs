using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
public class DrawPath : MonoBehaviour {
	[SerializeField] private PathFinding _pathFinding;
	[SerializeField] private CharacterManager _characterManager;
	[SerializeField] private GameObject _prefab;

	private List<GameObject> _prefabs = new ();
	private int2 _pastPosition = new int2(int.MinValue, int.MinValue);
	private int2 _currentPosition;

	private void Update() {
		_currentPosition = GetRoundToIntPosition(GetMousePosition());
		if (_characterManager.unit == gameObject && !_pastPosition.Equals(_currentPosition)) {
			if (_currentPosition.x > 0 && _currentPosition.y > 0 && _currentPosition.x < 34 && _currentPosition.y < 15) {
				_pathFinding.FindPath(new int2(GetRoundToIntPosition(transform.position)), _currentPosition);
				Delete();
				Draw();
			}
		}
		else if (_characterManager.unit != gameObject)
			Delete();
	}

	private void Draw() {
		if(!_pathFinding.path.IsEmpty) {
			foreach (int2 node in _pathFinding.path) {
				_prefabs.Add(Instantiate(_prefab, new Vector2(node.x, node.y), Quaternion.identity, _characterManager.transform));
			}
		}
	}

	private void Delete() {
		if (_prefabs.Count > 0) {
			foreach (GameObject gameObject in _prefabs) {
				Destroy(gameObject);
			}
		}
	}

	private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
	private int2 GetRoundToIntPosition(Vector2 position) => new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
}