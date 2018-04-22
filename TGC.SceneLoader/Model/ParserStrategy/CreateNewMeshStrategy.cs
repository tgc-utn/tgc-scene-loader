namespace TGC.SceneLoader.Model.ParserStrategy
{
    /// <summary>
    ///     Estrategia para crear un nuevo objmesh
    /// </summary>
    public class CreateNewMeshStrategy : ObjParseStrategy
    {
        private const int nameObject = 1;

        /// <summary>
        ///     Agrega la constante que indetifica a la clase en el atributo keyword
        /// </summary>
        public CreateNewMeshStrategy()
        {
            Keyword = OBJECT;
        }

        /// <summary>
        ///    Procesa la linea para crear un nuevo ObjMesh
        /// </summary>
        /// <param name="line">Clase ObjMaterialLader</param>
        /// <param name="objMeshContainer">Clase ObjMaterialLader</param>
        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            var split = line.Split(' ');
            objMeshContainer.ListObjMesh.Add(new ObjMesh(split[nameObject]));
        }
    }
}