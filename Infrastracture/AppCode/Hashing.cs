using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastracture.AppCode
{
    public static class Hashing
    {
        

        #region private methods

        private static byte[] TreeData(byte[] value)
        {
            MemoryStream mem = new MemoryStream(value);

            TripleDESCryptoServiceProvider tr = new TripleDESCryptoServiceProvider();

            tr.IV = new byte[] { 193, 23, 229, 126, 74, 171, 56, 49 };

            tr.Key = new byte[] { 161, 182, 70, 70, 27, 87, 10, 98, 12, 32, 82, 73, 92, 23, 45, 61, 53, 102, 48, 54, 7, 82, 18, 23 };

            new CryptoStream(mem, tr.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] buffer = new byte[mem.Length];

            mem.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        private static byte[] ConvertObjectToByteArray(object value)
        {
            MemoryStream Temp = new MemoryStream();

            BinaryFormatter bin = new BinaryFormatter();

            bin.Serialize(Temp, value);

            return Temp.ToArray();
        }

        private static object ConvertByteArrayToObject(byte[] value)
        {
            MemoryStream Temp = new MemoryStream(value);

            BinaryFormatter bin = new BinaryFormatter();

            return bin.Deserialize(Temp);
        }

        private static byte[] RowData(byte[] value)
        {
            MemoryStream Dmem = new MemoryStream(value);

            TripleDESCryptoServiceProvider tr = new TripleDESCryptoServiceProvider();

            tr.IV = new byte[] { 193, 23, 229, 126, 74, 171, 56, 49 };

            tr.Key = new byte[] { 161, 182, 70, 70, 27, 87, 10, 98, 12, 32, 82, 73, 92, 23, 45, 61, 53, 102, 48, 54, 7, 82, 18, 23 };

            new CryptoStream(Dmem, tr.CreateDecryptor(tr.Key, tr.IV), CryptoStreamMode.Read);

            byte[] buffer = new byte[Dmem.Length];

            Dmem.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        private static byte[] StringToByte(string InString)
        {

            string[] ByteStrings;

            ByteStrings = InString.Split(',');

            byte[] ByteOut;

            ByteOut = new byte[ByteStrings.Length];

            int Counter = 0;

            foreach (string item in ByteStrings)
            {
                ByteOut[Counter] = Convert.ToByte(item);

                Counter++;
            }

            return ByteOut;

        }

        #endregion


        #region public methods
        
        public static string Convert2SHA(string temp)
        {
            SHA512 mySHATemp = new SHA512Managed();

            byte[] MyData = Encoding.Default.GetBytes(temp);

            byte[] HashedData = mySHATemp.ComputeHash(MyData);

            StringBuilder TempData = new StringBuilder();

            foreach (byte item in HashedData)
            {
                TempData.Append(item.ToString("X"));
            }

            return TempData.ToString();
        }

        public static string Convert2MD5(string InputString)
        {
            string OutString = string.Empty;

            UTF8Encoding encoder = new UTF8Encoding();

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            byte[] temp = new byte[encoder.GetByteCount(InputString)];

            temp = md5Hasher.ComputeHash(encoder.GetBytes(InputString));


            foreach (byte item in temp)
            {
                OutString += item.ToString();
            }

            return OutString;
        }

        public static string MultiEncrypt(string value)
        {
            return Convert2MD5(Convert2MD5(Convert2SHA(Convert2SHA(value))));
        }

        public static string EncryptTPL(string value)
        {
            byte[] EncryptArray = TreeData(ConvertObjectToByteArray(value));

            string temp = string.Empty;

            foreach (byte item in EncryptArray)
            {
                temp += item + ",";
            }

            temp = temp.Substring(0, temp.Length - 1);

            return temp;
        }

        public static string DecryptTPL(string value)
        {
            byte[] array = Hashing.StringToByte(value);

            return (string)ConvertByteArrayToObject(RowData(array));
        }

        #endregion


    }
}