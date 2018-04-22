using NUnit.Framework;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class CreateNewMeshStrategyTest
    {
        private readonly CreateNewMeshStrategy _createNewMeshStrategy = new CreateNewMeshStrategy();
        private ObjMeshContainer _objMeshContainer;

        [SetUp]
        public void Init()
        {
            _objMeshContainer = new ObjMeshContainer();
        }

        [Test]
        public void ProccesLineNewObjetOk()
        {
            var line = "o Cube";
            _createNewMeshStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(_objMeshContainer.ListObjMesh.First().Name, "Cube");
        }
    }
}