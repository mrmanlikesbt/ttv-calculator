namespace TTV_Calculator
{
    public static class GasLibrary
    {
        public static readonly Dictionary<GasType, Gas> Gases = new()
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
    }
    
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
    public abstract class Gas
    {
        public abstract string Name { get; }
        public abstract float HeatCapacity { get; }
        public abstract float FusionPower { get; }
        public abstract float Rarity { get; }
        public abstract Color DisplayColor { get; }

        public override string ToString() => Name;
    }

    public sealed class Oxygen : Gas
    {
        public static Oxygen Instance { get; } = new Oxygen();

        public override string Name => "Oxygen";
        public override float FusionPower => 0f;
        public override float HeatCapacity => 20f;
        public override float Rarity => 900f;
        public override Color DisplayColor => Color.SkyBlue;
    }

    public sealed class Nitrogen : Gas
    {
        public static Nitrogen Instance { get; } = new Nitrogen();

        public override string Name => "Nitrogen";
        public override float FusionPower => 0f;
        public override float HeatCapacity => 20f;
        public override float Rarity => 1000f;
        public override Color DisplayColor => Color.OrangeRed;
    }

    public sealed class CarbonDioxide : Gas
    {
        public static CarbonDioxide Instance { get; } = new CarbonDioxide();

        public override string Name => "Carbon Dioxide";
        public override float FusionPower => 0f;
        public override float HeatCapacity => 30f;
        public override float Rarity => 700f;
        public override Color DisplayColor => Color.Gray;
    }

    public sealed class Plasma : Gas
    {
        public static Plasma Instance { get; } = new Plasma();

        public override string Name => "Plasma";
        public override float FusionPower => 0f;
        public override float HeatCapacity => 200f;
        public override float Rarity => 800f;
        public override Color DisplayColor => Color.Purple;
    }

    public sealed class WaterVapor : Gas
    {
        public static WaterVapor Instance { get; } = new WaterVapor();

        public override string Name => "Water Vapor";
        public override float FusionPower => 8f;
        public override float HeatCapacity => 40f;
        public override float Rarity => 500f;
        public override Color DisplayColor => Color.LightGray;
    }

    public sealed class HyperNoblium : Gas
    {
        public static HyperNoblium Instance { get; } = new HyperNoblium();

        public override string Name => "Hyper-Noblium";
        public override float FusionPower => 10f;
        public override float HeatCapacity => 2000f;
        public override float Rarity => 50f;
        public override Color DisplayColor => Color.Teal;
    }

    public sealed class NitrousOxide : Gas
    {
        public static NitrousOxide Instance { get; } = new NitrousOxide();

        public override string Name => "Nitrous Oxide";
        public override float FusionPower => 10f;
        public override float HeatCapacity => 40f;
        public override float Rarity => 600f;
        public override Color DisplayColor => Color.White;
    }

    public sealed class Nitrium : Gas
    {
        public static Nitrium Instance { get; } = new Nitrium();

        public override string Name => "Nitrium";
        public override float FusionPower => 7f;
        public override float HeatCapacity => 10f;
        public override float Rarity => 1f;
        public override Color DisplayColor => Color.Orange;
    }

    public sealed class Tritium : Gas
    {
        public static Tritium Instance { get; } = new Tritium();

        public override string Name => "Tritium";
        public override float FusionPower => 5f;
        public override float HeatCapacity => 10f;
        public override float Rarity => 300f;
        public override Color DisplayColor => Color.LightGreen;
    }

    public sealed class Bz : Gas
    {
        public static Bz Instance { get; } = new Bz();

        public override string Name => "Bz";
        public override float FusionPower => 8f;
        public override float HeatCapacity => 20f;
        public override float Rarity => 400f;
        public override Color DisplayColor => Color.Pink;
    }

    public sealed class Pluoxium : Gas
    {
        public static Pluoxium Instance { get; } = new Pluoxium();

        public override string Name => "Pluoxium";
        public override float FusionPower => -10f;
        public override float HeatCapacity => 80f;
        public override float Rarity => 200f;
        public override Color DisplayColor => Color.AliceBlue;
    }
}