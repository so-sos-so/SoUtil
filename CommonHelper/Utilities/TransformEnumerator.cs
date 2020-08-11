﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.CommonHelper
{
    public class TransformEnumerator : IEnumerator
    {
        private readonly IEnumerator _enumerator;
        private readonly Converter<object, object> _converter;

        public TransformEnumerator(IEnumerator enumerator, Converter<object, object> converter)
        {
            _enumerator = enumerator;
            _converter = converter;
        }

        public object Current { get; private set; }

        public bool MoveNext()
        {
            if (_enumerator.MoveNext())
            {
                var current = _enumerator.Current;
                Current = _converter(current);
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }

    public class TransformEnumerator<TInput, TOutput> : IEnumerator<TOutput>
    {
        private IEnumerator<TInput> _enumerator;
        private Converter<TInput, TOutput> _converter;

        public TransformEnumerator(IEnumerator<TInput> enumerator, Converter<TInput, TOutput> converter)
        {
            _enumerator = enumerator;
            _converter = converter;
        }

        public TOutput Current { get; private set; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_enumerator.MoveNext())
            {
                var current = _enumerator.Current;
                Current = _converter(current);
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Reset();
                _enumerator = null;
                _converter = null;
                _disposedValue = true;
            }
        }

        ~TransformEnumerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}