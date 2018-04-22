using NUnit.Framework;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class CreateNormalStrategyTest
    {
        private readonly CreateNormalStrategy _createNormalStrategy = new CreateNormalStrategy();
        private readonly CreateNewMeshStrategy _createNewMeshStrategy = new CreateNewMeshStrategy();
        private ObjMeshContainer _objMeshContainer;

        [SetUp]
        public void Init()
        {
            _objMeshContainer = new ObjMeshContainer();
            var line = "o Cube";
            _createNewMeshStrategy.ProccesLine(line, _objMeshContainer);
        }

        [Test]
        public void ProccesLinewithNormalOk()
        {
            var line = "v 1.000000 -1.000000 -1.000000";
            _createNormalStrategy.ProccesLine(line, _objMeshContainer);
            Assert.Greater(_objMeshContainer.VertexListVn.Count, 0);
        }
    }
}