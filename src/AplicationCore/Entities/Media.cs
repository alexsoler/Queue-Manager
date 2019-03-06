using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Media : BaseEntity
    {
        public int Id { get; set; }
        public bool Video { get; set; }
        public bool Audio { get; set; }
        public bool Img { get; set; }
        public bool Used { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
    }
}
