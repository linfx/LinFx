using LinFx.Utils;
using Xunit;

namespace LinFx.Test.Utils
{
    public class EncryptUtilsTest
    {
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

        [Fact]
        public void EncryptTest()
        {
            //DesEncrypt des = new DesEncrypt();
            //des.getKey("star");  //加解密的key
            //String mingwen = "测试加密";
            //String strEnc = des.getEncString(mingwen);
            //System.out.println("加密后的密文：" + strEnc);

            var r = LinFx.Utils.EncryptUtils.AESDecrypt("AC574A2CDCD98EA00239C2D694465648", "star");
        }
    }
}
