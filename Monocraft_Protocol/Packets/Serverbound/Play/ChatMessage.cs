using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Play
{
    public class ChatMessage : ServerboundPacket
    {

        public readonly string Message;


        public ChatMessage(string message) : base(0x03) 
        {
            Message = message;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(PacketID);
            writer.WriteString(Message);
            return writer.ToArray();
        }
    }
}
