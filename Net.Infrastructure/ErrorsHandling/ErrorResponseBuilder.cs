using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Net.Infrastructure.Exceptions;
using Net.Infrastructure.Extensions;
using Npgsql;

namespace Net.Infrastructure.ErrorsHandling
{
    public static class ErrorResponseBuilder
    {
        public static ErrorModel Create(Exception exception, bool skipDetails = false)
        {
            var errorModel = new ErrorModel()
            {
                Lines = GetLinesFromStackTrace(exception)
            };

            switch (exception)
            {
                case DbUpdateException dbUpdateException:
                    switch (dbUpdateException.InnerException)
                    {
                        case PostgresException pe:
                        {
                            errorModel.StatusCode = HttpStatusCode.Conflict; 
                            errorModel.ErrorCode = "DB0001";
                            errorModel.ErrorType = "DbException";
                            errorModel.Message = pe.MessageText.Replace("\"", "'");
                            if(!skipDetails && !string.IsNullOrEmpty(pe.Detail))
                                errorModel.Details = new []{pe.SchemaName + "." + pe.TableName + ": " + pe.Detail.Replace("\"", "'")};
                            break;
                        }
                        case NotFoundException nf:
                        {
                            errorModel.StatusCode = HttpStatusCode.NotFound;
                            errorModel.ErrorCode = "DB0002";
                            errorModel.ErrorType = "NotFoundException";
                            errorModel.Message = nf.Message.Replace("\"", "'");
                            errorModel.Details = null;
                            break;
                        }
                        default:
                        {
                            errorModel.StatusCode = HttpStatusCode.InternalServerError;
                            errorModel.ErrorCode = "DB0010";
                            errorModel.ErrorType = "InternalServerError";
                            errorModel.Message = dbUpdateException.Message.Replace("\"", "'");
                            errorModel.Details = GetDetails(dbUpdateException);

                            break;
                        }
                    }
                    break;
                case ValidateException validateException:
                    errorModel.StatusCode = HttpStatusCode.BadRequest;
                    errorModel.ErrorCode = "VE0001";
                    errorModel.ErrorType = "ValidateException";
                    errorModel.Message = "Validation errors";
                    errorModel.Lines = null;
                    errorModel.Details = validateException.Message.Split("^");
                    break;
                case UnauthorizedException unauthorizedException:
                    errorModel.StatusCode = HttpStatusCode.Unauthorized;
                    errorModel.ErrorCode = "UE0001";
                    errorModel.ErrorType = "UnauthorizedException";
                    errorModel.Message = "Authorize errors";
                    errorModel.Lines = null;
                    errorModel.Details = unauthorizedException.Message.Split("^");
                    break;
                case ForbidException forbidException:
                    errorModel.StatusCode = HttpStatusCode.Forbidden;
                    errorModel.ErrorCode = "FE0001";
                    errorModel.ErrorType = "ForbidException";
                    errorModel.Message = "Authorize errors";
                    errorModel.Lines = null;
                    errorModel.Details = forbidException.Message.Split("^");
                    break;
                case LockedException lockedException:
                    errorModel.StatusCode = HttpStatusCode.Locked;
                    errorModel.ErrorCode = "LE0001";
                    errorModel.ErrorType = "LockedException";
                    errorModel.Message = "Authorize errors";
                    errorModel.Lines = null;
                    errorModel.Details = lockedException.Message.Split("^");
                    break;
                default:
                {
                    errorModel.StatusCode = HttpStatusCode.InternalServerError;
                    errorModel.ErrorCode = exception.HResult.ToString();
                    errorModel.ErrorType = "InternalServerError";
                    errorModel.Message = exception.Message.Replace("\"", "'");
                    errorModel.Details = GetDetails(exception);

                    break;
                }
            }

            return errorModel;
        }

        private static string GetLineFromStackTrace(Exception exception)
        {
            var line = exception.StackTrace?.Split(Environment.NewLine)
                .FirstOrDefault(x => x.Contains(":line "));

            if (line == null)
                return null;

            var length = line.Length;
            var index = line.IndexOf(") in ", StringComparison.Ordinal);
            var leftPartLength = line.Left(index).Length;
            var text = line.Right(length - leftPartLength - 5);

            return text;
        }

        private static IEnumerable<string> GetLinesFromStackTrace(Exception exception)
        {
            var lines = exception.StackTrace?.Split(Environment.NewLine)
                .Where(x => x.Contains(":line ")).ToList();

            if (lines == null || !lines.Any())
                return null;

            var results = new List<string>();

            foreach (var line in lines)
            {
                var length = line.Length;
                var index = line.IndexOf(") in ", StringComparison.Ordinal);
                var leftPartLength = line.Left(index).Length;
                var text = line.Right(length - leftPartLength - 5);

                results.Add(text);
            }

            return results;
        }

        private static string GetMessage(Exception exception)
        {
            var message = exception?.Message;

            if (string.IsNullOrWhiteSpace(message))
                return message;

            if (exception is PostgresException pe)
                message = pe.Message + " | " + pe.Detail;


            var lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length > 1)
                return lines[0];

            return message;
        }

        private static IEnumerable<string> GetDetails(Exception exception)
        {
            var innerException = exception.InnerException;

            if (innerException == null)
                return null;

            var details = new List<string>();

            if (innerException is AggregateException aggregateException)
            {
                foreach (var ie in aggregateException.InnerExceptions)
                {
                    if(ie.InnerException == null)
                        details.Add(ie.Message);
                    else
                        details.AddRange(GetDetails(ie));
                }
            }
            else
            {
                if(innerException.InnerException == null)
                    details.Add(innerException.Message);
                else
                    details.AddRange(GetDetails(innerException));
            }


            return details;
        }
    }
}