﻿namespace Identity.ServiceResponder
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

        public string? StatusCode { get; set; }

        public List<string> ErrorMessages { get; set; } = null;
    }
}
