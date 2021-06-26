namespace Monocraft_Protocol.Packets.Serverbound
{
    public abstract class ServerboundPacket : Packet
    {
        public ServerboundPacket(byte packetID) : base(packetID) { }
    }
}