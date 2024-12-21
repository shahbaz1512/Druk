using DALC;
using IMPSTransactionRouter.Models;
using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.DALC.ConsumerTransactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IMPSTransactionRouter.Controllers
{
    public class MobilePortalManagementController : ApiController
    {
        CommanDetails _CommanDetails = new CommanDetails();
        ProcessMessage _ProcessMessage = new ProcessMessage();
        ProcessPortalRequest _ProcessPortalRequest = new ProcessPortalRequest();
        MobilePortalProcess _MobilePortalProcess = new MobilePortalProcess();
        [HttpPost]
        public MOBILEPORTAL_RES Get_RegisteredUsers([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Registered User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Get_RegisteredUsers(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {
                CommonLogger.WriteTransLog(this, string.Format("***** Get Registered User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES Get_RegisteredUsers_listCustomer([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Get_RegisteredUsers_listCustomer Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Get_RegisteredUsers_listCustomer(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {
                CommonLogger.WriteTransLog(this, string.Format("***** Get Get_RegisteredUsers_listCustomer User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES Get_RegisteredUserAccounts([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Registered User Account List Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Get_RegisteredUsersAccountList(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Registered User Account List Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES SET_BRANCH_ATM_LOCATION([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** SET BRANCH LOCATION Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Set_BRANCH_ATM_LOCATION(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** SET BRANCH LOCATION Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_BRANCH_ATM_LOCATION([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** GET BRANCH LOCATION Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GET_BRANCH_ATM_LOCATION(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** GET BRANCH LOCATION Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES UPDATE_BRANCH_ATM_LOCATION([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** UPDATE BRANCH LOCATION Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.UPDATE_BRANCH_ATM_LOCATION(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** UPDATE BRANCH LOCATION Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES DELETE_BRANCH_ATM_LOCATION([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** DELETE BRANCH LOCATION Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.DELETE_BRANCH_ATM_LOCATION(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** DELETE BRANCH LOCATION Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                      "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES BLOCKUSER([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** BLOCK USER Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "BLOCK USER", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }
                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.BLOCKUSER(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("****** BLOCK USER  Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES UNBLOCKUSER([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Unblock User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "UNBLOCK USER", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }
                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.UNBLOCKUSER(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Unblock User  Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES RESETPASSWORD([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Reset Password Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "RESET PASSWORD", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.RESETPASSWORD(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Reset Password  Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES RESETMPIN([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Reset Pin Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "RESET mPIN", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.RESETMPIN(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Reset Pin Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES MERCHANTDETAILS([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** GET merchant Details Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.MERCHANTDETAILS(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** GET merchant Details Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES ACTIVEINACTIVEMERCHANT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Activate Deactivate Merchant Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.ACTIVEINACTIVEMERCHANT(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Activate Deactivate Merchant Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES REGISTERMERCHANT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Register New Merchant Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.REGISTERMERCHANT(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Register New Merchant Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES UPDATEMERCHANT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** UPDATE Merchant Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.UPDATEMERCHANT(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** UPDATE Merchant Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES DeleteMerchant([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Delete Merchant Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.DeleteMerchant(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Delete Merchant Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_PENDINGCHECKDETAILS([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** GET Pending Checks Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GET_PENDINGCHECKDETAILS(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** GET Pending Checks Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_APPROVECHECKES([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Approve Checks Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GET_APPROVECHECKES(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Approve Checks Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GETCHECKDETAIL([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Check Details Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GETCHECKDETAIL(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Check Details Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES APPROVE_DECLINE_CHECK([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Approve Or Decline Check Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.APPROVE_DECLINE_CHECK(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Approve or Decline checks Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetBankDetails([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Bank Details Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.BankDetails = MobilePortalProcess.GetBankDetails();
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            ///////////////// Process Get Bank Details Response //////////////////////////

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Bank Details  Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetChequeDetails([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get cheque Details Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GETCHECKDETAIL(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            ///////////////// Process Get Bank Details Response //////////////////////////

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get cheque Details  Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetChequeReportDetails([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get cheque Report Details Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GETCHEQUEREPORTDETAIL(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            ///////////////// Process Get Bank Details Response //////////////////////////

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get cheque Report Details  Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        #region Manage Image Distribution
        [HttpPost]
        public MOBILEPORTAL_RES UploadDistributionImage([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Upload ImageTransaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.InsertUpdateDeleteImages(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "0");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Upload Image Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetDistributedImage([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                //#region Loger
                //try
                //{
                //    using (var stringWriter = new StringWriter())
                //    {
                //        using (var xmlWriter = XmlWriter.Create(stringWriter))
                //        {
                //            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                //            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                //        }
                //        string MobileRequestData = stringWriter.ToString();

                //        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                //        CommonLogger.WriteTransLog(this, string.Format("***** Get Distributed Image Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                //                                                        FormattedXML.ToString() + Environment.NewLine));
                //    }

                //}
                //catch (Exception ex)
                //{
                //    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                //}
                //#endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.InsertUpdateDeleteImages(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            //#region Loger
            //try
            //{

            //    CommonLogger.WriteTransLog(this, string.Format("***** Get Distributed Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
            //                                                   "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            //}
            //catch (Exception ex)
            //{
            //    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
            //    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
            //    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            //}
            //#endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES UpdateDistributedImage([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Update Distributed Image Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.InsertUpdateDeleteImages(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "2");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Update Distributed Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES DeleteDistributedImage([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Delete Distributed Image Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.InsertUpdateDeleteImages(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "3");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Delete Distributed Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }
        #endregion Manage Image Distribution

        #region Reports
        [HttpPost]
        public MOBILEPORTAL_RES GetIMPSReport([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get IMPS Transaction Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetIMPSReports(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get IMPS Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }



        [HttpPost]
        public MOBILEPORTAL_RES GetImpsRequestmoneyDetailsReport([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get IMPS Requestmoney Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetRequestMoney(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get IMPS Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }


        [HttpPost]
        public MOBILEPORTAL_RES GetPaymentReport([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Payment Transaction Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetPaymentReports(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Payment Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetRDTDReport([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get RD TD Transaction Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetRDTDReports(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get RD TD Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetPaymentReversalReport([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Payment Reversal Transaction Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetPaymentReversalReports(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ, "1");
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Payment Reversal Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }
        #endregion Reports

        [HttpPost]
        public MOBILEPORTAL_RES Set_GlobalLimit([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("Set Limit request recived"));
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Set IMPS Global Limit Transaction Report Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "RESET LIMIT", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Set_GlobalLimit(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {
                string MobileResponseData = string.Empty;
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_RES));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_RES);
                    }
                    MobileResponseData = stringWriter.ToString();

                }
                XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                CommonLogger.WriteTransLog(this, string.Format("***** Set IMPS Global Limit Transaction Report Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES Get_SignUpRequestUsers([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get SignUp Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Get_SignUpRequest(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get SignUp Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES Set_ApproveDeclineSignUpRequest([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Set Approve Decline SignUp Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    string Status = string.Empty;
                    if (_MOBILEPORTAL_REQ.ApproveStatus == "0")
                        Status = "APPROVED";
                    else
                        Status = "DECLINED";
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "APPROVE/DECLINE SIGNUP " + Status, _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Set_ApproveDeclineSignUpRequest(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Set Approve Decline Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        #region QR CODE

        [HttpPost]
        public MOBILEPORTAL_RES QRCODE([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get QR Data Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.QRCODE(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            return _MOBILEPORTAL_RES;

        }

        #endregion QR CODE


      


        #region Vote
        [HttpPost]
        public MOBILEPORTAL_RES Set_ShowMaster([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Set ShowMaster Transaction  Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    if (_MOBILEPORTAL_REQ.FLAG != "SELECT" && _MOBILEPORTAL_REQ.FLAG != "SPECIFIC" && _MOBILEPORTAL_REQ.FLAG != "EXCEL" && _MOBILEPORTAL_REQ.FLAG != "PDF")
                    {
                        MobilePortalProcess.ShowMasterInsertUpdateDelete(_MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.MerchantName, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToAccount, _MOBILEPORTAL_REQ.Amount, _MOBILEPORTAL_REQ.TransaferLimitAmount, out status, _MOBILEPORTAL_REQ.ProductCode, _MOBILEPORTAL_REQ.Image);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                }
                catch (Exception ex)
                {
                }
                if (_MOBILEPORTAL_REQ.FLAG == "EXCEL" || _MOBILEPORTAL_REQ.FLAG == "PDF" || _MOBILEPORTAL_REQ.FLAG == "SELECT")
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        _MOBILEPORTAL_REQ.FLAG = "SELECT";
                        _ProcessPortalRequest.Select_ShowDetails(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    });
                    task.Wait();
                }
                if (_MOBILEPORTAL_REQ.FLAG == "SPECIFIC")
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        _MOBILEPORTAL_REQ.FLAG = "SPECIFIC";
                        _ProcessPortalRequest.Select_ShowDetails(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    });
                    task.Wait();
                }

            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {
                string MobileResponseData = string.Empty;
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_RES));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_RES);
                    }
                    MobileResponseData = stringWriter.ToString();

                }
                XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                CommonLogger.WriteTransLog(this, string.Format("***** Set ShowMaster Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }


        public MOBILEPORTAL_RES Set_ShowContestentMaster([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            int status = -1;
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Set ContestentMaster Transaction  Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    if (_MOBILEPORTAL_REQ.FLAG != "SELECT" && _MOBILEPORTAL_REQ.FLAG != "SPECIFIC" && _MOBILEPORTAL_REQ.FLAG != "EXCEL" && _MOBILEPORTAL_REQ.FLAG != "PDF")
                    {
                        //_MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.ContestantID,  _MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ContestantName, _MOBILEPORTAL_REQ.ContestantAge,
                       //_MOBILEPORTAL_REQ.con, string State, string Mobile, _MOBILEPORTAL_REQ.conte, string ContestantImage, string Createdby, out Int32 status
                        MobilePortalProcess.ShowContestantMasterInsertUpdateDelete(_MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.ContestantID, _MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ContestantName, _MOBILEPORTAL_REQ.ContestantAge, _MOBILEPORTAL_REQ.ContestantCity, _MOBILEPORTAL_REQ.ContestantState, _MOBILEPORTAL_REQ.ContestantMobile, _MOBILEPORTAL_REQ.ContestantEmail, _MOBILEPORTAL_REQ.ContestantImage, _MOBILEPORTAL_REQ.LogedInUserID, out status, _MOBILEPORTAL_REQ.ContestantNumber, _MOBILEPORTAL_REQ.IsRemoved, _MOBILEPORTAL_REQ.OtherDetails);
                    }
                }
                catch { }

                //var task = Task.Factory.StartNew(() =>
                //{
                //    _MOBILEPORTAL_REQ.FLAG = "SELECT";
                //    _ProcessPortalRequest.Select_ShowContestantDetails(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                //    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                //    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                //});
                //task.Wait();

                  if (_MOBILEPORTAL_REQ.FLAG == "EXCEL" || _MOBILEPORTAL_REQ.FLAG == "PDF" || _MOBILEPORTAL_REQ.FLAG == "SELECT")
                {
                          var task = Task.Factory.StartNew(() =>
                      {
                          _MOBILEPORTAL_REQ.FLAG = "SELECT";
                          _ProcessPortalRequest.Select_ShowContestantDetails(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                          _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                          _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                      });
                          task.Wait();
                }
                if (_MOBILEPORTAL_REQ.FLAG == "SPECIFIC" )
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        _MOBILEPORTAL_REQ.FLAG = "SPECIFIC";
                        _ProcessPortalRequest.Select_ShowContestantDetails(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    });
                    task.Wait();
                }
          
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {
                string MobileResponseData = string.Empty;
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_RES));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_RES);
                    }
                    MobileResponseData = stringWriter.ToString();

                }
                XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                CommonLogger.WriteTransLog(this, string.Format("***** Set ContestentMaster Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                               "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }
       
        #endregion Vote


        #region Get Add Account Requested Data

        [HttpPost]
        public MOBILEPORTAL_RES Get_AddAccountRequest([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Add Account Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Get_AddAccountRequest(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Add Account Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES Set_ApproveDeclineAddAccountRequest([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Set Approve Decline Add Account Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "APPROVE/DECLINE ADD ACCOUNT " + _MOBILEPORTAL_REQ.ApproveStatus, _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.Set_ApproveDeclineAddAccountRequest(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Set Approve Decline Add Account Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES SET_ADDNEWACCOUNT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Add New Account Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    int status = -1;
                    string ReqType = string.Empty;
                    if (_MOBILEPORTAL_REQ.ISSECONDARYACCUPDATEREQ)
                        ReqType = "UPDATE";
                    else
                        ReqType = "INSERT";

                    if (ReqType == "INSERT")
                    {
                        string[] AccountNumber = _MOBILEPORTAL_REQ.CUSTOMERNEWACCOUNT.Split(',');
                        foreach (string ACCOUNT in AccountNumber)
                        {
                            MobilePortalProcess.ADDACCOUNT(_MOBILEPORTAL_REQ.CUSTOMERNAME, ACCOUNT.Substring(1, 9), _MOBILEPORTAL_REQ.CUSTOMERPRIMARYACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERMOBILENUMBER, ACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERACCTYPE, _MOBILEPORTAL_REQ.CUSTOMERCCY, "ACTIVE", _MOBILEPORTAL_REQ.LogedInUserID, ReqType, out status);
                        }
                    }
                    else
                    {
                        MobilePortalProcess.ADDACCOUNT(_MOBILEPORTAL_REQ.CUSTOMERNAME, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.CUSTOMERPRIMARYACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERMOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERNEWACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERACCTYPE, _MOBILEPORTAL_REQ.CUSTOMERCCY, "ACTIVE", _MOBILEPORTAL_REQ.LogedInUserID, ReqType, out status);
                    }
                    if (status == 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        if (_MOBILEPORTAL_REQ.ISSECONDARYACCUPDATEREQ)
                            _MOBILEPORTAL_RES.ResponseDesc = "Record Updated Successfully.";
                        else
                            _MOBILEPORTAL_RES.ResponseDesc = "New Account Added Successfully.";
                    }
                    else if (status == 2)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        if (_MOBILEPORTAL_REQ.ISSECONDARYACCUPDATEREQ)
                            _MOBILEPORTAL_RES.ResponseDesc = "Invalid Account Number";
                        else
                            _MOBILEPORTAL_RES.ResponseDesc = "This account number is already exists";
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Add New Account Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_ADDNEWACCOUNT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** GET New Account Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GET_ADDACCOUNT(_MOBILEPORTAL_REQ.CUSTOMERPRIMARYACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERMOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERNEWACCOUNT);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** GET New Account Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES DeleteSecondriAccount([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Delete Secondary Account Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.CUSTOMERNEWACCOUNT, _MOBILEPORTAL_REQ.MOBILENUMBER, "DELETE SECONDRI ACCOUNT", _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    int status = -1;
                    string ReqType = "DELETE";
                    MobilePortalProcess.ADDACCOUNT(_MOBILEPORTAL_REQ.CUSTOMERNAME, _MOBILEPORTAL_REQ.CUSTOMERID, _MOBILEPORTAL_REQ.CUSTOMERPRIMARYACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERMOBILENUMBER, _MOBILEPORTAL_REQ.CUSTOMERNEWACCOUNT, _MOBILEPORTAL_REQ.CUSTOMERACCTYPE, _MOBILEPORTAL_REQ.CUSTOMERCCY, "ACTIVE", _MOBILEPORTAL_REQ.LogedInUserID, ReqType, out status);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = "Record Deleted Successfully.";

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Delete Secondary Account Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GETMPORTALACTIVITY([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GETMPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            return _MOBILEPORTAL_RES;

        }

        #endregion Get Add Account Requested Data

        [HttpPost]
        public MOBILEPORTAL_RES DeleteUser([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Delete user Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                try
                {
                    MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, "USER TERMINATED " + _MOBILEPORTAL_REQ.ApproveStatus, _MOBILEPORTAL_REQ.LogedInUserID);
                }
                catch { }

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.DeleteUser(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Delete user Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GETIMPSOTPDETAILS([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GETIMPSOTPDETAILS();
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_OFFLINE_DEBITCARD([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();

            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get offline debit card requested Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Debit(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    try
                    {
                        _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                        for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                        {
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                if (status != 0)
                                {
                                    _MOBILEPORTAL_REQ.MailID = null;
                                }
                                else
                                {
                                    _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get offline debit card requested Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_OFFLINE_CHEQUEBOOK([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();

            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get offline cheque book requested Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Cheque(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    try
                    {
                        _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                        for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                        {
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                if (status != 0)
                                {
                                    _MOBILEPORTAL_REQ.MailID = null;
                                }
                                else
                                {
                                    _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get offline cheque book requested Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_OFFLINE_STATEMENT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get offline statement requested Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Statement(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    //try
                    //{
                    //    _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                    //    for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                    //    {
                    //        try
                    //        {
                    //            int status = -1;
                    //            DataTable DTCustomerdata = new DataTable();
                    //            DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    //            if (status != 0)
                    //            {
                    //                _MOBILEPORTAL_REQ.MailID = null;
                    //            }
                    //            else
                    //            {
                    //                if (string.IsNullOrEmpty(_MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"].ToString()))
                    //                {
                    //                    DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["PRIMARYACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    //                }
                    //                _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                    //            }

                    //            //if (string.IsNullOrEmpty(_MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"].ToString()))
                    //            //{
                    //            //    _MOBILEPORTAL_REQ.ACCOUNTNUMBER = _MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString();
                    //            //    _MOBILEPORTAL_REQ.CUSTOMERID = _MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString().Substring(1, 9);
                    //            //    _ProcessPortalRequest.Get_RegisteredUsersAccountList(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    //            //    for (int j = 0; j < _MOBILEPORTAL_RES.RegisteredUsers.Rows.Count; j++)
                    //            //    {
                    //            //        if (string.IsNullOrEmpty(_MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"].ToString()))
                    //            //        {
                    //            //            DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.RegisteredUsers.Rows[j]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    //            //            _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = DTCustomerdata.Rows[0]["e_mail"].ToString();
                    //            //        }
                    //            //    }
                    //            //}
                    //        }
                    //        catch { }
                    //    }
                    //}
                    //catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get offline statement requested Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES ApproveDeclineOfflineRequest_Statement([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline statement Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {
                int status = -1;
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.ApproveDeclineOfflineRequest_Statement(_MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.ApproveStatus, enumTransactionType.AccountStatementRequest.ToString(), _MOBILEPORTAL_REQ.DeclineReason, out status);
                    if (status == 0)
                    {
                        try
                        {
                            _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                            for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                            {
                                try
                                {
                                    status = -1;
                                    DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                    if (status != 0)
                                    {
                                        _MOBILEPORTAL_REQ.MailID = null;
                                    }
                                    else
                                    {
                                        _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        try
                        {
                            string SMS = string.Empty;
                            string MAIL = string.Empty;
                            status = -1;
                            if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                SMS = "Dear Customer, Your account statement request has been approved by the bank. It will be emailed to you shortly.";
                            else
                                SMS = "Dear Customer, Your account statement request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".";

                            DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                            if (status != 0)
                            {
                                _MOBILEPORTAL_REQ.MailID = null;
                            }
                            else
                            {
                                _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                    MAIL = "Dear Customer,<br/><br/>Your account statement request has been approved by the bank. It will be emailed to you shortly.<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";
                                else
                                    MAIL = "Dear Customer,<br/><br/>Your account statement request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";

                            }
                            _ProcessMessage.ProcessSendApproveDeclineOfflineNotification(_MOBILEPORTAL_REQ, SMS, MAIL);
                        }
                        catch { }
                        try
                        {
                            MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.ApproveStatus + " STATEMENT REQUEST", _MOBILEPORTAL_REQ.LogedInUserID);
                        }
                        catch { }
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline statement Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES ApproveDeclineOfflineRequest_Chequebook([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline Cheque book Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {
                int status = -1;
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.ApproveDeclineOfflineRequest_Chequebook(_MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.ApproveStatus, enumTransactionType.ChequeBook.ToString(), _MOBILEPORTAL_REQ.DeclineReason, out status);
                    if (status == 0)
                    {
                        try
                        {
                            _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                            for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                            {
                                try
                                {
                                    status = -1;
                                    DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                    if (status != 0)
                                    {
                                        _MOBILEPORTAL_REQ.MailID = null;
                                    }
                                    else
                                    {
                                        _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        try
                        {
                            status = -1;
                            string SMS = string.Empty;
                            string MAIL = string.Empty;
                            DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                            if (status != 0)
                            {
                                _MOBILEPORTAL_REQ.MailID = null;
                            }
                            else
                            {
                                _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                    SMS = "Dear Customer, Your cheque book request has been approved by the bank.";
                                else
                                    SMS = "Dear Customer, Your cheque book request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".";

                                if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                    MAIL = "Dear Customer,<br/><br/>Your cheque book request has been approved by the bank. It will be emailed to you shortly.<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";
                                else
                                    MAIL = "Dear Customer,<br/><br/>Your cheque book request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";
                            }
                            _ProcessMessage.ProcessSendApproveDeclineOfflineNotification(_MOBILEPORTAL_REQ, SMS, MAIL);
                        }
                        catch { }
                        try
                        {
                            MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.ApproveStatus + " CHEQUE BOOK REQUEST", _MOBILEPORTAL_REQ.LogedInUserID);
                        }
                        catch { }
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline Cheque book Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES ApproveDeclineOfflineRequest_DebitCard([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline Debit Card Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {
                int status = -1;
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.ApproveDeclineOfflineRequest_DebitCard(_MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.LogedInUserID, _MOBILEPORTAL_REQ.ApproveStatus, enumTransactionType.DebitCard.ToString(), _MOBILEPORTAL_REQ.DeclineReason, out status);
                    if (status == 0)
                    {
                        try
                        {
                            _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                            for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                            {
                                try
                                {
                                    status = -1;
                                    DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                    if (status != 0)
                                    {
                                        _MOBILEPORTAL_REQ.MailID = null;
                                    }
                                    else
                                    {
                                        _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                        try
                        {
                            string SMS = string.Empty;
                            string MAIL = string.Empty;
                            status = -1;
                            DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                            if (status != 0)
                            {
                                _MOBILEPORTAL_REQ.MailID = null;
                            }
                            else
                            {
                                _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                    SMS = "Dear Customer, Your debit card request has been approved by the bank.";
                                else
                                    SMS = "Dear Customer, Your debit card request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".";

                                if (_MOBILEPORTAL_REQ.ApproveStatus == "APPROVE")
                                    MAIL = "Dear Customer,<br/><br/>Your debit card request has been approved by the bank.<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";
                                else
                                    MAIL = "Dear Customer,<br/><br/>Your debit card request has been declined by the bank. Reason: " + _MOBILEPORTAL_REQ.DeclineReason + ".<br/><br/>Thank You. <br/><br/><br/><br/><span style='color:red'>*** This is an automatically generated email, please do not reply. ***</span>";
                            }
                            _ProcessMessage.ProcessSendApproveDeclineOfflineNotification(_MOBILEPORTAL_REQ, SMS, MAIL);
                        }
                        catch { }
                        try
                        {
                            MobilePortalProcess.MPORTALACTIVITY(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.ApproveStatus + " DEBIT CARD REQUEST", _MOBILEPORTAL_REQ.LogedInUserID);
                        }
                        catch { }
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Approve & Decline Debit card Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GETIMPSCUSTOMERACTIVITYDETAILS([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get DrukPay activity request Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GETIMPSCUSTOMERACTIVITYDETAILS();
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get DrukPay activity request Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_STATEMENTREPORT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get Statement Report Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Statement_Report(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                    //try
                    //{
                    //    _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                    //    for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                    //    {
                    //        try
                    //        {
                    //            int status = -1;
                    //            DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                    //            if (status != 0)
                    //            {
                    //                _MOBILEPORTAL_REQ.MailID = null;
                    //            }
                    //            else
                    //            {
                    //                _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                    //            }
                    //        }
                    //        catch { }
                    //    }
                    //}
                    //catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Statement Report Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_CHEQUEBOOKREPORT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get Cheque book Report Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Cheque_Report(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                    try
                    {
                        _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                        for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                        {
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                if (status != 0)
                                {
                                    _MOBILEPORTAL_REQ.MailID = null;
                                }
                                else
                                {
                                    _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Cheque book Report Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_DEBITCARDREPORT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get Debit Card Report Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.SelectOfflineRequest_Debit_Report(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                    try
                    {
                        _MOBILEPORTAL_RES.ReportData.Columns.Add("Mail");
                        for (int i = 0; i < _MOBILEPORTAL_RES.ReportData.Rows.Count; i++)
                        {
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA(_MOBILEPORTAL_RES.ReportData.Rows[i]["ACCOUNTNUMBER"].ToString(), _MOBILEPORTAL_REQ.MOBILENUMBER, out status);
                                if (status != 0)
                                {
                                    _MOBILEPORTAL_REQ.MailID = null;
                                }
                                else
                                {
                                    _MOBILEPORTAL_RES.ReportData.Rows[i]["Mail"] = _MOBILEPORTAL_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Debit Card Report Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_INSERTDONOT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Add New Donor Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    int status = -1;
                    string ReqType = string.Empty;
                    if (_MOBILEPORTAL_REQ.ISUPDATE)
                        ReqType = "UPDATE";
                    else if (_MOBILEPORTAL_REQ.ISSELECT)
                        ReqType = "SELECT";
                    else if (_MOBILEPORTAL_REQ.ISDELETE)
                        ReqType = "DELETE";
                    else
                        ReqType = "INSERT";

                    if (ReqType == "INSERT")
                    {
                        _MOBILEPORTAL_RES.DonorDetails = MobilePortalProcess.ADDDONOR(_MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.DonorName, _MOBILEPORTAL_REQ.ProductCode, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, ReqType, _MOBILEPORTAL_REQ.Type, out status);
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.DonorDetails = MobilePortalProcess.ADDDONOR(_MOBILEPORTAL_REQ.ID, _MOBILEPORTAL_REQ.DonorName, _MOBILEPORTAL_REQ.ProductCode, _MOBILEPORTAL_REQ.ACCOUNTNUMBER, ReqType,_MOBILEPORTAL_REQ.Type, out status);
                    }
                    if (status == 0)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        if (_MOBILEPORTAL_REQ.ISUPDATE)
                            _MOBILEPORTAL_RES.ResponseDesc = "Record Updated Successfully.";
                        else
                            _MOBILEPORTAL_RES.ResponseDesc = "New Recipient Added Successfully.";
                    }
                    else if (status == 1)
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        if (_MOBILEPORTAL_REQ.ISUPDATE)
                            _MOBILEPORTAL_RES.ResponseDesc = "Invalid Details";
                        else
                            _MOBILEPORTAL_RES.ResponseDesc = "This Record already exists";
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Add New Donor Request User Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_FEEDBACKREPORT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get FeedBack Report Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GetFeedBackReport(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.MOBILENUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get FeedBack Report Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GET_CONTESTANTREPORT([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            #region Loger
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                        _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                    }
                    string MobileRequestData = stringWriter.ToString();

                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    CommonLogger.WriteTransLog(this, string.Format("***** Get Contestant Report Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                   FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            try
            {

                var task = Task.Factory.StartNew(() =>
                {
                    if (_MOBILEPORTAL_REQ.FLAG == "S")
                    {
                        _MOBILEPORTAL_RES.ResponseVotes = MobilePortalProcess.GetContestantNoVoteReport(_MOBILEPORTAL_REQ.ShowID, _MOBILEPORTAL_REQ.ContestantID, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.ReferenceNumber, _MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.IsRemoved);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }
                    else
                    {
                        _MOBILEPORTAL_RES.ResponseDetailVotes = MobilePortalProcess.GetContestantNoVoteReport(_MOBILEPORTAL_REQ.ShowID, _MOBILEPORTAL_REQ.ContestantID, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.ReferenceNumber, _MOBILEPORTAL_REQ.FLAG, _MOBILEPORTAL_REQ.IsRemoved);
                        _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                    }

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Contestant Report Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }



        [HttpPost]
        public MOBILEPORTAL_RES GetRequestMoneyBlockDetails([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get RequestMoney Block List Details Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                int status = -1;
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.RequestMoneyBlockList = MobilePortalProcess.GetRequestMoneyBlockDetails(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.ReferenceNumber, "SELECT", out status, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            ///////////////// Process Get Bank Details Response //////////////////////////

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get RequestMoney Block List Details Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }


        [HttpPost]
     
       public MOBILEPORTAL_RES GETOTHERDETAILS([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get Other Details Transaction Request Received From Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                int status = -1;
                var task = Task.Factory.StartNew(() =>
                {
                    _MOBILEPORTAL_RES.ReportData = MobilePortalProcess.GetOtherDetailsTran(_MOBILEPORTAL_REQ.ACCOUNTNUMBER, _MOBILEPORTAL_REQ.FromDate, _MOBILEPORTAL_REQ.ToDate, _MOBILEPORTAL_REQ.ReferenceNumber, "SELECT", out status, _MOBILEPORTAL_REQ.MOBILENUMBER);
                    _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            ///////////////// Process Get Bank Details Response //////////////////////////

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** Get Other Details Transaction Response To Terminal For Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion
            return _MOBILEPORTAL_RES;

        }

        [HttpPost]
        public MOBILEPORTAL_RES GetMerchantCategory([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** GET GetMerchantCategory Details Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.GetMerchantCategory(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }

            #region Loger
            try
            {

                CommonLogger.WriteTransLog(this, string.Format("***** GET GetMerchantCategory Details Transaction Response Send To Terminal For User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       "Response Code : " + _MOBILEPORTAL_RES.ResponseCode.ToString() + "Response Code Desc : " + _MOBILEPORTAL_RES.ResponseDesc.ToString() + Environment.NewLine));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            #endregion

            return _MOBILEPORTAL_RES;

        }



        [HttpPost]
        public MOBILEPORTAL_RES NationalQRCODE([FromBody]MOBILEPORTAL_REQ _MOBILEPORTAL_REQ)
        {
            MOBILEPORTAL_RES _MOBILEPORTAL_RES = new MOBILEPORTAL_RES();
            try
            {
                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_REQ);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get National QR Data Request User Transaction Request Received From Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                var task = Task.Factory.StartNew(() =>
                {
                    _ProcessPortalRequest.NationalQRCODE(ref _MOBILEPORTAL_RES, _MOBILEPORTAL_REQ);
                    // _MOBILEPORTAL_RES.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    //_MOBILEPORTAL_RES.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEPORTAL_RES.ResponseCode);
                });
                task.Wait();

                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEPORTAL_RES));
                            _serelized.Serialize(xmlWriter, _MOBILEPORTAL_RES);
                        }
                        string MobileRequestData = stringWriter.ToString();

                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                        CommonLogger.WriteTransLog(this, string.Format("***** Get National QR Data Request User Transaction Response send to Terminal From User " + _MOBILEPORTAL_REQ.LogedInUserID + "  Reference Number : " + _MOBILEPORTAL_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                       FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            return _MOBILEPORTAL_RES;

        }


    }
}
