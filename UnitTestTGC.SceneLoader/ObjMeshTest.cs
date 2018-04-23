using NUnit.Framework;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.SceneLoader.Model;

namespace UnitTestTGC.SceneLoader
{
    [TestFixture]
    public class ObjMeshTest
    {
        private string _fullobjpathmultimaterial;
        private ObjMesh _resObjMesh;
        private Panel _panel3D;

        [SetUp]
        public void Init()
        {
            const string testDatabb8Multimaterial = "bb8\\bb8.obj";

            //Creamos los materiales para luego poder probar la creacion del mesh
            var dir = TestContext.CurrentContext.TestDirectory + "\\..\\..\\Resources";
            _fullobjpathmultimaterial = Path.Combine(dir, testDatabb8Multimaterial);

            //Instanciamos un panel para crear un divice
            _panel3D = new Panel
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
                Name = "panel3D",
                Size = new Size(784, 561),
                TabIndex = 0
            };
            //Crear Graphics Device
            D3DDevice.Instance.InitializeD3DDevice(_panel3D);
        }

        [TestCase]
        public void CreateMaterialIdsArrayOk()
        {
            TGCObjLoader tgcObjLoader = new TGCObjLoader();
            tgcObjLoader.LoadObjFromFile(_fullobjpathmultimaterial);
            _resObjMesh = tgcObjLoader.ObjMeshContainer.ListObjMesh.First();
            Assert.AreEqual(_resObjMesh.FaceTriangles.Count, _resObjMesh.CreateMaterialIdsArray().Length);
        }

        [TestCase]
        public void IndexMaterialIdsArrayOk()
        {
            TGCObjLoader tgcObjLoader = new TGCObjLoader();
            tgcObjLoader.LoadObjFromFile(_fullobjpathmultimaterial);
            _resObjMesh = tgcObjLoader.ObjMeshContainer.ListObjMesh.First();
            int[] materialIds = _resObjMesh.CreateMaterialIdsArray();
            Assert.AreEqual(materialIds[15810], 0);
            Assert.AreEqual(materialIds[17010], 1);
        }
    }
}