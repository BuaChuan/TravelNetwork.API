using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        //旅游路线的映射字典
        private Dictionary<string, PropertyMappingValue> _touristRouteMappingDictionary =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>(){ "Id" }) },
               { "Title", new PropertyMappingValue(new List<string>(){ "Title" })},
               { "Rating", new PropertyMappingValue(new List<string>(){ "Rating" })},
               { "OriginalPrice", new PropertyMappingValue(new List<string>(){ "OriginalPrice" })},
            };
        //内部属性映射器集合
        private IList<IPropertyMapper> propertyMappers = new List<IPropertyMapper>();

        public PropertyMappingService()
        {
            //初始化服务时添加旅游路线映射器(通过泛型类型确定映射器类型)
            propertyMappers.Add(
                new PropertyMapper<TouristRouteDto, TouristRoute>(_touristRouteMappingDictionary)
                );
        }
        //通过泛型类型选择对应映射器
        public IPropertyMapper GetPropertyMapper<TSource, TDestination>()
        {
            //通过类型选择对应映射器
            var filteredPropertyMappers = propertyMappers.OfType<PropertyMapper<TouristRouteDto, TouristRoute>>();
            if (filteredPropertyMappers.Count() == 1)
            {
                return filteredPropertyMappers.First();
            }
            else
            {
                throw new Exception(
                    $"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
            }
        }
        public bool IsMappingExists<Tsouce, Tdestination>(string fields)
        {
            var mappingDictionary = GetPropertyMapper<TouristRouteDto, TouristRoute>().GetMappingDictionary();
            string[] fieldsAfterSplit = fields.Split(",");
            foreach (var field in fieldsAfterSplit)
            {
                //去掉空格
                string trimmedField = field.Trim();
                //获取属性名字符串
                int spaceIndex = trimmedField.IndexOf(" ");
                string propertyName = (spaceIndex == -1) ? trimmedField : trimmedField.Remove(spaceIndex);
                if(!mappingDictionary.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
