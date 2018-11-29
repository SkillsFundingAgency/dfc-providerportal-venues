
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
        public async Task<bool> InsertDocsAsync(IEnumerable<Venue> venues, ILogger log) //TraceWriter log)
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
                    //v.id = Guid.NewGuid();
                    //Task<ResourceResponse<Document>> task = docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(SettingsHelper.Database, SettingsHelper.Collection),
                    //                                                                      v);
                    Task<ResourceResponse<Document>> task = InsertDocAsync(v, log);
                    task.Wait();
                }

            } catch (DocumentClientException ex) {
                Exception be = ex.GetBaseException();
                log.LogError($"Exception rasied at: {DateTime.Now}\n {be.Message}", ex);
                throw;
            } catch (Exception ex) {
                Exception be = ex.GetBaseException();
                log.LogError($"Exception rasied at: {DateTime.Now}\n {be.Message}", ex);
                throw;
            } finally {
            }
            return true;
        }

        /// <summary>
        /// Inserts a single venue document into the collection
        /// </summary>
        /// <param name="venue">The Venue to insert</param>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public async Task<ResourceResponse<Document>> InsertDocAsync(Venue venue, ILogger log)
        {
            // Add venue doc to collection
            try {
                venue.id = Guid.NewGuid();
                return await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(SettingsHelper.Database, SettingsHelper.Collection),
                                                           venue);
            } catch (Exception ex) {
                    throw ex;
            }
        }

        /// <summary>
        /// Gets all documents from the collection and returns the data as Venue objects
        /// </summary>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public async Task<IEnumerable<Venue>> GetAllAsync(ILogger log) //TraceWriter log)
        {
            try
            {
                // Get all venue documents in the collection
                string token = null;
                Task<FeedResponse<dynamic>> task = null;
                List<dynamic> docs = new List<dynamic>();
                log.LogInformation("Getting all venues from collection");

                // Read documents in batches, using continuation token to make sure we get them all
                do {
                    log.LogInformation("Querying collection with:");
                    log.LogInformation($"docCient hash: {docClient.GetHashCode().ToString()}");
                    log.LogInformation($"Database: {SettingsHelper.Database}");
                    log.LogInformation($"Collection: {SettingsHelper.Collection}");
                    log.LogInformation($"Coll Link: {Collection?.SelfLink}");
                    log.LogInformation($"Coll obj id: {Collection.Id.ToString()}");
                    task = docClient.ReadDocumentFeedAsync(Collection.SelfLink, new FeedOptions { MaxItemCount = -1, RequestContinuation = token });
                    //task.Wait();
                    token = task.Result.ResponseContinuation;
                    log.LogInformation("Collating results");
                    docs.AddRange(task.Result.ToList());
                } while (token != null);
                //FeedResponse<dynamic> response = await task;

                // Collections are schema-less and can therefore hold any data, even though we're only storing Venue docs
                // So we can cast the returned data by serializing to json and then deserialising into Venue objects
                log.LogInformation($"Serializing data for {docs.LongCount()} venues");
                string json = JsonConvert.SerializeObject(docs);
                return JsonConvert.DeserializeObject<IEnumerable<Venue>>(json);

            } catch (Exception ex) {
                throw ex;
            }
        }


        public IEnumerable<Venue> Sync(ILogger log, out int count) // TraceWriter log, out int count)
        {
            log.LogInformation("Syncing with SQL database");
            IEnumerable<Venue> venues = SQLSync.GetAll(out count);
            return venues;
        }


        async private Task<bool> TruncateCollectionAsync(ILogger log) //TraceWriter log)
        {
            try {
                log.LogInformation("Deleting all docs from venues collection");
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


        /// <summary>
        /// Gets document with matching id from the collection and returns the data as Venue object
        /// </summary>
        /// <param name="id">Document id to search by</param>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public Venue GetById(Guid id, ILogger log) //TraceWriter log)
        {
            // Get matching venue by PRN from the collection
            log.LogInformation($"Getting venues from collection with Id {id}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.id == id)
                            .AsEnumerable()
                            .FirstOrDefault();
        }

        /// <summary>
        /// Gets all documents with matching PRN from the collection and returns the data as Venue objects
        /// </summary>
        /// <param name="PRN">UKPRN to search by</param>
        /// <param name="log">TraceWriter for logging info/errors</param>
        public IEnumerable<Venue> GetByPRN(int PRN, ILogger log) //TraceWriter log)
        {
            // Get matching venue by PRN from the collection
            log.LogInformation($"Getting venues from collection with PRN {PRN}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.UKPRN == PRN)
                            .AsEnumerable();
        }

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
