using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    public class LoadFileCommand : ICommand
    {
        public LoadFileCommand(IConnection connection,
                               ICommandWriteConsole console,
                               ICommandFixtures fixtures,
                               IFileSystem fileSystem)
        {
            _connection = connection;
            _console = console;
            _fixtures = fixtures;
            _fileSystem = fileSystem;
        }
        
        private readonly IConnection _connection;
        private readonly ICommandWriteConsole _console;
        private readonly ICommandFixtures _fixtures;
        private readonly IFileSystem _fileSystem;

        public string Description => $"LOAD MyFixturesFile.{FixturesFile.Extension} replaces all currently loaded fixtures with those from the given file.";

        public bool TryExecute(string command)
        {
            bool result;
            if (!command.StartsWith("LOAD ", StringComparison.OrdinalIgnoreCase))
            {
                result = false;
            }
            else
            {
                string path = command[5..].Trim();  // i.e. strip "LOAD " and any whitespace from the beginning
                if (FixturesFile.HasExtension(path))
                {
                    result = true;
                    List<IFixture> fixtures = new FixturesFile(_connection, _fileSystem).Load(path).ToList();
                    _fixtures.SetFixtures(fixtures);
                    _console.WriteLine($"Loaded file '{path}' containing {fixtures.Count} fixtures.");
                }
                // TODO: once we have shows
                // if ShowsFile.HasExtension(path)
                // {
                //    
                // }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
