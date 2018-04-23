using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.IO;
using TGC.Core.BoundingVolumes;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.MeshFactory;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.SceneLoader.Model.CreateBufferStrategy;

namespace TGC.SceneLoader.Model
{
    public class MeshBuilder
    {
        //Constante
        private readonly string SEPARADOR = "\\";

        //Variables
        public Mesh DxMesh { get; set; }

        public IMeshFactory MeshFactory { get; set; } = new DefaultMeshFactory();
        public ChargueBufferStrategy ChargueBufferStrategy { get; set; } = new ChargueBufferColorSoloStrategy();
        public Material[] MeshMaterials { get; set; }
        private TgcTexture[] MeshTextures { get; set; }
        private bool AutoTransform { get; set; }
        private bool Enable { get; set; }
        private bool HasBoundingBox { get; set; }
        private List<TgcObjMaterialAux> MaterialsArray { get; set; }
        public Device Device { get; set; }

        /// <summary>
        ///     Agrega El/Los materiales, cambia el tipo de VertexElement
        /// </summary>
        /// <param name="objMaterialLoader">Clase ObjMaterialLader</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder AddMaterials(ObjMaterialsLoader objMaterialLoader)
        {
            //create material
            // TODO
            MaterialsArray = new List<TgcObjMaterialAux>();
            objMaterialLoader.ListObjMaterialMesh.ForEach(objMaterialMesh =>
                    {
                        MaterialsArray.Add(CreateTextureAndMaterial(objMaterialMesh, objMaterialLoader.CurrentDirectory));
                    }
            );

            //set nueva mesh strategy
            ChargueBufferStrategy = new ChargueBufferDiffuseMapStrategy();  // TODO ver que pasa caundo viene ligthmap
            return this;
        }

        /// <summary>
        ///     Agrega El/Los materiales y luego los hace el set de los atributos
        ///      meshMaterials y meshTextures
        /// </summary>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder ChargueMaterials()
        {
            //Cargar array de Materials y Texturas  TODO separar la creacion del material de la textura
            MeshMaterials = new Material[MaterialsArray.Count];
            MeshTextures = new TgcTexture[GetTextureCount()];     //GetTextureCount()
            var indexTexture = 0;
            MaterialsArray.ForEach((objMaterial) =>
            {
                MeshMaterials[MaterialsArray.IndexOf(objMaterial)] = objMaterial.materialId;

                if (objMaterial.textureFileName != null)
                {
                    MeshTextures[indexTexture] = TgcTexture.createTexture(
                        D3DDevice.Instance.Device,
                        objMaterial.textureFileName,
                        objMaterial.texturePath);
                    indexTexture++;
                }
            });

            return this;
        }

        /// <summary>
        ///     Obtiene la cantidad materiales que poseen textura
        /// </summary>
        /// <returns>int</returns>
        public int GetTextureCount()
        {
            var count = 0;

            MaterialsArray.ForEach((objMaterial) =>
            {
                if (objMaterial.textureFileName != null) count++;
            });

            return count;
        }

        /// <summary>
        ///     Cargar attributeBuffer con los id de las texturas de cada triángulo
        /// </summary>
        /// <param name="materialsIds">int[]</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder ChargeAttributeBuffer(int[] materialsIds)
        {
            var attributeBuffer = DxMesh.LockAttributeBufferArray(LockFlags.None);
            Array.Copy(materialsIds, attributeBuffer, attributeBuffer.Length);
            DxMesh.UnlockAttributeBuffer(attributeBuffer);
            return this;
        }

        /// <summary>
        ///     Crea una instancia del mesh de DirectX
        /// </summary>
        /// <param name="cantFace">int</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder AddDxMesh(int cantFace)
        {
            DxMesh = new Mesh(cantFace, cantFace * 3, MeshFlags.Managed, ChargueBufferStrategy.VertexElementInstance, D3DDevice.Instance.Device);
            return this;
        }

        /// <summary>
        ///     Indica al builder si el mesh posee autotransformacion
        /// </summary>
        /// <param name="flag">boolean</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder AddAutotransform(bool flag)
        {
            AutoTransform = flag;
            return this;
        }

        /// <summary>
        ///     Indica al builder si el mesh esta disponible para modificaciones
        /// </summary>
        /// <param name="flag">boolean</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder SetEnable(bool flag)
        {
            Enable = flag;
            return this;
        }

        /// <summary>
        ///     Indica al builder si se debe crear un bounding box
        ///     en base a los parametros de objMesh, por defecto genera uno stardar
        /// </summary>
        /// <param name="flag">boolean</param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder SetHasBoundingBox(bool flag)
        {
            HasBoundingBox = flag;
            return this;
        }

        /// <summary>
        ///    Carga el buffer del mesh de DirectX usando la estrategia correcta para s estructura
        /// </summary>
        /// <param name="objMeshContainer"></param>
        /// <param name="index"></param>
        /// <returns>MeshBuilder</returns>
        public MeshBuilder ChargeBuffer(ObjMeshContainer objMeshContainer, int index)
        {
            ChargueBufferStrategy.ChargeBuffer(objMeshContainer, DxMesh, index);
            return this;
        }

        /// <summary>
        ///   Construye el mesh con los atributos que se le fueron agregando
        /// </summary>
        /// <param name="objMesh">ObjMesh</param>
        /// <returns>MeshBuilder</returns>
        public TgcMesh Build(ObjMesh objMesh)
        {
            TgcMesh unMesh = MeshFactory.createNewMesh(DxMesh, objMesh.Name, ChargueBufferStrategy.RenderType);
            SetBoundingBox(unMesh);
            unMesh.AutoTransform = AutoTransform;
            unMesh.Enabled = Enable;
            unMesh.Materials = MeshMaterials;
            unMesh.DiffuseMaps = MeshTextures;
            return unMesh;
        }

        /// <summary>
        ///   Cargar indexBuffer del mesh de DirectX en forma plana
        /// </summary>
        /// <param name="unMesh"></param>
        private void SetBoundingBox(TgcMesh unMesh)
        {
            //Crear BoundingBox, aprovechar lo que viene del OBJ o crear uno por nuestra cuenta
            if (HasBoundingBox)
            {
                unMesh.BoundingBox = new TgcBoundingAxisAlignBox(
                    new TGCVector3(1, 1, 1),   //Esto es re saraza TODO hay que ver si la info del obj puede calcular los puntos minimos y maximos. o si se pueden agregar al archivo.
                    new TGCVector3(1, 1, 1),
                    unMesh.Position,
                    unMesh.Scale
                );
            }
            else
            {
                unMesh.createBoundingBox();
            }
        }

        /// <summary>
        ///     Estructura auxiliar para cargar SubMaterials y Texturas
        /// </summary>
        internal class TgcObjMaterialAux
        {
            public Material materialId;
            public string textureFileName;
            public string texturePath;
        }

        /// <summary>
        ///     Crea Material y Textura
        /// </summary>
        private TgcObjMaterialAux CreateTextureAndMaterial(ObjMaterialMesh objMaterialMesh, string currentDirectory)
        {
            var matAux = new TgcObjMaterialAux();

            //Crear material
            var material = new Material();
            matAux.materialId = material;
            material.AmbientColor = objMaterialMesh.Ka;
            material.DiffuseColor = objMaterialMesh.Kd;
            material.SpecularColor = objMaterialMesh.Ks;

            //TODO ver que hacer con Ni, con d, con Ns, con Ke.

            //guardar datos de textura
            matAux.texturePath = objMaterialMesh.GetTextura() ?? Path.GetFullPath(currentDirectory + SEPARADOR + objMaterialMesh.GetTexturaFileName());
            matAux.textureFileName = objMaterialMesh.GetTexturaFileName();

            return matAux;
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
    }
}