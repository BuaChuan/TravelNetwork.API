using System.Collections.Generic;

namespace FakeXiecheng.API.Services
{
    public interface IPropertyMapper
    {
        Dictionary<string, PropertyMappingValue> GetMappingDictionary();
    }
}