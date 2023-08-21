public class OnlyMeleeWeapon : Mutation {
	public OnlyMeleeWeapon() => MutationName = GetType().FullName;

	public override void ModifyStats(Stats stats) {
		stats.ModifyMaxHealth(-20); // Только ради теста.
	}
}