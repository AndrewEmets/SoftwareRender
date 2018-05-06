using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareRender;

namespace SoftwareRendererTest
{
    [TestClass]
    public class UnitTestMatrices
    {
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        [TestMethod]
        public void TestMethodTransMulScale()
        {
            var transVec = new Vector3(2, 4, 6);
            var a = Matrix4X4.Translation(transVec);
            var scaleVec = new Vector3(3, 2, 1);
            
            var b = Matrix4X4.Scale(scaleVec);
            
            var c = a * b;
            var expected = new Matrix4X4(new[,]
            {
                {scaleVec.x, 0, 0, transVec.x},
                {0, scaleVec.y, 0, transVec.y},
                {0, 0, scaleVec.z, transVec.z},
                {0, 0, 0, 1}
            });
            Assert.AreEqual(c,expected);
        }

        [TestMethod]
        public void TestMul()
        {
            var a = new Matrix4X4(new float[,]
            {
                {1, 2, 3, 4},
                {5, 6, 7, 8},
                {9, 0, 9, 8},
                {7, 6, 5, 4}
            });
            var b = new Matrix4X4(new float[,]
            {
                {0, 9, 8, 7},
                {6, 5, 4, 3},
                {2, 1, 2, 3},
                {4, 5, 6, 7},
            });
            var result = a * b;
            var expected = new Matrix4X4(new float[,]
            {
                {34, 42, 46, 50},
                {82, 122, 126, 130},
                {50, 130, 138, 146},
                {62, 118, 114, 110}
            });
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestMatMulVec()
        {
            var offset = new Vector3(1,1,1);
            var m = Matrix4X4.Translation(offset);
            var v = new Vector3(0,0,0);
            var result = m * v;
            var expected = v + offset;

            Assert.AreEqual(result,expected);
        }
    }
}
