using DbNetLink.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using MaxiSwitch.DALC.Logger;
using System.Configuration;
using MaxiSwitch.DALC.Configuration;
using DALC;
using System.Drawing;
using System.ComponentModel;

namespace MaxiSwitch.DALC.ConsumerTransactions
{
    public class IMPSTransactions
    {

        #region InsertUpdateTransaction

        public static void Transaction(int CommandType, int TransMode, int TransSource, string Source, string UBSCOMP, string PRD, string AccountNumber
                                     , string BranchCode, string BenificiaryBranchCode, string BenificiaryAccountNumber, int BankCode, string TransType, decimal TransactionAmount
                                     , string MSGID, string CollerID, string XREF, string FCCREF, string Multitripid, string Service, string Operation
                                     , string Destination, string FunctionID, string MessageStatus, string MAKERID, string MAKERSTAMP, string CHECKERID, string CHECKERSTAMP, string BOOKDATE
                                     , string ACCTITLE1, string ACCTITLE2, string BENFNAME, string BENFADDR1, string BENFADDR2, string CUSTNAME, string ACCTITLE23, int CycleSNO
                                     , string WarningResponseCode, string ResponseCode, string DeviceID, int Flag)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_IMPSInsertOrUpdateTransactions");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pCommandType", CommandType);
                        cmd.Params.Add("pTransMode", TransMode);
                        cmd.Params.Add("pTransSource", TransSource);
                        cmd.Params.Add("pSource", Source);
                        cmd.Params.Add("pUBSCOMP", UBSCOMP);
                        cmd.Params.Add("pPRD", PRD);
                        cmd.Params.Add("pAccountNumber", AccountNumber);
                        cmd.Params.Add("pBranchCode", BranchCode);
                        cmd.Params.Add("pBenificiaryBranchCode", BenificiaryBranchCode);
                        cmd.Params.Add("pBenificiaryAccountNumber", BenificiaryAccountNumber);
                        cmd.Params.Add("pBankCode", BankCode);
                        cmd.Params.Add("pTransType", TransType);
                        cmd.Params.Add("pTransactionAmount", TransactionAmount);
                        cmd.Params.Add("pMSGID", MSGID);
                        cmd.Params.Add("pCollerID", CollerID);
                        cmd.Params.Add("pXREF", XREF);
                        cmd.Params.Add("pMultitripid", Multitripid);
                        cmd.Params.Add("pService", Service);
                        cmd.Params.Add("pOperation", Operation);
                        cmd.Params.Add("pDestination", Destination);
                        cmd.Params.Add("pFunctionID", FunctionID);
                        cmd.Params.Add("pMessageStatus", MessageStatus);
                        cmd.Params.Add("pMAKERID", MAKERID);
                        cmd.Params.Add("pMAKERSTAMP", MAKERSTAMP);
                        cmd.Params.Add("pCHECKERID", CHECKERID);
                        cmd.Params.Add("pCHECKERSTAMP", CHECKERSTAMP);
                        cmd.Params.Add("pBOOKDATE", BOOKDATE);
                        cmd.Params.Add("pACCTITLE1", ACCTITLE1);
                        cmd.Params.Add("pACCTITLE2", ACCTITLE2);
                        cmd.Params.Add("pBENFNAME", BENFNAME);
                        cmd.Params.Add("pBENFADDR1", BENFADDR1);
                        cmd.Params.Add("pBENFADDR2", BENFADDR2);
                        cmd.Params.Add("pCUSTNAME", CUSTNAME);
                        cmd.Params.Add("pACCTITLE23", ACCTITLE23);
                        cmd.Params.Add("pCycleSNO", CycleSNO);
                        cmd.Params.Add("pWarningResponseCode", WarningResponseCode);
                        cmd.Params.Add("pResponseCode", ResponseCode);
                        cmd.Params.Add("pDeviceID", DeviceID);
                        cmd.Params.Add("pFlag", Flag);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@TransMode", TransMode);
                        cmd.Params.Add("@TransSource", TransSource);
                        cmd.Params.Add("@Source", Source);
                        cmd.Params.Add("@UBSCOMP", UBSCOMP);
                        cmd.Params.Add("@PRD", PRD);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@BranchCode", BranchCode);
                        cmd.Params.Add("@BenificiaryBranchCode", BenificiaryBranchCode);
                        cmd.Params.Add("@BenificiaryAccountNumber", BenificiaryAccountNumber);
                        cmd.Params.Add("@BankCode", BankCode);
                        cmd.Params.Add("@TransType", TransType);
                        cmd.Params.Add("@TransactionAmount", TransactionAmount);
                        cmd.Params.Add("@MSGID", MSGID);
                        cmd.Params.Add("@CollerID", CollerID);
                        cmd.Params.Add("@XREF", XREF);
                        cmd.Params.Add("@FCCREF", FCCREF);
                        cmd.Params.Add("@Multitripid", Multitripid);
                        cmd.Params.Add("@Service", Service);
                        cmd.Params.Add("@Operation", Operation);
                        cmd.Params.Add("@Destination", Destination);
                        cmd.Params.Add("@FunctionID", FunctionID);
                        cmd.Params.Add("@MessageStatus", MessageStatus);
                        cmd.Params.Add("@MAKERID", MAKERID);
                        cmd.Params.Add("@MAKERSTAMP", MAKERSTAMP);
                        cmd.Params.Add("@CHECKERID", CHECKERID);
                        cmd.Params.Add("@CHECKERSTAMP", CHECKERSTAMP);
                        cmd.Params.Add("@BOOKDATE", BOOKDATE);
                        cmd.Params.Add("@ACCTITLE1", ACCTITLE1);
                        cmd.Params.Add("@ACCTITLE2", ACCTITLE2);
                        cmd.Params.Add("@BENFNAME", BENFNAME);
                        cmd.Params.Add("@BENFADDR1", BENFADDR1);
                        cmd.Params.Add("@BENFADDR2", BENFADDR2);
                        cmd.Params.Add("@CUSTNAME", CUSTNAME);
                        cmd.Params.Add("@ACCTITLE23", ACCTITLE23);
                        cmd.Params.Add("@CycleSNO", CycleSNO);
                        cmd.Params.Add("@WarningResponseCode", WarningResponseCode);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@Flag", Flag);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void ReversalTransaction(int CommandType, int TransMode, string TransSource, string Source, string UBSCOMP, string PRD, string AccountNumber
            , string BranchCode, string BenificiaryBranchCode, string BenificiaryAccountNumber, int BankCode, string TransType, decimal TransactionAmount
            , string MSGID, string CollerID, string XREF, string Multitripid, string Service, string Operation
            , string Destination, string FunctionID, string MessageStatus, string MAKERID, string MAKERSTAMP, string CHECKERID, string CHECKERSTAMP, string BOOKDATE
            , string ACCTITLE1, string ACCTITLE2, string BENFNAME, string BENFADDR1, string BENFADDR2, string CUSTNAME, string ACCTITLE23, int CycleSNO
            , string WarningResponseCode, string ResponseCode, string DeviceID, int Flag)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_IMPSInsertOrUpdateReversalTransactions");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pCommandType", CommandType);
                        cmd.Params.Add("pTransMode", TransMode);
                        cmd.Params.Add("pTransSource", TransSource);
                        cmd.Params.Add("pSource", Source);
                        cmd.Params.Add("pUBSCOMP", UBSCOMP);
                        cmd.Params.Add("pPRD", PRD);
                        cmd.Params.Add("pAccountNumber", AccountNumber);
                        cmd.Params.Add("pBranchCode", BranchCode);
                        cmd.Params.Add("pBenificiaryBranchCode", BenificiaryBranchCode);
                        cmd.Params.Add("pBenificiaryAccountNumber", BenificiaryAccountNumber);
                        cmd.Params.Add("pBankCode", BankCode);
                        cmd.Params.Add("pTransType", TransType);
                        cmd.Params.Add("pTransactionAmount", TransactionAmount);
                        cmd.Params.Add("pMSGID", MSGID);
                        cmd.Params.Add("pCollerID", CollerID);
                        cmd.Params.Add("pXREF", XREF);
                        cmd.Params.Add("pMultitripid", Multitripid);
                        cmd.Params.Add("pService", Service);
                        cmd.Params.Add("pOperation", Operation);
                        cmd.Params.Add("pDestination", Destination);
                        cmd.Params.Add("pFunctionID", FunctionID);
                        cmd.Params.Add("pMessageStatus", MessageStatus);
                        cmd.Params.Add("pMAKERID", MAKERID);
                        cmd.Params.Add("pMAKERSTAMP", MAKERSTAMP);
                        cmd.Params.Add("pCHECKERID", CHECKERID);
                        cmd.Params.Add("pCHECKERSTAMP", CHECKERSTAMP);
                        cmd.Params.Add("pBOOKDATE", BOOKDATE);
                        cmd.Params.Add("pACCTITLE1", ACCTITLE1);
                        cmd.Params.Add("pACCTITLE2", ACCTITLE2);
                        cmd.Params.Add("pBENFNAME", BENFNAME);
                        cmd.Params.Add("pBENFADDR1", BENFADDR1);
                        cmd.Params.Add("pBENFADDR2", BENFADDR2);
                        cmd.Params.Add("pCUSTNAME", CUSTNAME);
                        cmd.Params.Add("pACCTITLE23", ACCTITLE23);
                        cmd.Params.Add("pCycleSNO", CycleSNO);
                        cmd.Params.Add("pWarningResponseCode", WarningResponseCode);
                        cmd.Params.Add("pResponseCode", ResponseCode);
                        cmd.Params.Add("pDeviceID", DeviceID);
                        cmd.Params.Add("pFlag", Flag);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@TransMode", TransMode);
                        cmd.Params.Add("@TransSource", TransSource);
                        cmd.Params.Add("@Source", Source);
                        cmd.Params.Add("@UBSCOMP", UBSCOMP);
                        cmd.Params.Add("@PRD", PRD);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@BranchCode", BranchCode);
                        cmd.Params.Add("@BenificiaryBranchCode", BenificiaryBranchCode);
                        cmd.Params.Add("@BenificiaryAccountNumber", BenificiaryAccountNumber);
                        cmd.Params.Add("@BankCode", BankCode);
                        cmd.Params.Add("@TransType", TransType);
                        cmd.Params.Add("@TransactionAmount", TransactionAmount);
                        cmd.Params.Add("@MSGID", MSGID);
                        cmd.Params.Add("@CollerID", CollerID);
                        cmd.Params.Add("@XREF", XREF);
                        cmd.Params.Add("@Multitripid", Multitripid);
                        cmd.Params.Add("@Service", Service);
                        cmd.Params.Add("@Operation", Operation);
                        cmd.Params.Add("@Destination", Destination);
                        cmd.Params.Add("@FunctionID", FunctionID);
                        cmd.Params.Add("@MessageStatus", MessageStatus);
                        cmd.Params.Add("@MAKERID", MAKERID);
                        cmd.Params.Add("@MAKERSTAMP", MAKERSTAMP);
                        cmd.Params.Add("@CHECKERID", CHECKERID);
                        cmd.Params.Add("@CHECKERSTAMP", CHECKERSTAMP);
                        cmd.Params.Add("@BOOKDATE", BOOKDATE);
                        cmd.Params.Add("@ACCTITLE1", ACCTITLE1);
                        cmd.Params.Add("@ACCTITLE2", ACCTITLE2);
                        cmd.Params.Add("@BENFNAME", BENFNAME);
                        cmd.Params.Add("@BENFADDR1", BENFADDR1);
                        cmd.Params.Add("@BENFADDR2", BENFADDR2);
                        cmd.Params.Add("@CUSTNAME", CUSTNAME);
                        cmd.Params.Add("@ACCTITLE23", ACCTITLE23);
                        cmd.Params.Add("@CycleSNO", CycleSNO);
                        cmd.Params.Add("@WarningResponseCode", WarningResponseCode);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@Flag", Flag);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void TransactionOther(int CommandType, string DeviceID, string TransactionType, string AccountNumber, string MobileNumber, string CustomerID, string UserID,
                                            string ResponseCode, string ReferenceNumber, string DOB, string CitizenshipCardNo, int Flag)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTOTHERTRANSACTION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@CustomerID", CustomerID);
                        cmd.Params.Add("@UserID", UserID);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("pFlag", Flag);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@CustomerID", CustomerID);
                        cmd.Params.Add("@UserID", UserID);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@DOB", DOB);
                        cmd.Params.Add("@CitizenShipNumber", CitizenshipCardNo);
                        cmd.Params.Add("@Flag", Flag);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void MpayTransactionOther(int CommandType, string DeviceID, string TransactionType, string AccountNumber, string MobileNumber, string CustomerID, string UserID,
                                         string ResponseCode, string ReferenceNumber, int Flag)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_MPAYINSERTOTHERTRANSACTION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@CustomerID", CustomerID);
                        cmd.Params.Add("@UserID", UserID);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("pFlag", Flag);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CommandType", CommandType);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@CustomerID", CustomerID);
                        cmd.Params.Add("@UserID", UserID);
                        cmd.Params.Add("@ResponseCode", ResponseCode);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@Flag", Flag);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }



        #endregion InsertUpdateTransaction

        #region Add & Modify benificiary

        public static void AddBenificiary(string NichName, string AccountNumber, string BankCode, string DeviceID, string UserID, string CustomerID, string BenificiaryMobile,
                                                  string RemitterMobile, int RegType, string AccountType, out int status)
        {
            status = -1;
            using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
            {
                Data.Open();
                DbParameter pstatus = null;
                QueryCommandConfig cmd = new QueryCommandConfig("ADDBENIFICIARY");
                if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                {
                    cmd.Params.Add("pNICKNAME", NichName);
                    cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                    cmd.Params.Add("pBANKCODE", BankCode);
                    cmd.Params.Add("pDEVICEID", DeviceID);
                    cmd.Params.Add("pUSERID", UserID);
                    cmd.Params.Add("pBENIFICIARYMOBILENUMBER", BenificiaryMobile);
                    cmd.Params.Add("pCustomerID", CustomerID);
                    cmd.Params.Add("pREGTYPE", RegType);
                    cmd.Params.Add("pACCOUNTTYPE", AccountType);
                    pstatus = new OracleParameter() { DbType = DbType.String, Size = 10, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                    cmd.Params.Add("pSTATUS", pstatus);
                }
                else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                {
                    cmd.Params.Add("@NICKNAME", NichName);
                    cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    cmd.Params.Add("@BANKCODE", BankCode);
                    cmd.Params.Add("@DEVICEID", DeviceID);
                    cmd.Params.Add("@USERID", UserID);
                    cmd.Params.Add("@BENIFICIARYMOBILENUMBER", BenificiaryMobile);
                    cmd.Params.Add("@CustomerID", CustomerID);
                    cmd.Params.Add("@REGTYPE", RegType);
                    cmd.Params.Add("@ACCOUNTTYPE", AccountType);
                    pstatus = new SqlParameter() { DbType = DbType.Int16, Size = 5, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    cmd.Params.Add("@STATUS", pstatus);
                }
                Data.ExecuteNonQuery(cmd);
                status = int.Parse(pstatus.Value.ToString());
                Data.Close();
                Data.Dispose();
            }
        }

        public static void GetBenificiary(string DeviceID, string UserID, string CustomerID, out string AccountDetails, int RegType, string Bankcode, out int status)
        {
            status = -1;
            AccountDetails = string.Empty;
            using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
            {
                Data.Open();
                DbParameter pstatus = null;
                DbParameter pBenificiaryData = null;
                QueryCommandConfig cmd = new QueryCommandConfig("GETBENIFICIARY");
                if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                {
                    cmd.Params.Add("pDEVICEID", DeviceID);
                    cmd.Params.Add("pUSERID", UserID);
                    cmd.Params.Add("pCustomerID", CustomerID);
                    cmd.Params.Add("pREGTYPE", RegType);
                    cmd.Params.Add("pBankCode", Bankcode);
                    pstatus = new OracleParameter() { DbType = DbType.String, Size = -1, Direction = ParameterDirection.Output, ParameterName = "pBenificiaryDetails" };
                    cmd.Params.Add("pBenificiaryDetails", pstatus);
                    pstatus = new OracleParameter() { DbType = DbType.String, Size = -1, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                    cmd.Params.Add("pSTATUS", pstatus);
                }
                else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                {
                    cmd.Params.Add("@DEVICEID", DeviceID);
                    cmd.Params.Add("@USERID", UserID);
                    cmd.Params.Add("@CustomerID", CustomerID);
                    cmd.Params.Add("@BankCode", Bankcode);
                    pBenificiaryData = new SqlParameter() { DbType = DbType.String, Size = -1, Direction = ParameterDirection.Output, ParameterName = "@BenificiaryDetails" };
                    cmd.Params.Add("@BenificiaryDetails", pBenificiaryData);
                    cmd.Params.Add("@REGTYPE", RegType);
                    pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    cmd.Params.Add("@STATUS", pstatus);
                }
                Data.ExecuteNonQuery(cmd);
                status = int.Parse(pstatus.Value.ToString());
                if (!System.DBNull.Value.Equals(pBenificiaryData.Value) && pBenificiaryData.Value != null)
                    AccountDetails = (string)pBenificiaryData.Value;
                else
                    AccountDetails = string.Empty;
                Data.Close();
                Data.Dispose();
            }
        }

        public static void DeleteBenificiary(string DeviceID, string UserID, string CustomerID, string AccountNumber, string MobileNumber, int RegType, out int status)
        {
            status = -1;
            using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
            {
                Data.Open();
                DbParameter pstatus = null;
                QueryCommandConfig cmd = new QueryCommandConfig("DELETEBENIFICIARY");
                if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                {
                    cmd.Params.Add("pDEVICEID", DeviceID);
                    cmd.Params.Add("pUSERID", UserID);
                    cmd.Params.Add("pCustomerID", CustomerID);
                    cmd.Params.Add("pAccountNumber", AccountNumber);
                    cmd.Params.Add("pMobileNumber", MobileNumber);
                    cmd.Params.Add("pREGTYPE", RegType);
                    pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                    cmd.Params.Add("pSTATUS", pstatus);
                }
                else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                {
                    cmd.Params.Add("@DEVICEID", DeviceID);
                    cmd.Params.Add("@USERID", UserID);
                    cmd.Params.Add("@CustomerID", CustomerID);
                    cmd.Params.Add("@AccountNumber", AccountNumber);
                    cmd.Params.Add("@MobileNumber", MobileNumber);
                    cmd.Params.Add("@REGTYPE", RegType);
                    pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    cmd.Params.Add("@STATUS", pstatus);
                }
                Data.ExecuteNonQuery(cmd);
                status = int.Parse(pstatus.Value.ToString());
                Data.Close();
                Data.Dispose();
            }
        }

        public static DataTable GetBeneficiaryByMob(string MobileNumber, out int status)
        {
            DataTable DTBeneficary = new DataTable();
            DTBeneficary = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETBENEFICIARYBYMOBILE");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    DTBeneficary = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTBeneficary != null && DTBeneficary.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTBeneficary;
            }
            catch (Exception ex)
            {
                DTBeneficary = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTBeneficary;
            }
        }

        #endregion

        #region bank & Customer details

        public static DataTable GetBankDetails(bool IsOtherBank)
        {
            string GetType = "000";
            if (IsOtherBank)
                GetType = "001";
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_SELECTBANK");
                    objCmd.Params.Add("@GetType", GetType);
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

        public static DataTable GetBranchAtmLocation()
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_SELECTBRANCHANDATM");
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

        public static DataTable GetShowDetails()
        {
            DataTable DTShowDetails = new DataTable();
            DataTable DTShowDT = new DataTable();
            DTShowDetails = null;
            DTShowDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_CONTEST_SHOWMASTER");
                    DTShowDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();

                    //DataColumn showImage = new DataColumn("ShowImage_String", typeof(string));
                    //showImage.AllowDBNull = true;
                    //DTShowDetails.Columns.Add(showImage);


                    //foreach (DataRow row in DTShowDetails.Rows)
                    //{
                    //    //byte[] imgBytes = (byte[])(row[6]);
                    //    //string imgString = Convert.ToBase64String(imgBytes);
                    //    //row["ShowImage_String"] = imgString;

                    //    byte[] imgBytes = (byte[])(row[6]);
                    //    string imgString = Convert.ToBase64String(imgBytes);
                    //    row["ShowImage_String"] = imgString;
                    //}


                }
                return DTShowDetails;

            }
            catch (Exception ex)
            {
                DTShowDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTShowDetails;
            }
        }

        public static DataTable GetContestantDetails(string ShowID)
        {
            DataTable DTContestantDetails = new DataTable();
            DTContestantDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_CONTEST_CONTESTMASTER");
                    objCmd.Params.Add("@ShowID", ShowID);
                    DTContestantDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();

                    //DataColumn ContestantImage = new DataColumn("ContestantImage_String", typeof(string));
                    //ContestantImage.AllowDBNull = true;
                    //DTContestantDetails.Columns.Add(ContestantImage);


                    //foreach (DataRow row in DTContestantDetails.Rows)
                    //{
                    //    byte[] imgBytes = (byte[])(row[8]);
                    //    string imgString = Convert.ToBase64String(imgBytes);
                    //    row["ContestantImage_String"] = imgString;
                    //}


                }
                return DTContestantDetails;

            }
            catch (Exception ex)
            {
                DTContestantDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTContestantDetails;
            }
        }

        public static DataTable GETCUSTOMERDETAILS_LOAN(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_LOAN";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERDETAILS_PAYBRN(string CustomerID, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pcustomerid"] = CustomerID;
                    query.Params["getmode"] = "PAYBRN";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERDETAILS_REMITTERBRANCH(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETBRANCH";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERDETAILS_ACC(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        if (DTCustomerData.Rows[0]["CCY"].ToString() == "BTN")
                            status = 0;
                        else
                            status = 74;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERDETAILS_ACC_CC(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        if (DTCustomerData.Rows[0]["CCY"].ToString() == "BTN" || DTCustomerData.Rows[0]["CCY"].ToString() == "USD")
                            status = 0;
                        else
                            status = 74;
                    }
                    else
                        status = 14;
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

        public static DataTable GET_CREDITCARD_CCY(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "CREDITCARDCCY_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        if (DTCustomerData.Rows[0]["CCY"].ToString() == "USD")
                            status = 0;
                        else if (DTCustomerData.Rows[0]["CCY"].ToString() == "INR")
                            status = 1;
                        else
                            status = 74;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERCURRENCY_ACC(string AccountNumber, string MobileNumber, out string CCY)
        {
            CCY = "BTN";
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        CCY = DTCustomerData.Rows[0]["CCY"].ToString();
                    }
                    else
                        CCY = "BTN";
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                CCY = "BTN";
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable GETCUSTOMERDETAILS_MOB(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, DataProvider.SqlClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_VERIFYBENIFICIARY_MOBILE");
                    objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                    DTCustomerData = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTCustomerData.Rows[0][0] == null || DTCustomerData.Rows[0][0].ToString() == "NA")
                {
                    if (MobileNumber.Length == 11)
                    {
                        MobileNumber = MobileNumber.Substring(3, 8);
                    }
                    //using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    //{
                    //    Data.Open();
                    //    DbParameter pstatus = null;
                    //    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    //    query.Params.Clear();
                    //    query.Params["pMobileNumber"] = MobileNumber;
                    //    query.Params["getmode"] = "GETCUSTOMERDETAILS_MOB";
                    //    object obj = null;
                    //    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    //    DTCustomerData = Data.GetDataTable(query);
                    //}
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    if (DTCustomerData.Rows[0]["CCY"].ToString() == "BTN")
                        status = 0;
                    else
                        status = 74;
                }
                else
                    status = 14;

                foreach (DataColumn column in DTCustomerData.Columns)
                    column.ColumnName = column.ColumnName.ToUpper();

                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable GetDefaultDetails()
        {
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("cust_no");
            dt.Columns.Add("cust_ac_no");
            dt.Columns.Add("account_type");
            dt.Columns.Add("first_name");
            dt.Columns.Add("last_name");
            dt.Columns.Add("date_of_birth");
            dt.Columns.Add("mobile_number");
            dt.Columns.Add("e_mail");

            dt.Rows.Add("NA", "NA", "NA", "NA", "NA", "", "NA", "NA");

            //DalcLogger.WriteErrorLog(null, ex);
            return DTCustomerData = dt;
            //}
        }

        #endregion

        #region Sign & Login
        public static DataTable VERIFYBENIFICIARY(string MOBILENUMBER, string CUSTOMERID, string USERID, out int status)
        {
            status = -1;
            DataTable DTcustomerdetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_VERIFYBENIFICIARY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        objCmd.Params.Add("pCUSTOMERID", CUSTOMERID);
                        objCmd.Params.Add("pUSERID", USERID);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        objCmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        objCmd.Params.Add("@CUSTOMERID", CUSTOMERID);
                        objCmd.Params.Add("@USERID", USERID);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    DTcustomerdetails = Data.GetDataTable(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }
                return DTcustomerdetails;

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
                return DTcustomerdetails = null;
            }
        }

        public static void VALIDATESIGNUP(string RefrenceNumber, int cycle, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_IMPSREGISTRATIONVALIDATION");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pCYCLE", cycle);
                        objCmd.Params.Add("pRRN", RefrenceNumber);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        objCmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@CYCLE", cycle);
                        objCmd.Params.Add("@RRN", RefrenceNumber);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        objCmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void RegisterTokenID(string TOKENID, string DEVICEID, string PRIMARYACCOUNT, string MOBILENUMBER, string DEVICETYPE, string ReferenceNumber, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTUPDATEDEVICETOKENID");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pTOKENID", TOKENID);
                        objCmd.Params.Add("pDEVICEID", DEVICEID);
                        objCmd.Params.Add("pREFERENCENUMBER", ReferenceNumber);
                        objCmd.Params.Add("pDeviceType", DEVICETYPE);
                        //objCmd.Params.Add("pPRIMARYACCOUNT", PRIMARYACCOUNT);
                        //objCmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        //objCmd.Params.Add("pDEVICETYPE", DEVICETYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        objCmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@TOKENID", TOKENID);
                        objCmd.Params.Add("@DEVICEID", DEVICEID);
                        objCmd.Params.Add("@REFERENCENUMBER", ReferenceNumber);
                        objCmd.Params.Add("@DeviceType", DEVICETYPE);
                        //objCmd.Params.Add("@PRIMARYACCOUNT", PRIMARYACCOUNT);
                        //objCmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        //objCmd.Params.Add("@DEVICETYPE", DEVICETYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        objCmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void InsertTokenDetails(string DEVICEID, string ReferenceNumber, string MobileNumber, string BENIFICIARYACC, string RemitterTokenID, string success, string failure, string message_id, int flag, decimal amount, string remark, out int status, string BenificiaryMobile, string ExpDT, string Remittername, string Sucess_Decline, string ResponseCode, string Benificiaeryname, string Finalpaymentremark)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTREQUESTMONEY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pDEVICEID", DEVICEID);
                        objCmd.Params.Add("pREFERENCENUMBER", ReferenceNumber);
                        objCmd.Params.Add("pMobileNumber", MobileNumber);
                        objCmd.Params.Add("pBENIFICIARYACC", BENIFICIARYACC);
                        objCmd.Params.Add("pRemitterTokenID", RemitterTokenID);
                        objCmd.Params.Add("psuccess", success);
                        objCmd.Params.Add("pfailure", failure);
                        objCmd.Params.Add("pmessage_id", message_id);
                        objCmd.Params.Add("pflag", flag);
                        objCmd.Params.Add("pamount", amount);
                        objCmd.Params.Add("premark", remark);
                        objCmd.Params.Add("pBenificiaryMobileNumber", BenificiaryMobile);
                        objCmd.Params.Add("pVALIDUPTO", ExpDT);
                        objCmd.Params.Add("pREMITTERNAME", Remittername);
                        objCmd.Params.Add("pSUCESS_DECL_TRANS", Sucess_Decline);
                        objCmd.Params.Add("pResponseCode", ResponseCode);
                        objCmd.Params.Add("pBenificiaryName", Benificiaeryname);
                        objCmd.Params.Add("pFinalpaymentRemark", Finalpaymentremark);

                        //pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        //objCmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@DEVICEID", DEVICEID);
                        objCmd.Params.Add("@REFERENCENUMBER", ReferenceNumber);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@BENIFICIARYACC", BENIFICIARYACC);
                        objCmd.Params.Add("@REMITTERTOKENID", RemitterTokenID);
                        objCmd.Params.Add("@SUCCESS", success);
                        objCmd.Params.Add("@FAILURE", failure);
                        objCmd.Params.Add("@MESSAGE_ID", message_id);
                        objCmd.Params.Add("@flag", flag);
                        objCmd.Params.Add("@amount", amount);
                        objCmd.Params.Add("@remark", remark);
                        objCmd.Params.Add("@BENIFICIARYMOBILENUMBER", BenificiaryMobile);
                        objCmd.Params.Add("@VALIDUPTO", ExpDT);
                        objCmd.Params.Add("@REMITTERNAME", Remittername);
                        objCmd.Params.Add("@SUCESS_DECL_TRANS", Sucess_Decline);
                        objCmd.Params.Add("@ResponseCode", ResponseCode);
                        objCmd.Params.Add("@BenificiaryName", Benificiaeryname);
                        objCmd.Params.Add("@FinalpaymentRemark", Finalpaymentremark);
                        //pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        //objCmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = 0;

                }

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);

            }
        }

        public static DataTable VERIFYCUSTOMERDATA_SIGNUP(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["pMobileNumber"] = MobileNumber;
                    query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_S";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pAccountNumber"] = AccountNumber;
                        query.Params["pMobileNumber"] = MobileNumber;
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_J";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pAccountNumber"] = AccountNumber;
                        query.Params["pMobileNumber"] = MobileNumber;
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_C";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = -1;
                    try
                    {
                        using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                            (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                        {
                            Data1.Open();
                            DbParameter pstatus1 = null;
                            QueryCommandConfig Cmd = new QueryCommandConfig("PROC_VERIFYSIGNUPDATA");
                            if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                            {
                                Cmd.Params.Add("PAccountNumber", AccountNumber);
                                Cmd.Params.Add("PContactNumber", MobileNumber);
                                pstatus1 = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "Pstatus" };
                                Cmd.Params.Add("Pstatus", pstatus1);
                            }
                            else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                            {
                                Cmd.Params.Add("@AccountNumber", AccountNumber);
                                Cmd.Params.Add("@ContactNumber", MobileNumber);
                                pstatus1 = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                                Cmd.Params.Add("@status", pstatus1);
                            }
                            Data1.ExecuteNonQuery(Cmd);
                            status = int.Parse(pstatus1.Value.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        DalcLogger.WriteErrorLog(null, ex);
                    }

                }
                else
                {
                    status = 17;
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

        public static DataTable VERIFYCUSTOMERDATA_REQMONEY(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["pMobileNumber"] = MobileNumber;
                    query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_S";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                    status = 0;
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pAccountNumber"] = AccountNumber;
                        query.Params["pMobileNumber"] = MobileNumber;
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_J";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                        status = 0;
                    }
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pAccountNumber"] = AccountNumber;
                        query.Params["pMobileNumber"] = MobileNumber;
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_SIGNUP_C";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                        status = 0;
                    }
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

        public static void VERIFYSIGNUPSTATUS(string AccountNumber, string MobileNumber, out int status, out string infovalue, out string infotype)
        {
            status = -1;
            infovalue = string.Empty;
            infotype = string.Empty;
            try
            {
                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    DbParameter pinfovalue = null;
                    DbParameter pinfotype = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("PROC_VERIFYSIGNUPDATA");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("PAccountNumber", AccountNumber);
                        Cmd.Params.Add("PContactNumber", MobileNumber);
                        pstatus1 = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "Pstatus" };
                        Cmd.Params.Add("Pstatus", pstatus1);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        Cmd.Params.Add("@ContactNumber", MobileNumber);
                        pstatus1 = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        Cmd.Params.Add("@status", pstatus1);
                        pinfovalue = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@Infovalue" };
                        Cmd.Params.Add("@Infovalue", pinfovalue);
                        pinfotype = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@Infotype" };
                        Cmd.Params.Add("@Infotype", pinfotype);
                    }
                    Data1.ExecuteNonQuery(Cmd);
                    status = int.Parse(pstatus1.Value.ToString());
                    infovalue = pinfovalue.Value.ToString();
                    infotype = pinfotype.Value.ToString();
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable SyncAccounts(string CustomerID, string DeviceID, int type, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                if (type == 31)
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
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "31");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "31");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }

                }
                else if (type == 38)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "MINI";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }

                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "38");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "38");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }

                }
                else if (type == 85)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "CREDITCARD_LIST";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "85");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "85");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else if (type == 97)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "PAYMENT";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "97");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "97");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else if (type == 90)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "CARDLESS";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "98");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "98");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else if (type == 92)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "RECURRING";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                else if (type == 93)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "TERM";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                else if (type == 101)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "FT_BIPS";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "101");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "101");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else
                {
                    //using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    //{
                    //    Data.Open();
                    //    DbParameter pstatus = null;
                    //    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    //    query.Params.Clear();
                    //    query.Params["pCustomerID"] = CustomerID;
                    //    query.Params["getmode"] = "FT";
                    //    object obj = null;
                    //    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    //    DTCustomerData = Data.GetDataTable(query);
                    //}

                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "48");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "48");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                if (DTCustomerData_New != null && DTCustomerData_New.Rows.Count > 0)
                {
                    status = 0;
                    //try
                    //{
                    //    DTCustomerData.Merge(DTCustomerData_New);
                    //}
                    //catch (Exception ex) { DalcLogger.WriteErrorLog(null, ex); }
                }
                else
                {
                    status = -1;
                }
                return DTCustomerData_New;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable MpaySyncAccounts(string CustomerID, string DeviceID, int type, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                if (type == 31)
                {
                    //using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    //{
                    //    Data.Open();
                    //    DbParameter pstatus = null;
                    //    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    //    query.Params.Clear();
                    //    query.Params["pCustomerID"] = CustomerID;
                    //    query.Params["getmode"] = "BLQ";
                    //    object obj = null;
                    //    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    //    DTCustomerData = Data.GetDataTable(query);

                    //   System.Data.DataColumn newColumn = new System.Data.DataColumn("Active_Deactive", typeof(System.String));
                    //   newColumn.DefaultValue ="1";
                    //    DTCustomerData.Columns.Add(newColumn);

                    //}



                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLISTBLOCKUNBLOCK");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);

                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);

                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);

                        //DTCustomerData_New = Data1.GetDataTable(Cmd);
                        //System.Data.DataColumn newColumn = new System.Data.DataColumn("Active_Deactive", typeof(System.String));
                        //newColumn.DefaultValue = "1";
                        //DTCustomerData.Columns.Add(newColumn);
                    }

                }


                if (DTCustomerData_New != null && DTCustomerData_New.Rows.Count > 0)
                {
                    status = 0;

                }
                else
                {
                    status = -1;
                }
                return DTCustomerData_New;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static void MpayBlockAccounts(string CustomerID, string DeviceID, string AccountNumber, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_BLOCKIMPSACCOUNT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {

                        objCmd.Params.Add("pCustomerID", CustomerID);
                        objCmd.Params.Add("pDeviceID", DeviceID);
                        objCmd.Params.Add("pAccountNumber", AccountNumber);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        objCmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@CustomerID", CustomerID);
                        objCmd.Params.Add("@DeviceID", DeviceID);
                        objCmd.Params.Add("@AccountNumber", AccountNumber);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        objCmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable MpayRequestmoneyhistory(string CustomerID, string DeviceID, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GET_REQUESTMONEYBLOCKLIST_MOBILELIST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("pCustomerID", CustomerID);
                        Cmd.Params.Add("pMobileNumber", MobileNumber);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@CustomerID", CustomerID);
                        Cmd.Params.Add("@MobileNumber", MobileNumber);


                    }
                    DTCustomerData = Data1.GetDataTable(Cmd);
                }


                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = 0;

                }
                else
                {
                    status = -1;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -12;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable CardVerification(string AccountNumber, string Cardnumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GET_MPAYACCOUNTCARDVERIFY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("pAccountNumber", AccountNumber);
                        Cmd.Params.Add("pCardNumber", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        Cmd.Params.Add("@CardNumber", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));


                    }
                    DTCustomerData = Data1.GetDataTable(Cmd);




                }


                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = 0;

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

        public static DataTable SyncCustomerID(string CustomerID, string DeviceID, int type, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData = null;
            try
            {

                if (type == 31)
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
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "31");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "31");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                        for (int i = 0; i < DTCustomerData_New.Rows.Count; i++)
                        {
                            DTCustomerData_New.Rows[i]["CUSTOMERID"] = DTCustomerData_New.Rows[i]["ACCOUNTNUMBER"].ToString().Substring(1, 9);
                        }
                    }

                }
                else if (type == 38)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "MINI";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }

                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "38");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "38");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }

                }
                else if (type == 85)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "CREDITCARD_LIST";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "85");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "85");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else if (type == 97)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "PAYMENT";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                   (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "97");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "97");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                else
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "FT";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }

                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "48");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@ProcessingCode", "48");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                    }
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = 0;
                    try
                    {
                        DTCustomerData.Merge(DTCustomerData_New);
                    }
                    catch (Exception ex) { DalcLogger.WriteErrorLog(null, ex); }
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

        public static DataTable LISTOFPRODUCTS(string CustomerID, string DeviceID, string ProductType, string AccountCreationType, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTACCCLASS = new DataTable();
            DTACCCLASS = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData_New = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pCustomerID"] = CustomerID;
                    query.Params["getmode"] = "GETACCOUNTTYPE";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                try
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params["pCustomerID"] = CustomerID;
                        query.Params["getmode"] = "GETACCOUNTCLASS";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTACCCLASS = Data.GetDataTable(query);
                    }
                }
                catch { }
                if (DTCustomerData.Rows.Count > 0)
                {
                    using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                    {
                        Data1.Open();
                        DbParameter pstatus1 = null;
                        QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                        if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@CustomerType", DTCustomerData.Rows[0]["CUSTOMER_TYPE"].ToString());
                            try
                            {
                                Cmd.Params.Add("@AccClass", DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString());
                            }
                            catch { }
                            Cmd.Params.Add("@ProductType", ProductType);
                            Cmd.Params.Add("@AccountCreationType", AccountCreationType);
                            Cmd.Params.Add("@ProcessingCode", "99");
                        }
                        else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                        {
                            Cmd.Params.Add("@CustomerID", CustomerID);
                            Cmd.Params.Add("@DeviceID", DeviceID);
                            Cmd.Params.Add("@CustomerType", DTCustomerData.Rows[0]["CUSTOMER_TYPE"].ToString());
                            try
                            {
                                Cmd.Params.Add("@AccClass", DTACCCLASS.Rows[0]["ACCOUNT_CLASS"].ToString());
                            }
                            catch { }
                            Cmd.Params.Add("@ProductType", ProductType);
                            Cmd.Params.Add("@AccountCreationType", AccountCreationType);
                            Cmd.Params.Add("@ProcessingCode", "99");
                        }
                        DTCustomerData_New = Data1.GetDataTable(Cmd);
                        if (DTCustomerData_New.Rows.Count > 0)
                        {
                            status = 0;
                        }
                        else
                        {
                            status = 17;
                        }
                    }
                    return DTCustomerData_New;
                }
                else
                {
                    status = 17;
                }

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData_New = null;
            }
            return DTCustomerData_New;
        }

        public static DataTable LISTOFDONOR(string CustomerID, string DeviceID, string ProductType, string AccountCreationType, bool IsMisc, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTACCCLASS = new DataTable();
            DTACCCLASS = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData_New = null;
            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GET_ACCOUNTLIST");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("@CustomerID", CustomerID);
                        Cmd.Params.Add("@DeviceID", DeviceID);
                        if (IsMisc)
                            Cmd.Params.Add("@ProcessingCode", "102");
                        else
                            Cmd.Params.Add("@ProcessingCode", "103");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@CustomerID", CustomerID);
                        Cmd.Params.Add("@DeviceID", DeviceID);
                        if (IsMisc)
                            Cmd.Params.Add("@ProcessingCode", "102");
                        else
                            Cmd.Params.Add("@ProcessingCode", "103");
                    }
                    DTCustomerData_New = Data1.GetDataTable(Cmd);
                    if (DTCustomerData_New.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                    {
                        status = 17;
                    }
                }
                return DTCustomerData_New;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData_New = null;
            }
            return DTCustomerData_New;
        }

        public static DataTable VERIFYCUSTOMERDATA(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params.Add("pAccountNumber", AccountNumber);
                    query.Params["getmode"] = "VERIFYCUSTOMERDATA_S";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0 || DTCustomerData.Rows[0]["mobile_number"].ToString().Trim().Length < 8)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params.Add("pAccountNumber", AccountNumber);
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_J";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0 || DTCustomerData.Rows[0]["mobile_number"].ToString().Trim().Length < 8)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params.Add("pAccountNumber", AccountNumber);
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_C";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    status = 0;
                else
                    status = 17;
                return DTCustomerData;
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }
        }

        public static DataTable VERIFYCUSTOMERDATA_FORMAIL(string AccountNumber, string MobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params.Add("pAccountNumber", AccountNumber);
                    query.Params["getmode"] = "VERIFYCUSTOMERDATA_S";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params.Add("pAccountNumber", AccountNumber);
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_J";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData == null || DTCustomerData.Rows.Count <= 0)
                {
                    using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                    {
                        Data.Open();
                        DbParameter pstatus = null;
                        QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                        query.Params.Clear();
                        query.Params.Add("pAccountNumber", AccountNumber);
                        query.Params["getmode"] = "VERIFYCUSTOMERDATA_C";
                        object obj = null;
                        query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                        DTCustomerData = Data.GetDataTable(query);
                    }
                }
                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    status = 0;
                else
                    status = 17;

                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData = null;
            }

        }

        public static void VERIFYCUSTOMERDATADETAILS(string AccountNumber, string CardNumber, out int status, string CardExp)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_VERIFYUSERD");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CardNumber));
                        objCmd.Params.Add("pEXPDT", CardExp.Substring(3, 2) + CardExp.Substring(0, 2));
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        objCmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CardNumber));
                        objCmd.Params.Add("@EXPDT", CardExp.Substring(3, 2) + CardExp.Substring(0, 2));
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void VERIFYCUSTOMERDATA_FP(string AccountNumber, string MobileNumber, string CustomerID, string DeviceID, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_VERIFYUSERD_FP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        objCmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("pDEVICEID", DeviceID);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        objCmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        objCmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        objCmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        objCmd.Params.Add("@DEVICEID", DeviceID);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        objCmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static bool INSERTOTP(string DeviceID, string RefrenceNumber, string OTP, string MobileNumber, string MailID, string TransType)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSOTP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pMobileNumber", MobileNumber);
                        cmd.Params.Add("pDeviceID", DeviceID);
                        cmd.Params.Add("pRefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("pOTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        cmd.Params.Add("@MailID", MailID);
                        cmd.Params.Add("@TransType", TransType);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        //cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        cmd.Params.Add("@OTP",OTP);
                        cmd.Params.Add("@MailID", MailID);
                        cmd.Params.Add("@TransType", TransType);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool VALIDATEOTP(string mobilenumber, string RefrenceNumber, string DeviceID, string OTP, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSOTP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@Mobile", mobilenumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@Mobile", mobilenumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool VALIDATEPGOTP(string mobilenumber, string RefrenceNumber, string DeviceID, string OTP, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSPGOTP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        // cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@Mobile", mobilenumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        // cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@Mobile", mobilenumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static DataTable UpdateMobileCBSWise(string DeviceID, string AccountNumber, string MobileNumberOld, string MobileNumberNew, out Int32 status)
        {
            status = -1;
            DataTable DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_UpdateMobileNumberCBSWise");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumberOld", MobileNumberOld);
                        cmd.Params.Add("@MobileNumberNew", MobileNumberNew);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumberOld", MobileNumberOld);
                        cmd.Params.Add("@MobileNumberNew", MobileNumberNew);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    DTCustomerData = Data.GetDataTable(cmd);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                        status = 0;
                    else
                        status = 17;
                    return DTCustomerData;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = 17;
                return DTCustomerData = null;
            }
        }

        public static DataTable UpdateCIDCBSWise(string DeviceID, string AccountNumber, string MobileNumber, string Infovalue, out Int32 status)
        {
            status = -1;
            DataTable DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_UpdateCID");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumberOld", MobileNumber);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@Infovalue", Infovalue);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    DTCustomerData = Data.GetDataTable(cmd);

                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                        status = 0;
                    else
                        status = 17;
                    return DTCustomerData;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = 17;
                return DTCustomerData = null;
            }
        }


        public static bool VALIDATEGREENPINOTP(string Accountnumber, string RefrenceNumber, string DeviceID, string OTP, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSGREENPINOTP");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@AccountNumber", Accountnumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@RefrenceNumber", RefrenceNumber);
                        cmd.Params.Add("@AccountNumber", Accountnumber);
                        cmd.Params.Add("@OTP", ConnectionStringEncryptDecrypt.EncryptString(OTP));
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool CREATEPASSWORD(string CustomerName, string CustomerID, string CID, string DeviceID, string MobileNumber, string AccountNumber, string AccountType, string LoginPassword, string OFFSET, string CCY, string RegType, string InfoValue, string Infotype, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_REGISTERCUSTOMER");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pCUSTOMERNAME", CustomerName);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pCID", CID);
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("pACCOUNTTYPE", AccountType);
                        cmd.Params.Add("pLOGINPASSWORD", LoginPassword);
                        cmd.Params.Add("pOFFSET", OFFSET);
                        cmd.Params.Add("pCCY", CCY);
                        cmd.Params.Add("pRegType", RegType);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CUSTOMERNAME", CustomerName);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@CID", CID);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@ACCOUNTTYPE", AccountType);
                        cmd.Params.Add("@LOGINPASSWORD", LoginPassword);
                        cmd.Params.Add("@OFFSET", OFFSET);
                        cmd.Params.Add("@CCY", CCY);
                        cmd.Params.Add("@RegType", RegType);
                        cmd.Params.Add("@BranchCode", AccountNumber.Substring(0, 3));//added by sk
                        cmd.Params.Add("@InfoValue", InfoValue);
                        cmd.Params.Add("@InfoType", Convert.ToInt32(Infotype));
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = 0;
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static void VALIDATELOGIN(string DeviceID, string MobileNumber, string Password, out string CustomerName, out int status)
        {
            status = -1;
            CustomerName = string.Empty;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYLOGIN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@LOGINPASSWORD", Password);
                        CUSTOMERNAMEPARAM = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CUSTOMERNAME" };
                        cmd.Params.Add("@CUSTOMERNAME", CUSTOMERNAMEPARAM);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@LOGINPASSWORD", Password);
                        CUSTOMERNAMEPARAM = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CUSTOMERNAME" };
                        cmd.Params.Add("@CUSTOMERNAME", CUSTOMERNAMEPARAM);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    if (!System.DBNull.Value.Equals(CUSTOMERNAMEPARAM.Value) && CUSTOMERNAMEPARAM.Value != null)
                        CustomerName = CUSTOMERNAMEPARAM.Value.ToString();
                    status = (int.Parse(pstatus.Value.ToString()));
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void VALIDATEBIOMETRICLOGIN(string DeviceID, string MobileNumber, out string CustomerName, out int status)
        {
            status = -1;
            CustomerName = string.Empty;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYLOGIN_BIOMETRIC");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);

                        CUSTOMERNAMEPARAM = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CUSTOMERNAME" };
                        cmd.Params.Add("@CUSTOMERNAME", CUSTOMERNAMEPARAM);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);

                        CUSTOMERNAMEPARAM = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CUSTOMERNAME" };
                        cmd.Params.Add("@CUSTOMERNAME", CUSTOMERNAMEPARAM);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    if (!System.DBNull.Value.Equals(CUSTOMERNAMEPARAM.Value) && CUSTOMERNAMEPARAM.Value != null)
                        CustomerName = CUSTOMERNAMEPARAM.Value.ToString();
                    status = (int.Parse(pstatus.Value.ToString()));
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void UPDATEOFFSET(string ACCOUNTNUMBER, string MOBILENUMBER, string OFFSET)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UPDATEIMPSOFFSET");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@OFFSET", OFFSET);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@OFFSET", OFFSET);
                    }
                    Data.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void UPDATEATMOFFSET(string CardNumber, string MOBILENUMBER, string OFFSET)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UpdateCardOffset");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pCardNumber", ConnectionStringEncryptDecrypt.EncryptString(CardNumber));
                        cmd.Params.Add("pPinOffset", OFFSET);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@CardNumber", ConnectionStringEncryptDecrypt.EncryptString(CardNumber));
                        cmd.Params.Add("@PinOffset", OFFSET);
                    }
                    Data.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void CHANGEPASSWORD(string DeviceID, string MobileNumber, string Password, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_CHANGEIMPSPASSWORD");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDeviceID", DeviceID);
                        cmd.Params.Add("pMobileNumber", MobileNumber);
                        cmd.Params.Add("pPassword", Password);
                        pstatus = new OracleParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@Password", Password);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = (int.Parse(pstatus.Value.ToString()));
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void VALIDATESIGNIN(string DeviceID, string AccountNumber, string RefrenceNumber, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SIGNIN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static DataTable VALIDATEUSERID(string MobileNumber, string AccountNumber, string ReferenceNumber, string DeviceID, string CitizenshipCardNo, string DOB, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();

            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter CUSTOMERNAMEPARAM = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYUSERID");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@CitizenshipCardNo", CitizenshipCardNo);
                        cmd.Params.Add("@DOB", DOB);
                        //CUSTOMERNAMEPARAM = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CUSTOMERNAME" };
                        //cmd.Params.Add("@CUSTOMERNAME", CUSTOMERNAMEPARAM);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@CitizenshipCardNo", CitizenshipCardNo);
                        cmd.Params.Add("@DOB", DOB);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    // Data.ExecuteNonQuery(cmd);
                    DTCustomerData = Data.GetDataTable(cmd);
                    status = (int.Parse(pstatus.Value.ToString()));
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }

            return DTCustomerData;
        }

        public static void VALIDATEFORGOTPASS(string DeviceID, string AccountNumber, string InfoValue, int InfoType, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VerifyForgotPass");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@InfoValue", InfoValue);
                        cmd.Params.Add("@InfoType", InfoType);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }


        #endregion Sign & Login

        #region Update Available Amount
        #endregion verify account

        #region ContestTrans
        public static void ContestTransaction(string ShowID, string ContestantID, string ContestantsName, string REMITTERACC, string REMITTERNAME, string ProductCode, string VoteCount, decimal TXNAMT, string ReferenceNumber, string CreateBy, string flag, string DeviceID, string ContestantNumber, string mobilenumber, string TXnRRN)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_Createupdate_Contestantdetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {

                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ContestantID", ContestantID);
                        cmd.Params.Add("@ContestantsName", ContestantsName);
                        cmd.Params.Add("@REMITTERACC", REMITTERACC);
                        cmd.Params.Add("@REMITTERNAME", REMITTERNAME);
                        cmd.Params.Add("@ProductCode", ProductCode);
                        cmd.Params.Add("@VoteCount", VoteCount);
                        cmd.Params.Add("@TXNAMT", TXNAMT.ToString());
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@CreatedBy", CreateBy);
                        cmd.Params.Add("@Flag", flag);
                        cmd.Params.Add("@ContestantNumber", ContestantNumber);
                        cmd.Params.Add("@MobileNumber", mobilenumber);
                        cmd.Params.Add("@TxnReferenceNumber", TXnRRN);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {

                        cmd.Params.Add("@ShowID", ShowID);
                        cmd.Params.Add("@ContestantID", ContestantID);
                        cmd.Params.Add("@ContestantsName", ContestantsName);
                        cmd.Params.Add("@REMITTERACC", REMITTERACC);
                        cmd.Params.Add("@REMITTERNAME", REMITTERNAME);
                        cmd.Params.Add("@ProductCode", ProductCode);
                        cmd.Params.Add("@VoteCount", VoteCount);
                        cmd.Params.Add("@TXNAMT", TXNAMT.ToString());
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@CreatedBy", CreateBy);
                        cmd.Params.Add("@Flag", flag);
                        cmd.Params.Add("@ContestantNumber", ContestantNumber);
                        cmd.Params.Add("@MobileNumber", mobilenumber);
                        cmd.Params.Add("@TxnReferenceNumber", TXnRRN);

                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        #endregion ContestTrans

        #region BlockRequestMoney
        public static DataTable BlockRequestMoneyMobileNumber(string REMITTERMOB, string BenificiaryMobileNumber, string flag, string REMITTERACC, out int status, string remitterName, string CustomerID, string Referencenumber)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            DbParameter pstatus = null;
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_RequestMoneyBlock");
                    objCmd.Params.Add("@RemitterNumber", REMITTERMOB);
                    objCmd.Params.Add("@BenificiaryNumber", BenificiaryMobileNumber);
                    objCmd.Params.Add("@BlockBy", remitterName);
                    objCmd.Params.Add("@CreatedBy", REMITTERACC);
                    objCmd.Params.Add("@flag", flag);
                    objCmd.Params.Add("@CustomerID", CustomerID);

                    objCmd.Params.Add("@ReferenceNumber", Referencenumber);

                    //pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@RowCount1" };
                    //objCmd.Params.Add("@RowCount1", pstatus);

                    DTBankDetails = Data.GetDataTable(objCmd);

                    //status = int.Parse(pstatus.Value.ToString());
                    if (flag == "0")
                    {
                        if (DTBankDetails != null || DTBankDetails.Rows.Count > 0)
                        {
                            status = 1;
                        }
                    }
                    if (flag == "1" && Convert.ToInt16(DTBankDetails.Rows[0][0]) == 1)
                    {
                        status = 1;
                    }
                    Data.Close();
                    Data.Dispose();
                }
                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }
        #endregion BlockRequestMoney

        #region BlockRequestAlreadyExist
        public static DataTable BlockRequestAlreadyExist(string REMITTERMOB, string BenificiaryMobileNumber, string flag, string REMITTERACC, out int status, string remitterName, string CustomerID, string Referencenumber)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            DbParameter pstatus = null;
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("proc_RequestMoneyalreadyBlock");
                    objCmd.Params.Add("@RemitterNumber", REMITTERMOB);
                    objCmd.Params.Add("@BenificiaryNumber", BenificiaryMobileNumber);
                    objCmd.Params.Add("@BlockBy", remitterName);
                    objCmd.Params.Add("@CreatedBy", REMITTERACC);
                    objCmd.Params.Add("@flag", flag);
                    objCmd.Params.Add("@CustomerID", CustomerID);

                    objCmd.Params.Add("@ReferenceNumber", Referencenumber);

                    //pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@RowCount1" };
                    //objCmd.Params.Add("@RowCount1", pstatus);

                    DTBankDetails = Data.GetDataTable(objCmd);

                    //status = int.Parse(pstatus.Value.ToString());
                    if (flag == "0")
                    {
                        if (DTBankDetails != null || DTBankDetails.Rows.Count > 0)
                        {
                            status = 1;
                        }
                    }

                    Data.Close();
                    Data.Dispose();
                }
                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }
        #endregion BlockRequestAlreadyExist

        #region CheckBlockRequestMoney
        public static DataTable CheckBlockRequestMoney(string REMITTERMOB, string BenificiaryMobileNumber, string flag, string REMITTERACC)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;

            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_RequestMoneyBlock");
                    objCmd.Params.Add("@RemitterNumber", REMITTERMOB);
                    objCmd.Params.Add("@BenificiaryNumber", BenificiaryMobileNumber);
                    objCmd.Params.Add("@BlockBy", "");
                    objCmd.Params.Add("@CreatedBy", REMITTERACC);
                    objCmd.Params.Add("@flag", flag);

                    DTBankDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }


                return DTBankDetails;

            }
            catch (Exception ex)
            {
                DTBankDetails = null;
                // status = 14;
                DalcLogger.WriteErrorLog(null, ex);
                return DTBankDetails;
            }
        }
        #endregion CheckBlockRequestMoney

        public void VERIFYIMPSACCOUNT(string REMITTERACC, string MobileNumber, string DeviceID, decimal TXNAMT, ref string AmountAvailable, ref string FtLimit, ref string AccountUseLimit,
                                      ref string AccountUseCount, ref string LastDate, ref string LastTime, ref string MaxPinCount, ref string MaxPinUseCount, ref string PinOffset, out int status
            , ref string ACQAmountAvailable, ref string ACQFtLimit, ref string BNgulAmountAvailable, ref string BNgulFtLimit)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pAmountAvailable = null;
                    DbParameter pFtLimit = null;
                    DbParameter pAccountUseLimit = null;
                    DbParameter pAccountUseCount = null;
                    DbParameter pLastDate = null;
                    DbParameter pLastTime = null;
                    DbParameter pMaxPinCount = null;
                    DbParameter pMaxPinUseCount = null;
                    DbParameter pPinOffset = null;
                    DbParameter pACQAmountAvailable = null;
                    DbParameter pACQFtLimit = null;
                    DbParameter pBNgulAmountAvailable = null;
                    DbParameter pBNgulFtLimit = null;

                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSACCOUNT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);


                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);


                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);
                    }
                    Data.ExecuteNonQuery(cmd);

                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0)
                    {
                        if (!System.DBNull.Value.Equals(pAmountAvailable.Value) && pAmountAvailable.Value != null)
                            AmountAvailable = (string)pAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pFtLimit.Value) && pFtLimit.Value != null)
                            FtLimit = (string)pFtLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseLimit.Value) && pAccountUseLimit.Value != null)
                            AccountUseLimit = (string)pAccountUseLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseCount.Value) && pAccountUseCount.Value != null)
                            AccountUseCount = (string)pAccountUseCount.Value;
                        if (!System.DBNull.Value.Equals(pLastDate.Value) && pLastDate.Value != null)
                            LastDate = (string)pLastDate.Value;
                        if (!System.DBNull.Value.Equals(pLastTime.Value) && pLastTime.Value != null)
                            LastTime = (string)pLastTime.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinCount.Value) && pMaxPinCount.Value != null)
                            MaxPinCount = (string)pMaxPinCount.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinUseCount.Value) && pMaxPinUseCount.Value != null)
                            MaxPinUseCount = (string)pMaxPinUseCount.Value;
                        if (!System.DBNull.Value.Equals(pPinOffset.Value) && pPinOffset.Value != null)
                            PinOffset = (string)pPinOffset.Value;

                        if (!System.DBNull.Value.Equals(pACQAmountAvailable.Value) && pACQAmountAvailable.Value != null)
                            ACQAmountAvailable = (string)pACQAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pACQFtLimit.Value) && pACQFtLimit.Value != null)
                            ACQFtLimit = (string)pACQFtLimit.Value;


                        if (!System.DBNull.Value.Equals(pBNgulAmountAvailable.Value) && pBNgulAmountAvailable.Value != null)
                            BNgulAmountAvailable = (string)pBNgulAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pBNgulFtLimit.Value) && pBNgulFtLimit.Value != null)
                            BNgulFtLimit = (string)pBNgulFtLimit.Value;

                    }

                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public void VERIFYIMPSACCOUNT_PAYMENT(string REMITTERACC, string MobileNumber, string DeviceID, decimal TXNAMT, ref string AmountAvailable, ref string FtLimit, ref string AccountUseLimit,
                                     ref string AccountUseCount, ref string LastDate, ref string LastTime, ref string MaxPinCount, ref string MaxPinUseCount, ref string PinOffset, out int status
           , ref string ACQAmountAvailable, ref string ACQFtLimit, ref string BNgulAmountAvailable, ref string BNgulFtLimit)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pAmountAvailable = null;
                    DbParameter pFtLimit = null;
                    DbParameter pAccountUseLimit = null;
                    DbParameter pAccountUseCount = null;
                    DbParameter pLastDate = null;
                    DbParameter pLastTime = null;
                    DbParameter pMaxPinCount = null;
                    DbParameter pMaxPinUseCount = null;
                    DbParameter pPinOffset = null;
                    DbParameter pACQAmountAvailable = null;
                    DbParameter pACQFtLimit = null;

                    DbParameter pBNgulAmountAvailable = null;
                    DbParameter pBNgulFtLimit = null;

                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSACCOUNT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);

                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);

                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);
                    }
                    Data.ExecuteNonQuery(cmd);

                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0 || status == 47 || status == 48)
                    {
                        if (!System.DBNull.Value.Equals(pAmountAvailable.Value) && pAmountAvailable.Value != null)
                            AmountAvailable = (string)pAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pFtLimit.Value) && pFtLimit.Value != null)
                            FtLimit = (string)pFtLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseLimit.Value) && pAccountUseLimit.Value != null)
                            AccountUseLimit = (string)pAccountUseLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseCount.Value) && pAccountUseCount.Value != null)
                            AccountUseCount = (string)pAccountUseCount.Value;
                        if (!System.DBNull.Value.Equals(pLastDate.Value) && pLastDate.Value != null)
                            LastDate = (string)pLastDate.Value;
                        if (!System.DBNull.Value.Equals(pLastTime.Value) && pLastTime.Value != null)
                            LastTime = (string)pLastTime.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinCount.Value) && pMaxPinCount.Value != null)
                            MaxPinCount = (string)pMaxPinCount.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinUseCount.Value) && pMaxPinUseCount.Value != null)
                            MaxPinUseCount = (string)pMaxPinUseCount.Value;
                        if (!System.DBNull.Value.Equals(pPinOffset.Value) && pPinOffset.Value != null)
                            PinOffset = (string)pPinOffset.Value;

                        if (!System.DBNull.Value.Equals(pACQAmountAvailable.Value) && pACQAmountAvailable.Value != null)
                            ACQAmountAvailable = (string)pACQAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pACQFtLimit.Value) && pACQFtLimit.Value != null)
                            ACQFtLimit = (string)pACQFtLimit.Value;


                        if (!System.DBNull.Value.Equals(pBNgulAmountAvailable.Value) && pBNgulAmountAvailable.Value != null)
                            BNgulAmountAvailable = (string)pBNgulAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pBNgulFtLimit.Value) && pBNgulFtLimit.Value != null)
                            BNgulFtLimit = (string)pBNgulFtLimit.Value;

                    }

                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public void VERIFYIMPSACCOUNTACQ(string REMITTERACC, string MobileNumber, string DeviceID, decimal TXNAMT, ref string AmountAvailable, ref string FtLimit, ref string AccountUseLimit,
                                    ref string AccountUseCount, ref string LastDate, ref string LastTime, ref string MaxPinCount, ref string MaxPinUseCount, ref string PinOffset, out int status, ref string ACQAmountAvailable, ref string ACQFtLimit, ref string BNgulAmountAvailable, ref string BNgulFtLimit)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pAmountAvailable = null;
                    DbParameter pFtLimit = null;
                    DbParameter pAccountUseLimit = null;
                    DbParameter pAccountUseCount = null;
                    DbParameter pLastDate = null;
                    DbParameter pLastTime = null;
                    DbParameter pMaxPinCount = null;
                    DbParameter pMaxPinUseCount = null;
                    DbParameter pPinOffset = null;
                    DbParameter pACQAmountAvailable = null;
                    DbParameter pACQFtLimit = null;
                    DbParameter pBNgulAmountAvailable = null;
                    DbParameter pBNgulFtLimit = null;

                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSACCOUNT_ACCQ");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);


                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);


                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);


                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);


                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);


                    }
                    Data.ExecuteNonQuery(cmd);

                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0)
                    {
                        if (!System.DBNull.Value.Equals(pAmountAvailable.Value) && pAmountAvailable.Value != null)
                            AmountAvailable = (string)pAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pFtLimit.Value) && pFtLimit.Value != null)
                            FtLimit = (string)pFtLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseLimit.Value) && pAccountUseLimit.Value != null)
                            AccountUseLimit = (string)pAccountUseLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseCount.Value) && pAccountUseCount.Value != null)
                            AccountUseCount = (string)pAccountUseCount.Value;
                        if (!System.DBNull.Value.Equals(pLastDate.Value) && pLastDate.Value != null)
                            LastDate = (string)pLastDate.Value;
                        if (!System.DBNull.Value.Equals(pLastTime.Value) && pLastTime.Value != null)
                            LastTime = (string)pLastTime.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinCount.Value) && pMaxPinCount.Value != null)
                            MaxPinCount = (string)pMaxPinCount.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinUseCount.Value) && pMaxPinUseCount.Value != null)
                            MaxPinUseCount = (string)pMaxPinUseCount.Value;
                        if (!System.DBNull.Value.Equals(pPinOffset.Value) && pPinOffset.Value != null)
                            PinOffset = (string)pPinOffset.Value;

                        if (!System.DBNull.Value.Equals(pACQAmountAvailable.Value) && pACQAmountAvailable.Value != null)
                            ACQAmountAvailable = (string)pACQAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pACQFtLimit.Value) && pACQFtLimit.Value != null)
                            ACQFtLimit = (string)pACQFtLimit.Value;

                        if (!System.DBNull.Value.Equals(pBNgulAmountAvailable.Value) && pBNgulAmountAvailable.Value != null)
                            BNgulAmountAvailable = (string)pBNgulAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pBNgulFtLimit.Value) && pBNgulFtLimit.Value != null)
                            BNgulFtLimit = (string)pBNgulFtLimit.Value;

                    }

                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public void VERIFYIMPSACCOUNTBngul(string REMITTERACC, string MobileNumber, string DeviceID, decimal TXNAMT, ref string AmountAvailable, ref string FtLimit, ref string AccountUseLimit,
                                ref string AccountUseCount, ref string LastDate, ref string LastTime, ref string MaxPinCount, ref string MaxPinUseCount,
        ref string PinOffset, out int status, ref string ACQAmountAvailable, ref string ACQFtLimit, ref string BNgulAmountAvailable, ref string BNgulFtLimit)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pAmountAvailable = null;
                    DbParameter pFtLimit = null;
                    DbParameter pAccountUseLimit = null;
                    DbParameter pAccountUseCount = null;
                    DbParameter pLastDate = null;
                    DbParameter pLastTime = null;
                    DbParameter pMaxPinCount = null;
                    DbParameter pMaxPinUseCount = null;
                    DbParameter pPinOffset = null;
                    DbParameter pACQAmountAvailable = null;
                    DbParameter pACQFtLimit = null;
                    DbParameter pBNgulAmountAvailable = null;
                    DbParameter pBNgulFtLimit = null;

                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VERIFYIMPSACCOUNT_BNGUL");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);

                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);

                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", REMITTERACC);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@TransactionAmount", TXNAMT);
                        cmd.Params.Add("@TXNTYPE", null);

                        pAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmountAvailable" };
                        cmd.Params.Add("@AmountAvailable", pAmountAvailable);
                        pFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@FtLimit" };
                        cmd.Params.Add("@FtLimit", pFtLimit);
                        pAccountUseLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseLimit" };
                        cmd.Params.Add("@AccountUseLimit", pAccountUseLimit);
                        pAccountUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AccountUseCount" };
                        cmd.Params.Add("@AccountUseCount", pAccountUseCount);
                        pLastDate = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastDate" };
                        cmd.Params.Add("@LastDate", pLastDate);
                        pLastTime = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@LastTime" };
                        cmd.Params.Add("@LastTime", pLastTime);
                        pMaxPinCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinCount" };
                        cmd.Params.Add("@MaxPinCount", pMaxPinCount);
                        pMaxPinUseCount = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@MaxPinUseCount" };
                        cmd.Params.Add("@MaxPinUseCount", pMaxPinUseCount);
                        pPinOffset = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@PinOffset" };
                        cmd.Params.Add("@PinOffset", pPinOffset);
                        pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);


                        pACQAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQAmountAvailable" };
                        cmd.Params.Add("@ACQAmountAvailable", pACQAmountAvailable);
                        pACQFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@ACQFtLimit" };
                        cmd.Params.Add("@ACQFtLimit", pACQFtLimit);

                        pBNgulAmountAvailable = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulAmountAvailable" };
                        cmd.Params.Add("@BNgulAmountAvailable", pBNgulAmountAvailable);
                        pBNgulFtLimit = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@BNgulFtLimit" };
                        cmd.Params.Add("@BNgulFtLimit", pBNgulFtLimit);

                    }
                    Data.ExecuteNonQuery(cmd);

                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0)
                    {
                        if (!System.DBNull.Value.Equals(pAmountAvailable.Value) && pAmountAvailable.Value != null)
                            AmountAvailable = (string)pAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pFtLimit.Value) && pFtLimit.Value != null)
                            FtLimit = (string)pFtLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseLimit.Value) && pAccountUseLimit.Value != null)
                            AccountUseLimit = (string)pAccountUseLimit.Value;
                        if (!System.DBNull.Value.Equals(pAccountUseCount.Value) && pAccountUseCount.Value != null)
                            AccountUseCount = (string)pAccountUseCount.Value;
                        if (!System.DBNull.Value.Equals(pLastDate.Value) && pLastDate.Value != null)
                            LastDate = (string)pLastDate.Value;
                        if (!System.DBNull.Value.Equals(pLastTime.Value) && pLastTime.Value != null)
                            LastTime = (string)pLastTime.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinCount.Value) && pMaxPinCount.Value != null)
                            MaxPinCount = (string)pMaxPinCount.Value;
                        if (!System.DBNull.Value.Equals(pMaxPinUseCount.Value) && pMaxPinUseCount.Value != null)
                            MaxPinUseCount = (string)pMaxPinUseCount.Value;
                        if (!System.DBNull.Value.Equals(pPinOffset.Value) && pPinOffset.Value != null)
                            PinOffset = (string)pPinOffset.Value;

                        if (!System.DBNull.Value.Equals(pACQAmountAvailable.Value) && pACQAmountAvailable.Value != null)
                            ACQAmountAvailable = (string)pACQAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pACQFtLimit.Value) && pACQFtLimit.Value != null)
                            ACQFtLimit = (string)pACQFtLimit.Value;

                        if (!System.DBNull.Value.Equals(pBNgulAmountAvailable.Value) && pBNgulAmountAvailable.Value != null)
                            BNgulAmountAvailable = (string)pBNgulAmountAvailable.Value;
                        if (!System.DBNull.Value.Equals(pBNgulFtLimit.Value) && pBNgulFtLimit.Value != null)
                            BNgulFtLimit = (string)pBNgulFtLimit.Value;

                    }

                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        /*
        public static bool UpdateCardTxnDetails(string AccountNumber, string MobileNumber, string DeviceID, string AmountAvailable, string CardUseCount, string LastDate, string LastTime, string PINRetryCount,string ACQSTRAMOUNTAVAILABLE)
        {
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UpdateAccountTxnDetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);
                        cmd.Params.Add("@ACQSTRAMOUNTAVAILABLE", ACQSTRAMOUNTAVAILABLE);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);
                        cmd.Params.Add("@ACQSTRAMOUNTAVAILABLE", ACQSTRAMOUNTAVAILABLE);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }
        */

        public static bool UpdateCardTxnDetails(string AccountNumber, string MobileNumber, string DeviceID, string AmountAvailable, string CardUseCount, string LastDate, string LastTime, string PINRetryCount, string ACQSTRAMOUNTAVAILABLE, string BNgulSTRAMOUNTAVAILABLE)
        {
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UpdateAccountTxnDetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);
                        cmd.Params.Add("@ACQSTRAMOUNTAVAILABLE", ACQSTRAMOUNTAVAILABLE);
                        cmd.Params.Add("@BNgulSTRAMOUNTAVAILABLE", BNgulSTRAMOUNTAVAILABLE);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);
                        cmd.Params.Add("@ACQSTRAMOUNTAVAILABLE", ACQSTRAMOUNTAVAILABLE);
                        cmd.Params.Add("@BNgulSTRAMOUNTAVAILABLE", BNgulSTRAMOUNTAVAILABLE);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool UpdateCardTxnDetailsACQ(string AccountNumber, string MobileNumber, string DeviceID, string AmountAvailable, string CardUseCount, string LastDate, string LastTime, string PINRetryCount)
        {
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UpdateAccountTxnDetails_ACQ");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@strAmountAvailable", AmountAvailable);
                        cmd.Params.Add("@strCardUseCount", CardUseCount);
                        cmd.Params.Add("@strLastDate", LastDate);
                        cmd.Params.Add("@strLastTime", LastTime);
                        cmd.Params.Add("@intPINRetryCount", PINRetryCount);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static bool UpdateScheduleTxnDetails(string AccountNumber, string ReferenceNumber)
        {
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UpdateScheduleTxnDetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@RefrenceNumber", ReferenceNumber);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccountNumber", AccountNumber);
                        cmd.Params.Add("@RefrenceNumber", ReferenceNumber);
                    }
                    Data.ExecuteNonQuery(cmd);
                    Data.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        #region verify account

        public static void GETCARDDETAILS(string DeviceID, string CustomerID, string MobileNumber, out string CardDetails, out int status)
        {
            status = -1;
            CardDetails = string.Empty;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    DbParameter CardDetailsparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_GetCardDetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        CardDetailsparam = new OracleParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "pCardList" };
                        cmd.Params.Add("pCardList", CardDetailsparam);
                        Statusparam = new OracleParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        CardDetailsparam = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@CardList" };
                        cmd.Params.Add("@CardList", CardDetailsparam);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);


                    }

                    Data.ExecuteNonQuery(cmd);
                    if (!System.DBNull.Value.Equals(CardDetailsparam.Value) && CardDetailsparam.Value != null)
                        CardDetails = CardDetailsparam.Value.ToString();
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void BLOCKCARD(string DeviceID, string MobileNumber, string Cardnumber, string AccountNumber, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    DbParameter CardDetailsparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_BLOCKCARDS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void UNBLOCKCARD(string DeviceID, string MobileNumber, string Cardnumber, string AccountNumber, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    DbParameter CardDetailsparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_UNBLOCKCARDS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(Cardnumber));
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void SET_CHANNELLIMIT(string DeviceID, string MobileNumber, string CARDNUMBER, string ACCOUNTNUMBER, string STRLIMITATM, string STRLIMITPOS, string STRLIMITECM, string Channel, out string StrAmountAvailable, out int status)
        {
            status = -1;
            StrAmountAvailable = string.Empty;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter Statusparam = null;
                    DbParameter AmtAvlparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_SETChannelLimit");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pSTRLIMITATM", STRLIMITATM);
                        cmd.Params.Add("pSTRLIMITPOS", STRLIMITPOS);
                        cmd.Params.Add("pSTRLIMITECM", STRLIMITECM);
                        cmd.Params.Add("pCHANNEL", Channel);
                        AmtAvlparam = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "pAmtAvl" };
                        cmd.Params.Add("pAmtAvl", AmtAvlparam);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@STRLIMITATM", STRLIMITATM);
                        cmd.Params.Add("@STRLIMITPOS", STRLIMITPOS);
                        cmd.Params.Add("@STRLIMITECM", STRLIMITECM);
                        cmd.Params.Add("@CHANNEL", Channel);
                        AmtAvlparam = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@AmtAvl" };
                        cmd.Params.Add("@AmtAvl", AmtAvlparam);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);
                    }
                    Data.ExecuteNonQuery(cmd);

                    if (!System.DBNull.Value.Equals(AmtAvlparam.Value) && AmtAvlparam.Value != null)
                        StrAmountAvailable = AmtAvlparam.Value.ToString();
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void SET_CHANNELSTATUS(string DeviceID, string MobileNumber, string CARDNUMBER, string ACCOUNTNUMBER, string STRSTATUSATM, string STRSTATUSPOS, string STRSTATUSECM, string Channel, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SETCHANNELSTATUS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pSTRSTATUSATM", STRSTATUSATM);
                        cmd.Params.Add("pSTRSTATUSPOS", STRSTATUSPOS);
                        cmd.Params.Add("pSTRSTATUSECM", STRSTATUSECM);
                        cmd.Params.Add("pCHANNEL", Channel);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@STRSTATUSATM", STRSTATUSATM);
                        cmd.Params.Add("@STRSTATUSPOS", STRSTATUSPOS);
                        cmd.Params.Add("@STRSTATUSECM", STRSTATUSECM);
                        cmd.Params.Add("@CHANNEL", Channel);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void SET_ACTIVITYHISTORY(string DEVICEID, string MOBILENUMBER, string CARDNUMBER, string ACCOUNTNUMBER, string STRCHANNEL, string STRACTIVITY, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTACTIVITY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DEVICEID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("pCARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("pACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("pCHANNEL", STRCHANNEL);
                        cmd.Params.Add("pACTIVITY", STRACTIVITY);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@CARDNUMBER", ConnectionStringEncryptDecrypt.EncryptString(CARDNUMBER));
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@CHANNEL", STRCHANNEL);
                        cmd.Params.Add("@ACTIVITY", STRACTIVITY);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void GET_ACTIVITYHISTORY(string DEVICEID, string MOBILENUMBER, out string STRACTIVITYHISTORY, out int status)
        {
            status = -1;
            STRACTIVITYHISTORY = string.Empty;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter HISTORYDetailsparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_SELECTACTIVITYHISTORY");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DEVICEID);
                        cmd.Params.Add("pMOBILENUMBER", MOBILENUMBER);
                        HISTORYDetailsparam = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "pHISTORYDETAILS" };
                        cmd.Params.Add("pHISTORYDETAILS", HISTORYDetailsparam);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        HISTORYDetailsparam = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@HISTORYDETAILS" };
                        cmd.Params.Add("@HISTORYDETAILS", HISTORYDetailsparam);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    if (!System.DBNull.Value.Equals(HISTORYDetailsparam.Value) && HISTORYDetailsparam.Value != null)
                        STRACTIVITYHISTORY = HISTORYDetailsparam.Value.ToString();
                    status = int.Parse(pstatus.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        #endregion Card Controller

        #region Account Management
        public static bool InsertAllAccounts(string RegAccountNumber, string CustomerName, string CustomerID, string CID, string DeviceID, string MobileNumber, string AccountNumber, string AccountType, string LoginPassword, string OFFSET, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSACCOUNTS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pREGACCOUNTNUMBER", RegAccountNumber);
                        cmd.Params.Add("pCUSTOMERNAME", CustomerName);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pCID", CID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("pACCOUNTTYPE", AccountType);
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        pstatus = new OracleParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@REGACCOUNTNUMBER", RegAccountNumber);
                        cmd.Params.Add("@CUSTOMERNAME", CustomerName);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@CID", CID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@ACCOUNTTYPE", AccountType);
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static DataTable GET_MANAGEACCOUNT(string DeviceID, string UserID, string CustomerID, int TransType, out int status)
        {
            status = -1;
            DataTable DtAccountDetails = null;
            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pBenificiaryData = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GET_MANAGEACCOUNT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pUSERID", UserID);
                        cmd.Params.Add("pCustomerID", CustomerID);
                        cmd.Params.Add("pTransType", TransType);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pStatus" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@USERID", UserID);
                        cmd.Params.Add("@CustomerID", CustomerID);
                        cmd.Params.Add("@TransType", TransType);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    DtAccountDetails = Data.GetDataTable(cmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                    return DtAccountDetails;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return DtAccountDetails = null;
            }
        }
        #endregion

        #region other transactions
        public static bool VERIFYLASTLRANSACTION(string DEVICEID, string RRN, int CHECKTYPE)
        {
            bool TranStatus = false;
            int status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pBenificiaryData = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSLTS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DEVICEID);
                        cmd.Params.Add("pRRN", RRN);
                        cmd.Params.Add("pCHECKTYPE", CHECKTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@RRN", RRN);
                        cmd.Params.Add("@CHECKTYPE", CHECKTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0)
                        TranStatus = true;
                    else
                        TranStatus = false;

                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                TranStatus = false;
            }
            return TranStatus;
        }

        public static bool VERIFYLASTInwardTransactionRRN(string DEVICEID, string RRN, int CHECKTYPE)
        {
            bool TranStatus = false;
            int status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter pBenificiaryData = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSINWARDTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DEVICEID);
                        cmd.Params.Add("pRRN", RRN);
                        cmd.Params.Add("pCHECKTYPE", CHECKTYPE);
                        pstatus = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pSTATUS" };
                        cmd.Params.Add("pSTATUS", pstatus);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@RRN", RRN);
                        cmd.Params.Add("@CHECKTYPE", CHECKTYPE);
                        pstatus = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                        cmd.Params.Add("@STATUS", pstatus);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(pstatus.Value.ToString());

                    if (status == 0)
                        TranStatus = true;
                    else
                        TranStatus = false;

                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                TranStatus = false;
            }
            return TranStatus;
        }

        public static string VERIFYLASTRRN(string DEVICEID, int CHECKTYPE)
        {
            bool TranStatus = false;
            int status = -1;
            string RNN = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter prrn = null;
                    DbParameter pBenificiaryData = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_GETRRN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DEVICEID);
                        cmd.Params.Add("pCHECKTYPE", CHECKTYPE);
                        prrn = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pRRN" };
                        cmd.Params.Add("pRRN", prrn);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@CHECKTYPE", CHECKTYPE);
                        prrn = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@RRN" };
                        cmd.Params.Add("@RRN", prrn);
                    }
                    Data.ExecuteNonQuery(cmd);
                    RNN = prrn.Value.ToString();

                    //if (status == 0)
                    //    TranStatus = true;
                    //else
                    //    TranStatus = false;

                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                TranStatus = false;
            }
            return RNN;
        }

        public static string VERIFYLASTInwardRRN(string DEVICEID, int CHECKTYPE)
        {
            bool TranStatus = false;
            int status = -1;
            string RNN = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter prrn = null;
                    DbParameter pBenificiaryData = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("GetLastInwardRRN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        //cmd.Params.Add("pDEVICEID", DEVICEID);
                        //cmd.Params.Add("pCHECKTYPE", CHECKTYPE);
                        prrn = new OracleParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "pRRN" };
                        cmd.Params.Add("pRRN", prrn);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        //cmd.Params.Add("@DEVICEID", DEVICEID);
                        //cmd.Params.Add("@CHECKTYPE", CHECKTYPE);
                        prrn = new SqlParameter() { DbType = DbType.String, Size = 1000, Direction = ParameterDirection.Output, ParameterName = "@RRN" };
                        cmd.Params.Add("@RRN", prrn);
                    }
                    Data.ExecuteNonQuery(cmd);
                    RNN = prrn.Value.ToString();

                    //if (status == 0)
                    //    TranStatus = true;
                    //else
                    //    TranStatus = false;

                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                TranStatus = false;
            }
            return RNN;
        }
        #endregion other transactions

        #region Cheque Deposit

        public static void ChequeDeposit(string DeviceID, string MobileNumber, string AccountNumber, string CustomerID, string UserID, string ChequeNumber, string FrontImage, string BackImage, string DepositAmount, string DepositAccount, string ChequeAccNumber, string ChequeDate, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter Statusparam = null;
                    DbParameter AmtAvlparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_DEPOSITCHEQUE");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pMOBILENUMBER", MobileNumber);
                        cmd.Params.Add("pCUSTOMERID", CustomerID);
                        cmd.Params.Add("pUSERID", UserID);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("pCHEQUENUMBER", ChequeNumber);
                        cmd.Params.Add("pCHEQUEAMOUNT", DepositAmount);
                        cmd.Params.Add("pFRONTIMAGE", FrontImage);
                        cmd.Params.Add("pBACKIMAGE", BackImage);
                        cmd.Params.Add("pDEPOSITACCOUNT", DepositAccount);
                        cmd.Params.Add("pCHEQUEACCOUNTNUMBER", ChequeAccNumber);
                        cmd.Params.Add("pCHEQUEDATE", ChequeDate);

                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "pstatus" };
                        cmd.Params.Add("pstatus", Statusparam);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@MOBILENUMBER", MobileNumber);
                        cmd.Params.Add("@CUSTOMERID", CustomerID);
                        cmd.Params.Add("@USERID", UserID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                        cmd.Params.Add("@CHEQUENUMBER", ChequeNumber);
                        cmd.Params.Add("@CHEQUEAMOUNT", DepositAmount);
                        cmd.Params.Add("@FRONTIMAGE", FrontImage);
                        cmd.Params.Add("@BACKIMAGE", BackImage);
                        cmd.Params.Add("@DEPOSITACCOUNT", DepositAccount);
                        cmd.Params.Add("@CHEQUEACCOUNTNUMBER", ChequeAccNumber);
                        cmd.Params.Add("@CHEQUEDATE", ChequeDate);
                        Statusparam = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@status" };
                        cmd.Params.Add("@status", Statusparam);
                    }
                    Data.ExecuteNonQuery(cmd);
                    status = int.Parse(Statusparam.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }


        public static DataTable CheckForUpdate()
        {
            DataTable DTCheckForUpdate = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_CHECKFORUPDATE");

                    DTCheckForUpdate = Data.GetDataTable(cmd);
                }
                return DTCheckForUpdate;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTCheckForUpdate = null;
                return DTCheckForUpdate;
            }
        }

        public static DataTable CheckForUpdate(string Version,string DeviceType, out int status)
        {
            DataTable DTCheckForUpdate = null;
            try
            {
               
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_CHECKFORUPDATE_VERSION");
                    cmd.Params.Add("@Version", Version);
                    cmd.Params.Add("@DeviceType", DeviceType);
                    DTCheckForUpdate = Data.GetDataTable(cmd);
                }
                if (DTCheckForUpdate.Rows.Count > 0)
                {
                    status =1;
                }
                else
                {
                    status = 0;
                }
                return DTCheckForUpdate;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTCheckForUpdate = null;
                status = -1;
                return DTCheckForUpdate;
            }
        }

        public static DataTable FxRate()
        {
            DataTable DTFxRate = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();

                    query.Params["getmode"] = "Fx_Rate";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTFxRate = Data.GetDataTable(query);
                }
                return DTFxRate;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTFxRate = null;
                return DTFxRate;
            }
        }

        public static DataTable ValidCheque(string ChequeAccountNumber, string ChequeNumber)
        {
            DataTable DTChequeDeposit = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = ChequeAccountNumber;
                    query.Params["pcustomerid"] = ChequeNumber;
                    query.Params["getmode"] = "CHEQUEDEPOSIT";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTChequeDeposit = Data.GetDataTable(query);
                }
                return DTChequeDeposit;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTChequeDeposit = null;
                return DTChequeDeposit;
            }
        }


        public static DataTable ValidChequePaymentStop(string ChequeAccountNumber, string ChequeNumber)
        {
            DataTable DTChequeDeposit = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = ChequeAccountNumber;
                    query.Params["pcustomerid"] = ChequeNumber;
                    query.Params["getmode"] = "CHEQUESTOPPAYMENT";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTChequeDeposit = Data.GetDataTable(query);
                }
                return DTChequeDeposit;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTChequeDeposit = null;
                return DTChequeDeposit;
            }
        }




        public static DataTable ValidChequeAmount(string ChequeAccountNumber, string ChequeAmount, string ExpireDate, out int status)
        {
            DataTable DTChequeDeposit = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = ChequeAccountNumber;
                    //query.Params["PChequeAmount"] = Convert.ToDecimal(ChequeAmount);
                    query.Params["getmode"] = "CHEQUEAMOUNT";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTChequeDeposit = Data.GetDataTable(query);
                    if (DTChequeDeposit != null && DTChequeDeposit.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTChequeDeposit;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTChequeDeposit = null;
                status = 14;
                return DTChequeDeposit;

            }
        }

        #endregion Cheque Deposit

        public static DataTable GETCUSTOMERDETAILS_ADDACC(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        public static DataTable GETCUSTOMERDETAILS_ASSETSLIABILITIES(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "ASSETSLIABILITIES";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        public static void ADDACCOUNT(string CustomerName, string CustomerID, string REGACCOUNTNUMBER, string DEVICEID, string MobileNumber, string AccountNumber, string AccountType, string CCY, string ACCSTATUS, string BRANCHCODE, out Int32 status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTIMPSACCOUNTS");
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
                        cmd.Params.Add("@DEVICEID", DEVICEID);
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
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@BRANCHCODE", BRANCHCODE);
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

        public static void OfflineRequest(string AccountNumber, string MobileNumber, string txntype, string FromDate, string Todate, string Mail, string Dzongkha, out int status)
        {
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
                    objCmd.Params.Add("@FROMDATE", FromDate);
                    objCmd.Params.Add("@TODATE", Todate);
                    objCmd.Params.Add("@APPROVEDBY", "");
                    objCmd.Params.Add("@Type", txntype);
                    objCmd.Params.Add("@EMAIL", Mail);
                    objCmd.Params.Add("@Dzongkha", Dzongkha);
                    objCmd.Params.Add("@REQTYPE", "INSERT");
                    pstatus = new SqlParameter() { DbType = DbType.AnsiString, Size = 8000, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void CardLessRequest(string ReferenceNumber, string Amount, string Mobile, string Account, string OTP, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTMPAYCARDLESS");
                    objCmd.Params.Add("@DEVICEID", "BNBLMPAY");
                    objCmd.Params.Add("@RRN", ReferenceNumber);
                    objCmd.Params.Add("@AMOUNT", Amount);
                    objCmd.Params.Add("@MOBILE", Mobile);
                    objCmd.Params.Add("@ACCOUNT", Account);
                    objCmd.Params.Add("@OTP", OTP);
                    pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void GreenPinOTPRequest(string ReferenceNumber, string CardExpire, string cardNumber, string Account, string OTP, out int status, string DeviceID)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_INSERTMPAYGREENPIN");
                    objCmd.Params.Add("@DEVICEID", DeviceID);
                    objCmd.Params.Add("@RRN", ReferenceNumber);
                    objCmd.Params.Add("@CardExpire", CardExpire);
                    objCmd.Params.Add("@cardNumber", cardNumber);
                    objCmd.Params.Add("@ACCOUNT", Account);
                    objCmd.Params.Add("@OTP", OTP);
                    pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static void FeedBackRequest(string DeviceID, string Name, string Mobile, string Account, string Email, string Product, string Comment, out int status)
        {
            status = -1;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_FEEDBACK");
                    objCmd.Params.Add("@DEVICEID", DeviceID);
                    objCmd.Params.Add("@NAME", Name);
                    objCmd.Params.Add("@MOBILE", Mobile);
                    objCmd.Params.Add("@ACCOUNT", Account);
                    objCmd.Params.Add("@EMAIL", Email);
                    objCmd.Params.Add("@PRODUCT", Product);
                    objCmd.Params.Add("@COMMENT", Comment);
                    objCmd.Params.Add("@TYPE", "FEEDBACK");
                    pstatus = new SqlParameter() { DbType = DbType.Int16, Direction = ParameterDirection.Output, ParameterName = "@STATUS" };
                    objCmd.Params.Add("@STATUS", pstatus);
                    Data.ExecuteNonQuery(objCmd);
                    status = int.Parse(pstatus.Value.ToString());
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                status = -1;
            }
        }

        public static DataTable GetProductDetails()
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_FEEDBACK");
                    objCmd.Params.Add("@TYPE", "PRODUCT");
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

        public static DataTable CalulatorValues(string CalcType, int Duration)
        {
            DataTable DtCalcValues = new DataTable();
            DtCalcValues = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETCALCVALUES");
                    objCmd.Params.Add("@GetType", CalcType);
                    objCmd.Params.Add("@Duration", Duration);
                    DtCalcValues = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DtCalcValues;

            }
            catch (Exception ex)
            {
                DtCalcValues = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DtCalcValues;
            }
        }

        //public static DataSet ValidateCustomerForRDTD(string AccountNumber, int Duration, string Type, string ProducType, string CustomerID)
        public static DataSet ValidateCustomerForRDTD(string AccountNumber, int Duration, string Type, string ProducType, string CustomerID, string RateCode)


        {
            if (Type == "TD")
                Type = "Fixed";
            else
                Type = "Recurring";
            DataSet DtValues = new DataSet();
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTTYPE = new DataTable();
            DTTYPE = null;
            DataTable DTCUSTDETAILS = new DataTable();
            DTCUSTDETAILS = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("VALIDATE_TDRD_CREATION");
                    objCmd.Params.Add("@ACCCOUNTNUMBER", AccountNumber);
                    objCmd.Params.Add("@Duration", Duration);
                    objCmd.Params.Add("@GetType", Type);
                    DTCUSTDETAILS = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();

                }
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pCustomerID"] = CustomerID;
                    query.Params["getmode"] = "GETACCOUNTTYPE";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTTYPE = Data.GetDataTable(query);

                    if (DTCUSTDETAILS != null && DTCUSTDETAILS.Rows.Count > 0)
                    {
                        DTCUSTDETAILS.Rows[0][0] = DTTYPE.Rows[0][3].ToString();
                    }
                }
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pcustomertype"] = DTTYPE.Rows[0]["CUSTOMER_TYPE"].ToString();
                    query.Params["pproducttype"] = ProducType;
                    query.Params["pduration"] = Duration;
                    query.Params["pratecode"] = RateCode;
                    query.Params["getmode"] = "RDTDINTERESTRATE";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                    DTCustomerData.TableName = "Rate";
                }
                DataTable DtTable1 = DTTYPE.Copy();
                DataTable DtTable2 = DTCustomerData.Copy();
                DtValues.Tables.Add(DtTable1);
                DtValues.Tables.Add(DtTable2);
                return DtValues;

            }
            catch (Exception ex)
            {
                DtValues = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DtValues;
            }
        }

        public static void INSERT_RECENTTRANSACTION(string PRIMARYNUMBER, string NAME, string AMOUNT, string ACCOUNTNUMBER, string MOBILENUMBER, string DEVICEID, string REGTYPE)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERT_SELECT_UPDATERECENTTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "INSERT");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "INSERT");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                    }
                    Data.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable SELECT_RECENTTRANSACTION(string PRIMARYNUMBER, string NAME, string AMOUNT, string ACCOUNTNUMBER, string MOBILENUMBER, string DEVICEID, string REGTYPE)
        {
            DataTable _DTrecentdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERT_SELECT_UPDATERECENTTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "SELECT");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "SELECT");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                    }
                    _DTrecentdata = Data.GetDataTable(cmd);
                    return _DTrecentdata;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                _DTrecentdata = null;
                return _DTrecentdata;
            }
        }

        public static DataTable SELECT_RECENTTRANSACTIONREMOVE(string PRIMARYNUMBER, string NAME, string AMOUNT, string ACCOUNTNUMBER, string MOBILENUMBER, string DEVICEID, string REGTYPE, int ID)
        {
            DataTable _DTrecentdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERT_SELECT_UPDATERECENTTXN");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "UPDATE");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        cmd.Params.Add("@ID", ID);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@PRIMARYNUMBER", PRIMARYNUMBER);
                        cmd.Params.Add("@NAME", NAME);
                        cmd.Params.Add("@AMOUNT", AMOUNT);
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@TYPE", "UPDATE");
                        cmd.Params.Add("@REGTYPE", REGTYPE);
                        cmd.Params.Add("@ID", ID);
                    }
                    _DTrecentdata = Data.GetDataTable(cmd);
                    return _DTrecentdata;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                _DTrecentdata = null;
                return _DTrecentdata;
            }
        }

        public static decimal GET_EXCHANGERATE(out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select A.CCY1 CURRENCY,CAST(CAST(A.BUY_RATE AS DECIMAL(10,2)) AS VARCHAR2(10)) BUY,CAST(CAST(A.SALE_RATE AS DECIMAL(10,2)) AS VARCHAR2(10)) SELL,mid_rate  from cytm_rates A where A.CCY1='USD' and A.BRANCH_CODE='100' AND A.RATE_TYPE='TT' AND A.CCY1<>'INR' AND A.RATE_DATE =(SELECT max(rate_date) FROM cytm_rates WHERE BRANCH_CODE='100')");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        string Exc = DTCustomerData.Rows[0]["mid_rate"].ToString();
                        ExchangeRate = Convert.ToDecimal(Exc);
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return ExchangeRate;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                return ExchangeRate;
            }
        }

        public static DataTable GET_LOANDETAILS(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select * from cltb_clterm_schedules where account_number='" + AccountNumber + "'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GET_LOANDETAILSLIABILITIES(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select * from cltb_clterm_schedules where account_number in(" + AccountNumber + "");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GET_LOANOVERDUEDETAILS(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select account_number,int_odue,prin_odue,tot_odue from cltb_clterm_summary where account_number='" + AccountNumber + "'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GetMinMaxData(string PRIMARYNUMBER, string NAME, string AMOUNT, string ACCOUNTNUMBER, string MOBILENUMBER, string DEVICEID, string REGTYPE)
        {
            DataTable _DTrecentdata = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("GETMINMAXDATA");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@TYPE", "BT");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@TYPE", "BT");
                    }
                    _DTrecentdata = Data.GetDataTable(cmd);
                    return _DTrecentdata;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                _DTrecentdata = null;
                return _DTrecentdata;
            }
        }

        public static DataTable GetProductDetails(string Desc)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETPRODUCT");
                    objCmd.Params.Add("@DESC", Desc);
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

        public static DataTable GetProductCode(string RecipientName)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETPRODUCTCODE");
                    objCmd.Params.Add("@DONORNAME", RecipientName);
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

        public static DataTable GetShowProductCode(string ShowID)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETSHOWPRODUCTCODE");
                    objCmd.Params.Add("@FLAG", "SELECT_PRODUCTID");
                    objCmd.Params.Add("@ShowID", ShowID);
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

        public static DataTable BTPLANS()
        {
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTACCCLASS = new DataTable();
            DTACCCLASS = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData_New = null;
            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GETBROADBANDPREPAIDPLANS");
                    DTCustomerData_New = Data1.GetDataTable(Cmd);
                }
                return DTCustomerData_New;

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData_New = null;
            }
            return DTCustomerData_New;
        }

        public static DataTable ReqValidation(string ReferenceNumber, out int status)
        {

            DataTable DTTokenDetails = new DataTable();
            DTTokenDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("proc_checkReqMoneyvalidRequest");
                    objCmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                    DTTokenDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTokenDetails != null && DTTokenDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTokenDetails;
            }
            catch (Exception ex)
            {
                DTTokenDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTokenDetails;
            }
        }

        public static DataTable GetTokenDetails(string MobileNumber, out int status)
        {
            DataTable DTTokenDetails = new DataTable();
            DTTokenDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETTOKENDETAILS");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    DTTokenDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTokenDetails != null && DTTokenDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTokenDetails;
            }
            catch (Exception ex)
            {
                DTTokenDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTokenDetails;
            }
        }

        public static DataTable GetRequestMoneyDashBoardDetails(string MobileNumber, out int status)
        {
            DataTable DTTokenDetails = new DataTable();
            DTTokenDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETREQUESTMONEYDETAILS");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    DTTokenDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTokenDetails != null && DTTokenDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTokenDetails;


            }
            catch (Exception ex)
            {
                DTTokenDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTokenDetails;
            }
        }

        public static DataTable GetRequestMoneyDashBoardHistoryDetails(string MobileNumber, out int status)
        {
            DataTable DTTokenDetails = new DataTable();
            DTTokenDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETREQUESTMONEYHISTORYDETAILS");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    DTTokenDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTokenDetails != null && DTTokenDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTokenDetails;


            }
            catch (Exception ex)
            {
                DTTokenDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTokenDetails;
            }
        }

        public static DataTable GetRequestBellMoneyDashBoardDetails(string MobileNumber, out int status)
        {
            DataTable DTTokenDetails = new DataTable();
            DTTokenDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETREQUESTMONEYDETAILS");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    DTTokenDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTokenDetails != null && DTTokenDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTokenDetails;


            }
            catch (Exception ex)
            {
                DTTokenDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTokenDetails;
            }
        }

        public static DataTable GetRequestMoneyDetails(string MobileNumber, string RefernceNumber, out int status)
        {
            DataTable DTRequestDetails = new DataTable();
            DTRequestDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETREQMONEYEXPDETAILS");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    objCmd.Params.Add("@RefernceNumber", RefernceNumber);

                    DTRequestDetails = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTRequestDetails != null && DTRequestDetails.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTRequestDetails;


            }
            catch (Exception ex)
            {
                DTRequestDetails = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTRequestDetails;
            }
        }

        public static DataTable GetMpayDashBoardDetails(string AccountNumber, out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;


            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_IMPSTRANSACTIONS_DASHBOARD");
                    objCmd.Params.Add("@Accounts", AccountNumber);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;




            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static DataTable GetAccountDetails(string BeneficaryAcc, string MobileNumber, out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;


            try
            {


                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETIMPSACCOUNT");
                    objCmd.Params.Add("@BeneficaryAcc", BeneficaryAcc);
                    objCmd.Params.Add("@BeneficaryMob", MobileNumber);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;


            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static DataTable GetTransactionNotificationHistory(string DeviceId)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_GetTransactionNotificationHistory");
                    objCmd.Params.Add("@DeviceId", DeviceId);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTTRANDT;
            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTTRANDT;
            }
        }

        public static DataTable GetotherNotificationHistory(string DeviceId)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_GetotherNotificationHistory");
                    objCmd.Params.Add("@DeviceId", DeviceId);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTTRANDT;
            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTTRANDT;
            }
        }


        public static DataTable GETCUSTOMERDETAILS_TRANSDETAILS(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["pAccountNumber"] = AccountNumber;
                    query.Params["getmode"] = "GETCUSTOMERDETAILS_ACC";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
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

        // weekly data
        public static DataTable GETWeeklyTransDT(string AccountNumber, out int status, string AccountType)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {



                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = AccountNumber;
                    query.Params["getmode"] = "DASHBOARDACCDTL";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTTRANDT = Data.GetDataTable(query);
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;



            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        //ASSETLIABLITY
        public static DataTable GETASSETLIABLITY(string AccountNumber, out int status, string AccountType)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {



                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig(CONFIGURATIONCONFIGDATA.SCHEMA + ".BNBL_M_PAY");
                    query.Params.Clear();
                    query.Params["paccountnumber"] = AccountNumber;
                    query.Params["getmode"] = "ASSETSLIABILITIES";
                    object obj = null;
                    query.Params["ptask"] = new System.Data.OracleClient.OracleParameter("ptask", System.Data.OracleClient.OracleType.Cursor) { Direction = ParameterDirection.Output, Value = obj };
                    DTTRANDT = Data.GetDataTable(query);
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;



            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        //Local data
        public static DataTable GETASSETLIABLITY_Loc(string AccountNumber, out int status, string AccountType)
        {
            DataTable DTCustomerData_New = new DataTable();

            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("ASSET");
                    DTCustomerData_New = Data1.GetDataTable(Cmd);
                }
                status = 0;
                return DTCustomerData_New;


            }
            catch (Exception ex)
            {
                DTCustomerData_New = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTCustomerData_New;
            }
        }

        public static DataTable GETWeeklyTransDTLocal(string AccountNumber, out int status, string AccountType)
        {
            DataTable DTCustomerData_New = new DataTable();

            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GaraphData");
                    DTCustomerData_New = Data1.GetDataTable(Cmd);
                }
                status = 0;
                return DTCustomerData_New;


            }
            catch (Exception ex)
            {
                DTCustomerData_New = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTCustomerData_New;
            }
        }


        public static DataTable GetNQRCPrimaryAccount(string identifier)
        {
            DataTable DTNQRCPrimaryAcc = new DataTable();
            DTNQRCPrimaryAcc = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_SelectNQRCIdentifier");
                    objCmd.Params.Add("@identifierNumber", ConnectionStringEncryptDecrypt.EncryptString(identifier));
                    DTNQRCPrimaryAcc = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTNQRCPrimaryAcc;

            }
            catch (Exception ex)
            {
                DTNQRCPrimaryAcc = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTNQRCPrimaryAcc;
            }
        }

        public static DataTable GET_INWARDNQRCACCOUNT(string ProductCode, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select cod_offset_brn,COD_OFFSET_acc from IFTM_ARC_MAINT WHERE COD_PROD_ACC_CLS='" + ProductCode + "' and record_stat='O'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GetPANDetails(string MerchantIdentifier, string branchcode, string flag, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;

            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("Proc_SelectMpayNQRCAccountDetails");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("@BranchCode", branchcode);
                        Cmd.Params.Add("@MerchantIdentifier", MerchantIdentifier);
                        Cmd.Params.Add("@flag", flag);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@BranchCode", branchcode);
                        Cmd.Params.Add("@MerchantIdentifier", MerchantIdentifier);
                        Cmd.Params.Add("@flag", flag);

                    }
                    DTCustomerData = Data1.GetDataTable(Cmd);

                }


                if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                {
                    status = 0;

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

        public static bool INSERTSelfNQRCDetails(string DeviceID, string RefrenceNumber, string MerchantIdentifier,
         string AccountHolderName, string SequecnceData, string typeofUser, string Category, string Currency, string country,
         string Accountnumber, string MobileNumber, string city, string BranchCode)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERTMPAYNQRCACCOUNTDETAILS");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {

                        cmd.Params.Add("@AccountNumber", Accountnumber);
                        cmd.Params.Add("@Merchantidentifier", ConnectionStringEncryptDecrypt.EncryptString(MerchantIdentifier));
                        cmd.Params.Add("@AccountHolderName", AccountHolderName);
                        cmd.Params.Add("@SequecnceData", ConnectionStringEncryptDecrypt.EncryptString(SequecnceData));
                        cmd.Params.Add("@TypeOfUser", typeofUser);
                        cmd.Params.Add("@Category", Category);
                        cmd.Params.Add("@Currency", Currency);
                        cmd.Params.Add("@Country", country);
                        cmd.Params.Add("@City", city);
                        cmd.Params.Add("@CreatedBy", Accountnumber);
                        cmd.Params.Add("@mobileNumber", MobileNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", RefrenceNumber);
                        cmd.Params.Add("@BranchCode", BranchCode);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccountNumber", Accountnumber);
                        cmd.Params.Add("@Merchantidentifier", ConnectionStringEncryptDecrypt.EncryptString(MerchantIdentifier));
                        cmd.Params.Add("@AccountHolderName", AccountHolderName);
                        cmd.Params.Add("@SequecnceData", ConnectionStringEncryptDecrypt.EncryptString(SequecnceData));
                        cmd.Params.Add("@TypeOfUser", typeofUser);
                        cmd.Params.Add("@Category", Category);
                        cmd.Params.Add("@Currency", Currency);
                        cmd.Params.Add("@Country", country);
                        cmd.Params.Add("@City", city);
                        cmd.Params.Add("@CreatedBy", Accountnumber);
                        cmd.Params.Add("@mobileNumber", MobileNumber);
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", RefrenceNumber);
                        cmd.Params.Add("@BranchCode", BranchCode);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static DataTable GET_NQRCBranchCity(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
               (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("GET_NQRCBranchCity");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        DTCustomerData = Data.GetDataTable(Cmd);
                        if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                        {
                            status = 0;
                        }
                        else
                            status = 14;
                    }

                    return DTCustomerData;
                }
            }

            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GET_NQRCBranch(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select branch_code,cust_ac_no from sttm_cust_account where cust_ac_no= '" + AccountNumber + "'" + " and record_stat='O'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GET_NQRCExistWithDetails(string AccountNumber, string City, string Name, string category, out int status, string MobileNumber)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;

            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("Proc_CheckNQRCExist");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        Cmd.Params.Add("@City", City);
                        Cmd.Params.Add("@Name", Name);
                        Cmd.Params.Add("@category", category);
                        Cmd.Params.Add("@MobileNumber", MobileNumber);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);
                        Cmd.Params.Add("@City", City);
                        Cmd.Params.Add("@Name", Name);
                        Cmd.Params.Add("@category", category);
                        Cmd.Params.Add("@MobileNumber", MobileNumber);
                    }
                    DTCustomerData = Data1.GetDataTable(Cmd);

                }


                if (DTCustomerData.Rows.Count == 0)
                {
                    status = 0;

                }
                else
                {
                    status = 11;
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

        public static DataTable GET_NQRCIdentifierExistWithDetails(string AccountNumber, string TypeofQR, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;

            try
            {

                using (DbNetData Data1 = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data1.Open();
                    DbParameter pstatus1 = null;
                    QueryCommandConfig Cmd = new QueryCommandConfig("Proc_CheckNQRCIdentifierExist");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);

                        Cmd.Params.Add("@TypeofQR", TypeofQR);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        Cmd.Params.Add("@AccountNumber", AccountNumber);

                        Cmd.Params.Add("@TypeofQR", TypeofQR);
                    }
                    DTCustomerData = Data1.GetDataTable(Cmd);

                }


                if (DTCustomerData.Rows.Count > 0)
                {
                    status = 0;

                }
                else
                {
                    status = 11;
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

        public static DataTable GetBankCodeDetails(string ID, string flag)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETBANKCODE");
                    objCmd.Params.Add("@FLAG", flag);
                    objCmd.Params.Add("@ID", ID);
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

        //added by krn for fetching AcquireCode
        public static DataTable GetAcquireCodeDetails(string ID, string flag)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_AcquireCodeCODE");
                    objCmd.Params.Add("@FLAG", flag);
                    objCmd.Params.Add("@ID", ID);
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



        public static DataTable GetACQBankCodeDetails(string ACQBankCode)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_getAcqbankDetails");

                    objCmd.Params.Add("@ID", ACQBankCode);
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

        public static DataTable GetACQNQRCBankCodeDetails(string ACQBankName)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_getAcqNQRCbankDetails");

                    objCmd.Params.Add("@ID", ACQBankName);
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


        public static DataTable GetMerchantMobileDetails(string ID)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_GetMerchantMobileNumber");

                    objCmd.Params.Add("@MerchantPAN", ID);
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

        public static DataTable GetMobileDetails(string AccountNumber, out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;

            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETIMPSMOBILENUMBER");
                    objCmd.Params.Add("@AccountNumber", AccountNumber);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;


            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static DataTable GET_FORGOTMPINDTL(string CIF, string RegMobileNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select a.customer_no,b.mobile_number,a.SSN from sttm_customer a,sttm_cust_personal b where a.customer_no=b.customer_no and a.customer_no='" + CIF + "' and mobile_number= '" + RegMobileNumber + "'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable MobileAlreadyInDB(string MobileNumber, string AccountNumber, string DeviceID, out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETMOBILENUMBEREXIST");
                    objCmd.Params.Add("@MobileNumber", MobileNumber);
                    objCmd.Params.Add("@AccountNumber", AccountNumber);
                    objCmd.Params.Add("@DeviceID", DeviceID);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;
            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static DataTable GET_STOPCHEQUEDETAILS(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("SELECT account,check_no FROM CATM_CHECK_DETAILS WHERE ACCOUNT='" + AccountNumber + "' and record_stat = 'O'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GET_CHEQUEBOOKBLOCK(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("SELECT account,first_check_no,check_leaves,issue_date FROM CATM_CHECK_BOOK WHERE ACCOUNT='" + AccountNumber + "' and record_stat = 'O'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static bool INSERT_CHEQUEBOOKBLOCK(string ACCOUNTNUMBER, string NAME, string CHEQUESTART, string CHEQUEEND, string MOBILENUMBER, string DEVICEID, string REFNO, string TYPE, string CHEQUEISSUEDATE)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_INSERT_SELECT_UPDATE_CHEQUEBOOKBLOCK");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@NAME", NAME);

                        cmd.Params.Add("@CHEQUESTART", CHEQUESTART);
                        cmd.Params.Add("@CHEQUEEND", CHEQUEEND);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@REFNO", REFNO);
                        cmd.Params.Add("@TYPE", TYPE);
                        cmd.Params.Add("@CHEQUEISSUEDATE", CHEQUEISSUEDATE);

                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@ACCOUNTNUMBER", ACCOUNTNUMBER);
                        cmd.Params.Add("@NAME", NAME);

                        cmd.Params.Add("@CHEQUESTART", CHEQUESTART);
                        cmd.Params.Add("@CHEQUEEND", CHEQUEEND);
                        cmd.Params.Add("@MOBILENUMBER", MOBILENUMBER);
                        cmd.Params.Add("@DEVICEID", DEVICEID);
                        cmd.Params.Add("@REFNO", REFNO);
                        cmd.Params.Add("@TYPE", TYPE);
                        cmd.Params.Add("@CHEQUEISSUEDATE", CHEQUEISSUEDATE);

                    }
                    Data.ExecuteNonQuery(cmd);
                    return true;
                }

            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static DataTable GET_RELESECHEQUEDETAILS(string AccountNumber, out int status)
        {
            status = -1;
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            decimal ExchangeRate = 0;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionStringOracle, DataProvider.OracleClient))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig query = new QueryCommandConfig("select account,trn_ref_no, stop_payment_no,start_check_no, end_check_no,effective_Date,expiry_date from catm_stop_payments WHERE ACCOUNT='" + AccountNumber + "' and record_stat = 'O'");
                    query.Params.Clear();
                    DTCustomerData = Data.GetDataTable(query);
                    if (DTCustomerData != null && DTCustomerData.Rows.Count > 0)
                    {
                        status = 0;
                    }
                    else
                        status = 14;
                }
                return DTCustomerData;

            }
            catch (Exception ex)
            {
                status = -1;
                DalcLogger.WriteErrorLog(null, ex);
                DTCustomerData = null;
                return DTCustomerData;
            }
        }

        public static DataTable GetDashboardDetails(out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;

            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_getDashBoardMenu");

                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;


            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static DataTable GetProcessingCode()
        {

            DataTable DtProcessingCode = new DataTable();
            DtProcessingCode = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("GetProcessingCode");

                    DtProcessingCode = Data.GetDataTable(cmd);
                    Data.Close();
                }
                return DtProcessingCode;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return DtProcessingCode = null;
            }

        }

        public static DataTable ValidateAccount(string DeviceID, string AccountNumber, string RefrenceNumber)
        {
            DataTable _dt = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_VALIDATEACCOUNT");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("pDEVICEID", DeviceID);
                        cmd.Params.Add("pACCOUNTNUMBER", AccountNumber);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DEVICEID", DeviceID);
                        cmd.Params.Add("@ACCOUNTNUMBER", AccountNumber);
                    }
                    _dt = Data.GetDataTable(cmd);
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex); _dt = null;
            }
            return _dt;
        }

        public static bool InsertScheduledPayment(string DeviceID, string MobileNumber, string ReferenceNumber, string TransactionType, string Amount,
            string RemitterAccountNumber, string BeneficiaryAccountNumber, string BankCode, string BankName, string Naration, int Frequency,
            string ScheduledOn, string StopSchedulerOn, string NextScheduleOn, int IsRetry)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("InsertScheduledPayment");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@Amount", Amount);
                        cmd.Params.Add("@FromAccountNumber", RemitterAccountNumber);
                        cmd.Params.Add("@ToAccountNumber", BeneficiaryAccountNumber);
                        cmd.Params.Add("@BankCode", BankCode);
                        cmd.Params.Add("@BankName", BankName);
                        cmd.Params.Add("@Naration", Naration);
                        cmd.Params.Add("@Frequency", Frequency);
                        cmd.Params.Add("@ScheduledOn", ScheduledOn);
                        cmd.Params.Add("@StopSchedulerOn", StopSchedulerOn);
                        cmd.Params.Add("@IsRetry", IsRetry);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@DeviceID", DeviceID);
                        cmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                        cmd.Params.Add("@MobileNumber", MobileNumber);
                        cmd.Params.Add("@TransactionType", TransactionType);
                        cmd.Params.Add("@Amount", Amount);
                        cmd.Params.Add("@FromAccountNumber", RemitterAccountNumber);
                        cmd.Params.Add("@ToAccountNumber", BeneficiaryAccountNumber);
                        cmd.Params.Add("@BankCode", BankCode);
                        cmd.Params.Add("@BankName", BankName);
                        cmd.Params.Add("@Naration", Naration);
                        cmd.Params.Add("@Frequency", Frequency);
                        cmd.Params.Add("@ScheduledOn", ScheduledOn);
                        cmd.Params.Add("@StopSchedulerOn", StopSchedulerOn);
                        cmd.Params.Add("@NextScheduledOn", NextScheduleOn);
                        cmd.Params.Add("@IsRetry", IsRetry);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return false;
            }
        }

        public static void GetScheduledPaymentsList(string CustomerID, string RemitterAccountNumber, string MobileNumber, DateTime Fromdate, DateTime Todate, int flag, ref string ScheduledPaymentsList)
        {
            ScheduledPaymentsList = string.Empty;
            DataTable _dtPaymentsList = new DataTable();
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("SelectScheduledPaymentsList");
                    cmd.Params.Add("@CustomerID", CustomerID);
                    cmd.Params.Add("@FromAccountNumber", RemitterAccountNumber);
                    cmd.Params.Add("@MobileNumber", MobileNumber);
                    cmd.Params.Add("@FromDate", Fromdate);
                    cmd.Params.Add("@Todate", Todate);
                    cmd.Params.Add("flag", flag);

                    _dtPaymentsList = Data.GetDataTable(cmd);

                    if (_dtPaymentsList.Rows.Count > 0)
                    {
                        ScheduledPaymentsList = Convert.ToString(_dtPaymentsList.Rows[0][0]);
                    }
                    Data.Close();
                }
            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); ScheduledPaymentsList = string.Empty; }
        }

        public static DataTable GetSchedulePayment()
        {


            DataTable _dtPaymentsList = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("ScheduledPaymentsList");

                    _dtPaymentsList = Data.GetDataTable(cmd);

                    Data.Close();
                }
                return _dtPaymentsList;
            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); return _dtPaymentsList; }

        }

        #region TashiApiToken

        public static DataTable GetTashiToken()
        {
            DataTable _dtApiToken = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("GetToken");

                    _dtApiToken = Data.GetDataTable(cmd);

                    Data.Close();
                }
                return _dtApiToken;
            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); return _dtApiToken; }

        }

        public static void InsertUpdateTcellTokens(string AccessToken, string RefreshToken)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("InsertUpdateTokens");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return;
            }
        }

        #endregion TashiApiToken

        #region FirebaseToken

        public static void InsertUpdateFbTokens(string AccessToken, string RefreshToken, string DeviceType)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("InsertUpdateFirebaseTokens");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                        cmd.Params.Add("@DeviceType", DeviceType);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return;
            }
        }

        public static DataTable GetfbToken( string DeviceType)
        {
            DataTable _dtApiToken = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("GetFirebaseToken");
                    cmd.Params.Add("@DeviceType", DeviceType);
                    _dtApiToken = Data.GetDataTable(cmd);
                    Data.Close();
                }
                return _dtApiToken;
            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); return _dtApiToken; }

        }



        #endregion FirebaseToken

        public static void InsertUpdatesoundboxTokens(string AccessToken, string RefreshToken)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("InsertUpdatesoundboxTokens");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@AccessToken", AccessToken);
                        cmd.Params.Add("@RefreshToken", RefreshToken);
                    }
                    Data.ExecuteNonQuery(cmd);
                    return;
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                return;
            }
        }

        public static DataTable GetsoundboxToken()
        {
            DataTable _dtApiToken = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig cmd = new QueryCommandConfig("GetsoundboxToken");

                    _dtApiToken = Data.GetDataTable(cmd);

                    Data.Close();
                }
                return _dtApiToken;
            }
            catch (Exception ex)
            { DalcLogger.WriteErrorLog(null, ex); return _dtApiToken; }

        }



        public static DataTable BHIMQRInsertUpdate(string MerchantName, string MerchantCode, string MerchantAccountNumber, string MerchantMobileNumber, string MerchantBankCode,
                                                   string MerchantBankName, string MerchantCID, string MerchantCategory, string MerchantBranch, string ReferenceNumber, string flag, out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;

            try
            {

                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_InsertBHIMQR");
                    objCmd.Params.Add("@MerchantName", MerchantName);
                    objCmd.Params.Add("@MerchantCode", MerchantCode);
                    objCmd.Params.Add("@MerchantAccountNumber", MerchantAccountNumber);
                    objCmd.Params.Add("@MerchantMobileNumber", MerchantMobileNumber);
                    objCmd.Params.Add("@MerchantBankCode", MerchantBankCode);
                    objCmd.Params.Add("@MerchantCID", MerchantCID);
                    objCmd.Params.Add("@MerchantCategory", MerchantCategory);
                    objCmd.Params.Add("@MerchantBranch", MerchantBranch);
                    objCmd.Params.Add("@ReferenceNumber", ReferenceNumber);
                    objCmd.Params.Add("@Flag", flag);
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0 && DTTRANDT.Rows[0][0].ToString() == "1")
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;


            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }


        public static DataTable BHIMQRSeries()
        {
            DataTable DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                       (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_BHIMQRSERIES");

                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }

                return DTTRANDT;
            }


            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);

                return DTTRANDT;
            }
        }

        public static DataTable GetRRNCheck(string RRN, string DeviceID, string TransType, string ResponseCode, string flag)
        {
            DataTable DTN = new DataTable();
            DTN = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("PROC_GETRRN");
                    objCmd.Params.Add("@RefferenceNumber", RRN);
                    objCmd.Params.Add("@DeviceID", DeviceID);
                    objCmd.Params.Add("@TransType", TransType);
                    objCmd.Params.Add("@ResponseCode", ResponseCode);
                    objCmd.Params.Add("@Flag", flag);
                    DTN = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                return DTN;
            }
            catch (Exception ex)
            {
                DTN = null;
                DalcLogger.WriteErrorLog(null, ex);
                return DTN;
            }
        }


        public static DataTable TermConditionDetails()
        {
            DataTable DTContact = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("Proc_TermConditionDetails");

                    DTContact = Data.GetDataTable(cmd);
                }
                return DTContact;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTContact = null;
                return DTContact;
            }
        }

        public static DataTable ContactDetails()
        {
            DataTable DTContact = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_GetContactDetails");

                    DTContact = Data.GetDataTable(cmd);
                }
                return DTContact;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTContact = null;
                return DTContact;
            }
        }


        public static DataTable AboutUSDetails()
        {
            DataTable DTContact = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    DbParameter Statusparam = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("PROC_GetAboutUsDetails");

                    DTContact = Data.GetDataTable(cmd);
                }
                return DTContact;
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
                DTContact = null;
                return DTContact;
            }
        }

        public static DataTable GetSequeanceNumber(out int status)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("Getsequance");
                    objCmd.Params.Add("@Flag", "1");
                    DTTRANDT = Data.GetDataTable(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
                if (DTTRANDT != null && DTTRANDT.Rows.Count > 0)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                return DTTRANDT;
            }
            catch (Exception ex)
            {
                DTTRANDT = null;
                DalcLogger.WriteErrorLog(null, ex);
                status = 1;
                return DTTRANDT;
            }
        }

        public static void NextSequanceNumberUpdate(int SequanceUpdateNo)
        {
            DataTable DTTRANDT = new DataTable();
            DTTRANDT = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    QueryCommandConfig objCmd = new QueryCommandConfig("Getsequance");
                    objCmd.Params.Add("@SequanceNo", SequanceUpdateNo);
                    objCmd.Params.Add("@Flag", "2");
                    Data.ExecuteNonQuery(objCmd);
                    Data.Close();
                    Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static void InsertNotification(string DeviceID, string TokenID, string NotificationMsg, string CreatedBy)
        {
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig cmd = new QueryCommandConfig("proc_NotificationHistory");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.OracleClient)
                    {
                        cmd.Params.Add("@deviceId", DeviceID);
                        cmd.Params.Add("@TokenID", TokenID);
                        cmd.Params.Add("@NotificationMsg", NotificationMsg);
                        cmd.Params.Add("@CreatedBy", CreatedBy);
                        cmd.Params.Add("@NotificationType", "Transaction");
                    }
                    else if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
                        cmd.Params.Add("@deviceId", DeviceID);
                        cmd.Params.Add("@TokenID", TokenID);
                        cmd.Params.Add("@NotificationMsg", NotificationMsg);
                        cmd.Params.Add("@CreatedBy", CreatedBy);
                        cmd.Params.Add("@NotificationType", "Transaction");
                    }
                    Data.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);
            }
        }

        public static DataTable GetSoundBoxDetails(string ID)
        {
            DataTable DTBankDetails = new DataTable();
            DTBankDetails = null;
            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();

                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_GetMerchartDeviceDtl");

                    objCmd.Params.Add("@MerchantPAN",ID);
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

        public static void InsertQSAPIDtl(string RRN, string MerchantAccountNumber, string MerchantPAN, string SerialNo, string DeviceVendor, string APIRequest, string APIResponse, string Responsecode, string CreatedBy, int flag)
        {

            try
            {
                using (DbNetData Data = new DbNetData(CONFIGURATIONCONFIGDATA.ConnectionString,
                    (DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider))))
                {
                    Data.Open();
                    DbParameter pstatus = null;
                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_InsertUpdateSelectQSApiDetails");
                    objCmd.Params.Add("@RRN", RRN);
                    objCmd.Params.Add("@MerchantAccountNumber", MerchantAccountNumber);
                    objCmd.Params.Add("@MerchantPAN", MerchantPAN);
                    objCmd.Params.Add("@SerialNo", SerialNo);
                    objCmd.Params.Add("@DeviceVendor", DeviceVendor);
                    objCmd.Params.Add("@APIRequest", APIRequest);
                    objCmd.Params.Add("@APIResponse", APIResponse);
                    objCmd.Params.Add("@Responsecode", Responsecode);
                    objCmd.Params.Add("@CreatedBy", CreatedBy);
                    objCmd.Params.Add("@Flag", flag);
                    Data.ExecuteNonQuery(objCmd);
                    Data.Close();
                    Data.Dispose();


                }
            }
            catch (Exception ex)
            {
                DalcLogger.WriteErrorLog(null, ex);

            }
        }

        public static DataTable SelectImages()
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
                    QueryCommandConfig objCmd = new QueryCommandConfig("Proc_GetImages");
                    if ((DataProvider)Enum.Parse(typeof(DataProvider), (CONFIGURATIONCONFIGDATA.Provider)) == DataProvider.SqlClient)
                    {
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

    }
}
