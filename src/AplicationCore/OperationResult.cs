using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore
{
    public class OperationResult
    {
        public OperationResult()
        {
            Errors = new List<string>();
        }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}
