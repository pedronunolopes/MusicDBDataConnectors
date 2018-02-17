using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Saves a byte array into an image on disk
        /// </summary>
        /// <param name="image"></param>
        public static void SaveImage(byte[] image, string strImagePath)
        {
            MemoryStream ms = new MemoryStream(image);

            using (Image imageFile = Image.FromStream(ms))
            {
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] jpegPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg",
                            Path.GetDirectoryName(strImagePath),
                            Path.GetFileNameWithoutExtension(strImagePath),
                            frame);
                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                    }
                }
            }
        }

        /// <summary> 
        /// Converts tiff imagae(s) to jpeg image(s). 
        /// </summary> 
        /// <param name="fileName">Full name to tiff image.</param> 
        /// <returns>String array having full name to jpeg images.</returns> 
        public static string[] ConvertImageToJpeg(string fileName)
        {
            using (Image imageFile = Image.FromFile(fileName))
            {
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] jpegPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg",
                            Path.GetDirectoryName(fileName),
                            Path.GetFileNameWithoutExtension(fileName),
                            frame);
                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                    }
                }

                return jpegPaths;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(System.Drawing.Bitmap imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Bitmap ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return new Bitmap(returnImage);
        }

    }
}