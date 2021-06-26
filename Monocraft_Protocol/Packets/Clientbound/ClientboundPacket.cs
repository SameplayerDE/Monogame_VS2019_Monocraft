namespace Monocraft_Protocol.Packets.Clientbound
{
    public abstract class ClientboundPacket : Packet
    {
        public ClientboundPacket(byte packetID) : base(packetID) { }
    }
}