using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Basic
{
    public class ResponseHandler
    {

        // Function To Handling Requers

        public Response<T> Updated<T>(string msg)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = msg ?? "Succed Update",

            };
        }
        public Response<T> Deleted<T>(string msg)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = msg,

            };
        }
        public Response<T> Success<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Success Process",
                Meta = Meta
            };
        }
        public Response<T> Unauthorized<T>(string Massege = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Success = false,
                Message = Massege ?? "UnAuthorized"
            };
        }
        public Response<T> BadRequest<T>(string Message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = Message ?? "Bad Request"
            };
        }
        public Response<T> UnprocessableEntity<T>(string Message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Success = false,
                Message = Message ?? "Unprocessable Entity"
            };
        }
        public Response<T> Conflict<T>(string Message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Conflict,
                Success = false,
                Message = Message ?? "Conflict"
            };
        }
        public Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Success = false,
                Message = message ?? "Not Found" 
            };
        }
        public Response<T> Created<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Success = true,
                Message = "Success Create",
                Meta = Meta
            };
        }
    }
}
