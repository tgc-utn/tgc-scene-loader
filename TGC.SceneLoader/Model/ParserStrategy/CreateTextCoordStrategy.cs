using Microsoft.DirectX;
using System;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    /// <summary>
    ///     Estrategia para crear vector de textura
    /// </summary>
    public class CreateTextCoordStrategy : ObjParseStrategy
    {
        /// <summary>
        ///     Agrega la constante que indetifica a la clase en el atributo keyword
        /// </summary>
        public CreateTextCoordStrategy()
        {
            Keyword = TEXTURE;
        }

        /// <summary>
        ///    Procesa la linea para crear un vector de textura
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <param name="objMeshContainer">Clase ObjMaterialLader</param>
        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            var indices = line.Split(' ');
            if (indices.Length != 3) throw new ArgumentException("El Archivo .obj no fue exportado de forma triangular");
            var vertex = CreateVector3(line);
            objMeshContainer.VertexListVt.Add((Vector2)vertex);
        }
    }
}