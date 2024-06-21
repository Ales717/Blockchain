using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alesBlockChain
{
    [Serializable]
    public class Block
    {
        public int index;
        public byte[] data;
        public DateTime time;
        public byte[] hash;
        public byte[] prevHash;
        public int diff;
        public int nonce;

        public Block(int index, byte[] data, DateTime time, byte[] hash, byte[] prevHash, int diff, int nonce)
        {
            this.index = index;
            this.data = data;
            this.time = time;
            this.hash = hash;
            this.prevHash = prevHash;
            this.diff = diff;
            this.nonce = nonce;
        }
    }
}
