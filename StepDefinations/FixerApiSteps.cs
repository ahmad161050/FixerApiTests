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

        public FixerApiSteps()
        {
            _fixerApiPage = new FixerApiPage();
        }

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


        [Given(@"user has a valid API key")]
        public void GivenUserHasAValidAPIKey()
        {
            Assert.IsNotNull(_fixerApiPage.ValidApiKey, "API Key is missing");
        }

        [When(@"user requests currency rates")]
        public async Task WhenUserRequestsCurrencyRates()
        {
            _response = await SendRequest("");
            string responseContent = await _response.Content.ReadAsStringAsync();
            _jsonResponse = JObject.Parse(responseContent);

            TestContext.WriteLine($"[API CALL] GET {_fixerApiPage.ValidEndpoint}?access_key=****");
            TestContext.WriteLine($"[RESPONSE] {responseContent}");
        }

        [Then(@"all currency rates should be displayed, the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
        {
            Assert.IsTrue((bool)_jsonResponse["success"], "API request failed");

            // Validate response structure
            Assert.IsNotNull(_jsonResponse["timestamp"], "Missing 'timestamp' field");
            Assert.IsNotNull(_jsonResponse["base"], "Missing 'base' field");
            Assert.IsNotNull(_jsonResponse["date"], "Missing 'date' field");
            Assert.IsNotNull(_jsonResponse["rates"], "Missing 'rates' field");

            TestContext.WriteLine($"[TEST PASSED] API responded successfully with full structure.");
        }

        [When(@"user requests currency rates of USD and GBP")]
        public async Task WhenUserRequestsCurrencyRatesWithSymbols()
        {
            _response = await SendRequest("&symbols=USD,GBP");
            _jsonResponse = JObject.Parse(await _response.Content.ReadAsStringAsync());
        }

        [Then(@"USD and GBP rates should be displayed, the response should be successful")]
        public void ThenTheResponseShouldContainRatesForUSDAndGBP()
        {
            Assert.IsTrue((bool)_jsonResponse["success"], "API request failed");

            string[] symbols = _fixerApiPage.Symbols.Split(','); // Fetch symbols from POM
            JObject rates = _jsonResponse["rates"].ToObject<JObject>();

            foreach (var symbol in symbols)
            {
                Assert.IsTrue(rates.ContainsKey(symbol), $"Missing rate for {symbol}");
            }

            Console.WriteLine($"Response JSON: {_jsonResponse.ToString()}");
        }


        [Given(@"user has an invalid API key")]
        public void GivenUserHasAnInvalidAPIKey()
        {
            _fixerApiPage.ValidApiKey = _fixerApiPage.InvalidApiKey;
        }

        [Then(@"the response should indicate an error with code 101")]
        public void ThenTheResponseShouldIndicateAnErrorWithCode101()
        {
            Assert.IsFalse((bool)_jsonResponse["success"], "API request should have failed");

            Assert.IsNotNull(_jsonResponse["error"], "Expected 'error' field in response");

            // Pretty-printing the JSON error response
            Console.WriteLine("Response JSON:");
            Console.WriteLine(_jsonResponse.ToString(Newtonsoft.Json.Formatting.Indented));

            TestContext.WriteLine($"[TEST PASSED] API returned expected error response.");
        }
        [Given(@"user tries to access an invalid API endpoint")]
        public void GivenUserTriesToAccessAnInvalidApiEndpoint()
        {
            _fixerApiPage.ValidEndpoint = _fixerApiPage.InvalidEndpoint;  // Set to invalid API endpoint
        }

        [When(@"user requests currency rates from the invalid endpoint")]
        public async Task WhenUserRequestsCurrencyRatesFromInvalidEndpoint()
        {
            _response = await SendRequest(""); // No additional parameters needed
        }

        [Then(@"the response should indicate an error with code 404")]
        public void ThenTheResponseShouldIndicateErrorWithCode404()
        {
            // Ensure response is not null
            Assert.IsNotNull(_response, "Response object is null");

            // Assert that the HTTP status code is 404 Not Found
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, _response.StatusCode,
                $"Expected 404 Not Found, but got {_response.StatusCode}");

            Console.WriteLine($"[TEST PASSED] API returned expected 404 Not Found error.");
        }

    }
}







