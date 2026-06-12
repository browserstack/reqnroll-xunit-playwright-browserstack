@sample-test
Feature: BStack demo shopping flow

  @sample-test
  Scenario: Add product to cart and verify
    Given I navigate to website
    Then I should see title StackDemo
    Then I add product to cart
    When I check if cart is opened
    Then I should see same product in cart
