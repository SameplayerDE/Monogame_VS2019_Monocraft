namespace Monocraft_Protocol_NET.Packets.Serverbound
{
    public abstract class ServerboundPacket : Packet
    {
        public ServerboundPacket(byte packetID) : base(packetID) { }
    }
}