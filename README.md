# Test UI
How to test in QA:
- navigate to WeBrick.Test.UI.QA in terminal
- run 'dotnet test' to run all tests or 'dotnet test --filter ' to run specific test (for example 'dotnet test --filter LoginFromFrontPage')

How to test in Production:
- navigate to WeBrick.Test.UI in terminal
- run 'dotnet test' to run all tests or 'dotnet test --filter ' to run specific test (for example 'dotnet test --filter LoginFromFrontPage')

**Right now we test the following:**
- Can you login?
- Can you logout?
- Is the footer correct?
- Is the frontpage correct?
- Is a /property page correct?

It would be nice to add tests for the loan application flow as well as claiming and unclaiming a home