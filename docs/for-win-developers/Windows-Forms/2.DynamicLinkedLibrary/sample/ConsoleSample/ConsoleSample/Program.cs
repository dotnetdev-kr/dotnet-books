using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

var executePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var dllPath = Path.Combine(executePath, "Driver");
if (Directory.Exists(dllPath) == false)
    Directory.CreateDirectory(dllPath);

Console.WriteLine(dllPath);

var dllFiles = Directory.GetFiles(dllPath, "*.dll");
foreach (var dllFile in dllFiles)
{
    Console.WriteLine(dllFile);
}
var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFiles[0]);

