

# Hands-On Experience Guide: EmployeeManagement Project

## Objective:

Guide users through setting up the **EmployeeManagement** project, experimenting with prompts to improve the `EmployeeRepository` class, and observing the modified output.

## Pre-requisites:

- Visual Studio 2022 and VSCode installed.
- GitHub Copilot enabled and configured.
- Lab 05 project files set up and accessible.


## Step 1: Setup the Environment
1. **Open the Project**
    - Open the **EmployeeManagement** project in Visual Studio 2022.
    - Ensure GitHub Copilot settings are configured:
        - Navigate to `Tools > Options > GitHub > Copilot`.
        - Check "Enable custom instructions to be loaded from .github/copilot-instructions.md files."
2. **Check Copilot Settings in VSCode**
    - Open the **EmployeeManagement** project in VSCode.
    - Click the Copilot status icon (bottom right) and select `Edit settings`.
    - Ensure "GitHub &gt; Copilot &gt; Chat &gt; Code Generation: Use Instruction Files Preview" is checked.


## Step 2: Experiment with Prompts on EmployeeRepository
1. **Optimize Code**
    - Select the `DeleteAsync` method in `EmployeeRepository.cs`.
    - Prompt: `/optimize`.
    - Observe changes, such as restructuring error handling or removing redundant logic.


## Step 3: Enhance Code Readability
1. **Rename Variables and Methods**
    - In Visual Studio, select a variable in the `DeleteAsync` method and press `Ctrl + R + R`.
    - Notice that Copilot suggests meaningful variable names.
    - Preview and apply suggestions for descriptive naming.
    - In VSCode, use `F2` to rename variables for similar functionality.
2. **Request Descriptive Naming**
    - Select the `DeleteEmployee` method.
    - Prompt: `"Can you suggest more descriptive names for the variables and functions in the selected code?"`
    - Preview changes and apply relevant improvements.


## Step 4: Modify Instructions
1. **Customize Copilot Instructions**
    - Open `.github/copilot-instructions.md` in the project directory.
    - Add: `"Modify from 'Dont use var keyword, but use explicit type names' to 'Use explicit type declaration instead of var keyword'"`
    - Save the file and return to `EmployeeRepository.cs`.
    - Select `DeleteAsync` and prompt: `/optimize`.
    - Observe the changes with the customized comments.


## Step 5: Explore Code Generation and Fixes
1. **Generate Fix Suggestions**
    - Open `EmployeeRepository.cs` in VSCode.
    - Prompt: `@workspace /fix`.
    - Review suggested fixes for potential improvements.


## Step 6: Review Output
1. Run the modified project in Visual Studio and verify the changes in functionality.
2. Check if the applied improvements enhance readability, maintainability, and performance.

