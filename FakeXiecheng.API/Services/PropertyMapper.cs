using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    //泛型用来判断映射器类型
    public class PropertyMapper<TSource, TDestination> : IPropertyMapper
    {
        private Dictionary<string, PropertyMappingValue> _mappingDictionary;
        public PropertyMapper(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }

        public Dictionary<string, PropertyMappingValue> GetMappingDictionary()
        {
            return _mappingDictionary;
        }
    }
}
