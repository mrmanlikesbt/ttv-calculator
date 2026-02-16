namespace TTV_Calculator.Code
{
    public static class GasLibrary
    {
        /// <summary>
        /// The amount of GasTypes that exist
        /// </summary>
        /// <remarks>This MUST be set before BuildArray() is called</remarks>
        public static readonly int GasCount = Enum.GetValues<GasType>().Length;

        public static readonly Gas[] Gases = BuildArray();

        private static Gas[] BuildArray()
        {
            Dictionary<GasType, Gas> gasInstances = new()
            {
                { GasType.Oxygen, Oxygen.Instance },
                { GasType.Nitrogen, Nitrogen.Instance },
                { GasType.CarbonDioxide, CarbonDioxide.Instance },
                { GasType.Plasma, Plasma.Instance },
                { GasType.WaterVapor, WaterVapor.Instance },
                { GasType.HyperNoblium, HyperNoblium.Instance },
                { GasType.NitrousOxide, NitrousOxide.Instance },
                { GasType.Nitrium, Nitrium.Instance },
                { GasType.Tritium, Tritium.Instance },
                { GasType.Bz, Bz.Instance },
                { GasType.Pluoxium, Pluoxium.Instance },
            };

            Gas[] array = new Gas[GasCount];
            foreach (var kvp in gasInstances)
            {
                array[(int)kvp.Key] = kvp.Value;
            }
            return array;
        }
    }
}
