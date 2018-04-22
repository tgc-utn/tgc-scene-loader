using NUnit.Framework;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class AddShadowStrategyTest
    {
        private readonly AddShadowStrategy _addShadowStrategy = new AddShadowStrategy();
        private readonly CreateNewMeshStrategy _createNewMeshStrategy = new CreateNewMeshStrategy();
        private readonly ObjMeshContainer _objMeshContainer = new ObjMeshContainer();

        [SetUp]
        public void Init()
        {
            var line = "o Cube";
            _createNewMeshStrategy.ProccesLine(line, _objMeshContainer);
        }

        [Test]
        public void AsignateAttributeShadowOk()
        {
            var line = "s 1";
            _addShadowStrategy.ProccesLine(line, _objMeshContainer);
            Assert.IsTrue(_objMeshContainer.ListObjMesh.Last().Shadow);
        }

        [Test]
        public void AsignateAttributeFalisByNumberOfParameters()
        {
            var line = "s";
            Assert.That(() => { _addShadowStrategy.ProccesLine(line, _objMeshContainer); }, Throws.ArgumentException);
        }

        [Test]
        public void AsignateAttributeShadowFaliedForWrongParametro()
        {
            var line = "s badParameter";
            Assert.That(() => { _addShadowStrategy.ProccesLine(line, _objMeshContainer); }, Throws.ArgumentException);
        }

        //TODO hacer el test cuando s viene 1 ó 0 por si es smooth o no
    }
}