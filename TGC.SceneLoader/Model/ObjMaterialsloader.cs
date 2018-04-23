using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TGC.SceneLoader.Model.ParseMaterialsStrategy;

namespace TGC.SceneLoader.Model
{
    public class ObjMaterialsLoader
    {
        private const string Separador = "\\";

        public List<ObjMaterialsParseStrategy> Strategies { get; set; } = new List<ObjMaterialsParseStrategy>();
        public List<ObjMaterialMesh> ListObjMaterialMesh { get; set; } = new List<ObjMaterialMesh>();
        public string CurrentDirectory { get; set; }

        public ObjMaterialsLoader()
        {
            Strategies.Add(new CreateNewMaterialStrategy());
            Strategies.Add(new ParseMaterialAndColorStrategy());
            Strategies.Add(new NoOperationStrategyForMaterial());
        }

        public void LoadMaterialsFromFiles(string pathMtllib, List<string> listMtllib)
        {
            foreach (string mtllib in listMtllib)
            {
                string pathMaterial = Path.GetDirectoryName(pathMtllib) + Separador + mtllib;
                if (File.Exists(pathMaterial))
                {
                    ParseMtlLib(pathMaterial);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot find file: {mtllib}");
                }
            }
        }

        public string MaterialPath(string mtllib)
        {
            return CurrentDirectory + Separador + mtllib;
        }

        private void ParseMtlLib(string path)
        {
            //Se leen todas las lineas
            var lines = File.ReadAllLines(path);

            //Parse de las sentencias
            foreach (var line in lines)
                ProccesLine(line);
        }

        private void ProccesLine(string line)
        {
            if (line == null)
                throw new InvalidOperationException("The line is null.");

            var action = line.Split(' ').FirstOrDefault();

            if (action == null)
                throw new InvalidOperationException($"Cannot find action for this line {line}");

            action = action.Trim();

            foreach (var strategy in Strategies)
                if (strategy.ResponseTo(action))
                {
                    strategy.ProccesLine(line, ListObjMaterialMesh);
                    return;
                }
            throw new InvalidOperationException($"Cannot find a correct parsing process for line {line}");
        }

        public void SetDirectoryPathMaterial(string path)
        {
            CurrentDirectory = Path.GetDirectoryName(path);
        }
    }
}