using NUnit.Framework;
using System.IO;
using System.Linq;
using TGC.SceneLoader.Model;

namespace UnitTestTGC.SceneLoader
{
    [TestFixture]
    public class ObjMeshTest
    {
        private string _fullobjpathmultimaterial;
        private ObjMesh _resObjMesh;

        [SetUp]
        public void Init()
        {
            const string testDatabb8Multimaterial = "bb8\\bb8.obj";

            //Creamos los materiales para luego poder probar la creacion del mesh
            var dir = TestContext.CurrentContext.TestDirectory + "\\..\\..\\Resources";
            _fullobjpathmultimaterial = Path.Combine(dir, testDatabb8Multimaterial);
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