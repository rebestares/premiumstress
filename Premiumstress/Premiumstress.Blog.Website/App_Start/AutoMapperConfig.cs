using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core.Domain;

namespace Premiumstress.Blog.Website
{
    using Core.Domain.Blog;

    public static class AutomMapperConfig
    {
        public static void Initialize()
        {
            Mapper.CreateMap<BlogModel, Blog>()
                .ForMember(dest => dest.Imagelinks,
                    mo =>
                        mo.MapFrom(
                            src =>
                                new List<Imagelink>
                                {
                                    new Imagelink
                                    {
                                        FullImageLink = src.ImageLinks.FullImageLink,
                                        ThumbnailImageLink = src.ImageLinks.ThumbnailImageLink
                                    }
                                }))
                 .ForMember(dest => dest.BlogCategories,
                    mo =>
                        mo.MapFrom(
                            src => new List<BlogCategory>
                                {
                                    new BlogCategory
                                    {
                                        CategoryID = src.Category.ID
                                    }
                                }));
            Mapper.CreateMap<Blog, BlogModel>()
                .ForMember(dest => dest.ImageLinks,
                    mo =>
                        mo.MapFrom(
                            src => src.Imagelinks.Count != 0
                                ? new ImageLinkModel()
                                {
                                    FullImageLink = src.Imagelinks.FirstOrDefault().FullImageLink,
                                    ThumbnailImageLink = src.Imagelinks.FirstOrDefault().ThumbnailImageLink

                                }
                                : null))
                .ForMember(dest => dest.Category,
                    mo =>
                        mo.MapFrom(
                            src => src.BlogCategories.Count != 0
                                ? new CategoryModel()
                                {
                                    Name = src.BlogCategories.FirstOrDefault(a => a.BlogID == src.ID).Category.Name,
                                    ID = src.BlogCategories.FirstOrDefault(a => a.BlogID == src.ID).Category.ID,
                                    DisplayOrder =
                                        src.BlogCategories.FirstOrDefault(a => a.BlogID == src.ID).Category.DisplayOrder,
                                    Type = src.BlogCategories.FirstOrDefault(a => a.BlogID == src.ID).Category.Type
                                }
                                : null))

                .ForMember(dest => dest.Keywords,
                    mo =>
                        mo.MapFrom(
                            src => src.Keywords != null
                                ? (from keyword in src.Keywords
                                   select keyword.Keywords).ToList()
                                : null))
                .ForMember(dest => dest.DatePosted,
                    mo =>
                        mo.MapFrom(
                            src =>
                                src.DatePosted != null ? src.DatePosted.Value.ToString("MMMM dd, yyyy") : string.Empty
                            ));
            Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.Imagelinks,
                    mo =>
                        mo.MapFrom(src => new List<Imagelink>
                        {
                            new Imagelink
                            {
                                FullImageLink = src.ImageLinks.FullImageLink,
                                ThumbnailImageLink = src.ImageLinks.ThumbnailImageLink
                            }
                        })
                );
            Mapper.CreateMap<CategoryModel, IEnumerable<BlogCategory>>();
            Mapper.CreateMap<ImageLinkModel, IEnumerable<Imagelink>>();
            Mapper.CreateMap<CategoryModel, Category>();
            Mapper.CreateMap<Category, CategoryModel>();
            Mapper.CreateMap<EmailModel, Email>();
            Mapper.CreateMap<Keyword, KeywordModel>()
                .ForMember(dest => dest.BlogCount,
                    mo => mo.MapFrom(src => src.Blogs.Count(a => a.IsDeleted != true && a.IsApproved == true)));
            Mapper.CreateMap<User, UserModel>()
                .ForMember(dest => dest.ImageLinks,
                    mo =>
                        mo.MapFrom(
                            src =>
                                new ImageLinkModel
                                {
                                    FullImageLink = src.Imagelinks.FirstOrDefault().FullImageLink,
                                    ThumbnailImageLink = src.Imagelinks.FirstOrDefault().ThumbnailImageLink
                                }));
            Mapper.CreateMap<List<string>, IEnumerable<Keyword>>();

        }
    }
}