using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class RawCubeMesh : RawMesh
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

        public RawCubeMesh() : base(_positions, _indicies, _indexed) { }
    }
    public class ColoredCubeMesh : ColoredMesh
    {

        private static Vector3[] _positions = new Vector3[]
        {
            new Vector3(-.5f, 0, -.5f), // 0
            new Vector3(.5f, 0, -.5f), // 1
            new Vector3(.5f, 1.63f, -.5f), // 2
            new Vector3(-.5f, 1.63f, -.5f), // 3

            new Vector3(-.5f, 0, .5f), // 4
            new Vector3(.5f, 0, .5f), // 5
            new Vector3(.5f, 1.63f, .5f), // 6
            new Vector3(-.5f, 1.63f, .5f), // 7
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

        public ColoredCubeMesh(Color color) : base(_positions, _indicies, _indexed, color) { }
    }

    public class TexturedCubeMesh : TexturedMesh
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

        private static Vector2[] _uvs = new Vector2[]
        {
            new Vector2(0, 0), // 0
            new Vector2(0, 1), // 1
            new Vector2(1, 0), // 2
            new Vector2(1, 1), // 3
        };

        private new static int[][] _indicies = new int[][]
        {
            new int[] { 0, 1, 2, 1, 3, 2}, // front
            new int[] { 2, 3, 0, 2, 0, 1},

            new int[] { 1, 5, 6, 1, 3, 2 }, // right
            new int[] { 6, 2, 1, 2, 0, 1 },

            new int[] { 7, 6, 5, 1, 3, 2 }, // back
            new int[] { 5, 4, 7, 2, 0, 1 },

            new int[] { 4, 0, 3, 1, 3, 2 }, // left
            new int[] { 3, 7, 4, 2, 0, 1 },

            new int[] { 4, 5, 1, 1, 3, 2 }, // bottom
            new int[] { 1, 0, 4, 2, 0, 1 },

            new int[] { 3, 2, 6, 1, 3, 2 }, // top
            new int[] { 6, 7, 3, 2, 0, 1 }
        };


        private new static bool _indexed = true;
        public Texture2D Texture;

        public TexturedCubeMesh() : base(_positions, _uvs, _indicies, _indexed)
        {
            Texture = MonocraftGame.TextureManager.Textures["Not_Found"];
        }

        public TexturedCubeMesh(Material material) : base(_positions, _uvs, _indicies, _indexed)
        {
            if (MonocraftGame.TextureManager.Textures.ContainsKey($"block/{material.ToString().ToLower()}"))
            {
                Texture = MonocraftGame.TextureManager.Textures[$"block/{material.ToString().ToLower()}"];
            }
            else
            {
                Texture = MonocraftGame.TextureManager.Textures["Not_Found"];
            }
        }

        public TexturedCubeMesh(Texture2D texture) : base(_positions, _uvs, _indicies, _indexed) 
        {
            Texture = texture;
        }
    }

}
