using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using NHibernate;
using Zuul.Data.Core;
using Zuul.Web.Api;
using Zuul.BusinessLogic.Services;
using Zuul.Data.Repositories;
using System.Web.Http;
using System.Web.Mvc;
using Zuul.Web.Api.Filters;
using Zuul.Web.Controllers;
using Zuul.Web.Filters;

namespace Zuul.Web.App_Start
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            RegisterBusinessLogic(builder);
            RegisterDataRepositories(builder);
            RegisterWebApiFilters(builder);
            RegisterMvcFilters(builder);
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(UserAccountController).Assembly);
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            var container = builder.Build();
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }

        private static void RegisterWebApiFilters(ContainerBuilder builder)
        {
            builder.Register(c => new ValidationExceptionFilter(c.Resolve<ISession>()))
                .AsWebApiExceptionFilterFor<ApiController>()
                .InstancePerRequest();
            builder.Register(c => new NhSessionFilter(c.Resolve<ISession>()))
                .AsWebApiActionFilterFor<ApiController>()
                .InstancePerRequest();
        }

        private static void RegisterMvcFilters(ContainerBuilder builder)
        {
            builder.RegisterFilterProvider();
            builder.Register(c => new NhSessionFilterMvc(c.Resolve<ISession>()))
                .AsActionFilterFor<HomeController>()
                .InstancePerRequest();
        }

        public static void RegisterBusinessLogic(ContainerBuilder builder)
        {
            builder.RegisterType<UserAccountService>().As<UserAccountService>();
            builder.RegisterType<PostService>().As<PostService>();
            builder.RegisterType<ThreadService>().As<ThreadService>();
        }

        public static void RegisterDataRepositories(ContainerBuilder builder)
        {
            // Register NHibernate Session
            builder.Register(x => NHibernateHelper.GetSessionFactory()).SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerRequest();
            // Register Data Repositories
            builder.RegisterType<UserAccountRepository>().As<IUserAccountRepository>();
            builder.RegisterType<PostRepository>().As<IPostRepository>();
            builder.RegisterType<ThreadRepository>().As<IThreadRepository>();
        }
    }
}