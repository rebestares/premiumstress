using Premiumstress.Core.Domain.Blog;

namespace Premiumstress.Blog.Services.Video
{
    public interface IVideoService
    {
        bool UpdateVideoLink(BlogVideo video, int blogId);
    }
}
