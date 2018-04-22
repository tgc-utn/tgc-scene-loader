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
    public class ObjMaterialLoaderTest
    {
        private string _fullMaterialPath;
        private Panel _panel3D;

        [SetUp]
        public void Init()
        {
            const string testDataArchivoBla = "cubotexturacaja.obj";  //TODO agregar el archivo para testear

            //Creamos los materiales para luego poder probar la creacion del mesh
            var dir = TestContext.CurrentContext.TestDirectory + "\\..\\..\\Resources";
            _fullMaterialPath = Path.Combine(dir, testDataArchivoBla);

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
        public void LoadObjMaterialFromFileOk()
        {
            TGCObjLoader tgcObjLoader = new TGCObjLoader();
            var lines = File.ReadAllLines(_fullMaterialPath);
            tgcObjLoader.GetListOfMaterials(lines, _fullMaterialPath);
            ObjMaterialsLoader objMaterialLoader = new ObjMaterialsLoader();
            objMaterialLoader.LoadMaterialsFromFiles(_fullMaterialPath, tgcObjLoader.ListMtllib);
            Assert.NotNull(objMaterialLoader.ListObjMaterialMesh.First());
        }

        [TestCase]
        public void GetPathMaterialOk()
        {
            ObjMaterialsLoader objMaterialLoader = new ObjMaterialsLoader();
            objMaterialLoader.SetDirectoryPathMaterial(_fullMaterialPath);
            string pathMaterial = objMaterialLoader.MaterialPath("\\cubotexturacaja.mtl");
            Assert.IsTrue(File.Exists(pathMaterial));
        }
    }
}