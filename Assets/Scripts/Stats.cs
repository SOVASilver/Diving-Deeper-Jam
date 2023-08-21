using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Stats : MonoBehaviour {
	[SerializeField] private float _maxHealth, _speed, _accuracy;
	[SerializeReference, MutationList] private List<Mutation> _mutations = new List<Mutation>() { new HandsDystrophy() };
	private float _health;

	private void Awake() {
		_mutations.Add(new HandsDystrophy());
		_mutations.Add(new HandsDystrophy());
		_mutations.Add(new HandsDystrophy());
		_mutations.Add(new HandsDystrophy());
		_health = _maxHealth;

		Debug.Log(_mutations[0].ToString());
		foreach (Mutation mutation in _mutations)
			mutation.ModifyStats(this);
		
	}

	public void ModifyHealth(float value) {
		_health -= value;

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