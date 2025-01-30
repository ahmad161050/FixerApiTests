namespace FixerApiTests.PageObjects
{
    public class FixerApiPage
    {
        public string ValidApiKey { get; set; } = "5b87d05611a0969fd78b249882acebcd"; //enter your key here
        public string InvalidApiKey { get; set; } = "d25cbe978ecf15c4ac7fe2de84a59999";
        public string ValidEndpoint { get; set; } = "http://data.fixer.io/api/latest";
        public string InvalidEndpoint { get; set; } = "http://data.fixer.io/api/hey-this-is-some-invalid-end-point";
         public string UnknownEndpoint { get; set; } = "https://jsonplaceholder.typicode.com/invalid-route";
        public string Symbols { get; set; } = "USD,GBP";  // currency symbols
    }
}