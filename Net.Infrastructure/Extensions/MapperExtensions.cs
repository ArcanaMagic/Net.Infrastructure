using AutoMapper;

namespace Net.Infrastructure.Extensions
{
    public static class MapperExtensions
    {
        public static TDestination MapSafe<TSource, TDestination>(this IMapper mapper, TSource source, TDestination destination = default) 
            => source == null ? destination : mapper.Map(source, destination);
    }
}