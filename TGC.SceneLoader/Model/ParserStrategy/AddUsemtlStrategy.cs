using System;
using System.Linq;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    /// <summary>
    ///     Estrategia para agregar material a la clase face
    /// </summary>
    public class AddUsemtlStrategy : ObjParseStrategy
    {
        private const int INDEXATTR = 1;

        /// <summary>
        ///     Agrega la constante que indetifica a la clase en el atributo keyword
        /// </summary>
        public AddUsemtlStrategy()
        {
            Keyword = USEMTL;
        }

        /// <summary>
        ///    Procesa la linea para guardar el material que va a usar los siguientes poligonos
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <param name="objMeshContainer">Clase ObjMaterialLader</param>
        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            var attribute = CheckAttribute(line);
            objMeshContainer.ListObjMesh.Last().Usemtl.Add(attribute);
        }

        /// <summary>
        ///    Checkea que la informacion de la linea sea correcta para procesarla
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <returns>string</returns>
        private string CheckAttribute(string line)
        {
            if (line.Split(' ').Length != 2) throw new ArgumentException("El atributo usemtl tiene formato incorrecto");
            var attribute = line.Split(' ')[INDEXATTR];
            return attribute;
        }
    }
}