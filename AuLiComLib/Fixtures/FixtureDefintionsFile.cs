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
    internal class FixtureDefintionsFile
    {
        public FixtureDefintionsFile(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private readonly IFileSystem _fileSystem;

        public IList<FixtureDefinition> Load(string path)
        {
            if (!HasExtension(path))
            {
                throw new ArgumentException($"Fixture file '{path}' has to have extension '{Extension}'.", nameof(path));
            }

            string fileContents = _fileSystem.File.ReadAllText(path) ?? throw CreateFileEmptyException();
            List<FixtureDefinition> result = JsonConvert.DeserializeObject<List<FixtureDefinition>>(fileContents) ?? throw CreateFileEmptyException();
            return result;

            ArgumentException CreateFileEmptyException() => new($"Fixture file '{path}' is empty.");
        }

        public static bool HasExtension(string path) => Path.GetExtension(path).Equals(Extension); // testable even without using IFileSystem

        private const string Extension = ".alcfix";  // [A]ula [L]ight [C]ommander [FIX]tures
    }
}
