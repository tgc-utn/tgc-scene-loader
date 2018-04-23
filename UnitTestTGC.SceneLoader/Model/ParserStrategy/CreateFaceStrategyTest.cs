using NUnit.Framework;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class CreateFaceStrategyTest
    {
        private readonly CreateFaceStrategy _createFaceStrategy = new CreateFaceStrategy();
        private readonly AddUsemtlStrategy _addUsemtlStrategy = new AddUsemtlStrategy();
        private readonly CreateNewMeshStrategy _createNewMeshStrategy = new CreateNewMeshStrategy();
        private readonly ObjMeshContainer _objMeshContainer = new ObjMeshContainer();

        [SetUp]
        public void Init()
        {
            var line = "o Cube";
            _createNewMeshStrategy.ProccesLine(line, _objMeshContainer);
            line = "usemtl Material.001";
            _addUsemtlStrategy.ProccesLine(line, _objMeshContainer);
        }

        [Test]
        public void CreateFaceOk()
        {
            var line = "f 2/2/1 4/1/1 1/1/1";
            _createFaceStrategy.ProccesLine(line, _objMeshContainer);
            Assert.Greater(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Count, 0);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().V1, 2);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().Vt1, 2);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().Vn1, 1);
        }

        [Test]
        public void CreateFaceWithoutVertexNormalOk()
        {
            var line = "f 2/2 4/2 1/2";
            _createFaceStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().Vn2, 0);
        }

        [Test]
        public void CreateFaceWithoutVertexTextureOk()
        {
            var line = "f 2//1 4//1 1//1";
            _createFaceStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().Vt2, 0);
        }

        [Test]
        public void AttributeUsemtlAdded()
        {
            var line = "f 2/2 4/2 1/2";
            _createFaceStrategy.ProccesLine(line, _objMeshContainer);
            line = "usemtl Material.001";
            _addUsemtlStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().FaceTriangles.Last().Usemtl, "Material.001");
        }

        [TestCase]
        public void CreateFaceProccesLineWithTextureFailsWhenCountParametersUp()
        {
            var line = "f 2/2/1 4/1/1 1/1/1 1/1/1";
            Assert.That(() => { _createFaceStrategy.ProccesLine(line, _objMeshContainer); }, Throws.ArgumentException);
        }
    }
}