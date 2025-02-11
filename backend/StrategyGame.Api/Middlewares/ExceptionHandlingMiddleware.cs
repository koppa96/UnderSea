﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Logger;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Api.Middlewares
{
    /// <summary>
    /// Provides a middleware that logs any exception using an <see cref="IDbLogger"/>, 
    /// and provides an appropriate error to the user.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbLogger logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await logger.LogExceptionAsync(e);
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            if (e is ArgumentOutOfRangeException)
            {
                context.Response.StatusCode = 404;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = e.Message
                });
            }

            if (e is ArgumentException || e is InvalidOperationException || e is InProgressException || e is LimitReachedException)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = e.Message
                });
            }

            if (e is UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = e.Message
                });
            }

            if (e is OperationCanceledException || e is TaskCanceledException)
            {
                context.Response.StatusCode = StatusCodes.Status418ImATeapot;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status418ImATeapot,
                    Title = "I'm a teapot",
                    Detail = "Turn-end in progress, retry later."
                });
            }

            context.Response.StatusCode = 500;
            return context.Response.WriteJsonAsync(new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = e.Message
            });
        }
    }
}