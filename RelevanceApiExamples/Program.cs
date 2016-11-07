using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Runtime.Caching;

namespace RelevanceApiExamples.Hmac
{
    class Program
    {

        private static DateTime UnixEpoch = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        static void Main()
        {
            Console.WriteLine("Starting application!");

            Console.WriteLine();

            Console.WriteLine("Starting to send Footfall data!");
            PostFootfallCountData();
            Console.WriteLine("Finished with sending Footfall data!");

            Console.WriteLine();

            Console.WriteLine("Starting to send POS data!");
            PostPosData();
            Console.WriteLine("Finished with sending POS data!");

            Console.WriteLine();

            Console.WriteLine("Application finished. Press any ENTER to exit.");
            Console.ReadKey();

        }

        private static void PostFootfallCountData()
        {
            // Creating a post request message with URL pointing to the api method that is supposed to be called
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Constants.RelevanceOneApiUrl + Constants.ApiImportFootfallData);

            // Randomly generated string (could be anything that the client will recognize that the response is the same as the call it made)
            string nonce = Guid.NewGuid().ToString("N");

            // We adopt a convention of calculating the time starting from Unix epoch time
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            string timestampString = ((ulong)(timestamp - UnixEpoch).TotalSeconds).ToString();

            // Adding the authorization header with a specific nonce
            MachineKeyEncoder.AddAuthorizationHeader(request, nonce, timestampString);

            // Adding the content that is going to be posted and telling the request that the content that is send is in JSON format with UTF8 encoding
            request.Content = new StringContent(Constants.FootfallExampleData, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                // Actually sending the request
                // NOTE: Since the method is asynchronous, but we intentionally wait for
                HttpResponseMessage response = client.SendAsync(request).Result;

                bool result = ValidateResponse(response, nonce, request);

                //If we are sure that the response is coming from the server that we have send to, only then we process the response
                if (result)
                {
                    // Checking the status code of the returned response
                    // A successful response is with HTTP 200 status
                    Console.WriteLine(response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        // If the data is successfully saved, then a "Saved" message is returned
                        string content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(content);
                    }
                }
                else
                {
                    Console.WriteLine("The server response was not valid");
                }
            }
        }

        private static void PostPosData()
        {
            // Creating a post request message with URL pointing to the api method that is supposed to be called
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Constants.RelevanceOneApiUrl + Constants.ApiImportPosDataForShop);

            // Randomly generated string (could be anything that the client will recognize that the response is the same as the call it made)
            string nonce = Guid.NewGuid().ToString("N");

            // We adopt a convention of calculating the time starting from Unix epoch time
            // We adopt a convention of calculating the time starting from Unix epoch time
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            string timestampString = ((ulong)(timestamp - UnixEpoch).TotalSeconds).ToString();
            // Adding the authorization header with a specific nonce
            MachineKeyEncoder.AddAuthorizationHeader(request, nonce, timestampString);

            // Adding the content that is going to be posted and telling the request that the content that is send is in JSON format with UTF8 encoding
            request.Content = new StringContent(Constants.PosExampleDataForShop, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                // Actually sending the request
                // NOTE: Since the method is asynchronous, but we intentionally wait for
                HttpResponseMessage response = client.SendAsync(request).Result;

                bool result = ValidateResponse(response, nonce, request);

                //If we are sure that the response is coming from the server that we have send to, only then we process the response
                if (result)
                {
                    // Checking the status code of the returned response
                    // A successful response is with HTTP 200 status
                    Console.WriteLine(response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        // If the data is successfully saved, then a "Saved" message is returned
                        string content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(content);
                    }
                }
                else
                {
                    Console.WriteLine("The server response was not valid");
                }
            }
        }
        private static bool ValidateResponse(HttpResponseMessage response, string nonce, HttpRequestMessage request)
        {
            bool result = true;
            if (response.Headers.Contains(HttpRequestHeader.Authorization.ToString()))
            {
                // We search the headers collection for a header that contains "Authorization" as a key (field name)
                var authorizationHeaders = response.Headers.GetValues(HttpRequestHeader.Authorization.ToString());

                // We check that we have found the header and that it is exactly one
                if (authorizationHeaders != null && authorizationHeaders.Count() == 1)
                {
                    var authorizationHeaderValue = authorizationHeaders.First();

                    // The Authorization header must start with amx (convention for hmac authorization)
                    if (authorizationHeaderValue.StartsWith("amx"))
                    {
                        #region Extract the fields in the Authorization header
                        var elements = authorizationHeaderValue.Substring(3).Trim().Split(':');
                        if (elements.Length != 4)
                        {
                            result = false;
                        }
                        string serverSendApiKey = elements[0];
                        string serverSendTimeStamp = elements[1];
                        string serverSendNonce = elements[2];
                        string serverSendHash = elements[3];
                        #endregion

                        #region Nonce validation
                        // Compare the nonce that was send with the nonce it was received in the response
                        // If a specific algorithm is used to generate the nonce, than that can be checked also
                        // This may be enough just to check if it is the right response
                        if (serverSendNonce != nonce)
                        {
                            result = false;
                        }
                        #endregion

                        #region Replay attack prevention
                        // Check if we have already made a request with the same nonce
                        // This will prevent replay attacks
                        if (MemoryCache.Default.Contains(nonce))
                        {
                            result = false;
                        }
                        else
                        {
                            // now that it's , prohibit replays for MaxTimestampOff after timestamp
                            MemoryCache.Default.Add(nonce, string.Empty, DateTimeOffset.Now.AddMinutes(5));
                        }
                        #endregion

                        #region Hash comparison
                        // Additionally the response Authorization header contains a HMAC hash of the send nonce (instead of the URL)
                        // So for additional security we could validate if the send hash is correct
                        string recomputedHash = MachineKeyEncoder.GenerateHmacHash(request.Method.Method, nonce, nonce, serverSendTimeStamp);
                        if (serverSendHash != recomputedHash)
                        {
                            result = false;
                        }
                        #endregion
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
