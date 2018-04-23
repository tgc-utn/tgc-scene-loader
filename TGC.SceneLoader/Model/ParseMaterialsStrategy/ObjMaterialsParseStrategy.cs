using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TGC.SceneLoader.Model.ParseMaterialsStrategy
{
    public abstract class ObjMaterialsParseStrategy
    {
        //variable keywords que pertenecen a la sentencia de Color
        internal const string Newmtl = "newmtl";

        internal const string Ns = "Ns";
        internal const string Ka = "Ka";
        internal const string Kd = "Kd";
        internal const string Ks = "Ks";
        internal const string Ke = "Ke";
        internal const string Ni = "Ni";
        internal const string Tr = "Tr";
        internal const string Tf = "Tf";
        internal const string d = "d";
        internal const string illum = "illum";

        // variable keywords que pertenecen a la sentencia de textura
        internal const string map_Kd = "map_Kd";

        internal const string disp = "disp";
        internal const string map_Bump = "map_Bump";
        internal const string map_Ka = "map_Ka";
        internal const string map_ks = "map_Ks";
        internal const string map_d = "map_d";

        // otras keywords
        internal const string COMMENT = "#";

        internal const string WHITELINE = "      ";
        internal const string EMPTYLINE = "";

        internal string Keyword = null;
        private NumberStyles Style { get; } = NumberStyles.Any;
        private CultureInfo Info { get; } = CultureInfo.InvariantCulture;

        public abstract void ProccesLine(string line, List<ObjMaterialMesh> listObjMaterialMesh);

        public virtual bool ResponseTo(string action)
        {
            return action == Keyword;
        }

        public ColorValue CreateColorValue(string line) //TODO esto podria ir en una clase utils porque se repite para Obj
        {
            var indices = line.Split(' ');
            if (indices.Length != 4) throw new ArgumentException("El Archivo esta corrupto o tiene cantidad incorrecta de parametros");
            return new ColorValue
                 (
                     float.Parse(indices[1], Style, Info),
                     float.Parse(indices[2], Style, Info),
                     float.Parse(indices[3], Style, Info)
                 );
        }

        public float ParseLineToFLoatValue(string line)
        {
            var indices = line.Split(' ');
            if (indices.Length != 2) throw new ArgumentException("El Archivo esta corrupto o tiene cantidad incorrecta de parametros");
            return float.Parse(indices[1], Style, Info);
        }

        public int ParseLineToIntValue(string line)
        {
            var indices = line.Split(' ');
            if (indices.Length != 2) throw new ArgumentException("El Archivo esta corrupto o tiene cantidad incorrecta de parametros");
            return int.Parse(indices[1], Style, Info);
        }

        public object ParsePathToStringExistValue(string line)
        {
            var path = line.Substring(line.Split(' ').First().Length);
            // TODO ver como verifacar que el string sea un path throw new ArgumentException("La ruta no es un PATH valido");
            return path;
        }
    }
}