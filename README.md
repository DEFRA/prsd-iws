# International Waste Shipments (IWS) Service

There are European and UK rules governing how you can ship waste into or out of the country.

The regulations apply from the point of loading the waste until it has been fully recovered or disposed of at the destination facility. If you fail to follow them, you may be committing a criminal offence and risk prosecution, financial penalties and/or imprisonment.

[https://www.gov.uk/guidance/importing-and-exporting-waste](https://www.gov.uk/guidance/importing-and-exporting-waste)

## Development environment

### Dependencies

The following system dependencies are required to build the solution:

* Visual Studio 2013+ ([Community Edition available](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx))
* [SQL Server Express 2014](https://www.microsoft.com/en-gb/server-cloud/products/sql-server-editions/sql-server-express.aspx)
* [.NET Framework 4.5.2 SDK](https://www.microsoft.com/en-gb/download/details.aspx?id=42637)

### Obtain the source code

Clone the repository, copying the project into a working directory:

    git clone https://github.com/EnvironmentAgency/prsd-iws.git
    
### Build the solution

1. Open the IWS solution file (EA.Iws.sln).
2. In Visual Studio, configure the NuGet Package Manager ('Tools' -> 'NuGet Package Manager' -> 'Package Manager Settings') to:
   * Allow NuGet to download missing packages.
   * Automatically check for missing packages during build in Visual Studio.
3. Build the project. NuGet will download the missing packages.

<!-- End of list -->

### Setup the database

The IWS project uses [AliaSQL](https://github.com/ClearMeasure/AliaSQL) to manage the creation of and updates to the database.

1. Within the solution, find the EA.Iws.Database project.
2. Open the App.config of this project. In the appSettings, set the value for 'DatabaseServer' as the database server to be used. The local SQL server express database (.\SQLEXPRESS) will be used if this value is not set.
3. You can alter the value of 'DatabaseName' if you wish the database to be created with a different name.
4. Run the database project (this can be in debug mode). You will be shown a list of possible actions. Choose 'Create', which will run the scripts to create the database.

<!-- End of list -->

## Tests
We use [xUnit](https://github.com/xunit/xunit) and [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy) for unit testing.

The tests can be [run using the test explorer in Visual Studio](https://xunit.github.io/docs/getting-started-desktop.html#run-tests-visualstudio), or via the [xUnit.net console runner](https://xunit.github.io/docs/getting-started-desktop.html#run-tests).

## Contributing to this project

If you have an idea you'd like to contribute please log an issue.

All contributions should be submitted via a pull request.

## License

THIS INFORMATION IS LICENSED UNDER THE CONDITIONS OF THE OPEN GOVERNMENT LICENCE found at:

http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3

The following attribution statement MUST be cited in your products and applications when using this information.

> Contains public sector information licensed under the Open Government license v3

### About the license

The Open Government Licence (OGL) was developed by the Controller of Her Majesty's Stationery Office (HMSO) to enable information providers in the public sector to license the use and re-use of their information under a common open licence.

It is designed to encourage use and re-use of information freely and flexibly, with only a few conditions.