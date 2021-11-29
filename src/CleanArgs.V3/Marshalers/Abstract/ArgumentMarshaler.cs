using CleanArgs.Interfaces.Marshalers;
using System;
using System.Collections.Generic;

namespace CleanArgs.Marshalers.Abstract
{
    public abstract class ArgumentMarshaler<T> : IArgumentMarshaler<T>
    {
        private T _value = default;
        private readonly int _minValuesCount;
        private readonly int _maxValuesCount;

        public ArgumentMarshaler() : this(0)
        {

        }

        public ArgumentMarshaler(int expectedValuesCount) : this(expectedValuesCount, expectedValuesCount)
        {

        }

        public ArgumentMarshaler(int minValuesCount, int maxValuesCount)
        {
            _minValuesCount = minValuesCount;
            _maxValuesCount = maxValuesCount;
        }

        public T Get()
        {
            return _value;
        }

        public void Set(List<string> values)
        {
            if(values.Count < _minValuesCount)
            {
                throw new ArgumentException($"No value found");
            }

            if(values.Count > _maxValuesCount)
            {
                throw new ArgumentException($"Too many values found");
            }

            _value = Parse(values);
        }

        public abstract T Parse(List<string> values);

        object IArgumentMarshaler.Get()
        {
            return Get();
        }
    }
}
