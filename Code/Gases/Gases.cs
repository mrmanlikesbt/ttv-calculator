namespace TTV_Calculator.Code
{
	public enum GasType
	{
		Oxygen,
		Nitrogen,
		CarbonDioxide,
		Plasma,
		WaterVapor,
		HyperNoblium,
		NitrousOxide,
		Nitrium,
		Tritium,
		Bz,
		Pluoxium,
	}

	public readonly struct Gas(string name, float heatCapacity, float fusionPower, float rarity, Color displayColor)
	{
		public readonly string Name = name;
		public readonly float HeatCapacity = heatCapacity;
		public readonly float FusionPower = fusionPower;
		public readonly float Rarity = rarity;
		public readonly Color DisplayColor = displayColor;

		public override string ToString() => Name;
	}

	public static class GasLibrary
	{
		public static readonly int GasCount = Enum.GetValues(typeof(GasType)).Length;

		// Indexed by GasType: Gases[(int)GasType.Oxygen] => Oxygen gas
		public static readonly Gas[] Gases = BuildArray();

		private static Gas[] BuildArray()
		{
			Gas[] array = new Gas[GasCount];

			array[(int)GasType.Oxygen] = new Gas("Oxygen", 20f, 0f, 900f, Color.SkyBlue);
			array[(int)GasType.Nitrogen] = new Gas("Nitrogen", 20f, 0f, 1000f, Color.OrangeRed);
			array[(int)GasType.CarbonDioxide] = new Gas("Carbon Dioxide", 30f, 0f, 700f, Color.Gray);
			array[(int)GasType.Plasma] = new Gas("Plasma", 200f, 0f, 800f, Color.Purple);
			array[(int)GasType.WaterVapor] = new Gas("Water Vapor", 40f, 8f, 500f, Color.LightGray);
			array[(int)GasType.HyperNoblium] = new Gas("Hyper-Noblium", 2000f, 10f, 50f, Color.Teal);
			array[(int)GasType.NitrousOxide] = new Gas("Nitrous Oxide", 40f, 10f, 600f, Color.White);
			array[(int)GasType.Nitrium] = new Gas("Nitrium", 10f, 7f, 1f, Color.Orange);
			array[(int)GasType.Tritium] = new Gas("Tritium", 10f, 5f, 300f, Color.LightGreen);
			array[(int)GasType.Bz] = new Gas("Bz", 20f, 8f, 400f, Color.Pink);
			array[(int)GasType.Pluoxium] = new Gas("Pluoxium", 80f, -10f, 200f, Color.AliceBlue);

			return array;
		}
	}
}