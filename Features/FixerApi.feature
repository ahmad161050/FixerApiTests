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

  # When user fetches the currency rates with an invalid API key, user should get an error (101).
  Scenario: Request currency rates with an invalid API key
    Given user has an invalid API key
    When user requests currency rates
    Then the response should indicate an error with code 101

  # When user fetches the currency rates with an invalid url/endpoint, user should get an error(404).
  Scenario: Request currency rates from an invalid endpoint
    Given user tries to access an invalid API endpoint
    When user requests currency rates from the invalid endpoint
    Then the response should indicate an error with code 404
