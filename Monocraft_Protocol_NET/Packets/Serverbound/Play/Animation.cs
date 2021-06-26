using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Serverbound.Play
{

    public enum Hand
    {
        Main_Hand = 0x00,
        Off_Hand = 0x01
    }

    public class Animation : ServerboundPacket
    {

        public readonly Hand Hand;

        public Animation(Hand hand) : base(0x2C) 
        {
            Hand = hand;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            writer.WriteVarInt((int)Hand);
            return writer.ToArray();
        }
    }
}
