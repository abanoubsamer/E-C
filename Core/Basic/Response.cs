using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Basic
{
    public class Response<T>
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message {  get; set; } 
        public object Meta { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public Response() { }
        public Response(string msg)
        {
            Success = false;
            Message = msg;
        }
        public Response(T data, string msg = null)
        {
            Success = true;
            Message = msg;
            Data = data;
                
        }
        public Response(string msg,bool sucess)
        {
            Success = sucess;
            Message = msg;
        }





    }
}
