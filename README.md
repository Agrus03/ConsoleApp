# Custom JSON Parser in C# (Zero Dependencies)

## Overview
This project is a custom, lightweight JSON deserializer built strictly from scratch in C#. It demonstrates fundamental programming concepts by parsing a complex, nested JSON file (`Transaction.json`) into strongly-typed C# objects (POCOs) **without using any external libraries** like `Newtonsoft.Json` or `System.Text.Json`.

## Technical Approach
The parser utilizes a **Finite State Machine (FSM)** algorithm to process the document line by line. 

Key architectural steps include:
1. **File I/O:** Reading the file directly into memory using `System.IO`.
2. **Sanitization:** Trimming whitespace and filtering out structural noise (empty lines, array brackets).
3. **Context Tracking (State Machine):** Using a `currentContext` tracker to dynamically switch states (`root`, `balance`, `transactions`, `metadata`) based on JSON object keys.
4. **Object Lifecycle Management:** Intercepting `{` and `}` characters to instantiate new objects (like `Transaction` or `Metadata`) and append them to generic Collections (`List<T>`) when the object scope closes.
5. **Tokenization & Mapping:** Splitting key-value pairs, removing structural characters (quotes, commas), and mapping raw strings to their respective C# class properties. It includes safe type conversions, such as parsing strings to `decimal` using `InvariantCulture` for financial data.

## Project Structure
* `Models.cs`: Contains the Data Models (`Wallet`, `Balance`, `Transaction`, `Metadata`).
* `Program.cs`: Contains the core FSM parsing logic and mapping engine.
* `Transaction.json`: The sample data file representing a digital wallet with a balance and a history of transactions.

## Why this project?
While modern C# provides excellent built-in JSON tools, building a custom parser is an excellent exercise in understanding:
* String manipulation and sanitization.
* Algorithm design (State Machines).
* Reference types, object instantiation, and memory management.
* Handling NullReferenceExceptions and edge cases in data structures.

## How to Run
1. Ensure you have the .NET SDK installed.
2. Clone this repository.
3. Run the project from your IDE (like Rider or Visual Studio) or via CLI:
   ```bash
   dotnet run
