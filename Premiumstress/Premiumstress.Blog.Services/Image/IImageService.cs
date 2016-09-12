using System.Web;
using Premiumstress.Core.Domain;

namespace Premiumstress.Blog.Services.Image
{
    public interface IImageService
    {
        Imagelink GetImagesOfUserById(int id);
        Imagelink GetImagesOfBlogById(int id);
        object UploadImage(HttpRequestBase request, string type);
        bool UpdateBlogImageLink(Imagelink updated, int blogId);
        bool UpdateUserImage(Imagelink updated, int userId);
    }
}