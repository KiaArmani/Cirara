# Cirara
Cirara (Commits in Repository as REST API) provides a JSON log of commits from Git repositories. 

- Users have animals 
- Stroking animals makes them happy  
- Feeding animals makes them less hungry 
- Animals start “neutral” on both metrics 
- Happiness decreases over time / hunger increases over time (even when the user is offline) 
- Users can own multiple animals of different types 
- Different animal types have metrics which increase/decrease at different rates

## Requirements

- **.NET Core** Runtime 3.1
- An **Internet Connection** to download *NuGet packages* for building the project
- A **MongoDB** instance (If you don't have one you can either get free cloud instance [here](https://www.mongodb.com/cloud/atlas/register) or [install a local server](https://docs.mongodb.com/manual/installation/))
- Environment Variables

Name|Description|Example Value
----------|----------|----------
MT_MONGODB_CONNECTION|[MongoDB Connection String](https://docs.mongodb.com/manual/reference/connection-string/)|mongodb://mongodb0.example.com:27017
MT_MONGODB_DATABASE|Name of the database to connect to|tamagotchi
MT_MONGODB_COLLECTION|Name of the collection that is being used|data

## Installation

- Clone the repository

```sh
git clone https://github.com/KiaArmani/PetGameBackend.git
```

- Open a (Power)Shell / Terminal
- Build the project

```sh
dotnet build --configuration Release
```
- Start the service

```sh
dotnet run --project .\PetGameBackend\PetGameBackend.csproj
```

## Usage

When compiled in `Debug` the service will automatically **delete and re-create** the database specified in `MT_MONGO_DATABASE` **every time the service starts** for easier debugging.
The service will be available at `https://localhost:5001`.

## Examples

After launching the application, at can find a [Swagger page](https://localhost:5001/swagger) in which you can find sample payloads, detailed information about each endpoint and a test client. 

Additionally, you can open the `index.html` to open the ReDoc documentation page.

## Testing

This project is using integration tests, therefore requires a real database to test against.
In `PetGameBackend.XTests.Setup` you will have to fill in values for the above mentioned environment variables.

## Project Structure

This project was created with the Microsoft ASP .NET Core API template. For reference on how to work with this (e.g. adding controllers) please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio
