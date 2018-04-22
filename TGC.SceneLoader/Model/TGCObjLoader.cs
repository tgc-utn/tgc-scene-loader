using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TGC.Core.SceneLoader;
using TGC.SceneLoader.Model.ParserStrategy;

namespace TGC.SceneLoader.Model
{
    public class TGCObjLoader
    {
        private const string MTLLIB = "mtllib";
        private const int INDEXATTR = 7;
        private const int ELEMENTOFMTLLIB = 2;
        private const int LIMITVERTEX = 21844;
        private const int INICIO = 0;

        public List<ObjParseStrategy> Strategies { get; set; } = new List<ObjParseStrategy>();
        public ObjMeshContainer ObjMeshContainer { get; set; } = new ObjMeshContainer();
        public ObjMaterialsLoader ObjMaterialsLoader { get; set; } = new ObjMaterialsLoader();
        public List<string> ListMtllib { get; set; } = new List<string>();
        public MeshBuilder MeshBuilder { get; set; } = new MeshBuilder();

        public TGCObjLoader()
        {
            Strategies.Add(new AddShadowStrategy());
            Strategies.Add(new AddUsemtlStrategy());
            Strategies.Add(new CreateNewMeshStrategy());
            Strategies.Add(new CreateNormalStrategy());
            Strategies.Add(new CreateFaceStrategy());
            Strategies.Add(new CreateTextCoordStrategy());
            Strategies.Add(new CreateVertexStrategy());
            Strategies.Add(new NoOperationStrategy());
        }

        public string GetPathObjforCurrentDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), @"UnitTestProjectObj\Resources\cubo.obj");
        }

        public void LoadObjFromFile(string path)
        {
            //Se leen todas las lineas
            var lines = File.ReadAllLines(path);
            //Se recolectan los materiales
            GetListOfMaterials(lines, path);
            //Se Parsea de los objetos
            foreach (var line in lines)
                ProccesLine(line);
            //se chekea que el objeto parseado funcione en el frmawork
            CheckParsedObj();
        }

        private void CheckParsedObj()
        {
            ObjMeshContainer.ListObjMesh.ForEach(
                (mesh) =>
                {
                    CheckMeshLimitVertex(mesh);
                    ChekMaterialsHaveTexture(ObjMaterialsLoader.ListObjMaterialMesh);
                });
        }

        private void ChekMaterialsHaveTexture(List<ObjMaterialMesh> listObjMaterialMesh)
        {
            if (listObjMaterialMesh.Any((objMaterialMesh) => objMaterialMesh.map_d == null))
                throw new ArgumentException("Algunos de los materiales no posee textura. TGC todavía no soporta materiales sin textura. ");
        }

        private void CheckMeshLimitVertex(ObjMesh mesh)
        {
            if (mesh.FaceTriangles.Count > LIMITVERTEX)
                throw new ArgumentOutOfRangeException($"FaceTriangles.Count, El límite actual para la creación de mesh es de: {LIMITVERTEX} y un objeto de su archivo posee: {mesh.FaceTriangles.Count}");
        }

        public void ProccesLine(string line)
        {
            var action = line.Split(' ').FirstOrDefault();
            if (action == null) throw new InvalidOperationException($"Cannot find action for this line {line}");

            foreach (var strategy in Strategies)
                if (strategy.ResponseTo(action))
                {
                    strategy.ProccesLine(line, ObjMeshContainer);
                    return;
                }
            throw new InvalidOperationException($"Cannot find a correct parsing process for line {line}");
        }

        public void GetListOfMaterials(string[] lines, string path)
        {
            //se obtiene puramente todas las lineas que son de material
            var linesFiltered = FilterByKeyword(lines, MTLLIB);
            //nos fijamos si tiene material
            if (linesFiltered.Length == 0) return;
            //agregamos la ruta de la carpeta donde se encuentran los materiales
            ObjMaterialsLoader.SetDirectoryPathMaterial(path);
            //
            foreach (var line in linesFiltered)
                SetMtllib(line);
            //Se hace parse de los materiales
            ObjMaterialsLoader.LoadMaterialsFromFiles(path, ListMtllib);  //TODO ver si devuelve una lista de materiales o le pasamos el objmesh como parametro
                                                                          // MeshBuilder.AddMaterials(ObjMaterialsLoader);
        }

        private void SetMtllib(string line)
        {
            string[] splitLine = line.Split(' ');
            if (splitLine.Length < ELEMENTOFMTLLIB) throw new ArgumentException("El atributo Mtllib tiene formato incorrecto");
            var attribute = line.Substring(INDEXATTR);
            if (!Path.GetExtension(attribute).Equals(".mtl"))
            {
                throw new ArgumentException("La extención de Mtllib es incorrecta, se esperaba: .mtl y se obtuvo: " + Path.GetExtension(attribute));
            }
            ListMtllib.Add(attribute);
        }

        public string[] FilterByKeyword(string[] lines, string keyWord)
        {
            List<string> linesWithKeyword = new List<string>();

            foreach (string line in lines)
            {
                if (line.Split(' ').FirstOrDefault().Equals(keyWord))
                    linesWithKeyword.Add(line);
            }
            return linesWithKeyword.ToArray();
        }

        public TgcMesh LoadTgcMeshFromObj(string fullobjpath, int index)
        {
            LoadObjFromFile(fullobjpath);
            ObjMesh objMesh = ObjMeshContainer.ListObjMesh[index];
            if (objMesh.Usemtl.Count > 0)
            {
                MeshBuilder
                    .AddMaterials(ObjMaterialsLoader)
                    .ChargueMaterials()
                    .AddDxMesh(objMesh.FaceTriangles.Count)
                    .ChargeBuffer(ObjMeshContainer, index)
                    .ChargeAttributeBuffer(objMesh.CreateMaterialIdsArray())
                    .SetEnable(true)
                    .AddAutotransform(true)
                    .SetHasBoundingBox(false);
            }
            else
            {
                MeshBuilder.AddDxMesh(objMesh.FaceTriangles.Count)
                    .ChargeBuffer(ObjMeshContainer, index)
                    .SetEnable(true)
                    .AddAutotransform(true)
                    .SetHasBoundingBox(false);
            }

            return MeshBuilder.Build(ObjMeshContainer.ListObjMesh[index]); //TODO pasarle el name no mas
        }
    }
}