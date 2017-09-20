using System.Windows;
using ImageFunctions;
using System.IO;
using System;
using System.Text;

namespace Wave_Lab
{

    public class ImageInfo 
    {
        ImageHandler imageHandler;

        #region Recuperer les information de l'image dans un certin format
        public String ImageInformation(ImageHandler imageHandler)
        {
            StringBuilder info = new StringBuilder();
            this.imageHandler = imageHandler;
            FileInfo fileInfo = new FileInfo(imageHandler.CurrentBitmapPath);
            info = info.Append("Nom de l'image          ").Append((char)10).Append(fileInfo.Name.Replace(fileInfo.Extension, "")).Append((char)10).Append((char)10);
            info = info.Append("Extension de l'image        ").Append((char)10).Append(fileInfo.Extension).Append((char)10).Append((char)10);
            string loc = fileInfo.DirectoryName;
            if (loc.Length > 50)
                loc = loc.Substring(0, 15) + "..." + loc.Substring(loc.LastIndexOf("\\"));
            info = info.Append("Emplacement de l'image       ").Append((char)10).Append(loc).Append((char)10).Append((char)10);
            info = info.Append("Dimension de l'image         ").Append((char)10).Append(imageHandler.CurrentBitmap.Width + " x " + imageHandler.CurrentBitmap.Height).Append((char)10).Append((char)10);
            info = info.Append("Taille de l'image            ").Append((char)10).Append((fileInfo.Length / 1024.0).ToString("0.0") + " KB").Append((char)10).Append((char)10);
            info = info.Append("Date de création             ").Append((char)10).Append(fileInfo.CreationTime.ToString("dddd dd MMMM yyyy")).Append((char)10).Append((char)10);

            return info.ToString();
        }
        #endregion
    }
}
