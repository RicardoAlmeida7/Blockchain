using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Models
{
    public class Device
    {
        public string DeviceId { get; private set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; private set; }

        public Device(string deviceId)
        {
            DeviceId = deviceId;
            (PublicKey, PrivateKey) = GenerateKeys();
        }

        private (string publicKey, string privateKey) GenerateKeys()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            var publicKey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            return (publicKey, privateKey);
        }

        public string SignData(string data)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"));
            return Convert.ToBase64String(signatureBytes);
        }

        public bool VerifySignature(string data, string signature)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);
            return rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA256"), signatureBytes);
        }
    }
}