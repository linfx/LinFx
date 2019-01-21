using Dapper;
using System.Collections.Generic;

namespace LinFx.Extensions.Dapper
{
    public class GridReaderResultReader : IMultipleResultReader
    {
        readonly SqlMapper.GridReader _reader;

        public GridReaderResultReader(SqlMapper.GridReader reader) => _reader = reader;

		public IEnumerable<T> Read<T>() => _reader.Read<T>();
    }

    public class SequenceReaderResultReader : IMultipleResultReader
    {
        private readonly Queue<SqlMapper.GridReader> _items;

        public SequenceReaderResultReader(IEnumerable<SqlMapper.GridReader> items) => _items = new Queue<SqlMapper.GridReader>(items);

        public IEnumerable<T> Read<T>()
        {
            SqlMapper.GridReader reader = _items.Dequeue();
            return reader.Read<T>();
        }
    }
}