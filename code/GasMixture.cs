using System.Runtime.CompilerServices;

namespace TTV_Calculator
{
    public struct GasMixtureConstructor(float temperature, float volume, float[] contents)
    {
        public readonly float[] Contents => contents;
        public readonly float Temperature = temperature;
        public readonly float Volume = volume;
        public readonly float HeatCapacity
        {
            get
            {
                float totalHeatCapacity = 0f;
                for (int i = 0; i < GasLibrary.GasCount; i++)
                {
                    float moles = Contents[i];
                    if (moles > 0)
                    {
                        totalHeatCapacity += GasLibrary.Gases[i].HeatCapacity * moles;
                    }
                }
                return MathF.Max(totalHeatCapacity, GasMixture.MINIMUM_HEAT_CAPACITY);
            }
        }
        public override readonly string ToString()
        {
            float totalMoles = 0f;
            for (int i = 0; i < Contents.Length; i++)
            {
                totalMoles += contents[i];
            }
            float pressure = totalMoles * GasMixture.R_IDEAL_GAS_EQUATION * Temperature / Volume;

            string[] str = [
                $"- Pressure: {pressure} kPa",
                $"- Temperature: {Temperature}",
                $"- Total Moles: {totalMoles} mols",
            ];
            return string.Join("\n", str);
        }
    }
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

        /// <summary>
        /// An array of the gases inside of us
        /// (int)GasType -> gas moles
        /// </summary>
        private float[] contents = new float[GasLibrary.GasCount];

        /// <summary>
        /// A buffer of reactions to avoid unnecessary list allocations 
        /// </summary>
        private static readonly List<GasReaction> _reactionBuffer = new();

        /// <summary>
        /// The volume of our mixture, used for pressure calculation
        /// </summary>
        public float Volume { get; set; }
        /// <summary>
        /// The temperature of our mixture
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// The pressure of our mixture
        /// </summary>
        public float Pressure
        {
            get
            {
                return TotalMoles * R_IDEAL_GAS_EQUATION * Temperature / Volume;
            }
        }

        /// <summary>
        /// The total amount of moles in us
        /// </summary>
        public float TotalMoles => _cachedTotalMoles;
        /// <summary>
        /// The cached amount of total moles. Modified in SetMoles() and AddMoles()
        /// </summary>
        private float _cachedTotalMoles = 0f;

        /// <summary>
        /// The combined heat capacity of all gases in us
        /// </summary>
        public float HeatCapacity => _cachedHeatCapacity;
        /// <summary>
        /// The cached heat capacity of all gases in us. Modified in SetMoles() and AddMoles()
        /// </summary>
        public float _cachedHeatCapacity = MINIMUM_HEAT_CAPACITY;

        /// <summary>
        /// Gets the fusion power of all gases in us
        /// </summary>
        /// <remarks>Not cached because this is only used in Fusion</remarks>
        public float FusionPower
        {
            get
            {
                float gasPower = 0f;
                for (int i = 0; i < contents.Length; i++)
                {
                    gasPower += contents[i];
                }
                return gasPower;
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

        public GasMixture(float newTemperature = 293.15f, float newVolume = 70f, float[]? newContents = null)
        {
            Temperature = newTemperature;
            Volume = newVolume;
            contents = newContents ?? contents;

            // Set our heat capacity and total mole caches
            UpdateHeatCapacityCache();
            UpdateTotalMolesCache();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixOne"></param>
        /// <param name="mixTwo"></param>
        public void Merge(GasMixtureConstructor mixOne, GasMixtureConstructor mixTwo)
        {
            float mixOneHeatCapacity = mixOne.HeatCapacity;
            float mixTwoHeatCapacity = mixTwo.HeatCapacity;
            float combinedHeatCapacity = mixOneHeatCapacity + mixTwoHeatCapacity;
            float newTemperature = (mixOne.Temperature * mixOneHeatCapacity + mixTwo.Temperature * mixTwoHeatCapacity) / combinedHeatCapacity;

            float newVolume = mixOne.Volume + mixTwo.Volume;

            float[] newContents = new float[GasLibrary.GasCount];
            for (int i = 0; i < GasLibrary.GasCount; i++)
            {
                newContents[i] = mixOne.Contents[i] + mixTwo.Contents[i];
            }

            SetMixtureTo(newTemperature, newVolume, newContents);
        }

        /// <summary>
        /// Sets our contents, temperature, and volume to the passed arguments
        /// </summary>
        /// <remarks>This exists so we don't have to constantly create new GasMixture instances when doing mass calculations</remarks>
        /// <param name="newTemperature">The temperature we set the mixture to. Not applied if null</param>
        /// <param name="newVolume">The volume we set the mixture to. Not applied if null</param>
        /// <param name="newContents">The gas contents we set the mixture to. Not applied if null</param>
        public void SetMixtureTo(float? newTemperature = null, float? newVolume = null, float[]? newContents = null)
        {
            Temperature = newTemperature ?? Temperature;
            Volume = newVolume ?? Volume;
            contents = newContents ?? contents;

            // Heat capacity and total moles caches are invalid, re-validate
            UpdateHeatCapacityCache();
            UpdateTotalMolesCache();
        }

        /// <summary>
        /// Adds an amount of moles to to a GasType in the mixture
        /// </summary>
        /// <param name="type">The GasType being added</param>
        /// <param name="amount">The amount being added</param>
        public void AddMoles(GasType gasType, float amount)
        {
            if (amount == 0f)
            {
                return;
            }
            int idx = (int)gasType;
            contents[idx] += amount;
            _cachedTotalMoles += amount;
            _cachedHeatCapacity += GasLibrary.Gases[idx].HeatCapacity * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetMoleAmount(GasType type) => contents[(int)type];

        /// <summary>
        /// Sets a GasType's mole amount in the mixture
        /// </summary>
        /// <param name="type">The GasType being set</param>
        /// <param name="amount">The amount being set to</param>
        public void SetMoles(GasType gasType, float amount)
        {
            int idx = (int)gasType;
            float old = contents[idx];
            float delta = amount - old;
            if (delta == 0f)
            {
                return;
            }
            contents[idx] = amount;
            _cachedTotalMoles += delta;
            _cachedHeatCapacity += GasLibrary.Gases[idx].HeatCapacity * delta;
        }

        /// <summary>
        /// Used to update our heat capacity cache when it's invalidated
        /// </summary>
        private void UpdateHeatCapacityCache()
        {
            float totalHeatCapacity = 0f;
            for (int i = 0; i < GasLibrary.GasCount; i++)
            {
                float moles = contents[i];
                if (moles > 0)
                {
                    totalHeatCapacity += GasLibrary.Gases[i].HeatCapacity * moles;
                }
            }
            _cachedHeatCapacity = MathF.Max(totalHeatCapacity, MINIMUM_HEAT_CAPACITY);
        }

        /// <summary>
        /// Used to update our total moles cache when it's invalidated
        /// </summary>
        private void UpdateTotalMolesCache()
        {
            float totalMoles = 0f;
            for (int i = 0; i < contents.Length; i++)
            {
                totalMoles += contents[i];
            }
            _cachedTotalMoles = totalMoles;
        }

        private static void AddListToBuffer(List<GasReaction> src)
        {
            if (src == null || src.Count == 0)
            {
                return;
            }
            _reactionBuffer.Capacity = Math.Max(_reactionBuffer.Capacity, _reactionBuffer.Count + src.Count);
            _reactionBuffer.AddRange(src);
        }

        /// <summary>
        /// Performs all of the reactions we can in order of PriorityGroup and then gas rarity.
        /// </summary>
        /// <returns>A bitfield of the reactions that occured</returns>
        public ReactionType React()
        {
            ReactionType reactionsOccurred = ReactionType.None;
            if (GetMoleAmount(GasType.HyperNoblium) >= 5f && Temperature > 20f)
            {
                return reactionsOccurred;
            }

            _reactionBuffer.Clear();

            for (int i = 0; i < GasLibrary.GasCount; i++)
            {
                float amount = contents[i];
                if (amount <= 0f)
                {
                    continue;
                }

                GasType gasType = (GasType)i;

                if (!GasReactionRegistry.OrderedReactions.TryGetValue(gasType, out var reactionSet))
                {
                    continue;
                }

                AddListToBuffer(reactionSet[PriorityGroup.PreFormation]);
                AddListToBuffer(reactionSet[PriorityGroup.Formation]);
                AddListToBuffer(reactionSet[PriorityGroup.PostFormation]);
                AddListToBuffer(reactionSet[PriorityGroup.Fire]);
            }

            float temp = Temperature;
            foreach (GasReaction reaction in _reactionBuffer)
            {
                ReactionRequirements requirements = reaction.Requirements;

                if (requirements.MinTemperature.HasValue && temp < requirements.MinTemperature)
                {
                    continue;
                }
                if (requirements.MaxTemperature.HasValue && temp > requirements.MaxTemperature)
                {
                    continue;
                }

                bool skip = false;
                if (requirements.GasRequirements != null)
                {
                    foreach (var (gasType, requiredMoles) in requirements.GasRequirements)
                    {
                        if (contents[(int)gasType] < requiredMoles)
                        {
                            skip = true;
                            break;
                        }
                    }
                }
                if (skip)
                {
                    continue;
                }

                reaction.React(this);
                reactionsOccurred |= reaction.ReactionType;
            }

            return reactionsOccurred;
        }
    }
}