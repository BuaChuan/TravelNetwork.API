using FakeXiecheng.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace FakeXiecheng.API.Helper
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary
            )
        {
            if(source == null)
            {
                throw new ArgumentNullException("source");
            }
            if(mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }
            if(string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }
            string orderByString = string.Empty;
            var orderByAfterSplit = orderBy.Split(",");
            foreach(var orderNameAndSortString in orderByAfterSplit)
            {
                string trimmedOrderNameAndSortString = orderNameAndSortString.Trim();
                int spaceIndex = trimmedOrderNameAndSortString.IndexOf(" ");
                bool isDescending = trimmedOrderNameAndSortString.EndsWith("desc");
                var sourcePropertyName = (spaceIndex == -1) ?
                    trimmedOrderNameAndSortString : trimmedOrderNameAndSortString.Remove(spaceIndex);
               if(!mappingDictionary.ContainsKey(sourcePropertyName))
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }
                PropertyMappingValue propertyMappingValue = mappingDictionary[sourcePropertyName];
                //将propertyMappingValue中的值全部加入排序字符串
                foreach (var destPropertyName in propertyMappingValue.Properties.Reverse())
                {
                    orderByString += (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ",")
                        + destPropertyName
                        + (isDescending ? " desc" : " asc");
                }
                
                
            }
            //使用了using System.Linq.Dynamic.Core拓展框架
            return source.OrderBy(orderByString);
        }
    }
}
