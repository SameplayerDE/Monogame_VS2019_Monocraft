using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Protocol_NET.Packets.Serverbound.Play
{
    public class ClientSettings : ServerboundPacket
    {

        public string Locale;
        public byte ViewDistance;
        public ChatTypeClientSettings ChatType;
        public bool ChatColours;
        public byte SkinFlag;
        public MainHand Hand;

        public ClientSettings(string locale, byte viewDistance, ChatTypeClientSettings chatType, bool chatColours, byte skinFlag, MainHand hand) : base(0x05)
        {
            Locale = locale;
            ViewDistance = viewDistance;
            ChatType = chatType;
            ChatColours = chatColours;
            SkinFlag = skinFlag;
            Hand = hand;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream stream = new PacketStream();
            if (compressionThreshold != -1 && compressionThreshold > 9)
            {
                stream.WriteByte(0x00); // No Compression
            }
            stream.WriteVarInt(PacketID);
            stream.WriteString(Locale);
            stream.WriteByte(ViewDistance);
            stream.WriteVarInt((int)ChatType);
            stream.WriteBoolean(ChatColours);
            stream.WriteByte(SkinFlag);
            stream.WriteVarInt((int)Hand);
            return stream.ToArray();
        }
    }
}
