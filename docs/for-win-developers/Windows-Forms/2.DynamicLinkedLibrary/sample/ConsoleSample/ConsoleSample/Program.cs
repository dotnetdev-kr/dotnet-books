using System;
using System.IO;
using System.Reflection;
using MotorDriver;


var driverPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Driver");
var motorDriverFactory = new MotorFactory(driverPath);

foreach (var driverName in motorDriverFactory.GetDriverNames())
    Console.WriteLine(driverName);

var motor1 = motorDriverFactory.Create("A Motor Driver", 1);
var motor2 = motorDriverFactory.Create("B Motor Driver", 2);

await motor1.MoveAsync(20);
await motor2.MoveAsync(20);
