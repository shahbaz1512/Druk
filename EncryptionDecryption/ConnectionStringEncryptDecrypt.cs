using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Text;

using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using DbNetLink.Data;

////*********** ********************************************   **********\\\\
////*********** Class File for 3 DES Encryption and Decrypion  **********\\\\
////*********** ********************************************   **********\\\\
namespace DALC
{
    public class ConnectionStringEncryptDecrypt
    {

        public static string ClearMEK = string.Empty;
        public static string ClearMEK2 = string.Empty;
        public static string ClearKEK = string.Empty;
        public static string ClearDEK = string.Empty;
        public static string ConnectionString = string.Empty;


        static ConnectionStringEncryptDecrypt()
        {
            string conn = ConnectionString;
            Load();
            ClearKEK = KEKKeys.Key();
        }

        #region "Data Encryption & Decryption Process"

        ////************** Data Encryption**************
        public static string EncryptString(string PlainText)
        {
            byte[] b = Encryption(PlainText);
            string CypherText = Convert.ToBase64String(b);

            return CypherText;
        }

        public static byte[] Encryption(string PlainText)
        {
            ////**************Used
            TripleDES des = CreateDES(ClearDEK);
            des.Mode = CipherMode.CBC;
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = Encoding.Unicode.GetBytes(PlainText);
            string sIV = "0000000000000000";
            return ct.TransformFinalBlock(input, 0, input.Length);

        }

        ////************** Data Decryption**************
        public static string DecryptString(string CypherText)
        {
            string DecryptedText = Decryption(CypherText);
            return DecryptedText;
        }
        public static string Decryption(string CypherText)
        {
            byte[] b = Convert.FromBase64String(CypherText);
            //TripleDES des = CreateDES(ClearDEK);
            TripleDES des = CreateDES(ClearDEK);
            des.Mode = CipherMode.CBC;
            ICryptoTransform ct = des.CreateDecryptor();
            string sIV = "0000000000000000";
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Convert.ToString(Encoding.Unicode.GetString(output));
        }

        public static TripleDES CreateDES(string key)
        {
            //MD5 md5 = new MD5CryptoServiceProvider();
            //TripleDES des = new TripleDESCryptoServiceProvider();
            //des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            //des.IV = new byte[des.BlockSize / 8];
            //return des;              
            byte[] keys = Encoding.ASCII.GetBytes(key);
            TripleDES des = TripleDES.Create();
            des.Key = keys;
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }
        #endregion "Data Encryption & Decryption Process"

        public static string DecryptEncryptedMEK(string CypherText)
        {
            ////**************Used
            string DecryptedText = DecryptionEncryptedMEK(CypherText);
            return DecryptedText;
        }
        public static string DecryptEncryptedDEK(string CypherText, string PlainText)
        {
            ////**************Used
            string DecryptedText = DecryptionEncryptedDEK(CypherText, PlainText);
            return DecryptedText;
        }


        public static string DecryptionEncryptedMEK(string CypherText)
        {

            ////**************Used
            byte[] b = Convert.FromBase64String(CypherText);
            string S = KEKKeys.Key();
            TripleDES des = CreateDES(S);
            des.Mode = CipherMode.CBC;
            ICryptoTransform ct = des.CreateDecryptor();
            string sIV = "0000000000000000";
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Convert.ToString(Encoding.Unicode.GetString(output));
        }
        public static string DecryptionConnectionstring(string CypherText)
        {
            ////**************Decryption Of Connectionstring*************************

            byte[] b = Convert.FromBase64String(CypherText);
            string S = ClearMEK;
            TripleDES des = CreateDES(S);
            des.Mode = CipherMode.CBC;
            ICryptoTransform ct = des.CreateDecryptor();
            string sIV = "0000000000000000";
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Convert.ToString(Encoding.Unicode.GetString(output));
        }
        public static string DecryptionEncryptedDEK(string CypherText, string PlainText)
        {
            ////**************Used
            byte[] b = Convert.FromBase64String(CypherText);
            string S = PlainText;
            TripleDES des = CreateDES(S);
            des.Mode = CipherMode.CBC;
            ICryptoTransform ct = des.CreateDecryptor();
            string sIV = "0000000000000000";
            Console.WriteLine("IV --->" + sIV);
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Convert.ToString(Encoding.Unicode.GetString(output));
        }




        public static string Load()
        {
            ////**************Initialisation of MEK Key***********************
            string EncryptedDEK = string.Empty;
            //string S = System.Configuration.ConfigurationManager.AppSettings["MekKey"].ToString();               
            //ClearMEK = DALC.ConnectionStringEncryptDecrypt.DecryptEncryptedMEK(S);
            //return S;
            string MEK1 = System.Configuration.ConfigurationManager.AppSettings["MekKey1"].ToString();
            string MEK2 = System.Configuration.ConfigurationManager.AppSettings["MekKey2"].ToString();
            ClearMEK = DALC.ConnectionStringEncryptDecrypt.DecryptEncryptedMEK(MEK1);
            ClearMEK2 = DALC.ConnectionStringEncryptDecrypt.DecryptEncryptedMEK(MEK2);
            return ClearMEK;

        }

        public static string GETENCRYPTEDDEK(string ClearConnectionstring)
        {

            ////***************Taking DEK Value from DB******************
            SqlConnection conn = new SqlConnection(ClearConnectionstring);
            string EncryptedDEK = string.Empty;
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            { }

            //SqlDataAdapter da = new SqlDataAdapter("Select top 1 TMK_LMK From TBLENCRYPTION", conn);               
            SqlCommand cmd = new SqlCommand("GETENCRYPTEDDEK", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);

                EncryptedDEK = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                conn.Close();
            }
            catch (Exception ex)
            { }

            ////***************Taking Encrypted DEK Value from Database******************              
            //ClearDEK = DALC.ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(EncryptedDEK, ClearMEK);
            ClearDEK = DALC.ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(EncryptedDEK, ClearMEK2);

            return ClearDEK;

        }
        public static string GetDataEncryptionKey(string ConnectionString, string Provider)
        {
            string cypherText = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                using (DbNetData dbNetData = new DbNetData(ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), Provider)))
                {
                    dbNetData.Open();
                    QueryCommandConfig cmdConfig = new QueryCommandConfig("GetIMPSEncryptedDEK");
                    dataTable = dbNetData.GetDataTable(cmdConfig);
                    cypherText = dataTable.Rows[0][0].ToString();
                    ConnectionStringEncryptDecrypt.ClearDEK = ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(cypherText, ConnectionStringEncryptDecrypt.ClearMEK2);
                    dbNetData.Close();
                    dbNetData.Dispose();
                }
            }
            catch (Exception var_4_96)
            {
            }
            return ConnectionStringEncryptDecrypt.ClearDEK;
        }

        public static string DecryptConnectionString(string CypherText)
        {
            ////**************Decryption Of Connectionstring*************************
            string DecryptedText = DecryptionConnectionstring(CypherText);
            return DecryptedText;
        }

        ////******************* Method Used For Encryption Of Password Using SHA - 2 Algorithm ********************////

        public static string EncryptUsingSHA2Algorithm(string password)
        {
            UnicodeEncoding Encode = new UnicodeEncoding();
            byte[] bytclearstring = Encode.GetBytes(password.ToString());
            SHA256Managed sh256managed = new SHA256Managed();
            byte[] hash = sh256managed.ComputeHash(bytclearstring);
            return Convert.ToBase64String(hash);
        }


        public static DataTable GTQSDtl(string ConnectionString, string Provider)
        {
            DataTable DTCustomerData = new DataTable();
            DTCustomerData = null;
            DataTable DTACCCLASS = new DataTable();
            DTACCCLASS = null;
            DataTable DTCustomerData_New = new DataTable();
            DTCustomerData_New = null;
            try
            {

                using (DbNetData dbNetData = new DbNetData(ConnectionString, (DataProvider)Enum.Parse(typeof(DataProvider), Provider)))
                {
                    dbNetData.Open();
                    QueryCommandConfig cmdConfig = new QueryCommandConfig("Proc_SelectQS2Details");

                    DTCustomerData_New = dbNetData.GetDataTable(cmdConfig);
                }
                return DTCustomerData_New;

            }
            catch (Exception ex)
            {
                // DalcLogger.WriteErrorLog(null, ex);
                return DTCustomerData_New = null;
            }

        }

    }

    //}

    //}
}
