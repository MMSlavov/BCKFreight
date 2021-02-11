namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// This class is used to compress the image to
    /// provided size.
    /// </summary>
    public class ImageCompress
    {
        private static volatile ImageCompress imageCompress;
        private Bitmap bitmap;
        private int width;
        private int height;
        private Image img;

        /// <summary>
        /// It is used to restrict to create the instance of the      ImageCompress.
        /// </summary>
        private ImageCompress()
        {
        }

        /// <summary>
        /// Gets ImageCompress object.
        /// </summary>
        public static ImageCompress GetImageCompressObject
        {
            get
            {
                if (imageCompress == null)
                {
                    imageCompress = new ImageCompress();
                }

                return imageCompress;
            }
        }

        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Gets or sets Image.
        /// </summary>
        public Bitmap GetImage
        {
            get { return this.bitmap; }
            set { this.bitmap = value; }
        }

        /// <summary>
        /// This function is used to save the image.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        public void Save(string fileName, string path)
        {
            if (this.ISValidFileType(fileName))
            {
                string pathaname = path + @"\" + fileName;
                this.save(pathaname, 60);
            }
        }

        /// <summary>
        /// This function is use to compress the image to
        /// predefine size.
        /// </summary>
        /// <returns>return bitmap in compress size.</returns>
        private Image CompressImage()
        {
            if (this.GetImage != null)
            {
                this.Width = (this.Width == 0) ? this.GetImage.Width : this.Width;
                this.Height = (this.Height == 0) ? this.GetImage.Height : this.Height;
                Bitmap newBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
                newBitmap = this.bitmap;
                newBitmap.SetResolution(80, 80);
                return newBitmap.GetThumbnailImage(this.Width, this.Height, null, IntPtr.Zero);
            }
            else
            {
                throw new Exception("Please provide bitmap");
            }
        }

        /// <summary>
        /// This function is used to check the file Type.
        /// </summary>
        /// <param name="fileName">String data type:contain the file name.</param>
        /// <returns>true or false on the file extention.</returns>
        private bool ISValidFileType(string fileName)
        {
            bool isValidExt = false;
            string fileExt = Path.GetExtension(fileName);
            // switch (fileExt.ToLower())
            // {
            //    case CommonConstant.JPEG:
            //    case CommonConstant.BTM:
            //    case CommonConstant.JPG:
            //    case CommonConstant.PNG:
            //        isValidExt = true;
            //        break;
            // }

            return isValidExt;
        }

        /// <summary>
        /// This function is used to get the imageCode info
        /// on the basis of mimeType.
        /// </summary>
        /// <param name="mimeType">string data type.</param>
        /// <returns>ImageCodecInfo data type.</returns>
        private ImageCodecInfo GetImageCoeInfo(string mimeType)
        {
            ImageCodecInfo[] codes = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i].MimeType == mimeType)
                {
                    return codes[i];
                }
            }

            return null;
        }

        /// <summary>
        /// this function is used to save the image into a
        /// given path.
        /// </summary>
        /// <param name="path">string data type.</param>
        /// <param name="quality">int data type.</param>
        private void save(string path, int quality)
        {
            this.img = this.CompressImage();
            ////Setting the quality of the picture
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ////Seting the format to save
            ImageCodecInfo imageCodec = this.GetImageCoeInfo("image/jpeg");
            ////Used to contain the poarameters of the quality
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = qualityParam;
            ////Used to save the image to a  given path
            this.img.Save(path, imageCodec, parameters);
        }
    }
}
