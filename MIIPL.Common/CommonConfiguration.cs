using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MIIPL.Common
{
    public static class CommonConfiguration
    {
        #region MemberVariables

        public static string HsmVersionNumber { get; set; }
        public static string HsmManagerName { get; set; }
        public static string HsmManagerIP { get; set; }
        public static int HsmManagerPort = 0;
        public static int HsmWhiteSpace = 0;
        public static int HSMHeaderLength = 0;
        public static string HSMIP { get; set; }
        public static int HSMPort = 0;
        public static int MinConnection = 0;
        public static int MaxConnection = 0;
        public static string SecurityModels { get; set; }
        public static HSMType HSMType { get; set; }
        public static bool ConfigHID { get; set; }
        public static int HsmHealthStatusCheck = 0;
        public static bool SecondaryHSM { get; set; }

        #endregion MemberVariables

        public static void LoadConfiguration()
        {
            try
            {
                HsmVersionNumber = ConfigurationManager.AppSettings["HsmVersionNumber"].ToString();
                HsmManagerName = ConfigurationManager.AppSettings["HsmManagerName"].ToString();
                HsmManagerIP = ConfigurationManager.AppSettings["HsmManagerIP"].ToString();
                HsmManagerPort = Convert.ToInt32(ConfigurationManager.AppSettings["HsmManagerPort"]);
                HsmWhiteSpace = Convert.ToInt32(ConfigurationManager.AppSettings["HsmWhiteSpace"]);
                HSMHeaderLength = Convert.ToInt32(ConfigurationManager.AppSettings["HSMHeaderLength"]);
                HSMIP = ConfigurationManager.AppSettings["HSMIP"].ToString();
                HSMPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMPort"]);
                MinConnection = Convert.ToInt32(ConfigurationManager.AppSettings["MinConnection"]);
                MaxConnection = Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnection"]);
                SecurityModels = ConfigurationManager.AppSettings["SecurityModels"].ToString();
                HSMType = (HSMType)Enum.Parse(typeof(HSMType),ConfigurationManager.AppSettings["HSMType"].ToString());
                ConfigHID = Convert.ToBoolean(ConfigurationManager.AppSettings["ConfigHID"]);
                HsmHealthStatusCheck = Convert.ToInt32(ConfigurationManager.AppSettings["HsmHealthStatusCheck"]);
                SecondaryHSM = Convert.ToBoolean(ConfigurationManager.AppSettings["SecondaryHSM"]);
            }
            catch (Exception ex)
            { }
        }
    }

}
