namespace FakeXiecheng.API.Services
{
    public interface IPropertyMappingService
    {
        IPropertyMapper GetPropertyMapper<TSource, TDestination>();
        bool IsMappingExists<Tsouce, Tdestination>(string fields);
    }
}