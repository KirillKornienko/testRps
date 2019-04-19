using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageDraw = System.Drawing;

namespace GameWPF.GameLogic
{
    class TextureController
    {
        Dictionary<SurfaceTypes, ImageDraw.Image> SurfaceTextures;


        //TODO: Текстуры могут не грузиться, 
        private bool LoadTextures(SurfaceTypes[] texture_codes)
        {
            try
            {
                SurfaceTextures = new Dictionary<SurfaceTypes, ImageDraw.Image>();

                foreach (var texture_code in texture_codes)
                {
                    //TODO: Необходимо узнать filepath по SurfaceType
                    /*
                    SurfaceTextures.Add(texture_code, 
                                ImageDraw.Image.FromFile(
                                                        Path.Combine(Settings.Default.SPRITES_DIRECTORY_NAME, Decoding.GetTextureName(texture_code.ToString()))));
                    */
                }

                return true;
            }
            catch (FileNotFoundException e)
            {
                //MessageBox.Show("Texture file " + e.Message + " not found.");
                return false;
            }
        }

    }
}
