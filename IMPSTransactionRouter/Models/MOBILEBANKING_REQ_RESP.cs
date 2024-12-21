using MaxiSwitch.Common.TerminalLogger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace IMPSTransactionRouter.Models
{
    public class MOBILEBANKING_REQ
    {
        public string DeviceID { get; set; }
        public string MobileNumber { get; set; }
        public string REMITTERACC { get; set; }
        public string REMITTERNAME { get; set; }
        public decimal TXNAMT { get; set; }
        public string BENIFICIARYACC { get; set; }
        public string BENIFICIARYNAME { get; set; }
        public string BenificiaryNickName { get; set; }
        public string BENIFICIARYMOBILE { get; set; }
        public string BENIFICIARYMOBILENUMBER { get; set; }
        public string ReferenceNumber { get; set; }
        public int BankCode { get; set; }
        public bool IsMobileFT { get; set; }
        public bool IsAccountFT { get; set; }
        public string Remark { get; set; }
        public string DeviceLocation { get; set; }
        public string mPIN { get; set; }
        public string CREDITCARDACC { get; set; }
        public string MailID { get; set; }
        public string RemitterMailID { get; set; }
        public string BRANCH_CODE { get; set; }
        public string CUST_AC_NO { get; set; }
        public string SOURCE { get; set; }
        public string UBSCOMP { get; set; }
        public string CORRELID { get; set; }
        public string USERID { get; set; }
        public string CUSTOMERID { get; set; }
        public string BRANCH { get; set; }
        public string MODULEID { get; set; }
        public string SERVICE { get; set; }
        public string OPERATION { get; set; }
        public string MSGSTAT { get; set; }
        public string PRD { get; set; }
        public string BRN { get; set; }
        public string MODULE { get; set; }
        public string NARRATIVE { get; set; }
        public string MSGID { get; set; }
        public string TransactionRefrenceNumber { get; set; }
        public string DateTime { get; set; }
        public string SerializedMessage { get; set; }
        public string ResponseData { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public string TransType { get; set; }
        public bool IsBLQ { get; set; }
        public bool IsMini { get; set; }
        public bool IsCC { get; set; }
        public bool IsPayment { get; set; }
        public string AmountAvailable = string.Empty;
        public string FtLimit = string.Empty;
        public string AccountUseLimit = string.Empty;
        public string AccountUseCount = string.Empty;
        public string LastDate = string.Empty;
        public string LastTime = string.Empty;
        public string MaxPinCount = string.Empty;
        public string MaxPinUseCount = string.Empty;
        public string PinOffset = string.Empty;
        public DateTime LastDateTime { get; set; }
        public bool IsMerchant { get; set; }
        public string RechargeMobileNumber { get; set; }
        public string ConsumerNumber { get; set; }
        public string ConsumerName { get; set; }
        public string LeaseLineNumber { get; set; }
        public string MerchantMobileNumber { get; set; }
        public string CustomerBaseNumber { get; set; }
        public string LoanAccountNumber { get; set; }
        public decimal RDTD_Amount { get; set; }
        public string PRODUCTTYPE { get; set; }
        public string ACCOUNTCREATIONTYPE { get; set; }
        public string XREF { get; set; }
        public string AccountClass { get; set; }
        public string LoanHolderName { get; set; }
        public string LoanCustomerID { get; set; }
        public string TenurInDays { get; set; }
        public string WaterBillNumber { get; set; }
        public string NationalID { get; set; }
        public string RentMonth { get; set; }
        public string RentYear { get; set; }
        public string FlatNumber { get; set; }
        public string NPPFLoanAcc { get; set; }
        public string DONORID { get; set; }
        public string VOUCHERNUMBER { get; set; }
        public string LedgerEnterID { get; set; }
        public decimal PenaltiAmount { get; set; }
        public bool IsMisc { get; set; }
        public bool IFSC { get; set; }
        public bool REMITERIFSC { get; set; }
        public string SERIALNO { get; set; }
        public string BILLNO { get; set; }
        public string USID { get; set; }
        public string TPN { get; set; }
        public string DeliveryChannel { get; set; }
        public string StopSchedulerOn { get; set; }
        public int Frequency { get; set; }
        public string ACQAmountAvailable = string.Empty;
        public string ACQFtLimit = string.Empty;
        public string FundTransferType = string.Empty;
        public string RemarkfinalPayment = string.Empty;
        public string BNgulAmountAvailable = string.Empty;
        public string BNgulFtLimit = string.Empty;
        public string ShowID = string.Empty;
        public string ContestantID = string.Empty;
        public string ContestantsName = string.Empty;
        public string VoteCount = string.Empty;
        public string ProductCode = string.Empty;
        public string ContestantNumber = string.Empty;
        public string TxnRRN = string.Empty;
        public string GreenPin = string.Empty;
        public int ID { get; set; }
        //public string MerchantQRMobileNumber { get; set; }
        public string NQRCBANKID { get; set; }
        public string NQRCBankName { get; set; }
        public string AcquirerBankID { get; set; }
        public string ACQNQRCBankName { get; set; }
        public string ScheduledOn { get; set; }
        public string Payloadformatindicator = string.Empty;
        public string Pointofinitiationmethod = string.Empty;
        public string Merchantidentifier = string.Empty;
        public string MerchantCategoryCode = string.Empty;
        public string TransactionCurrencyCode = string.Empty;
        public string CountryCode = string.Empty;
        public string MerchantName = string.Empty;
        public string NQRCcity = string.Empty;
        public string AdditionalDataField = string.Empty;
        public string CRC = string.Empty;
        public string ProcessingCode = string.Empty;
        public string QRTYPE = string.Empty;
        public string QRValue = string.Empty;
        public string QRMSGID = string.Empty;
        public string QRUniquePANNumber = string.Empty;
        public string Amount = string.Empty;
        public string INDICATOR_CONVIENCY = string.Empty;
        public string FEE_CONVIENCY = string.Empty;
        public string CONVIENCYPERCENT = string.Empty;
        public string POSTALCODE = string.Empty;
        public string ADDITIONALDATA = string.Empty;
        public string RATECODE = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Flag { get; set; }
        public string TXNID { get; set; }
        public string PanEntryMode { get; set; }
        public string NextScheduleOn { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Account_Id { get; set; }
        public string PAN { get; set; }
        public string DateRange { get; set; }
        public bool IsAccountVerification { get; set; }
        public bool IsQRCodeVerification { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string LastTransactionReferenceNumber { get; set; }


    }

    public class MOBILEBANKING_RESP
    {
        public string MSGID { get; set; }
        public string REMITTERACC { get; set; }
        public string REMITTERNAME1 { get; set; }
        public string REMITTERNAME2 { get; set; }
        public decimal TXNAMT { get; set; }
        public string BENIFICIARYACC { get; set; }
        public string BENFNAME { get; set; }
        public string BENFADDR1 { get; set; }
        public string BENFADDR2 { get; set; }
        public string BENFADDR3 { get; set; }
        public string DeviceID { get; set; }
        public string MobileNumber { get; set; }
        public string DeviceLocation { get; set; }
        public string BRANCH_CODE { get; set; }
        public string CUST_AC_NO { get; set; }
        public string SOURCE { get; set; }
        public string UBSCOMP { get; set; }
        public string CORRELID { get; set; }
        public string USERID { get; set; }
        public string BRANCH { get; set; }
        public string MODULEID { get; set; }
        public string SERVICE { get; set; }
        public string OPERATION { get; set; }
        public string MSGSTAT { get; set; }
        public string PRD { get; set; }
        public string BRN { get; set; }
        public string MODULE { get; set; }
        public string DESTINATION { get; set; }
        public string MULTITRIPID { get; set; }
        public string FUNCTIONID { get; set; }
        public string ACTION { get; set; }
        public string NARRATIVE { get; set; }
        public string XREF { get; set; }
        public string FCCREF { get; set; }
        public string OFFSETCCY { get; set; }
        public string ReferenceNumber { get; set; }
        public int BranchCode { get; set; }
        public int BankCode { get; set; }
        public bool IsMobileFT { get; set; }
        public bool IsAccountFT { get; set; }
        public string TransactionRefrenceNumber { get; set; }
        public string DateTime { get; set; }
        public string SerializedMessage { get; set; }
        public string CCY { get; set; }
        public string MSGTYPE { get; set; }
        public string RECEIVER { get; set; }
        public string MSGSTATUS { get; set; }
        public string TRNDT { get; set; }
        public string ResponseData { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public string HOSTResponseCODE { get; set; }
        public string HOSTResponseDesc { get; set; }
        public string CurrentBalance { get; set; }
        public string OpeningBalance { get; set; }
        public string AvailableBalance { get; set; }
        public string MinistatementData { get; set; }
        public string HostWarningCode { get; set; }
        public string HostWarningDesc { get; set; }
        public string AmountAvailable { get; set; }
        public string FtLimit { get; set; }
        public string AccountUseLimit { get; set; }
        public string AccountUseCount { get; set; }
        public string LastDate { get; set; }
        public string LastTime { get; set; }
        public string MaxPinCount { get; set; }
        public string MaxPinUseCount { get; set; }
        public string PinOffset { get; set; }
        public string BTResponse { get; set; }
        public string ConsumerName { get; set; }
        public string OutstandingAmount { get; set; }
        public DataTable RecentTransactions { get; set; }
        public DataTable MinMaxData { get; set; }
        public DataTable ListOfLoanAccounts { get; set; }
        public DataTable ListOfCustomerID { get; set; }
        public string LoanHolderName { get; set; }
        public string LoanAccountNumber { get; set; }
        public string AmountFinanced { get; set; }
        public string PrincipalOutstanding { get; set; }
        public string InterestOutstanding { get; set; }
        public string PenalityAmount { get; set; }
        public string TotalOutstandingAmount { get; set; }
        public string LoanDisbursment { get; set; }
        public string MaturityDate { get; set; }
        public string OverDueDays { get; set; }
        public string EMIAMOUNT { get; set; }
        public string PrincipleOverDue { get; set; }
        public string InterestOverDue { get; set; }
        public string TotalOverDueAmt { get; set; }
        public string ExpectedInterest { get; set; }
        public string LoanInterestRate { get; set; }
        public decimal RDTD_Amount { get; set; }
        public string BTFUNCTIONID { get; set; }
        public DataTable RecurringTermDetails { get; set; }
        public DataTable NPPFLOANAC { get; set; }
        public string NationalID { get; set; }
        public string FlatCode { get; set; }
        public DataTable WaterDetails { get; set; }
        public string TotalAmount { get; set; }
        public string TotalPenalityAmount { get; set; }
        public string GrandTotal { get; set; }
        public string VoucherNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string DepartmentCode { get; set; }
        public string Premium { get; set; }
        public string AssuredValue { get; set; }
        public string SanctionAmount { get; set; }
        public string OverDueAmount { get; set; }
        public string SerialNumber { get; set; }
        public string TPN { get; set; }
        public BTPOSTPAIDRESPHEADER BTOUTSTANDINGDETAILS { get; set; }
        public string TXREFID { get; set; }
        public DataTable PLANS { get; set; }
        public DataTable INSTALLMENT { get; set; }
        public string MODEOFTXN { get; set; }
        public string CBSRefNumber { get; set; }
        public int ID { get; set; }
        public DataTable BankCodeDetails { get; set; }
        public string BankName { get; set; }
        public string ResponseDesc_IOS { get; set; }
        public DataTable DashboardDetails { get; set; }
        public string EmailID { get; set; }

        public string ScheduledPaymentList = string.Empty;

        public int Flag { get; set; }
        public string RechargeStatus { get; set; }
        public string AccountId { get; set; }

        public string BTStatusCode = string.Empty;
        public string BTTransStatus = string.Empty;
        public string BTStatusDiscription = string.Empty;
        public BTPREPAIDRESPHEADER BTPREPAIDRESPHEADER { get; set; }
        public string BHIMQRValue { get; set; }

        public string Indicator { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }

        public DataTable ImagesList { get; set; }
    }

    public class REGISTRATION_REQ
    {
        public string BenificiaryAccountNumber { get; set; }
        public string BenificiaryBankCode { get; set; }
        public string BenificiaryMobileNumber { get; set; }
        public string BenificiaryNickName { get; set; }
        public string ReferenceNumber { get; set; }
        public string DeviceID { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public bool IsAccountReg { get; set; }
        public bool IsMobileReg { get; set; }
        public bool allbenificiarydetails { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string MailID { get; set; }
        public string mPIN { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
        public string NewMPIN { get; set; }
        public string PINOFFSET { get; set; }
        public bool IsFundTrasfer { get; set; }
        public bool IsBLQ { get; set; }
        public bool IsMini { get; set; }
        public bool IsCC { get; set; }
        public bool IsCreditCard { get; set; }
        public bool IsPayment { get; set; }
        public bool IsCardless { get; set; }
        public bool IsManageAccount { get; set; }
        public bool IsWithinBank { get; set; }
        public bool IsOtherBank { get; set; }
        public string CARDNUMBER { get; set; }
        public string STRLIMITATM { get; set; }
        public string STRLIMITPOS { get; set; }
        public string STRLIMITECM { get; set; }
        public string STRSTATUSATM { get; set; }
        public string STRSTATUSPOS { get; set; }
        public string STRSTATUSECM { get; set; }
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
        public byte[] FrontImageData { get; set; }
        public byte[] BackImageData { get; set; }
        public string DepositAccountNumber { get; set; }
        public string DepositAmount { get; set; }
        public string ChequeNumber { get; set; }
        public double LoanAmount { get; set; }
        public double DepositAmt { get; set; }
        public double InterestRate { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        public bool IsLoanPayment { get; set; }
        public string NewAccountNumber { get; set; }
        public string NewMobileNumber { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string REMITTERACC { get; set; }
        public string REMITTERMOB { get; set; }
        public string CARDTYPE { get; set; }
        public string WDLAMOUNT { get; set; }
        public string ACCOUNTCREATIONTYPE { get; set; }
        public string PRODUCTTYPE { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsTerm { get; set; }
        public string CustomerBaseNumber { get; set; }
        public string LoanCustomerID { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string ChequeAccountNumber { get; set; }
        public string Text { get; set; }
        public string Reason { get; set; }
        public string ExpiredDate { get; set; }
        public string CVV { get; set; }

        public string TokenID { get; set; }
        public string DeviceType { get; set; }
        public string ChequeDate { get; set; }
        public string Amount { get; set; }

        public string Remark { get; set; }
        public string REMITTERNAME { get; set; }
        public string SHOWID { get; set; }
        public string VoteCount { get; set; }
        public string GreenPin { get; set; }
        public string CARDEXP { get; set; }
        public string CARDCVV { get; set; }
        public string ATMPIN { get; set; }
        public string flag { get; set; }
        public string CUST_AC_NO { get; set; }


        public string MerchantIdentifier { get; set; }
        public string SequecnceData { get; set; }
        public string typeofUser { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
        public string country { get; set; }
        public string City { get; set; }
        public string RATECODE { get; set; }

        public string RegMobileNumber { get; set; }
        public string NationalID { get; set; }
        public string OLDMobileNumber { get; set; }

        public string ChequeStartNumber { get; set; }
        public string ChequeEndNumber { get; set; }
        public string CHEQUEISSUEDATE { get; set; }
        ////***********Added on 19092020
        public string CitizenshipCardNo { get; set; }
        public string DOB { get; set; }
        public bool Isbiometric { get; set; }
        ///********Added on 06012020
        public string CitizenId { get; set; }
        public string PassportNumber { get; set; }
        public string WorkPermit { get; set; }
        public string LicenseNumber { get; set; }
        public int InfoType { get; set; }
        public string InfoValue { get; set; }
        public string AllAccounts { get; set; }
        public string BranchCode { get; set; }
        public bool IsMobileUpdate { get; set; }
        public bool ISACCFT { get; set; }
        public bool ISMOBFT { get; set; }
        public bool ISQRFT { get; set; }
        public bool ISALLNOTICIFATION { get; set; }
        public string ResponseDesc { get; set; }
        public string IsVersionUpdate { get; set; }
        
        public string Version { get; set; }

    }

    public class REGISTRATION_RES
    {
        public string BenificiaryAccountNumber { get; set; }
        public string BenificiaryBankCode { get; set; }
        public string BenificiaryMobileNumber { get; set; }
        public string BenificiaryNickName { get; set; }
        public string ReferenceNumber { get; set; }
        public string DeviceID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string UserID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public string AccountDetails { get; set; }
        public DataTable BankDetails { get; set; }
        public DataTable CustomerDetails { get; set; }
        public DataTable CBSTransDT { get; set; }
        public DataTable MonthlyTransDT { get; set; }
        public DataTable TransDT { get; set; }
        public DataTable ShowDetails { get; set; }
        public DataTable ContestantDetails { get; set; }
        public DataTable RequestmoneyBlockHistory { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CycleNumber { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public string EncryptionKey { get; set; }
        public string BankName { get; set; }
        public string QRValue { get; set; }
        ////// CARD CONTROLLER
        public string CardDetails { get; set; }
        public string CardStatus { get; set; }
        public string ActivityHistory { get; set; }
        public string ATMLIMIT { get; set; }
        public string POSLIMIT { get; set; }
        public string AmountAvailable { get; set; }
        public DataTable QRDATA { get; set; }
        public DataTable CheckUpdate { get; set; }
        public DataTable FxRate { get; set; }

        public double LoanAmount { get; set; }
        public string InterestRate { get; set; }
        public int Months { get; set; }
        public string MonthlyPayment { get; set; }
        public DataTable DtBrekegofloan { get; set; }
        public string TotalDepositAmount { get; set; }
        public string MaturityAmount { get; set; }
        public string MaturityDate { get; set; }
        public string InterestAmount { get; set; }
        public string AccountType { get; set; }
        public string Amount { get; set; }
        public string Remark { get; set; }
        public double TotAmount { get; set; }
        public double TotAsset { get; set; }
        public double TotLiablity { get; set; }
        public string REMITTERNAME { get; set; }
        public string NotificationCount { get; set; }
        public DataTable MinMaxData { get; set; }
        public DataTable ChequeBookblockList { get; set; }
        public DataTable ChequeBookReleseList { get; set; }
        public string EmailID { get; set; }
        public string BranchCode { get; set; }
        public string AccountStatus { get; set; }
        public string AutoLogout { get; set; }
        public string DateOfBirth { get; set; }
        public string CustomerGovermentID { get; set; }
        public DataTable AllAccountsQRList { get; set; }
        public string RoundValue { get; set; }

        public bool IsMobileUpdate { get; set; }

        public DataTable ContactDetails { get; set; }
        public DataTable TermsCondition { get; set; }

        public DataTable AboutUs { get; set; }

        public DataTable Transactionhistory { get; set; }
        public DataTable Otherhistory { get; set; }

        public string VersionUpdate { get; set; }

        public DataTable ImagesList { get; set; }
    }


    public class MOBILEPORTAL_REQ
    {
        public string ID { get; set; }
        public string LogedInUserID { get; set; }
        public string ReferenceNumber { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string CUSTOMERID { get; set; }
        public string MOBILENUMBER { get; set; }
        public string BRANCHCODE { get; set; }
        public string BRANCHLOCATION { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public bool ISBRANCH { get; set; }
        public bool ISATM { get; set; }
        public string PINOFFSET { get; set; }
        public string LoginPassword { get; set; }
        public string NewMpin { get; set; }
        public string MailID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }

        public string FLAG { get; set; }
        public string NewLimit { get; set; }
        public string ACQNewLimit { get; set; }
        public string TransShowDetails { get; set; }

        public string BNGULSTRTRANSFERLIMIT { get; set; }

        public string MerchantName { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantAccountNumber { get; set; }
        public string MerchantMobileNumber { get; set; }
        public string MerchantAddress { get; set; }
        public string MerchantBankCode { get; set; }
        public string MerchantCID { get; set; }
        public string MerchantBankName { get; set; }
        public bool IsMerchantActive { get; set; }
        public bool IsMerchantDeActive { get; set; }
        public string State { get; set; }
        public string ApproveStatus { get; set; }
        public string ChequeID { get; set; }
        public string DeclineReason { get; set; }

        public byte[] Images { get; set; }
        public int ImagesID { get; set; }

        public string MSGID { get; set; }
        public string CollerID { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TxnType { get; set; }
        public string BankCode { get; set; }
        public string Status { get; set; }
        public string EMAIL { get; set; }

        public string CUSTOMERNAME { get; set; }
        public string CUSTOMERPRIMARYACCOUNT { get; set; }
        public string CUSTOMERNEWACCOUNT { get; set; }
        public string CUSTOMERACCTYPE { get; set; }
        public string CUSTOMERMOBILENUMBER { get; set; }
        public string CUSTOMERCCY { get; set; }
        public bool ISSECONDARYACCUPDATEREQ { get; set; }
        public string DonorName { get; set; }
        public string ProductCode { get; set; }
        public bool ISUPDATE { get; set; }
        public bool ISSELECT { get; set; }
        public bool ISDELETE { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
        public string TransaferLimitAmount { get; set; }
        public string ContestantID { get; set; }
        public string ShowID { get; set; }
        public string ContestantName { get; set; }
        public string ContestantAge { get; set; }
        public string ContestantEmail { get; set; }
        public string ContestantState { get; set; }
        public string ContestantCity { get; set; }
        public string ContestantMobile { get; set; }
        public string ContestantImage { get; set; }
        public string ContestantNumber { get; set; }
        public string Image { get; set; }
        public string IsRemoved { get; set; }
        public string OtherDetails { get; set; }

        public string MerchantCategory { get; set; }
        public string MerchantCity { get; set; }
    }

    public class MOBILEPORTAL_RES
    {
        public DataTable RegisteredUsers { get; set; }
        public DataTable BRANCH_ATM_LOCATION { get; set; }
        public DataTable MerchantDetails { get; set; }
        public string ReferenceNumber { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }

        public DataTable BankDetails { get; set; }
        public DataTable ChequeDetails { get; set; }
        public DataTable ImagesList { get; set; }
        public DataTable ReportData { get; set; }
        public DataTable ResponseVotes { get; set; }
        public DataTable ReqReportData { get; set; }
        public DataTable DonorDetails { get; set; }
        public string QRDATA { get; set; }
        public DataTable ShowMaster { get; set; }
        public DataTable ContestantMaster { get; set; }
        public DataTable ResponseDetailVotes { get; set; }
        public DataTable RequestMoneyBlockList { get; set; }
        public DataTable OtherDetails { get; set; }
        public DataTable MerchantCategoryList { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }
    }

    public class TCELLPARAM
    {
        public static string Version = "1";
        public static string MessageSeq = "9025212562532522";
        public static string BEID = "101";
        //public static string LoginSystemCode = "BNB";
        //public static string Password = "BNab123$%";
        public static string LoginSystemCode = "102";
        public static string Password = "fZI2rQD0EhxBuXidSEuEMw==";
        public static string RemoteIP = "127.0.0.1";
        public static string OperatorID = "764";//"10011";
        public static string ChannelID = "17";//"14";
        public static string AccessMode = "3";//"9";
        public static string MsgLanguageCode = "2002";
        public static string TimeType = "1";
        public static string TimeZoneID = "101";
        public static string RechargeSerialNo = "123123123132";
        public static string RechargeType = "2";
        public static string PrimaryIdentity = "77105676";
        public static string Amount = "100";
        public static string BankCode = "PNB";
        public static string BankBranchCode = "PNB";
        public static string opType = "1";
    }

    public class TCELLPARAM_RESP
    {
        public static string ResultCode { get; set; }
        public static string ResultDesc { get; set; }
        public static string RechargeSerialNo { get; set; }
        public static string BalanceType { get; set; }
        public static string BalanceID { get; set; }
        public static string BalanceTypeName { get; set; }
        public static string OldBalanceAmt { get; set; }
        public static string NewBalanceAmt { get; set; }
        public static string StatusName { get; set; }
        public static string StatusExpireTime { get; set; }
        public static string StatusIndex { get; set; }

        //////// Outstanding Amount Response
        public static string AcctKey { get; set; }
        public static string BillCycleID { get; set; }
        public static string BillCycleBeginTime { get; set; }
        public static string BillCycleEndTime { get; set; }
        public static string DueDate { get; set; }
        public static string OutStandingAmount { get; set; }
        public static string CurrencyID { get; set; }

    }

    public class TCELLAPI_RESP
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class TCELLAPI_REQ
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class TASHILEASEDLINEOUTSTANDING_RESP
    {
        public string request_id { get; set; }
        public string result_code { get; set; }
        public string result_desc { get; set; }
        public Account_info account_info { get; set; }
        public Service_info service_info { get; set; }
    }

    public class Account_info
    {
        public List<Basic_detail> basic_details { get; set; }
    }

    public class Basic_detail
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class Service_info
    {
        public List<Basic_detail> basic_details { get; set; }
    }

    #region TcellPaymentClass
    // request class
    public class TcellPayment //rootclass
    {
        public Order_information order_information { get; set; }
    }

    public class Order_information
    {
        public string order_type { get; set; }
        public Payment payment { get; set; }
    }

    public class Payment
    {
        public string account_id { get; set; }
        public string amount { get; set; }
        public string comment { get; set; }
        public string currency_code { get; set; }
        public string invoice_ids { get; set; }
        public string invoice_amounts { get; set; }
        public string transaction_id { get; set; }
        public List<Payment_details> payment_detail { get; set; }
    }

    public class Payment_details
    {
        public string payment_mode { get; set; }
        public string amount_paid { get; set; }
        public string reference_external_id { get; set; }
        public string card_holder_name { get; set; }
        public string beneficiary { get; set; }
        public List<Mode_details> mode_detail { get; set; }
    }

    public class Mode_details
    {
        public string key { get; set; }
        public string value { get; set; }
    }


    // response class

    public class TcellPaymentResp
    {
        public string request_id { get; set; }
        public string source_node { get; set; }
        public string result_code { get; set; }
        public string result_desc { get; set; }
        public string message { get; set; }
        public Dataset dataset { get; set; }
    }

    public class Dataset
    {
        public List<Param> param { get; set; }
    }

    public class Param
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    #endregion TcellPaymentClass

    #region Bhim

    public class BhimQrReq
    {
        public string ver { get; set; }
        public string mode { get; set; }
        public string Purpose { get; set; }
        public string orgId { get; set; }
        public string tr { get; set; }
        public string category { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public string am { get; set; }
        public string cc { get; set; }
        public string gstBrkUp { get; set; }
        public string bAm { get; set; }
        public string bCurr { get; set; }
        public string qrMedium { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string mn { get; set; }
        public string type { get; set; }
        public string Validitystart { get; set; }
        public string Validityend { get; set; }
        public string Amrule { get; set; }
        public string Recur { get; set; }
        public string Recurvalue { get; set; }
        public string Recurtype { get; set; }
        public string Rev { get; set; }
        public string Share { get; set; }
        public string Block { get; set; }
        public string Umn { get; set; }
        public string sign { get; set; }

        public string mid { get; set; }
        public string msid { get; set; }
        public string invoiceName { get; set; }
        public string mtid { get; set; }
        public string tn { get; set; }

        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAccountNumber { get; set; }
        public string EMAIL { get; set; }
        public string MerchantMobileNumber { get; set; }
        public string STATE { get; set; }
        public string MerchantAddress { get; set; }
        public string MerchantBankCode { get; set; }
        public string MerchantBankName { get; set; }
        public bool IsMerchantActive { get; set; }
        public bool IsMerchantDeActive { get; set; }
        public string ReferenceNumber { get; set; }
        public string LogedInUserID { get; set; }
        public string MerchantCID { get; set; }
        public string MerchantCategory { get; set; }
        public string MerchantID { get; set; }

        public string ReqSrc { get; set; }
    }

    public class BhimQrReqRes
    {

        public string QrData { get; set; }

        public string MerchntName { get; set; }
        public string VPA { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseDateTime { get; set; }

    }

    #endregion Bhim

    public class SOUNDBOX_RESP
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class SOUNDBOX_REQ
    {
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }

}