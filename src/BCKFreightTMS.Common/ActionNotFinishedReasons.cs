namespace BCKFreightTMS.Common
{
    public sealed class ActionNotFinishedReasons
    {
        public static readonly string NotArr = "the driver has not yet arrived";
        public static readonly string AtAdr1 = "will be at the address within 1 hour";
        public static readonly string AtAdr2 = "will be at the address within 2 hour";
        public static readonly string AtAdr3 = "will be at the address within 3 hour";
        public static readonly string AtAdr4 = "will be at the address within 4 hour";
        public static readonly string AtAdrToday = "will be at the address by the end of the day";
        public static readonly string OffTime = "the enterprise is off time";
        public static readonly string TrPolice = "problems with traffic police";
        public static readonly string Police = "problems with the POLICE";
        public static readonly string AccNoPart = "problems with road accident without participation";
        public static readonly string AccPart = "problems with road accident involved";
    }
}
