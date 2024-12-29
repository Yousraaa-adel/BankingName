# Banking System API

This project is a **Banking System API** built with **ASP.NET Core**, **Entity Framework Core**, and a **RESTful API** architecture. It allows you to perform operations such as deposits, withdrawals, transfers, account creation, and balance inquiries. The API supports multiple account types, including **Checking** and **Savings**, and implements basic error handling for different transaction scenarios.

## Features

- **Account Creation**: Create a new account with basic details like account type and initial balance.
- **Deposit**: Deposit funds into an account with validation for valid amounts.
- **Withdrawal**: Withdraw funds from an account, with overdraft protection for checking accounts.
- **Transfer**: Transfer funds between two accounts.
- **Balance Inquiry**: Retrieve the current balance of an account, including currency formatting.
- **Transaction Logging**: All operations are logged as transactions for future reference.
- **Currency Handling**: Balance amounts are returned with a currency symbol (`$`), and amounts are formatted to two decimal places.

## Technologies Used

- **ASP.NET Core** for building the API.
- **Entity Framework Core** for interacting with the database.
- **SQL Server** for database management.
- **AutoMapper** for object-to-object mapping between DTOs and entities.

## Setup and Installation

### Prerequisites

- **.NET 6.0 SDK** or higher.
- **SQL Server** or compatible database.
- **Visual Studio** or any code editor of your choice.

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/banking-system-api.git
   ```
2. Navigate to the project directory:
   ```bash
   cd banking-system-api
   ```
3. Restore the NuGet packages:

```bash
dotnet restore
```

4. Update your appsettings.json with your database connection string.
5. Run database migrations to create the necessary tables:

```bash
  dotnet ef database update
```

6. Start the API:

```bash
  dotnet run
```

### Database Setup (SQLite)

1. **SQLite Database Setup**:
   - Ensure you have **SQLite** installed on your system. If not, download and install it from the official website: [SQLite Download](https://www.sqlite.org/download.html).
2. **Configure the Database Connection**:

   - Open the `appsettings.json` file in the root directory of the project.
   - Locate the `ConnectionStrings` section and ensure that it is set to use SQLite as the database provider. For example:

     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=BankingSystem.db"
     }
     ```

   - This tells the application to use a file-based SQLite database (`BankingSystem.db`).

3. **Migrations and Database Creation**:

   - Open a terminal or command prompt and navigate to the project directory.
   - Use the following command to create the database and apply any migrations:

     ```bash
     dotnet ef database update
     ```

   - This will apply all the migrations and create the necessary tables in the SQLite database.

4. **Verify the Database**:
   - You can use a tool like [DB Browser for SQLite](https://sqlitebrowser.org/) to open the `BankingSystem.db` file and verify the structure and data.

The Banking System API provides a set of RESTful endpoints to manage accounts, perform transactions, and retrieve account information, including balances and transaction history.

## API Endpoints

### Account Endpoints

#### Create Account

- **POST** `/api/accounts/create`
- **Description**: Creates a new bank account.
- **Body**:
  ```json
  {
    "accountType": "Checking",
    "balance": "$1000"
  }
  ```

#### Deposit Funds

- **POST** `/api/accounts/deposit`
- **Description**: Deposits funds into a specified account.
- **Body**:
  ```json
  {
    "accountId": 1,
    "amount": "$500"
  }
  ```

#### Withdraw Funds

- **POST** `/api/accounts/withdraw`
- **Description**: Withdraws funds from a specified account.
- **Body**:
  ```json
  {
    "accountId": 1,
    "amount": "$200"
  }
  ```

#### Transfer Funds

- **POST** `/api/accounts/transfer`
- **Description**: Transfers funds between two accounts.
- **Body**:
  ```json
  {
    "sourceAccountId": 1,
    "targetAccountId": 2,
    "amount": "$300"
  }
  ```

#### Get Account Balance

- **GET** `/api/accounts/{id}/balance`
- **Description**: Retrieves the balance of a specified account.
- **Response**:
  ```json
  {
    "accountNumber": 1,
    "balance": "$1200.00"
  }
  ```

## Error Handling

- All errors are returned in a structured JSON format:
  ```json
  {
    "message": "Error message describing the problem"
  }
  ```
  - Common error scenarios include invalid inputs, insufficient funds for withdrawal, or account not found.

## How to Contribute

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-xyz`).
3. Make your changes and commit (`git commit -am 'Add feature xyz'`).
4. Push to your branch (`git push origin feature-xyz`).
5. Create a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

For any issues or improvements, please open an issue or submit a pull request.
