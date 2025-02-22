﻿using System;
using FMAplication.Core;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using X.PagedList;

namespace FMAplication.Controllers.Common
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult OkResult(IPagedList model)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 200,
                Status = "Success",
                Msg = "Successful",
                Data = new
                {
                    model.PageCount,
                    model.PageNumber,
                    model.PageSize,
                    model.TotalItemCount,
                    model.IsLastPage,
                    model.HasNextPage,
                    model.LastItemOnPage,
                    model.FirstItemOnPage,
                    model
                }
            };
            return ObjectResult(apiResult);
        }
        protected IActionResult OkResult(object data)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 200,
                Status = "Success",
                Msg = "Successful",
                Data = data
            };
            return ObjectResult(apiResult);
        }
        protected IActionResult OkResult(object data, string message)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 200,
                Status = "Success",
                Msg = message,
                Data = data
            };
            return ObjectResult(apiResult);
        }
        protected IActionResult ValidationResult(ModelStateDictionary modelState)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 400,
                Status = "ValidationError",
                Msg = "Validation Fail",
                Errors = modelState.GetErrors()
            };
            return ObjectResult(apiResult);
        }
        protected IActionResult ExceptionResult(Exception ex)
        {
            ex.ToWriteLog();

            var apiResult = new ApiResponse
            {
                StatusCode = 500,
                Status = "Error",
                Msg = ex.Message,
                Data = new object()
            };
            return ObjectResult(apiResult);
        }
        protected IActionResult ObjectResult(ApiResponse model)
        {
            var result = new ObjectResult(model)
            {
                StatusCode = model.StatusCode
            };
            return result;
        }

        protected BadRequestObjectResult Error(object error,AppErrorType type = AppErrorType.Alert)
        {
            return BadRequest(new AppError(error, type));
        }
    }
}