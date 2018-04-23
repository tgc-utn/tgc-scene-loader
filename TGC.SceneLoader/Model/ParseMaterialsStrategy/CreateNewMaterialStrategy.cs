using System.Collections.Generic;

namespace TGC.SceneLoader.Model.ParseMaterialsStrategy
{
    public class CreateNewMaterialStrategy : ObjMaterialsParseStrategy
    {
        private const int NameMaterial = 1;

        public CreateNewMaterialStrategy()
        {
            Keyword = Newmtl;
        }

        public override void ProccesLine(string line, List<ObjMaterialMesh> listObjMaterialMesh)
        {
            var split = line.Split(' ');
            listObjMaterialMesh.Add(new ObjMaterialMesh(split[NameMaterial]));
        }
    }
}