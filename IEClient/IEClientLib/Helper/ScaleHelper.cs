using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Helper
{
    public class ScaleHelper
    {
        /// <summary>
        /// 十进制数字转十六进制字符串
        /// </summary>
        /// <param name="deci">十进制数</param>
        /// <param name="upcase">是否大写字符</param>
        /// <param name="hexStringLength">十六进制字符串长度，长度为4的例子00FF,0001,FFFF...</param>
        /// <returns></returns>
        public static string DecimalToHexString(int deci, bool upcase = true, int? hexStringLength = 4)
        {
            string hexString = Convert.ToString(deci, 16);
            if (hexStringLength.HasValue)
            {
                hexString = hexString.PadLeft(hexStringLength.Value, '0');
            }
            return upcase ? hexString.ToUpper() : hexString;
        }

        /// <summary>
        /// 十六进制字符转十进制数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int HexByteToDecimal(byte data)
        {
            return Convert.ToInt32(HexBytesToString(new byte[1] { data },false), 16);
        }


        /// <summary>
        /// 十六进制字符数组转十进制数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int HexBytesToDecimal(byte[] data)
        {
            return Convert.ToInt32(HexBytesToString(data,false), 16);
        }

        /// <summary>
        /// HEX字符串转换为HEX数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// HEX数值转换为HEX字符串
        /// </summary>
        /// <param name="hexBytes"></param>
        /// <param name="withBlank">是否已空格分隔</param>
        /// <returns></returns>
        public static string HexBytesToString(byte[] hexBytes, bool withBlank=true)
        {
            string hexString = string.Empty;
            if (hexBytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < hexBytes.Length; i++)
                {
                    if (withBlank)
                    {
                        strB.Append(hexBytes[i].ToString("X2") + " ");
                    }
                    else {
                        strB.Append(hexBytes[i].ToString("X2"));
                    }
                }

                hexString = strB.ToString().TrimEnd();
            }
            return hexString;
        }
        
        /// <summary>
        /// 计算字节累加和，保留从低位到高位2字节数的结果
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CalculateFcc(byte[] bytes)
        {

            int num = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                num = (num + bytes[i]) % 0xffff;
            }
            bytes = BitConverter.GetBytes(num);
            return new byte[] { bytes[1], bytes[0] };
        }
    }
}
