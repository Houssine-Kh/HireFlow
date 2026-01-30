using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; }  //Result represents an outcome of a operation so the fields are readonly never allowed to change    
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok() => new Result(true, null);  // static so we can call this methods in the services without necessary creating an instance of the class 
        public static Result Fail(string error) => new Result(false, error);

    }
    public class Result<T> : Result
    {
        public T? Value { get; }
        private Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
        {
            Value = value;
        }
        public static Result<T> Ok(T value) => new Result<T>(true, null, value);
        public static new Result<T> Fail(string error) => new Result<T>(false, error, default); //new ignores the compiler warning

    }
}