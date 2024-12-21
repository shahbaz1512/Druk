/* Class used for connecting HSM Manager */
using System;
//using TransactionRouter.SwitchCommonDetails;
using MaxiSwitch.API.Terminal;
using TransactionRouter.CommunicationManager.HSM;
//using TransactionRouter.Logger;
using MaxiSwitch.Common.TerminalLogger;

namespace TransactionRouter.CommunicationChanel.HSM
{   
    public delegate Authentication SendAuthenticationRequest(ref Authentication RequestMsg, enumSecurityModuleType SecurityModuleType);
    
    public class HsmCommunicationChanel
    {
        public SystemLogger SystemLogger = null;
        public SendAuthenticationRequest SendAuthRequest { get; set; }
        public HsmCommunicationChanel()
        {   
            SendAuthRequest = new SendAuthenticationRequest(AuthenticationRequest);
        }

        #region Switch Connection Manager
        static HsmConnectionManager _hsmConnectionManager = null;
        public static HsmConnectionManager HsmConnectionManager
        {
            get
            {
                if (_hsmConnectionManager == null)
                {
                    _hsmConnectionManager = new HsmConnectionManager();                    
                }
                return _hsmConnectionManager;
            }
            set { _hsmConnectionManager = value; }
        }
        #endregion Switch Connection Manager

        #region Remort Commands

        internal Authentication AuthenticationRequest(ref Authentication RequestMsg, enumSecurityModuleType SecurityModuleType)
        {
            ////try
            ////{
                var Resource = ((HsmConnectionResource)HsmConnectionManager.ResourceMgr.GetItem());
                if (Resource != null)
                {
                    var AuthResponse = Resource.RemoteObject.AuthenticationRequest(MsgConvertor(RequestMsg, SecurityModuleType));
                    Resource.TaskCompleted.Invoke(Resource, new EventArgs());
                    RequestMsg = MsgConvertor(RequestMsg, AuthResponse);
                }
                else
                {
                    SystemLogger.WriteTransLog(null, "Hsm Resource Not Found Or Busy");
                    RequestMsg.TransactionStatus = enumTransactionStatus.Unknown;
                }
                return RequestMsg;
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); return RequestMsg; }
        }       

        #endregion

        #region Switch Message Formatting

        public Authentication MsgConvertor(Authentication ReqMsg, enumSecurityModuleType SecurityModuleType)
        {
            Authentication _authenticationRequest = new Authentication()
            {
                Cardnumber = ReqMsg.Cardnumber,
                CardTrack2 =  ReqMsg.CardTrack2,
                SsmComkey = ReqMsg.SsmComkey,
                SsmMasterKey = ReqMsg.SsmMasterKey,
                SsmPvk = ReqMsg.SsmPvk,
                SsmCvv1 = ReqMsg.SsmCvv1,
                SsmCvv2 = ReqMsg.SsmCvv2,
                SsmCvkPair= ReqMsg.SsmCvkPair,
                TmkEncryptedKey = ReqMsg.TmkEncryptedKey,
                HsmZpk = ReqMsg.HsmZpk,
                HsmComkey = ReqMsg.HsmComkey,
                HsmPvk = ReqMsg.HsmPvk,
                HsmCvk = ReqMsg.HsmCvk,
                HsmPosZpk = ReqMsg.HsmPosZpk,
                HsmEmvk = ReqMsg.HsmEmvk,
                PinOffset = ReqMsg.PinOffset,
                PinBlock = ReqMsg.PinBlock,
                PinChangeBlock = ReqMsg.PinChangeBlock,                                
                SecurityModule = SecurityModuleType,
                TransactionReqMode = ReqMsg.TransactionReqMode,
                TransactionStatus = ReqMsg.TransactionStatus,
                TransactionType = ReqMsg.TransactionType,
                EMVVerificationData = ReqMsg.EMVVerificationData,
                Product = ReqMsg.Product,
                CardScheme = ReqMsg.CardScheme,
                ResponseCode = Convert.ToString(ReqMsg.ResponseCode),
                CardAuthenticationResultsCode = ReqMsg.CardAuthenticationResultsCode,
            };
            return _authenticationRequest;
        }

        public Authentication MsgConvertor(Authentication ReqMsg, Authentication RespMsg)
        {
            try
            {
                ReqMsg.CCVResponse = RespMsg.CCVResponse;
                ReqMsg.VerifyPinResponse = RespMsg.VerifyPinResponse;
                ReqMsg.VerifyPinChangeResponse = RespMsg.VerifyPinChangeResponse;
                ReqMsg.NewOffset = RespMsg.NewOffset;
                ReqMsg.TransactionStatus = RespMsg.TransactionStatus;
                ReqMsg.SessionKeyResponse = RespMsg.SessionKeyResponse;
                ReqMsg.EMVVerificationResponse = RespMsg.EMVVerificationResponse;
                ReqMsg.ServiceCode = RespMsg.ServiceCode;
                ReqMsg.CardAuthenticationResultsCode = RespMsg.CardAuthenticationResultsCode;
            }
            catch(Exception ex)
            { SystemLogger.WriteErrorLog(this, ex); }
            return ReqMsg;
        }
       
        #endregion
    }
}
