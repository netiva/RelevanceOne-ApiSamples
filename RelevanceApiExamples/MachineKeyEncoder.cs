using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace RelevanceApiExamples.Hmac
{
    /// <summary>
    /// This is a reference implementation of how HTTP requests are encoded with a machine key.
    /// </summary>
    public static class MachineKeyEncoder
    {
        public static void AddAuthorizationHeader(HttpRequestMessage request, string nonce, string timestamp)
        {
            // Calculate the needed hmac hash function
            var hash = GenerateHmacHash(request.Method.Method, request.RequestUri.AbsoluteUri, nonce, timestamp);
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, $"{Constants.TestMachineKey}:{timestamp}:{nonce}:{hash}");
        }

        public static string GenerateHmacHash(string requestMethod, string absoluteUrl, string nonce, string timestamp)
        {
            string hash;
            // Constructing a string that is going to be encrypted
            string signature = $"{Constants.TestMachineKey}{requestMethod}{absoluteUrl}{timestamp}{nonce}";

            // HMAC encryption algorithm
            using (var hmac = new HMACSHA256(Convert.FromBase64String(Constants.TestSharedSecret)))
            {
                hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signature)));
            }
            return hash;
        }
    }
}
