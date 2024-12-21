using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using Microsoft.VisualBasic;

namespace MIIPL.Common
{
    public class Utils
    {
        public enum EncodeType
        {
            ASCII,
            EBCDIC,
        }

        public static void GetStringArray2AlrrayList(string[] data,ref ArrayList Al,int MaxMessageSize)
        {
           int CurrentSize=0;
           string tmp = string.Empty;
           for (int i = 0; i < data.Length; i++)
           {
               CurrentSize = CurrentSize + data[i].Length;
               if (CurrentSize > MaxMessageSize)
               {
                   Al.Add(tmp);
                   CurrentSize = data[i].Length;
                   tmp = string.Empty;
               }
               tmp = tmp + data[i];
           }
        }

        public static string GetStringArray2String(string[] data)
        {
            string Msg = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                Msg = Msg + data[i];
            }
            return Msg;
        }

        public static string GetStringArray2StringWithdelimiter(string[] data,char Delimiter)
        {
            string Msg = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                Msg = Msg + Delimiter + data[i];
            }
            return Msg.TrimStart(Delimiter);
        }        

        
        public static string ASCII2Binary(string asciivalue)
        {
            string binaryval = "";
            foreach (char c in asciivalue.ToCharArray())
                binaryval = binaryval + Convert.ToString((int)c, 2).PadLeft(8, '0');
            return binaryval;
        }

        public static string Binary2ASCII(string binaryvalue)
        {
            string binaryval = "";
            for (int i = 0; i < binaryvalue.Length - 7; i += 8)
                binaryval = binaryval + (char)(Convert.ToInt32(binaryvalue.Substring(i, 8), 2));
            return binaryval;
        }

        public static string HEX2Binary(string hexvalue)
        {
            string binaryval = "";
            foreach (char c in hexvalue.ToCharArray())
                binaryval = binaryval + Convert.ToString(Convert.ToInt32(c + "", 16), 2).PadLeft(4, '0');
            return binaryval;
        }
        public static string Binary2HEX(string binaryvalue)
        {
            string binaryval = "";
            for (int i = 0; i < binaryvalue.Length - 7; i += 8)
                binaryval = binaryval + Convert.ToString(Convert.ToInt32(binaryvalue.Substring(i, 8), 2), 16).PadLeft(2, '0');
            return binaryval;
        }

        public static string ASCII2HEX(string asciivalue)
        {
            string binaryval = "";
            foreach (char c in asciivalue.ToCharArray())
                binaryval = binaryval + Convert.ToString((int)c, 16).PadLeft(2, '0');
            return binaryval;
        }
        public static string HEX2ASCII(string hexvalue)
        {
            string binaryval = "";
            for (int i = 0; i < hexvalue.Length; i += 2)
                binaryval = binaryval + (char)Convert.ToInt32(hexvalue.Substring(i, 2), 16);
            return binaryval;
        }

        public static string ByteArrayToHex(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(); //(ba.Length * 2);
            if (byteArray != null)
                foreach (byte b in byteArray)
                    hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] HexToByteArray(String hex)
        {
            try
            {
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
            catch(Exception ex)
            { return null; }
        }

        
        public static string ByteArrayToASCII(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();
            if (byteArray != null)
                for (int i = 0; i < byteArray.Length; i++)
                    sb.Append((char)byteArray[i]);
            return sb.ToString();
            //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //return encoding.GetString(byteArray);
        }
        public static byte[] ASCIIToByteArray(string str)
        {
            char[] chars = str.ToCharArray();
            byte[] result = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)
                result[i] = (byte)chars[i];
            return result;           
        }

        public static byte[] GetSubBytes(byte[] source, int startIndex, int endIndex)
        {
            int BytesToRead = endIndex - startIndex + 1;
            byte[] result = new byte[BytesToRead];
            Array.Copy(source, startIndex, result, 0, BytesToRead);
            return result;
        }

        public static string GetBitmap(params int[] fields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 128; i++)
                sb.Append("0");
            string bitmap = sb.ToString();
            foreach (int idx in fields)
            {
                if (idx == 1)
                    bitmap = "1" + bitmap.Substring(1, bitmap.Length - idx);
                else if (idx == 128)
                {
                    bitmap = bitmap.Substring(0, 127) + "1";
                }
                else
                {
                    bitmap = bitmap.Substring(0, idx - 1) + "1" + bitmap.Substring(idx, bitmap.Length - idx);
                }
            }
            if (fields.Max() > 64 && bitmap.Substring(0, 1) == "0")
                bitmap = "1" + bitmap.Substring(1, bitmap.Length - 1);
            return bitmap;
        }

        public static double GetAmountToDouble(string Amount)
        {
            if (Amount != null && Amount.Length >= 2)
                return double.Parse(Amount.Substring(0, Amount.Length - 2));
            else
                return double.Parse(Amount);
        }
        public static string GetDoubleToAmount(double Amount)
        {
            return Amount.ToString("#0.00").Replace(".", "");
        }

        public static string XORHexStringsFull(string msg1, string msg2)
        {
            string result = string.Empty;
            for (int i = 0; i < msg1.Length; i++)
            {
                result = result + (Convert.ToInt32(msg1.Substring(i, 1), 16) ^ Convert.ToInt32(msg2.Substring(i, 1), 16)).ToString("X");
            }
            return result;
        }
        public static string XORHexStrings(string msg1, string msg2)
        {
            return XORHexStringsFull(msg1.Substring(0, 16), msg2.Substring(0, 16));
        }

        public static string ModuloAdd(string str1, string str2)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    int intOpt1 = Int32.Parse(str1.Substring(i, 1));
                    int intOpt2 = Int32.Parse(str2.Substring(i, 1));
                    result = result + ((int)((intOpt1 + intOpt2) % 10)).ToString();
                }
            }
            catch //(Exception ex)
            {
                result = null;
            }
            return result;
        }
        public static string ModuloSubstract(string str1, string str2)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    int intOpt1 = Int32.Parse(str1.Substring(i, 1));
                    int intOpt2 = Int32.Parse(str2.Substring(i, 1));
                    int intResult = intOpt2 - intOpt1;
                    if (intResult < 0) intResult = intResult + 10;
                    result = result + intResult.ToString();                    
                }
            }
            catch //(Exception ex)
            {
                result = null;
            }
            return result;
        }

        public static string EBCDIC2HEX(string strInput)
        {
            string result = string.Empty;
            Hashtable colEBCDIC2HEX = new Hashtable();
            colEBCDIC2HEX.Add("0", "0");
            colEBCDIC2HEX.Add("1", "1");
            colEBCDIC2HEX.Add("2", "2");
            colEBCDIC2HEX.Add("3", "3");
            colEBCDIC2HEX.Add("4", "4");
            colEBCDIC2HEX.Add("5", "5");
            colEBCDIC2HEX.Add("6", "6");
            colEBCDIC2HEX.Add("7", "7");
            colEBCDIC2HEX.Add("8", "8");
            colEBCDIC2HEX.Add("9", "9");
            colEBCDIC2HEX.Add(":", "A");
            colEBCDIC2HEX.Add(";", "B");
            colEBCDIC2HEX.Add("<", "C");
            colEBCDIC2HEX.Add("=", "D");
            colEBCDIC2HEX.Add(">", "E");
            colEBCDIC2HEX.Add("?", "F");
            try
            {
                int len = strInput.Length;
                for (int i = 0; i < len; i++)
                {
                    //string st1 = EBCDICHex(strInput.Substring(i, 1));
                    //strTempOutput = strTempOutput + st1.Substring(st1.Length - 1);
                    try
                    {
                        string st1 = colEBCDIC2HEX[strInput.Substring(i, 1)].ToString();
                        result = result + st1;
                    }
                    catch
                    {
                        result = result + strInput.Substring(i, 1);
                    }
                }
            }
            catch //(Exception ex)
            {
                //modLOGS.ErrorLog1(ex.Message, ex.Source, ex.StackTrace);
            }
            finally
            {
                colEBCDIC2HEX = null;
            }
            return result;
        }
        public static string HEX2EBCDIC(string strInput)
        {
            string result = string.Empty;
            Hashtable colHEX2EBCDIC = new Hashtable();
            colHEX2EBCDIC.Add("0", "0");
            colHEX2EBCDIC.Add("1", "1");
            colHEX2EBCDIC.Add("2", "2");
            colHEX2EBCDIC.Add("3", "3");
            colHEX2EBCDIC.Add("4", "4");
            colHEX2EBCDIC.Add("5", "5");
            colHEX2EBCDIC.Add("6", "6");
            colHEX2EBCDIC.Add("7", "7");
            colHEX2EBCDIC.Add("8", "8");
            colHEX2EBCDIC.Add("9", "9");
            colHEX2EBCDIC.Add("A", ":");
            colHEX2EBCDIC.Add("B", ";");
            colHEX2EBCDIC.Add("C", "<");
            colHEX2EBCDIC.Add("D", "=");
            colHEX2EBCDIC.Add("E", ">");
            colHEX2EBCDIC.Add("F", "?");
            try
            {
                int len = strInput.Length;
                for (int i = 0; i < len; i++)
                {
                    //string st1 = HexEBCDIC(strInput.Substring(i, 1));
                    //strTempOutput = strTempOutput + st1.Substring(st1.Length - 1);
                    try
                    {
                        string st1 = colHEX2EBCDIC[strInput.Substring(i, 1)].ToString();
                        result = result + st1;
                    }
                    catch
                    {
                        result = result + strInput.Substring(i, 1);
                    }
                }
            }
            catch //(Exception ex)
            {
                //modLOGS.ErrorLog1(ex.Message, ex.Source, ex.StackTrace);
            }
            finally
            {
                colHEX2EBCDIC = null;
            }
            return result;
        }

        public static string ToHEX(string source, Utils.EncodeType encodeType)
        {
            string result = string.Empty;
            try
            {
                if (source.Length != 0x10) return result;
                switch (encodeType)
                {
                    case Utils.EncodeType.ASCII:
                        result = Utils.ASCII2HEX(source);
                        break;
                    case Utils.EncodeType.EBCDIC:
                        result = Utils.EBCDIC2HEX(source);
                        break;
                }
            }
            catch //(Exception ex)
            {
                //WriteError(ex.Message);
            }
            return result;
        }
        public static string FromHEX(string source, Utils.EncodeType encodeType)
        {
            string result = string.Empty;
            try
            {
                if (source.Length != 0x10) return result;
                switch (encodeType)
                {
                    case Utils.EncodeType.ASCII:
                        result = Utils.HEX2ASCII(source);
                        break;
                    case Utils.EncodeType.EBCDIC:
                        result = Utils.HEX2EBCDIC(source);
                        break;
                }
            }
            catch //(Exception ex)
            {
                //WriteError(ex.Message);
            }
            return result;
        }

        public static bool IsHexString(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!char.IsDigit(source, i))
                {
                    //if ((msg.Substring(i, 1) < "A") || (msg.Substring(i, 1) > "F"))
                    if (source[i] < 'A' || source[i] > 'F')
                        return false;
                }
            }
            return true;
        }

        public static string DecimalToHEX(int length)
        {
            return length.ToString("X");
        }
        public static string DecimalToHEX(long length)
        {
            return length.ToString("X");
        }


    }
}
