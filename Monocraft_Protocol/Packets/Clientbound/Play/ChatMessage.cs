using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{
    public class ChatMessage : ClientboundPacket
    {

        public readonly string Json;
        public readonly sbyte Position;
        public readonly Guid UUID;

        public ChatMessage() : base(0x0E) { }
        public ChatMessage(DataReader reader) : base(0x0E) 
        {
            Json = reader.ReadString();
            Position = reader.ReadByte();
            UUID = reader.ReadUUID();
        }
        public ChatMessage(byte[] data) : base(0x0E) 
        {
            DataReader reader = new DataReader();
            Json = reader.ReadString();
            Position = reader.ReadByte();
            UUID = reader.ReadUUID();
        }
        public ChatMessage(string chat) : base(0x0E) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            return writer.ToArray();
        }
    }
}
