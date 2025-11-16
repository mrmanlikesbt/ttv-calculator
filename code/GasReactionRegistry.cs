using System.Reflection;

namespace TTV_Calculator.Code
{
    public static class GasReactionRegistry
    {
        public static Dictionary<GasType, Dictionary<PriorityGroup, List<GasReaction>>> OrderedReactions { get; }

        static GasReactionRegistry()
        {
            Dictionary<GasType, Dictionary<PriorityGroup, List<GasReaction>>> priorityReactions = new();

            List<GasReaction> allReactions = Assembly.GetAssembly(typeof(GasReaction))!
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(GasReaction)) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as GasReaction)
                .Where(r => r != null)
                .Cast<GasReaction>()
                .ToList();

            // Initialize our dictionary(s)
            foreach (GasType gasType in Enum.GetValues<GasType>())
            {
                priorityReactions[gasType] = new Dictionary<PriorityGroup, List<GasReaction>>()
                {
                    { PriorityGroup.PreFormation, new List<GasReaction>() },
                    { PriorityGroup.Formation, new List<GasReaction>() },
                    { PriorityGroup.PostFormation, new List<GasReaction>() },
                    { PriorityGroup.Fire, new List<GasReaction>() },
                };
            }

            // Order our stuff
            foreach (GasReaction reaction in allReactions)
            {
                GasType? reactionKey = null;
                ReactionRequirements requirements = reaction.Requirements;

                if (requirements.GasRequirements == null)
                {
                    continue;
                }

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

                priorityReactions[reactionKey.Value][reaction.PriorityGroup].Add(reaction);
            }

            // Cull empty gases
            foreach (GasType gasType in Enum.GetValues<GasType>())
            {
                bool passed = false;
                foreach (List<GasReaction> priorityGrouping in priorityReactions[gasType].Values)
                {
                    if (priorityGrouping.Count > 0)
                    {
                        passed = true;
                        break;
                    }
                }

                if (!passed)
                {
                    priorityReactions.Remove(gasType);
                }
            }

            OrderedReactions = priorityReactions;
        }
    }
}