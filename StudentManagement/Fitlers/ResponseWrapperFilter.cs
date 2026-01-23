using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagement.Wrappers;

namespace StudentManagement.Filters
{
    public class ResponseWrapperFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? 200;

                if (statusCode >= 200 && statusCode < 300)
                {
                    var response = new ApiResponse<object>(objectResult.Value);
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = statusCode
                    };
                }

                else
                {
                    string message = "An error occurred.";
                    string[]? errors = null;

                    if (objectResult.Value is string errorString)
                    {
                        message = errorString;
                    }
                    else if (objectResult.Value is HttpValidationProblemDetails validationErrors)
                    {
                        message = "One or more validation errors occurred.";
                        errors = validationErrors.Errors
                            .SelectMany(kvp => kvp.Value)
                            .ToArray();
                    }
                    else if (objectResult.Value is SerializableError serializableError)
                    {
                        message = "Validation failed.";
                        errors = serializableError
                            .SelectMany(kvp => (string[])kvp.Value)
                            .ToArray();
                    }
                    else if (objectResult.Value is ProblemDetails problemDetails)
                    {
                        message = problemDetails.Title ?? "An error occurred.";
                        if (!string.IsNullOrEmpty(problemDetails.Detail))
                        {
                            message = problemDetails.Detail;
                        }
                    }

                    var response = new ApiResponse<object>(message, errors);

                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = statusCode
                    };
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}