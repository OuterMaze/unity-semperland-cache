using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.Unity.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved balances.
        /// </summary>
        public class BalancesResult
        {
            /// <summary>
            ///   Each token balance.
            /// </summary>
            public class Balance
            {
                [JsonProperty("amount")]
                public string Amount;

                [JsonProperty("token")]
                public string Token;
            }

            /// <summary>
            ///   The retrieved permissions.
            /// </summary>
            [JsonProperty("balances")]
            public Balance[] Balances;
        }
    }
}