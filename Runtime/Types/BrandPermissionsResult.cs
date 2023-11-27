using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved brand permissions.
        /// </summary>
        public class BrandPermissionsResult
        {
            /// <summary>
            ///   Each brand permission. We only care about the
            ///   "permission" field since the other ones are
            ///   redundant, actually.
            /// </summary>
            public class BrandPermission
            {
                [JsonProperty("permission")]
                public string Permission;
            }

            /// <summary>
            ///   The retrieved permissions.
            /// </summary>
            [JsonProperty("permissions")]
            public BrandPermission[] Permissions;
        }
    }
}