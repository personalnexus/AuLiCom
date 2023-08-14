using AuLiComLib.Common;
using AuLiComLib.Fixtures.Types;
using AuLiComLib.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public class FixtureFactory : IFixtureFactory
    {
        public FixtureFactory(IConnection connection) => _converter = new TypedJsonConverter<IConfigurableFixture>(connection)
            .Register<GenericLamp>()
            .Register<CameoLedBar3Ch2>()
            .Register<CameoLedBar12Ch>()
            .Register<CameoPixBar650CPro3Ch1>()
            .Register<CameoPixBar650CPro3Ch2>()
            .Register<CameoThunderWash600Rgbw7Ch2>();

        private readonly TypedJsonConverter<IConfigurableFixture> _converter;

        public IEnumerable<IFixture> CreateFromJson(string fileContents) =>
            JsonConvert
            .DeserializeObject<IConfigurableFixture[]>(value: fileContents,
                                                       converters: _converter);

        public IFixture CreateFromFixtureInfo(FixtureInfo info)
        {
            IConfigurableFixture fixture = _converter.Create(info.FixtureType);
            fixture.Name = info.FixtureName;
            fixture.StartChannel = info.StartChannel;
            return fixture;
        }

        public IEnumerable<string> GetFixtureTypes() =>
            _converter
            .GetRegisteredTypeNames();
    }
}
