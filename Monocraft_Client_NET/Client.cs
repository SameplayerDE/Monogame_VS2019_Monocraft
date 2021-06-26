using fNbt;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Client_NET.Rendering;
using Monocraft_Client_NET.Rendering.Mesh;
using Monocraft_Client_NET.Rendering.Renderer;
using Monocraft_Core_NET;
using Monocraft_Core_NET.EntityActions;
using Monocraft_Protocol_NET;
using Monocraft_Protocol_NET.Data.Enums;
using Monocraft_Protocol_NET.Packets;
using Monocraft_Protocol_NET.Packets.Clientbound.Play;
using Monocraft_Protocol_NET.Packets.Serverbound.Handshake;
using Monocraft_Protocol_NET.Packets.Serverbound.Login;
using Monocraft_Protocol_NET.Packets.Serverbound.Play;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monocraft_Client_NET
{

    public class Client
    {

        private Thread _receivingThread;
        private Thread _sendingThread;
        private Thread _chunkThread;
        private Thread _blockChangeThread;
        private Thread _multiBlockChangeThread;
        private Thread _entityThread;
        private Thread _keepAliveThread;

        public static int CompressionThreshold = -1;
        public static bool CheckedCompression = false;
        public TcpClient TcpClient;
        public string Address;
        public ushort Port;

        public Queue<byte[]> PacketQueue = new Queue<byte[]>();
        public int Count = 0;
        public ClientConnectionState ClientConnectionState = ClientConnectionState.Disconnected;
        public ConnectionState ConnectionState = ConnectionState.Handshake;
        public bool ConfirmTeleportation = false;

        public Dictionary<int, string> Players = new Dictionary<int, string>();


        public ColoredCubeMesh player = new ColoredCubeMesh(Color.Red);
        public Vector3 playerPosition = new Vector3(0, 0, 0);

        public TexturedCubeMesh Cube;
        public BlockRenderer BlockRenderer;
        public EntityRenderer EntityRenderer;
        public BlockyModelRenderer BlockyModelRenderer;
        public BlockClusterRenderer BlockClusterRenderer;
        public Camera Camera;

        Queue<ChunkData> chunks = new Queue<ChunkData>();
        Queue<BlockChange> blockChanges = new Queue<BlockChange>();
        Queue<MultiBlockChange> multiBlockChanges = new Queue<MultiBlockChange>();
        Queue<EntityAction> entityActions = new Queue<EntityAction>();
        List<Entity> entities = new List<Entity>();

        World world = new World();

        public void Connect(string address, ushort port)
        {

            Console.Write("Connecting to server...");

            Address = address;
            Port = port;
            TcpClient = new TcpClient();
            Task connectionTask = TcpClient.ConnectAsync(Address, Port);

            while (!connectionTask.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }

            if (connectionTask.IsFaulted)
            {
                Console.WriteLine("\nCould not connect to server.");
                return;
            }
            else
            {
                Console.WriteLine("\nConnected to server.");
                ClientConnectionState = ClientConnectionState.Connected;
            }

            TcpClient.ReceiveBufferSize = short.MaxValue;
            TcpClient.SendBufferSize = 256;

            _receivingThread = new Thread(ReceivingMethod);
            _receivingThread.IsBackground = true;
            _receivingThread.Start();

            _sendingThread = new Thread(SendingMethod);
            _sendingThread.IsBackground = true;
            _sendingThread.Start();

            _chunkThread = new Thread(ChunkProcessing);
            _chunkThread.IsBackground = true;
            _chunkThread.Start();

            _blockChangeThread = new Thread(BlockChangeProcessing);
            _blockChangeThread.IsBackground = true;
            _blockChangeThread.Start();

            _multiBlockChangeThread = new Thread(MultiBlockChangeProcessing);
            _multiBlockChangeThread.IsBackground = true;
            _multiBlockChangeThread.Start();

            _entityThread = new Thread(EntityProcessing);
            _entityThread.IsBackground = true;
            _entityThread.Start();

            _keepAliveThread = new Thread(KeepAliveProcessing);
            _keepAliveThread.IsBackground = true;
            _keepAliveThread.Start();
        }

        public void LoadContent()
        {
            
        }


        private void KeepAliveProcessing(object obj)
        {
            
        }

        private void EntityProcessing(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                lock (entityActions)
                {
                    if (entityActions.Count > 0)
                    {
                        EntityAction action = entityActions.Dequeue();
                        if (action is EntityRelativeMoveAction moveAction)
                        {
                            foreach (Entity entity in new List<Entity>(entities))
                            {
                                if (entity.EntityID == moveAction.EntityID)
                                {
                                    entity.Position.X += (float)moveAction.DeltaX / (128f * 32f);
                                    entity.Position.Y += (float)moveAction.DeltaY / (128f * 32f);
                                    entity.Position.Z += (float)moveAction.DeltaZ / (128f * 32f);
                                }
                            }
                        }
                        if (action is EntityRelativeMoveAnRotationAction moveRotationAction)
                        {
                            foreach (Entity entity in new List<Entity>(entities))
                            {
                                if (entity.EntityID == moveRotationAction.EntityID)
                                {
                                    entity.Position.X += (float)moveRotationAction.DeltaX / (128f * 32f);
                                    entity.Position.Y += (float)moveRotationAction.DeltaY / (128f * 32f);
                                    entity.Position.Z += (float)moveRotationAction.DeltaZ / (128f * 32f);
                                }
                            }
                        }
                        if (action is EntityTeleportAction teleportAction)
                        {
                            foreach (Entity entity in new List<Entity>(entities))
                            {
                                if (entity.EntityID == teleportAction.EntityID)
                                {
                                    entity.Position.X = (float)teleportAction.X;
                                    entity.Position.Y = (float)teleportAction.Y;
                                    entity.Position.Z = (float)teleportAction.Z;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BlockChangeProcessing(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                lock (blockChanges)
                {
                    if (blockChanges.Count > 0)
                    {
                        BlockChange blockChange = blockChanges.Dequeue();
                        if (blockChange != null)
                        {
                            Vector3 position = new Vector3(blockChange.X, blockChange.Y, blockChange.Z);
                            Material material = blockChange.Material;

                            Console.WriteLine($"{material}");

                            world.SetBlock(position, (int)material);
                        }
                    }
                }
            }
        }

        private void MultiBlockChangeProcessing(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                lock (multiBlockChanges)
                {
                    if (multiBlockChanges.Count > 0)
                    {
                        MultiBlockChange multiBlockChange = multiBlockChanges.Dequeue();
                        if (multiBlockChange != null)
                        {
                            foreach (BlockChange blockChange in multiBlockChange.BlockUpdates)
                            {
                                Vector3 position = new Vector3(blockChange.X, blockChange.Y, blockChange.Z);
                                Material material = blockChange.Material;

                                world.SetBlock(position, (int)material);
                            }
                        }
                    }
                }
            }
        }

        private void ChunkProcessing(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                
                lock (chunks)
                {
                    if (chunks.Count > 0)
                    {
                        ChunkData chunkData = chunks.Dequeue();
                        if (chunkData != null)
                        {
                            if (chunkData.Data != null)
                            {
                                byte[] chunkDataArray = chunkData.Data;

                                if (chunkDataArray.Length != 0)
                                {

                                    MemoryStream memoryStream = new MemoryStream(chunkDataArray);
                                    PacketStream chunkStream = new PacketStream(memoryStream);

                                    Chunk chunk = new Chunk(chunkData.ChunkX, chunkData.ChunkY);
                                    chunk.Populate(chunkData.PrimaryBitmask, chunkStream);

                                    world.AddChunk(chunk);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Login(string username)
        {
            SendMessage(new Handshake(754, Address, Port, State.LOGIN));
            ConnectionState = ConnectionState.Login;
            SendMessage(new LoginStart(username));
        }

        private void SendingMethod(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                lock (PacketQueue) {
                    
                }
                //Console.Title = $"Queued Packets: {PacketQueue.Count}";
                if (PacketQueue.Count > 0)
                {
                    byte[] packet = PacketQueue.Dequeue();

                    try
                    {
                        TcpClient.GetStream().Write(packet, 0, packet.Length);
                    }
                    catch
                    {
                        Disconnect();
                    }
                }
            }
        }

        private void Disconnect()
        {
            PacketQueue.Clear();
            try
            {
                //SendMessage();
            }
            catch { }
            Thread.Sleep(1000);
            ClientConnectionState = ClientConnectionState.Disconnected;
            TcpClient.Client.Disconnect(false);
            TcpClient.Close();
        }

        public void SendMessage(byte[] data)
        {
            foreach (byte @byte in data)
            {
                //Console.Write($"{@byte:x2} ");
            }
            //Console.WriteLine();
            PacketQueue.Enqueue(data);
        }

        public void SendMessage(Packet packet)
        {
            SendMessage(packet.ToBytes(CompressionThreshold));
        }

        private void ReceivingMethod(object obj)
        {
            PacketStream stream = new PacketStream(TcpClient.GetStream());
            int packetLength = 0;
            int bytesRead = 0;
            int packetID = -1;
            while (ClientConnectionState == ClientConnectionState.Connected)
            {

                if (TcpClient.Available > 0)
                {
                    //Console.Title = $"Available: {TcpClient.Available}, Count: {Count}";
                    if (packetLength == 0)
                    {
                        packetLength = stream.ReadVarInt();
                        //Console.WriteLine($"{packetLength:x2}");

                    }
                    if (Count == 0)
                    {
                        packetID = stream.ReadVarInt();
                        bytesRead++;
                        if (packetID == 0x03)
                        {
                            CompressionThreshold = stream.ReadVarInt();
                            bytesRead = packetLength;
                        }
                        CheckedCompression = true;
                    }
                    else
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            while (packetLength > bytesRead)
                            {
                                byte @byte = stream.ReadUnsignedByte();
                                bytesRead++;
                                //Console.Write($"{@byte:x2} ");
                                memoryStream.WriteByte(@byte);
                            }
                            //Console.WriteLine();
                            OnMessageReceived(memoryStream.ToArray(), packetLength);
                        }

                    }
                    packetLength = 0;
                    bytesRead = 0;
                    Count++;
                }
            }
        }

        protected virtual void OnMessageReceived(byte[] data, int packetLength)
        {
            MemoryStream stream = new MemoryStream(data);
            PacketStream packetStream = new PacketStream(stream);
            int packetID = 0x00;

            if (CompressionThreshold != -1)
            {
                int compressionLength;
                int isCompressed = packetStream.ReadVarInt(out compressionLength);

                if (isCompressed == 0x00)
                {
                    packetID = packetStream.ReadByte();

                    if (packetID == 0x02)
                    {
                        ConnectionState = ConnectionState.Play;
                        SendClientSettings("en_GB");
                        return;
                    }
                    if (ConnectionState == ConnectionState.Play) {
                        if (packetID == 0x3B)
                        {
                            Console.WriteLine("Multiblock Uncompressed");
                            MultiBlockChange multiBlockChange = new MultiBlockChange();
                            multiBlockChange.Decode(packetStream);

                            multiBlockChanges.Enqueue(multiBlockChange);

                            return;
                        }
                        if (packetID == 0x1f)
                        {
                            KeepAlive(packetStream.ReadLong());
                            Console.WriteLine("+");
                            return;
                        }
                        if (packetID == 0x0e)
                        {
                            DisplayChat(packetStream.ReadString());
                            return;
                        }
                        if (packetID == 0x0b)
                        {
                            BlockChange blockChange = new BlockChange();
                            blockChange.Decode(packetStream);
                            blockChanges.Enqueue(blockChange);
                            return;
                        }
                        if (packetID == 0x27)
                        {
                            int entityID = packetStream.ReadVarInt();

                            short deltaX = packetStream.ReadSignedShort();
                            short deltaY = packetStream.ReadSignedShort();
                            short deltaZ = packetStream.ReadSignedShort();
                            bool onGround = packetStream.ReadBoolean();

                            entityActions.Enqueue(new EntityRelativeMoveAction(entityID, deltaX, deltaY, deltaZ, onGround));

                            return;
                        }
                        if (packetID == 0x28)
                        {
                            int entityID = packetStream.ReadVarInt();

                            short deltaX = packetStream.ReadSignedShort();
                            short deltaY = packetStream.ReadSignedShort();
                            short deltaZ = packetStream.ReadSignedShort();
                            float yaw = packetStream.ReadFloat();
                            float pitch = packetStream.ReadFloat();
                            bool onGround = packetStream.ReadBoolean();

                            entityActions.Enqueue(new EntityRelativeMoveAnRotationAction(entityID, deltaX, deltaY, deltaZ, yaw, pitch, onGround));

                            return;
                        }
                        if (packetID == 0x56)
                        {
                            int entityID = packetStream.ReadVarInt();
                            double x = packetStream.ReadDouble();
                            double y = packetStream.ReadDouble();
                            double z = packetStream.ReadDouble();
                            float yaw = packetStream.ReadFloat();
                            float pitch = packetStream.ReadFloat();
                            bool onGround = packetStream.ReadBoolean();

                            entityActions.Enqueue(new EntityTeleportAction(entityID, x, y, z, yaw, pitch, onGround));

                            return;
                        }
                        if (packetID == 0x34)
                        {
                            double x = packetStream.ReadDouble(); // x
                            double y = packetStream.ReadDouble(); // y
                            double z = packetStream.ReadDouble(); // z
                            float yaw = packetStream.ReadFloat(); // yaw
                            float pitch = packetStream.ReadFloat(); // pitch
                            byte flags = packetStream.ReadUnsignedByte(); // flags

                            double dirX = 0;
                            double dirY = 0;
                            double dirZ = 1;

                            Vector3 position = new Vector3((float)x, (float)y + 1.6f, (float)z);
                            Vector3 direction = new Vector3((float)dirX, (float)dirY, (float)dirZ);

                            Camera.RotationX = MathHelper.ToRadians(pitch);
                            //Camera.RotateX(pitch);
                            Camera.RotationY = MathHelper.ToRadians(-yaw);
                            //Camera.RotateY(-yaw);
                            Camera.Teleport(position);

                            //Console.WriteLine($"{x} {y} {z} | {Camera.RotationX} {Camera.RotationY}");

                            ConfirmTeleport(packetStream.ReadVarInt()); // TeleportID
                            return;
                        }
                        if (packetID == 0x04)
                        {
                            int entityID = packetStream.ReadVarInt();
                            Guid uuid = packetStream.ReadUUID();
                            double x = packetStream.ReadDouble(); // x
                            double y = packetStream.ReadDouble(); // y
                            double z = packetStream.ReadDouble(); // z
                            float yaw = packetStream.ReadFloat(); // yaw
                            float pitch = packetStream.ReadFloat(); // pitch


                            Entity entity = new Entity();
                            entity.Position = new Vector3((float)x, (float)y, (float)z);
                            //entity.Type = EntityType.Player;
                            entity.EntityID = entityID;

                            entities.Add(entity);

                            Console.WriteLine("Spawn Player");
                            return;
                        }
                        if (packetID == 0x49)
                        {
                            float health = packetStream.ReadFloat();
                            int food = packetStream.ReadVarInt();
                            float foodSaturation = packetStream.ReadFloat();
                            Console.WriteLine($"{health:0} | {food}");
                        }
                        if (packetID == 0x44)
                        {

                            /*int entityID = packetStream.ReadVarInt();
                            Console.WriteLine($"{entityID}");
                            int index = 0x00;

                            do
                            {
                                index = packetStream.ReadUnsignedByte();
                                Console.WriteLine("index: " + index);
                                if (index != 0xff)
                                {
                                    int type = packetStream.ReadVarInt();
                                    int value = packetStream.ReadVarInt();
                                    Console.WriteLine($"{type} : {value}");
                                }
                            } while (index != 0xff);*/
                            return;
                        }
                        if (packetID == 0x32)
                        {
                            int action = packetStream.ReadVarInt();
                            int numberOfPlayers = packetStream.ReadVarInt();

                            for (int i = 0; i < numberOfPlayers; i++)
                            {
                                Guid uuid = packetStream.ReadUUID();
                                if (action == 2) // ping
                                {
                                    int ping = packetStream.ReadVarInt();

                                    string pingIcon = "";
                                    if (ping < 150)
                                    {
                                        pingIcon = "|||||";
                                    }
                                    else if (ping < 300)
                                    {
                                        pingIcon = "||||";
                                    }
                                    else if (ping < 600)
                                    {
                                        pingIcon = "|||";
                                    }
                                    else if (ping < 1000)
                                    {
                                        pingIcon = "||";
                                    }
                                    else if (ping >= 1000)
                                    {
                                        pingIcon = "|";
                                    }

                                    Console.WriteLine($"{uuid} has a ping of {pingIcon} ({ping}ms)");
                                }
                            }

                            return;
                        }
                        if (packetID == 0x20)
                        {

                            ChunkData chunkData = new ChunkData();
                            chunkData.Decode(packetStream);

                            chunks.Enqueue(chunkData);
                            return;
                        }
                    }
                }
                else
                {
                    byte[] compressedData = packetStream.Read(packetLength - compressionLength);
                    using (PacketStream decompressedPacketStream = new PacketStream())
                    {
                        using (ZlibStream outZStream = new ZlibStream(decompressedPacketStream, CompressionMode.Decompress, CompressionLevel.Default, true))
                        {
                            outZStream.Write(compressedData, 0, compressedData.Length);
                        }

                        decompressedPacketStream.Seek(0, SeekOrigin.Begin);

                        int l;
                        packetID = decompressedPacketStream.ReadVarInt(out l);


                        if (ConnectionState == ConnectionState.Play)
                        {

                            if (packetID == 0x20)
                            {

                                ChunkData chunkData = new ChunkData();
                                chunkData.Decode(decompressedPacketStream);

                                chunks.Enqueue(chunkData);
                                return;
                            }

                            if (packetID == 0x3B)
                            {
                                MultiBlockChange multiBlockChange = new MultiBlockChange();
                                multiBlockChange.Decode(decompressedPacketStream);

                                multiBlockChanges.Enqueue(multiBlockChange);
                                Console.WriteLine("Multiblock Compressed");
                                return;
                            }

                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Chunk c in world.Chunks)
            {
                Vector3 position = new Vector3(c.ChunkX, 0, c.ChunkZ);
                if (c.IsPositionInChunk(Camera.Position))
                {
                    position.X *= 16;
                    position.Z *= 16;
                    Material material = 0x00;

                    foreach (ChunkSection section in c.ChunkSectionsAsList)
                    {
                        position.Y = section.ChunkSectionY;
                        if (section.IsPositionInSection(Camera.Position) || section.IsPositionInSection(Camera.Position + new Vector3(0, 16, 0)) || section.IsPositionInSection(Camera.Position - new Vector3(0, 16, 0)))
                        {

                            if (section.IsOneBlockType && !section.IsEmpty)
                            {

                                BlockClusterRenderer.DrawModel(new BlockClusterModel(Vector3.Zero, new Vector3(16, 16, 16), "oak_planks"), position, Camera.View, Camera.Projection, gameTime);

                            }
                            else
                            {

                                position.Y *= 16;
                                for (int y = 0; y < 16; y++)
                                {
                                    for (int z = 0; z < 16; z++)
                                    {
                                        for (int x = 0; x < 16; x++)
                                        {
                                            material = (Material)section.GetBlockAt(x, y, z);
                                            if (material != Material.Air && material != Material.Void_Air && material != Material.Cave_Air)
                                            {
                                                //BlockRenderer.DrawModel(new TexturedCubeMesh(material), position + new Vector3(x, y, z), Camera.View, Camera.Projection, gameTime);
                                                BlockyModelRenderer.DrawModel(new BlockyModelMesh(MonocraftGame.ModelManager.GetModel(material.ToString().ToLower()), true), position + new Vector3(x, y, z), Camera.View, Camera.Projection, gameTime);
                                            }
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }

            foreach (Entity entity in new List<Entity>(entities))
            {
                EntityRenderer.DrawModel(player, entity.Position, Camera.View, Camera.Projection, gameTime);
            }
            

        }

        public void Update(GameTime gameTime)
        {
            if (ClientConnectionState != ClientConnectionState.Connected)
            {
                Console.WriteLine("Disconnected.");
            }
        }

        private void SendClientSettings(string locale)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WritePacket(new Monocraft_Protocol_NET.Packets.Serverbound.Play.ClientSettings(locale, 2, ChatTypeClientSettings.Full, false, 127, MainHand.Right), 256);
            SendMessage(packetStream.ToPacket());
        }

        private void ConfirmTeleport(int id)
        {
            //Console.WriteLine(id);
            MemoryStream stream = new MemoryStream();
            PacketStream packetStream = new PacketStream(stream);
            packetStream.WriteByte(0x00);
            packetStream.WriteByte(0x00);
            packetStream.WriteVarInt(id);
            SendMessage(packetStream.ToPacket());
            Console.WriteLine("Humpf");
            ConfirmTeleportation = true;
        }

        private void DisplayChat(string message)
        {
            Console.WriteLine(message);
        }

        private void KeepAlive(long id)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WritePacket(new Monocraft_Protocol_NET.Packets.Serverbound.Play.KeepAlive(id), 256);
            SendMessage(packetStream.ToPacket());
        }

    }
}
