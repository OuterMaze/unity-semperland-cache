using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

namespace OuterMaze.Unity.SemperLand.Cache
{
    namespace Samples
    {
        using Types;
        
        public class SampleSemperlandCacheUser : MonoBehaviour
        {
            // Some accounts to have permissions at metaverse level.
            // Change them accordingly to make tests in your blockchain.
            private string[] managers =
            {
                "0x0849355a14264F51462CC068691fcFfE4F2dd259",
                "0x3889Def9bA5e4c75dDB5E12E63e9a84fDdf6116F"
            };

            // Some accounts to have permissions at brand level.
            // Change them accordingly to make tests in your blockchain.
            private string[] brandManagers =
            {
                "0x841518a249228b02Ec7F5292c9D0f6591ED1fdBa",
                "0x9c2E878836626aEa0F9dA9C1e8CA866F7084cEE6"
            };
            
            // The brands' owners. Change them accordingly to make tests in your blockchain.
            private string[] owners = {
                "0x5B6E99270D979F8844fF5423E91ba61ff40BB1C0",
                "0x1E573B9d781Ea5BFC80A420CB39154F96a93B376",
                "0xC5F998AC0752bEafDDa6bB4E94507A4312CB6509",
                "0xf45477B8Fd9761bC4566B94a998D85DA0342b1Cb",
                "0x1990F08AfE5dF650AbF4608c4a1257c89bD60f59"
            };
            
            // Start is called before the first frame update
            async void Start()
            {
                CacheResource resource = new CacheResource("http://localhost:8080");

                HashSet<string> allSponsors = new HashSet<string>();
                
                // Get the brands.
                BrandsResult brands = await resource.Brands(0, "");
                foreach (TokenMetadata metadata in brands.Brands)
                {
                    // Print the brand.
                    DebugObj(metadata, "Brand");
                    
                    // Print the brand's sponsors.
                    BrandSponsorsResult sponsors = await resource.BrandSponsors(metadata.Brand);
                    foreach (BrandSponsorsResult.BrandSponsor sponsor in sponsors.Sponsors)
                    {
                        // Print each sponsor.
                        DebugObj(sponsor, ">>> Sponsor");

                        // Adding the sponsor to the list.
                        allSponsors.Add(sponsor.Sponsor);
                    }
                    
                    // Print the brand tokens.
                    foreach (TokenMetadata subMetadata in (await resource.Tokens(metadata.Brand)).Tokens)
                    {
                        DebugObj(subMetadata, $">>> Token metadata (brand {metadata.Brand})");
                    }
                    
                    // Print the per-manager brand permissions.
                    foreach (var brandManager in brandManagers)
                    {
                        BrandPermissionsResult brandPermissions =
                            await resource.BrandPermissions(metadata.Brand, brandManager);
                        DebugObj(brandPermissions, $">>> Brand permissions (brand {metadata.Brand}, user {brandManager})");
                    }
                }
                
                // Print the metaverse's permissions.
                foreach (string manager in managers)
                {
                    MetaversePermissionsResult metaversePermissions = await resource.Permissions(manager);
                    DebugObj(metaversePermissions, $"Metaverse permissions (user {manager})");
                }
                
                // Print the metaverse's parameters.
                foreach (ParametersResult.Parameter parameter in (await resource.Parameters()).Parameters)
                {
                    DebugObj(parameter, "Parameter");
                }
                
                // Print all the metaverse's tokens.
                foreach (TokenMetadata metadata in (await resource.Tokens()).Tokens)
                {
                    DebugObj(metadata, "Token metadata (global)");
                }
                
                // Print the 0-brand tokens.
                foreach (TokenMetadata metadata in (await resource.Tokens("0x0000000000000000000000000000000000000000")).Tokens)
                {
                    DebugObj(metadata, "Token metadata (Brand 0)");
                }
                
                // Print the sponsors.
                foreach (string sponsor in allSponsors)
                {
                    SponsoredBrandsResult sponsoredBrands = await resource.SponsoredBrands(sponsor);
                    DebugObj(sponsoredBrands, "Sponsored brands for " + sponsor);
                }

                // Print the balances and deals.
                foreach (string owner in owners)
                {
                    BalancesResult balances = await resource.Balances(owner);
                    DebugObj(balances, $">>> Balances (owner: {owner})");

                    DealsResult deals = await resource.Deals(owner);
                    DebugObj(deals, $">>> Deals (owner: {owner}");
                }
            }

            private void DebugObj(Object obj, string label = "Deserialized")
            {
                string text = "";
                using (TextWriter textWriter = new StringWriter())
                using (JsonWriter writer = new JsonTextWriter(textWriter))
                {
                    JsonSerializer.Create().Serialize(writer, obj);
                    text = textWriter.ToString();
                }
                Debug.Log($"{label}: {text}");
            }
        }
    }
}
