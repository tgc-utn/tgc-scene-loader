using NUnit.Framework;
using System.Collections.Generic;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParserStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParserStrategy
{
    [TestFixture]
    public class NoOperationStrategyTest
    {
        private ObjMeshContainer _objMeshContainer;
        private readonly NoOperationStrategy _noOperationStrategy = new NoOperationStrategy();

        [SetUp]
        public void Init()
        {
            _objMeshContainer = new ObjMeshContainer();
        }

        [Test]
        public void ProccesLineNoOperationHashtagOk()
        {
            // TODO tiene sentido el test?
            List<ObjMesh> listObjMesh = new List<ObjMesh>();
            var line = "# o Cube";
            _noOperationStrategy.ProccesLine(line, _objMeshContainer);
            Assert.AreEqual(listObjMesh.Count, 0);
        }
    }
}