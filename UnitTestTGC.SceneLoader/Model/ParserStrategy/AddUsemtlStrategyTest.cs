using NUnit.Framework;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class AddUsemtlStrategyTest
    {
        private readonly AddUsemtlStrategy _addUsemtlStrategy = new AddUsemtlStrategy();
        private readonly CreateNewMeshStrategy _createNewMeshStrategy = new CreateNewMeshStrategy();
        private readonly ObjMeshContainer _objMeshContainer = new ObjMeshContainer();

        [SetUp]
        public void Init()
        {
            var line = "o Cube";
            _createNewMeshStrategy.ProccesLine(line, _objMeshContainer);
        }

        [Test]
        public void HaveAttributeUsemtlinitialized()
        {
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().Usemtl.Count, 0);
            Assert.NotNull(_objMeshContainer.ListObjMesh.Last().Usemtl);
        }

        [Test]
        public void AsignateAttributeUsemtlOk()
        {
            var line = "usemtl Material.001";
            _addUsemtlStrategy.ProccesLine(line, _objMeshContainer);
            Assert.Greater(_objMeshContainer.ListObjMesh.Last().Usemtl.Count, 0);
        }

        [Test]
        public void AsignateAttributeUsemtlWhenIsNoneOk()
        {
            var line = "usemtl Material.001";
            _addUsemtlStrategy.ProccesLine(line, _objMeshContainer);
            line = "usemtl None";
            _addUsemtlStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.Last().Usemtl.Last(), "None");
        }

        [Test]
        public void AsignateAttributeUsemtlFalisByNumberOfParameters()
        {
            var line = "s bad Parameter";
            Assert.That(() => { _addUsemtlStrategy.ProccesLine(line, _objMeshContainer); }, Throws.ArgumentException);
        }
    }
}