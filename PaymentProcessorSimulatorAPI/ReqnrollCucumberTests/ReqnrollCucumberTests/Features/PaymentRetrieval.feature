Feature: Payment Retrieval
  As a payment processor
  I want to retrieve payments
  So that I can view payment information

  Background:
    Given the Payment API is running

  @negative
  Scenario: Get a payment with non-existent ID
    Given I have a payment ID that does not exist
    When I request the payment by its ID
    Then the response should be not found
    And the response should contain an error
