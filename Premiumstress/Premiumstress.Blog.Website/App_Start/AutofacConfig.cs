using System.Data.Entity;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Premiumstress.Blog.Services.Blogs;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Contact;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.Tag;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Services.Video;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Website
{
    using Core.Domain.Blog;

    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof (MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register our Data dependencies
            //builder.RegisterModule(new DataModule("MVCWithAutofacDB"));

            //Services
            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerRequest();
            builder.RegisterType<TagService>().As<ITagService>().InstancePerRequest();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerRequest();
            builder.RegisterType<ContactService>().As<IContactService>().InstancePerRequest();
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerRequest();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<VideoService>().As<IVideoService>().InstancePerRequest();

            //Repos
            builder.RegisterType<Repository<Blog>>().As<IRepository<Blog>>().InstancePerRequest();
            builder.RegisterType<Repository<BlogCategory>>().As<IRepository<BlogCategory>>().InstancePerRequest();
            builder.RegisterType<Repository<Imagelink>>().As<IRepository<Imagelink>>().InstancePerRequest();
            builder.RegisterType<Repository<Imagelink>>().As<IRepository<Imagelink>>().InstancePerRequest();
            builder.RegisterType<Repository<Category>>().As<IRepository<Category>>().InstancePerRequest();
            builder.RegisterType<Repository<User>>().As<IRepository<User>>().InstancePerRequest();
            builder.RegisterType<Repository<Keyword>>().As<IRepository<Keyword>>().InstancePerRequest();
            builder.RegisterType<Repository<Imagelink>>().As<IRepository<Imagelink>>().InstancePerRequest();
            builder.RegisterType<Repository<BlogVideo>>().As<IRepository<BlogVideo>>().InstancePerRequest();
            builder.RegisterType<ObjectContextBase>().As<IDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(EfContext<>)).As(typeof(IRepository<>)).InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType(typeof(PremiumStressContext)).As(typeof(DbContext)).InstancePerLifetimeScope();

            var container = builder.Build();
            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}