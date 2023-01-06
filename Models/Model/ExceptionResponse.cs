using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class ExceptionResponse
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string StackTrace { get; set; }
        public string ErrorCode { get; set; }

        public ExceptionResponse(Exception ex)
        {
            Message = ex.Message;
            Type = ex.GetType().Name;
            Status = false;
            StatusCode = 500;
            StackTrace = ex.ToString();
            if (ex is HttpStatusException httpStatusException)
            {
                this.StatusCode = (int)httpStatusException.Status;
                this.ErrorCode = httpStatusException.ErrorCode;
            }
        }

        public class HttpStatusException : Exception
        {
            public HttpStatusCode Status { get; set; }
            public string ErrorCode { get; set; }
            public HttpStatusException(HttpStatusCode code, string msg, string ErrorCode = null) : base(msg)
            {
                this.Status = code;
                this.ErrorCode = ErrorCode;
            }
        }
    }
}
