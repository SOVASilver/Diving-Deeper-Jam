public class HandsDystrophy : Mutation {
	public HandsDystrophy() {
		_name = GetType().FullName; ;
	}

	public override void ModifyStats(Stats stats) {
		stats.ModifyAccuracy(-15);
	}
}