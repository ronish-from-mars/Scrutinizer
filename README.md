# Checkout Scrutiniser

Validate files against a JSON schema through a simple user interface built with electron and .NET core. It supports both fixed length and separated value files validation.

  - Select the input file
  - Select the JSON schema
  - Magic - Click on validate file
  
  

# Features

  - Length check
  - Text alignment and padding check
  - Data type check (int, string, datetime)
  - Format check (datetime for e.g. ddMMyyyy)
  - Allowed values check (check whether a column contains pre-defined values)

You can also:
  - Export validation results as HTML (Reporting made easier)


### Tech

* [Electron.net](https://github.com/ElectronNET/Electron.NET) - Build cross platform desktop apps with .NET Core.

### Limitations/Known Issues
 - Only the top 50 errors are shown
 - There’s currently a bug on the electron .NET project whereby the application cannot be package into an exe. (Issue)

### Installation

Scrutiniser requires [Node.js](https://nodejs.org/), .Net Core SDK & Runtine to run.

Open Powershell or CMD

```sh
PS cd src\Checkout.Scrutinizer.UI
PS dotnet electronize start
```

### Next steps

Want to contribute? Great!

 - Validation of checksum for file header and trailer
 - Pagination & filtering
 - Add a workspace to keep track of all files, schemas and results

