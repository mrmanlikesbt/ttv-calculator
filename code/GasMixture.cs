namespace TTV_Calculator
{
    public class GasMixture
    {
        public const float R_IDEAL_GAS_EQUATION = 8.31f;
        public const float ONE_ATMOSPHERE = 101.325f;
        public const float TCMB = 2.7f;
        public const float T0C = 273.15f;
        public const float T20C = 293.15f;
        public const float MINIMUM_HEAT_CAPACITY = 0.0003f;
        public const float MINIMUM_MOLE_COUNT = 0.01f;
        public const float MOLAR_ACCURACY = 1e-4f;
        public const float MOLES_GAS_VISIBLE = 0.25f;

        public Dictionary<GasType, float> Contents { get; private set; } = new();
        public float Volume { get; set; }
        public float Temperature { get; set; }
        public float Pressure
        {
            get
            {
                return TotalMoles * R_IDEAL_GAS_EQUATION * Temperature / Volume;
            }
        }
        public float TotalMoles
        {
            get
            {
                return Contents.Values.Sum();
            }
        }
        public float HeatCapacity
        {
            get
            {
                float totalHeatCapacity = 0f;
                foreach (var (gasType, moles) in Contents)
                {
                    if (moles > 0)
                    {
                        totalHeatCapacity += GasLibrary.Gases[gasType].HeatCapacity * moles;
                    }
                }
                return Math.Max(totalHeatCapacity, MINIMUM_HEAT_CAPACITY);
            }
        }

        public GasMixture(float temperature = 293.15f, float volume = 70f, Dictionary<GasType, float>? contents = null)
        {
            Temperature = temperature;
            Volume = volume;

            // Initialize with 0 moles for all gases
            if (contents != null)
            {
                Contents = contents;
            }
            else
            {
                foreach (GasType gas in Enum.GetValues(typeof(GasType)))
                {
                    Contents[gas] = 0f;
                }
            }
        }

        public override string ToString()
        {
            string[] str = [
                $"- Pressure: {Pressure} kPa",
                $"- Temperature: {Temperature}",
                $"- Total Moles: {TotalMoles} mols",
            ];
            return string.Join("\n", str);
        }

        public static GasMixture Merge(GasMixture mixOne, GasMixture mixTwo)
        {
            float combinedVolume = mixOne.Volume + mixTwo.Volume;

            float mixOneHeatCapacity = mixOne.HeatCapacity;
            float mixTwoHeatCapacity = mixTwo.HeatCapacity;
            float combinedHeatCapacity = mixOneHeatCapacity + mixTwoHeatCapacity;

            float newTemperature = (mixOne.Temperature * mixOneHeatCapacity + mixTwo.Temperature * mixTwoHeatCapacity) / combinedHeatCapacity;

            return new GasMixture(newTemperature, combinedVolume, mixOne.Contents.Concat(mixTwo.Contents)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kv => kv.Value)));
        }

        public void AddMoles(GasType type, float amount)
        {
            Contents[type] += amount;
        }

        public float GetMoleAmount(GasType type)
        {
            return Contents[type];
        }

        public void SetMoles(GasType type, float amount)
        {
            Contents[type] = amount;
        }

        public List<string>? React()
        {
            if (GetMoleAmount(GasType.HyperNoblium) >= 5f && Temperature > 20f)
            {
                return null;
            }
        
            List<GasReaction> preFormationReactions = new();
            List<GasReaction> formationReactions = new();
            List<GasReaction> postFormationReactions = new();
            List<GasReaction> fireReactions = new();

            foreach (GasType gasType in Contents.Keys)
            {   
                if (!GasReactionRegistry.OrderedReactions.TryGetValue(gasType, out Dictionary<PriorityGroup, List<GasReaction>>? reactionSet))
                {
                    continue;
                }

                preFormationReactions.AddRange(reactionSet[PriorityGroup.PreFormation]);
                formationReactions.AddRange(reactionSet[PriorityGroup.Formation]);
                postFormationReactions.AddRange(reactionSet[PriorityGroup.PostFormation]);
                fireReactions.AddRange(reactionSet[PriorityGroup.Fire]);
            }

            List<GasReaction> reactions = preFormationReactions
                .Concat(formationReactions)
                .Concat(postFormationReactions)
                .Concat(fireReactions)
                .ToList();

            float temp = Temperature;
            List<string> reactionsOccurred = new();
            foreach (GasReaction reaction in reactions)
            {
                Dictionary<ReactionRequirement, object> requirements = reaction.Requirements;

                if (requirements.TryGetValue(ReactionRequirement.MinimumTemperature, out var minTemp) && temp < (float)minTemp)
                {
                    continue;
                }
                if (requirements.TryGetValue(ReactionRequirement.MaximumTemperature, out var maxTemp) && temp > (float)maxTemp)
                {
                    continue;
                }

                if (requirements.TryGetValue(ReactionRequirement.GasRequirement, out var gasReqs))
                {
                    Dictionary<GasType, float> requiredGases = (Dictionary<GasType, float>)gasReqs;
                    if (requiredGases.Any(kvp => GetMoleAmount(kvp.Key) < kvp.Value))
                    {
                        continue;
                    }
                }

                reaction.React(this);
                reactionsOccurred.Add(reaction.Name);
            }

            return reactionsOccurred;
        }
    }
}