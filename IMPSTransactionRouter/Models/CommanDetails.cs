using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxiSwitch.DALC.Configuration;
using System.Data;
using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.ConsumerTransactions;
using DbNetLink.Data;

namespace IMPSTransactionRouter.Models
{
    public class CommanDetails
    {
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;

        public CommanDetails()
        {
            try
            {
                SystemLogger = new SystemLogger();
                CommonLogger = new CommonLogger();
            }
            catch { }
        }
        private static ConfigurationData _dalConfigurationData;
        public static ConfigurationData DalConfigurationData
        {
            get
            {
                if (_dalConfigurationData == null)
                    _dalConfigurationData = new ConfigurationData();
                return _dalConfigurationData;
            }
            set { _dalConfigurationData = value; }
        }

        static List<BankDetails> _BankDetails = null;
        public static List<BankDetails> BankDetails
        {
            get
            {
                if (_BankDetails == null)
                {
                    try
                    {
                        bool Type = false;
                        _BankDetails = new List<BankDetails>();
                        DataTable DTResponseCodes = IMPSTransactions.GetBankDetails(Type);
                        foreach (DataRow row in DTResponseCodes.Rows)
                        {
                            BankDetails _BankDetailss = new BankDetails()
                            {
                                BankCode = row[0].ToString(),
                                BankName = row[1].ToString(),
                            };
                            _BankDetails.Add(_BankDetailss);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _BankDetails;
            }
            set { _BankDetails = value; }
        }

        static List<ResponseCodeDetails> _responceCodes = null;
        public static List<ResponseCodeDetails> ResponceCodes
        {
            get
            {
                if (_responceCodes == null)
                {
                    try
                    {
                        _responceCodes = new List<ResponseCodeDetails>();
                        DataTable DTResponseCodes = DalConfigurationData.SelectResponseCodes();
                        foreach (DataRow row in DTResponseCodes.Rows)
                        {
                            ResponseCodeDetails _responseCode = new ResponseCodeDetails()
                            {
                                ResponseCode = row[4].ToString(),
                                ErrorMessage = row[1].ToString(),
                                ResponseCodeHOST = row[0].ToString(),
                            };
                            _responceCodes.Add(_responseCode);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _responceCodes;
            }
            set { _responceCodes = value; }
        }

        private static List<TransactionProcessingCode> _processingcode = null;

        private static List<TransactionProcessingCode> Processingcode
        {
            get
            {
                if (_processingcode == null)
                {
                    try
                    {
                        _processingcode = new List<TransactionProcessingCode>();
                        DataTable DtProcessingCode = IMPSTransactions.GetProcessingCode();
                        if (DtProcessingCode.Rows.Count > 0)
                        {
                            foreach (DataRow row in DtProcessingCode.Rows)
                            {
                                TransactionProcessingCode _ProcessingCode = new TransactionProcessingCode()
                                {
                                    //ProcessingCodeId = (int)row[0],
                                    AccountType = row[1].ToString(),
                                    TransactionType = row[2].ToString(),
                                    TransactionMode = row[3].ToString(),
                                    CardScheme = row[4].ToString(),
                                    ProcessingCode = row[5].ToString(),
                                };
                                _processingcode.Add(_ProcessingCode);
                            }

                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _processingcode;
            }
            set { _processingcode = value; }
        }        

        public static string GetResponseCodeDescription(string _responseCode)
        {
            string Msg = string.Empty;
            try
            {
                Msg = (from RCode in ResponceCodes.AsParallel()
                       where RCode.ResponseCodeHOST == _responseCode
                       select RCode.ErrorMessage).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(Msg))
                Msg = "UNABLE TO PROCESS";

            return Msg;
        }

        public static string GetResponseCode(int EnumResponseCode)
        {
            string RespCode = (from RCode in ResponceCodes.AsParallel()
                               where RCode.ResponseCodeEnum == (enumResponseCode)EnumResponseCode
                               select RCode.ResponseCode).FirstOrDefault();

            if (string.IsNullOrEmpty(RespCode))
                RespCode = "91";

            return RespCode;
        }

        //public static enumResponseCode GetResponseCodeEnum(string ResponseCode)
        //{
        //    enumResponseCode ResponseCodeEnum = (from RCode in ResponceCodes.AsParallel()
        //                                         where RCode.ResponseCode == ResponseCode
        //                                         select RCode.ResponseCodeEnum).FirstOrDefault();
        //    return ResponseCodeEnum;
        //}

        ////////// Host Response Code /////////////////
        public static string GetResponseCodeHost(string EnumResponseCode)
        {
            string RespCode = (from RCode in ResponceCodes.AsParallel()
                               where RCode.ResponseCodeHOST == EnumResponseCode
                               select RCode.ResponseCode).FirstOrDefault();

            if (string.IsNullOrEmpty(RespCode))
                RespCode = "91";

            return RespCode;
        }


        public static string GetResponseCodeDescriptionHost(string _responseCode)
        {
            string Msg = string.Empty;
            try
            {
                Msg = (from RCode in ResponceCodes.AsParallel()
                       where RCode.ResponseCode == _responseCode
                       select RCode.ErrorMessage).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(Msg))
                Msg = "UNABLE TO PROCESS";

            return Msg;
        }


        public static string GetBankName(string bankcode)
        {
            string Msg = string.Empty;
            try
            {
                Msg = (from RCode in BankDetails.AsParallel()
                       where RCode.BankCode == bankcode
                       select RCode.BankName).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(Msg))
                Msg = "NA";

            return Msg;
        }

        ////**************For Getting Transaction Processing Code

        public static string GetProcessingCode(int AccountType, int TransactionType, int TransactionMode, int CardScheme)
        {
            string ProcessingCode = (from ProcessCode in Processingcode.AsParallel()
                                     where ProcessCode.AccountType == AccountType.ToString()
                                     && ProcessCode.TransactionType == TransactionType.ToString()
                                     && ProcessCode.TransactionMode == TransactionMode.ToString()
                                     && ProcessCode.CardScheme == CardScheme.ToString()
                                     select ProcessCode.ProcessingCode.ToString()).FirstOrDefault();
            return ProcessingCode;
        }

        /*MFS API (start)*/

        static List<VoiceDetails> _VoiceCodes = null;
        public static List<VoiceDetails> VoiceCodes
        {
            get
            {
                if (_VoiceCodes == null)
                {
                    try
                    {
                        _VoiceCodes = new List<VoiceDetails>();
                        DataTable DTVoiceCodes = DalConfigurationData.SelectVoiceCodes();
                        foreach (DataRow row in DTVoiceCodes.Rows)
                        {
                            VoiceDetails _VoiceCode = new VoiceDetails()
                            {
                                ID = row[0].ToString(),
                                Voice1 = row[1].ToString(),
                                Voice2 = row[2].ToString(),
                                Package = row[3].ToString(),
                                VoiceString = row[4].ToString(),

                            };
                            _VoiceCodes.Add(_VoiceCode);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _VoiceCodes;
            }
            set { _VoiceCodes = value; }
        }
        public static string GetVoiceCodeHost(string ID)
        {
            string RespCode = (from RCode in VoiceCodes.AsParallel()
                               where RCode.ID == ID
                               select RCode.VoiceString).FirstOrDefault();

            if (string.IsNullOrEmpty(RespCode))
                RespCode = "91";

            return RespCode;
        }


        static List<MFSResponseCodeDetails> _MFSresponceCodes = null;
        public static List<MFSResponseCodeDetails> MFSResponceCodes
        {
            get
            {
                if (_MFSresponceCodes == null)
                {
                    try
                    {
                        _MFSresponceCodes = new List<MFSResponseCodeDetails>();
                        DataTable DTMFSResponseCodes = DalConfigurationData.SelectMFSResponseCodes();
                        foreach (DataRow row in DTMFSResponseCodes.Rows)
                        {
                            MFSResponseCodeDetails _MFSresponseCode = new MFSResponseCodeDetails()
                            {
                                ResponseCode = row[4].ToString(),
                                ErrorMessage = row[1].ToString(),
                                ResponseCodeHOST = row[0].ToString(),
                                Action = row[5].ToString(),
                            };
                            _MFSresponceCodes.Add(_MFSresponseCode);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return _MFSresponceCodes;
            }
            set { _MFSresponceCodes = value; }
        }
        public class MFSResponseCodeDetails
        {
            public string ResponseCode = string.Empty;
            public string ErrorMessage = string.Empty;
            public enumResponseCode ResponseCodeEnum;
            public string ResponseCodeHOST;
            public string Action = string.Empty;



        }
        public static string GetMFSResponseCodeHost(string EnumResponseCode)
        {
            string RespCode = (from RCode in MFSResponceCodes.AsParallel()
                               where RCode.ResponseCodeHOST == EnumResponseCode
                               select RCode.Action).FirstOrDefault();



            if (string.IsNullOrEmpty(RespCode))
                RespCode = "91";



            return RespCode;
        }
        public static string GetMFSResponseCodeDescription(string _responseCode)
        {
            string Msg = string.Empty;
            try
            {
                Msg = (from RCode in MFSResponceCodes.AsParallel()
                       where RCode.ResponseCodeHOST == _responseCode
                       select RCode.ErrorMessage).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(Msg))
                Msg = "UNABLE TO PROCESS";



            return Msg;
        }






        /*MFS API (End)*/


    }
    public class ResponseCodeDetails
    {
        public string ResponseCode = string.Empty;
        public string ErrorMessage = string.Empty;
        public enumResponseCode ResponseCodeEnum;
        public string ResponseCodeHOST;
    }
    public class BankDetails
    {
        public string BankCode = string.Empty;
        public string BankName = string.Empty;
    }
    public class TransactionProcessingCode
    {
        public string AccountType { get; set; }
        public string TransactionType { get; set; }
        public string TransactionMode { get; set; }
        public string CardScheme { get; set; }
        public string ProcessingCode { get; set; }
    }

    public class VoiceDetails
    {
        public string ID = string.Empty;
        public string Voice1 = string.Empty;
        public string Voice2 = string.Empty;
        public string Package = string.Empty;
        public string VoiceString = string.Empty;



    }

}