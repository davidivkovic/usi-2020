# Hospital Calendar 🏥

A GUI based Hospital calendar and general planning System, developed during the Introduction to Software engineering University course.

# Built with
- [Windows Presentation Foundation](https://docs.microsoft.com/en-us/dotnet/framework/wpf/getting-started/introduction-to-wpf-in-vs)
- [.NET Core 3.1](https://dotnet.microsoft.com/download)
- [Entity Framework Core 3.1](https://docs.microsoft.com/en-us/ef/core/get-started/install/)
- [MVVMLight](https://www.nuget.org/packages/MvvmLightLibsStd10)
- [PropertyChanged.Fody](https://www.nuget.org/packages/PropertyChanged.Fody/)
- [Material Design for XAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

### Prerequisites:
- A Microsoft SQL Server runnining remotely, locally or in docker
- [Visual studio 2019](https://visualstudio.microsoft.com/vs/)
- The [.NET Desktop Development Workload for Visual studio](https://visualstudio.microsoft.com/vs/features/net-development/)

# Showcase 📸
![login](/images/login.png)
![admin](/images/admin.png)
![doctor](/images/doctor.png)
![manager](/images/manager.png)

## How to run 🚀

- Open `HospitalCalendar.sln` in Visual Studio
- `Restore NuGet Packages` for the solution *(Right click the solution -> Restore NuGet packages)*

##### Editing the connection string
- Set `HospitalCalendar.EntityFramework` as the startup project *(Right Click -> Set As Startup Project)*
- Provide your connection string to a server database in the file `HospitalCalendarDbContextFactory.cs` under the `DbContextOptionsBuilder`

##### Code first migration

- Open the `Package Manager Console` *(View -> Other Windows -> Package Manager Console)*
- In the console, set the default project to `HospitalCalendar.EntityFramework`
- Enter the command `$ Add-Migration initial` *(This will generate an Entity Framework migration containing the SQL database schema which references the project `HospitalCalendar.Domain`)*
- Enter the command `$ Update-Database` (This will apply the migration schema to the database)

##### Creating an administrator account
- Set the `ServicesTestConsoleApp` as the startup project
- Enter your administrator account data in the line `userService.Register(typeof(Administrator), "FirstName", "LastName", "username", "password").GetAwaiter().GetResult();`
- Run the console application ***(The registration should be successful if no exceptions are thrown)***

##### Logging in
- Set `HospitalCalendar.WPF` as the startup project
- Finally, Run the aplication
