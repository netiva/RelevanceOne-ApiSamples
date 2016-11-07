namespace RelevanceApiExamples
{
    public static class Constants
    {
        public const string BaseAddress = "https://login.relevanceone.com/identity";

        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";

        public const string RelevanceOneApiUrl = "https://int.relevanceone.com/";

        public const string ApiImportFootfallData = "api/data/footfall/footfall";
        public const string FootfallExampleData = @"{'name': 'Some name',
                                      'counts': [
                                        {
                                          'time': '2016-10-01T09:00:00.000Z',
                                          'count': '10'
                                        }
                                      ]
                                    }";


        public const string ApiImportPosDataForShop = "api/data/pos/shop";
        public const string PosExampleDataForShop = @"{
                                                      'shopId': 1,
                                                      'entries': [
                                                        {
                                                          'time': '2016-11-01T09:00:00.000Z',
                                                          'tickets': 20,
                                                          'sales': 40
                                                        }
                                                      ]
                                                    }";
        // Used for HMAC Authentication
        public const string AuthenticationScheme = "amx";
        public const string TestMachineKey = "4d53bce03ec34c0a911182d4c228ee6c";
        public const string TestSharedSecret = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
    }
}