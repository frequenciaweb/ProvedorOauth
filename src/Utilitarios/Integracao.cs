using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilitarios
{
    public static class Integracao
    {
        public static async Task<DiscoveryDocumentResponse> RecuperaEndpoints()
        {
            using (var client = new HttpClient())
            {
                DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(Constantes.EnderecoIdentityServer);
                if (disco.IsError)
                {
                    throw new System.Exception(disco.Error);
                }
                return disco;
            }
        }

        public static async Task<string> RequisitarToken(string clienteID, string clientSecret, string scope)
        {
            var endpoint = RecuperaEndpoints().Result.TokenEndpoint;

            using (var client = new HttpClient())
            {
                TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = endpoint,
                        ClientId = clienteID,
                        ClientSecret = clientSecret,
                        Scope = scope
                    });

                if (tokenResponse.IsError)
                {
                    throw new Exception(tokenResponse.Error);
                }

                return tokenResponse.AccessToken;
            }
        }

        public static async Task<string> ChamarApi(string endpoint, string token = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.SetBearerToken(token);
                }

                HttpResponseMessage response = await client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return endpoint + " => Não autorizado";
                    }
                    //throw new Exception(response.StatusCode.GetHashCode() + " - " + response.StatusCode.ToString());
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return null;

        }
    }
}
