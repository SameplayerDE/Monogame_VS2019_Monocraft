using Monocraft_Protocol;
using Monocraft_Protocol.Packets;
using Monocraft_Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monocraft_Client_NET;
using Monocraft_Protocol.Packets.Serverbound.Play;
using Monocraft_Protocol.Data.Json;
using Newtonsoft.Json;
using Monocraft_API.Generic;
using Monocraft_Protocol.Data;

namespace Monocraft_Test
{

    /*public class Client
    {
        private TcpClient _client;
        private NetworkStream _networkStream;
        private int _count = 0;
        private int _compressed = 256;
        private bool _checkedCompression = false;
        

        private DataWriter _writer = new DataWriter();
        private DataReader _reader = new DataReader();
        private byte[] _data = new byte[short.MaxValue];

        public byte[] packet;

        //public delegate void PacketHandlerDelegate(byte[] data);
        public delegate void PacketHandlerDelegate(DataReader reader);
        public MultiKeyMap<int, ConnectionState, PacketHandlerDelegate> multiKeyMap = new MultiKeyMap<int, ConnectionState, PacketHandlerDelegate>();

        public NetworkStream NetworkStream { get { return _networkStream; } }
        public bool IsConnected { get { return _client.Connected; } }
        public ConnectionState State = ConnectionState.Handshake;

        public Client()
        {
            _client = new TcpClient();
            _client.ReceiveBufferSize = 65535;
            _client.ReceiveTimeout = 60 * 1000;

            Init();

        }

        private void Init()
        {
            multiKeyMap.Add(0x03, ConnectionState.Login, Compression);
            multiKeyMap.Add(0x02, ConnectionState.Login, SwitchConnectionState);
            multiKeyMap.Add(0x34, ConnectionState.Play, SendTeleportConfirm);
            multiKeyMap.Add(0x1f, ConnectionState.Play, SendKeepAlive);
            multiKeyMap.Add(0x0B, ConnectionState.Play, ChangeBlock);
            multiKeyMap.Add(0x20, ConnectionState.Play, ChunkData);
        }

        private void Compression(DataReader reader)
        {
            if (!_checkedCompression)
            {
                _compressed = reader.ReadVarInt();
                Console.WriteLine($"Packets compressed with {_compressed} threshold.");
                _checkedCompression = true;
            }
        }

        private void ChunkData(DataReader reader)
        {

            Console.WriteLine($"Chunk");
        }

        private void ChangeBlock(DataReader reader)
        {
            Position position = reader.ReadPosition();
            int blockID = reader.ReadVarInt();
            Console.WriteLine($"{blockID} at {position}");
        }

        public void SwitchConnectionState(DataReader reader)
        {
            State = ConnectionState.Play;
        }

        public void SendKeepAlive(DataReader reader)
        {
            long keepAliveID = reader.ReadLong();
            Console.Write(" + ");
            SendP(new KeepAlive(keepAliveID));
        }

        public void SendTeleportConfirm(DataReader reader)
        {
            Monocraft_Protocol.Packets.Clientbound.Play.PlayerPositionAndLook packet = new Monocraft_Protocol.Packets.Clientbound.Play.PlayerPositionAndLook(reader);
            SendP(new TeleportConfirm(packet.TeleportID));
        }

        public void Connect(string host, ushort port)
        {

            Task task = _client.ConnectAsync(host, port);
            Console.WriteLine("Connecting to Minecraft server..");

            while (!task.IsCompleted)
            {
                Console.WriteLine("Connecting..");
                Thread.Sleep(250);
            }

            if (!_client.Connected)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to connect to the server");
                Console.ResetColor();
                Console.ReadKey(true);
                Environment.Exit(1);
            }

            _networkStream = _client.GetStream();
            _networkStream.BeginRead(_data, 0, short.MaxValue, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int length = _networkStream.EndRead(ar);
                if (length <= 0)
                {
                    return;
                }

                byte[] data = new byte[length];
                Array.Copy(_data, data, length);

                HandleData(data, State);

                _networkStream.BeginRead(_data, 0, short.MaxValue, ReceiveCallback, null);
            }
            catch
            {
                // TODO: disconnect
            }
        }

        private void HandleData(byte[] data, ConnectionState state)
        {

            DataReader reader = new DataReader(data);

            int packetLength = reader.ReadVarInt();
            int packetID = 0;
            int uncompressedPacketLength = 0;

            if (_checkedCompression == false) // If not checked for compression
            {
                packetID = reader.ReadVarInt();

                if (multiKeyMap.Has(packetID, state))
                {
                    multiKeyMap.Get(packetID, state)?.Invoke(reader);
                }

            }
            else
            {
                if (_compressed != -1) // Compression set
                {
                    
                    uncompressedPacketLength = reader.ReadVarInt();
                    

                    if (uncompressedPacketLength == 0) // Packet not compressed
                    {
                        
                        packetID = reader.ReadVarInt();

                        if (multiKeyMap.Has(packetID, state))
                        {
                            multiKeyMap.Get(packetID, state)?.Invoke(reader);
                        }
                    }
                    else
                    {

                        byte[] decompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(reader.ReadUBytes(packetLength));

                    }
                }
            }
            _count++;
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public int Read()
        {

            int length = _networkStream.Read(_data, 0, _data.Length);
            if (length > 0 && length < _client.ReceiveBufferSize)
            {
                Array.Resize(ref _data, length);
                _reader.SetData(_data);
            }
            return length;
        }

        public Packet ResolvePacket()
        {
            Packet packet = PacketResolver.ResolveClientbound(_reader);
            return packet;
        }

        public void SendPacket(Packet packet)
        {
            _writer.WriteBytes(packet.ToBytes());
            _networkStream.Write(_writer.Data, 0, _writer.Data.Length);
            _writer.Clear();
        }

        public void SendP(Packet packet)
        {
            try
            {
                byte[] data = packet.ToBytes();
                _networkStream.BeginWrite(data, 0, data.Length, SendCallback, null);
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _networkStream.EndWrite(ar);
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public void Flush()
        {
            byte[] data = _writer.ToArray();
            _writer.Clear();
            _networkStream.Write(data, 0, data.Length);
        }

    }*/
}