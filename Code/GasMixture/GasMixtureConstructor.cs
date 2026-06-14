namespace TTV_Calculator.Code
{
	public readonly struct GasMixtureConstructor
	{
		public readonly float[] Contents;
		public readonly float Temperature;
		public readonly float Volume;
		public readonly float HeatCapacity;

		public GasMixtureConstructor(float temperature, float volume, float[] contents)
		{
			Temperature = temperature;
			Volume = volume;
			Contents = contents;

			float totalHeatCapacity = 0f;
			for (int i = 0; i < GasLibrary.GasCount; i++)
			{
				if (contents[i] > 0)
				{
					totalHeatCapacity += GasLibrary.Gases[i].HeatCapacity * contents[i];
				}
			}
			HeatCapacity = float.Max(totalHeatCapacity, GasMixture.MINIMUM_HEAT_CAPACITY);
		}

		public override readonly string ToString()
		{
			float totalMoles = Contents.Sum();
			float pressure = totalMoles * GasMixture.R_IDEAL_GAS_EQUATION * Temperature / Volume;

			List<string> data = [
				$"- {pressure} kPa",
				$"- {Temperature} kelvin",
			];
			if (totalMoles > 0f)
			{
				for (int gasId = 0; gasId < Contents.Length; gasId++)
				{
					if (Contents[gasId] > 0f)
					{
						data.Add($"- {GasLibrary.Gases[gasId].Name}: {Contents[gasId]} mols");
					}
				}
			}

			return string.Join(Environment.NewLine, data);
		}
	}
}
