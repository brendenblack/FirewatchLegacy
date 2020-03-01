using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Blackbox.Firewatch.Application.Features
{
    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get
            {
                Contract.Requires(IsSuccess);

                return _value;
            }

            private set
            {
                _value = value;
            }
        }

        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            Contract.Requires(value != null || !success);

            Value = value;
        }
    }
}
