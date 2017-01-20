namespace RelevanceApiExamples
{
    public static class Constants
    {
        #region Identity server addresses (urls)
        private const string BaseAddress = "https://login.relevanceone.com/identity";
        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";
        #endregion

        #region API address (url)
        public const string RelevanceOneApiUrl = "https://int.relevanceone.com/";
        #endregion

        #region HMAC Authentication constants
        public const string AuthenticationScheme = "amx";
        public const string TestMachineKey = "4d53bce03ec34c0a911182d4c228ee6c";
        public const string TestSharedSecret = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
        #endregion

        #region Methods addresses (urls) and data which is send
        public const string ApiImportFootfallData = "footfall";
        public const string FootfallExampleData = @"{'name': 'Some name',
                                                     'counts': [
                                                                {
                                                                  'time': '2016-10-01T09:00:00.000Z',
                                                                  'count': '10'
                                                                }
                                                              ]
                                                            }";

        public const string ApiImportSalesDataForShop = "sales";
        public const string SalesExampleDataForShop = @"[
                                                          {
                                                            'storeCode': 'A1',
                                                            'hour': '2017-01-18T10:00:00.000+01:00',
                                                            'tickets': '45',
                                                            'amount': '877.5'
                                                          }
                                                        ]";


        public const string ApiImportMediaClipsData = "media/clips";
        public const string MediaExampleClipData = @"[
                                                      {
                                                        'id': '12345',
                                                        'name': 'Some name',
                                                        'tags': [
                                                          'tags_name'
                                                        ]
                                                        }
                                                    ]";

        #endregion
    }
}