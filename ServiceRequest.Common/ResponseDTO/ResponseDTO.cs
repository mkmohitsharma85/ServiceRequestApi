using System.Net;

namespace ServiceRequest.Common.ResponseDTO
{
    public class ResponseDTO<T>
    {
        public string? Message { get; set; }

        public HttpStatusCode Code { get; set; }

        public Response<T>? Response { get; set; }
    }

    public class Response<T>
    {
        public T? Data { get; set; }
    }
    public class ResponseListDTO<T>
    {
        public string? Message { get; set; }

        public HttpStatusCode Code { get; set; }

        public ResponseList<T>? Response { get; set; }
    }

    public class ResponseList<T>
    {
        public List<T>? Data { get; set; }
    }
}
