using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class ApiDto
    {
        public int Id { get; set; }
        public int? MasterId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public ApiDto Parent { get; set; }
    }

    public class MasterApiDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public ICollection<MasterApiDto> Children { get; set; }
        public ICollection<ApiDto> Workers { get; set; }
        public MasterApiDto Parent { get; set; }

    }
}