using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
                byte[] bytes_md5_out = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes_md5_out).Replace("-", "");
            }
        }
        #endregion

        #region RSA

        ///// <summary>
        ///// RSA加密
        ///// </summary>
        ///// <param name="publickey"></param>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public static string RSAEncrypt(string content, string publickey)
        //{
        //    //publickey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //    //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    //byte[] cipherbytes;
        //    //rsa.FromXmlString(publickey);
        //    //cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

        //    using (RSA rsa = RSA.Create())
        //    {
        //        rsa.e
        //    }

        //    return Convert.ToBase64String(cipherbytes);
        //}

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
            //var signer = SignerUtilities.GetSigner("SHA1withRSA");
            var signer = SignerUtilities.GetSigner("MD5withRSA");
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
            //var signer = SignerUtilities.GetSigner("SHA1withRSA");
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
    }
}