using AuLiComLib;
using AuLiComLib.Common;
using AuLiComLib.CommandExecutor;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using System.Data;
using System.Runtime.CompilerServices;
using System.Reflection;
using AuLiComSim;

Version? consoleVersion = Assembly.GetExecutingAssembly().GetName().Version;
Version? libraryVersion = typeof(DmxConnection).Assembly.GetName().Version;

var console = new SystemConsole();
console.WriteLine("AuLiCom -- AULA LIGHT COMMANDER");
console.WriteLine($"Console {consoleVersion}, Library {libraryVersion}");
console.WriteLine();

Dictionary<string, ISerialPort> portsByName = DmxPorts.GetPortsByName();

IConnection connection;

if (portsByName.Count == 0)
{
    console.WriteLine($"No DMX port available, using simulator instead");
    connection = new SimulatorConnection();
}
else
{
    ISerialPort selectedPort;

    if (portsByName.Count == 1)
    {
        selectedPort = portsByName.Values.First();
        console.WriteLine($"Only one DMX port available: {selectedPort.PortName}");
    }
    else
    {
        console.WriteLine("Enter the name of one of these ports to use for a DMX connection:");
        foreach (string portName in portsByName.Keys)
        {
            console.WriteLine(portName);
        }
        string selectedPortName = console.ReadLineTrim() ?? throw new ArgumentException("You did not enter a port name");
        selectedPort = portsByName[selectedPortName];
    }

    console.WriteLine();
    console.WriteLine($"Establishing DMX connection to {selectedPort.PortName}...");
    connection = new DmxConnection(port: selectedPort,
                                    executor: new SystemThread($"DmxConnection{selectedPort.PortName}"));

    console.WriteLine($"DMX connection established.");
}

var commandExecutor = new CommandLoopExecutor(connection, console);
commandExecutor.Loop();

console.WriteLine();
console.WriteLine($"Closing DMX connection and port...");
connection.SetValuesToZero();
Thread.Sleep(TimeSpan.FromSeconds(1)); // TODO: create wait event for DMX connection end
connection.Dispose();
Thread.Sleep(TimeSpan.FromSeconds(2)); // TODO: create wait event for DMX connection end
foreach (ISerialPort port in portsByName.Values)
{
    port.Dispose();
}
