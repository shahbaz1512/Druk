using DALC;
using HSMCommunicationChanel;
using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.DALC.ConsumerTransactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace IMPSTransactionRouter.Models
{
    public class ProcessPortalRequest
    {
        CommanDetails _CommanDetails = new CommanDetails();
        SSM _SSM = new SSM();
        SMSJson _SMSJson = new SMSJson();
        Authentication _Authentication = new Authentication();
        ProcessMessage _ProcessMessage = new ProcessMessage();
        ProcessHost _ProcessHost = new ProcessHost();
        public void Get_RegisteredUsers(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {

                DtUserDetails = MobilePortalProcess.GET_REGISTEREDUSERS(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, (int)enumRequestType.GetRegisteredfUser, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, out status);
                if (status == 0)
                {
                    /*
                    System.Data.DataColumn C_PAN = new System.Data.DataColumn("PAN", typeof(System.String));
                    System.Data.DataColumn MASK_PAN = new System.Data.DataColumn("MASKPAN", typeof(System.String));

                    C_PAN.DefaultValue = "";
                    MASK_PAN.DefaultValue = "";
                    DtUserDetails.Columns.Add(C_PAN);
                    DtUserDetails.Columns.Add(MASK_PAN);
                    foreach (DataRow row in DtUserDetails.Rows)
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DtUserDetails.Rows.Count);

                        _CommanDetails.SystemLogger.WriteTransLog(this, "ROWS:   " + row[18].ToString());

                        if (row[18].ToString() != "")
                        {
                            string _CardNumber = string.Empty;
                            _CardNumber = ConnectionStringEncryptDecrypt.DecryptString(row[18].ToString());
                            row["PAN"] = _CardNumber;
                            row["MASKPAN"] = _CardNumber.Substring(0, 6) + new string('X', _CardNumber.Length - 10) + _CardNumber.Substring(_CardNumber.Length - 4); ;
                        }
                    }*/
                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void Get_RegisteredUsers_listCustomer(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {

                DtUserDetails = MobilePortalProcess.GET_REGISTEREDUSERS_LIST(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, (int)enumRequestType.GetRegisteredfUser, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, out status);
                if (status == 0)
                {
                    System.Data.DataColumn C_PAN = new System.Data.DataColumn("PAN", typeof(System.String));
                    System.Data.DataColumn MASK_PAN = new System.Data.DataColumn("MASKPAN", typeof(System.String));

                    C_PAN.DefaultValue = "";
                    MASK_PAN.DefaultValue = "";
                    DtUserDetails.Columns.Add(C_PAN);
                    DtUserDetails.Columns.Add(MASK_PAN);
                    foreach (DataRow row in DtUserDetails.Rows)
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DtUserDetails.Rows.Count);

                        _CommanDetails.SystemLogger.WriteTransLog(this, "ROWS:   " + row[18].ToString());

                        if (row[18].ToString() != "")
                        {
                            string _CardNumber = string.Empty;
                            _CardNumber = ConnectionStringEncryptDecrypt.DecryptString(row[18].ToString());
                            row["PAN"] = _CardNumber;
                            row["MASKPAN"] = _CardNumber.Substring(0, 6) + new string('X', _CardNumber.Length - 10) + _CardNumber.Substring(_CardNumber.Length - 4); ;
                        }
                    }
                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }




        public void Get_RegisteredUsersAccountList(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {
                DtUserDetails = MobilePortalProcess.SyncAccounts(_MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsersAccountList";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsersAccountList";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void Set_BRANCH_ATM_LOCATION(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                DtUserDetails = MobilePortalProcess.SET_INSERTBRANCH_ATM_LOCATION(_MOBILEPORTAL_REQ.BRANCHCODE, _MOBILEPORTAL_REQ.BRANCHLOCATION, _MOBILEPORTAL_REQ.LATITUDE, _MOBILEPORTAL_REQ.LONGITUDE, RegType, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BranchAlreadyExist);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GET_BRANCH_ATM_LOCATION(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtLocationDetails = null;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                DtLocationDetails = MobilePortalProcess.GET_ATM_LOCATION(RegType, _MOBILEPORTAL_REQ.BRANCHCODE, out status);
                if (status == 0)
                {
                    if (DtLocationDetails == null || DtLocationDetails.Rows.Count <= 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BranchNotFound);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = DtLocationDetails;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BranchNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void UPDATE_BRANCH_ATM_LOCATION(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            DataTable DtLocationDetails = null;
            int status = -1;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                DtLocationDetails = MobilePortalProcess.UPDATE_ATM_LOCATION(_MOBILEPORTAL_REQ.BRANCHCODE, _MOBILEPORTAL_REQ.BRANCHLOCATION, _MOBILEPORTAL_REQ.LATITUDE, _MOBILEPORTAL_REQ.LONGITUDE, RegType, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = DtLocationDetails;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = null;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void DELETE_BRANCH_ATM_LOCATION(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                MobilePortalProcess.DELETE_ATM_LOCATION(_MOBILEPORTAL_REQ.BRANCHCODE, _MOBILEPORTAL_REQ.BRANCHLOCATION, _MOBILEPORTAL_REQ.LATITUDE, _MOBILEPORTAL_REQ.LONGITUDE, RegType, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void BLOCKUSER(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            try
            {
                MobilePortalProcess.BLOCK_UNBLOCK_RESETPASS_PIN(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, (int)enumAction.BlockUser, enumRequestType.BLOCKUNBLOCK.ToString(), _MOBILEPORTAL_REQ.LoginPassword, _MOBILEPORTAL_REQ.PINOFFSET, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_REQ.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void UNBLOCKUSER(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            try
            {
                MobilePortalProcess.BLOCK_UNBLOCK_RESETPASS_PIN(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, (int)enumAction.UnBlockUser, enumRequestType.BLOCKUNBLOCK.ToString(), _MOBILEPORTAL_REQ.LoginPassword, _MOBILEPORTAL_REQ.PINOFFSET, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_REQ.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void RESETPASSWORD(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            _MOBILEPORTAL_REQ.LoginPassword = GetUniqueKey(4);
            try
            {
                CONFIGURATIONCONFIGDATA.GetKeys(ref _Authentication.SsmComkey, ref _Authentication.SsmMasterKey, ref _Authentication.SsmPvk, ref _Authentication.HsmZpk, ref _Authentication.HsmPvk, ref _Authentication.HsmComkey, ref _Authentication.HsmCvv1, ref _Authentication.HsmCvv2, ref _Authentication.TmkEncryptedKey);

                MobilePortalProcess.BLOCK_UNBLOCK_RESETPASS_PIN(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, (int)enumAction.BlockUser, enumRequestType.RESETPASS.ToString(), MaximusAESEncryption.EncryptString(_MOBILEPORTAL_REQ.LoginPassword, _Authentication.SsmMasterKey), _MOBILEPORTAL_REQ.PINOFFSET, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                   // DataTable DTCustomerdata = null;//commented by sk
                     // DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);

                    _ProcessHost.ProcessAccountQueryFromHostPortal(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);

                    if (status != 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        return;
                    }
                    else if (_MOBILEPORTAL_RES.ResponseCode=="00" || _MOBILEPORTAL_RES.ResponseCode=="000")
                    {
                        //_MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();//commented by sk
                        if (!string.IsNullOrEmpty(_MOBILEPORTAL_RES.MobileNumber))
                        {
                            //_ProcessMessage.ProcessSendResetPassword(_MOBILEPORTAL_REQ);
                            string SMSBody = "Dear Druk Customer," + Environment.NewLine + "Your new password for DrukPay is : " + _MOBILEPORTAL_REQ.LoginPassword + "." + Environment.NewLine + "Do not share this password to anyone for security reasons.";
                            _ProcessMessage.SendSmsCommon(SMSBody, _MOBILEPORTAL_RES.MobileNumber);
                            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                            _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        }
                        //else if (!string.IsNullOrEmpty(_MOBILEPORTAL_RES.EmailID))
                        //{
                        //    _ProcessMessage.ProcessSendResetPassword(_MOBILEPORTAL_REQ);
                        //    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        //    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        //    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        //}
                        else
                        {
                            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidTransaction);
                            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                            _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        }
                    }
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_REQ.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void RESETMPIN(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            try
            {
                string mPIN = string.Empty;
                _SSM.ResetMpin(ref _Authentication, ref _MOBILEPORTAL_REQ, out mPIN);
                if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                {
                    _MOBILEPORTAL_REQ.NewMpin = mPIN;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                    return;
                }
               // CommonLogger.WriteTransLog(this, "New Offset is : " + _Authentication.NewOffset + " New Pin :- " + _MOBILEPORTAL_REQ.NewMpin);
                MobilePortalProcess.BLOCK_UNBLOCK_RESETPASS_PIN(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, (int)enumAction.BlockUser, enumRequestType.RESETMPIN.ToString(), _MOBILEPORTAL_REQ.LoginPassword, _Authentication.NewOffset, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                    DataTable DTCustomerdata = null;
                    // DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    _ProcessHost.ProcessAccountQueryFromHostPortal(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);

                    if (status != 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        return;
                    }
                    else if (_MOBILEPORTAL_RES.ResponseCode == "00" || _MOBILEPORTAL_RES.ResponseCode == "000")
                    {
                        //_MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();//commented by sk
                        if (!string.IsNullOrEmpty(_MOBILEPORTAL_RES.MobileNumber))
                        {
                           // _ProcessMessage.ProcessSendResetPassword(_MOBILEPORTAL_REQ);
                            string SMSBody = "Dear Druk Customer," + Environment.NewLine + "Your TPIN for DrukPay is : " + _MOBILEPORTAL_REQ.NewMpin + "."  + Environment.NewLine + "Do not share this TPIN to anyone for security reasons.";
                            _ProcessMessage.SendSmsCommon(SMSBody, _MOBILEPORTAL_RES.MobileNumber);
                            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                            _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        }
                        //else if (!string.IsNullOrEmpty(_MOBILEPORTAL_RES.EmailID))
                        //{
                        //    _ProcessMessage.ProcessSendResetPassword(_MOBILEPORTAL_REQ);
                        //    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        //    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        //    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        //}
                        else
                        {
                            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidTransaction);
                            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                            _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        }
                    }
                    else
                    {
                        //_MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                        _ProcessMessage.ProcessSendResetmPIN(_MOBILEPORTAL_REQ);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TpinSucessful);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                    }
                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_REQ.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        //public void MERCHANTDETAILS(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        //{
        //    int status = -1;
        //    DataTable DTMerchantDetails = null;
        //    try
        //    {
        //        DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
        //                                                                 _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.GETMERCHANTDETAILS.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status);
        //        if (status == 0)
        //        {

        //            _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
        //            _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
        //            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
        //            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
        //        }
        //        else
        //        {

        //            _MOBILEPORTAL_RES.MerchantDetails = null;
        //            _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
        //            _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
        //            _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _MOBILEPORTAL_RES.MerchantDetails = null;
        //        _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
        //        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
        //        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
        //        _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
        //    }
        //}


        public void MERCHANTDETAILS(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.GETMERCHANTDETAILS.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.MerchantCategory, "", "");
                if (status == 0)
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DTMerchantDetails.Rows.Count);

                    System.Data.DataColumn C_PAN = new System.Data.DataColumn("PAN", typeof(System.String));
                    System.Data.DataColumn MASK_PAN = new System.Data.DataColumn("MASKPAN", typeof(System.String));

                    C_PAN.DefaultValue = "";
                    MASK_PAN.DefaultValue = "";
                    DTMerchantDetails.Columns.Add(C_PAN);
                    DTMerchantDetails.Columns.Add(MASK_PAN);
                    foreach (DataRow row in DTMerchantDetails.Rows)
                    {
                        //_CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DTMerchantDetails.Rows.Count);

                        //_CommanDetails.SystemLogger.WriteTransLog(this, "ROWS:   " + row[15].ToString());
                        if (row[15].ToString() != "")
                        {
                            string _CardNumber = string.Empty;
                            _CardNumber = ConnectionStringEncryptDecrypt.DecryptString(row[15].ToString());
                            row["PAN"] = _CardNumber;
                            row["MASKPAN"] = _CardNumber.Substring(0, 6) + new string('X', _CardNumber.Length - 10) + _CardNumber.Substring(_CardNumber.Length - 4); ;
                        }
                    }




                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

      

          #region old
        /*
        public void ACTIVEINACTIVEMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            string TransType = string.Empty;
            if (_MOBILEPORTAL_REQ.IsMerchantActive)
                TransType = enumRequestType.MERCHANTACTIVATE.ToString();
            else
                TransType = enumRequestType.MERCHANTINACTIVATE.ToString();
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, TransType, _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    if (_MOBILEPORTAL_REQ.IsMerchantActive)
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantActivated);
                    if (_MOBILEPORTAL_REQ.IsMerchantDeActive)
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantInActivated);

                    if (_MOBILEPORTAL_REQ.IsMerchantActive)
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantActivated);
                    if (_MOBILEPORTAL_REQ.IsMerchantDeActive)
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantInActivated);

                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void REGISTERMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.INSERTMERCHANT.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAddedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAddedSuccess);
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAlreadyRegistered);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAlreadyRegistered);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void UPDATEMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.UPDATEMERCHANT.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantUpdatedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantUpdatedSuccess);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void DeleteMerchant(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.DELETE.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAddedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = "Merchant Deleted Successfully.";
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAlreadyRegistered);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAlreadyRegistered);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }  */
          #endregion old

        public void ACTIVEINACTIVEMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            string TransType = string.Empty;
            if (_MOBILEPORTAL_REQ.IsMerchantActive)
                TransType = enumRequestType.MERCHANTACTIVATE.ToString();
            else
                TransType = enumRequestType.MERCHANTINACTIVATE.ToString();
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, TransType, _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.MerchantCategory, "", "");
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    if (_MOBILEPORTAL_REQ.IsMerchantActive)
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantActivated);
                    if (_MOBILEPORTAL_REQ.IsMerchantDeActive)
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantInActivated);

                    if (_MOBILEPORTAL_REQ.IsMerchantActive)
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantActivated);
                    if (_MOBILEPORTAL_REQ.IsMerchantDeActive)
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantInActivated);

                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void REGISTERMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.INSERTMERCHANT.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.MerchantCategory, "", "");
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAddedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAddedSuccess);
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAlreadyRegistered);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAlreadyRegistered);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void UPDATEMERCHANT(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.UPDATEMERCHANT.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.MerchantCategory, "", "");
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantUpdatedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantUpdatedSuccess);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void DeleteMerchant(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                         _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.DELETE.ToString(), _MOBILEPORTAL_REQ.State, _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.MerchantCategory, "", "");
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAddedSuccess);
                    _MOBILEPORTAL_RES.ResponseDesc = "Merchant Deleted Successfully.";
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantAlreadyRegistered);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantAlreadyRegistered);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantDetails = null;
                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void GET_PENDINGCHECKDETAILS(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtLocationDetails = null;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                DtLocationDetails = MobilePortalProcess.GET_ATM_LOCATION(RegType, _MOBILEPORTAL_REQ.BRANCHCODE, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = DtLocationDetails;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = null;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = null;
                _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GET_APPROVECHECKES(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtLocationDetails = null;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                DtLocationDetails = MobilePortalProcess.GET_ATM_LOCATION(RegType, _MOBILEPORTAL_REQ.BRANCHCODE, out status);
                if (status == 0)
                {

                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = DtLocationDetails;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = null;
                    _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION = null;
                _MOBILEPORTAL_RES.BRANCH_ATM_LOCATION.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GETCHECKDETAIL(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtChequeDetails = null;

            try
            {
                DtChequeDetails = MobilePortalProcess.GetChequeDetails(_MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.ReferenceNumber, out status, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                if (status == 0)
                {
                    DataTable DtChequeDetails1 = new DataTable();
                    try
                    {
                        DtChequeDetails1.Columns.Add("ACCOUNTNUMBER");
                        DtChequeDetails1.Columns.Add("CUSTOMERID");
                        DtChequeDetails1.Columns.Add("MOBILENUMBER");
                        DtChequeDetails1.Columns.Add("DEPOSITTOACC");
                        DtChequeDetails1.Columns.Add("CHEQUENUMBER");
                        DtChequeDetails1.Columns.Add("CHEQUEAMOUNT");
                        DtChequeDetails1.Columns.Add("CHEQUEFRONTIMAGE");
                        DtChequeDetails1.Columns.Add("CHEQUEBACKIMAGE");
                        DtChequeDetails1.Columns.Add("STATUS");
                        DtChequeDetails1.Columns.Add("CHEQUEACCOUNTNUMBER");
                        DtChequeDetails1.Columns.Add("SUBMITTEDON");
                        DtChequeDetails1.Columns.Add("CHEQUEDATE");


                        _CommanDetails.SystemLogger.WriteTransLog(this,"1");
                        

                        for (int i = 0; i < DtChequeDetails.Rows.Count; i++)
                        {
                            string FrontImage = DtChequeDetails.Rows[i][6].ToString();
                            string BackImage = DtChequeDetails.Rows[i][7].ToString();

                            string FrontString = Path.Combine(ConfigurationManager.AppSettings["CHEQUEIMAGE"].ToString(), "ChequeImages", FrontImage);
                            string BackString = Path.Combine(ConfigurationManager.AppSettings["CHEQUEIMAGE"].ToString(), "ChequeImages", BackImage);

                            //string FrontString = DtChequeDetails.Rows[i][6].ToString();
                            //string BackString = DtChequeDetails.Rows[i][7].ToString();


                            DtChequeDetails1.Rows.Add(DtChequeDetails.Rows[i][0], DtChequeDetails.Rows[i][1], DtChequeDetails.Rows[i][2], DtChequeDetails.Rows[i][3], DtChequeDetails.Rows[i][4],
                                                      DtChequeDetails.Rows[i][5], FrontString, BackString, DtChequeDetails.Rows[i][8], DtChequeDetails.Rows[i][9], DtChequeDetails.Rows[i][10], DtChequeDetails.Rows[i][11]);
                            _CommanDetails.SystemLogger.WriteTransLog(this,"ChequeDate :" + DtChequeDetails.Rows[i][11].ToString());
                        }
                    }
                    catch { }

                    _MOBILEPORTAL_RES.ChequeDetails = DtChequeDetails1;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.ChequeDetails = DtChequeDetails;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.ChequeDataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.ChequeDetails = null;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ChequeDetails = null;
                _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GETCHEQUEREPORTDETAIL(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtChequeDetails = null;

            try
            {
                DtChequeDetails = MobilePortalProcess.GetChequeReportDetails(_MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.ReferenceNumber, out status, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                if (status == 0)
                {
                    DataTable DtChequeDetails1 = new DataTable();
                    try
                    {
                        DtChequeDetails1.Columns.Add("ACCOUNTNUMBER");
                        DtChequeDetails1.Columns.Add("CUSTOMERID");
                        DtChequeDetails1.Columns.Add("MOBILENUMBER");
                        DtChequeDetails1.Columns.Add("DEPOSITTOACC");
                        DtChequeDetails1.Columns.Add("CHEQUENUMBER");
                        DtChequeDetails1.Columns.Add("CHEQUEAMOUNT");
                        DtChequeDetails1.Columns.Add("CHEQUEFRONTIMAGE");
                        DtChequeDetails1.Columns.Add("CHEQUEBACKIMAGE");
                        DtChequeDetails1.Columns.Add("STATUS");
                        DtChequeDetails1.Columns.Add("CHEQUEACCOUNTNUMBER");
                        DtChequeDetails1.Columns.Add("SUBMITTEDON");
                        DtChequeDetails1.Columns.Add("APPROVEDBY");
                        DtChequeDetails1.Columns.Add("REASON");
                        DtChequeDetails1.Columns.Add("CHEQUEDATE");
                        for (int i = 0; i < DtChequeDetails.Rows.Count; i++)
                        {
                            string FrontImage = DtChequeDetails.Rows[i][6].ToString();
                            string BackImage = DtChequeDetails.Rows[i][7].ToString();

                            string FrontString = Path.Combine(ConfigurationManager.AppSettings["CHEQUEIMAGE"].ToString(), "ChequeImages", FrontImage);
                            string BackString = Path.Combine(ConfigurationManager.AppSettings["CHEQUEIMAGE"].ToString(), "ChequeImages", BackImage);

                            //string FrontString = DtChequeDetails.Rows[i][6].ToString();
                            //string BackString = DtChequeDetails.Rows[i][7].ToString();


                            DtChequeDetails1.Rows.Add(DtChequeDetails.Rows[i][0], DtChequeDetails.Rows[i][1], DtChequeDetails.Rows[i][2], DtChequeDetails.Rows[i][3], DtChequeDetails.Rows[i][4],
                                                      DtChequeDetails.Rows[i][5], FrontString, BackString, DtChequeDetails.Rows[i][8], DtChequeDetails.Rows[i][9], DtChequeDetails.Rows[i][10], DtChequeDetails.Rows[i][11], DtChequeDetails.Rows[i][12]
                                                       ,DtChequeDetails.Rows[i][13]);
                        }
                    }
                    catch { }

                    _MOBILEPORTAL_RES.ChequeDetails = DtChequeDetails1;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else if (status == 1)
                {

                    _MOBILEPORTAL_RES.ChequeDetails = DtChequeDetails;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.ChequeDataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.ChequeDetails = null;
                    _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ChequeDetails = null;
                _MOBILEPORTAL_RES.ChequeDetails.TableName = "DtChequeDetails";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void APPROVE_DECLINE_CHECK(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            REGISTRATION_REQ _REGISTRATION_REQ = new REGISTRATION_REQ();
            ProcessMessage _ProcessMessage = new ProcessMessage();
            _REGISTRATION_REQ.ChequeAccountNumber = _MOBILEPORTAL_REQ.FromAccount;
            _REGISTRATION_REQ.DepositAccountNumber = _MOBILEPORTAL_REQ.ToAccount;
            _REGISTRATION_REQ.ChequeNumber = _MOBILEPORTAL_REQ.ChequeID;
            _REGISTRATION_REQ.DepositAmount = _MOBILEPORTAL_REQ.Amount;
            _REGISTRATION_REQ.Reason = _MOBILEPORTAL_REQ.DeclineReason;
            int status = -1;
            DataTable DtLocationDetails = null;
            int RegType = -1;
            if (_MOBILEPORTAL_REQ.ISBRANCH)
                RegType = (int)enumRequestType.AddBranchLocation;
            else
                RegType = (int)enumRequestType.AddATMLocation;
            try
            {
                MobilePortalProcess.APPROVE_DECLINE_CHECK(_MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.ChequeID, _MOBILEPORTAL_REQ.ApproveStatus, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason);

                DataTable DTCustomerdataCheque = new DataTable();
                DataTable DTCustomerdataDeposit = new DataTable();
                try { DTCustomerdataCheque = IMPSTransactions.GETCUSTOMERDETAILS_ADDACC(_REGISTRATION_REQ.ChequeAccountNumber, out status); }
                catch { }

                try { DTCustomerdataDeposit = IMPSTransactions.GETCUSTOMERDETAILS_ADDACC(_REGISTRATION_REQ.DepositAccountNumber, out status); }
                catch { }
                try
                {
                    try { _REGISTRATION_REQ.MailID = DTCustomerdataCheque.Rows[0]["e_mail"].ToString(); }
                    catch { }
                    try { _REGISTRATION_REQ.MobileNumber = DTCustomerdataCheque.Rows[0]["mobile_number"].ToString(); }
                    catch { }
                    try { _REGISTRATION_REQ.CustomerName = DTCustomerdataDeposit.Rows[0]["FIRST_NAME"].ToString(); }
                    catch { }
                    _REGISTRATION_REQ.Text = CommanDetails.GetResponseCodeDescription(ConstResponseCode.ChequeDepositedSuccess);
                    try { _CommanDetails.SystemLogger.WriteTransLog(this, "Benificiary Email : " + _REGISTRATION_REQ.Email + " Mobile : " + _REGISTRATION_REQ.MobileNumber + " for reference number : " + _REGISTRATION_REQ.ReferenceNumber); }
                    catch { }
                    if (_MOBILEPORTAL_REQ.ApproveStatus == "1")
                        _ProcessMessage.ProcessSendChequeDepositPortalRemitter(_REGISTRATION_REQ);
                    else
                        _ProcessMessage.ProcessSendChequeDepositPortalDeclineRemitter(_REGISTRATION_REQ);
                }
                catch { }
                try
                {
                    try { _REGISTRATION_REQ.MailID = DTCustomerdataDeposit.Rows[0]["e_mail"].ToString(); }
                    catch { }
                    try { _REGISTRATION_REQ.MobileNumber = DTCustomerdataDeposit.Rows[0]["mobile_number"].ToString(); }
                    catch { }
                    try { _REGISTRATION_REQ.CustomerName = DTCustomerdataDeposit.Rows[0]["FIRST_NAME"].ToString(); }
                    catch { }
                    _REGISTRATION_REQ.Text = CommanDetails.GetResponseCodeDescription(ConstResponseCode.ChequeDepositedSuccess);
                    try { _CommanDetails.SystemLogger.WriteTransLog(this, "Remitter Email : " + _REGISTRATION_REQ.Email + " Mobile : " + _REGISTRATION_REQ.MobileNumber + " for reference number : " + _REGISTRATION_REQ.ReferenceNumber); }
                    catch { }
                    if (_MOBILEPORTAL_REQ.ApproveStatus == "1")
                        _ProcessMessage.ProcessSendChequeDepositPortalBeneficiary(_REGISTRATION_REQ);
                    else
                        _ProcessMessage.ProcessSendChequeDepositPortalDeclineBeneficiary(_REGISTRATION_REQ);
                }
                catch { }

                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void InsertUpdateDeleteImages(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DtImageList = null;

            try
            {
                try
                {
                    MemoryStream ms = new MemoryStream(_MOBILEPORTAL_REQ.Images);
                    Image _Image = Image.FromStream(ms);
                }
                catch { }
                if (Action == "0")
                    DtImageList = MobilePortalProcess.UploadImages(_MOBILEPORTAL_REQ.Images, Action, _MOBILEPORTAL_REQ.ImagesID);
                else if (Action == "2")
                    DtImageList = MobilePortalProcess.UploadImages(_MOBILEPORTAL_REQ.Images, Action, _MOBILEPORTAL_REQ.ImagesID);
                else if (Action == "3")
                    DtImageList = MobilePortalProcess.UploadImages(_MOBILEPORTAL_REQ.Images, Action, _MOBILEPORTAL_REQ.ImagesID);
                else
                    DtImageList = MobilePortalProcess.Get_DisImages(_MOBILEPORTAL_REQ.Images, Action, _MOBILEPORTAL_REQ.ImagesID);

                //for (int i = 0; i < DtImageList.Rows.Count; i++)
                //{
                //    DtImageList.Rows[i][1] = (byte[])(DtImageList.Rows[i][1]);
                //}
                DataTable dt = new DataTable();
                try
                {
                    dt.Columns.Add("ID");
                    dt.Columns.Add("IMAGEDATA");
                    for (int i = 0; i < DtImageList.Rows.Count; i++)
                    {
                        byte[] imgBytes = (byte[])(DtImageList.Rows[i][1]);

                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                        Bitmap MyBitmap = (Bitmap)tc.ConvertFrom(imgBytes);


                        string imgString = Convert.ToBase64String(imgBytes);
                        dt.Rows.Add(DtImageList.Rows[i][0], imgString);
                        //Set the source with data:image/bmp
                        //img.Src = String.Format("data:image/Bmp;base64,{0}\"", imgString);   //img is the Image control ID
                    }
                }
                catch { }

                _MOBILEPORTAL_RES.ImagesList = dt;
                _MOBILEPORTAL_RES.ImagesList.TableName = "ImagesList";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);



            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ChequeDetails = null;
                _MOBILEPORTAL_RES.ChequeDetails.TableName = "ImagesList";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GetIMPSReports(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DTReportData = null;
            _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:  1");
            try
            {
                DTReportData = MobilePortalProcess.GetIMPSReports(_MOBILEPORTAL_REQ.MSGID, _MOBILEPORTAL_REQ.CollerID, _MOBILEPORTAL_REQ.FromAccount, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.ResponseCode, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.TxnType, _MOBILEPORTAL_REQ.BankCode, _MOBILEPORTAL_REQ.Status);
             
                if(DTReportData!=null && DTReportData.Rows.Count >0 )
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DTReportData.Rows.Count);
                    System.Data.DataColumn C_PAN = new System.Data.DataColumn("PAN", typeof(System.String));
                    System.Data.DataColumn MASK_PAN = new System.Data.DataColumn("MASKPAN", typeof(System.String));
                    C_PAN.DefaultValue = "";
                    MASK_PAN.DefaultValue = "";
                    DTReportData.Columns.Add(C_PAN);
                    DTReportData.Columns.Add(MASK_PAN);
                    foreach (DataRow row in DTReportData.Rows)
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "ROWS:   " + row[21].ToString());
                        if (row[21].ToString() != "")
                        {
                            string _CardNumber = string.Empty;
                            _CardNumber = ConnectionStringEncryptDecrypt.DecryptString(row[21].ToString());
                            row["PAN"] = _CardNumber;
                            row["MASKPAN"] = _CardNumber.Substring(0, 6) + new string('X', _CardNumber.Length - 10) + _CardNumber.Substring(_CardNumber.Length - 4); ;
                        }
                    }
                    _MOBILEPORTAL_RES.ReportData = DTReportData;
                    _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                 _CommanDetails.SystemLogger.WriteTransLog(this, "DTMerchantDetails:   " + DTReportData.Rows.Count);
                _MOBILEPORTAL_RES.ReportData = DTReportData;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ReportData = null;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void GetRequestMoney(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DTReportData = null;

            try
            {
                DTReportData = MobilePortalProcess.GetIMPSReqReportsDetails(_MOBILEPORTAL_REQ.MSGID, _MOBILEPORTAL_REQ.CollerID, _MOBILEPORTAL_REQ.FromAccount, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.ResponseCode, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.TxnType, _MOBILEPORTAL_REQ.BankCode, _MOBILEPORTAL_REQ.Status);


                _MOBILEPORTAL_RES.ReqReportData = DTReportData;
                _MOBILEPORTAL_RES.ReqReportData.TableName = "ReqReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ReqReportData = DTReportData;
                _MOBILEPORTAL_RES.ReqReportData.TableName = "ReqReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }




        public void GetPaymentReports(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DTReportData = null;

            try
            {
                DTReportData = MobilePortalProcess.GetPaymentReports(_MOBILEPORTAL_REQ.MSGID, _MOBILEPORTAL_REQ.CollerID, _MOBILEPORTAL_REQ.FromAccount, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.ResponseCode, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.TxnType, _MOBILEPORTAL_REQ.BankCode, _MOBILEPORTAL_REQ.Status);


                _MOBILEPORTAL_RES.ReportData = DTReportData;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ReportData = null;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GetRDTDReports(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DTReportData = null;

            try
            {
                DTReportData = MobilePortalProcess.GetRDTDReports(_MOBILEPORTAL_REQ.MSGID, _MOBILEPORTAL_REQ.CollerID, _MOBILEPORTAL_REQ.FromAccount, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.ResponseCode, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.TxnType, _MOBILEPORTAL_REQ.BankCode, _MOBILEPORTAL_REQ.Status);


                _MOBILEPORTAL_RES.ReportData = DTReportData;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ReportData = null;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void GetPaymentReversalReports(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, string Action)
        {
            DataTable DTReportData = null;

            try
            {
                DTReportData = MobilePortalProcess.GetPaymentReversalReports(_MOBILEPORTAL_REQ.MSGID, _MOBILEPORTAL_REQ.CollerID, _MOBILEPORTAL_REQ.FromAccount, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.ResponseCode, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.TxnType, _MOBILEPORTAL_REQ.BankCode, _MOBILEPORTAL_REQ.Status);


                _MOBILEPORTAL_RES.ReportData = DTReportData;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.ReportData = null;
                _MOBILEPORTAL_RES.ReportData.TableName = "ReportData";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void Set_GlobalLimit(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            DataTable DTReportData = null;

            try
            {
                _CommanDetails.SystemLogger.WriteTraceLog("1", "1");
                DTReportData = MobilePortalProcess.Set_GlobalLimit(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.NewLimit, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ACQNewLimit, _MOBILEPORTAL_REQ.BNGULSTRTRANSFERLIMIT);
                _CommanDetails.SystemLogger.WriteTraceLog("2", "2");
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);


                /*sms Send to customer */

                #region SMS Send 
                if(DTReportData != null || DTReportData.Rows.Count > 0)
                {
                    _CommanDetails.SystemLogger.WriteTraceLog("DTReportData 1", "DTReportData 1");
                    DataTable DTCustomerdata=null;
                    int status=-1;
                    if (_MOBILEPORTAL_REQ.MOBILENUMBER.Length > 8)
                    {
                      _MOBILEPORTAL_REQ.MOBILENUMBER=  _MOBILEPORTAL_REQ.MOBILENUMBER.Substring(3, _MOBILEPORTAL_REQ.MOBILENUMBER.Length - 3);
                    }
                    DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                      if (status != 0)
                      {


                          _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());

                          _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                          _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_RES.ResponseCode"  + _MOBILEPORTAL_RES.ResponseCode);
                          _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_RES.ResponseDesc" + _MOBILEPORTAL_RES.ResponseDesc);

                          _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_REQ.MOBILENUMBER"+_MOBILEPORTAL_REQ.MOBILENUMBER.ToString());
                          _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_REQ.ACCOUNTNUMBER" + _MOBILEPORTAL_REQ.ACCOUNTNUMBER);
                          _CommanDetails.SystemLogger.WriteTraceLog("No data found at CBS", "No data found at CBS");
                      }
                      else
                      {
                          _CommanDetails.SystemLogger.WriteTraceLog(this, "Email and SMS works");
                          _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();

                          _CommanDetails.SystemLogger.WriteTraceLog("Enhancement cust mail ID : ", _MOBILEPORTAL_REQ.MailID);

                          ProcessMessage _ProcessMessage = new ProcessMessage();
                            SMSJson _SMSJson = new SMSJson();
                            MOBILEBANKING_REQ _MOBILEBANKING_REQ=new MOBILEBANKING_REQ();
                          if(_MOBILEPORTAL_REQ.MOBILENUMBER.Length > 8)
                          {
                              _MOBILEBANKING_REQ.BENIFICIARYMOBILE = _MOBILEPORTAL_REQ.MOBILENUMBER.Substring(3, _MOBILEPORTAL_REQ.MOBILENUMBER.Length - 3);
                          }
                          else
                          {
                              _MOBILEBANKING_REQ.BENIFICIARYMOBILE = _MOBILEPORTAL_REQ.MOBILENUMBER;
                          }
                            _MOBILEBANKING_REQ.MailID = _MOBILEPORTAL_REQ.MailID;

                            // XXXXX" + _MOBILEBANKING_REQ.BENIFICIARYACC.Substring( _MOBILEPORTAL_REQ.ACCOUNTNUMBER.Length - 8)  

                            string value = _MOBILEPORTAL_REQ.NewLimit.Replace(",", "");
                            string AmountThousandSeperated = ""; ;
                            long ul;
                            if (long.TryParse(value, out ul))
                            {

                                 AmountThousandSeperated = string.Format("{0:#,#0}", ul);

                            }


                            //string SMS = "Your request to enhance intra fund transfer limit to Nu." + AmountThousandSeperated + " for account XXXXX" + _MOBILEPORTAL_REQ.ACCOUNTNUMBER.Substring(_MOBILEPORTAL_REQ.ACCOUNTNUMBER.Length - 8) + " is approved. Thank you. ";

                            string SMS = "Your request to enhance intra fund transfer limit to Nu." + AmountThousandSeperated +".00"+ " for account " + _MOBILEPORTAL_REQ.ACCOUNTNUMBER.Substring(0,1) +"XXXXXXXXXXX"+ _MOBILEPORTAL_REQ.ACCOUNTNUMBER.Substring(_MOBILEPORTAL_REQ.ACCOUNTNUMBER.Length - 4) + " is approved. Thank you. ";
                           

                            _ProcessMessage.ProcessSendSMS(_MOBILEBANKING_REQ, SMS, "Limit Enchanement", SMS);

                          _CommanDetails.SystemLogger.WriteTransLog(this, "SMS STATUS : " + OTPmPINStatus.SmsStatus + "\t MAIL STATUS : " + OTPmPINStatus.MailStatus);
                          if (OTPmPINStatus.SmsStatus || OTPmPINStatus.MailStatus)
                          {
                              _CommanDetails.SystemLogger.WriteTraceLog(this, "Email and SMS works send success");
                          }
                          else
                          {
                              _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToSendMessage);
                              _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                              _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_REQ.MOBILENUMBER" + _MOBILEPORTAL_REQ.MOBILENUMBER.ToString());
                              _CommanDetails.SystemLogger.WriteTraceLog(this, "_MOBILEPORTAL_REQ.ACCOUNTNUMBER" + _MOBILEPORTAL_REQ.ACCOUNTNUMBER);
                              _CommanDetails.SystemLogger.WriteTraceLog(this, "SMS NOt send");
                              
                          }
                      }

                }
                #endregion SMS Send


            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }



   

        public void Select_ShowDetails(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            DataTable DTReportData = null;

            try
            {
                 DTReportData = MobilePortalProcess.Set_Select_ShowDetails(_MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.Amount, _MOBILEPORTAL_REQ.TransaferLimitAmount);
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                if (DTReportData != null || DTReportData.Rows.Count >0)
                {
                    _MOBILEPORTAL_RES.ShowMaster = DTReportData;
                    _MOBILEPORTAL_RES.ShowMaster.TableName = "ShowMaster";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.ShowMaster.TableName = "ShowMaster";
                    _MOBILEPORTAL_RES.ShowMaster = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }

            }



            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void Select_ShowContestantDetails(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            DataTable DTReportData = null;

            try
            {
                DTReportData = MobilePortalProcess.Set_Select_ContestantDetails(_MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.ContestantID, _MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ContestantName,_MOBILEPORTAL_REQ.ContestantNumber,_MOBILEPORTAL_REQ.IsRemoved);
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                if (DTReportData != null || DTReportData.Rows.Count > 0)
                {
                    _MOBILEPORTAL_RES.ContestantMaster = DTReportData;
                    _MOBILEPORTAL_RES.ContestantMaster.TableName = "ContestantMaster";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.ContestantMaster.TableName = "ContestantMaster";
                    _MOBILEPORTAL_RES.ContestantMaster = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }

            }



            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

  
        public void Get_SignUpRequest(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {

                DtUserDetails = MobilePortalProcess.GET_SignUpRequest(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, (int)enumRequestType.GetRegisteredfUser, out status);
                if (status == 0)
                {
                    DataTable DTCustomerdata = new DataTable();
                    string ACC = string.Empty;
                    string CID = string.Empty;
                    for (int i = 0; i < DtUserDetails.Rows.Count; i++)
                    {
                        try
                        {
                            ACC = DtUserDetails.Rows[i]["ACCOUNTNUMBER"].ToString();
                            CID = DtUserDetails.Rows[i]["ACCOUNTNUMBER"].ToString().Substring(1, 9);
                            _MOBILEPORTAL_REQ.CUSTOMERID = CID;
                            _MOBILEPORTAL_REQ.ACCOUNTNUMBER = ACC;
                            //Get_RegisteredUsersAccountList(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                            //for (int j = 0; j < _MOBILEPORTAL_RES.RegisteredUsers.Rows.Count; j++)
                            //{
                            try
                            {
                                DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(DtUserDetails.Rows[i]["ACCOUNTNUMBER"].ToString(), null, out status);
                                DtUserDetails.Rows[i]["LASTLOGEDIN"] = DTCustomerdata.Rows[0]["e_mail"].ToString();
                            }
                            catch { }
                            //}
                        }
                        catch { }
                    }

                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void Set_ApproveDeclineSignUpRequest(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {

                DtUserDetails = MobilePortalProcess.Set_ApproveDeclineSignUpRequest(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ApproveStatus, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    DataTable DTCustomerdata = null;
                    DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    CommonLogger.WriteTransLog(this, "Account Number :" + _MOBILEPORTAL_REQ.ACCOUNTNUMBER);
                    CommonLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEPORTAL_REQ.MOBILENUMBER);
                    CommonLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                    if (status != 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        return;
                    }
                    else
                    {
                        _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                        _ProcessMessage.ProcessSendApproveDeclineNotification(_MOBILEPORTAL_REQ);

                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                    }
                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void QRCODE(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            try
            {
                DataTable DtQRDATA = MobilePortalProcess.GET_QRDATA(_MOBILEPORTAL_REQ.MerchantAccountNumber, _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantCode);
                if (DtQRDATA != null || DtQRDATA.Rows.Count > 0)
                {

                    _Authentication.SsmMasterKey = "49FBC7A88F61F4B3CB023270B634971C";

                    _MOBILEPORTAL_RES.QRDATA = DtQRDATA.Rows[0]["MERCHANTNAME"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTCODE"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTACCOUNTNUMBER"].ToString() + "^"
                                             + DtQRDATA.Rows[0]["MERCHANTMOBILENUMBER"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTBANKCODE"].ToString() + "^"
                                             + DtQRDATA.Rows[0]["BANKNAME"].ToString() + "^";

                    _MOBILEPORTAL_RES.QRDATA = MaximusAESEncryption.EncryptString(_MOBILEPORTAL_RES.QRDATA, _Authentication.SsmMasterKey);
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }

            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void NationalQRCODE(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            try
            {


                DataTable DtQRDATA = MobilePortalProcess.GET_QRDATA(_MOBILEPORTAL_REQ.MerchantAccountNumber, _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantCode);
                if (DtQRDATA != null || DtQRDATA.Rows.Count > 0)
                {

                    // _Authentication.SsmMasterKey = "49FBC7A88F61F4B3CB023270B634971C";

                    //_MOBILEPORTAL_RES.QRDATA = DtQRDATA.Rows[0]["MERCHANTNAME"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTCODE"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTACCOUNTNUMBER"].ToString() + "^"
                    //                         + DtQRDATA.Rows[0]["MERCHANTMOBILENUMBER"].ToString() + "^" + DtQRDATA.Rows[0]["MERCHANTBANKCODE"].ToString() + "^"
                    //                         + DtQRDATA.Rows[0]["BANKNAME"].ToString() + "^";

                    string MERCHANT_IDENTIFIER_NUMBER = string.Empty;

                    #region Calc
                    DataTable DTBranchCity = IMPSTransactions.GET_NQRCBranchCity(_MOBILEPORTAL_REQ.MerchantAccountNumber, out status);

                    if (DTBranchCity != null && DTBranchCity.Rows.Count > 0)
                    {

                        _MOBILEPORTAL_REQ.MerchantCity = DTBranchCity.Rows[0][2].ToString();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "City : " + DTBranchCity.Rows[0][2].ToString());

                    }

                    string BranchCode = Convert.ToString(ConfigurationManager.AppSettings["BankQRBankCode"]) + "1";

                    _CommanDetails.SystemLogger.WriteTransLog(this, "Branch : " + BranchCode);


                    //_REGISTRATION_REQ.typeofUser = "SelfNQRC";

                    if (_MOBILEPORTAL_REQ.MerchantCity != "" && BranchCode != "" && !String.IsNullOrEmpty(_MOBILEPORTAL_REQ.MerchantCity))
                    {

                        DataTable DTNQRCExist = IMPSTransactions.GET_NQRCExistWithDetails(_MOBILEPORTAL_REQ.MerchantAccountNumber, DTBranchCity.Rows[0][2].ToString(), DtQRDATA.Rows[0]["MERCHANTNAME"].ToString(), DtQRDATA.Rows[0]["MerchantCategory"].ToString(), out status, DtQRDATA.Rows[0]["MERCHANTMOBILENUMBER"].ToString());

                        if (status == 0)
                        {
                            int identifierstatus = -1;
                            DataTable DTIdentifierExist = IMPSTransactions.GET_NQRCIdentifierExistWithDetails(_MOBILEPORTAL_REQ.MerchantAccountNumber, "Portal", out identifierstatus);

                            _CommanDetails.SystemLogger.WriteTransLog(this, "DTIdentifierExist : " + DTIdentifierExist.Rows.Count);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "identifierstatus : " + identifierstatus);

                            if (identifierstatus == 0)
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "inside : " + DTIdentifierExist.Rows[0][5].ToString());
                                MERCHANT_IDENTIFIER_NUMBER = ConnectionStringEncryptDecrypt.DecryptString(DTIdentifierExist.Rows[0][5].ToString());
                            }
                            else
                            {
                                MERCHANT_IDENTIFIER_NUMBER = _ProcessMessage.MerchantPANGeneration(BranchCode, "");
                            }
                            _CommanDetails.SystemLogger.WriteTransLog(this, "MerchantIdentifier STATUS : " + MERCHANT_IDENTIFIER_NUMBER);

                            string Additionaldata = "02" + Convert.ToString(DtQRDATA.Rows[0]["MERCHANTMOBILENUMBER"].ToString().Length).PadLeft(2, '0') + DtQRDATA.Rows[0]["MERCHANTMOBILENUMBER"].ToString();


                            string QRDATA = NQRCConfiguration.PAYLOAD_FORMAT_INDICATOR.ToString() + NQRCConfiguration.POINT_OF_INITIATION_METHOD.ToString()
                            + NQRCConfiguration.MERCHANT_IDENTIFIER.ToString() + MERCHANT_IDENTIFIER_NUMBER
                           + NQRCConfiguration.MERCHANT_CATEGORY_CODE.ToString() + DtQRDATA.Rows[0]["MerchantCategory"].ToString()
                           + NQRCConfiguration.TRANSACTION_CURRENCY_CODE.ToString() + NQRCConfiguration.COUNTRY_CODE.ToString()
                           + NQRCConfiguration.MERCHANT_NAME + Convert.ToString(DtQRDATA.Rows[0]["MERCHANTNAME"].ToString().Length).PadLeft(2, '0')
                           + DtQRDATA.Rows[0]["MERCHANTNAME"].ToString()




                           + NQRCConfiguration.MERCHANT_CITY
                           + Convert.ToString(_MOBILEPORTAL_REQ.MerchantCity.Length).PadLeft(2, '0')
                           + _MOBILEPORTAL_REQ.MerchantCity

                            + NQRCConfiguration.MERCHANTADDITIONALDATA +
                           Convert.ToString(Additionaldata.Length).PadLeft(2, '0') + Additionaldata +


                           NQRCConfiguration.MERCHANT_CRC;

                            string CRC = _ProcessMessage.CalcCRC16(QRDATA);

                            _MOBILEPORTAL_RES.QRDATA = QRDATA + CRC;

                    #endregion Calc


                            _CommanDetails.SystemLogger.WriteTransLog(this, " MERCHANT_IDENTIFIER_NUMBER 11111: " + MERCHANT_IDENTIFIER_NUMBER);

                            if (IMPSTransactions.INSERTSelfNQRCDetails(_MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.ReferenceNumber,
                                MERCHANT_IDENTIFIER_NUMBER, DtQRDATA.Rows[0]["MERCHANTNAME"].ToString(), _MOBILEPORTAL_RES.QRDATA,
                                "Portal", DtQRDATA.Rows[0]["MerchantCategory"].ToString(),
                                            NQRCConfiguration.TRANSACTION_CURRENCY_CODE.ToString().Substring(4, 3),
                                            NQRCConfiguration.COUNTRY_CODE.ToString().Substring(4, 2), _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                            _MOBILEPORTAL_REQ.MerchantMobileNumber, DTBranchCity.Rows[0][2].ToString(), BranchCode))
                            {

                                DataTable DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                              _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress,
                                                                              _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID,
                                                                              enumRequestType.UPDATEMERCHANT.ToString(), _MOBILEPORTAL_REQ.State,
                                                                              _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID, out status,
                                                                              _MOBILEPORTAL_REQ.MerchantCategory, MERCHANT_IDENTIFIER_NUMBER, "NQRC");
                                if (status == 0)
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, " REGISTERMERCHANT NQRC Updation : " + status);
                                    _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                                    _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantUpdatedSuccess);
                                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantUpdatedSuccess);



                                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_RES.ReferenceNumber;
                                    _MOBILEPORTAL_RES.QRDATA = _MOBILEPORTAL_RES.QRDATA;
                                }
                            }
                            else
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Failed REGISTERMERCHANT NQRC Insertion table : ");
                                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                                _MOBILEPORTAL_RES.ResponseDesc = null;
                                _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_RES.ReferenceNumber;
                            }
                        }
                        else
                        {
                            DataTable DTMerchantDetails = MobilePortalProcess.REGISTERMERCHANT(_MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.MerchantCode, _MOBILEPORTAL_REQ.MerchantAccountNumber,
                                                                                      _MOBILEPORTAL_REQ.MerchantMobileNumber, _MOBILEPORTAL_REQ.MerchantAddress, _MOBILEPORTAL_REQ.MerchantBankCode, _MOBILEPORTAL_REQ.MerchantCID, enumRequestType.GETMERCHANTDETAILS.ToString(), _MOBILEPORTAL_REQ.State,
                                                                                      _MOBILEPORTAL_REQ.EMAIL, _MOBILEPORTAL_REQ.LogedInUserID,
                                                                                      out status, _MOBILEPORTAL_REQ.MerchantCategory,
                                                                                     ConnectionStringEncryptDecrypt.DecryptString(DTNQRCExist.Rows[0][5].ToString()), "NQRC");
                            if (status == 0)
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, " NQRC already in GET_NQRCExistWithDetails  : " + status);
                                _MOBILEPORTAL_RES.MerchantDetails = DTMerchantDetails;
                                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Merchant";
                                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.MerchantUpdatedSuccess);
                                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.MerchantUpdatedSuccess);

                                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                                _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_RES.ReferenceNumber;
                                _MOBILEPORTAL_RES.QRDATA = ConnectionStringEncryptDecrypt.DecryptString(DTNQRCExist.Rows[0]["SequecnceData"].ToString());
                            }

                        }

                    }
                    else
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Failed  NQRC MerchantCity Data not found at CBS ");
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFoundDB);
                        _MOBILEPORTAL_RES.ResponseDesc = null;
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_RES.ReferenceNumber;
                    }

                    //_MOBILEPORTAL_RES.QRDATA = NQRCConfiguration.PAYLOAD_FORMAT_INDICATOR.ToString() + NQRCConfiguration.POINT_OF_INITIATION_METHOD.ToString()
                    //    + NQRCConfiguration.MERCHANT_IDENTIFIER.ToString() + MERCHANT_IDENTIFIER_NUMBER + 
                    //    + NQRCConfiguration.TRANSACTION_CURRENCY_CODE.ToString() + NQRCConfiguration.COUNTRY_CODE.ToString() + DtQRDATA.Rows[0]["MERCHANTNAME"].ToString() 
                    //    +DtQRDATA.Rows[0]["CITY"].ToString() + CRC;

                }


                else
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Failed REGISTERMERCHANT NQRC Data not found in merchant table : " + status);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = null;
                }


            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = null;
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "0123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public void Get_AddAccountRequest(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DtUserDetails = null;
            try
            {

                DtUserDetails = MobilePortalProcess.GET_AddAccountRequest(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.LogedInUserID, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }

        public void Set_ApproveDeclineAddAccountRequest(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            string PrimaryAccount = string.Empty;
            string PrimaryMobile = string.Empty;
            DataTable DtUserDetails = null;
            try
            {
                DtUserDetails = MobilePortalProcess.Set_ApproveDeclineAddAccountRequest(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.ApproveStatus, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status, out PrimaryAccount, out PrimaryMobile);
                CommonLogger.WriteTransLog(this, "Approve Decline Status :" + status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.RegisteredUsers = DtUserDetails;
                    _MOBILEPORTAL_RES.RegisteredUsers.TableName = "RegisteredUsers";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    DataTable DTCustomerdata = null;
                    _MOBILEPORTAL_REQ.MOBILENUMBER = PrimaryMobile;
                    DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(PrimaryAccount, PrimaryMobile, out status);
                    CommonLogger.WriteTransLog(this, "Primary Account Number :" + PrimaryAccount);
                    CommonLogger.WriteTransLog(this, "Primary Mobile Number  :" + PrimaryMobile);
                    CommonLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                    if (status != 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                        return;
                    }
                    else
                    {
                        _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                        _ProcessMessage.ProcessSendApproveDeclineAddAccountNotification(_MOBILEPORTAL_REQ);

                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                    }
                }
                else
                {
                    _MOBILEPORTAL_RES.RegisteredUsers = null;
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.DataNotFound);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }
        public void DeleteUser(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTMerchantDetails = null;
            try
            {
                MobilePortalProcess.BLOCK_UNBLOCK_RESETPASS_PIN(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, (int)enumAction.TerminateUser, enumRequestType.TerminateUser.ToString(), _MOBILEPORTAL_REQ.LoginPassword, _MOBILEPORTAL_REQ.PINOFFSET, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.DeclineReason, out status);
                if (status == 0)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;

                    var task = Task.Factory.StartNew(() =>
                    {
                        DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                        CommonLogger.WriteTransLog(this, "Primary Account Number :" + _MOBILEPORTAL_REQ.ACCOUNTNUMBER);
                        CommonLogger.WriteTransLog(this, "Primary Mobile Number  :" + _MOBILEPORTAL_REQ.MOBILENUMBER);
                        CommonLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                        _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                        _ProcessMessage.ProcessSendTerminationNotification(_MOBILEPORTAL_REQ);
                    });

                }
                else if (status == 1)
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidUser);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
                else
                {
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_REQ.ResponseCode);
                    _MOBILEPORTAL_RES.ReferenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantDetails = null;
                _MOBILEPORTAL_RES.MerchantDetails.TableName = "Location";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }


        public void GetMerchantCategory(ref MOBILEPORTAL_RES _MOBILEPORTAL_RES, MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            DataTable DTmerchant = null;
            try
            {
                DTmerchant = MobilePortalProcess.GetMerchantCategoryDetails();

                if (DTmerchant.Rows.Count > 0)
                {

                    _MOBILEPORTAL_RES.MerchantCategoryList = DTmerchant;
                    _MOBILEPORTAL_RES.MerchantCategoryList.TableName = "MerchantCategoryList";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
                else
                {

                    _MOBILEPORTAL_RES.MerchantCategoryList = null;
                    _MOBILEPORTAL_RES.MerchantCategoryList.TableName = "MerchantCategoryList";
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                }
            }
            catch (Exception ex)
            {

                _MOBILEPORTAL_RES.MerchantCategoryList = null;
                _MOBILEPORTAL_RES.MerchantCategoryList.TableName = "MerchantCategoryList";
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
            }
        }
    }
    public enum enumRequestType
    {
        GetRegisteredfUser = 0,
        GetAccountList = 1,
        AddBranchLocation = 2,
        AddATMLocation = 3,
        BLOCKUNBLOCK = 4,
        RESETPASS = 5,
        RESETMPIN = 6,
        INSERTMERCHANT = 7,
        UPDATEMERCHANT = 8,
        MERCHANTACTIVATE = 9,
        MERCHANTINACTIVATE = 10,
        GETMERCHANTDETAILS = 11,
        DELETE = 12,
        TerminateUser = 13

    }
    public enum enumAction
    {
        UnBlockUser = 0,
        BlockUser = 1,
        AddBranchLocation = 2,
        AddATMLocation = 3,
        TerminateUser = 4,

    }
}