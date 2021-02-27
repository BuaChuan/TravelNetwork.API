using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Helper
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> sources,
            string fields
        )
        {
            //找到fields对应TSource中全部的property
            //用expandoObject动态加载有效property的name与value(dictionary(name, value))
            if(sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }
            var expandoObjects = new List<ExpandoObject>();
            //创建一个属性信息表，避免在列表中遍历数据
            var propertyInfoToList = new List<PropertyInfo>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                propertyInfoToList.AddRange(propertyInfos);
            }
            else
            {
                //逗号分割字符串
                var fieldsAfterSplit = fields.Split(',');
                foreach (var field in fieldsAfterSplit)
                {
                    string propertyName = field.Trim();
                    PropertyInfo propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        throw new Exception($"属性 {propertyName} 找不到" +
                            $" {typeof(TSource)}");
                    }
                    propertyInfoToList.Add(propertyInfo);
                }
                foreach (var source in sources)
                {
                    var expandoObject = new ExpandoObject();
                    foreach (var propertyInfo in propertyInfoToList)
                    {
                        var propertyValue = propertyInfo.GetValue(source);
                        ((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
                    }
                    expandoObjects.Add(expandoObject);
                }
                
            }
            return expandoObjects;
        }
    }
}
