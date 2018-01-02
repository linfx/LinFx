using LinFx.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LinFx.UnitTest.Utils
{
    public class RSAHelperTest
    {
        [Fact]
        public void RSATst()
        {
            //2048 公钥
            string publicKey = @"
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxr667vXwINmQB9JBtUihPZHSJ
1iCLFnL9mQ4pt/uKD6yV7qLdVG4pHjPiRepLpbXpoUKTqrQaXEdu1TCna9D9uhfR
0Xv9zza6rnddwSOwMTNvbOOTHCIJDfxEEk5LpbQXwcwQwZD2pVLU8qcUMZqfcs0h
UBjunWrRutb2pku3/wIDAQAB
";

            //2048 私钥
            string privateKey = @"
MIICXAIBAAKBgQCxr667vXwINmQB9JBtUihPZHSJ1iCLFnL9mQ4pt/uKD6yV7qLd
VG4pHjPiRepLpbXpoUKTqrQaXEdu1TCna9D9uhfR0Xv9zza6rnddwSOwMTNvbOOT
HCIJDfxEEk5LpbQXwcwQwZD2pVLU8qcUMZqfcs0hUBjunWrRutb2pku3/wIDAQAB
AoGASK1pNRU+BEXrBfm4kV6HamHWYQKacQmPozbVWi8Mzd23Y+Ql7Y25OUxHQIgE
W0i+bX8uMiQiYp3YAfdlXTV2V4JSwKjfkFm6pN8Sl5JxA15jMxje80Kz/jSiKvuB
M7Ga6alIr2s71A60kNib0N/ooK8AlwDAHe13XLef0yJWxZECQQDrVvXIgCOgxfe0
jz/BrEmoXxuvWmno2brWwJ39ChQboKy5ELwZSj8q83Z0DTHUEzaSCwEgKfprVBOd
/oWLFmI9AkEAwUkD9UUvc5LebQdb5Eziolxm4MtXhYj4Wre1Tx6FU6dRZlaqea3O
qRrSobNx+34LgCgLWu+2H72GGfmrPuhS6wJAEEYvMPJLhG6sNnxBeG8lmNMa4wFp
mYSU+wzO4BS2V0LBLvsNRuJvg9TaOCRBcdzyRR8lsMe2XX2u7ZoQOhIOMQJBAJWu
NaJ7MYQO+LD2QfNKlzek1wa+cci3iZy3J3Fd8WIW8LKP6vTP5HqQiw0uKdbYhY95
c1G40RFDc9YpwrO0toECQFOh5CTFMldyX2qMQdbsmoNm3cUPJ9YtDcdx0CAJlY6h
ZPQGvuS0wCz9uuBGsUSULhFPPVUNge8D0pnvrcXklsM=
";


            var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, privateKey, publicKey);

            string str = "博客园 http://www.cnblogs.com/";

            Console.WriteLine("原始字符串：" + str);

            //加密
            string enStr = rsa.Encrypt(str);

            Console.WriteLine("加密字符串：" + enStr);

            //解密
            string deStr = rsa.Decrypt(enStr);

            Console.WriteLine("解密字符串：" + deStr);

            //私钥签名
            string signStr = rsa.Sign(str);

            Console.WriteLine("字符串签名：" + signStr);

            //公钥验证签名
            bool signVerify = rsa.Verify(str, signStr);

            Console.WriteLine("验证签名：" + signVerify);

            Console.ReadKey();
        }

    }
}
