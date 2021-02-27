using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> Properties { get; private set; }
        public PropertyMappingValue(IEnumerable<string> properties)
        {
            Properties = properties;
        }
    }
}
