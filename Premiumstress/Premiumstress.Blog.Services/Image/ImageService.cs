using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Image
{
    using System.Drawing;
    using Core.Domain;
    public class ImageService : IImageService
    {
        private readonly IRepository<Imagelink> _imageLinkRepository;
        private readonly IRepository<User> _userRepository;

        public ImageService(IRepository<Imagelink> imageLinkRepository, IRepository<User> userRepository)
        {
            _imageLinkRepository = imageLinkRepository;
            _userRepository = userRepository;
        }

        public object UploadImage(HttpRequestBase request, string type)
        {
            var imageFileLink = string.Empty;
            var thumbNailLink = string.Empty;

            foreach (var pathToImage in request.Form.Cast<string>()
                .Select(filePath => request.Form[filePath])
                .Select(imagePath => imagePath.Replace("/", "\\"))
                .Select(imagePath => imagePath.Substring(1))
                .Select(imagePath => AppDomain.CurrentDomain.BaseDirectory + imagePath)
                .Where(File.Exists))
            {
                File.Delete(pathToImage);
            }

            var destinationOfImage = string.Empty;

            switch (type)
            {
                case "blog":
                    destinationOfImage = "BlogImages";
                    break;
                case "user":
                    destinationOfImage = "UserPictures";
                    break;
                case "shortfilm":
                    destinationOfImage = "ShortFilmImages";
                    break;
            }

            foreach (string file in request.Files)
            {
                var hpf = request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                var blogImagePath = "Images\\" + destinationOfImage + "\\";
                var imagePath = AppDomain.CurrentDomain.BaseDirectory + blogImagePath;
                var fileName = Guid.NewGuid() + Path.GetExtension(Path.GetFileName(hpf.FileName));
                var savedFileName = Path.Combine(imagePath, fileName);

                hpf.SaveAs(savedFileName);


                //Save thumbnail
                var img = Image.FromFile(savedFileName);
                byte[] arr;
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }
                var stream = new MemoryStream(arr);
                ResizeImage(250, 250, stream, imagePath + "Thumbnails\\" + fileName);

                blogImagePath = "/Images/" + destinationOfImage + "/";
                imageFileLink = blogImagePath + fileName;
                thumbNailLink = blogImagePath + "Thumbnails/" + fileName;
            }

            var pictureLinks = new
            {
                FullImageLink = imageFileLink,
                ThumbnailImageLink = thumbNailLink
            };


            return pictureLinks;
        }

        public static void ResizeImage(int newWidth, int newHeight, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var imgPhoto = Image.FromStream(sourcePath);

                var sourceWidth = imgPhoto.Width;
                var sourceHeight = imgPhoto.Height;

                if (sourceWidth > newWidth || sourceHeight > newHeight)
                {
                    //Consider vertical pics
                    if (sourceWidth < sourceHeight)
                    {
                        int buff = newWidth;

                        newWidth = newHeight;
                        newHeight = buff;
                    }

                    int destX = 0, destY = 0;
                    float nPercent = 0, nPercentW = 0, nPercentH = 0;

                    nPercentW = ((float)newWidth / (float)sourceWidth);
                    nPercentH = ((float)newHeight / (float)sourceHeight);
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentH;
                        destX = System.Convert.ToInt16((newWidth -
                              (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentW;
                        destY = System.Convert.ToInt16((newHeight -
                              (sourceHeight * nPercent)) / 2);
                    }
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        public Imagelink GetImagesOfUserById(int id)
        {
            var query = _imageLinkRepository.Table;
            var imageOfUser = query.FirstOrDefault(a => a.ID == id);
            return imageOfUser;
        }

        public Imagelink GetImagesOfBlogById(int id)
        {
            var query = _imageLinkRepository.Table;
            var imageOfBlog = query.SingleOrDefault(a => a.Blogs.Any(b => b.ID == id));
            return imageOfBlog;
        }

        public bool UpdateBlogImageLink(Imagelink updated, int blogId)
        {
            if (updated == null) return false;

            var query = _imageLinkRepository.Table;
            var original = query.SingleOrDefault(a => a.Blogs.Any(b => b.ID == blogId));

            return UpdateImage(original, updated,blogId);
        }

        public bool UpdateUserImage(Imagelink updated, int userId)
        {
            if (updated == null) return false;

            var query = _imageLinkRepository.Table;
            var original = query.SingleOrDefault(a => a.Users.Any(b => b.ID == userId));

            return UpdateImage(original, updated,userId);
        }

        private bool UpdateImage(Imagelink original, Imagelink updated,int id)
        {
            //If original image is not existing 
            //Create a new one.
            if(original == null)
            {
                var user = (from userObj in _userRepository.Table
                           where userObj.ID == id
                           select userObj).SingleOrDefault();

                if (user == null) return false;

                var newImageLink = new Imagelink()
                {
                    FullImageLink = updated.FullImageLink,
                    ThumbnailImageLink = updated.ThumbnailImageLink
                };

                user.Imagelinks = new[] { newImageLink };

                return _userRepository.Update(user);
            }

            original.FullImageLink = updated.FullImageLink;
            original.ThumbnailImageLink = updated.ThumbnailImageLink;

            return _imageLinkRepository.Update(original);
        }
    }
}
