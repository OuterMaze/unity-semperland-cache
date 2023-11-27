using Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved metaverse parameters.
        /// </summary>
        public class ParametersResult
        {
            /// <summary>
            ///   Each parameter. We only care about the
            ///   key and value.
            /// </summary>
            public class Parameter
            {
                [JsonProperty("key")]
                public string Key;

                [JsonProperty("value")]
                public object Value;
            }

            /// <summary>
            ///   The retrieved metaverse parameters.
            /// </summary>
            [JsonProperty("parameters")]
            public Parameter[] Parameters;
        }
    }
}