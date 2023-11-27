using Newtonsoft.Json;

namespace OuterMaze.Unity.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved brands.
        /// </summary>
        public class BrandsResult
        {
            /// <summary>
            ///   The list of brands.
            /// </summary>
            [JsonProperty("brands")]
            public TokenMetadata[] Brands;
        }
    }
}