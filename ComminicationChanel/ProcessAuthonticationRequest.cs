using MIIPL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSMCommunicationChanel
{
    public class ProcessAuthonticationRequest
    {
        #region HsmPackCommand

        public string ThalesPackCommand(params string[] args)
        {
            string HsmCmd = string.Empty;
            lock (this)
            {
                foreach (string arg in args)
                {
                    HsmCmd = HsmCmd + arg;
                }
                HsmCmd = new string('#', CommonConfiguration.HSMHeaderLength) + HsmCmd;
            }
            return HsmCmd;
        }
        public string SafenetPackCommand(params string[] args)
        {
            string HsmCmd = string.Empty;
            int MessageLength = 0;
            try
            {
                lock (this)
                {
                    foreach (string arg in args)
                    {
                        HsmCmd = HsmCmd + arg;
                    }

                    MessageLength = Utils.HexToByteArray(HsmCmd).Length;

                    HsmCmd = SafenetHSMHeader.SOH + SafenetHSMHeader.Version + Utils.DecimalToHEX(SafenetHSMHeader.SequenceNo).PadLeft(4, '0')
                             + Utils.DecimalToHEX(MessageLength).PadLeft(4, '0') + HsmCmd;
                    SafenetHSMHeader.SequenceNo = SafenetHSMHeader.SequenceNo + 1;
                }
            }
            catch (Exception ex)
            {

            }
            return HsmCmd;
        }

        #endregion HsmPackCommand
    }
}
