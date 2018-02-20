using System;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    internal interface IMapperSource<TEntity>
    {
        IMapper<TEntity> GetMapper();
    }
}
