using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
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
