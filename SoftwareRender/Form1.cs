using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRender
{

    public partial class Form1 : Form
    {
        private Renderer renderer;

        public Form1()
        {
            InitializeComponent();

            renderer = new Renderer(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            Vector3[] verts =
            {
                new Vector3(-1, -1, -1),
                new Vector3( 1, -1, -1),
                new Vector3(-1,  1, -1),
                new Vector3( 1,  1, -1),

                new Vector3(-1, -1,  1),
                new Vector3( 1, -1,  1),
                new Vector3(-1,  1,  1),
                new Vector3( 1,  1,  1),
            };

            int[] tris =
            {
                0, 2, 1,  2, 3, 1,
                0, 4, 1,  4, 5, 1,
                4, 6, 5,  6, 7, 5
            };*/

            /*Vector3[] verts =
            {
                new Vector3(20,  45 , 0),
                new Vector3(35,  20 , 0),
                new Vector3(75,  25 , 0),
                new Vector3(120, 50, 0),
                new Vector3(150, 30, 0),
            };

            int[] tris =
            {
                0,2,1,
                0,2,3,
                2,3,4
            };

            var mesh = new Mesh(verts, tris);*/

            var mesh = ObjDriver.LoadMeshFromFile("C:\\head.obj");

            renderer.Render(mesh);
            RenderImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
        }

        private void RenderImage()
        {
            pictureBox1.Image = renderer.GetBitmap();
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //renderer.GetBitmap(e.Graphics);

            //e.Graphics.Dispose();
        }
    }
}
