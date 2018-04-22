using System;
using System.Globalization;
using Microsoft.DirectX;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    public abstract class ObjParseStrategy
    {
        internal const string OBJECT = "o";
        internal const string VERTEX = "v";
        internal const string TEXTURE = "vt";
        internal const string NORMAL = "vn";
        internal const string FACE = "f";
        internal const string MTLLIB = "mtllib";
        internal const string USEMTL = "usemtl";
        internal const string SHADOW = "s";
        internal const string COMMENT = "#";
        internal const string WHITELINE = "      ";
        internal const string EMPTYLINE = "";

        internal string Keyword = null;
        private NumberStyles Style { get; } = NumberStyles.Any;
        private CultureInfo Info { get; } = CultureInfo.InvariantCulture;

        public abstract void ProccesLine(string line, ObjMeshContainer objMeshContainer);

        public virtual bool ResponseTo(string action)
        {
            return action == Keyword;
        }

        public Object CreateVector3(string line) //TODO esto podria ir en una clase utils porque se repite para materiales
        {
            var vertex = new Object();
            var indices = line.Split(' ');
            if (indices.Length != 4 && indices.Length != 3) throw new ArgumentException("El Archivo .obj no fue exportado de forma triangular");
            if (indices.Length == 4)
            {
                vertex = new Vector3
                {
                    X = float.Parse(indices[1], Style, Info),
                    Y = float.Parse(indices[2], Style, Info),
                    Z = float.Parse(indices[3], Style, Info)
                };
            }
            else
            {
                vertex = new Vector2
                {
                    X = float.Parse(indices[1], Style, Info),
                    Y = float.Parse(indices[2], Style, Info)
                };
            }
            return vertex;
        }
    }
}