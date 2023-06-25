﻿using AuLiComLib.Common;
using AuLiComLib.Fixtures.Kinds;
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
        public FixtureFactory(IConnection connection)
        {
            _converter = new TypedJsonConverter<IConfigurableFixture>(connection);
            _converter.Register<GenericLamp>();
            _converter.Register<CameoLedBar3Ch2>();
            _converter.Register<CameoLedBar12Ch>();
        }

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