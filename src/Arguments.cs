using PowerArgs;

namespace Pi.Web
{
    public class Arguments
    {
        [ArgDefaultValue("console")]
        [ArgShortcut("m")]
        [ArgDescription("Run mode")]
        public RunMode Mode { get; set; }

        [ArgDefaultValue(20)]
        [ArgShortcut("dp")]
        [ArgDescription("Decimal places to calculate")]
        public int DecimalPlaces {get; set;}

        [ArgDefaultValue("/pi.txt")]
        [ArgShortcut("o")]
        [ArgDescription("Output path for file mode")]
        public string OutputPath { get; set; }
    }   
}
