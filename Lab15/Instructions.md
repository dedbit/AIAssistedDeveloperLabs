# Scripting AI assistance

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
- Focus on suggested change, click the Ellipsis (...) and select "Apply In editor"
- Observe the change(s) in editor and select 'Accept changes' for each
- Run the script again and observe that errors are fixed, or reiterate.

