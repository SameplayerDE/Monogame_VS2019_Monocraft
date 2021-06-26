using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Data
{
    public class Position
    {

        public static readonly Position Zero = new Position();

        public float X = 0;
        public float Y = 0;
        public float Z = 0;

        public Position() { }
        public Position(int x, int y, int z) 
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{"{" + X}, {Y}, {Z + "}"}";
        }

    }
}
