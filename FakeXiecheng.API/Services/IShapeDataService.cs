using System.Collections.Generic;
using System.Dynamic;

namespace FakeXiecheng.API.Services
{
    public interface IShapeDataService
    {
        bool FieldsIsValid<TSource>(string fields);
        IEnumerable<ExpandoObject> ShapeMultipleData<TSource>(string fields, IEnumerable<TSource> sources);
        ExpandoObject ShapeSingleData<TSource>(string fields, TSource source);
    }
}