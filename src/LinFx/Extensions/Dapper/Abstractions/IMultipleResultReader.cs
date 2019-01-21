using System.Collections.Generic;

namespace LinFx.Extensions.Dapper
{
    public interface IMultipleResultReader
    {
        IEnumerable<T> Read<T>();
    }
}
