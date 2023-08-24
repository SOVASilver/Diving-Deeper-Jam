using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
public class DrawPath : MonoBehaviour {
	[SerializeField] private PathFinding _pathFinding;
	[SerializeField] private CharacterManager _characterManager;
	[SerializeField] private GameObject _prefab;

	private List<GameObject> _prefabs = new ();
	private int2 _position = new int2(int.MinValue, int.MinValue);

	private void Update() {
		if (_characterManager.unit == gameObject && !_position.Equals(GetRoundToIntPosition(GetMousePosition()))) {
			_pathFinding.FindPath(new int2(GetRoundToIntPosition(transform.position)), GetRoundToIntPosition(GetMousePosition()));
			Delete();
			Draw();
		}
	}

	private void Draw() {
		if(!_pathFinding.path.IsEmpty) {
			foreach (int2 node in _pathFinding.path) {
				_prefabs.Add(Instantiate(_prefab, new Vector2(node.x, node.y), Quaternion.identity));
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