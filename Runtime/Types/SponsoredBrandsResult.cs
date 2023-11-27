using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved sponsored brands.
        /// </summary>
        public class SponsoredBrandsResult
        {
            /// <summary>
            ///   Each brand sponsor. We only care about the
            ///   "brand" field since the other ones are
            ///   redundant, actually.
            /// </summary>
            public class SponsoredBrand
            {
                [JsonProperty("brand")]
                public string Brand;
            }

            /// <summary>
            ///   The retrieved brands.
            /// </summary>
            [JsonProperty("brands")]
            public SponsoredBrand[] Brands;
        }
    }
}