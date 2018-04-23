using System;
using System.Linq;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    /// <summary>
    ///     Estrategia para agregar si posee shadow o no
    /// </summary>
    public class AddShadowStrategy : ObjParseStrategy
    {
        private const int INDEXATTR = 1;

        /// <summary>
        ///     Agrega la constante que indetifica a la clase en el atributo keyword
        /// </summary>
        public AddShadowStrategy()
        {
            Keyword = SHADOW;
        }

        /// <summary>
        ///    Procesa la linea para habilitar el shadow.
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <param name="objMeshContainer">Clase ObjMaterialLader</param>
        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            string attribute = CheckAttribute(line); //
            if (attribute.Equals("1")) objMeshContainer.ListObjMesh.Last().Shadow = true;
        }

        /// <summary>
        ///    Checkea que la informacion de la linea sea correcta para procesarla
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <returns>string</returns>
        private string CheckAttribute(string line)
        {
            if (line.Split(' ').Length != 2) throw new ArgumentException("El atributo Shadow tiene formato incorrecto");
            var attribute = line.Split(' ')[INDEXATTR];
            if (!attribute.Equals("1") && !attribute.Equals("off")) throw new ArgumentException("Comando para el atributo shadow incorrecto. Se esperaba: \"1\" u \"off\" y se obtuvo:" + attribute);
            return attribute;
        }
    }
}