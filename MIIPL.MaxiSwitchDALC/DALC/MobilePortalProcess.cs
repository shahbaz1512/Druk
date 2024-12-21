using DbNetLink.Data;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.DALC.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DALC
{
    public class MobilePortalProcess
    {
        public static void MPORTALACTIVITY(string ACCOUNTNUMBER, string MOBILENUMBER, string ACTIVITYTYPE, string REQUESTEDBY)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTGETMPORTALACTIVITY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@ACTIVITYTYPE", ACTIVITYTYPE);
                        cmd.Params.Add("@REQUESTEDBY", REQUESTEDBY);
                        cmd.Params.Add("@REQTYPR", "INSERT");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@ACTIVITYTYPE", ACTIVITYTYPE);
                        cmd.Params.Add("@REQUESTEDBY", REQUESTEDBY);
                        cmd.Params.Add("@REQTYPR", "INSERT");
                    }
                    Data.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }
        public static DataTable GETMPORTALACTIVITY(string ACCOUNTNUMBER, string MOBILENUMBER)
        {
            DataTable Dtdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTGETMPORTALACTIVITY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@ACTIVITYTYPE", null);
                        cmd.Params.Add("@REQUESTEDBY", null);
                        cmd.Params.Add("@REQTYPR", "SELECT");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@ACTIVITYTYPE", null);
                        cmd.Params.Add("@REQUESTEDBY", null);
                        cmd.Params.Add("@REQTYPR", "SELECT");
                    }
                    Dtdata = Data.GetDataTable(cmd);
                }
                return Dtdata;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                Dtdata = null;
                return Dtdata;
            }
        }

        public static void ShowMasterInsertUpdateDelete(string FLAG, string ShowID, string ShowName, string DateFrom, string DateTo, string Amount, string TransferLimitAmount, out Int32 status, string ProductCode, string Images)
        {
            int statusvalue = -1;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_CONTEST_SHOWMASTER_INSERTUPDATE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@FLAG", FLAG);
                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ShowName", ShowName);
                        cmd.Params.Add("@DateFrom", DateFrom);
                        cmd.Params.Add("@DateTo", DateTo);
                        cmd.Params.Add("@Amount", Amount);
                        cmd.Params.Add("@TransferLimit", TransferLimitAmount);
                        cmd.Params.Add("@ProductCode", ProductCode);
                        cmd.Params.Add("@Image", Images);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@FLAG", FLAG);
                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ShowName", ShowName);
                        cmd.Params.Add("@DateFrom", DateFrom);
                        cmd.Params.Add("@DateTo", DateTo);
                        cmd.Params.Add("@Amount", Amount);
                        cmd.Params.Add("@TransferLimit", TransferLimitAmount);
                        cmd.Params.Add("@ProductCode", ProductCode);
                        cmd.Params.Add("@Image", Images);
                    }
                    Data.ExecuteNonQuery(cmd);
                    statusvalue = 0;
                    status = statusvalue;
                }

            }
            catch (Exception ex)
            {
                status = statusvalue;
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void ShowContestantMasterInsertUpdateDelete(string FLAG, string ContestID, string ShowID, string ContestantName, string Age, string City, string State, string Mobile, string EmailId, string ContestantImage, string Createdby, out Int32 status, string ContestantNumber, string IsRemoved, string OtherDetails)
        {
            int statusvalue = -1;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_CONTEST_CONTESTMASTER_INSERTUPDATE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@FLAG", FLAG);
                        cmd.Params.Add("@ContestID", ContestID);
                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ContestantName", ContestantName);
                        cmd.Params.Add("@Age", Age);
                        cmd.Params.Add("@City", City);
                        cmd.Params.Add("@State", State);
                        cmd.Params.Add("@Mobile", Mobile);
                        cmd.Params.Add("@EmailId", EmailId);
                        cmd.Params.Add("@ContestantImage", ContestantImage);
                        cmd.Params.Add("@contestantNumber", ContestantNumber);
                        cmd.Params.Add("@Createdby", Createdby);
                        cmd.Params.Add("@IsRemoved", IsRemoved);
                        cmd.Params.Add("@OtherDetails", OtherDetails);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@FLAG", FLAG);
                        cmd.Params.Add("@ContestID", ContestID);
                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ContestantName", ContestantName);
                        cmd.Params.Add("@Age", Age);
                        cmd.Params.Add("@City", City);
                        cmd.Params.Add("@State", State);
                        cmd.Params.Add("@Mobile", Mobile);
                        cmd.Params.Add("@EmailId", EmailId);
                        cmd.Params.Add("@ContestantImage", ContestantImage);
                        cmd.Params.Add("@contestantNumber", ContestantNumber);
                        cmd.Params.Add("@Createdby", Createdby);
                        cmd.Params.Add("@IsRemoved", IsRemoved);
                        cmd.Params.Add("@OtherDetails", OtherDetails);
                    }
                    Data.ExecuteNonQuery(cmd);
                    statusvalue = 0;
                    status = statusvalue;
                }

            }
            catch (Exception ex)
            {
                status = statusvalue;
                DalcLogger.WriteErrorLog(null, ex);
            }
        }


        public static DataTable GETIMPSOTPDETAILS()
        {
            DataTable Dtdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("SELECT_IMPSOTP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {

                    }
                    Dtdata = Data.GetDataTable(cmd);
                    for (int i = 0; Dtdata.Rows.Count > i; i++)
                    {
                        Dtdata.Rows[i]["OTP"] = ConnectionStringEncryptDecrypt.DecryptString(Dtdata.Rows[i]["OTP"].ToString());
                    }
                }
                return Dtdata;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                Dtdata = null;
                return Dtdata;
            }
        }

        #region User Management
        public static DataTable GET_REGISTEREDUSERS(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, int RequestType, string FromDate, string Todate, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("pREQUESTTYPE", RequestType);
                        cmd.Params.Add("pFROMDATE", FromDate);
                        cmd.Params.Add("pTODATE", Todate);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        cmd.Params.Add("@FROMDATE", FromDate);
                        cmd.Params.Add("@TODATE", Todate);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }

                try
                {
                    for (int i = 0; i < DtRegisteredusers.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(DtRegisteredusers.Rows[i]["ACCCLASS"].ToString()))
                        {
                            using (DbNetData DataNew = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                            {
                                DataNew.Open();
                                DbParameter pstatusNew = null;
                                QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                                query.Params.Clear();
                                query.Params["paccountnumber"] = DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString();
                                query.Params["getmode"] = "GETACCOUNTCLASS_RU";
                                object obj = null;
                                query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                                DataTable DTACCCLASS = DataNew.GetDataTable(query);
                                if (DTACCCLASS != null && DTACCCLASS.Rows.Count > 0)
                                {
                                    DtRegisteredusers.Rows[i]["ACCCLASS"] = DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString();
                                    try
                                    {
                                        using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                                        {
                                            Data.Open();
                                            DbParameter pstatus = null;
                                            QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS");
                                            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                                            {
                                                cmd.Params.Add("@ACCOUNTNUMBER", DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString());
                                                cmd.Params.Add("@CUSTOMERID", DtRegisteredusers.Rows[i]["CUSTOMERID"].ToString());
                                                cmd.Params.Add("@ACCLASS", DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString());
                                                cmd.Params.Add("@REQUESTTYPE", 3);
                                                pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                                                cmd.Params.Add("@STATUS", pstatus);
                                                Data.ExecuteNonQuery(cmd);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }

                        //////// Update Branch Code in DB from oracle database

                        if (string.IsNullOrEmpty(DtRegisteredusers.Rows[i]["BRNCODE"].ToString()))
                        {
                            using (DbNetData DataNew = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                            {
                                DataNew.Open();
                                DbParameter pstatusNew = null;
                                QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                                query.Params.Clear();
                                query.Params["paccountnumber"] = DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString();
                                query.Params["getmode"] = "GETACCOUNTCLASS_RU";
                                object obj = null;
                                query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                                DataTable DTACCCLASS = DataNew.GetDataTable(query);
                                if (DTACCCLASS != null && DTACCCLASS.Rows.Count > 0)
                                {
                                    DtRegisteredusers.Rows[i]["BRNCODE"] = DTACCCLASS.Rows[0]["BRNCODE"].ToString();
                                    try
                                    {
                                        using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                                        {
                                            Data.Open();
                                            DbParameter pstatus = null;
                                            QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS");
                                            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                                            {
                                                cmd.Params.Add("@ACCOUNTNUMBER", DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString());
                                                cmd.Params.Add("@CUSTOMERID", DtRegisteredusers.Rows[i]["CUSTOMERID"].ToString());
                                                cmd.Params.Add("@BRNCODE", DTACCCLASS.Rows[0]["BRNCODE"].ToString());
                                                cmd.Params.Add("@REQUESTTYPE", 4);
                                                pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                                                cmd.Params.Add("@STATUS", pstatus);
                                                Data.ExecuteNonQuery(cmd);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
                return DtRegisteredusers;
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable GET_REGISTEREDUSERS_LIST(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, int RequestType, string FromDate, string Todate, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS_LISTCUSTOMER");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("pREQUESTTYPE", RequestType);
                        cmd.Params.Add("pFROMDATE", FromDate);
                        cmd.Params.Add("pTODATE", Todate);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        cmd.Params.Add("@FROMDATE", FromDate);
                        cmd.Params.Add("@TODATE", Todate);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }

                try
                {
                    for (int i = 0; i < DtRegisteredusers.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(DtRegisteredusers.Rows[i]["ACCCLASS"].ToString()))
                        {
                            using (DbNetData DataNew = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                            {
                                DataNew.Open();
                                DbParameter pstatusNew = null;
                                QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                                query.Params.Clear();
                                query.Params["paccountnumber"] = DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString();
                                query.Params["getmode"] = "GETACCOUNTCLASS_RU";
                                object obj = null;
                                query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                                DataTable DTACCCLASS = DataNew.GetDataTable(query);
                                if (DTACCCLASS != null && DTACCCLASS.Rows.Count > 0)
                                {
                                    DtRegisteredusers.Rows[i]["ACCCLASS"] = DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString();
                                    try
                                    {
                                        using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                                        {
                                            Data.Open();
                                            DbParameter pstatus = null;
                                            QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS");
                                            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                                            {
                                                cmd.Params.Add("@ACCOUNTNUMBER", DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString());
                                                cmd.Params.Add("@CUSTOMERID", DtRegisteredusers.Rows[i]["CUSTOMERID"].ToString());
                                                cmd.Params.Add("@ACCLASS", DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString());
                                                cmd.Params.Add("@REQUESTTYPE", 3);
                                                pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                                                cmd.Params.Add("@STATUS", pstatus);
                                                Data.ExecuteNonQuery(cmd);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }

                        //////// Update Branch Code in DB from oracle database

                        if (string.IsNullOrEmpty(DtRegisteredusers.Rows[i]["BRNCODE"].ToString()))
                        {
                            using (DbNetData DataNew = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                            {
                                DataNew.Open();
                                DbParameter pstatusNew = null;
                                QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                                query.Params.Clear();
                                query.Params["paccountnumber"] = DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString();
                                query.Params["getmode"] = "GETACCOUNTCLASS_RU";
                                object obj = null;
                                query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                                DataTable DTACCCLASS = DataNew.GetDataTable(query);
                                if (DTACCCLASS != null && DTACCCLASS.Rows.Count > 0)
                                {
                                    DtRegisteredusers.Rows[i]["BRNCODE"] = DTACCCLASS.Rows[0]["BRNCODE"].ToString();
                                    try
                                    {
                                        using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                                        {
                                            Data.Open();
                                            DbParameter pstatus = null;
                                            QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSREGISTEREDUSERS");
                                            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                                            {
                                                cmd.Params.Add("@ACCOUNTNUMBER", DtRegisteredusers.Rows[i]["ACCOUNTNUMBER"].ToString());
                                                cmd.Params.Add("@CUSTOMERID", DtRegisteredusers.Rows[i]["CUSTOMERID"].ToString());
                                                cmd.Params.Add("@BRNCODE", DTACCCLASS.Rows[0]["BRNCODE"].ToString());
                                                cmd.Params.Add("@REQUESTTYPE", 4);
                                                pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                                                cmd.Params.Add("@STATUS", pstatus);
                                                Data.ExecuteNonQuery(cmd);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
                return DtRegisteredusers;
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }


        

        public static DataTable GET_SignUpRequest(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, int RequestType, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSSIGNUPREQUEST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("pREQUESTTYPE", RequestType);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable Set_ApproveDeclineSignUpRequest(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, string RequestType, string ApprovedBy, string Reason, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("SET_IMPSSIGNUPREQUEST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("pREQUESTTYPE", RequestType);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REASON", Reason);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REASON", Reason);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable GET_AddAccountRequest(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, string LoginUser, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSADDACCOUNTREQUEST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@REQUESTTYPE", "SELECT");
                        cmd.Params.Add("@APPROVEDECLINEDBY", LoginUser);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REASON", null);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REQUESTTYPE", "SELECT");
                        cmd.Params.Add("@APPROVEDECLINEDBY", LoginUser);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REASON", null);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable Set_ApproveDeclineAddAccountRequest(string ACCOUNTNUMBER, string MOBILENUMBER, string CustomerID, string RequestType, string LoginUser, string Reason, out int status, out string PrimaryAccount, out string PrimaryMob)
        {
            status = -1;
            PrimaryAccount = string.Empty;
            PrimaryMob = string.Empty;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pPrimaryAcc = null;
                    DbParameter pPrimaryMob = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSADDACCOUNTREQUEST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        cmd.Params.Add("@APPROVEDECLINEDBY", LoginUser);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REASON", Reason);
                        pPrimaryAcc = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@PRIMARYACC" };
                        cmd.Params.Add("@PRIMARYACC", pPrimaryAcc);
                        pPrimaryMob = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@PRIMARYMOB" };
                        cmd.Params.Add("@PRIMARYMOB", pPrimaryMob);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REQUESTTYPE", RequestType);
                        cmd.Params.Add("@APPROVEDECLINEDBY", LoginUser);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@REASON", Reason);
                        pPrimaryAcc = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@PRIMARYACC" };
                        cmd.Params.Add("@PRIMARYACC", pPrimaryAcc);
                        pPrimaryMob = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@PRIMARYMOB" };
                        cmd.Params.Add("@PRIMARYMOB", pPrimaryMob);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    PrimaryAccount = pPrimaryAcc.Value.ToString();
                    PrimaryMob = pPrimaryMob.Value.ToString();
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        #endregion

        #region location
        public static DataTable SET_INSERTBRANCH_ATM_LOCATION(string BRANCHCODE, string BRANCHLOCATION, string LATTITUDE, string LONGITUDE, int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTBRANCH_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pBRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("pBRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("pLATTITUDE", LATTITUDE);
                        cmd.Params.Add("pLONGITUDE", LONGITUDE);
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@BRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("@BRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("@LATTITUDE", LATTITUDE);
                        cmd.Params.Add("@LONGITUDE", LONGITUDE);
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable GET_ATM_LOCATION(int REGTYPE, string BranchCode, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        cmd.Params.Add("pBRANCHCODE", BranchCode);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        cmd.Params.Add("@BRANCHCODE", BranchCode);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable UPDATE_ATM_LOCATION(string BRANCHCODE, string BRANCHLOCATION, string LATTITUDE, string LONGITUDE, int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UPDATE_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pBRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("pBRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("pLATTITUDE", LATTITUDE);
                        cmd.Params.Add("pLONGITUDE", LONGITUDE);
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@BRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("@BRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("@LATTITUDE", LATTITUDE);
                        cmd.Params.Add("@LONGITUDE", LONGITUDE);
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static void DELETE_ATM_LOCATION(string BRANCHCODE, string BRANCHLOCATION, string LATTITUDE, string LONGITUDE, int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_DELETE_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pBRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("pBRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("pLATTITUDE", LATTITUDE);
                        cmd.Params.Add("pLONGITUDE", LONGITUDE);
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@BRANCHCODE", BRANCHCODE);
                        cmd.Params.Add("@BRANCHLOCATION", BRANCHLOCATION);
                        cmd.Params.Add("@LATTITUDE", LATTITUDE);
                        cmd.Params.Add("@LONGITUDE", LONGITUDE);
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
            }
        }
        #endregion location

        public static void BLOCK_UNBLOCK_RESETPASS_PIN(string AccountNumber, string MobileNumber, int Action, string TransType, string LoginPass, string Offset, string User, string Reason, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_BLOCKIMPSUSER");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pACTION", Action);
                        cmd.Params.Add("pTRANSTYPE", TransType);
                        cmd.Params.Add("pLOGINPASS", LoginPass);
                        cmd.Params.Add("pOFFSET", Offset);
                        cmd.Params.Add("pAPPROVEDBY", User);
                        cmd.Params.Add("pREASON", Reason);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACTION", Action);
                        cmd.Params.Add("@TRANSTYPE", TransType);
                        cmd.Params.Add("@LOGINPASS", LoginPass);
                        cmd.Params.Add("@OFFSET", Offset);
                        cmd.Params.Add("@APPROVEDBY", User);
                        cmd.Params.Add("@REASON", Reason);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();

                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable MERCHANTDETAILS(int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable ACTIVEINACTIVEMERCHANT(int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        //public static DataTable REGISTERMERCHANT(string MerchantName, string MerchantCode, string MerchantAccountNumber, string MerchantMobileNumber, string MerchantAddress, string MerchantBankCode, string MerchantCID, string TransType, string STATE, string EMAIL, string UserID, out int status)
        //{
        //    status = -1;
        //    DataTable DtMerchantdetails = null;
        //    try
        //    {

        //        using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
        //        {
        //            Data.Open();
        //            DbParameter pstatus = null;
        //            QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTUPDATEMERCHANT");
        //            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
        //            {
        //                cmd.Params.Add("pMERCHANTNAME", MerchantName);
        //                cmd.Params.Add("pMERCHANTCODE", MerchantCode);
        //                cmd.Params.Add("pMERCHANTACCOUNTNUMBER", MerchantAccountNumber);
        //                cmd.Params.Add("pMERCHANTMOBILENUMBER", MerchantMobileNumber);
        //                cmd.Params.Add("pMERCHANTADDRESS", MerchantAddress);
        //                cmd.Params.Add("pMERCHANTBANKCODE", MerchantBankCode);
        //                cmd.Params.Add("pMERCHANTCID", MerchantCID);
        //                cmd.Params.Add("pTRANSTYPE", TransType);
        //                cmd.Params.Add("pSTATE", STATE);
        //                cmd.Params.Add("pEMAIL", EMAIL);
        //                cmd.Params.Add("@CREATEDBY", UserID);
        //                cmd.Params.Add("@UPDATEDBY", UserID);
        //                pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
        //                cmd.Params.Add("pSTATUS", pstatus);
        //            }
        //            else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
        //            {
        //                cmd.Params.Add("@MERCHANTNAME", MerchantName);
        //                cmd.Params.Add("@MERCHANTCODE", MerchantCode);
        //                cmd.Params.Add("@MERCHANTACCOUNTNUMBER", MerchantAccountNumber);
        //                cmd.Params.Add("@MERCHANTMOBILENUMBER", MerchantMobileNumber);
        //                cmd.Params.Add("@MERCHANTADDRESS", MerchantAddress);
        //                cmd.Params.Add("@MERCHANTBANKCODE", MerchantBankCode);
        //                cmd.Params.Add("@MERCHANTCID", MerchantCID);
        //                cmd.Params.Add("@TRANSTYPE", TransType);
        //                cmd.Params.Add("@STATE", STATE);
        //                cmd.Params.Add("@EMAIL", EMAIL);
        //                cmd.Params.Add("@CREATEDBY", UserID);
        //                cmd.Params.Add("@UPDATEDBY", UserID);
        //                pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
        //                cmd.Params.Add("@STATUS", pstatus);
        //            }
        //            DtMerchantdetails = Data.GetDataTable(cmd);
        //            status = int.Parse(pstatus.Value.ToString());
        //            Data.Close();
        //            Data.Dispose();
        //            return DtMerchantdetails;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = -1;
        //        DalcLogger.WriteErrorLog(null, ex);
        //        return DtMerchantdetails = null;
        //    }
        //}



        public static DataTable REGISTERMERCHANT(string MerchantName, string MerchantCode, string MerchantAccountNumber, string MerchantMobileNumber, string MerchantAddress, string MerchantBankCode, string MerchantCID, string TransType, string STATE, string EMAIL, string UserID, out int status, string MerchantCategory, string MerchantPAN, string QRType)
        {
            status = -1;
            DataTable DtMerchantdetails = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTUPDATEMERCHANT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pMERCHANTNAME", MerchantName);
                        cmd.Params.Add("pMERCHANTCODE", MerchantCode);
                        cmd.Params.Add("pMERCHANTACCOUNTNUMBER", MerchantAccountNumber);
                        cmd.Params.Add("pMERCHANTMOBILENUMBER", MerchantMobileNumber);
                        cmd.Params.Add("pMERCHANTADDRESS", MerchantAddress);
                        cmd.Params.Add("pMERCHANTBANKCODE", MerchantBankCode);
                        cmd.Params.Add("pMERCHANTCID", MerchantCID);
                        cmd.Params.Add("pTRANSTYPE", TransType);
                        cmd.Params.Add("pSTATE", STATE);
                        cmd.Params.Add("pEMAIL", EMAIL);
                        cmd.Params.Add("@CREATEDBY", UserID);
                        cmd.Params.Add("@UPDATEDBY", UserID);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                        cmd.Params.Add("@MERCHANTCATEGORY", MerchantCategory);
                        cmd.Params.Add("@MERCHANTPAN", ConnectionStringEncryptDecrypt.EncryptString(MerchantPAN));
                        cmd.Params.Add("@QRTYPE", QRType);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@MERCHANTNAME", MerchantName);
                        cmd.Params.Add("@MERCHANTCODE", MerchantCode);
                        cmd.Params.Add("@MERCHANTACCOUNTNUMBER", MerchantAccountNumber);
                        cmd.Params.Add("@MERCHANTMOBILENUMBER", MerchantMobileNumber);
                        cmd.Params.Add("@MERCHANTADDRESS", MerchantAddress);
                        cmd.Params.Add("@MERCHANTBANKCODE", MerchantBankCode);
                        cmd.Params.Add("@MERCHANTCID", MerchantCID);
                        cmd.Params.Add("@TRANSTYPE", TransType);
                        cmd.Params.Add("@STATE", STATE);
                        cmd.Params.Add("@EMAIL", EMAIL);
                        cmd.Params.Add("@CREATEDBY", UserID);
                        cmd.Params.Add("@UPDATEDBY", UserID);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                        cmd.Params.Add("@MERCHANTCATEGORY", MerchantCategory);
                        cmd.Params.Add("@MERCHANTPAN", ConnectionStringEncryptDecrypt.EncryptString(MerchantPAN));
                        cmd.Params.Add("@QRTYPE", QRType);
                    }
                    DtMerchantdetails = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtMerchantdetails;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtMerchantdetails = null;
            }
        }


        public static DataTable GET_PENDINGCHECKDETAILS(int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable GET_APPROVECHECKES(int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static DataTable GETCHECKDETAIL(int REGTYPE, out int status)
        {
            status = -1;
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECT_ATM_LOCATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGTYPE", REGTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusers = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusers;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusers = null;
            }
        }

        public static void APPROVE_DECLINE_CHECK(string MobileNumber, string CustomerID, string AccountNumber, string ChequeID, string Status, string ApprovedBy, string Reason)
        {
            DataTable DtRegisteredusers = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_APPROVE_DECLINE_CHEQUE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@CHEQUEID", ChequeID);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REASON", Reason);
                        cmd.Params.Add("@STATUS", Status);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@CHEQUEID", ChequeID);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REASON", Reason);
                        cmd.Params.Add("@STATUS", Status);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable GetBankDetails()
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_SELECTBANK");
                    DTBankDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }

        public static DataTable GetChequeDetails(string MobileNumber, string CustomerID, string AccountNumber,string chequeno, out int status,string fromdt,string todt)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GET_DEPOSITEDCHEQUE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CHEQUENO", chequeno);
                        objCmd.Params.Add("@TYPE", "DATA");
                        objCmd.Params.Add("@FROMDATE", fromdt);
                        objCmd.Params.Add("@TODATE", todt);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CHEQUENO", chequeno);
                        objCmd.Params.Add("@FROMDATE", fromdt);
                        objCmd.Params.Add("@TODATE", todt);
                        objCmd.Params.Add("@TYPE", "DATA");
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    DTBankDetails = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }

        public static DataTable GetChequeReportDetails(string MobileNumber, string CustomerID, string AccountNumber, string chequeno, out int status, string fromdt, string todt)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GET_DEPOSITEDCHEQUE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CHEQUENO", chequeno);
                        objCmd.Params.Add("@TYPE", "REPORT");
                         objCmd.Params.Add("@FROMDATE", fromdt);
                        objCmd.Params.Add("@TODATE", todt);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CHEQUENO", chequeno);
                        objCmd.Params.Add("@TYPE", "REPORT");
                         objCmd.Params.Add("@FROMDATE", fromdt);
                        objCmd.Params.Add("@TODATE", todt);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    DTBankDetails = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }

        public static DataTable UploadImages(byte[] IMAGEDATA, string Action, int ID)
        {
            DataTable DTImageList = new DataTable();
            DTImageList = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_IMPSIMAGEDISTRIBUTION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@IMAGEDATA", IMAGEDATA);
                        objCmd.Params.Add("@ACTION", Action);
                        objCmd.Params.Add("@ID", ID);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        if (IMAGEDATA == null)
                        {
                            objCmd.Params.Add("@ACTION", Action);
                            objCmd.Params.Add("@ID", ID);
                        }
                        else
                        {
                            objCmd.Params.Add("@IMAGEDATA", IMAGEDATA);
                            objCmd.Params.Add("@ACTION", Action);
                            objCmd.Params.Add("@ID", ID);
                        }
                    }
                    Data.ExecuteNonQuery(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImageList;

            }
            catch (Exception ex)
            {
                DTImageList = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImageList;
            }
        }

        public static DataTable Get_DisImages(byte[] IMAGEDATA, string Action, int ID)
        {
            DataTable DTImageList = new DataTable();
            DTImageList = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETIMPSIMAGEDISTRIBUTION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@ID", ID);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@ID", ID);
                    }
                    DTImageList = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImageList;

            }
            catch (Exception ex)
            {
                DTImageList = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImageList;
            }
        }

        public static DataTable GetIMPSReports(string MSGID, string CollerID, string FromAccount, string ToAccount, string ResponseCode, string FromDate, string ToDate, string TxnType, string BankCode, string Status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_IMPSTRANSACTIONS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable Set_Select_ShowDetails(string FLAG, string ShowID, string ShowName, string DateFrom, string DateTo, string Amount, string TransferLimit)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_CONTEST_SHOWMASTER_INSERTUPDATE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ShowID", ShowID);
                        objCmd.Params.Add("@ShowName", ShowName);
                        objCmd.Params.Add("@DateFrom", DateFrom);
                        objCmd.Params.Add("@DateTo", DateTo);
                        objCmd.Params.Add("@Amount", Amount);
                        objCmd.Params.Add("@TransferLimit", TransferLimit);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ShowID", ShowID);
                        objCmd.Params.Add("@ShowName", ShowName);
                        objCmd.Params.Add("@DateFrom", DateFrom);
                        objCmd.Params.Add("@DateTo", DateTo);
                        objCmd.Params.Add("@Amount", Amount);
                        objCmd.Params.Add("@TransferLimit", TransferLimit);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable GetIMPSReqReportsDetails(string MSGID, string CollerID, string FromAccount, string ToAccount, string ResponseCode, string FromDate, string ToDate, string TxnType, string BankCode, string Status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_RequestMoneyReport");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        //objCmd.Params.Add("@MSGID", MSGID);
                        //objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMMOBILE", FromAccount);
                        objCmd.Params.Add("@TOMOBILE", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@REFERENCENUMBER", TxnType);
                        objCmd.Params.Add("@STATUS", Status);
                        //objCmd.Params.Add("@BANKCODE", BankCode);
                       
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        //objCmd.Params.Add("@MSGID", MSGID);
                        //objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMMOBILE", FromAccount);
                        objCmd.Params.Add("@TOMOBILE", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@REFERENCENUMBER", TxnType);
                        objCmd.Params.Add("@STATUS", Status);
                        //objCmd.Params.Add("@BANKCODE", BankCode);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }


        public static DataTable Set_GlobalLimit(string AccountNumber, string MobileNumber, string Limit, string CustomerID, string ACQLimit, string BNGULSTRTRANSFERLIMIT)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("SET_IMPSREGISTEREDUSERS_LIMIT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@Limit", Limit);
                        objCmd.Params.Add("@ACQLimit", ACQLimit);
                        objCmd.Params.Add("@BNGULSTRTRANSFERLIMIT", BNGULSTRTRANSFERLIMIT);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CUSTOMERID", CustomerID);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@Limit", Limit);
                        objCmd.Params.Add("@ACQLimit", ACQLimit);
                        objCmd.Params.Add("@BNGULSTRTRANSFERLIMIT", BNGULSTRTRANSFERLIMIT);
                    }
                   // Data.ExecuteNonQuery(objCmd);
                    DTImpsReport = Data.GetDataTable(objCmd);

                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }



        public static DataTable Set_Select_ContestantDetails(string FLAG, string ContestantID, string ID, string ContestantName, string ContestantNumber, string IsRemoved)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_CONTEST_CONTESTMASTER_INSERTUPDATE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ContestID", ContestantID);
                        objCmd.Params.Add("@ShowID", ID);
                        objCmd.Params.Add("@ContestantName", ContestantName);
                        objCmd.Params.Add("@contestantNumber", ContestantNumber);
                        objCmd.Params.Add("@IsRemoved", IsRemoved);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ContestID", ContestantID);
                        objCmd.Params.Add("@ShowID", ID);
                        objCmd.Params.Add("@ContestantName", ContestantName);
                        objCmd.Params.Add("@contestantNumber", ContestantNumber);
                        objCmd.Params.Add("@IsRemoved", IsRemoved);

                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable Set_InsertUpdateDelete_ShowDetails(string FLAG, string ShowID, string ShowName, string DateFrom, string DateTo, string Amount, string TransferLimit, out Int32 status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            DbParameter pstatus = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_CONTEST_SHOWMASTER_INSERTUPDATE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ShowID", ShowID);
                        objCmd.Params.Add("@ShowName", ShowName);
                        objCmd.Params.Add("@DateFrom", DateFrom);
                        objCmd.Params.Add("@DateTo", DateTo);
                        objCmd.Params.Add("@Amount", Amount);
                        objCmd.Params.Add("@TransferLimit", TransferLimit);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@FLAG", FLAG);
                        objCmd.Params.Add("@ShowID", ShowID);
                        objCmd.Params.Add("@ShowName", ShowName);
                        objCmd.Params.Add("@DateFrom", DateFrom);
                        objCmd.Params.Add("@DateTo", DateTo);
                        objCmd.Params.Add("@Amount", Amount);
                        objCmd.Params.Add("@TransferLimit", TransferLimit);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;

            }
        }

        

        public static DataTable GET_QRDATA(string AccountNumber, string MobileNumber, string MerchantCode)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("GET_QRDATA");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@MERCHANTCODE", MerchantCode);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@MERCHANTCODE", MerchantCode);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static void ADDACCOUNT(string CustomerName, string CustomerID, string REGACCOUNTNUMBER, string MobileNumber, string AccountNumber, string AccountType, string CCY, string ACCSTATUS, string ApprovedBy, string ReqType, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSACCOUNTS_MASTER");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@REGACCOUNTNUMBER", REGACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERNAME", CustomerName);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@ACCOUNTTYPE", AccountType);
                        cmd.Params.Add("@CCY", CCY);
                        cmd.Params.Add("@ACCSTATUS", ACCSTATUS);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REQTYPE", ReqType);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGACCOUNTNUMBER", REGACCOUNTNUMBER);
                        cmd.Params.Add("@CUSTOMERNAME", CustomerName);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@ACCOUNTTYPE", AccountType);
                        cmd.Params.Add("@CCY", CCY);
                        cmd.Params.Add("@ACCSTATUS", ACCSTATUS);
                        cmd.Params.Add("@APPROVEDBY", ApprovedBy);
                        cmd.Params.Add("@REQTYPE", ReqType);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);

                    status = int.Parse(pstatus.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable ADDDONOR(string ID, string Name, string ProductCode, string AccountNumber, string ReqType,string Type, out Int32 status)
        {
            status = -1;
            DataTable dtDonor = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_INSERTDONOR");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ID", ID);
                        cmd.Params.Add("@Name", Name);
                        cmd.Params.Add("@Product", ProductCode);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@ReqType", ReqType);
                        cmd.Params.Add("@TYPE", Type);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ID", ID);
                        cmd.Params.Add("@Name", Name);
                        cmd.Params.Add("@Product", ProductCode);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@ReqType", ReqType);
                        cmd.Params.Add("@TYPE", Type);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    dtDonor = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    return dtDonor;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return dtDonor;
            }
        }

        public static DataTable GET_ADDACCOUNT(string REGACCOUNTNUMBER, string MobileNumber, string AccountNumber)
        {
            DataTable DTACCOUNTDETAILS = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_GETADDTIMPSACCOUNTS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@REGACCOUNTNUMBER", REGACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGACCOUNTNUMBER", REGACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    }
                    DTACCOUNTDETAILS = Data.GetDataTable(cmd);
                }
                return DTACCOUNTDETAILS;

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTACCOUNTDETAILS = null;
                return DTACCOUNTDETAILS;
            }
        }

        public static DataTable SyncAccounts(string CustomerID, string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pCustomerID"] = CustomerID;
                    query.Params["getmode"] = "BLQ";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
               (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST_MASTER");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("PAccountNumber", AccountNumber);
                        Cmd.Params.Add("@ProcessingCode", "31");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        Cmd.Params.Add("@ProcessingCode", "31");
                    }
                    DTCustomerData_New = Data1.GetDataTable(Cmd);
                }

                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = 0;
                    try
                    {
                        DTCustomerData.Merge(DTCustomerData_New);
                    }
                    catch { }
                }
                else
                {
                    status = -1;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable SelectOfflineRequest_Debit(string AccountNumber, string MobileNumber)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_DEBITCARD");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable SelectOfflineRequest_Cheque(string AccountNumber, string MobileNumber)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_CHEQUE");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable SelectOfflineRequest_Statement(string AccountNumber, string MobileNumber)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_STATEMENT");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable ApproveDeclineOfflineRequest_Statement(string ID, string AccountNumber, string MobileNumber, string Loginuserid, string ReqType, string TransactionType, string Reason, out int status)
        {
            DataTable DtReport = new DataTable();
            status = -1;
            try
            {
                DbParameter pstatus = null;
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", Loginuserid);
                    objCmd.Params.Add("@Type", TransactionType);
                    objCmd.Params.Add("@REQTYPE", ReqType);
                    objCmd.Params.Add("@ID", ID);
                    objCmd.Params.Add("@REASON", Reason);
                    pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    DtReport = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
            return DtReport;
        }

        public static DataTable ApproveDeclineOfflineRequest_Chequebook(string ID, string AccountNumber, string MobileNumber, string Loginuserid, string ReqType, string TransactionType, string Reason, out int status)
        {
            DataTable DtReport = new DataTable();
            status = -1;
            try
            {
                DbParameter pstatus = null;
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", Loginuserid);
                    objCmd.Params.Add("@Type", TransactionType);
                    objCmd.Params.Add("@REQTYPE", ReqType);
                    objCmd.Params.Add("@ID", ID);
                    objCmd.Params.Add("@REASON", Reason);
                    pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    DtReport = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
            return DtReport;
        }

        public static DataTable ApproveDeclineOfflineRequest_DebitCard(string ID, string AccountNumber, string MobileNumber, string Loginuserid, string ReqType, string TransactionType, string Reason, out int status)
        {
            DataTable DtReport = new DataTable();
            status = -1;
            try
            {
                DbParameter pstatus = null;
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", null);
                    objCmd.Params.Add("@TODATE", null);
                    objCmd.Params.Add("@APPROVEDBY", Loginuserid);
                    objCmd.Params.Add("@Type", TransactionType);
                    objCmd.Params.Add("@REQTYPE", ReqType);
                    objCmd.Params.Add("@ID", ID);
                    objCmd.Params.Add("@REASON", Reason);
                    pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    DtReport = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
            return DtReport;
        }

        public static DataTable GETIMPSCUSTOMERACTIVITYDETAILS()
        {
            DataTable Dtdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_IMPSOTHERTRANSACTIONREPORT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {

                    }
                    Dtdata = Data.GetDataTable(cmd);
                }
                return Dtdata;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                Dtdata = null;
                return Dtdata;
            }
        }

        public static DataTable SelectOfflineRequest_Debit_Report(string AccountNumber, string MobileNumber, string FromDate, string ToDate)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", ToDate);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_DEBITCARD_REPORT");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable SelectOfflineRequest_Cheque_Report(string AccountNumber, string MobileNumber, string FromDate, string ToDate)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", ToDate);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_CHEQUE_REPORT");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable SelectOfflineRequest_Statement_Report(string AccountNumber, string MobileNumber, string FromDate, string ToDate)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEOFFLINEREQUEST");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", ToDate);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "SELECT_STATEMENT_REPORT");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable GetPaymentReports(string MSGID, string CollerID, string FromAccount, string ToAccount, string ResponseCode, string FromDate, string ToDate, string TxnType, string BankCode, string Status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_RECHARGE_PAYMENTTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable GetRDTDReports(string MSGID, string CollerID, string FromAccount, string ToAccount, string ResponseCode, string FromDate, string ToDate, string TxnType, string BankCode, string Status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_RDTD_REPORT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable GetPaymentReversalReports(string MSGID, string CollerID, string FromAccount, string ToAccount, string ResponseCode, string FromDate, string ToDate, string TxnType, string BankCode, string Status)
        {
            DataTable DTImpsReport = new DataTable();
            DTImpsReport = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pIMAGEDATA = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_PAYMENTREVERSALTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MSGID", MSGID);
                        objCmd.Params.Add("@COLLERID", CollerID);
                        objCmd.Params.Add("@FROMACCOUNT", FromAccount);
                        objCmd.Params.Add("@TOACCOUNT", ToAccount);
                        objCmd.Params.Add("@REPONSECODE", ResponseCode);
                        objCmd.Params.Add("@FROMDATE", FromDate);
                        objCmd.Params.Add("@TODATE", ToDate);
                        objCmd.Params.Add("@TXNTYPE", TxnType);
                        objCmd.Params.Add("@BANKCODE", BankCode);
                        objCmd.Params.Add("@STATUS", Status);
                    }
                    DTImpsReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTImpsReport;

            }
            catch (Exception ex)
            {
                DTImpsReport = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTImpsReport;
            }
        }

        public static DataTable RecurringAccountDetails(string CustomerID, string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = AccountNumber;
                    query.Params["getmode"] = "RECURRINGDETAILS";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    status = 0;
                else
                {
                    status = -1;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable TermAccountDetails(string CustomerID, string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = AccountNumber;
                    query.Params["getmode"] = "TERMDETAILS";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    status = 0;
                else
                {
                    status = -1;
                }
                return DTCustomerData;
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable GetFeedBackReport(string AccountNumber, string MobileNumber, string FromDate, string ToDate)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETFEEDBACKREPORT");
                    objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", ToDate);
                    objCmd.Params.Add("@APPROVEDBY", null);
                    objCmd.Params.Add("@Type", null);
                    objCmd.Params.Add("@REQTYPE", "");
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }

        public static DataTable GetContestantNoVoteReport(string ShowID, string ContestantID, string FromDate, string ToDate, string ReferenceNumber, string flag, string IsRemoved)
        {
            DataTable DtReport = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("ContestantsVotingTransactionDetails");
                    objCmd.Params.Add("@ShowID", ShowID);
                    objCmd.Params.Add("@ContestantID", ContestantID);
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", ToDate);
                    objCmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                    objCmd.Params.Add("@Flag", flag);
                    objCmd.Params.Add("@IsRemoved", IsRemoved);
                    DtReport = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DtReport = null;
                DalcLogger.WriteErrorLog(null, ex);
            }
            return DtReport;
        }


        public static DataTable GetRequestMoneyBlockDetails(string AccountNumber, string FromDate, string ToDate, string ReferenceNumber, string flag, out int status, string MobileNumber)
        {  
            status = -1;
            DataTable DtRegisteredusersRequestMoneyBlock = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_REQUESTMONEYBLOCKLIST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pflag", "SELECT");
                        cmd.Params.Add("pAccountNumber", AccountNumber);
                        cmd.Params.Add("pFromDate", FromDate);
                        cmd.Params.Add("pToDate", ToDate);
                        cmd.Params.Add("pReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("pMobileNumber", MobileNumber);
                        //pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        //cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@flag", "SELECT");
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@FromDate", FromDate);
                        cmd.Params.Add("@ToDate", ToDate);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        //pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        //cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusersRequestMoneyBlock = Data.GetDataTable(cmd);
                    //   status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusersRequestMoneyBlock;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusersRequestMoneyBlock = null;
            }
        }


        public static DataTable GetOtherDetailsTran(string AccountNumber, string FromDate, string ToDate, string ReferenceNumber, string flag, out int status, string MobileNumber)
        {
            status = -1;
            DataTable DtRegisteredusersRequestMoneyBlock = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GETOTHERDETAILS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pflag", "SELECT");
                        cmd.Params.Add("pAccountNumber", AccountNumber);
                        cmd.Params.Add("pFromDate", FromDate);
                        cmd.Params.Add("pToDate", ToDate);
                        cmd.Params.Add("pReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("pMobileNumber", MobileNumber);
                        //pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        //cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@flag", "SELECT");
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@FromDate", FromDate);
                        cmd.Params.Add("@ToDate", ToDate);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        //pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        //cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtRegisteredusersRequestMoneyBlock = Data.GetDataTable(cmd);
                    //   status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtRegisteredusersRequestMoneyBlock;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtRegisteredusersRequestMoneyBlock = null;
            }
        }

        public static DataTable GetMerchantCategoryDetails()
        {
            DataTable DTStateDetails = new DataTable();
            DTStateDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("SelectMerchantCategory");
                    DTStateDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTStateDetails;

            }
            catch (Exception ex)
            {
                DTStateDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTStateDetails;
            }
        }
    }
}
