using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Stats : MonoBehaviour {
	[SerializeField] private float _maxHealth, _speed, _accuracy;
	[SerializeField] private Mutation _onlyMeleeWeapon;
	[SerializeReference] private List<Mutation> _mutations = new List<Mutation>();
	[ReadOnly] private float _normalMaxHealth = 100, _normalSpeed = 8, _normalAccuracy = 80;
	private float _health;

	private void Awake() {
		CalculateStats();
		Debug.Log(_mutations[0].MutationName);
	}

	private void Update() {
		if (Time.realtimeSinceStartup > 7 && !_mutations.Contains(_onlyMeleeWeapon)) 
			AddMutation(_onlyMeleeWeapon);
	}

	private void CalculateStats() {
		_maxHealth = _normalMaxHealth;
		_speed = _normalSpeed;
		_accuracy = _normalAccuracy;

		foreach (Mutation mutation in _mutations)
			mutation.ModifyStats(this);
	}

	public void AddMutation(Mutation mutation) {
		_mutations.Add(mutation);

		CalculateStats();
	}

	public void ModifyHealth(float value) {
		_health += value;

		if (_health <= 0)
			Destroy(gameObject);
		else if(_health > _maxHealth)
			_health = _maxHealth;
	}

	public void ModifyMaxHealth(float value) {
		_maxHealth += value;

		if (_maxHealth <= 0) 
			_maxHealth = 1;
	}

	public void ModifyAccuracy(float value) {
		_accuracy += value;

		if(_accuracy < 0) 
			_accuracy = 0;
	}
}