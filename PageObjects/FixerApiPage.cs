namespace FixerApiTests.PageObjects
{
    public class FixerApiPage
    {
        public string ValidApiKey { get; set; } = "c30ff8d2e8aea481654117b92b71025c"; //enter your key here
        public string InvalidApiKey { get; set; } = "d25cbe978ecf15c4ac7fe2de84a59999";
        public string ValidEndpoint { get; set; } = "http://data.fixer.io/api/latest";
        public string InvalidEndpoint { get; set; } = "https://jsonplaceholder.typicode.com/invalid-route";
        public string Symbols { get; set; } = "USD,GBP";  // currency symbols
    }
}