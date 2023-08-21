using System;

[Serializable]
public abstract class Mutation {
	public string _name = "base";

	public abstract void ModifyStats(Stats stats);
}