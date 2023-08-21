public class HandsDystrophy : Mutation {
	public HandsDystrophy() => MutationName = GetType().FullName;

	public override void ModifyStats(Stats stats) {
		stats.ModifyAccuracy(-15);
	}
}