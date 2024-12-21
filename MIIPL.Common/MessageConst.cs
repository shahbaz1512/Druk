using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIIPL.Common
{
    public class ThalesHsmCommandCode
    {
        public const string TerminalPinCommandCode = "DA";
        public const string TerminalPinTransformationCommandCode = "CA";
        public const string InterchangePinCommandCode = "EA";
        public const string CardVerifyCommandCode = "CY";
        public const string GetTerminalOffsetCommandCode = "DU";
        public const string GetInterchangeRandomPinCommandCode = "JE";
        public const string GetInterchangeIBMOffsetCommandCode = "BK";
        public const string SessionKeyCommandCode = "HC";
        public const string ImportKey = "A6";
        public const string ExportKey = "A8";
    }

    public class ThalesHsmCommandConst
    {
        public const string ThalesHSMStatusCode = "NO";
        public const string SessionKeyConst = ";XU0";
        public const string LMKIdentifier = "00";
    }

    public class ThalesMessageConst
    {
        public const string DefaultCheckLength = "04";
        public const string DefaultAcquirerCheckLength = "06";
        public const string DecimalizationTable = "0123456789012345";
        public const string DefaultDelimeter = "*";
        public const string DefaultExcludePinCount = "00";
        public const string DefaultMaxPinLength = "12";
        public const string DieboldPinBlockFormat = "03";   ////Diebold  PinBlock Format
        public const string AnsiPinBlockFormat = "01";   ////American National Standard Institution PinBlock Format
        public const string PinBlockKeyZPK_LMK = "001";  //'001' : ZPK encrypted under LMK 06-07
        public const string PinBlockKeyTPK_LMK = "002";  //'002' : TPK encrypted under LMK 14-15
        public const string ThalesHsmStatusMode = "00";        
    }

    public class ThalesKeySchemes
    {
        public const string EncryptingKeysUnderVariantLMK_Double = "U";
        public const string EncryptingKeysUnderVariantLMK_Triple = "T";
        public const string EncryptingKeysANSIX917Method_Double = "X";
        public const string EncryptingKeysANSIX917Method_Triple = "Y";
        public const string EncryptingKeysVerifone_Giske_Des = "V";
        public const string EncryptingKeysUnderKeyblockLMK = "S";
    }

    public class LogFilePath 
    {
        public static readonly string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLog");       
        public static readonly string TransactionLogPath = Path.Combine(RootPath, "TransactionLog");        
        public static readonly string ErrorLogPath = Path.Combine(RootPath, "ErrorLog");
    }

    public class AuditLogFilePath
    {
        public static readonly string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLog");
        public static readonly string AuditTrailLogPath = Path.Combine(RootPath, "AuditTrailLog");
    }

    public class SafenetHSMFunctionCode
    {
        public const string HSMErrorLogStatus = "FFF0";
        public const string TerminalMasterKeyGeneration = "EE0E01";
        public const string CommKeyGenration = "49";
        public const string PINTranslation = "EE0602";
        public const string HsmStatus = "01";
        public const string HSMErrorstatus = "FFF0";
        public const string BDKFunction = "EE0408";
        public const string PINEncryption = "EE0600";
        public const string Initial_Session_Key = "EE0400";
        public const string KVC = "EEBF29";
        public const string KeyMailer = "EE0E01";
        public const string Get_Key_Details = "EE0202";
        public const string Initial_Session_Key_Interchange = "EE0402";
        public const string Random_Key_Gen = "EF0618";
        public const string PINVerification = "EE0603";
        public const string PINOffsetGeneration = "EE0604";
        public const string CVVVerification = "EE0803";
        public const string ZmkSessionKey = "EE0200";
    }

    public class SafenetMessageConst
    {
        public const string DefaultCheckLength = "04";
        public const string DefaultAcquirerCheckLength = "06";
        public const string DecimalizationTable = "0123456789012345";
        public const string DefaultDelimeter = "*";
        public const string DefaultExcludePinCount = "00";
        public const string DefaultMaxPinLength = "12";
        public const string DieboldPinBlockFormat = "03";   ////Diebold  PinBlock Format
        public const string AnsiPinBlockFormat = "01";   ////American National Standard Institution PinBlock Format
        public const string PinBlockKeyZPK_LMK = "001";  //'001' : ZPK encrypted under LMK 06-07
        public const string PinBlockKeyTPK_LMK = "002";  //'002' : TPK encrypted under LMK 14-15        
        public const string KeyVersionNumber = "02";
        public const string DoNotPad = "00";
        ////***************HSM Index***************************
        public const string HsmIndex = "0200";
    }

    public class SafenetHSMPinBlockFormat
    {
        public const string ANSI = "01";
        public const string ZKA = "09";
        public const string Docutel2 = "02";
        public const string Docutel = "08";
        public const string PINPadDiebold = "03";
        public const string ISO0 = "10";
        public const string ISO1 = "11";
        public const string ISO2 = "12";
        public const string ISO3 = "13";
    }

    public class SafenetKeySpecifier
    {
        public const string EncSingleLength = "10";
        public const string EncDoubleLength_ECB = "11";
        public const string EncTripleLength_ECB = "12";
        public const string EncDoubleLength_CBC = "13";
        public const string EncTripleLength_CBC = "14";
    }

    public class SafenetHSMKeyType
    {
        public const string DPK = "00";
        public const string PPK = "01";
        public const string MPK = "02";
        public const string KIS = "03";
        public const string KIR = "04";
        public const string KTM = "05";
        public const string KPVV = "08";
        public const string KCVV = "09";
        public const string BDK = "24";
    }

    public class SafenetHSMHeader
    {
        public const string SOH = "01";
        public const string Version = "01";
        static int sequenceNo = 1;
        public static int SequenceNo
        {
            get
            {
                if (sequenceNo == 0 || sequenceNo >= 9998)
                { sequenceNo = 1; }
                return sequenceNo;
            }
            set
            { sequenceNo = value; }
        }

    }

    public class SafenetHSMFunctionModifier
    {
        public const string FM = "00";
    }    
}
