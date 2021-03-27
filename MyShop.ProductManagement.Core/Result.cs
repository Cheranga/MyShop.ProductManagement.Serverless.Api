﻿using FluentValidation.Results;

namespace MyShop.ProductManagement.Domain
{
    public class Result
    {
        public bool Status => Validation != null && Validation.IsValid;
        public ValidationResult Validation { get; set; } = new ValidationResult();

        public static Result Success()
        {
            return new Result();
        }

        public static Result Failure(params ValidationFailure[] failures)
        {
            return new Result
            {
                Validation = new ValidationResult(failures)
            };
        }

        public static Result Failure(ValidationResult validationResult)
        {
            return new Result
            {
                Validation = validationResult
            };
        }

        public static Result Failure(string field, string failure)
        {
            return Failure(new ValidationFailure(field, failure));
        }
    }

    public class Result<T>
    {
        public T Data { get; set; }
        public string ErrorCode { get; set; }
        public ValidationResult Validation { get; set; } = new ValidationResult();
        public bool Status => Validation != null && Validation.IsValid;

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Data = data
            };
        }

        public static Result<T> Failure(string errorCode, ValidationResult validationResult)
        {
            return new Result<T>
            {
                ErrorCode = errorCode,
                Validation = validationResult
            };
        }

        public static Result<T> Failure(string errorCode, string errorMessage)
        {
            return Failure(errorCode, new ValidationResult(new[] { new ValidationFailure("", errorMessage) }));
        }
    }
}