namespace FixerApiTests.PageObjects
{
    public class FixerApiPage
    {
        public string ApiKey { get; set; } = "d25cbe978ecf15c4ac7fe2de84a51694";
        public string InvalidApiKey { get; set; } = "d25cbe978ecf15c4ac7fe2de84a59999";
        public string Endpoint { get; set; } = "http://data.fixer.io/api/latest";
        public string Symbols { get; set; } = "USD,GBP";  // currency symbols
    }
}

