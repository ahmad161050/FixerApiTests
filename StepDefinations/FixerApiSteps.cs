using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using NUnit.Framework;
using FixerApiTests.PageObjects;

namespace FixerApiTests.Steps
{
    [Binding]
    public class FixerApiSteps
    {
        private HttpResponseMessage _response;
        private JObject _jsonResponse;
        private readonly FixerApiPage _fixerApiPage;

        // Constructor initializing FixerApiPage object
        public FixerApiSteps()
        {
            _fixerApiPage = new FixerApiPage();
        }

        /// Sends a GET request to the Fixer API.
        /// <param name="queryParams">Additional query parameters to append to the API URL.</param>
        /// <returns>Returns the HttpResponseMessage object from the API request.</returns>
        private async Task<HttpResponseMessage> SendRequest(string queryParams)
        {
            string requestUrl = $"{_fixerApiPage.ValidEndpoint}?access_key={_fixerApiPage.ValidApiKey}{queryParams}";

            Console.WriteLine($"[REQUEST] GET {requestUrl}");  // Logging request URL

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                Console.WriteLine($"[RESPONSE] Status Code: {response.StatusCode}"); // Logging status

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[RESPONSE BODY] {responseBody}"); // Logging response body

                return response;
            }
        }

        // Scenario 1: Fetch all currency rates with a valid API key

        [Given(@"user has a valid API key")]
        public void GivenUserHasAValidAPIKey()
        {
            Assert.IsNotNull(_fixerApiPage.ValidApiKey, "API Key is missing");
        }

        [When(@"user requests currency rates")]
        public async Task WhenUserRequestsCurrencyRates()
        {
            _response = await SendRequest("");

        }

        [Then(@"all currency rates should be displayed, the response should be successful")]
        public async Task ThenTheResponseShouldBeSuccessfulAsync()
        {
            string responseContent = await _response.Content.ReadAsStringAsync();
            _jsonResponse = JObject.Parse(responseContent);

            TestContext.WriteLine($"[API CALL] GET {_fixerApiPage.ValidEndpoint}?access_key=****");
            TestContext.WriteLine($"[RESPONSE] {responseContent}");
            Assert.IsTrue((bool)_jsonResponse["success"], "API request failed");

            // Validate response structure
            Assert.IsNotNull(_jsonResponse["timestamp"], "Missing 'timestamp' field");
            Assert.IsNotNull(_jsonResponse["base"], "Missing 'base' field");
            Assert.IsNotNull(_jsonResponse["date"], "Missing 'date' field");
            Assert.IsNotNull(_jsonResponse["rates"], "Missing 'rates' field");

            TestContext.WriteLine($"[TEST PASSED] API responded successfully with full structure.");
        }

        // Scenario 2: Fetch specific currency rates (USD & GBP)

        [When(@"user requests currency rates of USD and GBP")]
        public async Task WhenUserRequestsCurrencyRatesWithSymbols()
        {
            _response = await SendRequest("&symbols=USD,GBP");

        }

        [Then(@"USD and GBP rates should be displayed, the response should be successful")]
        public async Task ThenTheResponseShouldContainRatesForUSDAndGBP()
        {
            _jsonResponse = JObject.Parse(await _response.Content.ReadAsStringAsync());
            Assert.IsTrue((bool)_jsonResponse["success"], "API request failed");

            string[] symbols = _fixerApiPage.Symbols.Split(','); // Fetch symbols from POM
            JObject rates = _jsonResponse["rates"].ToObject<JObject>();

            foreach (var symbol in symbols)
            {
                Assert.IsTrue(rates.ContainsKey(symbol), $"Missing rate for {symbol}");
            }

            Console.WriteLine($"Response JSON: {_jsonResponse.ToString()}");
        }

        // Scenario 3: Request currency rates with an invalid API key


        [Given(@"user has an invalid API key")]
        public void GivenUserHasAnInvalidAPIKey()
        {
            _fixerApiPage.ValidApiKey = _fixerApiPage.InvalidApiKey;
        }
        [Then(@"the response should indicate an error of invalid API")]
        public async Task ThenTheResponseShouldIndicateAnErrorOfInvalidAPI()
        {
            Assert.IsNotNull(_response, "Response object is null.");
            string responseContent = await _response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(responseContent), "Response content is empty.");

            // Parse the JSON response
            _jsonResponse = JObject.Parse(responseContent);

            Assert.IsTrue(_jsonResponse.ContainsKey("success"), "Response does not contain 'success' field.");
            Assert.IsFalse((bool)_jsonResponse["success"], "API request should have failed.");
            Assert.IsTrue(_jsonResponse.ContainsKey("error"), "Expected 'error' field in response.");

            // Log the response
            Console.WriteLine("Response JSON:");
            Console.WriteLine(_jsonResponse.ToString(Newtonsoft.Json.Formatting.Indented));
        }


        // Scenario 4: Request currency rates from an invalid endpoint

        [Given(@"user tries to access an invalid API endpoint")]
        public void GivenUserTriesToAccessAnInvalidApiEndpoint()
        {
            _fixerApiPage.ValidEndpoint = _fixerApiPage.InvalidEndpoint;
        }

        [When(@"user requests currency rates from the invalid endpoint")]
        public async Task WhenUserRequestsCurrencyRatesFromTheInvalidEndpoint()
        {
            _response = await SendRequest("");
        }

        [Then(@"the response should indicate an error of invalid endpoint")]
        public async Task ThenTheResponseShouldIndicateErrorOfInvalidEndpoint()
        {

            var responseContent = await _response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseContent);

            Assert.IsFalse((bool)responseJson["success"], "Expected API failure");

            Console.WriteLine("[ERROR DETECTED] You have provided an invalid API endpoint.");
        }

        // Scenario 5: Request currency rates from an unknown endpoint (404)

        [Given(@"user tries to access an unknwon API endpoint")]
        public void GivenUserTriesToAccessAnUnknwonAPIEndpoint()
        {
            _fixerApiPage.ValidEndpoint = _fixerApiPage.UnknownEndpoint;
        }

        [When(@"user requests currency rates from the unknown endpoint")]
        public async Task WhenUserRequestsCurrencyRatesFromTheUnknownEndpoint()
        {
            _response = await SendRequest("");
        }

        [Then(@"the response should indicate an error with code 404")]
        public void ThenTheResponseShouldIndicateErrorWithCode404()
        {
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, _response.StatusCode,
                $"Expected 404 Not Found, but got {_response.StatusCode}");

            Console.WriteLine($"[TEST PASSED] API returned expected 404 Not Found error.");
        }

        // Scenario 6: Request currency rates without an API key

        [Given(@"user does not provide an API key")]
        public void GivenUserDoesNotProvideAnAPIKey()
        {
            // TODO: Implement step to make an API request without an API key
        }

        [Then(@"the response should indicate an error of missing API key")]
        public void ThenTheResponseShouldIndicateAnErrorOfMissingAPIKey()
        {
            // TODO: Implement validation for missing API key error response
        }


        // Scenario 7: Request currency rates with an unsupported base currency

        [Given(@"user sets the base currency to USD")]
        public void GivenUserSetsTheBaseCurrencyToUSD()
        {
            // TODO: Implement step to set base currency to an unsupported currency (e.g., USD)
        }

        [Then(@"the response should indicate an error of unsupported base currency")]
        public void ThenTheResponseShouldIndicateAnErrorOfUnsupportedBaseCurrency()
        {
            // TODO: Implement validation for unsupported base currency error response
        }

    }
}







