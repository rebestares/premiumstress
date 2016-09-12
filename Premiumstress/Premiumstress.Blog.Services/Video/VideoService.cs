using System.Linq;
using Premiumstress.Core.Domain.Blog;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Video
{
    public class VideoService : IVideoService
    {
        private readonly IRepository<BlogVideo> _blogVideoRepository;

        public VideoService(IRepository<BlogVideo> blogVideoRepository)
        {
            _blogVideoRepository = blogVideoRepository;
        }

        public bool UpdateVideoLink(BlogVideo blog, int blogId)
        {
            var query = _blogVideoRepository.Table;
            var videoLink = query.SingleOrDefault(a => a.BlogID == blogId);

            if (videoLink == null)
            {
                var blogVideo = new BlogVideo()
                {
                    
                    VideoLink = blog.VideoLink,
                    BlogID = blogId
                };
                return _blogVideoRepository.Insert(blogVideo);
            };

            videoLink.VideoLink = blog.VideoLink;
            return _blogVideoRepository.Update(videoLink);
        }
    }
}
