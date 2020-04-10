using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace LinFx.Utils
{
    /// <summary>
    /// 加密工具类
    /// </summary>
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
        /// <returns></returns>
        public static string MD5Encrypt(string input)
        {
            Check.NotNull(input, nameof(input));

            using var md5 = MD5.Create();
            var bytes_md5_out = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes_md5_out).Replace("-", "").ToLower();
        }

        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5Encrypt(byte[] input)
        {
            Check.NotNull(input, nameof(input));

            using var md5 = MD5.Create();
            var bytes_md5_out = md5.ComputeHash(input);
            return BitConverter.ToString(bytes_md5_out).Replace("-", "").ToLower();
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
            //publickey = @"<RSAKeyValue><Modulus>" + publickey + "</Modulus><Exponent>1234567890123456</Exponent></RSAKeyValue>";
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //byte[] cipherbytes;
            //rsa.FromXmlString(publickey);
            //cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            //RSAParameters p = new RSAParameters();
            //p.Modulus = Convert.FromBase64String()

            using (var rsa = RSA.Create())
            {
                //rsa.FromLvccXmlString(publickey);

                RSAParameters p = new RSAParameters
                {
                    Exponent = Encoding.UTF8.GetBytes("1234567890123456"),
                    Modulus = Convert.FromBase64String(publickey)
                };
                rsa.ImportParameters(p);

                var buffer = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// RSA私钥加密
        /// </summary>
        /// <param name="data">加密明文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>返回密文</returns>
        public static byte[] RSAEncryptWithPrivateKey(string data, string privateKey)
        {
            var parameters = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            var c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            c.Init(true, parameters);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            byteData = c.DoFinal(byteData, 0, byteData.Length);
            return byteData;
        }

        /// <summary>
        /// RSA私钥解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privatekey"></param>
        /// <returns></returns>
        public static string RSADecrypt(byte[] data, string privatekey)
        {
            var privateKey = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privatekey));
            var c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            c.Init(false, privateKey);
            var result = c.DoFinal(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">数据</param>
        /// <param name="privateKey">RSA密钥</param>
        /// <returns></returns>
        public static string RSASign_MD5withRSA_GBK(string content, string privateKey)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        public static byte[] AESEncrypt(string input, string key)
        {
            return AESEncrypt(input, Encoding.UTF8.GetBytes(key));
        }

        public static byte[] AESEncrypt(string input, byte[] key)
        {
            if (string.IsNullOrEmpty(input)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(input);
            var rm = new RijndaelManaged
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(input), 0, toEncryptArray.Length);
            return resultArray;
        }

        public static string AESDecrypt(string input, string key)
        {
            return AESDecrypt(Encoding.UTF8.GetBytes(input), Encoding.UTF8.GetBytes(key));
        }

        public static string AESDecrypt(byte[] input, string key)
        {
            return AESDecrypt(input, Encoding.UTF8.GetBytes(key));
        }

        public static string AESDecrypt(byte[] input, byte[] key)
        {
            var rm = new RijndaelManaged
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                BlockSize = 128,
            };
            ICryptoTransform cTransform = rm.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region 哈希加密算法
        /// <summary>
        /// HMAC-MD5 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns></returns>
        public static string HMACSMD5Encrypt(string input, string key)
        {
            return HMACSMD5Encrypt(input, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMAC-MD5 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSMD5Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACMD5(encoding.GetBytes(key)), input, encoding);
        }

        /// <summary>
        /// 哈希加密算法
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private static string HashEncrypt(HashAlgorithm hashAlgorithm, string input, Encoding encoding)
        {
            var data = hashAlgorithm.ComputeHash(encoding.GetBytes(input));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        } 
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
        /// <param name="xmlString">RSA的Key序列化XML字符串</param>
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