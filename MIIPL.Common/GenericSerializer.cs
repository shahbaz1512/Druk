using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace MIIPL.Common
{
    #region GenericSerializer
    /// <summary>
    /// A generic class used to serialize objects.
    /// </summary>
    public class GenericSerializer
    {
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString.Replace("﻿<?xml", "<?xml"));
            //string outString = System.Text.Encoding.Unicode.GetString(characters);
            //return outString.Replace("﻿<?xml", "<?xml");
        }
        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;

            //return System.Text.Encoding.Unicode.GetBytes(pXmlString);
        }
        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static String Serialize(Object pObject, Type t)
        {
            try
            {
                //XmlSerializer xs1 = new XmlSerializer(t);//pObject.GetType()); //(t);
                //StringBuilder sb = new StringBuilder();
                //xs1.Serialize(sw, pObject);
                //return sb.ToString();

                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(t);//pObject.GetType()); //(t);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                //xmlTextWriter.Formatting = Formatting.Indented;
                //xmlTextWriter.Indentation = 5;// int.Parse(5);
                //xmlTextWriter.IndentChar = ' ';//(radioButton3.Checked ? ' ' : '\t');

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static String Serialize(Object pObject, Type t, Type[] extraTypes)
        {
            //XmlSerializer xs = null;
            StringWriter sw = null;
            try
            {
                //xs = new XmlSerializer(t, extraTypes);
                //sw = new StringWriter();
                //xs.Serialize(sw, pObject);
                //sw.Flush();
                //return sw.ToString();
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(t, extraTypes);//pObject.GetType()); //(t);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                //xmlTextWriter.Formatting = Formatting.Indented;
                //xmlTextWriter.Indentation = 5;// int.Parse(5);
                //xmlTextWriter.IndentChar = ' ';//(radioButton3.Checked ? ' ' : '\t');

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                XmlizedString = XmlizedString.Replace("﻿<?xml", "<?xml");
                return XmlizedString;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="writer">The writer to be used for output in the serialization.</param>
        public static void Serialize(Object obj, XmlWriter writer, Type T)
        {
            XmlSerializer xs = new XmlSerializer(T);
            xs.Serialize(writer, obj);
        }
        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="writer">The writer to be used for output in the serialization.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///       of additional object types to serialize.</param>
        public static void Serialize(Object obj, XmlWriter writer, Type T, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(T, extraTypes);
            xs.Serialize(writer, obj);
        }
        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static Object Deserialize(String pXmlizedString, Type t)
        {
            try
            {
                pXmlizedString = pXmlizedString.Replace("utf-16", "utf-8");
                pXmlizedString = pXmlizedString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "");
                XmlSerializer xs = new XmlSerializer(t);
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                return xs.Deserialize(memoryStream);

                //XmlSerializer xmlSerializer = new XmlSerializer(t);

                //StringReader stringReader = new StringReader(pXmlizedString);

                //return xmlSerializer.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static Object Deserialize(String pXmlizedString, Type t, Type[] extraTypes)
        {
            try
            {
                pXmlizedString = pXmlizedString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "");
                XmlSerializer xs = new XmlSerializer(t, extraTypes);
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                return xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="reader">The reader used to retrieve the serialized object.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static Object Deserialize(XmlReader reader, Type T)
        {
            XmlSerializer xs = new XmlSerializer(T);
            return xs.Deserialize(reader);
        }
        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="reader">The reader used to retrieve the serialized object.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///           of additional object types to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static Object Deserialize(XmlReader reader, Type T, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(T, extraTypes);
            return xs.Deserialize(reader);
        }
        public static void SaveAs(Object Obj, string FileName, Encoding encoding, Type T, Type[] extraTypes)
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(FileName));
            if (!di.Exists)
                di.Create();
            XmlDocument document = new XmlDocument();
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            wSettings.Encoding = encoding;
            wSettings.CloseOutput = true;
            wSettings.CheckCharacters = false;
            using (XmlWriter writer = XmlWriter.Create(FileName, wSettings))
            {
                if (extraTypes != null)
                    Serialize(Obj, writer, T, extraTypes);
                else
                    Serialize(Obj, writer, T);
                writer.Flush();
                document.Save(writer);
            }
        }
        public static void SaveAs(Object Obj, string FileName, Type T, Type[] extraTypes)
        {
            //SaveAs(Obj, FileName, Encoding.UTF8, T, extraTypes);
            SaveAs(Obj, FileName, Encoding.Unicode, T, extraTypes);
        }
        public static void SaveAs(Object Obj, string FileName, Encoding encoding, Type T)
        {
            SaveAs(Obj, FileName, encoding, T, null);
        }
        public static void SaveAs(Object Obj, string FileName, Type T)
        {
            //SaveAs(Obj, FileName, Encoding.UTF8, T);
            SaveAs(Obj, FileName, Encoding.Unicode, T);
        }
        public static Object Open(string FileName, Type T, Type[] extraTypes)
        {
            Object obj = null;
            if (File.Exists(FileName))
            {
                XmlReaderSettings rSettings = new XmlReaderSettings();
                rSettings.CloseInput = true;
                rSettings.CheckCharacters = false;
                using (XmlReader reader = XmlReader.Create(FileName, rSettings))
                {
                    reader.ReadOuterXml();
                    if (extraTypes != null)
                        obj = Deserialize(reader, T, extraTypes);
                    else
                        obj = Deserialize(reader, T);
                }
            }
            return obj;
        }
        public static Object Open(string FileName, Type T)
        {
            return Open(FileName, T, null);
        }


        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>String representation of the serialized object.</returns>
        public static string Serialize<T>(T obj)
        {
            XmlSerializer xs = null;
            StringWriter sw = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                sw = new StringWriter();
                xs.Serialize(sw, obj);
                sw.Flush();
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        public static string Serialize<T>(T obj, Type[] extraTypes)
        {
            XmlSerializer xs = null;
            StringWriter sw = null;
            try
            {
                xs = new XmlSerializer(typeof(T), extraTypes);
                sw = new StringWriter();
                xs.Serialize(sw, obj);
                sw.Flush();
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="writer">The writer to be used for output in the serialization.</param>
        public static void Serialize<T>(T obj, XmlWriter writer)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(writer, obj);
        }
        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="writer">The writer to be used for output in the serialization.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///       of additional object types to serialize.</param>
        public static void Serialize<T>(T obj, XmlWriter writer, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            xs.Serialize(writer, obj);
        }
        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="reader">The reader used to retrieve the serialized object.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T Deserialize<T>(XmlReader reader)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(reader);
        }
        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="reader">The reader used to retrieve the serialized object.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///           of additional object types to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T Deserialize<T>(XmlReader reader, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            return (T)xs.Deserialize(reader);
        }
        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="XML">The XML file containing the serialized object.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T Deserialize<T>(string XML)
        {
            if (XML == null || XML == string.Empty)
                return default(T);
            XmlSerializer xs = null;
            StringReader sr = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                sr = new StringReader(XML);
                return (T)xs.Deserialize(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
        public static T Deserialize<T>(string XML, Type[] extraTypes)
        {
            if (XML == null || XML == string.Empty)
                return default(T);
            XmlSerializer xs = null;
            StringReader sr = null;
            try
            {
                xs = new XmlSerializer(typeof(T), extraTypes);
                sr = new StringReader(XML);
                return (T)xs.Deserialize(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
        public static void SaveAs<T>(T Obj, string FileName, Encoding encoding, Type[] extraTypes)
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(FileName));
            if (!di.Exists)
                di.Create();
            XmlDocument document = new XmlDocument();
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            wSettings.Encoding = encoding;
            wSettings.CloseOutput = true;
            wSettings.CheckCharacters = false;
            using (XmlWriter writer = XmlWriter.Create(FileName, wSettings))
            {
                if (extraTypes != null)
                    Serialize<T>(Obj, writer, extraTypes);
                else
                    Serialize<T>(Obj, writer);
                writer.Flush();
                document.Save(writer);
            }
        }
        public static void SaveAs<T>(T Obj, string FileName, Type[] extraTypes)
        {
            SaveAs<T>(Obj, FileName, Encoding.UTF8, extraTypes);
        }
        public static void SaveAs<T>(T Obj, string FileName, Encoding encoding)
        {
            SaveAs<T>(Obj, FileName, encoding, null);
        }
        public static void SaveAs<T>(T Obj, string FileName)
        {
            SaveAs<T>(Obj, FileName, Encoding.UTF8);
        }
        public static T Open<T>(string FileName, Type[] extraTypes)
        {
            T obj = default(T);
            if (File.Exists(FileName))
            {
                XmlReaderSettings rSettings = new XmlReaderSettings();
                rSettings.CloseInput = true;
                rSettings.CheckCharacters = false;
                using (XmlReader reader = XmlReader.Create(FileName, rSettings))
                {
                    reader.ReadOuterXml();
                    if (extraTypes != null)
                        obj = Deserialize<T>(reader, extraTypes);
                    else
                        obj = Deserialize<T>(reader);
                }
            }
            return obj;
        }
        public static T Open<T>(string FileName)
        {
            return Open<T>(FileName, null);
        }


    }
    #endregion
}
