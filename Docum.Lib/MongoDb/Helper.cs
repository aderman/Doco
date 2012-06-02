namespace Docum.Lib.MongoDb
{
    using System.Configuration;

    public static class Helper
    {
        public static string DataBaseName
        {
            get
            {
                return "Mongo";
            }   
        }

        public static string GetConString(string name, string defaulValue)
        {
            var s = ConfigurationManager.ConnectionStrings[name];
            return s != null ? s.ConnectionString : defaulValue;
        }
    }
}