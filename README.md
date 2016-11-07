# RelevanceOne API Samples


## Flow on the client side:

Client should build a string by combining all the data that will be sent, this string contains the following parameters (API key, HTTP method, request URI, request time stamp and nonce).

- API key will be a "string" provided by Relevance 
- HTTP Method can also be represented as a string from an allowed collection ("GET", "POST", "DELETE", "PUT")
- Request URI is the universal resource identifier or universal resource location (URL) string, which describes which method from the API is the client calling (ex. https://api.relevanceone.com/api/customers/customer/1)
- Request time stamp is calculated using UNIX time (number of seconds since Jan. 1st 1970) to overcome any issues related to a different time zone between client and server. The server will have a tolerance of +/- 60 seconds for any time difference.
- Nonce is an arbitrary number/string used only once which is generated by the client and send back unmodified to the client. It is used as a way for the client to know that on any send request a response is received and that it came from the server it is send to
		
### Client will need to:
- Convert the previously combined string to byte array using an UTF-8 Encoding
- Using the "Shared secret" as a Key for the HMACSHA256, the hash is computed from the previous calculated byte array. The result for this hash is a unique signature for this request
- The resulting hash is converted to a base64 string
- And finally it added in the Authorization header in the following order "amx ApiKey:TimeStamp:Nonce:HMACSHA256hash" 

The signature will be sent in the Authorization header using a custom scheme such as ”amx”. The data in the Authorization header will contain the API Key, request time stamp, and nonce separated by colon ‘:’. 

The format for the Authorization header will be like: [Authorization: amx APPId:Signature:Nonce:Timestamp].

### Client send the request as usual along with the data over a secure connection (HTTPS).
After the request is processed by the server the client receives a response stating if the operation was successful or not.
 
Along with the response, the server adds an Authorization header which is built in a similar way as previous, with the only difference that instead of an URL the client nonce is used.

This will allow the client additional security to make sure that the communication is secure, but the check it is not mandatory (it depends entirely on the client).

The client can then perform additional validation to check if the response is valid.
 
- Check if the received nonce matches the one that was send
- If the nonce is received back for the first time, then it should be memorized. If there already is a nonce in memory, then it might be the case of a replay attack.
- Generate a HMAC hash in a same manner as before with replacement of URL with the nonce, and then compare them with the HMAC hash send from the server

## Flow on the server side:

- Server receives all the data included in the request along with the Authorization header.
- Server extracts the values (API Key, Signature, Nonce and Request Time stamp) from the Authorization header.
- The server will be responsible to validate and verify this request, and if it is a non-valid request or a replay request, it will reject it.
- If the request does not conform to the rules the server will reject the request and returns HTTP status code 401 unauthorized.
