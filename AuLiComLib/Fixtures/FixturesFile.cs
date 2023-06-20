using AuLiComLib.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public class FixturesFile
    {
        public FixturesFile(IConnection connection, IFileSystem fileSystem)
        {
            _connection = connection;
            _fileSystem = fileSystem;
        }

        private readonly IFileSystem _fileSystem;
        private readonly IConnection _connection;

        public IFixture[] Load(string path)
        {
            if (!HasExtension(path))
            {
                throw new ArgumentException($"Fixture file '{path}' has to have extension '{Extension}'.", nameof(path));
            }

            string fileContents = _fileSystem.File.ReadAllText(path) ?? throw CreateFileEmptyException();
            IFixture[] result = JsonConvert.DeserializeObject<IFixture[]>(value: fileContents,
                                                                          converters: new FixtureKindJsonConverter(_connection))
                                                                          ?? throw CreateFileEmptyException();
            return result;

            ArgumentException CreateFileEmptyException() => new($"Fixture file '{path}' is empty.");
        }

        public static bool HasExtension(string path) => Extension.Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase); // testable even without using IFileSystem

        public const string Extension = ".alcfix";  // [A]ula [L]ight [C]ommander [FIX]tures
    }
}
