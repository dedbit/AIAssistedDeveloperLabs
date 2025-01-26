# Lab 08: Bug fixing instructions

# Copilot understands locals
- Open Lab08 console app in Visual Studio
- Open Program.cs
- Set breakpoint at "int value = Int32.Parse(args[0]);"
- F5 debug
- On active breakpoint, press alt + ' to open Copilot inline chat
- Prompt: #locals what is the value of args
- Notice that chat understands the current value. 

- prompt: Why is the value string[0] ?
- Notice we get an explanation of the current value.
- Notice suggested follow up question. E.g. What happens if I try to access 'args[0]' when 'args' is empty?
- Click the suggestion and observe the response.
- If a code suggestion is shown click 'Preview'
- Evaluate if suggestion will work and click 'Apply'

- On active breakpoint, hover over args and click the 'Ask Coppilot' button
- Observe that the response answers based on the values of the args variable

# Copilot understands exceptions
- Press F10 to step over the line
- Observe that exception is caught and 'Exception unhandles' dialog is shown
- Click the 'Ask Copilot' button

# Conditional breakpoints
- Right click in margin for the line 'names.Add("Name: " + item);' and select 'Insert conditional breakpoint'
- Click in the input box for conditional expression
- Notice: 'Asking copilot for suggestions' message is shown shortly
- Notice: 3 suggestions provided
- Select 'Item == "John"'

- Right click 'ConsoleApp' and select 'Properties'
- Navigate to Debug > Open debug launch profiles UI
- Insert sample values like: 5, John, Lisa
- Restart the debugger
- Notice that breakpoint is hit only when item is "John"

# IEnumerable tabular visualizer
- Ensure that breakpoint is set for: 'names.Add("Name: " + item);'
- Start debugging using F5
- Hover mouse over 'names' variable and click the IEnumerable visualizer icon
- Click the Copilot icon
- Notice the suggesions
- Prompt: Get items that start with name, and that doesnt contain numbers
- Click 'Show in Visualizer'
- Notice the results are filtered