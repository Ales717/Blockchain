using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Security.Policy;

namespace alesBlockChain
{
    public partial class Form1 : Form
    {
        static Block FirstBlock = new Block
        (0, Encoding.ASCII.GetBytes("firstBlockchain.xyz"), DateTime.ParseExact("16 01 2023 20 08 07", "dd MM yyyy HH mm ss", null), new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes("\x00" + "30 12 2021 20 08 07" + "firstBlockchain.xyz" + "\x00" + "\x00" + "\x00")), new byte[] { 0 }, 0, 0);
        static List<Block> BlockChain = new List<Block>();

        static int DIFFICULTY = 5;
        static int GEN_TIME = 10;
        static int DIFF_INTER = 10;
        static int CLIENTS_CONNECTED = 0;


        public Form1()
        {
            InitializeComponent();
            BlockChain.Add(FirstBlock);
        }

        // Write functions for threading
        delegate void SetTextCallback(string text);
        private void listenPortSetText(string text) 
        {
            if (this.listeningLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(listenPortSetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listeningLabel.Text = text;
            }
        }
        private void blockChainSetText(string text) 
        {
            if (this.blockChainDisplay.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(blockChainSetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.blockChainDisplay.Text = text;
            }
        }
        private void clientsConnectedSetText(string text)
        {
            if (this.clientsConnectedLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(clientsConnectedSetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.clientsConnectedLabel.Text = text;
            }
        }

        public void updateBlockChainDisplay()
        {
            string together = "";
            foreach (Block block in BlockChain)
            {
                string hexHash = BitConverter.ToString(block.hash).Replace("-", "");
                together += $"{block.index}: {hexHash}\n";
                
            }
            blockChainSetText(together);
        }
        public void changeDifficulty()
        {
            //po 10 blokih izraèuna timeExpected in pa dejasnski èas za generacijo blokov,
            //èe je dejanski porabljen èas krajši od polovice prièakovanega èasa, poveèa težavnost za 1
            //èe je dejanski porabljen èas daljši od polovice prièakovanega èasa, zmanjša težavnost za 1
            if (BlockChain.Count > 10)
            {
                Block prevBlock = BlockChain[BlockChain.Count-DIFF_INTER];
                int timeExpected = GEN_TIME * DIFF_INTER;
                double timeTaken = (BlockChain.Last().time - prevBlock.time).Seconds;
                string hexHash = BitConverter.ToString(prevBlock.hash).Replace("-", "");
                if (timeTaken < Math.Floor((double)timeExpected / 2))
                {
                    DIFFICULTY = prevBlock.diff + 1;
                }
                else if (timeTaken > Math.Floor((double)timeExpected / 2))
                {
                    DIFFICULTY = prevBlock.diff - 1;
                }
                else
                {
                    DIFFICULTY = prevBlock.diff;
                }
            }
        }
        //genereira hash za blok
        public byte[] createHash(int index, DateTime timestamp, int diff, int nonce, byte[] data, byte[] prevHash)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] indexBytes = BitConverter.GetBytes(index);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(indexBytes);
                byte[] timestampBytes = Encoding.ASCII.GetBytes(timestamp.ToString("dd MM yyyy HH mm ss"));
                byte[] diffBytes = BitConverter.GetBytes(diff);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(diffBytes);
                byte[] nonceBytes = BitConverter.GetBytes(nonce);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(nonceBytes);
                byte[] input = new byte[indexBytes.Length + timestampBytes.Length + data.Length + prevHash.Length + diffBytes.Length + nonceBytes.Length];
                Array.Copy(indexBytes, 0, input, 0, indexBytes.Length);
                Array.Copy(timestampBytes, 0, input, indexBytes.Length, timestampBytes.Length);
                Array.Copy(data, 0, input, indexBytes.Length + timestampBytes.Length, data.Length);
                Array.Copy(prevHash, 0, input, indexBytes.Length + timestampBytes.Length + data.Length, prevHash.Length);
                Array.Copy(diffBytes, 0, input, indexBytes.Length + timestampBytes.Length + data.Length + prevHash.Length, diffBytes.Length);
                Array.Copy(nonceBytes, 0, input, indexBytes.Length + timestampBytes.Length + data.Length + prevHash.Length + diffBytes.Length, nonceBytes.Length);

                byte[] hash = sha256.ComputeHash(input);
                return hash;
            }
        }
        public bool blockValidation(Block block, Block prevBlock)
        //preveri ce je blok valid, to pa naredi tako da preveri:
        //- èe je index za eno veèji od prejšnega bloka,
        //- èe se èe prevhash blocka enak heshu prejsnega bloca
        //- Zgošèena vrednost trenutnega bloka je enaka zgošèeni vrednosti,
        //ki jo ustvari funkcija createHash, ko posreduje indeks, èas, težavnost, nonce, podatke in prevHash bloka.
        {
            if (block.index != prevBlock.index+1)
            {
                return false;
            }
            if (block.prevHash != prevBlock.hash)
            {
                return false;
            }
            byte[] blockHash = createHash(block.index, block.time, block.diff, block.nonce, block.data, block.prevHash);
            if (BitConverter.ToString(blockHash).Replace("-", "") != BitConverter.ToString(block.hash).Replace("-", ""))
            {
                return false;
            }
            return true;
        }
        bool chainValidation(List<Block> chain)
        {
            for (int i = 1; i < chain.Count; i++)
            {
                if (!blockValidation(chain[i], chain[i - 1]))
                {
                    return false;
                }
            }
            return true;
        }
        public double comulativeDifference(List<Block> chain)
        {
            
            double sum = 0;
            foreach (Block block in chain)
            {
                sum += Math.Pow(2, block.diff);
            }
            return sum;
        }
        public Block mineBlock()
        {
            int nonce = 0;
            var random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string data = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);

            while (true)
            {
                int index = BlockChain.Count;
                DateTime timestamp = DateTime.UtcNow;
                byte[] prevHash = BlockChain.Last().hash;
                int diff = DIFFICULTY;

                try
                {
                    byte[] hash = createHash(index, timestamp, diff, nonce, dataBytes, prevHash);
                    string hexHash = BitConverter.ToString(hash).Replace("-", "");

                    int count = 0;
                    foreach (var i in hexHash)
                    {
                        if (i != '0')
                        {
                            break;
                        }
                        count++;
                    }
                    if (count == DIFFICULTY)
                    {
                        return new Block(index, dataBytes, timestamp, hash, prevHash, diff, nonce);
                    }
                    nonce += 1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void miner()
        {
            while (true)
            {
                Block block = mineBlock();

                if (blockValidation(block, BlockChain.Last()))
                {
                    BlockChain.Add(block);
                    changeDifficulty();
                    updateBlockChainDisplay();

                }
            }
        }
        public void updateChain(List<Block> chain)
            // gre cez tvoj block chain in chez block chain od peera in ju primerja, èe najde raliko v katerm bloku breka for loop
            // in update tisti block chain ki ima veèjo komulativno vrednost
        {
            if (BlockChain != chain)
            {
                int i = 0, j = 0;
                for (; i < BlockChain.Count && j < chain.Count; i++, j++)
                {
                    if (BlockChain[i] != chain[j])
                        break;
                }
                if (comulativeDifference(BlockChain.GetRange(i, BlockChain.Count - i)) < comulativeDifference(chain.GetRange(j, chain.Count - j)))
                {
                    BlockChain = chain;
                }
            }
            updateBlockChainDisplay();
        }
        public async void startServer(TcpClient client) {
            CLIENTS_CONNECTED++;
            clientsConnectedSetText($"Clients connected: {CLIENTS_CONNECTED}");
            while (true)
            {
                byte[] buff = new byte[1024];
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(data, 0, data.Length);
                List<Block> blocks;
                using (var ms = new MemoryStream(data, 0, bytesRead))
                {
                    blocks = (List<Block>)new BinaryFormatter().Deserialize(ms);
                }

                if (chainValidation(blocks))
                {
                    updateChain(blocks);
                    changeDifficulty();
                }
                
            }
        }
        public async void startClient()
        {
            IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)numericPort.Value);

            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Connect(address);
                while (true)
                {
                    byte[] dataToSend;
                    using (var ms = new MemoryStream())
                    {
                        new BinaryFormatter().Serialize(ms, BlockChain);
                        dataToSend = ms.ToArray();
                    }
                    sock.Send(dataToSend);
                    Thread.Sleep(60 * 1000);
                }
            }
        }
        public void startMainServer()
        {
            TcpListener server;
            int port;
            Random rnd = new Random();
            while (true)
            {
                
                try
                {
                    port = rnd.Next(1024, 65535);
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    break;
                }
                catch (SocketException){}
            }
            listenPortSetText($"Started listening on port {port}");
            Thread minerThread = new Thread(miner);
            minerThread.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread thread = new Thread(() => startServer(client));
                thread.Start();
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        private void startMiner_Click(object sender, EventArgs e)
        {
            startMiner.Enabled = false;
            connectButton.Enabled = true;
            numericPort.Enabled = true;

            Thread thread = new Thread(startMainServer);
            thread.Start();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Thread connect = new Thread(startClient);
            connect.Start();
        }
    }
}