using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Play
{
    public class TeleportConfirm : ServerboundPacket
    {

        public readonly int TeleportID;

        public TeleportConfirm(int teleportID) : base(0x00) 
        {
            TeleportID = teleportID;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WriteVarInt(PacketID);
            packetStream.WriteVarInt(TeleportID);
            return packetStream.ToPacket();
        }
    }
}
