using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Tag
{
    using Core.Domain.Blog;
    public class TagService : ITagService
    {

        private readonly IRepository<Keyword> _keywordRepository;
        private readonly IRepository<Blog> _blogRepository;
        private IUnitOfWork _unitOfWork;

        #region Ctor
        public TagService
            (
            IRepository<Keyword> keywordRepository,
            IUnitOfWork unitOfWork,
            IRepository<Blog> blogRepository
            )
        {
            _keywordRepository = keywordRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        /// <summary>
        /// Get all keywords
        /// </summary>
        /// <returns>Keywords</returns>
        public List<Keyword> GetAllKeywords()
        {
            var keywordList = new List<Keyword>();

            var keywordsCollection = from blog in _blogRepository.Table
                                     where blog.IsApproved.Value && blog.IsDeleted != true
                                     select blog.Keywords.Select(a => a);

            foreach (var keywords in keywordsCollection)
            {
                keywordList.AddRange(keywords);
            }

            return keywordList.Distinct().ToList();
        }

        public Keyword GetSpecificKeyword(Keyword keyword)
        {
            var keywordReturn = (from keywordObj in _keywordRepository.Table
                where keywordObj.ID == keyword.ID
                select keyword).SingleOrDefault();

            return keywordReturn;
        }

        public bool AddNewTag(Keyword keyword)
        {
            var isSuccess = _keywordRepository.Insert(keyword);
            return isSuccess;
        }

        public bool UpdateTag(Keyword keyword)
        {
            var isSuccess = _keywordRepository.Update(keyword);
            return isSuccess;
        }

        public bool DeleteTag(Keyword keyword)
        {
            var isSuccess = _keywordRepository.Delete(keyword);
            return isSuccess;
        }
    }
}
