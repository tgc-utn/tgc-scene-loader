using Microsoft.DirectX.Direct3D;
using System.IO;

namespace TGC.SceneLoader.Model
{
    public class ObjMaterialMesh
    {
        //Material Name
        public string Name { get; set; }

        //Material color & ilumination
        public ColorValue Ka { get; set; }

        public ColorValue Kd { get; set; }
        public ColorValue Ks { get; set; }
        public ColorValue Ke { get; set; }
        public ColorValue Tf { get; set; }

        /*
         ILUM REFENCE NUMBERS
            0 Color on and Ambient off
            1 Color on and Ambient on
            2 Highlight on
            3 Reflection on and Ray trace on
            4 Transparency: Glass on
              Reflection: Ray trace on
            5 Reflection: Fresnel on and Ray trace on
            6 Transparency: Refraction on
              Reflection: Fresnel off and Ray trace on
            7 Transparency: Refraction on
              Reflection: Fresnel on and Ray trace on
            8 Reflection on and Ray trace off
            9 Transparency: Glass on
              Reflection: Ray trace off
           10 Casts shadows onto invisible surfaces
        */
        public int illum { get; set; }
        public float d { get; set; }
        public float Ns { get; set; }
        public int Sharpness { get; set; }
        public float Ni { get; set; }
        public float Tr { get; set; }

        //Texture mas statament
        //TODO prodia ser una estructura de dato que represente la sentencia de textura
        public string map_Kd { get; set; }

        public string disp { get; set; }
        public string map_Bump { get; set; }
        public string map_Ka { get; set; }
        public string map_Ks { get; set; }
        public string map_d { get; set; }

        //Reflection map statament

        public ObjMaterialMesh(string name)
        {
            Name = name;
        }

        //TODO solucionar el problema cuando el nombre del archivo viene con espacios
        public string GetTextura()
        {
            return File.Exists(map_d) ? Path.GetFullPath(map_d) : null; // prodia ser cualquiera, TODO indentificar bien cual usar
        }

        public string GetTexturaFileName()
        {
            return Path.GetFileName(map_d).TrimStart().TrimEnd();
        }
    }
}