using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.SceneLoader.Model.CreateBufferStrategy
{
    public abstract class ChargueBufferStrategy
    {
        public abstract VertexElement[] VertexElementInstance { get; set; }

        public TgcMesh.MeshRenderType RenderType;

        /// <summary>
        ///    Carga el buffer del mesh de DirectX
        /// </summary>
        /// <param name="objMesh">ObjMesh</param>
        /// <param name="dxMesh"></param>
        /// <param name="index"></param>
        /// <returns>MeshBuilder</returns>
        public abstract void ChargeBuffer(ObjMeshContainer objMesh, Mesh dxMesh, int index);

        /// <summary>
        ///   Cargar indexBuffer del mesh de DirectX en forma plana
        /// </summary>
        /// <param name="objMesh">ObjMesh</param>
        /// /// <param name="dxMesh"></param>
        public void ChargeIndexBuffer(ObjMesh objMesh, Mesh dxMesh)
        {
            using (var ib = dxMesh.IndexBuffer)
            {
                var indices = new short[objMesh.FaceTriangles.Count * 3];
                for (var i = 0; i < indices.Length; i++)
                {
                    indices[i] = (short)i;
                }
                ib.SetData(indices, 0, LockFlags.None);
            }
        }
    }
}