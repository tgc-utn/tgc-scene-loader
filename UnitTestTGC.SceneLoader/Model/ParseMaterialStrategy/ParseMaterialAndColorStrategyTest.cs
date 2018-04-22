using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TGC.SceneLoader.Model;
using TGC.SceneLoader.Model.ParseMaterialsStrategy;

namespace UnitTestTGC.SceneLoader.Model.ParseMaterialStrategy
{
    [TestFixture]
    public class ParseMaterialAndColorStrategyTest
    {
        private readonly ParseMaterialAndColorStrategy _parseMaterialAndColorStrategy = new ParseMaterialAndColorStrategy();
        private readonly CreateNewMaterialStrategy _createNewMeshMaterialStrategy = new CreateNewMaterialStrategy();
        private List<ObjMaterialMesh> ListObjMaterialMesh { get; } = new List<ObjMaterialMesh>();

        [SetUp]
        public void Init()
        {
            var line = "newmtl Material.001";
            _createNewMeshMaterialStrategy.ProccesLine(line, ListObjMaterialMesh);
        }

        [TestCase]
        public void ProccesLineWithAmbientValueOk()
        {
            var line = "Ka 0.000000 0.000000 0.000000";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Ka);
        }

        [TestCase]
        public void ProccesLineWithdiffuseValueOk()
        {
            var line = "Kd 0.640000 0.640000 0.640000";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Kd);
        }

        [TestCase]
        public void ProccesLineWithSpecularValueOk()
        {
            var line = "Ks 0.500000 0.500000 0.500000";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Ks);
        }

        [TestCase]
        public void ProccesLineWithDissolveValueOk()
        {
            var line = "d 1.000000";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().d);
        }

        [TestCase]
        public void ProccesLineWithIlumValueOk()
        {
            var line = "illum 2";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().illum);
        }

        [TestCase]
        public void ProccesLineWithSpecularComponentValueOk()
        {
            var line = "Ns 96.078431";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Ns);
        }

        [TestCase]
        public void ProccesLineWithOpticalDensityValueOk()
        {
            var line = "Ni 96.078431";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Ni);
        }

        [TestCase]
        public void ProccesLineWithTabCaracterInsertedNotFailed()
        {
            var line = "    Ns 96.078431";
            _parseMaterialAndColorStrategy.ProccesLine(line, ListObjMaterialMesh);
            Assert.NotNull(ListObjMaterialMesh.First().Ns);
        }
    }
}