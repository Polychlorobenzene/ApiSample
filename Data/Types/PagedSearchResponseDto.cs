using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSample.Data.Entities
{
    public class PagedSearchResponseDto<T> : PagedSearchDto
    {       
        public T Result { get; set; }
    }

    public class PersonSearchResultDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
