
using System;
using Microsoft.Extensions.Configuration;


namespace Dfc.ProviderPortal.Venues
{
    /// <summary>
    /// Read settings from environment variables (no settings available here as they are through DI in a .NET Core MVC app)
    /// These can be input in Debug tab of project's properties and are held in launchSettings.json
    /// They should match environment variables set up by DevOps in settings for Azure Functions
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Built config root from settings file
        /// </summary>
        static private IConfigurationRoot config = new ConfigurationBuilder().AddEnvironmentVariables()
                                                                            .Build();

        /// <summary>
        /// Properties wrapping up app setttings
        /// </summary>
        static public string ConnectionString = config.GetValue<string>("APPSETTING_SQLConnectionString");
        static public string StorageURI = config.GetValue<string>("APPSETTING_CosmosStorageURI");
        static public string PrimaryKey = config.GetValue<string>("APPSETTING_CosmosPrimaryKey");
        static public string Database = config.GetValue<string>("APPSETTING_CosmosDatabase");
        static public string Collection = config.GetValue<string>("APPSETTING_CosmosCollection");
    }
}
