using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace LinFx.Utils
{
    public class EncryptUtils
    {
        #region DES
        //默认密钥向量
        private static readonly byte[] keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string DESEncode(string encryptString, string encryptKey = "Key!123456789#AB")
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
            byte[] rgbIV = keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 

        public static string DESDecrypt(string decryptString, string decryptKey = "Key!123456789#AB")
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
            byte[] rgbIV = keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            Byte[] inputByteArrays = new byte[inputByteArray.Length];
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        #endregion

        #region MD5
        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="input">The string to be encrypted.</param>
        /// <param name="length">The length of hash result , default value is <see cref="MD5Length.L32"/>.</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input)
        {
            Check.NotNull(input, nameof(input));

            using (var md5 = MD5.Create())
            {
                var bytes_md5_out = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes_md5_out).Replace("-", "");
            }
        }
        #endregion

        #region RSA

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string input, string publickey)
        {
            publickey = @"<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //byte[] cipherbytes;
            //rsa.FromXmlString(publickey);
            //cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            using (var rsa = RSA.Create())
            {
                rsa.FromLvccXmlString(publickey);
                var buffer = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.OaepSHA512);
                return Convert.ToBase64String(buffer);
            }
        }

        //public static string RSAEncrypt2(string publicKey, string srcString)
        //{
        //    using (RSA rsa = RSA.Create())
        //    {
        //        rsa.FromJsonString(publicKey);
        //        byte[] encryptBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(srcString), RSAEncryptionPadding.OaepSHA512);
        //        return encryptBytes.ToHexString();
        //    }
        //}

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string content, string privatekey)
        {
            privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">数据</param>
        /// <param name="privateKey">RSA密钥</param>
        /// <returns></returns>
        public static string RSASign_MD5withRSA_GBK(string content, string privateKey)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("GBK");
            return RSASign_MD5withRSA(content, privateKey, encoding);
        }

        public static string RSASign_MD5withRSA(string content, string privateKey)
        {
            return RSASign_MD5withRSA(content, privateKey, Encoding.UTF8);
        }

        public static string RSASign_MD5withRSA(string content, string privateKey, Encoding encoding)
        {
            return RSASign("MD5withRSA", content, privateKey, encoding);
        }

        public static string RSASign_SHA1withRSA(string content, string privateKey)
        {
            return RSASign("SHA1withRSA", content, privateKey, Encoding.UTF8);
        }

        public static string RSASign(string algorithm, string content, string privateKey, Encoding encoding)
        {
            var signer = SignerUtilities.GetSigner(algorithm);
            var privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));//将java格式的rsa密钥转换成.net格式
            signer.Init(true, privateKeyParam);
            var plainBytes = encoding.GetBytes(content);
            signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
            var signBytes = signer.GenerateSignature();
            return Convert.ToBase64String(signBytes);
        }

        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="publicKey">RSA公钥</param>
        /// <param name="signData">签名字段</param>
        /// <returns></returns>
        public static bool VerifyRSASign_MD5withRSA_GBK(string content, string publicKey, string signData)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("GBK");
            return VerifyRSASign_MD5withRSA(content, publicKey, signData, encoding);
        }

        public static bool VerifyRSASign_MD5withRSA(string content, string publicKey, string signData)
        {
            return VerifyRSASign_MD5withRSA(content, publicKey, signData, Encoding.UTF8);
        }

        public static bool VerifyRSASign_MD5withRSA(string content, string publicKey, string signData, Encoding encoding)
        {
            var signer = SignerUtilities.GetSigner("MD5withRSA");
            var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            signer.Init(false, publicKeyParam);
            var signBytes = Convert.FromBase64String(signData);
            var plainBytes = encoding.GetBytes(content);
            signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
            var ret = signer.VerifySignature(signBytes);
            return ret;
        }

        #endregion

        #region AES

        public static string AESEncrypt(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(input);
            var rm = new RijndaelManaged
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(input), 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESEncrypt2(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(input);
            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(input), 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESDecrypt(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(input);
            var rm = new RijndaelManaged
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        ///// <summary>
        ///// AES decrypt( no IV)  
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string AESEncrypt(string input, string key)
        //{
        //    using (var mStream = new MemoryStream())
        //    {
        //        using (Aes aes = Aes.Create())
        //        {
        //            byte[] plainBytes = Encoding.UTF8.GetBytes(input);
        //            Byte[] bKey = new Byte[32];
        //            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
        //            aes.Mode = CipherMode.ECB;
        //            aes.Padding = PaddingMode.PKCS7;
        //            aes.KeySize = 128;
        //            aes.Key = bKey;
        //            using (CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                try
        //                {
        //                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        //                    cryptoStream.FlushFinalBlock();
        //                    return Convert.ToBase64String(mStream.ToArray());
        //                }
        //                catch (Exception)
        //                {
        //                    return null;
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// RSA参数格式化扩展
    /// </summary>
    internal static class RSAKeyExtensions
    {
        #region XML

        /// <summary>
        /// RSA导入key
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="jsonString">RSA的Key序列化XML字符串</param>
        public static void FromLvccXmlString(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }
            rsa.ImportParameters(parameters);
        }

        /// <summary>
        /// 获取RSA Key序列化XML
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="includePrivateParameters">是否包含私钥</param>
        /// <returns></returns>
        public static string ToLvccXmlString(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }

        #endregion
    }
}