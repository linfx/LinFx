using System;
using System.Collections.Generic;
using Dapper;

namespace LinFx.Data.Dapper.Extensions
{
    public interface IMultipleResultReader
    {
        IEnumerable<T> Read<T>();
        T ReadSingle<T>();
    }

    public class GridReaderResultReader : IMultipleResultReader
    {
        readonly SqlMapper.GridReader _reader;

        public GridReaderResultReader(SqlMapper.GridReader reader)
        {
            _reader = reader;
        }

        public IEnumerable<T> Read<T>()
        {
            return _reader.Read<T>();
        }

        public T ReadSingle<T>()
        {
            return _reader.ReadSingle();
        }
    }

    public class SequenceReaderResultReader : IMultipleResultReader
    {
        private readonly Queue<SqlMapper.GridReader> _items;

        public SequenceReaderResultReader(IEnumerable<SqlMapper.GridReader> items)
        {
            _items = new Queue<SqlMapper.GridReader>(items);
        }

        public IEnumerable<T> Read<T>()
        {
            SqlMapper.GridReader reader = _items.Dequeue();
            return reader.Read<T>();
        }

        public T ReadSingle<T>()
        {
            SqlMapper.GridReader reader = _items.Dequeue();
            return reader.ReadSingle<T>();
        }
    }
}