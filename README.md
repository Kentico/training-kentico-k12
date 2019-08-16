# Kentico CMS/EMS MVC training website

This is an ASP.NET MVC 5 sample project built for the purpose of our [Kentico MVC for Developers](https://www.kentico.com/services/training/courses/developers/kentico-mvc-for-developers) training.

It is a website of a fictional medical institution named Medio Clinic.

## How to run the project

To make the project work, follow these steps:

1. Make sure your computer meets the [system requirements](https://docs.kentico.com/k12/installation/system-requirements) outlined in the documentation.
1. Clone the repo (`git clone https://github.com/Kentico/training-kentico-k12`).
1. Extract either a database backup file out of [/Db/MedioClinic.zip](/Db/MedioClinic.zip) or, a database build script out of [/Db/MedioClinicSqlScript.zip](/Db/MedioClinicSqlScript.zip) (if you happen to have an older version of SQL Server).
1. Start your [SQL Server management studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) and restore the extracted MedioClinic.bak file.
1. Register both the [administration interface](/CMS) and the [Medio Clinic website](/MedioClinic) in IIS.
    * If you register the administration interface as an application that sits under `Default Web Site` and has a `Kentico12_Admin` alias, then you won't have to do any adjustments in Visual Studio.
    * The same applies to the Medio Clinic project: If you register it under `Default Web Site` as `Kentico12_MedioClinic`, then you should be ready to compile and run.
1. Open Visual Studio with elevated credentials, open the `WebApp.sln` solution and build it (`Ctrl+Shift+B`).
1. Open the `web.config` file and adjust the connection string to your SQL Server instance (if your database instance runs on a different machine).
1. Close the solution.
1. Open the `MedioClinic.sln` solution.
    * If you haven't opened Visual Studio with elevated credentials, then you may encounter an error message saying Visual Studio doesn't have access to your local IIS.
    * If you haven't registed the project under `Default Web Site` as `Kentico12_Admin`, then you might want to adjust debugging settings through the following steps:
        * Go to the solution explorer
        * Right-click the `MedioClinic` project
        * Go to the Web tab
        * Under the Servers section > Project Url, set the correct URL according to your IIS configuration.
1. Build the solution.
1. Open the `/Config/ConnectionStrings.config` file to eventually adjust the connection string (in the same way as you did with the administration interface project).

## Troubleshooting

If you encounter a problem while going through the course, please let us know either through the course survey or by [filing an issue](https://github.com/Kentico/training-kentico-k12/issues/new) here in GitHub.
