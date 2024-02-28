using System;
using System.Configuration;

namespace AcceptjsDotnet
{
    public class AppConfig
    {
        public static string AcceptUIURL { get { return Get("AcceptUIURL"); } }

        public static string AuthorizeNetLoginID { get { return Get("AuthorizeNetLoginID"); } }

        public static string AuthorizeNetTransactionKey { get { return Get("AuthorizeNetTransactionKey"); } }

        private static string Get(string key)
        {
            // try to convert
            try
            {
                return (string)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(string));
            }
            catch (Exception) { }

            // return default
            return default(string);
        }
    }
}