You have downloaded the Kentico.Libraries Nuget package maintained by Kentico Software (http://www.kentico.com).


Using the Kentico API Externally
--------------------------------
Please see https://docs.kentico.com/x/xBEbB for more information about using the Kentico API.


Updating from older versions
------------------------------
Prior to version 11, Kentico.Libraries had a dependency on the 'Kentico.Libraries.Dependencies' NuGet package, which integrated third-party libraries. 
The required libraries are now added via dependencies on other third-party NuGet packages.

- Uninstall the 'Kentico.Libraries.Dependencies' package from your project (for example through the NuGet Package Manager UI)
- Delete the 'CMSDependencies' folder from your project's root folder and/or the build output directory. 


Visit our documentation at https://docs.kentico.com/.
