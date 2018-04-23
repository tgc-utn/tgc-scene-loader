using System.Collections.Generic;
using System.Linq;

namespace TGC.SceneLoader.Model
{
    public class ObjMesh
    {
        public string Name { get; set; }
        public bool Shadow { get; set; } = false;
        public List<string> Usemtl { get; set; } = new List<string>();
        public List<FaceTriangle> FaceTriangles { get; set; } = new List<FaceTriangle>();

        public ObjMesh(string name)
        {
            Name = name;
        }

        public int[] CreateMaterialIdsArray()
        {
            int[] materialsId = new int[FaceTriangles.Count];
            var numMaterial = 0;
            var index = 0;
            var usemtl = Usemtl.First();
            FaceTriangles.ForEach((face) =>
            {
                if (!usemtl.Equals(face.Usemtl))
                {
                    numMaterial++;
                    usemtl = face.Usemtl;
                }
                materialsId[index] = numMaterial;
                index++;
            });
            return materialsId;
        }
    }
}