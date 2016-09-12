using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Tag;
using Premiumstress.Core.Domain.Blog;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.Category
{
    using Core.Domain;
    using Core.Domain.Blog;
    [TestClass]
    public class TagServiceTest
    {
        private TagService _tagService;
        private IRepository<Keyword> _keywordRepository;
        private IRepository<Blog> _blogRepository;
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _unitOfWork = new UnitOfWork();
            _blogRepository = new Repository<Blog>(_context);
            _keywordRepository = new Repository<Keyword>(_context);
            _tagService = new TagService(_keywordRepository,_unitOfWork, _blogRepository); 

        }

        [TestMethod]
        public void Query_GetAllKeywords()
        {
            var keywords = _tagService.GetAllKeywords();

            foreach (var keyword in keywords)
            {
                Trace.TraceInformation("All Keywords: {0}, {1}", keyword.Keywords,keyword.ID);
            }

        }

        [TestMethod]
        public void Add_Keyword()
        {
            Keyword keyword = new Keyword() {Keywords = "test keyword"};
            var isSuccess = _tagService.AddNewTag(keyword);
            
            Assert.AreEqual(true,isSuccess);
        }

        [TestMethod]
        public void Update_Keyword()
        {
            var keyword = _keywordRepository.Table.SingleOrDefault(a => a.ID == 2029);
            if (keyword != null)
            {
                keyword.Keywords = "updated in test";
                var isSuccess = _tagService.UpdateTag(keyword);

                Assert.AreEqual(true, isSuccess);
            }
        }

        [TestMethod]
        public void Delete_Keyword()
        {
            var keyword = _keywordRepository.Table.SingleOrDefault(a => a.ID == 7053);
            if (keyword != null)
            {
                var isSuccess = _tagService.DeleteTag(keyword);

                Assert.AreEqual(true, isSuccess);
            }
        }
    }
}
