using System.Runtime.Serialization;

namespace MaxiSwitch.Common.Enumeration
{
    public enum EnumTerminalMessageProtocal:int
    {
        Unknown=0,
        NDC=1,
        DDC=2,
    }

    public enum EnumHostMessage:int
    {
        Unknown = 0,
        ISO=1,
        ISBS=2,
        Cash24=3
    }

    public enum EnumTerminalReqMsgType:int
    {
        Unknown = 0,
        Solicited=1,
        Unsolicited=2,
        ConsumerRequest=3,
        OtherSolicited=4,
    }

    public enum EnumTerminalProfiles : int
    {
        //NCRV1=1,
        //NCRV2=2,
        //D422=3,
        //D429=4,
        //Wincore=5
        NDC = 1,
        DDC = 2,
        Wincor = 3,
        D429 = 4
    }
   
    public enum EnumTransactionType : int
    {
        OnUs=1,
        OffUS=2
    }

    public enum EnumErrorSeverity : int
    {
        NoError=0,
        RoutineError=1,
        Warning=2,
        Suspend=3,
        fatal=4,
        NoNewStatus=5
    }

    public enum EnumSupplyStatus : int
    {
        NoNewState = 0,
        GoodState = 1,
        MediaLow = 2,
        MediaOut = 3,
        Overfill = 4,
        TopSearchFault = 5,
        HeadReturnFault = 6,
        LinePrintFault = 7,
        ElectricalFault = 8
    }

    public enum EnumCassettePosition : int
    {
        FirstCassettePosition = 1,
        SecondCassettePosition = 2,
        ThirdCassettePosition = 3,
        ForthCassettePosition = 4,
    }

    public enum EnumDeviceStatusDescriptor : int
    {
        DeviceFault=0,
        Ready9=1,
        ReadyB=2,
        GCommandReject=3,
        SCommandReject=4,
        TerminalState=5
    }

    public enum EnumActionType : int
    {
        InService=0,
        OutOffService=1,
        Download=2,
        Reversal=3,
        NoAction=4
    }

    public enum EnumDeviceStatusType : int
    {
        Hardware=1,
        Supplydetails=2
    }

    public enum EnumDevice : int
    {
        CashHandler = 0,
        CardReader = 1,
        ReceiptPrinter = 2,
        JournalPrinter = 3,
        StatementPrinter = 4,
        VandalGuard = 5,
        DoorAccess = 6,
        Depository = 7,
        Supervisor = 8,
        PowerUP = 9,
        TimeOfDayClock = 10,
        HighOrderCommunications = 11,
        SystemDisk = 12,
        NightSafeDepository = 13,
        Encryptor = 14,
        SecurityCamera = 15,
        FlexDisk = 16,
        CassetteType1 = 17,
        CassetteType2 = 18,
        CassetteType3 = 19,
        CassetteType4 = 20,
        SignageDisplay = 21,
        CoinDispenser = 22,
        SystemDisplay = 23,
        MediaEntryIndicators = 24,
        EnvelopeDispenser = 25,
        DocumentProcessingModule = 26,
        CoinDispensingModuleTamperIndication = 27,
        DocumentProcessingModuleTamperIndicationModule = 28,
        DigitalAudioService = 29,
        EDMModule = 30,
        ////****DDC**********
        
         Alarm = 31,
        CardWriter = 32,
        WithdrawalAreaSensors = 33,
        Printers = 34,
        Presenter = 35,
        EnhancedStatus = 36,
        DDCConfigurationInformation = 37,
        EnhancedSupplyStatus = 38,
        ////***NDC*******************
        SupervisorKeyMessages = 39,
        ////****DDC**********
        WithdrawalDoorStatus = 40,
        SupplyStaus = 41,
        
    }
    
    public enum EnumSourceChanel : int
    {
        Unknown =0,
        Terminal=1,
        Switch=2,
        HSM=3,
        Controller=4,
        SwitchAndController=5
    }
        
    public enum EnumTerminalCommands : int
    {
        UnKnown = 0,
        InService=1,
        OutOfService=2,
        SupplyCounters=4,
        ErrorLogInformation=5,
        ConfigurationInformation=7,
        DateAndTimeInformation=8,
        ReloadTerminalConfigurations = 9
    }

    public enum TerminalResponceType:int
    {
        UnKnown=0,
        CustomCommand=1,
        DownLoad=2,
        TransactionResponse=3,
        PendingDownload=4,
    }

    public enum EnumDownload : int
    {
        ////NotStarted=0,
        ////OutOfService = 1,
        ////ConfigurationInformation=2,
        ////SupplyInformation=3,
        ////CustomizationDownload = 4,
        ////StatesDownload = 5,
        ////ScreenDownload = 6,
        ////FitDownload = 7,
        ////TPKKeyDownload = 8,
        ////ConfigurationIDDownload = 9,
        ////InService = 10,
        ////DDCConfigurationInformation = 11,
        ////EnhancedSupplyStatus = 12,
        NotStarted = 0,
        ConfigurationInformation = 1,
        SupplyInformation = 2,
        OutOfService = 3,
        CustomizationDownload = 4,
        StatesDownload = 5,
        ScreenDownload = 6,
        FitDownload = 7,
        TPKKeyDownload = 8,
        ConfigurationIDDownload = 9,
        InService = 10,
        DDCConfigurationInformation = 11,
        EnhancedSupplyStatus = 12,
    }
    
    public enum EnumAccountType : int
    {
        UNKNOWN = 0,
        SAVING = 1,
        CURRENT = 2,
    }

    public enum EnumTransactionCashStatus:int
    {
        Avalible=0,
        UnableToDispance=1,
        TryEDenominations=2,
        TryFDenominations=3,
        TryGDenominations=4,
        TryHDenominations=5
    }

    public enum EnumTerminalDeviceHealthStatus : int
    {   
        Online,
        Offline
    }

    public enum EnumTerminalStatus : int
    {
        Offline = 0,
        OutOfService = 1,
        Online = 2,        
    }

    public enum EnumDDCCassetteInformation : int
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7,
        H = 8,
    }
    
    public enum EnumNDCCassetteInformation : int
    {
        BA = 1,
        BB = 2,
        BC = 3,
        BD = 4,
       
    }
}
