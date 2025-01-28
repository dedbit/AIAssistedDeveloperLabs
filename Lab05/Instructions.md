


# 'Debug with copilot' on Lab05 tests for VSCode
- Open the EmployeeManagement solution in VSCode. 
- Open tests and run all tests
- Navigate to failing test, set marker inside test method and run Copilot prompt: @workspace /fixTestFailure
- Notice the steps Copilot takes to suggest a solution.  (Investigating the code aroud ...)
- Find failing test in test explorer
- Click the magic icon to fix the test
- Notice that the steps taken are the same.



# 'Debug with copilot' on Lab05 tests for Visual Studio
- Open the EmployeeManagement solution in Visual studio
- Open tests and run all tests
- Navigate to failing test, right click and select 'Debug with copilot'
- Notice that copilot sets break points
- Notice Copilot Identifies objects to monitor, inspects values
- Explain - Github copilot is context aware.
- Copilot may 'Suggest a code fix' or 'Continue debugging'
- When code is fixed, stop the debugger, and try again


# Create tests in Visual studio
- Open EmployeeManagement in Visual Studio
- Open EmployeeList.razor
- Copilot prompt: /tests create xunit tests for #employeeList.razor
- Check package reference to BUnit, project reference, 
- Run tests. 
- Run 'Debug with copilot' on failing test 
- Notice "Only managed languages are currently supported for this feature."


# Custom test generation instructions in VSCode
- Open VSCode
- Open user settings (settings.json)
- Set configuration of: testGeneration.instructions
```json
"github.copilot.chat.testGeneration.instructions": [
    
        {
            "file": ".copilot-test-instructions.md"
        }
    ]
```

- Edit .copilot-test-instructions.md and update content with the following:

```markdown
Always try uniting related tests in a suite.
Always name test methods with {methodundertest}_{scenario} format, all lower case.
Dont use var keyword, but use explicit types.
Use Xunit for unit tests.
Suggest installation of NuGet packages like Moq using dotnet add package <package-name>
Add a comment with a famous quote in start of each method
```

- Select a method or class
- Right click > Copilot > Generate tests
- Click 'View in chat'
- Notice in chat that .copilot-test-instructions.md is included as a reference
- Notice that the output respects the instructions
- Notice that package installations might be included in the output

- Change instructions in .copilot-test-instructions.md
- In Copilot chat window Prompt: @workspace /tests Generate tests for #selection
- Notice that test output reflects the changes from instructions

