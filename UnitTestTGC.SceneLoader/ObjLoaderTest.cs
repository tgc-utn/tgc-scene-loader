using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.SceneLoader;
using TGC.SceneLoader.Model;

namespace UnitTestTGC.SceneLoader
{
    [TestFixture]
    public class ObjLoaderTest
    {
        private string _fullobjpath;
        private string _fullobjpathmeshcolorsolo;
        private string _fullobjpathmeshcontextura;
        private Panel _panel3D;

        [SetUp]
        public void Init()
        {
            const string testDataCuboTextura = "cubotexturacaja.obj";
            const string testDataMeshColorSolo = "tgcito\\Tgcito color solo.obj";
            const string testDataMeshConTextura = "tgcito\\tgcito con textura.obj";

            //Creamos los materiales para luego poder probar la creacion del mesh
            var dir = TestContext.CurrentContext.TestDirectory + "\\..\\..\\Resources";
            _fullobjpath = Path.Combine(dir, testDataCuboTextura);
            _fullobjpathmeshcolorsolo = Path.Combine(dir, testDataMeshColorSolo);
            _fullobjpathmeshcontextura = Path.Combine(dir, testDataMeshConTextura);

            //Instanciamos un panel para crear un device
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
        public void LoadObjFromFileOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            tgcObjLoader.LoadObjFromFile(_fullobjpath);
            Assert.Greater(tgcObjLoader.ObjMeshContainer.ListObjMesh.Count, 0);
            Assert.AreEqual(tgcObjLoader.ObjMeshContainer.VertexListV.Count, 8);
        }

        [TestCase]
        public void ProcessLineReturnWithLineBlanck()
        {
            var tgcObjLoader = new TGCObjLoader();
            var line = String.Empty;
            tgcObjLoader.ProccesLine(line);
            Assert.AreEqual(tgcObjLoader.ObjMeshContainer.ListObjMesh.Count, 0);
        }

        [TestCase]
        public void ProcessLineReturnWithSpaceBlanck()
        {
            var tgcObjLoader = new TGCObjLoader();
            var line = "        ";
            tgcObjLoader.ProccesLine(line);
            Assert.AreEqual(tgcObjLoader.ObjMeshContainer.ListObjMesh.Count, 0);
        }

        [TestCase]
        public void ProcessLineReturnWithFirstCaracterHastag()
        {
            var tgcObjLoader = new TGCObjLoader();
            var line = "# Blender v2.79 (sub 0) OBJ File: ''";
            tgcObjLoader.ProccesLine(line);
            Assert.AreEqual(tgcObjLoader.ObjMeshContainer.ListObjMesh.Count, 0);
        }

        [TestCase]
        public void ProcessLineThrowWithBadAction()
        {
            var tgcObjLoader = new TGCObjLoader();
            var line = "badAction Blender v2.79 (sub 0) OBJ File: ''";
            Assert.That(() => { tgcObjLoader.ProccesLine(line); }, Throws.InvalidOperationException);
        }

        [TestCase]
        public void ProccesLineNewObjet()
        {
            var tgcObjLoader = new TGCObjLoader();
            var line = "o Cube";
            tgcObjLoader.ProccesLine(line);
            Assert.AreEqual(tgcObjLoader.ObjMeshContainer.ListObjMesh.First().Name, "Cube");
        }

        [TestCase]
        public void GetListOfMaterialsOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            var lines = File.ReadAllLines(_fullobjpath);
            tgcObjLoader.GetListOfMaterials(lines, _fullobjpath);
            Assert.Greater(tgcObjLoader.ListMtllib.Count, 0);
        }

        [TestCase]
        public void GetListOfMaterialsWithNameOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            var lines = File.ReadAllLines(_fullobjpath);
            tgcObjLoader.GetListOfMaterials(lines, _fullobjpath);
            Assert.AreEqual(tgcObjLoader.ListMtllib.First(), "cubotexturacaja.mtl");
        }

        [TestCase]
        public void GetListOfMaterialsWithWhiteSpaceOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            var lines = File.ReadAllLines(_fullobjpathmeshcontextura);
            tgcObjLoader.GetListOfMaterials(lines, _fullobjpathmeshcontextura);
            Assert.AreEqual(tgcObjLoader.ListMtllib.First(), "tgcito con textura.mtl");
        }

        [TestCase]
        public void FilterByKeyWordOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            var lines = File.ReadAllLines(_fullobjpath);
            string mtllib = tgcObjLoader.FilterByKeyword(lines, "mtllib")[0];
            Assert.AreEqual(mtllib, "mtllib cubotexturacaja.mtl");
        }

        [TestCase]
        public void LoadTgcMeshFromObjwithOutMaterialsOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            TgcMesh tgcMesh = tgcObjLoader.LoadTgcMeshFromObj(_fullobjpathmeshcolorsolo, 0);
            Assert.NotNull(tgcMesh);
        }

        [TestCase]
        public void LoadTgcMeshFromObjOk()
        {
            var tgcObjLoader = new TGCObjLoader();
            TgcMesh tgcMesh = tgcObjLoader.LoadTgcMeshFromObj(_fullobjpath, 0);
            Assert.NotNull(tgcMesh);
        }
    }
}