Feature: Fixer API Currency Rates

  # As a user of Fixer API, user wants to fetch all currency rates.
  Scenario: Fetch all currency rates with a valid API key
    Given user has a valid API key
    When user requests currency rates
    Then all currency rates should be displayed, the response should be successful

  # As a user of Fixer API, user wants to fetch some specific currency rates.
  Scenario: Fetch specific currency rates of USD and GBP
    Given user has a valid API key
    When user requests currency rates of USD and GBP
    Then USD and GBP rates should be displayed, the response should be successful

  # When user fetches the currency rates with an invalid API key, user should get an error of invalid API.
  Scenario: Request currency rates with an invalid API key
    Given user has an invalid API key
    When user requests currency rates
    Then the response should indicate an error of invalid API

  # When user fetches the currency rates with an invalid url/endpoint, user should get an error of invalid endpoint.
  Scenario: Request currency rates from an invalid endpoint
    Given user tries to access an invalid API endpoint
    When user requests currency rates from the invalid endpoint
    Then the response should indicate an error of invalid endpoint

  # When user fetches the currency rates with an unknwon url/endpoint, user should get an error(404).
  Scenario: Request currency rates from an unknwon endpoint
    Given user tries to access an unknwon API endpoint
    When user requests currency rates from the unknown endpoint
    Then the response should indicate an error with code 404

  # Additional Tests

  # When user requests currency rates without providing an API key, user should get an error of missing API key.
  Scenario: Request currency rates without an API key
    Given user does not provide an API key
    When user requests currency rates
    Then the response should indicate an error of missing API key

  # When user requests currency rates using an unsupported base currency, user should get an error.
  Scenario: Request currency rates with an unsupported base currency
    Given user sets the base currency to USD
    When user requests currency rates
    Then the response should indicate an error of unsupported base currency
