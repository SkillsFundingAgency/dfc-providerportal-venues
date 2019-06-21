﻿
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;


//namespace Dfc.ProviderPortal.Venues.API.Controllers
//{
//    /// <summary>
//    /// Test controller for lars search
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        /// <summary>
//        /// test facets
//        /// </summary>
//        public enum LarsSearchFacet
//        {
//            /// <summary/>
//            NotionalNVQLevelv2 = 0,
//            /// <summary/>
//            AwardOrgCode = 1,
//            /// <summary/>
//            AwardOrgAimRef = 2,
//            /// <summary/>
//            SectorSubjectAreaTier1 = 3,
//            /// <summary/>
//            SectorSubjectAreaTier2 = 4
//        }

//        /// <summary>
//        /// test criteria
//        /// </summary>
//        public class LarsSearchCriteria //: ValueObject<LarsSearchCriteria>, ILarsSearchCriteria
//        {
//            /// <summary/>
//            public string Search { get; set; }
//            /// <summary/>
//            public int Top { get; set; }
//            /// <summary/>
//            public int Skip { get; set; }
//            /// <summary/>
//            public bool Count => true;
//            /// <summary/>
//            public string Filter { get; set; }
//            /// <summary/>
//            public IEnumerable<LarsSearchFacet> Facets { get; set; }
//        }

//        /// <summary>
//        /// test result
//        /// </summary>
//        public class LarsSearchResult //: ValueObject<LarsSearchResult>, ILarsSearchResult
//        {
//            /// <summary/>
//            [JsonProperty(PropertyName = "@odata.context")]
//            public string ODataContext { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "@odata.count")]
//            public int? ODataCount { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "@search.facets")]
//            public LarsSearchFacets SearchFacets { get; set; }
//            /// <summary/>
//            public IEnumerable<LarsSearchResultItem> Value { get; set; }
//        }

//        /// <summary>
//        /// test facets
//        /// </summary>
//        public class LarsSearchFacets //: ValueObject<LarsSearchFacets>, ILarsSearchFacets
//        {
//            /// <summary/>
//            public IEnumerable<SearchFacet> AwardOrgCode { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "AwardOrgCode@odata.type")]
//            public string AwardOrgCodeODataType { get; set; }
//            /// <summary/>
//            public IEnumerable<SearchFacet> NotionalNVQLevelv2 { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "NotionalNVQLevelv2@odata.type")]
//            public string NotionalNVQLevelv2ODataType { get; set; }
//            /// <summary/>
//            public IEnumerable<SearchFacet> SectorSubjectAreaTier1 { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "SectorSubjectAreaTier1@odata.type")]
//            public string SectorSubjectAreaTier1ODataType { get; set; }
//            /// <summary/>
//            public IEnumerable<SearchFacet> SectorSubjectAreaTier2 { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "SectorSubjectAreaTier2@odata.type")]
//            public string SectorSubjectAreaTier2ODataType { get; set; }
//            /// <summary/>
//            public IEnumerable<SearchFacet> AwardOrgAimRef { get; set; }
//            /// <summary/>
//            [JsonProperty(PropertyName = "AwardOrgAimRef@odata.type")]
//            public string AwardOrgAimRefODataType { get; set; }
//        }

//        /// <summary>
//        /// test search facet
//        /// </summary>
//        public class SearchFacet //: ValueObject<SearchFacet>, ISearchFacet
//        {
//            /// <summary/>
//            public int Count { get; set; }
//            /// <summary/>
//            public string Value { get; set; }
//        }

//        /// <summary>
//        /// test result item
//        /// </summary>
//        public class LarsSearchResultItem //: ValueObject<LarsSearchResultItem>, ILarsSearchResultItem
//        {
//            /// <summary/>
//            [JsonProperty(PropertyName = "@search.score")]
//            public decimal SearchScore { get; set; }
//            /// <summary/>
//            public string LearnAimRef { get; set; }
//            /// <summary/>
//            public string LearnAimRefTitle { get; set; }
//            /// <summary/>
//            public string NotionalNVQLevelv2 { get; set; }
//            /// <summary/>
//            public string AwardOrgCode { get; set; }
//            /// <summary/>
//            public string LearnDirectClassSystemCode1 { get; set; }
//            /// <summary/>
//            public string LearnDirectClassSystemCode2 { get; set; }
//            /// <summary/>
//            public string GuidedLearningHours { get; set; }
//            /// <summary/>
//            public string TotalQualificationTime { get; set; }
//            /// <summary/>
//            public string UnitType { get; set; }
//            /// <summary/>
//            public string AwardOrgName { get; set; }
//            /// <summary/>
//            public string LearnAimRefTypeDesc { get; set; }
//            /// <summary/>
//            public DateTime? CertificationEndDate { get; set; }
//            /// <summary/>
//            public string SectorSubjectAreaTier1Desc { get; set; }
//            /// <summary/>
//            public string SectorSubjectAreaTier2Desc { get; set; }
//            /// <summary/>
//            public string AwardOrgAimRef { get; set; }
//        }


//        /// <summary>
//        /// Test method
//        /// POST api/lars
//        /// </summary>
//        /// <param name="criteria">Criteria</param>
//        [HttpPost]
//        public LarsSearchResult Post([FromBody] LarsSearchCriteria criteria)
//        {
//            return new LarsSearchResult()
//            {
//                ODataContext = "https://dfc-dev-prov-sch.search.windows.net/indexes('course')/$metadata#docs(*)",
//                ODataCount = 567,
//                SearchFacets = new LarsSearchFacets() { AwardOrgCodeODataType = "AOCODT", AwardOrgCode = new SearchFacet[] { new SearchFacet() { Value = "abc", Count = 3 } } },
//                Value = new LarsSearchResultItem[] { new LarsSearchResultItem() { SearchScore = 123.4567800009M, AwardOrgAimRef = "AOAR", NotionalNVQLevelv2 = "nvq" } }
//            };
//        }
//    }
//}