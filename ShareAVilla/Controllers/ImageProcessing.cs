using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Controllers
{
    public class ImageProcessing
    {
        public static Models.Picture ReadPicture(HttpPostedFileBase upload)
        {

            if (upload == null)
            {
                return null;

            }
            var avatar = new Models.Picture
            {

                FileName = System.IO.Path.GetFileName(upload.FileName),
                PictureType = Models.PictureType.Avatar,
                ContentType = upload.ContentType
            };
            HttpPostedFileBase uploadforMini = upload;
            //make small
            upload.InputStream.Seek(0, 0);
            using (var stream = new System.IO.MemoryStream())
            {
                ImageResizer.ImageJob image = new ImageResizer.ImageJob(upload, stream, new ImageResizer.Instructions("width=200;height=200;crop=auto;format=jpg;anchor=middlecenter;autorotate=true;quality=100"));

                image.Build();

                System.Drawing.Image Image = System.Drawing.Image.FromStream(stream);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                avatar.Content = ms.ToArray();
                //make smaller

                uploadforMini.InputStream.Seek(0, 0);
                using (var streamMini = new System.IO.MemoryStream())
                {
                    ImageResizer.ImageJob imageMini = new ImageResizer.ImageJob(uploadforMini, streamMini, new ImageResizer.Instructions("width=50;height=50;crop=auto;format=jpg;anchor=middlecenter;autorotate=true;quality=100"));

                    imageMini.Build();

                    System.Drawing.Image ImageMini = System.Drawing.Image.FromStream(streamMini);
                    System.IO.MemoryStream msMini = new System.IO.MemoryStream();

                    ImageMini.Save(msMini, System.Drawing.Imaging.ImageFormat.Jpeg);
                    avatar.ContentMini = msMini.ToArray();

                    return avatar;
                    //}
                }
            }
            //return null;

        }

        //public static Models.Picture GetPicture(HttpPostedFileBase upload)
        //{

        //    var avatar = new Models.Picture
        //    {
        //        FileName = System.IO.Path.GetFileName(upload.FileName),
        //        PictureType = Models.PictureType.Avatar,
        //        ContentType = upload.ContentType
        //    };
        //    avatar.ContentMini=
        //}

        public static Models.Picture ReadPicture(string filename)
        {

            var avatar = new Models.Picture
            {
                FileName = filename,
                PictureType = Models.PictureType.Avatar,
                ContentType = "NoPic",    

            };
            System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
            using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                avatar.ContentMini = mStream.ToArray();
                avatar.Content = mStream.ToArray();
                return avatar;
            }

            
            
        }


        public byte[] ProcessPicture(HttpPostedFileBase upload)
        {
            

                if (upload == null) { return null; }
                
                
                upload.InputStream.Seek(0, 0);
                System.IO.Stream stream = new System.IO.MemoryStream();
                ImageResizer.ImageJob image = new ImageResizer.ImageJob(upload, stream, new ImageResizer.Instructions("width=50;height=50;crop=auto;format=jpg;anchor=middlecenter;autorotate=true;quality=100"));

                image.Build();



                System.Drawing.Image Image = System.Drawing.Image.FromStream(stream);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();

            }
    }
}