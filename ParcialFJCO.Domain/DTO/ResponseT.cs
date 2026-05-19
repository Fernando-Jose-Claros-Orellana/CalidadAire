using System;
using System.Collections.Generic;
using System.Text;

namespace ParcialFJCO.Domain.DTO
{
    public class ResponseT
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
