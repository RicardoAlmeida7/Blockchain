using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Models
{
    public class Block
    {
        private const int StartIndex = 0;

        public int Index { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string PreviousHash { get; set; }
        public string Hash { get; private set; }
        public string Data { get; private set; }
        public int Nonce { get; private set; }
        public string DeviceId { get; private set; }

        public Block(int index, string previousHash, string data, string deviceId, int nonce = 0)
        {
            this.Index = index;
            this.Timestamp = DateTime.Now;
            this.PreviousHash = previousHash;
            this.Data = data;
            this.Hash = CalculateHash();
            this.Nonce = nonce;
            this.DeviceId = deviceId;
        }

        public string CalculateHash()
        {
            using var sha256 = SHA256.Create();

            var rawData = this.Index + 
                this.Timestamp.ToString() + 
                this.PreviousHash + 
                this.Data + 
                this.Nonce + 
                this.DeviceId;

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();

            foreach ( var b in bytes ) { 
                builder.Append(b.ToString("x2"));   
            }

            return builder.ToString();
        }

        public void MineBlock(int difficulty)
        {
            var hashValidation = new String('0', difficulty);
            while (this.Hash.Substring(StartIndex, difficulty) != hashValidation)
            { 
                Nonce++;
                Hash = CalculateHash();
            }
            Console.WriteLine($"Mined block: {this.Hash}");
        }

        public override string ToString()
        {
            return @$"Index: {this.Index}
                      Timestamp {this.Timestamp}
                      Previous Hash: {this.PreviousHash}
                      Hash: {this.Hash}
                      Data: {this.Data}
                      Nonce: {this.Nonce}
                      DeviceId: {this.DeviceId}";
        }
    }
}
