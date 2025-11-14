using System.Reflection;

namespace TTV_Calculator
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

                foreach (var requirement in reaction.Requirements)
                {
                    if (requirement.Key != ReactionRequirement.GasRequirement)
                    {
                        continue;
                    }

                    Dictionary<GasType, float> requiredGases = (Dictionary<GasType, float>)requirement.Value;
                    foreach (GasType gasType in requiredGases.Keys)
                    {
                        if (reactionKey == null || GasLibrary.Gases[reactionKey.Value].Rarity > GasLibrary.Gases[gasType].Rarity)
                        {
                            reactionKey = gasType;
                        }
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
    
        /* For debugging purposes, print out the entire reaction tree in a readable format */
        /* 
        using System.Text;
        public static string GetOrderedReactionsTree()
        {
            var stringBuilder = new StringBuilder();

            foreach (var gasEntry in OrderedReactions)
            {
                stringBuilder.AppendLine($"{gasEntry.Key}"); // GasType level

                foreach (var priorityEntry in gasEntry.Value)
                {
                    stringBuilder.AppendLine($"  └─ {priorityEntry.Key}"); // PriorityGroup level

                    foreach (var reaction in priorityEntry.Value)
                    {
                        stringBuilder.AppendLine($"      └─ {reaction}"); // GasReaction level
                    }
                }
            }

            return stringBuilder.ToString();
        }
        */
    }
}