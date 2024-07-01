# Excel Password Brute Force Tool

## Description

This tool is designed to brute force the password of an Excel file. It allows configuration of the types of characters to include in the password guesses and supports saving and resuming from previous attempts to ensure no passwords are retried.

## Features

- Configurable character sets (numbers, letters, uppercase, lowercase, special characters).
- Configurable password length range.
- Save and resume functionality to avoid retrying previously tested passwords.
- Real-time display of the number of password attempts.
- Dynamic console updates to provide a clear and concise view of the brute force progress.

## Prerequisites

- .NET Core 8
- ExcelDataReader library

## Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/MounsC/ExcelUnlocker.git
   cd ExcelUnlocker
   ```

2. Install dependencies:

   ```sh
   dotnet restore
   ```

3. Create the configuration file if it doesn't exist:

   ```sh
   dotnet run
   ```

   This will generate a default `config.json` file in the root directory.

## Configuration

Edit the `config.json` file to configure the character sets and password length range:

```json
{
  "IncludeNumbers": true,
  "IncludeLetters": true,
  "IncludeUpperCase": true,
  "IncludeLowerCase": true,
  "IncludeSpecialChars": false,
  "MinPasswordLength": 4,
  "MaxPasswordLength": 6
}
```

## Usage

1. Place your Excel files in the `Excel` directory at the root of the executable.

2. Run the program:

   ```sh
   dotnet run
   ```

3. Follow the prompts to select the Excel file you want to brute force.

## Code Structure

- **Configuration.cs**: Manages the creation and loading of the configuration file.
- **PasswordGenerator.cs**: Generates passwords based on the configuration.
- **ExcelHandler.cs**: Handles selection and opening of Excel files.
- **Program.cs**: Main entry point of the application, managing password attempts and displaying progress.

## Example

```sh
Tentatives de mot de passe:
1
...
Mot de passe trouvé: password123
```

## Contribution

Contributions are welcome! Please feel free to submit a Pull Request.
