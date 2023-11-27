using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace OuterMaze.SemperLand.Cache
{
    namespace Types
    {
        /// <summary>
        ///   Retrieves elements from the SemperLand cache.
        /// </summary>
        public class CacheResource
        {
            /// <summary>
            ///   An exception interacting with this cache.
            /// </summary>
            public class CacheException : Exception
            {
                public CacheException() {}
                public CacheException(string message) : base(message) {}
            }
            
            /// <summary>
            ///   Exception: The cache is unreachable.
            /// </summary>
            public class UnreachableException : CacheException {}

            /// <summary>
            ///   Exception: The code is not 200.
            /// </summary>
            public class UnexpectedCode : CacheException
            {
                /// <summary>
                ///   The HTTP code.
                /// </summary>
                public readonly long Code;
                
                public UnexpectedCode(long code, string message) : base(message)
                {
                    Code = code;
                }
            }

            /// <summary>
            ///   Exception: Invalid JSON Response.
            /// </summary>
            public class InvalidResponse : CacheException {}

            // The path to list brands.
            private const string BrandsPath = "/brands";
            
            // The path to list permissions of a brand and a user.
            private const string BrandPermissionsPath = "/brands/{0}/permissions/{1}";
            
            // The path to list sponsors of a brand.
            private const string BrandSponsorsPath = "/brands/{0}/sponsors";
            
            // The path to list balances of a user.
            private const string BalancesPath = "/balances/{0}";
            
            // The path to list deals of a user.
            private const string DealsPath = "/deals/{0}";
            
            // The path to list the current parameters of the metaverse.
            private const string MetaverseParametersPath = "/parameters";
            
            // The path to list permissions of the metaverse and a user.
            private const string MetaversePermissionsPath = "/permissions/{0}";
            
            // The path to list sponsors.
            private const string SponsoredBrandsPath = "/sponsors/{0}/brands";
            
            // The path to list tokens (e.g. full-text searches).
            private const string TokensPath = "/tokens";

            // The path to list tokens for a brand (also allows for full-text searches).
            private const string TokensForBrandPath = "/tokens/{0}";

            /// <summary>
            ///   The base endpoint for this cache.
            /// </summary>
            public readonly string BaseEndpoint;

            public CacheResource(string baseEndpoint)
            {
                BaseEndpoint = baseEndpoint.TrimEnd('/');
            }

            // Makes the full URL.
            private string MakeFullUrl(string url, Dictionary<string, object> data)
            {
                string fullUrl = $"{BaseEndpoint}{url}";
                if (data.Count != 0)
                {
                    fullUrl += "?" + string.Join(
                        "&",
                        from item in data
                        select $"{System.Uri.EscapeDataString(item.Key)}={System.Uri.EscapeDataString(item.Value.ToString())}" 
                    );
                }
                return fullUrl;
            }

            // Sends a request, waits for it, and captures some errors.
            private static async Task SendRequest(UnityWebRequest request)
            {
                await request.SendWebRequest();
                // Check whether the request was done successfully.
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new UnreachableException();
                }
            }
            
            // De-serializes the content.
            private static ElementType Deserialize<ElementType>(byte[] data)
            {
                try
                {
                    return JsonSerializer.Create().Deserialize<ElementType>(
                        new JsonTextReader(new StreamReader(new MemoryStream(data)))
                    );
                }
                catch (Exception)
                {
                    throw new InvalidResponse();
                }
            }
            
            private async Task<E> Get<E>(string fullUrl)
            {
                UnityWebRequest request = new UnityWebRequest(fullUrl);
                request.method = "GET";
                request.downloadHandler = new DownloadHandlerBuffer();
                await SendRequest(request);
                long status = request.responseCode;
                if (status != 200) throw new UnexpectedCode(status, fullUrl);
                return Deserialize<E>(request.downloadHandler.data);
            }

            /// <summary>
            ///   Loads the brands.
            /// </summary>
            /// <param name="page">The page</param>
            /// <param name="text">The text to filter by</param>
            /// <returns>The brands (full metadata of them)</returns>
            public Task<BrandsResult> Brands(uint page = 0, string text = "")
            {
                text = text.Trim();
                return Get<BrandsResult>(MakeFullUrl(BrandsPath, new Dictionary<string,object>
                {
                    {"page", page},
                    {"text", text}
                }));
            }

            /// <summary>
            ///   Loads the permissions for a brand and a user.
            /// </summary>
            /// <param name="page">The page</param>
            /// <param name="brand">The brand</param>
            /// <param name="user">The user</param>
            /// <returns>The permissions (only the permission key for each)</returns>
            public Task<BrandPermissionsResult> BrandPermissions(string brand, string user, uint page = 0)
            {
                brand = brand.Trim();
                user = user.Trim();
                return Get<BrandPermissionsResult>(
                    MakeFullUrl(string.Format(BrandPermissionsPath, brand, user), new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }

            /// <summary>
            ///   Loads the sponsors of a brand.
            /// </summary>
            /// <param name="brand">The brand</param>
            /// <param name="page">The page</param>
            /// <returns>The sponsors (only addresses)</returns>
            public Task<BrandSponsorsResult> BrandSponsors(string brand, uint page = 0)
            {
                brand = brand.Trim();
                return Get<BrandSponsorsResult>(
                    MakeFullUrl(string.Format(BrandSponsorsPath, brand), new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }

            /// <summary>
            ///   Retrieves balances of tokens for an owner.
            /// </summary>
            /// <param name="owner">The owner</param>
            /// <param name="page">The page</param>
            /// <param name="tokens">Optionally, the tokens</param>
            /// <returns>The balances</returns>
            public Task<BalancesResult> Balances(string owner, uint page = 0, List<string> tokens = null)
            {
                owner = owner.Trim();
                return Get<BalancesResult>(
                    MakeFullUrl(string.Format(BalancesPath, owner), new Dictionary<string, object>
                    {
                        {"page", page},
                        {"tokens", tokens != null ? string.Join(",", tokens) : ""}
                    })
                );
            }

            /// <summary>
            ///   Retrieves the deals of a user.
            /// </summary>
            /// <param name="dealer">The user</param>
            /// <param name="page">The page</param>
            /// <returns>The deals</returns>
            public Task<DealsResult> Deals(string dealer, uint page = 0)
            {
                dealer = dealer.Trim();
                return Get<DealsResult>(
                    MakeFullUrl(string.Format(DealsPath, dealer), new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }

            /// <summary>
            ///   Retrieves the parameters of the metaverse.
            /// </summary>
            /// <param name="page">The page</param>
            /// <returns>The parameters</returns>
            public Task<ParametersResult> Parameters(uint page = 0)
            {
                return Get<ParametersResult>(
                    MakeFullUrl(MetaverseParametersPath, new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }
            
            /// <summary>
            ///   Loads the permissions for the metaverse and a user.
            /// </summary>
            /// <param name="page">The page</param>
            /// <param name="user">The user</param>
            /// <returns>The permissions (only the permission key for each)</returns>
            public Task<MetaversePermissionsResult> Permissions(string user, uint page = 0)
            {
                user = user.Trim();
                return Get<MetaversePermissionsResult>(
                    MakeFullUrl(string.Format(MetaversePermissionsPath, user), new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }
            
            /// <summary>
            ///   Loads the details (sponsored brands) of a sponsor.
            /// </summary>
            /// <param name="sponsor">The sponsor</param>
            /// <param name="page">The page</param>
            /// <returns>The brands (only addresses)</returns>
            public Task<SponsoredBrandsResult> SponsoredBrands(string sponsor, uint page = 0)
            {
                sponsor = sponsor.Trim();
                return Get<SponsoredBrandsResult>(
                    MakeFullUrl(string.Format(SponsoredBrandsPath, sponsor), new Dictionary<string, object>
                    {
                        {"page", page}
                    })
                );
            }

            /// <summary>
            ///   Loads some tokens.
            /// </summary>
            /// <param name="brand">The brand</param>
            /// <param name="page">The page</param>
            /// <param name="text">The text to filter by</param>
            /// <param name="tokens">The tokens</param>
            /// <returns>The tokens' metadata</returns>
            public Task<TokensResult> Tokens(
                string brand = "", uint page = 0, string text = "", List<string> tokens = null
            ) {
                brand = (brand ?? "").Trim();
                string url = brand == "" ? TokensPath : string.Format(TokensForBrandPath, brand);
                return Get<TokensResult>(
                    MakeFullUrl(url, new Dictionary<string, object>
                    {
                        {"page", page},
                        {"text", text},
                        {"tokens", tokens != null ? string.Join(",", tokens) : ""}
                    })
                );
            }
        }
    }
}