using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Play
{
    public class ClientStatus : ServerboundPacket
    {

        public readonly int ActionID = 0x00;

        public ClientStatus(int actionID) : base(0x04) 
        {
            ActionID = actionID;
        }


        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(0);
            writer.WriteVarInt(PacketID);
            writer.WriteVarInt(ActionID);
            return writer.ToArray();
        }
    }
}
