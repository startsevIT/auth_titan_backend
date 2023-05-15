using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace auth_titan_backend
{
	public class Configuration
	{
		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new ApiScope("TitanWebApi","Web API")
			};

		public static IEnumerable<IdentityResource> IdentityResources =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile()
			};

		public static IEnumerable<ApiResource> ApiResources =>
			new List<ApiResource>
			{
				new ApiResource("TitanWebApi","Web API", new []
				{JwtClaimTypes.Name})
				{
					Scopes = {"TitanWebApi"}
				}
			};

		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId= "titan-web-api",
					ClientName = "Titan Api",
					AllowedGrantTypes= GrantTypes.Code,
					RequireClientSecret= false,
					RequirePkce= true,
					RedirectUris =
					{
						""
					},
					AllowedCorsOrigins =
					{
						""
					},
					PostLogoutRedirectUris=
					{
						""
					},
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"TitanWebAPI"
					},
					AllowAccessTokensViaBrowser= true
				}
			};
	}
}
