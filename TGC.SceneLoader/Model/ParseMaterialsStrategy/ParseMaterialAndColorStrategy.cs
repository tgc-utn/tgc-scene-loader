using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TGC.SceneLoader.Model.ParseMaterialsStrategy
{
    public class ParseMaterialAndColorStrategy : ObjMaterialsParseStrategy
    {
        private readonly string[] _keyWords;
        private const char Espacio = ' ';
        private readonly string[] _colorVariables;
        private readonly string[] _vectorVariables;
        private readonly string[] _pathTextureVariables;

        public ParseMaterialAndColorStrategy()
        {
            _keyWords = new[] { Ns, Ka, Kd, Ks, Ke, Ni, Tr, Tf, d, illum, map_Kd, disp, map_Bump, map_Ka, map_ks, map_d };
            _colorVariables = new[] { Ka, Kd, Ks, Ke, Tf };
            _vectorVariables = new[] { Ns, Ni, d, Tr };
            _pathTextureVariables = new[] { map_Kd, disp, map_Bump, map_Ka, map_ks, map_d };
        }

        //TODO este proccesLine esta bien feito, refactorizarlo
        public override void ProccesLine(string line, List<ObjMaterialMesh> listObjMaterialMesh)
        {
            line = line.TrimStart().TrimEnd(); //Sacamos los espacios del pricncipio y del final
            string key = line.Split(Espacio).FirstOrDefault();
            ObjMaterialMesh auxObjMaterialMesh = listObjMaterialMesh.Last();
            PropertyInfo pInfo = auxObjMaterialMesh.GetType().GetProperty(key);
            if (pInfo == null) throw new ArgumentException("No se encuantra el atributo de clase con la key: ", key);
            if (_colorVariables.Contains(key))
            {
                pInfo.SetValue(auxObjMaterialMesh, CreateColorValue(line), null);
            }

            if (_vectorVariables.Contains(key))
            {
                pInfo.SetValue(auxObjMaterialMesh, ParseLineToFLoatValue(line), null);
            }

            if (key.Equals(illum))
            {
                pInfo.SetValue(auxObjMaterialMesh, ParseLineToIntValue(line), null);
            }

            if (_pathTextureVariables.Contains(key))
            {
                pInfo.SetValue(auxObjMaterialMesh, ParsePathToStringExistValue(line), null);
            }
        }

        public override bool ResponseTo(string action)
        {
            return _keyWords.Contains(action);
        }
    }
}