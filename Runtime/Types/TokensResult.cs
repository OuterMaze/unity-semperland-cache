using Newtonsoft.Json;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved tokens.
        /// </summary>
        public class TokensResult
        {
            /// <summary>
            ///   The list of tokens.
            /// </summary>
            [JsonProperty("tokens")]
            public TokenMetadata[] Tokens;
        }
    }
}