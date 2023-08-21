using System;
using UnityEngine;

public abstract class Mutation : MonoBehaviour {
	public string MutationName { get; protected set; }
	public abstract void ModifyStats(Stats stats);
}