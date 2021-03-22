Feature: BookLoan
	In order to list books I will need to open the book loan application
	I want to see a list containing at least one book
	
@mytag
Scenario: List books
	Given I have opened the book loan application
	When I list the books
	Then I see a list of books

Scenario: Add a new book
	Given I have opened the book loan application
	And I have student access to the application
	And I have added a new book record 
	And I list the books
	Then I will see at least one book