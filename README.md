# Cirara

Cirara (Commits in Repository as REST API) provides a JSON log of commits from Git repositories.

Inspired by https://commits.facepunch.com/

- Configurable via dotnet secrets. (https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=linux)
- Simple syntax (https://api:port/r/{repoName}/{branch=YourBranch}/{commit=SHA1HashHere})

## Requirements

- **.NET Core** Runtime 3.1
- An **Internet Connection** to download *NuGet packages* for building the project

## Installation

- Clone the repository

```sh
git clone https://github.com/Regensturm/Cirara.git
```
- Add Config / Secrets
```sh
dotnet user-secrets set "Config:DefaultBranch" "main"
dotnet user-secrets set "repoName:Url" "https://github.com/yourname/yourrepo.git"
dotnet user-secrets set "repoName:User" "GithubUser"
dotnet user-secrets set "repoName:Pass" "GithubAppToken"
```
Keep in mind that you **need** a Personal Access Token for accessing (private) repositories.
Learn more here: https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token

- Open a (Power)Shell / Terminal
- Build the project

```sh
dotnet build --configuration Release
```
- Start the service

```sh
dotnet run --project .\Cirara\Cirara.csproj
```

## Project Structure

This project was created with the Microsoft ASP .NET Core API template. For reference on how to work with this (e.g. adding controllers) please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio
