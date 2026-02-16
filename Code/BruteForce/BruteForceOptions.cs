namespace TTV_Calculator.Code
{
    public struct BruteForceOptions
    {
        // Cold tank

        public float ColdPressureMin;
        public float ColdPressureMax;
        public float ColdPressureStep;

        public float ColdTemperatureMin;
        public float ColdTemperatureMax;
        public float ColdTemperatureStep;

        public float ColdGasPercentStep;
        public GasType[] ColdGasTypes;

        // Hot tank

        public float HotPressureMin;
        public float HotPressureMax;
        public float HotPressureStep;

        public float HotTemperatureMin;
        public float HotTemperatureMax;
        public float HotTemperatureStep;

        public float HotGasPercentStep;
        public GasType[] HotGasTypes;
    }
}
