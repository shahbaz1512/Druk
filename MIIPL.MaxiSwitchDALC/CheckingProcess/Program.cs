using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using DbNetLink.Data;
using MaxiSwitch.DALC.TerminalConfiguration;
using MaxiSwitch.DALC.CommonConst;

namespace CheckingProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            TerminalConfiguration tc = new TerminalConfiguration();
            CommonConst cc=new CommonConst();
            DataTable dt = new DataTable();
            dt =tc.SelectTerminalConfiguration("192.168.1.151");
            
        }
    }
}
