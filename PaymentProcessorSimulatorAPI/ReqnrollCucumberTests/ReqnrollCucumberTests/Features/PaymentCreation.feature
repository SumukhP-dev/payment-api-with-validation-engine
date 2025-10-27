Feature: Payment Creation
  As a payment processor
  I want to create payments
  So that I can process customer transactions

  Background:
    Given the Payment API is running

  @positive
  Scenario: Create a valid payment
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | John Doe     | 100.50 | USD      |
    When I create the payment
    Then the payment should be created successfully
    And the response should contain the payment details
    And the payment status should be "Pending"

  @positive
  Scenario: Create a payment with different currency
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | Jane Smith   | 250.75 | EUR      |
    When I create the payment
    Then the payment should be created successfully
    And the response should contain the payment details
    And the payment currency should be "EUR"

  @negative
  Scenario: Create a payment with empty customer name
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      |              | 100.50 | USD      |
    When I create the payment
    Then the payment should be rejected
    And the response should contain a validation error

  @negative
  Scenario: Create a payment with negative amount
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | John Doe     | -50.00 | USD      |
    When I create the payment
    Then the payment should be rejected
    And the response should contain a validation error

  @negative
  Scenario: Create a payment with invalid currency
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | John Doe     | 100.50 | INVALID  |
    When I create the payment
    Then the payment should be rejected
    And the response should contain a validation error

  @negative
  Scenario: Create a payment with zero amount
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | John Doe     | 0.00   | USD      |
    When I create the payment
    Then the payment should be rejected
    And the response should contain a validation error
