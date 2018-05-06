using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SoftwareRender
{
    public class ObjDriver
    {
        public static Mesh LoadMeshFromFile(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException();

            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> tris = new List<int>();

            using (StreamReader sr = new StreamReader(path))
            {
                var separators = new[] { ' ' };

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                        continue;

                    if (words[0] == "v")
                    {
                        var vert = GetVertex(words);
                        verts.Add(vert);
                    }
                    else if (words[0] == "f")
                    {
                        var face = GetFace(words);
                        tris.AddRange(face);
                    }
                    else if (words[0] == "vt")
                    {
                        var uv = GetUV(words);
                        uvs.Add(uv);
                    }
                }
            }

            var mesh = new Mesh(verts.ToArray(), tris.ToArray())
            {
                transform = new Transform(Vector3.Zero, Vector3.One, Vector3.Zero)
            };

            return mesh;
        }
        private static Vector2 GetUV(string[] values)
        {
            var x = float.Parse(values[1], CultureInfo.InvariantCulture);
            var y = float.Parse(values[2], CultureInfo.InvariantCulture);

            return new Vector2(x, y);
        }

        private static int[] GetFace(string[] words)
        {
            int[] result = new int[3];

            for (int i = 1; i < words.Length; i++)
            {
                var ind = words[i].IndexOf('/');
                var value = words[i].Substring(0, ind);
                int t = int.Parse(value);
                result[i - 1] = t - 1;
            }

            return result;
        }

        private static Vector3 GetVertex(string[] values)
        {
            var result = new Vector3();

            for (int i = 1; i < values.Length; i++)
            {
                var value = float.Parse(values[i], CultureInfo.InvariantCulture);
                result[i - 1] = value;
            }
            result.w = 1;
            return result;
        }
    }
}
