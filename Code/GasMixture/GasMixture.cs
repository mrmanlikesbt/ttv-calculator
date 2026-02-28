using System.Runtime.CompilerServices;

namespace TTV_Calculator.Code
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
		public const float DEFAULT_VOLUME = 70f;

		/// <summary>
		/// An array of the gases inside of us
		/// (int)GasType -> gas moles
		/// </summary>
		private float[] contents = new float[GasLibrary.GasCount];

		/// <summary>
		/// The volume of our mixture, used for pressure calculation
		/// </summary>
		public float Volume { get; set; }
		/// <summary>
		/// The temperature of our mixture
		/// </summary>
		public float Temperature { get; set; }

		/// <summary>
		/// The pressure of this mixture
		/// </summary>
		public float Pressure
		{
			get
			{
				return TotalMoles * R_IDEAL_GAS_EQUATION * Temperature / Volume;
			}
		}

		/// <summary>
		/// The amount of moles in this gas mixture
		/// </summary>
		/// <remarks>Modified in SetMoles() and AddMoles()</remarks>
		public float TotalMoles => _cachedTotalMoles;
		private float _cachedTotalMoles = 0f;

		/// <summary>
		/// The heat capacity of this gas mixture
		/// </summary>
		/// <remarks>Modified in SetMoles() and AddMoles()</remarks>
		public float HeatCapacity => _cachedHeatCapacity;
		private float _cachedHeatCapacity = MINIMUM_HEAT_CAPACITY;

		/// <summary>
		/// The fusion power of this gas mixture
		/// </summary>
		/// <remarks>This isn't worth caching as it's only used in fusion</remarks>
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
			List<string> data = [
				$"- {Pressure} kPa",
				$"- {Temperature} kelvin",
			];
			if (TotalMoles > 0f)
			{
				for (int gasId = 0; gasId < contents.Length; gasId++)
				{
					if (contents[gasId] > 0f)
					{
						data.Add($"- {GasLibrary.Gases[gasId].Name}: {contents[gasId]} mols");
					}
				}
			}

			return string.Join("\n", data);
		}

		public GasMixture(float newTemperature = 293.15f, float newVolume = DEFAULT_VOLUME, float[]? newContents = null)
		{
			Temperature = newTemperature;
			Volume = newVolume;
			contents = newContents ?? contents;

			// Set our heat capacity and total mole caches
			UpdateHeatCapacityCache();
			UpdateTotalMolesCache();
		}

		public void Merge(GasMixtureConstructor mixOne, GasMixtureConstructor mixTwo)
		{
			float mixOneHeatCapacity = mixOne.HeatCapacity;
			float mixTwoHeatCapacity = mixTwo.HeatCapacity;
			float combinedHeatCapacity = mixOneHeatCapacity + mixTwoHeatCapacity;

			// Temperature Calculation
			float newTemperature = 0f;
			if (combinedHeatCapacity > 0)
			{
				newTemperature = (mixOne.Temperature * mixOneHeatCapacity + mixTwo.Temperature * mixTwoHeatCapacity) / combinedHeatCapacity;
			}

			float newVolume = mixOne.Volume + mixTwo.Volume;

			for (int i = 0; i < GasLibrary.GasCount; i++)
			{
				contents[i] = mixOne.Contents[i] + mixTwo.Contents[i];
			}

			// Update state directly without calling SetMixtureTo (which triggers extra checks)
			Temperature = newTemperature;
			Volume = newVolume;

			// Manually trigger cache updates
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetBombSize() => float.Max((Pressure - 4053f) / 607.95f, 0f);

		/// <summary>
		/// Performs all possible gas reactions.
		/// </summary>
		/// <returns>A bitfield of the reactions that occured</returns>
		public ReactionType React()
		{
			ReactionType reactionsOccurred = ReactionType.None;
			// Quick exit
			if (TotalMoles < MINIMUM_MOLE_COUNT)
			{
				return reactionsOccurred;
			}

			if (GetMoleAmount(GasType.HyperNoblium) >= 5f && Temperature > 20f)
			{
				return ReactionType.NobliumSupression;
			}
			   
			// This is intentionally not updated after we call GasReaction.React(), idk it's just how ss13 does it
			float currentTemp = Temperature;

			List<GasReaction>[][] registry = GasReactionRegistry.OrderedReactions;

			// Local reference for faster access
			ReadOnlySpan<float> contentsSpan = contents;

			for (int i = 0; i < GasLibrary.GasCount; i++)
			{
				// Only check gases we actually have
				if (contentsSpan[i] <= 0f)
				{
					continue;
				}

				List<GasReaction>[] reactionGroups = registry[i];
				if (reactionGroups == null)
				{
					continue;
				}

				// Iterate through Priority Groups
				for (int p = 0; p < reactionGroups.Length; p++)
				{
					List<GasReaction> reactions = reactionGroups[p];
					// reactions can be null here, for example:
					// in a standard tritium bomb, the PreFormation and PostFormation reaction groups will be null
					if (reactions == null)
					{
						continue;
					}

					// Manual loop is fastest for List<T>
					for (int r = 0; r < reactions.Count; r++)
					{
						GasReaction reaction = reactions[r];
						if (CheckRequirements(reaction.Requirements, currentTemp))
						{
							if (reaction.React(this))
							{
								reactionsOccurred |= reaction.ReactionType;
							}
						}
					}
				}
			}

			return reactionsOccurred;
		}

		/// <summary>
		/// Helper method that checks if reaction requirements are met
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckRequirements(ReactionRequirements reqs, float temp)
		{
			if (temp < reqs.MinTemperature)
			{
				return false;
			}
			if (temp > reqs.MaxTemperature)
			{
				return false;
			}

			if (reqs.GasRequirements != null)
			{
				// Using a span or manual index here would be even faster, 
				// but this is already much better.
				foreach ((GasType gasRequirement, float requiredMoles) in reqs.GasRequirements)
				{
					if (GetMoleAmount(gasRequirement) < requiredMoles)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}