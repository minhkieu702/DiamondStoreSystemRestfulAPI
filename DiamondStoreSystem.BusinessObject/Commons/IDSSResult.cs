using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.Commons
{
    public interface IDSSResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }
    public class DSSResult : IDSSResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public DSSResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public DSSResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public DSSResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
