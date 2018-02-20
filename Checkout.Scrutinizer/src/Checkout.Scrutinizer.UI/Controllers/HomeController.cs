namespace Checkout.Scrutinizer.UI.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System;

    using Microsoft.AspNetCore.Mvc;

    using ElectronNET.API;
    using ElectronNET.API.Entities;

    using Newtonsoft.Json;

    using Checkout.Scrutinizer.UI.Models;
    using Checkout.Scrutinizer.Infrastructure;
    using Checkout.Scrutinizer.Core;
    using Checkout.Scrutinizer.UI.Utilities;
    using System.IO;
    using FlatFiles.Scrutinizer.Resources;
    using System.Collections.Generic;

    public class HomeController : Controller
    {
        private readonly IViewRenderingService _viewRenderService;

        public HomeController(IViewRenderingService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("select-file-path", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions
                    {
                        Properties = new OpenDialogProperty[]
                        {
                            OpenDialogProperty.openFile,
                        }
                    };

                    string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
                    Electron.IpcMain.Send(mainWindow, "select-file-path-reply", files);
                });

                Electron.IpcMain.On("select-schema-path", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions
                    {
                        Properties = new OpenDialogProperty[] {
                        OpenDialogProperty.openFile
                        },
                        Filters = new FileFilter[]
                        {
                           new FileFilter
                           {
                                Name = "Json schema",
                                Extensions = new string[] { "json"}
                           }
                        }
                    };

                    string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
                    Electron.IpcMain.Send(mainWindow, "select-schema-path-reply", files);
                });

                Electron.IpcMain.On("validate-file-btn", (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    try
                    {
                        args = args.ToString().Replace(@"\", @"\\");
                        var arguments = JsonConvert.DeserializeObject<FileDefinition>(args.ToString());
                        var filePath = arguments.FilePath;
                        var schemaPath = arguments.SchemaFilePath;
                        var fileType= arguments.FileType;
                        if (!System.IO.File.Exists(filePath))
                        {
                            Electron.IpcMain.Send(mainWindow, "results", $"<div class='alert alert-danger' role='alert'>Invalid file {filePath}</div>");
                            return;
                        }
                        if (!System.IO.File.Exists(schemaPath))
                        {
                            Electron.IpcMain.Send(mainWindow, "results", $"<div class='alert alert-danger' role='alert'>Invalid file {schemaPath}</div>");
                            return;
                        }

                        var schemaData = string.Empty;
                        var schemaParser = new SchemaParser();
                        try
                        {
                            using (var sr = new StreamReader(System.IO.File.OpenRead(schemaPath)))
                            {
                                schemaData = sr.ReadToEnd();
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMsg = String.Format(null, SharedResources.SchemaError, ex.Message);
                            Console.WriteLine($"{errorMsg}");
                        }

                        if (string.IsNullOrEmpty(schemaData))
                        {
                            Electron.IpcMain.Send(mainWindow, "results", "<div class='alert alert-danger' role='alert'>Schema file is empty.</div>");
                            return;
                        }

                        var parsedSchema = schemaParser.ParseSchema(schemaData);
                        var schemaValidator = new FluentSchemaValidator();
                        var schemaValidationResults = schemaValidator.Validate(parsedSchema);
                        if (!schemaValidationResults.IsValid && schemaValidationResults.Errors.Any())
                        {
                            var results = _viewRenderService.RenderToStringAsync("Partial/_SchemaFileValidationResult", schemaValidationResults.Errors);
                            if (results.IsCompletedSuccessfully && !results.IsFaulted)
                            {
                                Electron.IpcMain.Send(mainWindow, "results", results.Result);
                                return;
                            }
                            else
                            {
                                Electron.IpcMain.Send(mainWindow, "results", "<div class='alert alert-danger' role='alert'>An Error has occurred while processing the file. Please contact your administrator.</div>");
                                return;
                            }
                        }

                        if (parsedSchema.FileFormat.ToLower() == "fixed")
                        {
                            var validator = new FixedLengthFileValidator(filePath, schemaPath, parsedSchema);
                            var result = validator.ValidateFile();
                            result.Results = result.Results.Take(50).ToList();
                            var results = _viewRenderService.RenderToStringAsync("Partial/_FixedLengthFileValidationResult", result);
                            if (results.IsCompletedSuccessfully && !results.IsFaulted)
                            {
                                Electron.IpcMain.Send(mainWindow, "results", results.Result);
                                return;
                            }
                            else
                            {
                                Electron.IpcMain.Send(mainWindow, "results", "<div class='alert alert-danger' role='alert'>An Error has occurred while processing the file. Please contact your administrator.</div>");
                                return;
                            }
                        }
                        else if (parsedSchema.FileFormat.ToLower() == "separated")
                        {
                            var validator = new SeparatedFileValidator(filePath, schemaPath, parsedSchema);
                            var result = validator.ValidateFile();
                            result.Results = result.Results.Take(50).ToList();
                            var results = _viewRenderService.RenderToStringAsync("Partial/_SeparatedFileValidationResult", result);
                            if (results.IsCompletedSuccessfully && !results.IsFaulted)
                            {
                                Electron.IpcMain.Send(mainWindow, "results", results.Result);
                                return;
                            }
                            else
                            {
                                Electron.IpcMain.Send(mainWindow, "results", "<div class='alert alert-danger' role='alert'>An Error has occurred while processing the file. Please contact your administrator.</div>");
                                return;
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        Electron.IpcMain.Send(mainWindow, "results", $"<div class='alert alert-danger' role='alert'>{ex.Message.ToString()}</div>");
                        return;
                    }

                });

                Electron.IpcMain.On("create-html-file", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions
                    {
                        Properties = new OpenDialogProperty[]
                        {
                            OpenDialogProperty.openDirectory,
                        }
                    };

                    string[] directories = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
                    var directory = directories[0];
                    var pathString = Path.Combine(directory, "Validation_Result.html");
                    var file = System.IO.File.Create(pathString);
                    file.Close();
                    Electron.IpcMain.Send(mainWindow, "created-html-file-reply", pathString);
                });
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
