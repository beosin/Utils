using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace OL.Utils.Extensions
{
    public enum EncodingType
    {
        ASCII,
        UTF8,
        gbk,
        gb2312,
        Default,
        Unicode
    }
    public static  class BytesEx
    {    
        public static byte[] ToBytes(this string value, EncodingType type = EncodingType.UTF8)
        {
            switch (type)
            {
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetBytes(value);

                case EncodingType.UTF8:
                    return Encoding.UTF8.GetBytes(value);

                case EncodingType.gbk:
                    return Encoding.GetEncoding("gbk").GetBytes(value);

                case EncodingType.gb2312:
                    return Encoding.GetEncoding("gb2312").GetBytes(value);

                case EncodingType.Default:
                    return Encoding.Default.GetBytes(value);

                case EncodingType.Unicode:
                    return Encoding.Unicode.GetBytes(value);

                default:
                    return Encoding.UTF8.GetBytes(value);
            }
        }
        /// <summary>
        ///    
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(EncodingType type = EncodingType.UTF8)
        {
            switch (type)
            {
                case EncodingType.ASCII:
                    return Encoding.ASCII;
                case EncodingType.UTF8:
                    return Encoding.UTF8;
                case EncodingType.gbk:
                    return Encoding.GetEncoding("gbk");
                case EncodingType.gb2312:
                    return Encoding.GetEncoding(936);
                case EncodingType.Default:
                    return Encoding.Default;
                case EncodingType.Unicode:
                    return Encoding.Unicode;
                default:
                    return Encoding.UTF8;
            }
        }
        public static string ByteToString(this byte[] bytes, EncodingType type = EncodingType.UTF8)
        {
            switch (type)
            {
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetString(bytes);

                case EncodingType.UTF8:
                    return Encoding.UTF8.GetString(bytes);

                case EncodingType.gbk:
                    return Encoding.GetEncoding("gbk").GetString(bytes);

                case EncodingType.gb2312:
                    return Encoding.GetEncoding("gb2312").GetString(bytes);

                case EncodingType.Default:
                    return Encoding.Default.GetString(bytes);

                case EncodingType.Unicode:
                    return Encoding.Unicode.GetString(bytes);

                default:
                    return Encoding.UTF8.GetString(bytes);
            }
        }
        public static byte[] ToBytes(this object value)
        {
            if (value.IsNullT())
            {
                return default;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);
                return ms.GetBuffer();
            }
        }
        public static Stream ToStream(this object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);
                return ms;
            }
        }
        public static Stream ToStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return stream;
        }
        public static T ToObject<T>(this byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }

        public static BinaryReader GetBinaryReader(this byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryReader = new BinaryReader(memoryStream);
                return binaryReader;
            }
        }

        public static T ToObject<T>(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                ms.WriteTo(stream);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }

        public static byte[] ToBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }             
        }

        public static string ReadToString(this Stream stream, Encoding encoding)
        {
            string resStr = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            resStr = new StreamReader(stream, encoding).ReadToEnd();
            stream.Seek(0, SeekOrigin.Begin);
            return resStr;
        }

        public static byte[] Compress(this byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(this byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var input = new MemoryStream(bytes))
                using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                {
                    gzip.CopyTo(ms);
                }
                return ms.ToArray();
            }
        }

        public static byte[] CompressD(this byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var def = new DeflateStream(ms, CompressionMode.Compress))
                {
                    def.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }
        public static byte[] DecompressD(this byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var input = new MemoryStream(bytes))
                using (var def = new DeflateStream(input, CompressionMode.Decompress))
                {
                    def.CopyTo(ms);
                }
                return ms.ToArray();
            }
        }

        #region SetBit
        /// <summary>设置数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static UInt16 SetBit(this UInt16 value, Int32 position, bool flag)
        {
            return SetBits(value, position, 1, (flag ? (Byte)1 : (Byte)0));
        }

        /// <summary>设置数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static UInt16 SetBits(this UInt16 value, Int32 position, Int32 length, UInt16 bits)
        {
            if (length <= 0 || position >= 16) return value;
            var mask = (2 << (length - 1)) - 1;
            value &= (UInt16)~(mask << position);
            value |= (UInt16)((bits & mask) << position);
            return value;
        }

        /// <summary>设置数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static Byte SetBit(this Byte value, Int32 position, bool flag)
        {
            if (position >= 8) return value;
            var mask = (2 << (1 - 1)) - 1;
            value &= (Byte)~(mask << position);
            value |= (Byte)(((flag ? (Byte)1 : (Byte)0) & mask) << position);
            return value;
        }
        #endregion
        #region GetBit
        /// <summary>获取数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool  GetBit(this UInt16 value, Int32 position)
        {
            return GetBits(value, position, 1) == 1;
        }

        /// <summary>获取数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static UInt16 GetBits(this UInt16 value, Int32 position, Int32 length)
        {
            if (length <= 0 || position >= 16) return 0;
            var mask = (2 << (length - 1)) - 1;
            return (UInt16)((value >> position) & mask);
        }

        /// <summary>获取数据位</summary>
        /// <param name="value">数值</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool  GetBit(this Byte value, Int32 position)
        {
            if (position >= 8) return false;
            var mask = (2 << (1 - 1)) - 1;
            return ((Byte)((value >> position) & mask)) == 1;
        } 
        #endregion
    }
}
