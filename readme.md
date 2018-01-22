
# Bakery Coding Challenge

## Prerequisites
These are the prerequisites to run this solution.
- Visual Studio Code with C# and NuGet extension
- .NET Core 1.1

## Quick Start Guide
There will be an executables attached besides source code to quickly run and test the project.
The executables takes in two files as argument, the first one is file for orders request and the second file is for Products and Packages information. There are existing files example that can be found inside **inputFiles/** folder which can be used to run the project with.

To run the project inside Code:

    These commands should be run when opening the solution for the first time in Code:
   >   dotnet restore
	    dotnet build CodingChallenge.sln
	
	Use this command to run the solution in Code
   >   dotnet run --project Bakery/Bakery.csproj Bakery/inputFiles/input Bakery/inputFiles/product-pack

    To run test in Code
   >   dotnet test --project Tests/Tests.csproj
     
C# extension has a nice support to run/debug unit tests via CodeLens annotations above the test methods.

## Assumptions
- Since the bakery only accepts order in packs now, the system can only partially fulfill the order with the maximum number of quantity possible and report the remainder unfulfillable quantity to the user.
