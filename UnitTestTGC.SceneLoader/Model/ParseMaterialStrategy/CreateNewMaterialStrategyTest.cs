using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParseMaterialsStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParseMaterialStrategy
{
    [TestFixture]
    public class CreateNewMaterialStrategyTest
    {
        private readonly CreateNewMaterialStrategy _createNewMeshMaterialStrategy = new CreateNewMaterialStrategy();
        private List<ObjMaterialMesh> ListObjMaterialMesh { get; } = new List<ObjMaterialMesh>();

        [TestCase]
        public void ProccesLineNewObjetOk()
        {
            var line = "newmtl Material.001";
            _createNewMeshMaterialStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.AreEqual(ListObjMaterialMesh.First().Name, "Material.001");
        }
    }
}