# **Fixer API Testing with SpecFlow & NUnit**

## Project Overview
This project is a **C# automation testing framework** built using **SpecFlow** and **NUnit** to test the Fixer.io API. It verifies various API functionalities like:
- Fetching all currency exchange rates.
- Fetching specific exchange rates (USD, GBP).
- Handling invalid API key errors.
- Handling invalid API endpoints.

---

## **Technologies Used**
- **Programming Language:** C# (.NET 8.0)
- **Test Framework:** SpecFlow with NUnit
- **Dependency Management:** NuGet
- **HTTP Requests:** HttpClient
- **JSON Parsing:** Newtonsoft.Json
- **Test Runner:** dotnet test

---
## **Project Structure**
| Folder/File           | Description |
|----------------------|-------------|
| `FixerApiTests/`       | Root project directory |
| `Features/`           | Contains SpecFlow feature files |
| `FixerApi.feature`    | Feature file defining test scenarios |
| `PageObjects/`        | Page Object Model (POM) for API configurations |
| `FixerApiPage.cs`     | Stores API key, endpoint details |
| `StepDefinitions/`    | Step definition files mapping to SpecFlow steps |
| `FixerApiSteps.cs`    | Implements test logic |
| `FixerApiTests.csproj`| Project configuration and dependencies |



## **Setup & Installation**
### **1️ Prerequisites**
Ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio Code](https://code.visualstudio.com/) (Recommended)
- [SpecFlow Plugin for VS Code](https://marketplace.visualstudio.com/items?itemName=SpecFlowTeam.SpecFlow) (for BDD support)

### **2️ Clone the Repository**
```sh
git clone https://github.com/your-repository/FixerApiTests.git
cd FixerApiTests

### **3 Install Dependencies**
Run the following command to restore the required NuGet packages:
dotnet restore

### **4 Running Tests**
Running Tests:
dotnet test


