using Microsoft.Xna.Framework;
using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET
{
    public class Block
    {

        public Material Material = Material.Air;
        public int X = 0;
        public int Y = 0;
        public int Z = 0;
        public Vector3 Position { get { return new Vector3(X, Y, Z); } set { X = (int)value.X; Y = (int)value.Y; Z = (int)value.Z; } }

        public Block() { }
        public Block(Material material)
        {
            Material = material;
        }
        public Block(int x, int y, int z, Material material) : this(material)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Block(Vector3 position, Material material) : this((int)position.X, (int)position.Y, (int)position.Z, material)
        {

        }
    }
}
