using NUnit.Framework;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class CreateVertexStrategyTest
    {
        private readonly CreateVertexStrategy _createVertexStrategy = new CreateVertexStrategy();
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
        public void ProccesLinewithVertexOk()
        {
            var line = "v 1.000000 -1.000000 -1.000000";
            _createVertexStrategy.ProccesLine(line, _objMeshContainer);
            Assert.Greater(_objMeshContainer.VertexListV.Count, 0);
        }
    }
}