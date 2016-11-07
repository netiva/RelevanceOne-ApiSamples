using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using RelevanceApiExamples;

namespace RelevanceApiExample.Identity
{
    class Program
    {
        static void Main(string[] args)
        {
            var response = RequestToken();
            ShowResponse(response);

            Console.ReadLine();
            CallService(response.AccessToken);
        }

        static TokenResponse RequestToken()
        {
           var client = new TokenClient(
                Constants.TokenEndpoint,
                "ra_identity",
                "r1a2n3d4o5m");


            return client.RequestResourceOwnerPasswordAsync("name@mail.com","SeCur3PassW0rd", "permissions openid").Result;
        }

        static void CallService(string token)
        {
            var baseAddress = Constants.RelevanceOneApiUrl;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = client.GetStringAsync(Constants.ApiImportFootfallData).Result;

            Console.WriteLine("Service claims:");
            Console.WriteLine(JArray.Parse(response));
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                Console.WriteLine("Token response:");
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    Console.WriteLine("\nAccess Token (decoded):");

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))));
                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))));
                }
            }
            else
            {
                if (response.IsError)
                {
                    Console.WriteLine("HTTP error: ");
                    Console.WriteLine(response.Error);
                    Console.WriteLine("HTTP error reason: ");
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    Console.WriteLine("Protocol error response:");
                    Console.WriteLine(response.Json);
                }
            }
        }
    }
}
