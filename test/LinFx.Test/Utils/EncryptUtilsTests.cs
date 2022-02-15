using LinFx.Utils;
using System;
using System.Text;
using Xunit;

namespace LinFx.Test.Utils;

public class EncryptUtilsTests
{
    /// <summary>
    /// RSA 加密
    /// </summary>
    [Fact]
    public void RSAEncryptTest()
    {
        //            string privateKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCBY+FhINScrVASZ53NBTr8Sq0E
        //vEFpWjmMqIaGRRG1Z5b1FN+zsVLDy/Fb5edjmNfyeDcYSz/Ew12Epl3hned6VNv/
        //5TzTTmPiC2LA1Uq1QNEKCKQ8qyp7arw9FsK78QjHnzxV3O56zFdTK3xZKOjLEvgb
        //z6mrgschd+g2lRN5CwIDAQAB";

        //            string content = "abcd";
        //            ////var tmp = EncryptUtils.RSAEncrypt("123456", content);
        //            ////var result = EncryptUtils.RSADecrypt("123456", tmp);

        //            var t = EncryptUtils.RSASign(content, key);

        //Assert.Equal(content, result);

    }

    /// <summary>
    /// AES 加密
    /// </summary>
    [Fact]
    public void AESEncryptTest()
    {
        string pwd = "123456";
        string key = "CJQmqeroXB6kg1Jr";
        var r = EncryptUtils.AESEncrypt(pwd, key);

        // base64 编码
        var base64Pwd = r.ToBase64String();

        // base64 解码
        var base64Bytes = base64Pwd.ToBase64Bytes();

        var r2 = EncryptUtils.AESDecrypt(base64Bytes, key);

        Assert.Equal(pwd, r2);
    }

    [Fact]
    public void EncryptTest()
    {
        //DesEncrypt des = new DesEncrypt();
        //des.getKey("star");  //加解密的key
        //String mingwen = "测试加密";
        //String strEnc = des.getEncString(mingwen);
        //System.out.println("加密后的密文：" + strEnc);

        var r = EncryptUtils.AESDecrypt("AC574A2CDCD98EA00239C2D694465648", "star");
    }
}
