using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SnapRegisters
{
    public static class LogoOperations
    {



        public static System.Drawing.Image StringToImage(String hex)
        {
            return byteArrayToImage(StringToByteArray(hex));
        }

        public static string ImageToString(System.Drawing.Image img)
        {
            return ByteArrayToString(imgToByteArray(img));
        }










        //converts string to bytearray
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        //converts bytearray to string
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        //convert image to bytearray
        private static byte[] imgToByteArray(System.Drawing.Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
        //convert bytearray to image
        private static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return System.Drawing.Image.FromStream(mStream);
            }
        }
    }
}
