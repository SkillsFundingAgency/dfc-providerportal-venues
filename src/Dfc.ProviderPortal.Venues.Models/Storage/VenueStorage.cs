﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dfc.ProviderPortal.Venues;
using Dfc.ProviderPortal.Venues.Models;
using Document = Microsoft.Azure.Documents.Document;


namespace Dfc.ProviderPortal.Venues.Storage
{
    public class VenueStorage
    {
        public class Sequence
        {
            public int SequenceId { get; set; }
        }

        /// <summary>
        /// CosmosDB client and collection
        /// </summary>

        const int LOCATIONID_OFFSET = 100000;   // offset is hardcoded here (rather than configurable via settings) because changing it later could produce duplicate values!
                                                // legacy values from Tribal database are all below this value
        static private DocumentClient docClient = StorageFactory.DocumentClient;
        static private DocumentCollection Collection = StorageFactory.DocumentCollection;

        private static SearchServiceClient _queryService;
        private static ISearchIndexClient _onspdIndex;
        int count = 0;

        /// <summary>
        /// Public constructor
        /// </summary>
        public VenueStorage()
        {
            _queryService = new SearchServiceClient(SettingsHelper.SearchService, new SearchCredentials(SettingsHelper.QueryKey));
            _onspdIndex = _queryService?.Indexes?.GetClient(SettingsHelper.PostcodeIndex);

        }

        /// <summary>
        /// Inserts passed objects as documents into CosmosDB collection
        /// </summary>
        /// <param name="venues">Venue data from SQL database</param>
        /// <param name="log">ILogger for logging info/errors</param>
        public async Task<bool> InsertDocsAsync(IEnumerable<Venue> venues, ILogger log)
        {
            // Insert documents into collection
            try
            {
                // Truncate collection first
                bool ret = await TruncateCollectionAsync(log);

                // Insert each venue in turn as a document
                foreach (Venue v in venues)
                {
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
        /// Set all venues to have LocationId set to value from Tribal database, if any
        /// </summary>
        /// <param name="log">ILogger for logging info/errors</param>
        /// <returns></returns>
        public async Task<IEnumerable<Venue>> UpdateLocationIdsAsync(ILogger log)
        {
            //Update venues docs in Cosmos to have LocationId set to value from Tribal database
            try {
                IEnumerable<Venue> tribalVenues = SQLSync.GetAll(out int count);
                foreach (Venue tribalVenue in tribalVenues.Where(tv => tv.LocationId.HasValue))
                {
                    Venue v = GetByVenueId(tribalVenue.VENUE_ID, log);
                    v.LocationId = tribalVenue.LocationId;
                    v.TribalLocationId = v.LocationId;
                    await UpdateDocAsync(v, log);
                }
                return tribalVenues;

            } catch (Exception ex) {
                Exception be = ex.GetBaseException();
                log.LogError($"Exception rasied at: {DateTime.Now}\n {be?.Message}", ex);
                throw;

            } finally {
            }
        }

        /// <summary>
        /// Inserts a single venue document into the collection
        /// </summary>
        /// <param name="venue">The Venue to insert</param>
        /// <param name="log">ILogger for logging info/errors</param>
        public async Task<ResourceResponse<Document>> InsertDocAsync(Venue venue, ILogger log)
        {
            // Add venue doc to collection
            try {
                if (venue.id == Guid.Empty)
                    venue.id = Guid.NewGuid();
                Uri uri = UriFactory.CreateDocumentCollectionUri(SettingsHelper.Database, SettingsHelper.Collection);

                // Insert venue doc, get it's cosmos "sequence" value, then write it back into LocationId property
                Document doc = await docClient.CreateDocumentAsync(uri, venue);
                Sequence sequence = docClient.CreateDocumentQuery<Sequence>(uri, $"SELECT DOCUMENTID(v) AS SequenceId FROM v " +
                                                                                 $"WHERE v.id = \"{venue.id}\"")
                                             .ToList()
                                             .First();
                doc.SetPropertyValue("LocationId", sequence.SequenceId + LOCATIONID_OFFSET);

                // Add latitude & logitude from onspd azure search
                log.LogInformation($"{count++}: getting lat/long for location {venue.POSTCODE}");
                SearchParameters parameters = new SearchParameters
                {
                    Select = new[] { "pcds", "lat", "long" },
                    SearchMode = SearchMode.All,
                    Top = 1,
                    QueryType = QueryType.Full
                };
                DocumentSearchResult<dynamic> results = _onspdIndex.Documents.Search<dynamic>(venue.POSTCODE, parameters);
                doc.SetPropertyValue("Latitude", (decimal?)results?.Results?.FirstOrDefault()?.Document?.lat);
                doc.SetPropertyValue("Longitude", (decimal?)results?.Results?.FirstOrDefault()?.Document?.@long);

                return await docClient.UpsertDocumentAsync(uri, doc);

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Updates a single venue document in the collection
        /// </summary>
        /// <param name="venue">The Venue to update</param>
        /// <param name="log">ILogger for logging info/errors</param>
        public async Task<ResourceResponse<Document>> UpdateDocAsync(Venue venue, ILogger log)
        {
            try {
                // Get matching venue by Id from the collection
                log.LogInformation($"Getting venues from collection with Id {venue?.id}");
                Document updated = docClient.CreateDocumentQuery(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                                .Where(u => u.Id == venue.id.ToString())
                                .AsEnumerable()
                                .FirstOrDefault();

                if (updated == null)
                    return null;

                updated.SetPropertyValue("ADDRESS_1", venue.ADDRESS_1);
                updated.SetPropertyValue("ADDRESS_2", venue.ADDRESS_2);
                updated.SetPropertyValue("COUNTY", venue.COUNTY);
                updated.SetPropertyValue("POSTCODE", venue.POSTCODE);
                updated.SetPropertyValue("TOWN", venue.TOWN);
                updated.SetPropertyValue("VENUE_NAME", venue.VENUE_NAME);
                //updated.SetPropertyValue("PHONE", venue.PHONE);
                //updated.SetPropertyValue("EMAIL", venue.EMAIL);
                //updated.SetPropertyValue("WEBSITE", venue.WEBSITE);

                updated.SetPropertyValue("Status", (int)venue.Status);
                updated.SetPropertyValue("DateUpdated", DateTime.Now);
                updated.SetPropertyValue("Latitude", venue.Latitude);
                updated.SetPropertyValue("Longitude", venue.Longitude);
                updated.SetPropertyValue("UpdatedBy", venue.UpdatedBy);

                return await docClient.ReplaceDocumentAsync(updated);

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all documents from the collection and returns the data as Venue objects
        /// </summary>
        /// <param name="log">ILogger for logging info/errors</param>
        public async Task<IEnumerable<Venue>> GetAllAsync(ILogger log)
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
                    //log.LogInformation("Querying collection with:");
                    //log.LogInformation($"StorageURI: {SettingsHelper.StorageURI}");
                    //log.LogInformation($"docCient hash: {docClient?.GetHashCode().ToString()}");
                    //log.LogInformation($"Database: {SettingsHelper.Database}");
                    //log.LogInformation($"Collection: {SettingsHelper.Collection}");
                    //log.LogInformation($"Coll Link: {Collection?.SelfLink}");
                    //log.LogInformation($"Coll obj id: {Collection.Id.ToString()}");
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


        public IEnumerable<Venue> Sync(ILogger log, out int count)
        {
            log.LogInformation("Syncing with SQL database");
            IEnumerable<Venue> venues = SQLSync.GetAll(out count);
            return venues;
        }


        async private Task<bool> TruncateCollectionAsync(ILogger log)
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
        /// <param name="log">Ilogger for logging info/errors</param>
        public Venue GetById(Guid id, ILogger log)
        {
            // Get matching venue by id from the collection
            log.LogInformation($"Getting venues from collection with Id {id}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.id == id)
                            .AsEnumerable()
                            .FirstOrDefault();
        }

        /// <summary>
        /// Gets document with matching VenueId from the collection and returns the data as Venue object
        /// </summary>
        /// <param name="venueId">VenueId to search by</param>
        /// <param name="log">Ilogger for logging info/errors</param>
        public Venue GetByVenueId(int venueId, ILogger log)
        {
            // Get matching venue by VenueId from the collection
            log.LogInformation($"Getting venue from collection with VenueId {venueId}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.VENUE_ID == venueId)
                            .AsEnumerable()
                            .FirstOrDefault();
        }

        /// <summary>
        /// Gets document with matching LocationId from the collection and returns the data as Venue object
        /// </summary>
        /// <param name="locationId">LocationId to search by</param>
        /// <param name="log">Ilogger for logging info/errors</param>
        public Venue GetByLocationId(int locationId, ILogger log)
        {
            // Get matching venue by LocationId from the collection
            log.LogInformation($"Getting venue from collection with LocationId {locationId}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.LocationId == locationId)
                            .AsEnumerable()
                            .FirstOrDefault();
        }

        /// <summary>
        /// Gets all documents with matching PRN from the collection and returns the data as Venue objects
        /// </summary>
        /// <param name="PRN">UKPRN to search by</param>
        /// <param name="log">ILogger for logging info/errors</param>
        public IEnumerable<Venue> GetByPRN(int PRN, ILogger log)
        {
            // Get matching venue by PRN from the collection
            log.LogInformation($"Getting venues from collection with PRN {PRN}");
            return docClient.CreateDocumentQuery<Venue>(Collection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 })
                            .Where(v => v.UKPRN == PRN)
                            .AsEnumerable();
        }
    }
}
