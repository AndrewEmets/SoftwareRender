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

        Mesh mesh;

        private void Form1_Load(object sender, EventArgs e)
        {
            mesh = ObjDriver.LoadMeshFromFile("E:\\head.obj");

            renderer.Render(mesh);
            RenderImage();
        }
        Vector3 offset = Vector3.Zero;
        private void button1_Click(object sender, EventArgs e)
        {
            renderer.Render(mesh);
            //pictureBox1.Refresh();
            RenderImage();
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
