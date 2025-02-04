# GitHub Portfolio API

## Introduction
This project is a .NET 7 Web API that provides an interface for fetching and searching GitHub repositories using the Octokit library. It allows users to retrieve a portfolio of repositories and search for repositories by name, language, and user.

## Technologies Used
- **.NET 7**
- **C#**
- **Octokit (GitHub API Client for .NET)**
- **ASP.NET Core Web API**
- **Dependency Injection**

## Features
- Fetch all repositories for a specified GitHub user.
- Search repositories by name, programming language, and owner.
- Secure authentication using a GitHub token.

## Installation & Setup
### Prerequisites
- .NET 7 SDK installed
- GitHub personal access token with necessary permissions

### Configuration
1. Clone the repository:
   ```sh
   git clone https://github.com/your-repository.git
   cd your-repository
   ```
2. Create an `secrets.json` file and add the following configuration:
   ```json
   {
     "GitHubSettings": {
       "UserName": "your-github-username",
       "Token": "your-github-token"
     }
   }
   ```
3. Restore dependencies and build the project:
   ```sh
   dotnet restore
   dotnet build
   ```

## Running the API
Start the application with:
```sh
   dotnet run
```
The API will be available at `https://localhost:5001`.

## API Endpoints
### 1. Get Portfolio Repositories
**Endpoint:** `GET /api/github/portfolio/{username}`  
**Description:** Retrieves all repositories for the specified GitHub user.

### 2. Search Repositories
**Endpoint:** `GET /api/github/search`  
**Query Parameters:**
- `repoName` (optional) - Repository name
- `language` (optional) - Programming language
- `userName` (optional) - GitHub username

**Example:**
```sh
GET /api/github/search?repoName=dotnet&language=C%23&userName=microsoft
```

## Dependencies
- `Octokit` - GitHub API client for .NET
- `Microsoft.Extensions.Configuration` - For app settings management

