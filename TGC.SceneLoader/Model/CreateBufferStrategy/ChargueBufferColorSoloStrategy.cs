using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using TGC.Core.SceneLoader;

namespace TGC.SceneLoader.Model.CreateBufferStrategy
{
    internal class ChargueBufferColorSoloStrategy : ChargueBufferStrategy
    {
        public override VertexElement[] VertexElementInstance { get; set; }

        public ChargueBufferColorSoloStrategy()
        {
            VertexElementInstance = VertexColorVertexElements;
            RenderType = TgcMesh.MeshRenderType.VERTEX_COLOR;
        }

        public override void ChargeBuffer(ObjMeshContainer objMeshContainer, Mesh dxMesh, int index)
        {
            var objMesh = objMeshContainer.ListObjMesh[index];
            //Cargar VertexBuffer
            using (var vb = dxMesh.VertexBuffer)
            {
                var data = vb.Lock(0, 0, LockFlags.None);
                var v = new VertexColorVertex();
                objMesh.FaceTriangles.ForEach(face =>
                {
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V1) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn1) - 1];
                    v.Color = -16777047;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V2) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn2) - 1];
                    v.Color = -16777047;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V3) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn3) - 1];
                    v.Color = -16777047;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                });
                vb.Unlock();
            }

            ChargeIndexBuffer(objMesh, dxMesh);
        }

        /// <summary>
        ///     FVF para formato de malla VERTEX_COLOR
        /// </summary>
        public static readonly VertexElement[] VertexColorVertexElements =
        {
            new VertexElement(0, 0, DeclarationType.Float3,
                DeclarationMethod.Default,
                DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3,
                DeclarationMethod.Default,
                DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Color,
                DeclarationMethod.Default,
                DeclarationUsage.Color, 0),
            VertexElement.VertexDeclarationEnd
        };

        /// <summary>
        ///     Estructura de Vertice para formato de malla VERTEX_COLOR
        /// </summary>
        public struct VertexColorVertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public int Color;
        }
    }
}