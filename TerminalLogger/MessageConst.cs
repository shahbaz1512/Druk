using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiSwitch.Common.cons
{
    public class ConstMessageType
    {
        public const char FS = '\x001c';
        public const string SolicitedMesg = "22";
        public const string UnsolicitedMesg = "12";
        public const string ConsumerRequestMesg = "11";
        public const string OtherSolicitedMesg = "23";
        public const short GS = 29;   
        public const short RS = 30;

        public const string CurrentRow = "[CR]";
        public const string LineFeed = "[LF]";
        public const string OneLine = "[LF][CR]";
        public const string TwoLine = "[LF][LF][CR]";
        public const string ThreeLine = "[LF][LF][LF][CR]";
        public const string CutDeliverPrintOut = "[FF]";


        public const short NumberofDecimalPlace = 2;
        public const short MaximusAmountWidthOnPrint = 15;
        public const short MaximusAmountWidthOnScreen = 14;
        public const short MaximusPaperSize = 36;

        ////******Printer Flags***************
        public const string DoNotPrint = "0";
        public const string PrintOnJournalPrinter = "1";
        public const string PrintOnCustomerPrinter = "2";
        public const string PrintOnBothPrinter = "3";
        public const string PrintOnDeposit = "4";

    }

    
    public class ConstDeviceStatusDescriptionType
    {
        public const string DeviceFaultOrConfigInfo = "8";
        public const string Ready9 = "9";
        public const string ReadyB = "B";
        public const string GeneralCommandReject = "A";
        public const string SpecificMACCommandReject = "C";
        public const string TerminalState = "F";
    }

    public class ConstStatusInformations
    {
        public const string SendConfigurationInformation = "1";
        public const string SendSupplyInformation = "2";
        public const string SendErrorLogInformation = "4";
        public const string SendDateTimeInformation = "5";
        public const string SendConfigurationIDInformation = "6";
        public const string HardwareConfigurationData = "H";
        public const string SuppliesData = "I";
        public const string FitnessData = "J";
        public const string TamperAndSensorStatusData = "K";
        public const string SoftwareIDAndReleaseNumber = "L";
        public const string LocalConfigurationOptionDigits = "M";

        public const string DDCConfigurationInformation = "<";
        public const string DDCSupplyStatus = "H";
        public const string DDCHardwareConfiguration = "B";
        public const string DDCCheckPointStatus = "C";
        public const string DDCCassetteConfigurationStatus = "D";
    }

    public class ConstTerminalCommand
    {
        public const string TerminalCommandMsgID = "1";
        public const string TerminalResponseFlag = "0";
        public const string MsgSequenceNumber = "000";
        public const string TerminalDownloadCommandID = "3";
        public const char DownloadDelimiter = '#';
        public const int MaxMsgLength = 500;
        public const string TerminalFunctionCommandIdentifier = "4";
        public const string TerminalBufferIdentifier = "4";
        public static string TerminalTrack3Data = "4" + new string('0', 0x63);
        public const string SingleKeyLength = "32";
        public const string DoubleKeyLength = "42";
        public const string DoubleKeyLengthData = "030";
    }

    public class AccountType
    {
        public const string BalanceEnquiry = "INQ-BALQ";
        public const string Withdrawal = "WDL-NORM";
        public const string FastCash = "WDL-FAST";
        public const string MiniStatement = "REQ-MINI";
        public const string PinChange = "PIN-CHANGE";
        public const string FundTransfer = "FT";
        public const string Other = "OTH";
        public const string DepositCashCheque = "DEP-AMT";
        public const string BillPayment = "BILL-PMT";
        public const string PrintCounter = "PRINT-COUNTER";
        public const string MBR = "MBR";
    }

    public class OtherRequest
    {
        public const string FundTransfer = "FT";
        public const string MBR = "MBR";
        public const string AadharSeeding = "ANS";
        public const string StatementRequest = "STRQ";
        public const string ChequeRequest = "CHQRQ";
        public const string FastCash = "WDL-FAST";
        public const string MobileTopUp = "MTP";
        public const string OtpGeneration = "OTP";
        public const string SetPin = "SETPIN";
    }

    public class LogFilePath
    {
        public static readonly string RootPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SystemLog");
        public static readonly string TerminalLogFilePath = Path.Combine(RootPath, "TerminalsLog");
        public static readonly string TransactionLogPath = Path.Combine(RootPath, "TransactionLog");
        public static readonly string HSMTransactionLogPath = Path.Combine(RootPath, "HsmLogs");
        public static readonly string TraceLogPath = Path.Combine(RootPath, "TraceLog");        
        public static readonly string ErrorLogPath = Path.Combine(RootPath, "ErrorLog");
    }

    public class CommmonLogFilePath
    {
        public static readonly string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLog");
        public static readonly string TransactionLogPath = Path.Combine(RootPath, "TransactionLog");
        public static readonly string CommanTransactionLogPath = Path.Combine(RootPath, "");
        public static readonly string ErrorLogPath = Path.Combine(RootPath, "ErrorLog");
        public static readonly string AuditTrailLogPath = Path.Combine(RootPath, "AuditTrailLog");
    }
}
