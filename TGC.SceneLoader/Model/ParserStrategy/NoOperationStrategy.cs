using System.Linq;

namespace TGC.SceneLoader.Model.ParserStrategy
{
    public class NoOperationStrategy : ObjParseStrategy
    {
        private readonly string[] _keyWords;

        public NoOperationStrategy()
        {
            _keyWords = new[] { COMMENT, EMPTYLINE, WHITELINE, MTLLIB };
        }

        public override void ProccesLine(string line, ObjMeshContainer objMeshContainer)
        {
            //There is no need to perform any operation.
        }

        public override bool ResponseTo(string action)
        {
            return _keyWords.Contains(action);
        }
    }
}