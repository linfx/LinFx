using QRCoder;
using System.DrawingCore;

namespace LinFx.Utils
{
    public static class QRCodeUtils
    {
        public static Bitmap CreateQrCode(string plainText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
    }
}
