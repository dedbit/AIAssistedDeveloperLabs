# Scripting AI assistance

## Introduction to the Lab

In this lab, you will learn how to debug a PowerShell script using Visual Studio Code and GitHub Copilot. The objective is to identify and resolve errors in a provided script by leveraging AI-assisted development features. You will gain hands-on experience with observing error output, analyzing issues, and applying automated fixes suggested by GitHub Copilot.

By the end of this lab, you should:

- Understand how to use GitHub Copilot's contextual awareness for debugging.
- Apply suggested fixes to resolve script errors.
- Develop a workflow for iterative debugging and improvement using AI tools.

### GitHub Copilot

Follow the steps outlined below to complete the lab.

- In Visual studio Code, open the DemoScript.ps1
- Press Ctrl + A to select the entire script
- Execute the selected script in terminal, by pressing Ctrl + Shift + P and typing "Terminal: Run Selected Text In Active Terminal"
- Observe errors in the output. 
- Press Ctrl + Alt + I to open the AI assisted developer
- Enter the following prompt: "list the errors from #terminalLastCommand"
- Observe that GitHub copilot understands the context. 
- Enter the following prompt: "/explain errors with #terminalLastCommand"
- Observe that GitHub copilot explains the errors in the output.
- Select specific error output in the terminal
- Enter the following prompt: "/fix #terminalSelection"
- Focus on one of the suggested changes, click the Ellipsis (...) and select "Apply In editor"
- Observe the change(s) in editor and select 'Accept changes' for each
- Run the script again and observe that errors are fixed, or reiterate.


### gh copilot


### Powertoys paste

