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
            .Register<AriaPc>()
            .Register<CameoLedBar_3Ch2>()
            .Register<CameoLedBar_12Ch>()
            .Register<CameoPixBar650CPro_3Ch1>()
            .Register<CameoPixBar650CPro_3Ch2>()
            .Register<CameoThunderWash600Rgbw_7Ch2>()
            .Register<CantataPc>()
            .Register<CantataProfiler>()
            .Register<ElationKl4Fresnel_1Ch>()
            .Register<ElationPar_11Ch>()
            .Register<ElationProfiler_10Ch>()
            .Register<GenericLamp>()
            .Register<OtherPc>()
            .Register<Par64>()
            .Register<Par56Silver>()
            .Register<Profiler>()
            .Register<Quartz>();


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
