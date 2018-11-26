﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Dfc.ProviderPortal.Venues;
using Dfc.ProviderPortal.Venues.Models;
using Newtonsoft.Json;


namespace Dfc.ProviderPortal.Venues.Storage
{
    public class VenueStorage
    {
        /// <summary>
        /// CosmosDB client and collection
        /// </summary>
        static private DocumentClient docClient = StorageFactory.DocumentClient;
        static private DocumentCollection Collection = StorageFactory.DocumentCollection;

        /// <summary>
        /// Public constructor
        /// </summary>
        public VenueStorage() { }

        /// <summary>
        /// Inserts passed objects as documents into CosmosDB collection
        /// </summary>
        /// <param name="venues">Venue data from SQL database</param>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public async Task<bool> InsertDocs(IEnumerable<Venue> venues, TraceWriter log)
        {
            // Insert documents into collection
            try
            {
                // Truncate collection first
                bool ret = await TruncateCollectionAsync(log);

                // Insert each venue in turn as a document
                foreach (Venue v in venues)
                {
                    // Add venue doc to collection
                    v.id = Guid.NewGuid();
                    Task<ResourceResponse<Document>> task = docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(SettingsHelper.Database, SettingsHelper.Collection),
                                                                                          v);
                    task.Wait();
                }

            } catch (DocumentClientException ex) {
                Exception be = ex.GetBaseException();
                log.Error($"Exception rasied at: {DateTime.Now}\n {be.Message}", ex);
                throw;
            } catch (Exception ex) {
                Exception be = ex.GetBaseException();
                log.Error($"Exception rasied at: {DateTime.Now}\n {be.Message}", ex);
                throw;
            } finally {
            }
            return true;
        }

        /// <summary>
        /// Gets all documents from the collection and returns the data as Venue objects
        /// </summary>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public async Task<IEnumerable<Venue>> GetAll(TraceWriter log)
        {
            // Get all venue documents in the collection
            string token = null;
            Task<FeedResponse<dynamic>> task = null;
            List<dynamic> docs = new List<dynamic>();
            log.Info("Getting all venues from collection");


            // Read documents in batches, using continuation token to make sure we get them all
            do {
                log.Info("Querying collection");
                task = docClient.ReadDocumentFeedAsync(Collection.SelfLink, new FeedOptions { MaxItemCount = -1, RequestContinuation = token });
                //task.Wait();
                token = task.Result.ResponseContinuation;
                log.Info("Collating results");
                docs.AddRange(task.Result.ToList());
            } while (token != null);
            //FeedResponse<dynamic> response = await task;

            // Collections are schema-less and can therefore hold any data, even though we're only storing Venue docs
            // So we can cast the returned data by serializing to json and then deserialising into Venue objects
            log.Info($"Serializing data for {docs.LongCount()} venues");
            string json = JsonConvert.SerializeObject(docs);
            return JsonConvert.DeserializeObject<IEnumerable<Venue>>(json);
        }


        //public async Task<IEnumerable<Venue>> Sync(TraceWriter log)
        public IEnumerable<Venue> Sync(TraceWriter log, out int count)
        {
            log.Info("Syncing with SQL database");
            IEnumerable<Venue> venues = SQLSync.GetAll(out count);
            return venues;
        }


        async private Task<bool> TruncateCollectionAsync(TraceWriter log)
        {
            try {
                log.Info("Deleting all docs from venues collection");
                IEnumerable<Document> docs = docClient.CreateDocumentQuery<Document>(Collection.SelfLink,
                                                                                     new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                                                      .AsEnumerable();
                foreach (Document d in docs) {
                    await docClient.DeleteDocumentAsync(d.SelfLink);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return true;
        }


        ///// <summary>
        ///// Gets all documents with matching PRN from the collection and returns the data as Venue objects
        ///// </summary>
        ///// <param name="PRN">UKPRN to search by</param>
        ///// <param name="log">TraceWriter for logging info/errors</param>
        //public Venue GetByPRN(string PRN, TraceWriter log)
        //{
        //    // Get matching venue by PRN from the collection
        //    log.LogInformation($"Getting venues from collection with PRN {PRN}");
        //    return client.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
        //                 .Where(v => v.PROVIDER_ID == PRN)
        //                 .AsEnumerable()
        //                 .FirstOrDefault();
        //}

        ///// <summary>
        ///// Gets all documents with partial matching Name from the collection and returns the data as Venue objects
        ///// </summary>
        ///// <param name="Name">Name fragment to search by</param>
        ///// <param name="log">TraceWriter for logging info/errors</param>
        //public IEnumerable<Venue> GetByName(string Name, TraceWriter log, out long count)
        //{
        //    // Get matching venue by passed fragment of Name from the collection
        //    log.LogInformation($"Getting venues from collection matching Name {Name}");
        //    IQueryable<Venue> qry = client.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
        //                                     .Where(p => p.PROVIDER_ID.ToLower().Contains(Name.ToLower()));
        //    IEnumerable<Venue> matches = qry.AsEnumerable();
        //    count = matches.LongCount();
        //    return matches;
        //}
    }
}
