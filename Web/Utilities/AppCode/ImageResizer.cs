using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;


namespace Web.Utilities.AppCode
{

   public static class ImageResizer
   {

       public static bool ResizeAndSave(int newWidthIn, int newHeightIn, string directory, string filename, Stream inputStream)
      {

           // file name sample : 20121025112236cv3d.jpg Or 20121025112236cv3d_s.jpg
           // directory sample : physical address of uploadPath >> Server.MapPath(uploadPath) >> uploadPath sample : "~/up/news/"

        try
        {

            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap originalBMP = new Bitmap(inputStream);

            // Calculate the new image dimensions
            int origWidth = originalBMP.Width;
            int origHeight = originalBMP.Height;
            float sngRatio = 1;
            int newWidth = 0;
            int newHeight = 0;

            if (origWidth > newWidthIn || origHeight > newHeightIn)
            {

                if (origWidth > origHeight)
                {
                    sngRatio = (float)origWidth / (float)origHeight;
                    newWidth = newWidthIn;
                    newHeight = Convert.ToInt32(newWidthIn / sngRatio);
                }
                else if (origWidth < origHeight)
                {
                    sngRatio = (float)origHeight / (float)origWidth;
                    newHeight = newHeightIn;
                    newWidth = Convert.ToInt32(newHeightIn / sngRatio);
                }
                else
                {
                    if (origWidth > newWidthIn)
                    {
                        sngRatio = (float)origWidth / (float)newWidthIn;
                        newWidth = newWidthIn;
                        newHeight = Convert.ToInt32(origHeight / sngRatio);
                    }
                    else if (origHeight > newHeightIn)
                    {
                        sngRatio = (float)origHeight / (float)newHeightIn;
                        newHeight = newHeightIn;
                        newWidth = Convert.ToInt32(origWidth / sngRatio);
                    }
                }

                // Create a new bitmap which will hold the previous resized bitmap
                Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);

                // Create a graphic based on the new bitmap
                Graphics G = Graphics.FromImage(newBMP);
                // Set the properties for the new graphic file
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Draw the new graphic based on the resized bitmap
                G.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                // >------ Save resize Image ------------- //
                newBMP.Save(directory + filename);

                // Once finished with the bitmap objects, we deallocate them.
                originalBMP.Dispose();
                newBMP.Dispose();
                G.Dispose();

                return true;
            }
            else
            {

                originalBMP.Save(directory + filename);

               return true;
            }

        }
        catch (Exception)
        {
           return false;
        }

      }

   }

}