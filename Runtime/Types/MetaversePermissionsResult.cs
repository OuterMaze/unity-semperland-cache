using Unity.Plastic.Newtonsoft.Json;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   A result depicting the retrieved metaverse permissions.
        /// </summary>
        public class MetaversePermissionsResult
        {
            /// <summary>
            ///   Each metaverse permission. We only care about
            ///   the "permission" field since the other ones are
            ///   redundant, actually.
            /// </summary>
            public class MetaversePermission
            {
                [JsonProperty("permission")]
                public string Permission;
            }

            /// <summary>
            ///   The retrieved permissions.
            /// </summary>
            [JsonProperty("permissions")]
            public MetaversePermission[] Permissions;
        }
    }
}