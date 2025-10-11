using System;

namespace TTV_Calculator
{
    public enum PriorityGroup
    {
        PreFormation,
        Formation,
        PostFormation,
        Fire,
    }

    public enum ReactionRequirement
    {
        GasRequirement,
        MinimumTemperature,
        MaximumTemperature,
    }

    public abstract class GasReaction
    {
        public abstract string Name { get; }
        public abstract PriorityGroup PriorityGroup { get; }
        public abstract Dictionary<ReactionRequirement, object> Requirements { get; }

        public abstract bool React(GasMixture mixture);
    }

    public sealed class WaterVaporCondensation : GasReaction
    {
        public override string Name => "Water Vapor Condensation";
        public override PriorityGroup PriorityGroup => PriorityGroup.PostFormation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.WaterVapor, GasMixture.MOLES_GAS_VISIBLE},
                }},
            {ReactionRequirement.MaximumTemperature, WATER_VAPOR_CONDENSATION_POINT},
        };

        public const float WATER_VAPOR_CONDENSATION_POINT = GasMixture.T20C + 10f;

        public override bool React(GasMixture mixture)
        {
            mixture.AddMoles(GasType.WaterVapor, -0.25f);
            return true;
        }
    }

    public sealed class PlasmaCombustion : GasReaction
    {
        public override string Name => "Plasma Combustion";
        public override PriorityGroup PriorityGroup => PriorityGroup.Fire;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Plasma, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Oxygen, GasMixture.MINIMUM_MOLE_COUNT},
                }},
            { ReactionRequirement.MinimumTemperature, PLASMA_MINIMUM_BURN_TEMPERATURE},
        };

        private const float PLASMA_MINIMUM_BURN_TEMPERATURE = 373.15f;
        private const float PLASMA_UPPER_TEMPERATURE = PLASMA_MINIMUM_BURN_TEMPERATURE + 1270f;
        private const float OXYGEN_BURN_RATIO_BASE = 1.4f;
        private const float PLASMA_OXYGEN_FULLBURN = 10f;
        private const float SUPER_SATURATION_THRESHOLD = 96f;
        private const float PLASMA_BURN_RATE_DELTA = 9f;
        private const float FIRE_PLASMA_ENERGY_RELEASED = 3e6f;

        public override bool React(GasMixture mixture)
        {
            float temperature = mixture.Temperature;
            float temperatureScale;

            if (temperature > PLASMA_UPPER_TEMPERATURE)
            {
                temperatureScale = 1f;
            }
            else
            {
                temperatureScale = (temperature - PLASMA_MINIMUM_BURN_TEMPERATURE) / (PLASMA_UPPER_TEMPERATURE - PLASMA_MINIMUM_BURN_TEMPERATURE);
            }
            if (temperatureScale <= 0)
            {
                return false;
            }

            float oxygenBurnRatio = OXYGEN_BURN_RATIO_BASE - temperatureScale;
            float plasmaBurnRate;
            bool superSaturation = false; // Whether we should make tritium.

            float plasmaOxygenRatio = mixture.GetMoleAmount(GasType.Oxygen) / mixture.GetMoleAmount(GasType.Plasma);
            if (plasmaOxygenRatio >= SUPER_SATURATION_THRESHOLD)
            {
                plasmaBurnRate = mixture.GetMoleAmount(GasType.Plasma) / PLASMA_BURN_RATE_DELTA * temperatureScale;
                superSaturation = true; // Begin to form tritium
            }
            else if (plasmaOxygenRatio >= PLASMA_OXYGEN_FULLBURN)
            {
                plasmaBurnRate = mixture.GetMoleAmount(GasType.Plasma) / PLASMA_BURN_RATE_DELTA * temperatureScale;
            }
            else
            {
                plasmaBurnRate = mixture.GetMoleAmount(GasType.Plasma) / PLASMA_OXYGEN_FULLBURN / PLASMA_BURN_RATE_DELTA * temperatureScale;
            }

            if (plasmaBurnRate < GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;
            plasmaBurnRate = Math.Min(Math.Min(plasmaBurnRate, mixture.GetMoleAmount(GasType.Plasma)), mixture.GetMoleAmount(GasType.Oxygen) * (1f / oxygenBurnRatio)); //Ensures matter is conserved properly
            mixture.SetMoles(GasType.Plasma, /* QUANTIZE( */mixture.GetMoleAmount(GasType.Plasma)/* ) */ - plasmaBurnRate);
            mixture.SetMoles(GasType.Oxygen, /* QUANTIZE( */mixture.GetMoleAmount(GasType.Oxygen)/* ) */ - (plasmaBurnRate * oxygenBurnRatio));
            if (superSaturation)
            {
                mixture.AddMoles(GasType.Tritium, plasmaBurnRate);
            }
            else
            {
                mixture.AddMoles(GasType.CarbonDioxide, plasmaBurnRate * 0.75f);
                mixture.AddMoles(GasType.WaterVapor, plasmaBurnRate * 0.25f);
            }

            float energyReleased = FIRE_PLASMA_ENERGY_RELEASED * plasmaBurnRate;
            float NewHeatCapacity = mixture.HeatCapacity;
            if (NewHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = (temperature * oldHeatCapacity + energyReleased) / NewHeatCapacity;
            }

            return true;
        }
    }

    public sealed class TritiumCombustion : GasReaction
    {
        public override string Name => "Tritium Combustion";
        public override PriorityGroup PriorityGroup => PriorityGroup.Fire;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Tritium, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Oxygen, GasMixture.MINIMUM_MOLE_COUNT},
                }},
            { ReactionRequirement.MinimumTemperature, TRITIUM_MINIMUM_BURN_TEMPERATURE},
        };

        private const float TRITIUM_MINIMUM_BURN_TEMPERATURE = 373.15f;
        private const float TRITIUM_BURN_OXY_FACTOR = 100f;
        private const float TRITIUM_OXYGEN_FULLBURN = 10f;
        private const float MINIMUM_TRIT_OXYBURN_ENERGY = 2000000f;
        private const float FIRE_TRITIUM_ENERGY_RELEASED = 280000f;

        public override bool React(GasMixture mixture)
        {
            float energyReleased = mixture.Temperature;
            float oldHeatCapacity = mixture.HeatCapacity;
            float temperature = mixture.Temperature;
            float initialTritium = mixture.GetMoleAmount(GasType.Tritium);
            float burnedFuel;

            if (mixture.GetMoleAmount(GasType.Oxygen) < initialTritium || MINIMUM_TRIT_OXYBURN_ENERGY > temperature * oldHeatCapacity)
            {
                burnedFuel = mixture.GetMoleAmount(GasType.Oxygen) / TRITIUM_BURN_OXY_FACTOR;
                if (burnedFuel > initialTritium)
                {
                    burnedFuel = initialTritium;
                }
                mixture.AddMoles(GasType.Tritium, -burnedFuel);
            }
            else
            {
                burnedFuel = initialTritium;
                mixture.SetMoles(GasType.Tritium, mixture.GetMoleAmount(GasType.Tritium) * (1f - 1f / TRITIUM_OXYGEN_FULLBURN));
                mixture.AddMoles(GasType.Oxygen, -mixture.GetMoleAmount(GasType.Tritium));
                energyReleased += FIRE_TRITIUM_ENERGY_RELEASED * burnedFuel * (TRITIUM_OXYGEN_FULLBURN - 1);
            }

            mixture.AddMoles(GasType.WaterVapor, burnedFuel);

            if (burnedFuel > 0)
            {
                energyReleased += FIRE_TRITIUM_ENERGY_RELEASED * burnedFuel;
            }

            if (energyReleased > 0)
            {
                float newHeatCapacity = mixture.HeatCapacity;
                if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
                {
                    mixture.Temperature = (temperature * oldHeatCapacity + energyReleased) / newHeatCapacity;
                }
            }

            return true;
        }
    }

    public sealed class NitrousFormation : GasReaction
    {
        public override string Name => "Nitrous Oxide Formation";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Oxygen, 10f},
                    {GasType.Nitrogen, 20f},
                    {GasType.Bz, 5f},
                }},
            { ReactionRequirement.MinimumTemperature, N2O_FORMATION_MIN_TEMPERATURE},
            { ReactionRequirement.MaximumTemperature, N2O_FORMATION_MAX_TEMPERATURE},
        };

        private const float N2O_FORMATION_MIN_TEMPERATURE = 200f;
        private const float N2O_FORMATION_MAX_TEMPERATURE = 250f;
        private const float N2O_FORMATION_ENERGY = 10000f;

        public override bool React(GasMixture mixture)
        {
            float heatEfficiency = Math.Min(mixture.GetMoleAmount(GasType.Oxygen) * 2, mixture.GetMoleAmount(GasType.Nitrogen));
            if ((mixture.GetMoleAmount(GasType.Oxygen) - heatEfficiency * 0.5f < 0f) || (mixture.GetMoleAmount(GasType.Nitrogen) - heatEfficiency < 0f))
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;
            mixture.AddMoles(GasType.Oxygen, -heatEfficiency * 0.5f);
            mixture.AddMoles(GasType.Nitrogen, -heatEfficiency);
            mixture.AddMoles(GasType.NitrousOxide, heatEfficiency);

            float energyReleased = heatEfficiency * N2O_FORMATION_ENERGY;
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class NitrousDecomposition : GasReaction
    {
        public override string Name => "Nitrous Oxide Decomposition";
        public override PriorityGroup PriorityGroup => PriorityGroup.PostFormation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.NitrousOxide, GasMixture.MINIMUM_MOLE_COUNT * 2f},
                }},
            { ReactionRequirement.MinimumTemperature, N2O_DECOMPOSITION_MIN_TEMPERATURE},
            { ReactionRequirement.MaximumTemperature, N2O_DECOMPOSITION_MAX_TEMPERATURE},
        };

        private const float N2O_DECOMPOSITION_MIN_TEMPERATURE = 1400f;
        private const float N2O_DECOMPOSITION_MAX_TEMPERATURE = 100000f;
        private const float N2O_DECOMPOSITION_RATE_DIVISOR = 2f;
        private const float N2O_DECOMPOSITION_MIN_SCALE_TEMP = 0f;
        private const float N2O_DECOMPOSITION_MAX_SCALE_TEMP = 100000f;
        private const float N2O_DECOMPOSITION_SCALE_DIVISOR = -0.25f * ((N2O_DECOMPOSITION_MAX_SCALE_TEMP - N2O_DECOMPOSITION_MIN_SCALE_TEMP) * 2f);
        private const float N2O_DECOMPOSITION_ENERGY = 200000f;

        public override bool React(GasMixture mixture)
        {
            float temperature = mixture.Temperature;
            float burnedFuel = mixture.GetMoleAmount(GasType.NitrousOxide) / N2O_DECOMPOSITION_RATE_DIVISOR * ((temperature - N2O_DECOMPOSITION_MIN_SCALE_TEMP) * (temperature - N2O_DECOMPOSITION_MAX_SCALE_TEMP) / (N2O_DECOMPOSITION_SCALE_DIVISOR));
            if (burnedFuel <= 0f || mixture.GetMoleAmount(GasType.NitrousOxide) - burnedFuel < 0f)
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;
            mixture.AddMoles(GasType.NitrousOxide, -burnedFuel);
            mixture.AddMoles(GasType.Nitrogen, -burnedFuel);
            mixture.AddMoles(GasType.Oxygen, -burnedFuel / 2f);

            float energyReleased = N2O_DECOMPOSITION_ENERGY * burnedFuel;
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class BzFormation : GasReaction
    {
        public override string Name => "Bz Formation";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.NitrousOxide, 10f},
                    {GasType.Plasma, 10f},
                }},
            { ReactionRequirement.MaximumTemperature, BZ_FORMATION_MAX_TEMPERATURE},
        };

        private const float BZ_FORMATION_MAX_TEMPERATURE = 313.15f;
        private const float BZ_FORMATION_ENERGY = 80000f;
        private const float N2O_DECOMPOSITION_ENERGY = 200000f;

        public override bool React(GasMixture mixture)
        {
            float pressure = mixture.Pressure;
            float volume = mixture.Volume;
            float enviromentEfficiency = volume / pressure;
            float ratioEfficiency = Math.Min(mixture.GetMoleAmount(GasType.NitrousOxide) / mixture.GetMoleAmount(GasType.Plasma), 1f);
            float nitrousOxideDecomposedFactor = Math.Max(4f * (mixture.GetMoleAmount(GasType.Plasma) / (mixture.GetMoleAmount(GasType.NitrousOxide) + mixture.GetMoleAmount(GasType.Plasma)) - 0.75f), 0f);
            float bzFormed = Math.Min(
                0.01f * ratioEfficiency * enviromentEfficiency,
                Math.Min(
                    mixture.GetMoleAmount(GasType.NitrousOxide) * 2.5f,
                    mixture.GetMoleAmount(GasType.Plasma) * (1f / (0.8f * (1f - nitrousOxideDecomposedFactor)))
                )
            );

            if (mixture.GetMoleAmount(GasType.NitrousOxide) - bzFormed * 0.4f < 0f ||
                mixture.GetMoleAmount(GasType.Plasma) - 0.8f * bzFormed * (1f - nitrousOxideDecomposedFactor) < 0f ||
                bzFormed <= 0f)
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;

            if (nitrousOxideDecomposedFactor > 0f)
            {
                float amountDecomposed = 0.4f * bzFormed * nitrousOxideDecomposedFactor;
                mixture.AddMoles(GasType.Nitrogen, amountDecomposed);
                mixture.AddMoles(GasType.Oxygen, 0.5f * amountDecomposed);
            }

            mixture.AddMoles(GasType.Bz, bzFormed * (1 - nitrousOxideDecomposedFactor));
            mixture.AddMoles(GasType.NitrousOxide, -0.4f * bzFormed);
            mixture.AddMoles(GasType.Plasma, -0.8f * bzFormed * (1 - nitrousOxideDecomposedFactor));

            float energyReleased = bzFormed * (BZ_FORMATION_ENERGY + nitrousOxideDecomposedFactor * (N2O_DECOMPOSITION_ENERGY - BZ_FORMATION_ENERGY));
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class PluoxiumFormation : GasReaction
    {
        public override string Name => "Pluoxium Formation";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.CarbonDioxide, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Oxygen, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Tritium, GasMixture.MINIMUM_MOLE_COUNT},
                }},
            { ReactionRequirement.MinimumTemperature, PLUOXIUM_FORMATION_MIN_TEMP},
            { ReactionRequirement.MaximumTemperature, PLUOXIUM_FORMATION_MAX_TEMP},
        };

        private const float PLUOXIUM_FORMATION_MIN_TEMP = 50f;
        private const float PLUOXIUM_FORMATION_MAX_TEMP = GasMixture.T0C;
        private const float PLUOXIUM_FORMATION_MAX_RATE = 5f;
        private const float PLUOXIUM_FORMATION_ENERGY = 250f;

        public override bool React(GasMixture mixture)
        {
            float producedAmount = Math.Min(
                PLUOXIUM_FORMATION_MAX_RATE,
                Math.Min(
                    mixture.GetMoleAmount(GasType.CarbonDioxide),
                    Math.Min(
                        mixture.GetMoleAmount(GasType.Oxygen) * 2f,
                        mixture.GetMoleAmount(GasType.Tritium) * 100f
                    )
                )
            );
            if (producedAmount <= 0f ||
                mixture.GetMoleAmount(GasType.CarbonDioxide) - producedAmount < 0f ||
                mixture.GetMoleAmount(GasType.Oxygen) - producedAmount * 0.5f < 0f ||
                mixture.GetMoleAmount(GasType.Tritium) - producedAmount * 0.01f < 0f)
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;

            mixture.AddMoles(GasType.CarbonDioxide, -producedAmount);
            mixture.AddMoles(GasType.Oxygen, -producedAmount * 0.5f);
            mixture.AddMoles(GasType.Tritium, -producedAmount * 0.01f);
            mixture.AddMoles(GasType.Pluoxium, producedAmount);

            float energyReleased = producedAmount * PLUOXIUM_FORMATION_ENERGY;
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class NitriumFormation : GasReaction
    {
        public override string Name => "Nitrium Formation";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Tritium, 20f},
                    {GasType.Nitrogen, 10f},
                    {GasType.Bz, 5f},
                }},
            { ReactionRequirement.MinimumTemperature, NITRIUM_FORMATION_MIN_TEMP},
        };

        private const float NITRIUM_FORMATION_MIN_TEMP = 1500f;
        private const float NITRIUM_FORMATION_TEMP_DIVISOR = 2985.2f;
        private const float NITRIUM_FORMATION_ENERGY = 100000f;

        public override bool React(GasMixture mixture)
        {
            float temperature = mixture.Temperature;
            float heatEfficiency = Math.Min(
                temperature / NITRIUM_FORMATION_TEMP_DIVISOR,
                Math.Min(
                    mixture.GetMoleAmount(GasType.Tritium),
                    Math.Min(
                        mixture.GetMoleAmount(GasType.Nitrogen),
                        mixture.GetMoleAmount(GasType.Bz) * 20f
                    )
                )
            );

            if (heatEfficiency <= 0 ||
                (mixture.GetMoleAmount(GasType.Tritium) - heatEfficiency < 0f) ||
                (mixture.GetMoleAmount(GasType.Nitrogen) - heatEfficiency < 0f) ||
                (mixture.GetMoleAmount(GasType.Bz) - heatEfficiency * 0.05f < 0f))
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;

            mixture.AddMoles(GasType.Tritium, -heatEfficiency);
            mixture.AddMoles(GasType.Nitrogen, -heatEfficiency);
            mixture.AddMoles(GasType.Bz, -heatEfficiency * 0.05f);
            mixture.AddMoles(GasType.Nitrium, heatEfficiency);

            float energyUsed = heatEfficiency * NITRIUM_FORMATION_ENERGY;
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity - energyUsed) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class NitriumDecomposition : GasReaction
    {
        public override string Name => "Nitrium Decomposition";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Oxygen, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Nitrium, GasMixture.MINIMUM_MOLE_COUNT},
                }},
            { ReactionRequirement.MaximumTemperature, NITRIUM_DECOMPOSITION_MAX_TEMP},
        };

        private const float NITRIUM_DECOMPOSITION_MAX_TEMP = GasMixture.T0C + 70f;
        private const float NITRIUM_DECOMPOSITION_TEMP_DIVISOR = 2985.2f;
        private const float NITRIUM_DECOMPOSITION_ENERGY = 30000f;

        public override bool React(GasMixture mixture)
        {
            float temperature = mixture.Temperature;
            float heatEfficiency = Math.Min(temperature / NITRIUM_DECOMPOSITION_TEMP_DIVISOR, mixture.GetMoleAmount(GasType.Nitrium));

            if (heatEfficiency <= 0f || (mixture.GetMoleAmount(GasType.Nitrium) - heatEfficiency < 0f))
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;

            mixture.AddMoles(GasType.Nitrium, -heatEfficiency);
            mixture.AddMoles(GasType.Nitrogen, heatEfficiency);

            float energyReleased = heatEfficiency * NITRIUM_DECOMPOSITION_ENERGY;
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class NobliumFormation : GasReaction
    {
        public override string Name => "Noblium Formation";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Nitrogen, GasMixture.MINIMUM_MOLE_COUNT},
                    {GasType.Tritium, GasMixture.MINIMUM_MOLE_COUNT},
                }},
            { ReactionRequirement.MinimumTemperature, NOBLIUM_FORMATION_MIN_TEMP},
            { ReactionRequirement.MaximumTemperature, NOBLIUM_FORMATION_MAX_TEMP},
        };

        private const float NOBLIUM_FORMATION_MIN_TEMP = GasMixture.TCMB;
        private const float NOBLIUM_FORMATION_MAX_TEMP = 15f;
        private const float NOBLIUM_FORMATION_ENERGY = 2e7f;

        public override bool React(GasMixture mixture)
        {
            float reductionFactor = (float)Math.Clamp((double)(mixture.GetMoleAmount(GasType.Tritium) / (mixture.GetMoleAmount(GasType.Tritium) + mixture.GetMoleAmount(GasType.Bz))), 0.001, 1.0);
            float nobFormed = Math.Min(
                (mixture.GetMoleAmount(GasType.Nitrogen) + mixture.GetMoleAmount(GasType.Tritium)) * 0.01f,
                Math.Min(
                    mixture.GetMoleAmount(GasType.Tritium) * (1 / (5 * reductionFactor)),
                    mixture.GetMoleAmount(GasType.Nitrogen) * 0.1f
                )
            );

            if (nobFormed <= 0 || (mixture.GetMoleAmount(GasType.Tritium) - 5 * nobFormed * reductionFactor < 0) || (mixture.GetMoleAmount(GasType.Nitrogen) - 10 * nobFormed < 0))
            {
                return false;
            }

            float oldHeatCapacity = mixture.HeatCapacity;
            mixture.AddMoles(GasType.Tritium, -5 * nobFormed * reductionFactor);
            mixture.AddMoles(GasType.Nitrogen, -10 * nobFormed);
            mixture.AddMoles(GasType.HyperNoblium, nobFormed);

            float energyReleased = nobFormed * (NOBLIUM_FORMATION_ENERGY / Math.Max(mixture.GetMoleAmount(GasType.Bz), 1));
            float newHeatCapacity = mixture.HeatCapacity;
            if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
            {
                mixture.Temperature = Math.Max((mixture.Temperature * oldHeatCapacity + energyReleased) / newHeatCapacity, GasMixture.TCMB);
            }

            return true;
        }
    }

    public sealed class Fusion : GasReaction
    {
        public override string Name => "Plasmic Fusion";
        public override PriorityGroup PriorityGroup => PriorityGroup.Formation;
        public override Dictionary<ReactionRequirement, object> Requirements => new()
        {
            {ReactionRequirement.GasRequirement, new Dictionary<GasType, float>()
                {
                    {GasType.Tritium, FUSION_TRITIUM_MOLES_USED},
                    {GasType.Plasma, FUSION_MOLE_THRESHOLD},
                    {GasType.CarbonDioxide, FUSION_MOLE_THRESHOLD},
                }},
            { ReactionRequirement.MinimumTemperature, FUSION_TEMPERATURE_THRESHOLD},
        };

        private const float FUSION_MOLE_THRESHOLD = 250f;
        private const float FUSION_TRITIUM_CONVERSION_COEFFICIENT = 0.002f;
        private const float INSTABILITY_GAS_POWER_FACTOR = 3f;
        private const float FUSION_TRITIUM_MOLES_USED = 1f;
        private const float PLASMA_BINDING_ENERGY = 20000000f;
        private const float TOROID_CALCULATED_THRESHOLD = 5.96f;
        private const float FUSION_TEMPERATURE_THRESHOLD = 10000f;
        private const float FUSION_INSTABILITY_ENDOTHERMALITY = 2f;
        private const float FUSION_SCALE_DIVISOR = 10f;
        private const float FUSION_MINIMAL_SCALE = 50f;
        private const float FUSION_SLOPE_DIVISOR = 1250f;
        private const float FUSION_ENERGY_TRANSLATION_EXPONENT = 1.25f;
        private const float FUSION_BASE_TEMPSCALE = 6f;
        private const float FUSION_MIDDLE_ENERGY_REFERENCE = 1e6f;
        private const float FUSION_BUFFER_DIVISOR = 1f;

        public override bool React(GasMixture mixture)
        {
            float thermalEnergy = mixture.Temperature * mixture.HeatCapacity;
            float reactionEnergy;
            float initialPlasma = mixture.GetMoleAmount(GasType.Plasma);
            float initialCarbon = mixture.GetMoleAmount(GasType.CarbonDioxide);
            float scaleFactor = Math.Max(mixture.Volume / FUSION_SCALE_DIVISOR, FUSION_MINIMAL_SCALE);
            float temperatureScale = (float)Math.Log(10, mixture.Temperature);
            float toroidalSize = TOROID_CALCULATED_THRESHOLD + (temperatureScale <= FUSION_BASE_TEMPSCALE ?
                (temperatureScale - FUSION_BASE_TEMPSCALE) / FUSION_BUFFER_DIVISOR :
                (float)Math.Pow(4f, (temperatureScale - FUSION_BASE_TEMPSCALE) / FUSION_SLOPE_DIVISOR));
            float gasPower = 0f;
            foreach (GasType gasType in mixture.Contents.Keys)
            {
                gasPower += GasLibrary.Gases[gasType].FusionPower;
            }
            float instability = gasPower * INSTABILITY_GAS_POWER_FACTOR % toroidalSize;

            float plasma = (initialPlasma - FUSION_MOLE_THRESHOLD) / scaleFactor;
            float carbon = (initialCarbon - FUSION_MOLE_THRESHOLD) / scaleFactor;

            plasma = plasma - instability * (float)Math.Sin(carbon * 57.2957795f) % toroidalSize; // 57.2957795f is the conversion byond uses
            carbon = carbon - plasma % toroidalSize;

            mixture.SetMoles(GasType.Plasma, plasma * scaleFactor + FUSION_MOLE_THRESHOLD);
            mixture.SetMoles(GasType.CarbonDioxide, carbon * scaleFactor + FUSION_MOLE_THRESHOLD);

            float deltaPlasma = Math.Min(initialPlasma - mixture.GetMoleAmount(GasType.Plasma), toroidalSize * scaleFactor * 1.5f);

            reactionEnergy = instability <= FUSION_INSTABILITY_ENDOTHERMALITY || deltaPlasma > 0f ?
                Math.Max(deltaPlasma * PLASMA_BINDING_ENERGY, 0f) :
                deltaPlasma * PLASMA_BINDING_ENERGY * (float)Math.Pow(instability - FUSION_INSTABILITY_ENDOTHERMALITY, 0.5f);

            if (reactionEnergy > 0f)
            {
                float middleEnergy = ((TOROID_CALCULATED_THRESHOLD / 2f * scaleFactor) + FUSION_MOLE_THRESHOLD) * (200f * FUSION_MIDDLE_ENERGY_REFERENCE);
                thermalEnergy = middleEnergy * (float)Math.Pow(FUSION_ENERGY_TRANSLATION_EXPONENT, Math.Log(10f, thermalEnergy / middleEnergy));

                float bowdlerizedReactionEnergy = Math.Clamp(reactionEnergy,
                    thermalEnergy * ((float)Math.Pow(1f / FUSION_ENERGY_TRANSLATION_EXPONENT, 2f) - 1f),
                    thermalEnergy * ((float)Math.Pow(FUSION_ENERGY_TRANSLATION_EXPONENT, 2f) - 1f)
                );
                thermalEnergy = middleEnergy * (float)Math.Pow(10f, Math.Log(FUSION_ENERGY_TRANSLATION_EXPONENT, (thermalEnergy + bowdlerizedReactionEnergy) / middleEnergy));
            }

            mixture.AddMoles(GasType.Tritium, -FUSION_TRITIUM_MOLES_USED);

            float standardWasteGasOutput = scaleFactor * (FUSION_TRITIUM_CONVERSION_COEFFICIENT * FUSION_TRITIUM_MOLES_USED);
            if (deltaPlasma > 0f)
            {
                mixture.AddMoles(GasType.WaterVapor, standardWasteGasOutput);
            }
            else
            {
                mixture.AddMoles(GasType.Bz, standardWasteGasOutput);
            }

            mixture.AddMoles(GasType.Oxygen, standardWasteGasOutput);

            if (reactionEnergy > 0f || (reactionEnergy == 0f && instability <= FUSION_INSTABILITY_ENDOTHERMALITY))
            {
                float newHeatCapacity = mixture.HeatCapacity;
                if (newHeatCapacity > GasMixture.MINIMUM_HEAT_CAPACITY)
                {
                    mixture.Temperature = Math.Clamp(thermalEnergy / newHeatCapacity, GasMixture.TCMB, 1e31f); // 1e31f is infinity in byond
                }
            }

            return true;
        }
    }
}
