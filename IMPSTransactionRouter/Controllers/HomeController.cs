using IMPSTransactionRouter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace IMPSTransactionRouter.Controllers
{
    public class HomeController : Controller
    {
        MobileBankingController _MobileBankingController = new MobileBankingController();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BLQ()
        {
            return View();
        }
        public ActionResult Mini()
        {
            return View();
        }
        public ActionResult FT_BNBL()
        {
            return View();
        }
        public ActionResult FT_OTHER()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BLQ(MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            string MobileRequestData = string.Empty;
            MOBILEBANKING_RESP _MOBILEBANKING_RESP = new MOBILEBANKING_RESP();
            _MOBILEBANKING_RESP = _MobileBankingController.Balanceinquiry(_MOBILEBANKING_REQ);
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    MobileRequestData = stringWriter.ToString();

                }
            }
            catch (Exception ex)
            {
                //_IMPSParameters.SystemLogger.WriteErrorLog(null, ex);
            }
            return Json(_MOBILEBANKING_RESP);
        }

        [HttpPost]
        public ActionResult Mini(MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            string MobileRequestData = string.Empty;
            MOBILEBANKING_RESP _MOBILEBANKING_RESP = new MOBILEBANKING_RESP();
            _MOBILEBANKING_RESP = _MobileBankingController.Ministatement(_MOBILEBANKING_REQ);
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    MobileRequestData = stringWriter.ToString();

                }
            }
            catch (Exception ex)
            {
                //_IMPSParameters.SystemLogger.WriteErrorLog(null, ex);
            }
            return Json(_MOBILEBANKING_RESP);
        }

        [HttpPost]
        public ActionResult FDI(MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            string MobileRequestData = string.Empty;
            MOBILEBANKING_RESP _MOBILEBANKING_RESP = new MOBILEBANKING_RESP();
            _MOBILEBANKING_RESP = _MobileBankingController.IntraFundTransfer(_MOBILEBANKING_REQ);
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    MobileRequestData = stringWriter.ToString();

                }
            }
            catch (Exception ex)
            {
                //_IMPSParameters.SystemLogger.WriteErrorLog(null, ex);
            }
            return Json(_MOBILEBANKING_RESP);
        }

        [HttpPost]
        public ActionResult FDO(MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            string MobileRequestData = string.Empty;
            MOBILEBANKING_RESP _MOBILEBANKING_RESP = new MOBILEBANKING_RESP();
            _MOBILEBANKING_RESP = _MobileBankingController.OutwardFundTransfer(_MOBILEBANKING_REQ);
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    MobileRequestData = stringWriter.ToString();

                }
            }
            catch (Exception ex)
            {
                //_IMPSParameters.SystemLogger.WriteErrorLog(null, ex);
            }
            return Json(_MOBILEBANKING_RESP);
        }
    }
}
