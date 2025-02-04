using ConsoleAppFramework;
using Tfvars.Azurerm;

var app = ConsoleApp.Create();
// app.ConfigureServices((ctx,services) =>
// {
// 	services
// 		.AddTransient<StorageService>()
// 		.AddTransient<ProjectService>();
// });
app.UseFilter<InitFilter>();
app.Run(args);
