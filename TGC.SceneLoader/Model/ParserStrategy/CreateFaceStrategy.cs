using System;
using System.Linq;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    /// <summary>
    ///     Estrategia para crear un nuevo poligono
    /// </summary>
    public class CreateFaceStrategy : ObjParseStrategy
    {
        private const int Vertex = 0;
        private const int Texture = 1;
        private const int Normal = 2;
        private const int CompleteArray = 6; //Igual a 6 quiere decir que los tres vertices tienen un valor de textura y uno de posicion
        private const char TypeVertexDelimiter = '/';
        private const char VertexDelimiter = ' ';

        /// <summary>
        ///     Agrega la constante que indetifica a la clase en el atributo keyword
        /// </summary>
        public CreateFaceStrategy()
        {
            Keyword = FACE;
        }

        /// <summary>
        ///    Procesa la linea para crear un nuevo Poligono
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <param name="objMeshContainer">Clase ObjMaterialLader</param>
        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            var f = CheckFaceFormatCorrect(line);
            var arrayVertex1 = CheckTriangleFormatCorrect(f[0]);
            var arrayVertex2 = CheckTriangleFormatCorrect(f[1]);
            var arrayVertex3 = CheckTriangleFormatCorrect(f[2]);

            var face = new FaceTriangle(arrayVertex1[Vertex], arrayVertex2[Vertex], arrayVertex3[Vertex]);

            if (arrayVertex1.Length + arrayVertex2.Length + arrayVertex3.Length == CompleteArray)
            {
                face.SetTexturesValues(arrayVertex1[Texture], arrayVertex2[Texture], arrayVertex3[Texture]);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(arrayVertex1[Texture]))
                    face.SetTexturesValues(arrayVertex1[Texture], arrayVertex2[Texture], arrayVertex3[Texture]);
                face.SetNormalValues(arrayVertex1[Normal], arrayVertex2[Normal], arrayVertex3[Normal]);
            }
            face.Usemtl = objMeshContainer.ListObjMesh.Last().Usemtl.Count > 0 ? objMeshContainer.ListObjMesh.Last().Usemtl.Last() : null;

            objMeshContainer.ListObjMesh.Last().FaceTriangles.Add(face);
        }

        /// <summary>
        ///    Checkea que la se pueda parsear la face ("f")
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private string[] CheckTriangleFormatCorrect(string f)
        {
            var arrayVertex = f.Split(TypeVertexDelimiter);
            if (arrayVertex.Length == 4) throw new ArgumentException("Formato no soportado, cantidad de vertices: 4 ");
            if (arrayVertex.Length > 4 || arrayVertex.Length == 0)
                throw new ArgumentException($"Cantidad de argumentos invalidos, se esperaban entre 1 y 3 y se obtuvieron: {arrayVertex.Length}");
            return arrayVertex;
        }

        /// <summary>
        ///    Checkea que la informacion de la linea sea correcta para procesarla
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <returns>string</returns>
        private string[] CheckFaceFormatCorrect(string line)
        {
            var face = line.Remove(0, 2).Split(VertexDelimiter);
            if (face.Length == 4) throw new ArgumentException("Formato no soportado, Se esperaba exportado en forma triangular");
            if (face.Length > 4 || face.Length == 0)
                throw new ArgumentException("Cantidad de argumentos invalidos");
            return face;
        }
    }
}