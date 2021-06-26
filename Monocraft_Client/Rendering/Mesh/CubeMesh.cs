using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Rendering
{
    public class CubeMesh : RawMesh
    {

        private static Vector3[] _positions = new Vector3[]
        {
            new Vector3(0, 0, 0), // 0
            new Vector3(1, 0, 0), // 1
            new Vector3(1, 1, 0), // 2
            new Vector3(0, 1, 0), // 3

            new Vector3(0, 0, 1), // 4
            new Vector3(1, 0, 1), // 5
            new Vector3(1, 1, 1), // 6
            new Vector3(0, 1, 1), // 7
        };
        private new static int[][] _indicies = new int[][]
        {
            new int[] { 0, 1, 2 }, // front
            new int[] { 2, 3, 0 },

            new int[] { 1, 5, 6 }, // right
            new int[] { 6, 2, 1 },

            new int[] { 7, 6, 5 }, // back
            new int[] { 5, 4, 7 },

            new int[] { 4, 0, 3 }, // left
            new int[] { 3, 7, 4 },

            new int[] { 4, 5, 1 }, // bottom
            new int[] { 1, 0, 4 },

            new int[] { 3, 2, 6 }, // top
            new int[] { 6, 7, 3 }
        };
        private new static bool _indexed = true;

        public CubeMesh() : base(_positions, _indicies, _indexed) { }
    }
}
