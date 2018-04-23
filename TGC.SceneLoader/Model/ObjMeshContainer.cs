using Microsoft.DirectX;
using System.Collections.Generic;

namespace TGC.SceneLoader.Model
{
    public class ObjMeshContainer
    {
        public List<Vector3> VertexListV { get; set; } = new List<Vector3>();
        public List<Vector2> VertexListVt { get; set; } = new List<Vector2>();
        public List<Vector3> VertexListVn { get; set; } = new List<Vector3>();
        public List<ObjMesh> ListObjMesh { get; set; } = new List<ObjMesh>();
    }
}