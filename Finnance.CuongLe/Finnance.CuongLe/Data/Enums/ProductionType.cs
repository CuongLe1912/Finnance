using System;

namespace Finnance.CuongLe.Data.Enums
{
    public enum ProductionType
    {
        Local = 0,
        Dev,
        Stag,
        Production
    }

    public static class EnvironmentType
    {
        public static String Dev = "Development";
        public static String Stag = "Staging";
        public static String Production = "Production";
    }
}
