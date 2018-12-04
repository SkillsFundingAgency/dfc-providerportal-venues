
//using System;
//using Microsoft.Extensions.DependencyInjection;
//using Dfc.ProviderPortal.Venues.Cosmos.Helper;
//using Dfc.ProviderPortal.Venues.GetCustomerByIdHttpTrigger.Service;
//using Dfc.ProviderPortal.Venues.GetCustomerHttpTrigger.Service;
//using Dfc.ProviderPortal.Venues.Helpers;
//using Dfc.ProviderPortal.Venues.PatchCustomerHttpTrigger.Service;
//using Dfc.ProviderPortal.Venues.PostCustomerHttpTrigger;
//using Dfc.ProviderPortal.Venues.PostCustomerHttpTrigger.Service;
//using Dfc.ProviderPortal.Venues.SearchCustomerHttpTrigger.Service;
//using Dfc.ProviderPortal.Venues.Validation;


//namespace Dfc.ProviderPortal.Venues.Ioc
//{
//    public class RegisterServiceProvider
//    {
//        public IServiceProvider CreateServiceProvider()
//        {
//            var services = new ServiceCollection();

//            services.AddTransient<IGetCustomerByIdHttpTriggerService, GetCustomerByIdHttpTriggerService>();
//            services.AddTransient<IPostCustomerHttpTriggerService, PostCustomerHttpTriggerService>();
//            services.AddTransient<IPatchCustomerHttpTriggerService, PatchCustomerHttpTriggerService>();
//            services.AddTransient<IGetCustomerHttpTriggerService, GetCustomerHttpTriggerService >();
//            services.AddTransient<ISearchCustomerHttpTriggerService, SearchCustomerHttpTriggerService>();

//            services.AddTransient<IResourceHelper, ResourceHelper>();
//            services.AddTransient<IValidate, Validate>();
//            services.AddTransient<IHttpRequestMessageHelper, HttpRequestMessageHelper>();
//            return services.BuildServiceProvider(true);
//        }
//    }
//}
