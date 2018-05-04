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
            List<int> tris = new List<int>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var words = line.Split(' ');

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
                }
            }

            var mesh = new Mesh(verts.ToArray(), tris.ToArray());

            return mesh;
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

            return result;
        }
    }
}
