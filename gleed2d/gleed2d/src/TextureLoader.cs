using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GLEED2D
{
    class TextureLoader
    {

        private static TextureLoader instance;
        public static TextureLoader Instance
        {
            get
            {
                if (instance == null) instance = new TextureLoader();
                return instance;
            }
        }

        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();



        public Texture2D FromFile(GraphicsDevice gd, string filename)
        {
            
            if (!textures.ContainsKey(filename))
            {
                //Copy it to local dir and then open!
                string workingDir = Constants.Instance.DefaultContentRootFolder + "\\Working";
                string baseFileName = System.IO.Path.GetFileName(filename);
                if (!System.IO.Directory.Exists(workingDir))
                    System.IO.Directory.CreateDirectory(workingDir);
                System.IO.File.Copy(filename,  workingDir + "\\" + baseFileName,true);

                //Constants.Instance.DefaultContentRootFolder
                System.IO.Stream inStream = System.IO.File.Open(workingDir + "\\" + baseFileName, System.IO.FileMode.Open);
                textures[filename] = Texture2D.FromStream(gd,inStream);
            }
            return textures[filename];
        }

        public void Clear()
        {
            textures.Clear();
        }

    }
}
