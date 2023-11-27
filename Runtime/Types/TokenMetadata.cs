using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting a token metadata entry.
        ///   It has not just the EVM metadata but also
        ///   some other discriminators.
        /// </summary>
        public class TokenMetadata
        {
            /// <summary>
            ///   The inner contents of the metadata.
            /// </summary>
            public class TokenMetadataContent
            {
                /// <summary>
                ///   The name of the token.
                /// </summary>
                [JsonProperty("name")]
                public string Name;

                /// <summary>
                ///   The description of the token.
                /// </summary>
                [JsonProperty("description")]
                public string Description;

                /// <summary>
                ///   The image of the token.
                /// </summary>
                [JsonProperty("image")]
                public string Image;

                /// <summary>
                ///   The decimals of the token. 0 for NFTs and
                ///   many tokens with no fractional part. 18
                ///   for currency tokens.
                /// </summary>
                [JsonProperty("decimal")]
                public int Decimals;

                /// <summary>
                ///   The arbitrary token properties.
                /// </summary>
                [JsonProperty("properties")]
                public JObject Properties;
            }
            
            /// <summary>
            ///   The token id.
            /// </summary>
            [JsonProperty("token")]
            public string Token;

            /// <summary>
            ///   The token group (ft or nft).
            /// </summary>
            [JsonProperty("token_group")]
            public string TokenGroup;

            /// <summary>
            ///   The brand this token belongs to.
            /// </summary>
            [JsonProperty("brand")]
            public string Brand;

            /// <summary>
            ///   The actual metadata content.
            /// </summary>
            [JsonProperty("metadata")]
            public TokenMetadataContent Metadata;
        }
    }
}