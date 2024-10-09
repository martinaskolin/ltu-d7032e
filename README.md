# Apples To Apples

## Description
Home Exam in D7032 at LTU. Apples to Apples is a party card game that revolves around humor and subjective matching.

## Requirements
- .NET 7.0 SDK
- Windows 64-bit OS

## Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/martinaskolin/ltu-d7032e.git
    cd ltu-d7032e
    ```

## Running the Game
1. Run the game locally against bots
    ```sh
    .\ApplesToApples\ApplesToApples\bin\Release\net7.0\win-x64\ApplesToApples.exe
    ```
2. Run the game as a host
    ```sh
    .\ApplesToApples\ApplesToApples\bin\Release\net7.0\win-x64\ApplesToApples.exe [Number of Player]
    ```

2. Run the game as a client
    ```sh
    .\ApplesToApples\ApplesToApples\bin\Release\net7.0\win-x64\ApplesToApples.exe [IP Address (localhost)]
    ```

## Running Tests
```sh
    dotnet test
```
