using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using DbNetLink.Data;
using MaxiSwitch.DALC.Logger;
using System.Configuration;
using DALC;


namespace MaxiSwitch.DALC.Configuration
{
    public class ConfigurationData
    {
        #region "ResponseCodes"

        public DataTable SelectResponseCodes()
        {
            DataTable DThostmessagecodes = new DataTable();
            DThostmessagecodes = null;

            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("GetIMPSResponseCodes");
                    DThostmessagecodes = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DThostmessagecodes;

            }
            catch (Exception ex)
            {
                DThostmessagecodes = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DThostmessagecodes;
            }
        }

        public DataTable SelectMFSResponseCodes()
        {
            DataTable DThostmessagecodes = new DataTable();
            DThostmessagecodes = null;



            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("GetIMPSMFSResponseCodes");
                    DThostmessagecodes = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DThostmessagecodes;



            }
            catch (Exception ex)
            {
                DThostmessagecodes = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DThostmessagecodes;
            }
        }

        #endregion "ResponseCodes"

        public DataTable SelectVoiceCodes()
        {
            DataTable DTVoicecodes = new DataTable();
            DTVoicecodes = null;

            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_SelectVoice");
                    DTVoicecodes = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTVoicecodes;

            }
            catch (Exception ex)
            {
                DTVoicecodes = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTVoicecodes;
            }
        }

    }
    public static class CONFIGURATIONCONFIGDATA
    {
        public static string ConnectionString = string.Empty;
        public static string ConnectionStringOracle = string.Empty;
        public static string Provider = string.Empty;
        public static bool ConfigHID { get; set; }
        public static string HSMType { get; set; }
        public static Int64 TransactionTimeOut { get; set; }
        public static int MaxWaitingTime { get; set; }
        public static int RunningItemStatus { get; set; }
        public static string HSMIP = string.Empty;
        public static int HSMPORT = 0;
        public static int MaxConnection = 0;
        public static int MinConnection = 0;
        public static int BatchCount = 0;
        public static string SecurityModuleType { get; set; }
        public static string BankCode { get; set; }
        public static string SMSURL { get; set; }
        public static string SCHEMA { get; set; }
        public static bool SKIPMPIN { get; set; }
        public static bool ISBTreversal { get; set; }
        public static bool BTACTIVE { get; set; }
        public static bool BPCACTIVE { get; set; }
        public static bool NPPFACTIVE { get; set; }
        public static bool TCELLACTIVE { get; set; }
        public static bool WATERACTIVE { get; set; }
        public static bool RRCOACTIVE { get; set; }
        public static bool SKIPVIEWLOANLOG { get; set; }
        public static bool RICBACTIVE { get; set; }
        public static bool BTPOSTPAID { get; set; }
        public static string SERVERKEY { get; set; }
        public static string SERVERKEYIos { get; set; }
        public static string SENDERID { get; set; }
        public static string WEBADDR { get; set; }
        public static string REQUESTMONEYNOTIFICATIONTITLE { get; set; }
        public static string MailAddress { get; set; }
        public static string MailName { get; set; }
        public static string MailPort { get; set; }
        public static string MailHost { get; set; }
        public static string MailUserName { get; set; }
        public static string MailUserPassword { get; set; }
        public static string MailUserTo { get; set; }
        public static string MailUserCc { get; set; }
        public static string MailUserBcc { get; set; }
        public static string HostInterfaceAddress { get; set; }
        public static string HostInterfaceAddress2 { get; set; }
        public static string MessageQueAddress { get; set; }
        public static string BankBIN { get; set; }
        public static string MerchantCategoryCode { get; set; }
        public static string TransactionCurrencyCode { get; set; }
        public static string CardAccepterName { get; set; }

        public static string QSAccessKey { get; set; }
        public static string QSSecretKey { get; set; }
        public static string QSversion { get; set; }
        public static string QSVoice1 { get; set; }
        public static string QSVoice2 { get; set; }
        public static string QSVoice3 { get; set; }
        public static string QSVendorType { get; set; }
        public static string QSPayload { get; set; }

        public static string  QSUsername {get;set;}

        public static string QSPassword { get; set; }

        public static string HostInterfaceAddress3 { get; set; }
        public static string HostInterfaceAddress4 { get; set; }

        private static void LoadConfiguration()
        {
            try
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                ConnectionString = ConnectionStringEncryptDecrypt.DecryptConnectionString(ConnectionString);
                ConnectionStringOracle = ConfigurationManager.AppSettings["ConnectionStringOracle"].ToString();
                ConnectionStringOracle = ConnectionStringEncryptDecrypt.DecryptConnectionString(ConnectionStringOracle);
                Provider = ConfigurationManager.AppSettings["Provider"].ToString();
                ConnectionStringEncryptDecrypt.GetDataEncryptionKey(ConnectionString, Provider);
                ConfigHID = Convert.ToBoolean(ConfigurationManager.AppSettings["ConfigHID"]);
                TransactionTimeOut = Int64.Parse(ConfigurationManager.AppSettings["TransactionTimeOut"].ToString());
                HSMIP = ConfigurationManager.AppSettings["HSMIP"].ToString();
                HSMPORT = int.Parse(ConfigurationManager.AppSettings["HSMPORT"].ToString());
                BankCode = ConfigurationManager.AppSettings["BankCode"].ToString();
                SMSURL = ConfigurationManager.AppSettings["URI"].ToString();
                SCHEMA = ConfigurationManager.AppSettings["Schema"].ToString();
                SKIPMPIN = Convert.ToBoolean(ConfigurationManager.AppSettings["SkipMPIN"].ToString());
                SKIPVIEWLOANLOG = Convert.ToBoolean(ConfigurationManager.AppSettings["SKIPVIEWLOANLOG"].ToString());
                BTACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["BTACTIVE"]);
                BPCACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["BPCACTIVE"]);
                TCELLACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["TCELLACTIVE"]);
                NPPFACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["NPPFACTIVE"]);
                WATERACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["WATERACTIVE"]);
                RRCOACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["RRCOACTIVE"]);
                RICBACTIVE = Convert.ToBoolean(ConfigurationManager.AppSettings["RICBACTIVE"]);
                BTPOSTPAID = Convert.ToBoolean(ConfigurationManager.AppSettings["BTPOSTPAIDACTIVE"]);
                SERVERKEY = Convert.ToString(ConfigurationManager.AppSettings["SERVERKEY"]);
                WEBADDR = Convert.ToString(ConfigurationManager.AppSettings["WEBADDR"]);
                SERVERKEYIos = Convert.ToString(ConfigurationManager.AppSettings["SERVERKEYIos"]);
                SENDERID = Convert.ToString(ConfigurationManager.AppSettings["SENDERID"]);
                REQUESTMONEYNOTIFICATIONTITLE = Convert.ToString(ConfigurationManager.AppSettings["REQUESTMONEYNOTIFICATIONTITLE"]);
                MailAddress = Convert.ToString(ConfigurationManager.AppSettings["MailAddress"]);
                MailName = Convert.ToString(ConfigurationManager.AppSettings["MailName"]);
                MailPort = Convert.ToString(ConfigurationManager.AppSettings["MailPort"]);
                MailHost = Convert.ToString(ConfigurationManager.AppSettings["MailHost"]);
                MailUserName = Convert.ToString(ConfigurationManager.AppSettings["MailUserName"]);
                MailUserPassword = Convert.ToString(ConfigurationManager.AppSettings["MailUserPassword"]);
                MailUserTo = Convert.ToString(ConfigurationManager.AppSettings["MailUserTo"]);
                MailUserCc = Convert.ToString(ConfigurationManager.AppSettings["MailUserCc"]);
                MailUserBcc = Convert.ToString(ConfigurationManager.AppSettings["MailUserBcc"]);
                HostInterfaceAddress = ConfigurationManager.AppSettings["HostInterfaceAddress"].ToString();
                HostInterfaceAddress2 = ConfigurationManager.AppSettings["HostInterfaceAddress2"].ToString();
                MessageQueAddress = ConfigurationManager.AppSettings["MessageQueAddress"].ToString();
                BankBIN = ConfigurationManager.AppSettings["BankBIN"].ToString();
                MerchantCategoryCode = ConfigurationManager.AppSettings["MerchantCategoryCode"].ToString();
                TransactionCurrencyCode = ConfigurationManager.AppSettings["TransactionCurrencyCode"].ToString();
                CardAccepterName = ConfigurationManager.AppSettings["CardAccepterName"].ToString();
                MinConnection = Convert.ToInt32(ConfigurationManager.AppSettings["MinConnection"]);
                MaxConnection = Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnection"]);
                BatchCount = Convert.ToInt32(ConfigurationManager.AppSettings["BatchCount"]);
                ISBTreversal = Convert.ToBoolean(ConfigurationManager.AppSettings["ISBTReversal"]);
                #region QS2
                DataTable GetQSDtls = ConnectionStringEncryptDecrypt.GTQSDtl(ConnectionString, Provider);
                QSAccessKey = GetQSDtls.Rows[0][0].ToString();
                QSSecretKey = GetQSDtls.Rows[0][1].ToString();
                QSUsername = GetQSDtls.Rows[0][2].ToString();
                QSPassword = GetQSDtls.Rows[0][3].ToString();
                #endregion QS2
                HostInterfaceAddress3 = ConfigurationManager.AppSettings["HostInterfaceAddress3"].ToString();
                HostInterfaceAddress4 = ConfigurationManager.AppSettings["HostInterfaceAddress4"].ToString();

            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); }
        }

        public static void init()
        {

            LoadConfiguration();
        }

        public static bool GetKeys(ref string SsmComkey, ref string SsmMasterKey, ref string SsmPvk, ref string HsmZpk,
                                   ref string HsmPvk, ref string HsmComkey, ref string HsmCvv1, ref string HsmCvv2, ref string HsmTpk)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                                                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("GetIMPSKeys");
                    DbParameter ssmComkey = null;
                    DbParameter ssmMasterKey = null;
                    DbParameter ssmPvk = null;
                    ////DbParameter tmkEncryptedKey = null;
                    DbParameter hsmComkey = null;
                    DbParameter hsmPvk = null;
                    DbParameter hsmZpk = null;
                    DbParameter hsmCvv1 = null;
                    DbParameter hsmCvv2 = null;
                    DbParameter Tpk = null;


                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        ssmComkey = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pSsmCommKey" };
                        objCmd.Params.Add("pSsmCommKey", ssmComkey);
                        ssmMasterKey = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pSsmMasterKey" };
                        objCmd.Params.Add("pSsmMasterKey", ssmMasterKey);
                        ssmPvk = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pSsmPvk" };
                        objCmd.Params.Add("pSsmPvk", ssmPvk);
                        ////tmkEncryptedKey = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pTmkEncryptedKey" };
                        ////objCmd.Params.Add("pTmkEncryptedKey", tmkEncryptedKey);                     
                        hsmZpk = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pHsmZpk" };
                        objCmd.Params.Add("pHsmZpk", hsmZpk);
                        hsmPvk = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pHsmPvk" };
                        objCmd.Params.Add("pHsmPvk", hsmPvk);
                        hsmComkey = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pHsmComkey" };
                        objCmd.Params.Add("pHsmComkey", hsmComkey);
                        hsmCvv1 = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pHsmCvv1" };
                        objCmd.Params.Add("pHsmCvv1", hsmCvv1);
                        hsmCvv2 = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pHsmCvv2" };
                        objCmd.Params.Add("pHsmCvv2", hsmCvv2);
                        Tpk = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pTPK" };
                        objCmd.Params.Add("pTPK", Tpk);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        ssmComkey = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@SsmCommKey" };
                        objCmd.Params.Add("@SsmCommKey", ssmComkey);
                        ssmMasterKey = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@SsmMasterKey" };
                        objCmd.Params.Add("@SsmMasterKey", ssmMasterKey);
                        ssmPvk = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@SsmPvk" };
                        objCmd.Params.Add("@SsmPvk", ssmPvk);
                        hsmZpk = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmZpk" };
                        objCmd.Params.Add("@HsmZpk", hsmZpk);
                        hsmPvk = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmPvk" };
                        objCmd.Params.Add("@HsmPvk", hsmPvk);
                        hsmComkey = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmComkey" };
                        objCmd.Params.Add("@HsmComkey", hsmComkey);
                        hsmCvv1 = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmCvv1" };
                        objCmd.Params.Add("@HsmCvv1", hsmCvv1);
                        hsmCvv2 = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmCvv2" };
                        objCmd.Params.Add("@HsmCvv2", hsmCvv2);
                        Tpk = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@HsmTpk" };
                        objCmd.Params.Add("@HsmTpk", Tpk);
                    }

                    Data.ExecuteNonQuery(objCmd);

                    SsmComkey = ssmComkey.Value.ToString();
                    SsmMasterKey = ssmMasterKey.Value.ToString();
                    SsmPvk = ssmPvk.Value.ToString();
                    HsmZpk = hsmZpk.Value.ToString();
                    HsmPvk = hsmPvk.Value.ToString();
                    HsmComkey = hsmComkey.Value.ToString();
                    HsmCvv1 = hsmCvv1.Value.ToString();
                    HsmCvv2 = hsmCvv2.Value.ToString();
                    HsmTpk = Tpk.Value.ToString();

                }
                return true;
            }
            catch (Exception ex)
            {
                SsmComkey = string.Empty;
                SsmMasterKey = string.Empty;
                SsmPvk = string.Empty;
                HsmZpk = string.Empty;
                HsmPvk = string.Empty;
                HsmComkey = string.Empty;
                HsmCvv1 = string.Empty;
                HsmCvv2 = string.Empty;
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool GET_OFFSET(string DeviceID, string ACCOUNTNUMBER, ref string OFFSET)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                                                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETIMPSOFFSET");
                    DbParameter Poffset = null;


                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pDEVICEID", DeviceID);
                        objCmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        Poffset = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pPINOFFSET" };
                        objCmd.Params.Add("pPINOFFSET", Poffset);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@DEVICEID", DeviceID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        Poffset = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@PINOFFSET" };
                        objCmd.Params.Add("@PINOFFSET", Poffset);
                    }

                    Data.ExecuteNonQuery(objCmd);

                    OFFSET = Poffset.Value.ToString();

                }
                return true;
            }
            catch (Exception ex)
            {
                OFFSET = string.Empty;
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool GET_ATMOFFSET(string DeviceID, string Cardnumber, ref string OFFSET ,string CardExp,string AccountNumber)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                                                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETOFFSET");
                    DbParameter Poffset = null;


                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pDEVICEID", DeviceID);
                        objCmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("pCardNumber", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        objCmd.Params.Add("pCardExp", CardExp);
                        Poffset = new OracleParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "pPINOFFSET" };
                        objCmd.Params.Add("pPINOFFSET", Poffset);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@DEVICEID", DeviceID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CardNumber", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        objCmd.Params.Add("@CardExp", CardExp.Substring(3,2)+CardExp.Substring(0,2));
                        Poffset = new SqlParameter() { DbType = DbType.String, Size = 40, Direction = ParameterDirection.Output, ParameterName = "@PINOFFSET" };
                        objCmd.Params.Add("@PINOFFSET", Poffset);
                    }

                    Data.ExecuteNonQuery(objCmd);

                    OFFSET = Poffset.Value.ToString();

                }
                return true;
            }
            catch (Exception ex)
            {
                OFFSET = string.Empty;
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static DataTable GetData(string DeviceID)
        {
            DataTable data = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                                                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETDATA");
                    
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pDEVICEID", DeviceID);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@DEVICEID", DeviceID);
                    }

                    data= Data.GetDataTable(objCmd);
                    
                }
                return data;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return data;
            }
        }

        public static DataTable GetPurpose(string DeviceID)
        {
            DataTable data = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                                                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETPURPOSE");

                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pDEVICEID", DeviceID);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@DEVICEID", DeviceID);
                    }

                    data = Data.GetDataTable(objCmd);

                }
                return data;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return data;
            }
        }

    }

    public enum enumResponseCode
    {
        Unknown = -1,
        Approved = 0,
    }

    public enum enumCalcType
    {
        Recurring = 0,
        Fixed = 1,
    }

    public class ConstResponseCode
    {
        public const string UnableToProcess = "-1";
        public const string IssuerDown = "91";
        public const string Approved = "00";
        public const string BTApproved = "0000";
        public const string SystemError = "96";
        public const string BenificiaryExists = "10";
        public const string NoBenificiaryFound = "11";
        public const string Unabletodeletebenificiary = "12";
        public const string benificiarydeleted = "13";
        public const string InvalidBenificiary = "14";
        public const string DuplicateTransaction = "15";
        public const string AlreadyRegistered = "16";
        public const string AccountNumberNotRegisteredWithBank = "17";
        public const string INVALIDOTP = "18";
        public const string UserNotRegistered = "19";
        public const string InvalidUserorPass = "20";
        public const string UserBlocked = "21";
        public const string UserLoginwithDiffrentDevice = "22";
        public const string InvalidTransaction = "23";
        public const string InvalidAccount = "25";
        public const string InvalidUser = "26";
        public const string UserBlockedSuccess = "27";
        public const string UserUnBlockedSuccess = "28";
        public const string UserPassResetSuccess = "29";
        public const string UserMpinResetSuccess = "30";
        public const string PasswordChanged = "31";
        public const string UnableToChangePassword = "32";
        public const string MerchantAddedSuccess = "33";
        public const string MerchantAlreadyRegistered = "34";
        public const string MerchantUpdatedSuccess = "35";
        public const string MerchantActivated = "36";
        public const string MerchantInActivated = "37";
        public const string BenificiaryAdded = "38";
        public const string SyncAccountSuccess = "39";
        public const string CardNotFound = "41";
        public const string CardBlockeded = "42";
        public const string UnableToBlock = "43";
        public const string CardUnblocked = "44";
        public const string UnableToUnBlock = "45";
        public const string UnableToSendMessage = "52";
        public const string CanNotSetLimitMorethanMaxLimit = "53";
        public const string YourLimitChangeSuccessFully = "54";
        public const string UnableToChangeLimit = "56";
        public const string statuschangesuccess = "57";
        public const string UnableToChangeStatus = "58";
        public const string BranchAlreadyExist = "59";
        public const string BranchNotFound = "60";
        public const string ChequeDepositedSuccess = "61";
        public const string ChequeDepositedUnSuccess = "62";
        public const string ChequeDataNotFound = "63";
        public const string AccountAndBeneficiaryshouldnotsame = "64";
        public const string MobileAndBeneficiaryShouldNotSame = "65";
        public const string PayerAndPayeeShouldNotSame = "66";
        public const string AllMendatoryFieldsRequired = "67";
        public const string ChangeMpinSuccess = "68";
        public const string TransactionAlreadyProcessed = "69";
        public const string FTSuccess = "70";
        public const string NeedToApprove = "71";
        public const string InvalidCurrencyCode = "74";
        public const string NoRoutingGateway = "92";
        public const string SecurityKeyViolation = "63";
        public const string DataNotFound = "20";
        public const string LateResponse = "68";
        public const string EnterProperValue = "69";
        public const string NoBenificiaryFound_OFUS = "72";
        public const string IncorrectMPIN = "55";
        public const string ExceedFrequencyLimit = "65";
        public const string ExceedAmountLimit = "48";//"21";
        public const string PinRetriesExceeded = "75";
        public const string AccountAddedSuccess = "76";
        public const string StatementRequestAccepted = "77";
        public const string BPCBillalreadypaid = "78";
        public const string BPCInvalidConsumerNumber = "79";
        public const string ChequeAlreadyExist = "80";
        public const string BPCSuccess = "81";
        public const string BPCUnSuccess = "82";
        public const string TCELLSuccessPrepaid = "83";
        public const string TCELLSuccessPostpaid = "84";
        public const string TCELLSuccessLeaseLine = "85";
        public const string TCELLUnSuccessPrepaid = "86";
        public const string TCELLUnSuccessPostpaid = "87";
        public const string TCELLUnSuccessLeaseLine = "88";
        public const string AccountNumberNotFound = "89";
        public const string CCSuccess = "92";
        public const string DrukComSuccess = "93";
        public const string NoOutstandingAmount = "94";
        public const string MultipleOf100 = "95";
        public const string ChequeBookRequestAccepted = "97";
        public const string DebitCardRequestAccepted = "98";
        public const string CardLessRequestAccepted = "99";
        public const string InvalidCrossCurrencyCode = "100";
        public const string UnableToConnectBT = "101";
        public const string UnableToConnectTcell = "102";
        public const string UnableToConnectBPC = "103";
        public const string NoCreditCardAccuntFound = "104";
        public const string InvalidServiceNumber = "105";
        public const string BTIsUnderMaintenance = "111";
        public const string BPCIsUnderMaintenance = "112";
        public const string TCELLIsUnderMaintenance = "113";
        public const string EmailIDisnotupdated = "114";
        public const string StatementValidation = "115";
        public const string LoanPaymentSuccess = "116";
        public const string BPCTIMEOUT = "117";
        public const string RDSUCCESS = "118";
        public const string TDSUCCESS = "119";
        public const string NoProductFound = "120";
        public const string CustomerNotIligible = "121";
        public const string UnableToConnectNPPF = "122";
        public const string InvalidNationalID = "123";
        public const string InsufficiantBalance = "AC-OVD02";
        public const string NPPFIsUnderMaintenance = "124";
        public const string NPPFTIMEOUT = "125";
        public const string NPPFPAYMENTSUCCESS = "126";
        public const string NPPFRENTPAYMENTSUCCESS = "127";
        public const string InvalidServiceID = "128";
        public const string UnableToConnectRRCO = "129";
        public const string InvalidVoucher = "130";
        public const string VoucherCanceled = "131";
        public const string VoucherReconciled = "132";
        public const string VoucherReconciledPending = "133";
        public const string VoucherExpired = "134";
        public const string DonationSuccess = "135";
        public const string UnableToConnectWater = "136";
        public const string NoRecipientFound = "137";
        public const string FeedBackAccepted = "138";
        public const string ChequeAlreadyProcessed = "139";
        public const string InvalidChequeDetails = "140";
        public const string RICBIsUnderMaintenance = "141";
        public const string InvalidLifeInsuranceAccount = "142";
        public const string MiscSuccess = "143";
        public const string UnableToConnectRICB = "144";
        public const string BTPostPaidSuccess = "145";
        public const string BTBroadBandPostPaidSuccess = "146";
        public const string BTBroadBandPrepaidSuccess = "147";
        public const string BTLandLineSuccess = "148";
        public const string RRCOSuccess = "149";
        public const string InvalidMode = "150";

        public const string NoFundAvailable = "51";
        public const string TransactionNotAllowed = "39";
        public const string USERALREADYREGISTER = "15";

        public const string TokenUpdated = "151";
        public const string InvalidExpire = "152";
        public const string InsufficiantChequeBalance = "153";
        public const string ChequeExpiry = "154";
        public const string ChequeStopForPayment = "155";

        public const string RequestMoneyServerDown = "156";
        public const string RequestMoneySuccessful = "157";
        public const string TokenNotfound = "158";

        public const string RequestMoneyApproved = "159";
        public const string RequestMoneyDecline = "160";
        public const string VailidityExpire = "161";
        public const string BTLeaseLineSuccess = "162";
        public const string invalidmobile = "163";
       
    
       


        public const string BNgulCashInSucess = "164";
        public const string ForRequestMoneyusernotRegister = "165";
        public const string Usernotregister = "166";
        public const string VotingSuccess = "167";
        public const string ChangeATMpinSuccess = "168";
        public const string AccountBlock = "169";
        public const string UserBlockForReqMoney = "170";
        public const string AccountAlreadyBlock = "171";
        public const string RequestmoneyAccountBlock = "172";
        public const string GreenPinRequestAccepted = "174";
        

        public const string ExceedRDMonth = "175";
        public const string ExceedRDAmount = "176";
        public const string RequestmoneyAccountUnBlock = "178";
        public const string RequestMoneyAlreadyBlock = "179";

        public const string DataNotFoundDB = "180";
        public const string ExceedTDMonth = "181";
        public const string ExceedTDAmount = "182";
        public const string TpinSucessful = "183";
        public const string AccountDelete = "184";
        public const string CardlessLimit = "185";

        public const string WrongCRC = "186";
        public const string SelfQRGenerated = "187";
        public const string NQRCAlreadyExist = "188";

        public const string InwardNQRCMSG = "189";
        public const string OutwardNQRCMSG = "190";
        public const string MobileAlreadyExist = "191";
        


        public const string WrongAccountNo = "192";
        public const string WrongMobileNo = "193";
        public const string WrongCID = "194";
        public const string LoanDisable = "195";

        public const string Nochequeisissued = "196";
        public const string AccountStopQRInvalid = "197";

        public const string IntraFundTransfer = "199";
        public const string InvalidCIDDOB = "200";

        public const string BHIMQRAlreadyExist = "235";
        public const string BHIMQRPending = "236";
        public const string BHIMQRGenerated = "237";
        public const string BHIMQRSendingforApproval = "238";

        public const string VersionUpdate = "239";


        public const string Reserved3 = "13";
        public const string Reserved5 = "14";
        public const string Reserved6 = "15";
        public const string Reserved7 = "16";
        public const string Reserved8 = "17";
        public const string Reserved9 = "18";
        public const string Reserved10 = "19";
    }

    public class ConstNPPFResponseCode
    {
        public const string Approved = "00";
        public const string InvalidServiceID = "01";
        public const string SuppliedParameteresWrong = "04";
        public const string InvalidNationalID = "08";
        public const string BranchIsNotOperational = "12";
        public const string TxnDateIsNotAllined = "13";
        public const string InsuficiantFund = "17";
        public const string Unabletopeocess = "99";
        
    }

    public class ConstRRCOResponseCode
    {
        public const string Approved = "00";
        public const string BadRequest = "01";
        public const string UnAthorizedAccess = "02";
        public const string ChallanCanceled = "03";
        public const string ChallanPaidReconciled = "04";
        public const string ChallanNotFound = "05";
        public const string VoucherCancelled = "06";
        public const string VoucherPaidReconciled = "07";
        public const string VoucherPaidPendingReconciled = "08";
        public const string VoucherNotFound = "09";
        public const string VoucherExpired = "10";

    }

    public class CBSConfigurationData
    {
        //////// Parameters for balance enquiry ///////////////
        public const string BLQ_BRANCH = "100";
        public const string BLQ_SOURCE = "MBANKING";
        public const string BLQ_USERID = "FLEXSWITCH";
        public const string BLQ_MODULEID = "CO";
        public const string BLQ_SERVICE = "FCUBSAccService";
        public const string BLQ_OPERATION = "QueryAccBal";

        //////// Parameters for Mini-Statement ///////////////
        public const string MINI_BRANCH = "100";
        public const string MINI_SOURCE = "MBANKING";
        public const string MINI_USERID = "FLEXSWITCH";
        public const string MINI_MODULEID = "CO";
        public const string MINI_SERVICE = "FCUBSAccFinService";
        public const string MINI_OPERATION = "RequestAccStmt";

     

        //////// Parameters for Intra FundTransfer ///////////////
        public const string INTRA_PRODUCT = "MFTB";
        public const string INTRA_BRN = "100";
        public const string INTRA_MODULEID = "RT";
        public const string INTRA_CCY = "BTN";

        public const string INTRA_SOURCE = "MBANKING";
        public const string INTRA_USERID = "FLEXSWITCH";
        public const string INTRA_BRANCH = "000";
        public const string INTRA_SERVICE = "FCUBSRTService";
        public const string INTRA_OPERATION = "CreateTransaction";

        public const string SCANPAY_PRODUCT = "MBQR";

        //////// Parameters for Outward FundTransfer ///////////////
        public const string OUT_PRODUCT = "MFTR";
        public const string OUT_BRN = "100";
        public const string OUT_MODULEID = "RT";
        public const string OUT_CCY = "BTN";

        public const string OUT_SOURCE = "MBANKING";
        public const string OUT_USERID = "FLEXSWITCH";
        public const string OUT_BRANCH = "000";
        public const string OUT_SERVICE = "FCUBSRTService";
        public const string OUT_OPERATION = "CreateTransaction";

        //////// Parameters for BT Recharge ///////////////
        public const string BT_PRODUCT = "BTOP";
        public const string BT_BRN = "000";
        public const string BT_MODULEID = "UP";
        public const string BT_CCY = "BTN";

        public const string BT_SOURCE = "MBANKING";
        public const string BT_USERID = "MBANKING";
        public const string BT_BRANCH = "999";
        public const string BT_SERVICE = "FCUBSUPService";
        public const string BT_OPERATION = "CreateUPTransaction";

        //////// Parameters for BPC Recharge ///////////////
        public const string BPC_PRODUCT = "EBPC";
        public const string BPC_BRN = "000";
        public const string BPC_MODULEID = "UP";
        public const string BPC_CCY = "BTN";

        public const string BPC_SOURCE = "MBANKING";
        public const string BPC_USERID = "MBANKING";
        public const string BPC_BRANCH = "999";
        public const string BPC_SERVICE = "FCUBSUPService";
        public const string BPC_OPERATION = "CreateUPTransaction";

        //////// Parameters for Credit Card outstanding ///////////////

        public const string CC_BRN = "100";
        public const string CC_MODULEID = "CO";
        public const string CC_CCY = "BTN";

        public const string CC_SOURCE = "MBANKING";
        public const string CC_USERID = "MBANKING";
        public const string CC_BRANCH = "100";
        public const string CC_SERVICE = "FCUBSAccService";
        public const string CC_OPERATION = "QueryAccBal";

        //////// Parameters for Credit Card Payment ///////////////
        public const string CCP_PRODUCT = "CCPY";
        public const string CCP_BRN = "100";
        public const string CCP_MODULEID = "RT";
        public const string CCP_CCY = "BTN";
        public const string CCP_OFFSETCCY = "USD";
        public const string CCP_SOURCE = "MBANKING";
        public const string CCP_USERID = "FLEXSWITCH";
        public const string CCP_BRANCH = "000";
        public const string CCP_SERVICE = "FCUBSRTService";
        public const string CCP_OPERATION = "CreateTransaction";

        //////// Parameters for TashiCell Prepaid Recharge ///////////////
        public const string TTOP_PRODUCT = "TTOP";
        public const string TTOP_BRN = "100";
        public const string TTOP_MODULEID = "UP";
        public const string TTOP_CCY = "BTN";
        public const string TTOP_OFFSETCCY = "USD";
        public const string TTOP_SOURCE = "MBANKING";
        public const string TTOP_USERID = "MBANKING";
        public const string TTOP_BRANCH = "999";
        public const string TTOP_SERVICE = "FCUBSUPService";
        public const string TTOP_OPERATION = "CreateUPTransaction";

        //////// Parameters for TashiCell PostPaid Payment ///////////////
        public const string TCPD_PRODUCT = "TCPD";
        public const string TCPD_BRN = "100";
        public const string TCPD_MODULEID = "UP";
        public const string TCPD_CCY = "BTN";
        public const string TCPD_OFFSETCCY = "USD";
        public const string TCPD_SOURCE = "MBANKING";
        public const string TCPD_USERID = "MBANKING";
        public const string TCPD_BRANCH = "999";
        public const string TCPD_SERVICE = "FCUBSUPService";
        public const string TCPD_OPERATION = "CreateUPTransaction";

        //////// Parameters for TashiCell LeasedLine Payment ///////////////
        public const string TLIN_PRODUCT = "TLIN";
        public const string TLIN_BRN = "100";
        public const string TLIN_MODULEID = "UP";
        public const string TLIN_CCY = "BTN";
        public const string TLIN_OFFSETCCY = "USD";
        public const string TLIN_SOURCE = "MBANKING";
        public const string TLIN_USERID = "MBANKING";
        public const string TLIN_BRANCH = "999";
        public const string TLIN_SERVICE = "FCUBSUPService";
        public const string TLIN_OPERATION = "CreateUPTransaction";

        //////// Parameters for DrukCom Payment ///////////////
        public const string DPAY_PRODUCT = "DPAY";
        public const string DPAY_BRN = "100";
        public const string DPAY_MODULEID = "UP";
        public const string DPAY_CCY = "BTN";
        public const string DPAY_OFFSETCCY = "USD";
        public const string DPAY_SOURCE = "MBANKING";
        public const string DPAY_USERID = "MBANKING";
        public const string DPAY_BRANCH = "999";
        public const string DPAY_SERVICE = "FCUBSUPService";
        public const string DPAY_OPERATION = "CreateUPTransaction";

        //////// Parameters for List of Loan Account ///////////////
 
        public const string LA_BRN = "100";
        public const string LA_MODULEID = "CO";
        public const string LA_CCY = "BTN";
        public const string LA_OFFSETCCY = "USD";
        public const string LA_SOURCE = "MBANKING";
        public const string LA_USERID = "MBANKING";
        public const string LA_BRANCH = "000";
        public const string LA_SERVICE = "FCUBSAccService";
        public const string LA_OPERATION = "QueryAccSumm";

        //////// Parameters for View Laon Account Details ///////////////

        public const string LD_BRN = "100";
        public const string LD_MODULEID = "CL";
        public const string LD_CCY = "BTN";
        public const string LD_OFFSETCCY = "USD";
        public const string LD_SOURCE = "MBANKING";
        public const string LD_USERID = "MBANKING";
        public const string LD_BRANCH = "000";
        public const string LD_SERVICE = "FCUBSCLService";
        public const string LD_OPERATION = "QueryAccount";

        //////// Parameters for Laon Payment ///////////////

        public const string LP_BRN = "100";
        public const string LP_MODULEID = "CL";
        public const string LP_CCY = "BTN";
        public const string LP_OFFSETCCY = "USD";
        public const string LP_SOURCE = "MBANKING";
        public const string LP_USERID = "MBANKING";
        public const string LP_BRANCH = "000";
        public const string LP_SERVICE = "FCUBSCLService";
        public const string LP_OPERATION = "CreatePayment";

        //////// Parameters for Create RD & TD Account ///////////////

        public const string RDTD_BRN = "100";
        public const string RDTD_MODULEID = "IC";
        public const string RDTD_CCY = "BTN";
        public const string RDTD_OFFSETCCY = "USD";
        public const string RDTD_SOURCE = "MBANKING";
        public const string RDTD_USERID = "MBANKING";
        public const string RDTD_BRANCH = "000";
        public const string RDTD_SERVICE = "FCUBSAccService";
        public const string RDTD_OPERATION = "CreateTDCustAcc";

        //////// Parameters for View Recurring Details ///////////////

        public const string RD_BRN = "100";
        public const string RD_MODULEID = "CL";
        public const string RD_CCY = "BTN";
        public const string RD_OFFSETCCY = "USD";
        public const string RD_SOURCE = "MBANKING";
        public const string RD_USERID = "MBANKING";
        public const string RD_BRANCH = "000";
        public const string RD_SERVICE = "FCUBSCLService";
        public const string RD_OPERATION = "QueryAccount";

        //////// Parameters for View Term Details ///////////////

        public const string TD_BRN = "100";
        public const string TD_MODULEID = "CL";
        public const string TD_CCY = "BTN";
        public const string TD_OFFSETCCY = "USD";
        public const string TD_SOURCE = "MBANKING";
        public const string TD_USERID = "MBANKING";
        public const string TD_BRANCH = "000";
        public const string TD_SERVICE = "FCUBSCLService";
        public const string TD_OPERATION = "QueryAccount";

        //////// Parameters for Water Bill Pay ///////////////

        public const string WBP_PRODUCT = "EWAT";
        public const string WBP_BRN = "100";
        public const string WBP_MODULEID = "UP";
        public const string WBP_CCY = "BTN";
        public const string WBP_OFFSETCCY = "USD";
        public const string WBP_SOURCE = "MBANKING";
        public const string WBP_USERID = "MBANKING";
        public const string WBP_BRANCH = "999";
        public const string WBP_SERVICE = "FCUBSUPService";
        public const string WBP_OPERATION = "CreateUPTransaction";

        //////// Parameters for NPPF Loan Payment ///////////////

        public const string NPPL_PRODUCT = "NPPL";
        public const string NPPL_BRN = "100";
        public const string NPPL_MODULEID = "UP";
        public const string NPPL_CCY = "BTN";
        public const string NPPL_OFFSETCCY = "USD";
        public const string NPPL_SOURCE = "MBANKING";
        public const string NPPL_USERID = "MBANKING";
        public const string NPPL_BRANCH = "999";
        public const string NPPL_SERVICE = "FCUBSUPService";
        public const string NPPL_OPERATION = "CreateUPTransaction";

        //////// Parameters for NPPF Rent Pay ///////////////

        public const string NPPR_PRODUCT = "NPPR";
        public const string NPPR_BRN = "100";
        public const string NPPR_MODULEID = "UP";
        public const string NPPR_CCY = "BTN";
        public const string NPPR_OFFSETCCY = "USD";
        public const string NPPR_SOURCE = "MBANKING";
        public const string NPPR_USERID = "MBANKING";
        public const string NPPR_BRANCH = "999";
        public const string NPPR_SERVICE = "FCUBSUPService";
        public const string NPPR_OPERATION = "CreateUPTransaction";

        //////// Parameters for ETHOMETHO Payment ///////////////
        public const string EMCS_PRODUCT = "EMCS";
        public const string EMCS_BRN = "100";
        public const string EMCS_MODULEID = "UP";
        public const string EMCS_CCY = "BTN";
        public const string EMCS_OFFSETCCY = "USD";
        public const string EMCS_SOURCE = "MBANKING";
        public const string EMCS_USERID = "MBANKING";
        public const string EMCS_BRANCH = "999";
        public const string EMCS_SERVICE = "FCUBSUPService";
        public const string EMCS_OPERATION = "CreateUPTransaction";

        //////// Parameters for NORLING Payment ///////////////
        public const string NLCS_PRODUCT = "NLTV";// "EMCS";
        public const string NLCS_BRN = "100";
        public const string NLCS_MODULEID = "UP";
        public const string NLCS_CCY = "BTN";
        public const string NLCS_OFFSETCCY = "USD";
        public const string NLCS_SOURCE = "MBANKING";
        public const string NLCS_USERID = "MBANKING";
        public const string NLCS_BRANCH = "999";
        public const string NLCS_SERVICE = "FCUBSUPService";
        public const string NLCS_OPERATION = "CreateUPTransaction";

        //////// Parameters for Donation Pay ///////////////

        public const string DP_PRODUCT = "NPPR";
        public const string DP_BRN = "100";
        public const string DP_MODULEID = "UP";
        public const string DP_CCY = "BTN";
        public const string DP_OFFSETCCY = "USD";
        public const string DP_SOURCE = "MBANKING";
        public const string DP_USERID = "MBANKING";
        public const string DP_BRANCH = "999";
        public const string DP_SERVICE = "FCUBSUPService";
        public const string DP_OPERATION = "CreateUPTransaction";

        //////// Parameters for Tax Pay ///////////////

        public const string TAX_PRODUCT = "ETAX";
        public const string TAX_BRN = "100";
        public const string TAX_MODULEID = "UP";
        public const string TAX_CCY = "BTN";
        public const string TAX_OFFSETCCY = "USD";
        public const string TAX_SOURCE = "MBANKING";
        public const string TAX_USERID = "MBANKING";
        public const string TAX_BRANCH = "999";
        public const string TAX_SERVICE = "FCUBSUPService";
        public const string TAX_OPERATION = "CreateUPTransaction";

        //////// Parameters for RICB Laon ///////////////
        public const string RICBL_PRODUCT = "EBPC";
        public const string RICBL_BRN = "000";
        public const string RICBL_MODULEID = "UP";
        public const string RICBL_CCY = "BTN";
        public const string RICBL_SOURCE = "MBANKING";
        public const string RICBL_USERID = "MBANKING";
        public const string RICBL_BRANCH = "999";
        public const string RICBL_SERVICE = "FCUBSUPService";
        public const string RICBL_OPERATION = "CreateUPTransaction";

        //////// Parameters for BT Post Paid Payment ///////////////
        public const string BTPP_PRODUCT = "BTMP";
        public const string BTPP_BRN = "100";
        public const string BTPP_MODULEID = "UP";
        public const string BTPP_CCY = "BTN";
        public const string BTPP_OFFSETCCY = "USD";
        public const string BTPP_SOURCE = "MBANKING";
        public const string BTPP_USERID = "MBANKING";
        public const string BTPP_BRANCH = "999";
        public const string BTPP_SERVICE = "FCUBSUPService";
        public const string BTPP_OPERATION = "CreateUPTransaction";

        //////// Parameters for BT Broad Band Post Paid Payment ///////////////
        public const string BTBBPP_PRODUCT = "BTPO";
        public const string BTBBPP_BRN = "100";
        public const string BTBBPP_MODULEID = "UP";
        public const string BTBBPP_CCY = "BTN";
        public const string BTBBPP_OFFSETCCY = "USD";
        public const string BTBBPP_SOURCE = "MBANKING";
        public const string BTBBPP_USERID = "MBANKING";
        public const string BTBBPP_BRANCH = "999";
        public const string BTBBPP_SERVICE = "FCUBSUPService";
        public const string BTBBPP_OPERATION = "CreateUPTransaction";

        //////// Parameters for BT Broad Band Pre Paid Payment ///////////////
        public const string BTBBPR_PRODUCT = "BTPR";
        public const string BTBBPR_BRN = "100";
        public const string BTBBPR_MODULEID = "UP";
        public const string BTBBPR_CCY = "BTN";
        public const string BTBBPR_OFFSETCCY = "USD";
        public const string BTBBPR_SOURCE = "MBANKING";
        public const string BTBBPR_USERID = "MBANKING";
        public const string BTBBPR_BRANCH = "999";
        public const string BTBBPR_SERVICE = "FCUBSUPService";
        public const string BTBBPR_OPERATION = "CreateUPTransaction";

        //////// Parameters for BT Land Line Payment ///////////////
        public const string BTLL_PRODUCT = "BTLL";
        public const string BTLL_BRN = "100";
        public const string BTLL_MODULEID = "UP";
        public const string BTLL_CCY = "BTN";
        public const string BTLL_OFFSETCCY = "USD";
        public const string BTLL_SOURCE = "MBANKING";
        public const string BTLL_USERID = "MBANKING";
        public const string BTLL_BRANCH = "999";
        public const string BTLL_SERVICE = "FCUBSUPService";
        public const string BTLL_OPERATION = "CreateUPTransaction";

        //////// Parameters for Life Insurance Payment ///////////////
        public const string RILI_PRODUCT = "RILI";
        public const string RILI_BRN = "100";
        public const string RILI_MODULEID = "UP";
        public const string RILI_CCY = "BTN";
        public const string RILI_OFFSETCCY = "USD";
        public const string RILI_SOURCE = "MBANKING";
        public const string RILI_USERID = "MBANKING";
        public const string RILI_BRANCH = "999";
        public const string RILI_SERVICE = "FCUBSUPService";
        public const string RILI_OPERATION = "CreateUPTransaction";

        //////// Parameters for Life Annuity Payment ///////////////
        public const string RILA_PRODUCT = "RILA";
        public const string RILA_BRN = "100";
        public const string RILA_MODULEID = "UP";
        public const string RILA_CCY = "BTN";
        public const string RILA_OFFSETCCY = "USD";
        public const string RILA_SOURCE = "MBANKING";
        public const string RILA_USERID = "MBANKING";
        public const string RILA_BRANCH = "999";
        public const string RILA_SERVICE = "FCUBSUPService";
        public const string RILA_OPERATION = "CreateUPTransaction";

        //////// Parameters for Loan Payment ///////////////
        public const string RILN_PRODUCT = "RILN";
        public const string RILN_BRN = "100";
        public const string RILN_MODULEID = "UP";
        public const string RILN_CCY = "BTN";
        public const string RILN_OFFSETCCY = "USD";
        public const string RILN_SOURCE = "MBANKING";
        public const string RILN_USERID = "MBANKING";
        public const string RILN_BRANCH = "999";
        public const string RILN_SERVICE = "FCUBSUPService";
        public const string RILN_OPERATION = "CreateUPTransaction";

        ////////////////Parameters for  Request Money//////////

        public const string REQM_PRODUCT = "RMFT";
        public const string REQM_BRN = "100";
        public const string REQM_MODULEID = "RT";
        public const string REQM_CCY = "BTN";
        public const string REQM_SOURCE = "MBANKING";
        public const string REQM_USERID = "FLEXSWITCH";
        public const string REQM_BRANCH = "000";
        public const string REQM_SERVICE = "FCUBSRTService";
        public const string REQM_OPERATION = "CreateTransaction";


        ////////////////Parameters for LeaseLine//////////

        public const string BTLLS_PRODUCT = "BLIN";
        public const string BTLLS_BRN = "100";
        public const string BTLLS_MODULEID = "UP";
        public const string BTLLS_CCY = "BTN";
        public const string BTLLS_OFFSETCCY = "USD";
        public const string BTLLS_SOURCE = "MBANKING";
        public const string BTLLS_USERID = "MBANKING";
        public const string BTLLS_BRANCH = "999";
        public const string BTLLS_SERVICE = "FCUBSUPService";
        public const string BTLLS_OPERATION = "CreateUPTransaction";



        ////////////////Parameters for B-NGUL payment//////////

        public const string BNGULP_PRODUCT = "BTCI";
        public const string BNGULP_BRN = "100";
        public const string BNGULP_MODULEID = "UP";
        public const string BNGULP_CCY = "BTN";
        public const string BNGULP_OFFSETCCY = "USD";
        public const string BNGULP_SOURCE = "MBANKING";
        public const string BNGULP_USERID = "MBANKING";
        public const string BNGULP_BRANCH = "999";
        public const string BNGULP_SERVICE = "FCUBSUPService";
        public const string BNGULP_OPERATION = "CreateUPTransaction";

        ////////////////Parameters for Voting payment//////////
        public const string VP_PRODUCT = "NPPR";
        public const string VP_BRN = "100";
        public const string VP_MODULEID = "UP";
        public const string VP_CCY = "BTN";
        public const string VP_OFFSETCCY = "USD";
        public const string VP_SOURCE = "MBANKING";
        public const string VP_USERID = "MBANKING";
        public const string VP_BRANCH = "999";
        public const string VP_SERVICE = "FCUBSUPService";
        public const string VP_OPERATION = "CreateUPTransaction";


      /////NQRC
       // public const string SCANPAY_PRODUCT = "MBQR";
        public const string SCANPAY_PRODUCT_NQRI = "NQRI";
        public const string SCANPAY_PRODUCT_NQRO = "NQRO";
        public const string INTRARMA_BRN = "999";
      

    }

    public class NQRCConfiguration
    {
        public const string PAYLOAD_FORMAT_INDICATOR = "000201";
        public const string POINT_OF_INITIATION_METHOD = "010211";//Static QR
        public const string MERCHANT_IDENTIFIER = "0916";
        public const string MERCHANT_CATEGORY_CODE = "5204";
        public const string TRANSACTION_CURRENCY_CODE = "5303064";
        public const string COUNTRY_CODE = "5802BT";
        public const string MERCHANT_NAME = "59";
        public const string MERCHANT_CITY = "60";
        public const string MERCHANTADDITIONALDATA = "62";
        public const string MERCHANT_CRC = "6304";
        public const string NQRCOFFSETACC = "103030011";
        public const string NQRCOFFSETBRN = "999";
        public const string NQRCOFFSETCCY = "BTN";
    }

}


