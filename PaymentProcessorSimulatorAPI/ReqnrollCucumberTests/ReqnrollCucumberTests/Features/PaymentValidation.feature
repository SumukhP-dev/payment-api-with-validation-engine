Feature: Payment Validation
  As a payment processor
  I want to validate payments using the C++ validation engine
  So that only valid payments are processed

  Background:
    Given the Payment API is running

  @validation
  Scenario: Validate payment with all valid fields
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | Alice Johnson| 500.00 | USD      |
    When I create the payment
    Then the C++ validation engine should validate the payment
    And the payment should be created successfully

  @validation
  Scenario: Validate payment with empty customer name
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      |              | 100.00 | USD      |
    When I create the payment
    Then the C++ validation engine should reject the payment
    And the validation error should mention customer name

  @validation
  Scenario: Validate payment with negative amount
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | Bob Wilson   | -25.00 | USD      |
    When I create the payment
    Then the C++ validation engine should reject the payment
    And the validation error should mention amount

  @validation
  Scenario: Validate payment with invalid currency
    Given I have a payment with the following details:
      | CustomerName | Amount | Currency |
      | Carol Davis  | 100.00 | XYZ      |
    When I create the payment
    Then the C++ validation engine should reject the payment
    And the validation error should mention currency

