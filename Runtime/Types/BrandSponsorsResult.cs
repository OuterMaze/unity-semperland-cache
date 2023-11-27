using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.Unity.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved brand sponsors.
        /// </summary>
        public class BrandSponsorsResult
        {
            /// <summary>
            ///   Each brand sponsor. We only care about the
            ///   "sponsor" field since the other ones are
            ///   redundant, actually.
            /// </summary>
            public class BrandSponsor
            {
                [JsonProperty("sponsor")]
                public string Sponsor;
            }

            /// <summary>
            ///   The retrieved sponsors.
            /// </summary>
            [JsonProperty("sponsors")]
            public BrandSponsor[] Sponsors;
        }
    }
}