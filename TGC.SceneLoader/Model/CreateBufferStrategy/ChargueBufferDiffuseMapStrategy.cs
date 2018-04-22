using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using TGC.Core.SceneLoader;

namespace TGC.SceneLoader.Model.CreateBufferStrategy
{
    internal class ChargueBufferDiffuseMapStrategy : ChargueBufferStrategy
    {
        public override VertexElement[] VertexElementInstance { get; set; }

        public ChargueBufferDiffuseMapStrategy()
        {
            VertexElementInstance = DiffuseMapVertexElements;
            RenderType = TgcMesh.MeshRenderType.DIFFUSE_MAP;
        }

        public override void ChargeBuffer(ObjMeshContainer objMeshContainer, Mesh dxMesh, int index)
        {
            var objMesh = objMeshContainer.ListObjMesh[index];
            //Cargar VertexBuffer
            using (var vb = dxMesh.VertexBuffer)
            {
                var data = vb.Lock(0, 0, LockFlags.None);
                var v = new VertexMesh();

                objMesh.FaceTriangles.ForEach(face =>
                {
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V1) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn1) - 1];
                    v.Tu0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt1) - 1].X;
                    v.Tv0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt1) - 1].Y;
                    v.Color = -1;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V2) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn2) - 1];
                    v.Tu0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt2) - 1].X;
                    v.Tv0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt2) - 1].Y;
                    v.Color = -1;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                    v.Position = objMeshContainer.VertexListV[Convert.ToInt32(face.V3) - 1];
                    v.Normal = objMeshContainer.VertexListVn[Convert.ToInt32(face.Vn3) - 1];
                    v.Tu0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt3) - 1].X;
                    v.Tv0 = objMeshContainer.VertexListVt[Convert.ToInt32(face.Vt3) - 1].Y;
                    v.Color = -1;  //TODO que corresponde poner aca con respecto obj Mesh
                    data.Write(v);
                });
                vb.Unlock();
            }
            ChargeIndexBuffer(objMesh, dxMesh);
        }

        //TODO tratar de reutilizar las estructuras de SceneLoader
        /// <summary>
        ///     Estructura de Vertice para formato de malla DIFFUSE_MAP
        /// </summary>
        public struct VertexMesh
        {
            public Vector3 Position;
            public Vector3 Normal;
            public int Color;
            public float Tu0;
            public float Tv0;
        }

        /// <summary>
        ///     FVF para formato de malla  DIFFUSE_MAP
        /// </summary>
        public static readonly VertexElement[] DiffuseMapVertexElements =
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
            new VertexElement(0, 28, DeclarationType.Float2,
                DeclarationMethod.Default,
                DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
    }
}