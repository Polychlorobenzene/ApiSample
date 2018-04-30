using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSample.Data.Entities
{
    public class PagedSearchDto
    {
        /// <summary>
        /// Number of records that make up a page.
        /// <defalut>25</defalut>
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// Current page Number. Determines how many records to skip before returning the set.
        /// <default>1</default>
        /// </summary>
        public int? PageNumber { get; set; }
        /// <summary>
        /// Column name to order the results by
        /// <default>PersonId</default>
        /// </summary>
        public string OrderByColumn { get; set; }
        /// <summary>
        /// Order By Ascending?
        /// <default>true</default>
        /// </summary>
        public bool? OrderAscending { get; set; }
        /// <summary>
        /// Total Rows the query returns before paging.
        /// </summary>
        public int? TotalRows { get; set; }

    }
}