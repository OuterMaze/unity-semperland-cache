using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved deals.
        /// </summary>
        public class DealsResult
        {
            /// <summary>
            ///   Each deal.
            /// </summary>
            public class Deal
            {
                [JsonProperty("index")]
                public string Index;
                
                [JsonProperty("emitter")]
                public string Emitter;

                [JsonProperty("emitter_ids")]
                public string[] EmitterTokens;

                [JsonProperty("emitter_amounts")]
                public string[] EmitterAmounts;

                [JsonProperty("receiver")]
                public string Receiver;
                
                [JsonProperty("receiver_ids")]
                public string[] ReceiverTokens;

                [JsonProperty("receiver_amounts")]
                public string[] ReceiverAmounts;

                [JsonProperty("status")]
                public string Status;
            }

            /// <summary>
            ///   The retrieved deals.
            /// </summary>
            [JsonProperty("deals")]
            public Deal[] Deals;
        }
    }
}