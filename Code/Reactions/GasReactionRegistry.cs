using System.Reflection;

namespace TTV_Calculator.Code
{
	public static class GasReactionRegistry
	{
		/// <summary>
		/// A jagged array of reactions.
		/// </summary>
		/// <remarks>Access via: OrderedReactions[(int)GasType][(int)PriorityGroup]</remarks>
		public static List<GasReaction>[][] OrderedReactions { get; }

		static GasReactionRegistry()
		{
			int gasCount = GasLibrary.GasCount;
			int priorityCount = Enum.GetValues<PriorityGroup>().Length;
			var reactionArray = new List<GasReaction>[gasCount][];

			List<GasReaction> allReactions = Assembly.GetAssembly(typeof(GasReaction))!
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(GasReaction)) && !t.IsAbstract)
				.Select(t => Activator.CreateInstance(t) as GasReaction)
				.Where(r => r != null)
				.Cast<GasReaction>()
				.ToList();

			foreach (GasReaction reaction in allReactions)
			{
				GasType? reactionKey = null;
				ReactionRequirements requirements = reaction.Requirements;

				if (requirements.GasRequirements == null)
				{
					continue;
				}

				// Determine which gas "owns" this reaction (based on rarity)
				foreach (var (gasType, requiredMoles) in requirements.GasRequirements)
				{
					if (reactionKey == null || GasLibrary.Gases[(int)reactionKey.Value].Rarity > GasLibrary.Gases[(int)gasType].Rarity)
					{
						reactionKey = gasType;
					}
				}

				if (reactionKey == null)
				{
					continue;
				}

				int gasIndex = (int)reactionKey.Value;
				int priorityIndex = (int)reaction.PriorityGroup;

				reactionArray[gasIndex] ??= new List<GasReaction>[priorityCount];
				reactionArray[gasIndex][priorityIndex] ??= new List<GasReaction>();
				reactionArray[gasIndex][priorityIndex].Add(reaction);
			}

			OrderedReactions = reactionArray;
		}
	}
}