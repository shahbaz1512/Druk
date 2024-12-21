using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ThalesSim.Core.Cryptography;
using MaxiSwitch.DALC.Configuration;
using HSMCommunicationChanel;
using DALC;

namespace IMPSTransactionRouter.Models
{
    public class HSMAuthentication
    {
        public string Cardnumber;
        public string CardTrack2;
        public string CCVResponse;
        public string HsmComkey;
        public string HsmCvkPair;
        public string HsmCvv1;
        public string HsmCvv2;
        public string HsmPvk;
        public string HsmZpk;
        public string NewOffset;
        public string PinBlock;
        public string PinChangeBlock;
        public string PinOffset;
        public enumSecurityModuleType SecurityModule;
        public string SessionKeyResponse;
        public string SsmComkey;
        public string SsmCvkPair;
        public string SsmCvv1;
        public string SsmCvv2;
        public string SsmMasterKey;
        public string SsmPvk;
        public string TmkEncryptedKey;
        public enumModeOfTransaction TransactionReqMode;
        public enumTransactionStatus TransactionStatus;
        public string TransactionType;
        public string VerifyPinChangeResponse;
        public string VerifyPinResponse;
        public string HsmHeader { get; set; }
        public string TransactionRefrenceNumber;
    }

    public class SSM
    {
        CommanDetails _CommanDetails = new CommanDetails();
        HsmCommunicationChanel _HsmCommunicationChanel = new HsmCommunicationChanel();
        private HsmCommunicationChanel _hsmCommunincationChanel = null;
        public HsmCommunicationChanel HsmCommunincationChanel
        {
            get
            {
                if (_hsmCommunincationChanel == null)
                {
                    _hsmCommunincationChanel = new HsmCommunicationChanel();
                }
                return _hsmCommunincationChanel;
            }
            set { _hsmCommunincationChanel = value; }
        }

        public static string ANSITODeibold(string strCardnumber, string strclearTPK, string strPIN)
        {

            string clearTPK = strclearTPK;
            string cardNumber = strCardnumber;
            string acct = cardNumber.Substring(cardNumber.Length - 12 - 1, 12);
            string newPinBlock = ThalesSim.Core.PIN.PINBlockFormat.ToPINBlock(strPIN, acct, ThalesSim.Core.PIN.PINBlockFormat.PIN_Block_Format.Diebold);
            string encPinBlock = ThalesSim.Core.Cryptography.TripleDES.TripleDESEncrypt(new HexKey(clearTPK), newPinBlock);
            return encPinBlock;
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

        public string XORHexStringsFull(string TMK1, string TMK2)
        {


            string result = string.Empty;
            for (int i = 0; i < TMK1.Length; i++)
            {
                result = result + (Convert.ToInt32(TMK1.Substring(i, 1), 16) ^ Convert.ToInt32(TMK2.Substring(i, 1), 16)).ToString("X");
            }
            return result;
        }
        
        public string GetClearTMK(string XOR, string EncrypedTMK)
        {
            return ThalesSim.Core.Cryptography.TripleDES.TripleDESDecrypt(new ThalesSim.Core.Cryptography.HexKey(XOR), EncrypedTMK);
        }

        public string GetPINBLock(string Cardnumber, string ClearTPK, string PIN)
        {
            return SSM.ANSITODeibold(Cardnumber, ClearTPK, PIN);
        }

        public bool ResetMpin(ref Authentication RequestMsg, ref MOBILEPORTAL_REQ _MOBILEPORTAL_REQ, out string PIN)
        {
            PIN = string.Empty;
            try
            {
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {
                    RequestMsg.TransactionRefrenceNumber = _MOBILEPORTAL_REQ.ReferenceNumber;
                    PIN = GetUniqueKey(4);
                    RequestMsg.CommandType = enumPINCommandType.SetPin;
                    string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                    RequestMsg.PinChangeBlock = GetPINBLock(_MOBILEPORTAL_REQ.ACCOUNTNUMBER.PadLeft(16, '0'), ClearTMK, PIN);
                    RequestMsg.Cardnumber = _MOBILEPORTAL_REQ.ACCOUNTNUMBER.PadLeft(16, '0');
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM : " + RequestMsg.TransactionRefrenceNumber);
                    _HsmCommunicationChanel.GenerateOffsetFormPAY(ref RequestMsg);
                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        return true;
                    }
                    else
                    {
                        RequestMsg.NewOffset = string.Empty;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }

        public bool GetOffset(ref Authentication RequestMsg, ref REGISTRATION_REQ _REGISTRATION_REQ, out string PIN)
        {
            PIN = string.Empty;
            try
            {
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {
                    RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                    PIN = GetUniqueKey(4);
                    //_CommanDetails.SystemLogger.WriteTransLog(this, "Transaction  pin : " +PIN);
                    RequestMsg.CommandType = enumPINCommandType.SetPin;
                    string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                    RequestMsg.PinChangeBlock = GetPINBLock(_REGISTRATION_REQ.AccountNumber.PadLeft(16, '0'), ClearTMK, PIN);
                    //_CommanDetails.SystemLogger.WriteTransLog(this, "cleartmk : " + ClearTMK + " pinblock : " + RequestMsg.PinChangeBlock);
                    RequestMsg.Cardnumber = _REGISTRATION_REQ.AccountNumber.PadLeft(16, '0');
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM : " + RequestMsg.TransactionRefrenceNumber);
                    _HsmCommunicationChanel.GenerateOffsetFormPAY(ref RequestMsg);
                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        return true;
                    }
                    else
                    {
                        _REGISTRATION_REQ.PINOFFSET = string.Empty;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }

        public bool ChangePin(ref Authentication RequestMsg, ref REGISTRATION_REQ _REGISTRATION_REQ, out string PIN)
        {
            PIN = string.Empty;
            try
            {
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {
                    RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                    PIN = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.NewMPIN.Trim(), RequestMsg.SsmMasterKey);
                    RequestMsg.CommandType = enumPINCommandType.SetPin;
                    string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);

                    RequestMsg.PinChangeBlock = GetPINBLock(_REGISTRATION_REQ.AccountNumber.PadLeft(16, '0'), ClearTMK, PIN);

                   // RequestMsg.PinBlock = GetPINBLock(AccountNumber.PadLeft(16, '0'), ClearTMK, MaximusAESEncryption.DecryptString(mPIN.Trim(), RequestMsg.SsmMasterKey));

                    RequestMsg.Cardnumber = _REGISTRATION_REQ.AccountNumber.PadLeft(16, '0');
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM : " + RequestMsg.TransactionRefrenceNumber);

                    _HsmCommunicationChanel.GenerateOffsetFormPAY(ref RequestMsg);
                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        return true;
                    }
                    else
                    {
                        _REGISTRATION_REQ.PINOFFSET = string.Empty;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }

        public bool GenrationGreenPin(ref Authentication RequestMsg, ref REGISTRATION_REQ _REGISTRATION_REQ, out string PIN)
        {
            PIN = string.Empty;
            try
            {
                //encryptedTMK = Convert.ToString(_getKeysTable.Rows[0][0]);--
                //Comkey = Convert.ToString(_getKeysTable.Rows[0][1]);--
                //Pvk = Convert.ToString(_getKeysTable.Rows[0][2]);--
                //Zpk = Convert.ToString(_getKeysTable.Rows[0][3]);--
                //HsmCvk = Convert.ToString(_getKeysTable.Rows[0][4]); --till

                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "GetKeys failed");
                    return false;

                }
                else
                {

                    /*value to assing */
                    _CommanDetails.SystemLogger.WriteTransLog(this, "GetKeys Success");
                    RequestMsg.HsmCvk = (RequestMsg.HsmCvv1 + RequestMsg.HsmCvv1).ToString();

                    _CommanDetails.SystemLogger.WriteTransLog(this, "RequestMsg.HsmCvk  " + RequestMsg.HsmCvk);

                    RequestMsg.DecryptedCardNo = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.CARDNUMBER.Trim(), RequestMsg.SsmMasterKey);
                    _CommanDetails.SystemLogger.WriteTransLog(this, " RequestMsg.DecryptedCardNo " + RequestMsg.DecryptedCardNo);

                    RequestMsg.CardExp = _REGISTRATION_REQ.CARDEXP.Trim();
                    _CommanDetails.SystemLogger.WriteTransLog(this, "RequestMsg.CardExp  " + RequestMsg.CardExp);

                    //RequestMsg.CardCVV = _REGISTRATION_REQ.CARDCVV.Trim();
                    //_CommanDetails.SystemLogger.WriteTransLog(this, "RequestMsg.CardCVV   " + RequestMsg.CardCVV);

                    RequestMsg.NewAtmPin = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.ATMPIN.Trim(), RequestMsg.SsmMasterKey);
                   // _CommanDetails.SystemLogger.WriteTransLog(this, " RequestMsg.NewAtmPin  " + RequestMsg.NewAtmPin);

                    RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                    _CommanDetails.SystemLogger.WriteTransLog(this, "RequestMsg.TransactionRefrenceNumber " + RequestMsg.TransactionRefrenceNumber);

                    RequestMsg.CommandType = enumPINCommandType.SetATMPin;



                    //PIN = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.NewMPIN.Trim(), RequestMsg.SsmMasterKey);
                    //string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                    //RequestMsg.PinChangeBlock = GetPINBLock(_REGISTRATION_REQ.AccountNumber.PadLeft(16, '0'), ClearTMK, PIN);
                    //RequestMsg.Cardnumber = _REGISTRATION_REQ.AccountNumber.PadLeft(16, '0');

                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM : " + RequestMsg.TransactionRefrenceNumber);

                    _HsmCommunicationChanel.GenerateGreenPinFormPAY(ref RequestMsg);

                    //_CommanDetails.SystemLogger.WriteTransLog(this, "New Pin : " + RequestMsg.NewOffset);

                    _CommanDetails.SystemLogger.WriteTransLog(this, "GenerateGreenPinForDrukPay  Done " + RequestMsg.TransactionStatus);

                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "RequestMsg.TransactionStatus :" + RequestMsg.TransactionStatus);
                        return true;
                    }
                    else
                    {
                        _REGISTRATION_REQ.PINOFFSET = string.Empty;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }

        public bool GetkeyDetails(ref Authentication RequestMsg, ref REGISTRATION_REQ _REGISTRATION_REQ)
        {
            
            try
            {
                //encryptedTMK = Convert.ToString(_getKeysTable.Rows[0][0]);--
                //Comkey = Convert.ToString(_getKeysTable.Rows[0][1]);--
                //Pvk = Convert.ToString(_getKeysTable.Rows[0][2]);--
                //Zpk = Convert.ToString(_getKeysTable.Rows[0][3]);--
                //HsmCvk = Convert.ToString(_getKeysTable.Rows[0][4]); --till

                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {

                    /*value to assing */

                    RequestMsg.HsmCvk = (RequestMsg.HsmCvv1 + RequestMsg.HsmCvv1).ToString();
                    RequestMsg.DecryptedCardNo = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.CARDNUMBER.Trim(), RequestMsg.SsmMasterKey);
                    RequestMsg.CardExp = _REGISTRATION_REQ.CARDEXP.Trim();
                   // RequestMsg.CardCVV = _REGISTRATION_REQ.CARDCVV.Trim();
                    RequestMsg.NewAtmPin = MaximusAESEncryption.DecryptString(_REGISTRATION_REQ.ATMPIN.Trim(), RequestMsg.SsmMasterKey);
                    RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                    RequestMsg.CommandType = enumPINCommandType.SetATMPin;

                    return true;
                  
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }
        
        public bool VerifyPin(ref Authentication RequestMsg, string AccountNumber, string mPIN, string DeviceID)
        {
            try
            {
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {
                    CONFIGURATIONCONFIGDATA.GET_OFFSET(DeviceID, AccountNumber, ref RequestMsg.PinOffset);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("PinOffset IS : {0} for AccountNumber : {1} ", RequestMsg.PinOffset, AccountNumber));
                    RequestMsg.CommandType = enumPINCommandType.VerifyPin;
                    string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                    RequestMsg.PinBlock = GetPINBLock(AccountNumber.PadLeft(16, '0'), ClearTMK, MaximusAESEncryption.DecryptString(mPIN.Trim(), RequestMsg.SsmMasterKey));
                   // RequestMsg.PinBlock = GetPINBLock(AccountNumber.PadLeft(16, '0'), ClearTMK,mPIN);//for plain mpin
                    RequestMsg.Cardnumber = AccountNumber.PadLeft(16, '0');
                    _CommanDetails.SystemLogger.WriteTransLog(this, "cleartmk : " + ClearTMK + " pinblock : " + RequestMsg.PinBlock);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM For ReferenceNumber: " + RequestMsg.TransactionRefrenceNumber);
                    _HsmCommunicationChanel.Safenet_VerifymPAYPIN(ref RequestMsg);
                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }
        }

        public bool VerifyATMPin(ref Authentication RequestMsg, string[] Track2, string mPIN, string DeviceID,string CardExp,string AccountNumber)
        {
           
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "GetPINBLock Before1");
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {

                    return false;
                }
                else
                {
              
                    CONFIGURATIONCONFIGDATA.GET_ATMOFFSET(DeviceID, Track2[0], ref RequestMsg.PinOffset, CardExp, AccountNumber);

                    if (RequestMsg.PinOffset.ToString() == "0" || RequestMsg.PinOffset.ToString() == "")

                    {
                        
                        return false;
                    }
                    else
                    {
                       
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("PinOffset IS : {0} for CardNumber : {1} ", RequestMsg.PinOffset, Track2[0].Substring(0, 6) + (new string('X', Track2[0].Length - 10)) + Track2[0].Substring(Track2[0].Length - 4, 4)));
                        RequestMsg.CommandType = enumPINCommandType.VerifyPin;

                        string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                        //_CommanDetails.SystemLogger.WriteTransLog(this, " ClearTMK : " + ClearTMK.ToString());

                        //string ClearTMK ="B98AE07AF2B3BC687534A49D2354294C"; 

                        // GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                        //RequestMsg.PinChangeBlock = GetPINBLock(AccountNumber.PadLeft(16, '0'), ClearTMK, MaximusAESEncryption.DecryptString(mPIN.Trim(), RequestMsg.SsmMasterKey));

                        _CommanDetails.SystemLogger.WriteTransLog(this, "GetPINBLock Before");


                        RequestMsg.PinBlock = GetPINBLock(Track2[0].PadLeft(16, '0'), ClearTMK, MaximusAESEncryption.DecryptString(mPIN.Trim(), RequestMsg.SsmMasterKey));
                        //_CommanDetails.SystemLogger.WriteTransLog(this, "  _CommanDetails.SystemLogger.WriteTransLog(this : :  " + MaximusAESEncryption.DecryptString(mPIN.Trim(), RequestMsg.SsmMasterKey).ToString());
                        //_CommanDetails.SystemLogger.WriteTransLog(this, " RequestMsg.PinBlock : : " + RequestMsg.PinBlock.ToString());

                        //_CommanDetails.SystemLogger.WriteTransLog(this, "GetPINBLock After");
                        RequestMsg.Cardnumber = Track2[0].PadLeft(16, '0');
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM For ReferenceNumber: " + RequestMsg.TransactionRefrenceNumber);
                        _HsmCommunicationChanel.ProcessVerifyAtmPin(ref RequestMsg, Track2);


                        if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }
        }

        public bool ResetTpin(ref Authentication RequestMsg, ref REGISTRATION_REQ _REGISTRATION_REQ, out string PIN)
        {
            PIN = string.Empty;
            try
            {
                if (!CONFIGURATIONCONFIGDATA.GetKeys(ref RequestMsg.SsmComkey, ref RequestMsg.SsmMasterKey, ref RequestMsg.SsmPvk, ref RequestMsg.HsmZpk, ref RequestMsg.HsmPvk, ref RequestMsg.HsmComkey, ref RequestMsg.HsmCvv1, ref RequestMsg.HsmCvv2, ref RequestMsg.TmkEncryptedKey))
                {
                    return false;
                }
                else
                {
                    RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                    PIN = GetUniqueKey(4);
                    RequestMsg.CommandType = enumPINCommandType.SetPin;
                    string ClearTMK = GetClearTMK(XORHexStringsFull(RequestMsg.HsmCvv1, RequestMsg.HsmCvv2), RequestMsg.TmkEncryptedKey);
                    RequestMsg.PinChangeBlock = GetPINBLock(_REGISTRATION_REQ.AccountNumber.PadLeft(16, '0'), ClearTMK, PIN);
                    RequestMsg.Cardnumber = _REGISTRATION_REQ.AccountNumber.PadLeft(16, '0');
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction Sent To HSM : " + RequestMsg.TransactionRefrenceNumber);
                    _HsmCommunicationChanel.GenerateOffsetFormPAY(ref RequestMsg);
                    if (RequestMsg.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        return true;
                    }
                    else
                    {
                        RequestMsg.NewOffset = string.Empty;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                RequestMsg.TransactionStatus = enumTransactionStatus.UnSuccessful;
                return false;
            }

        }

    }
}