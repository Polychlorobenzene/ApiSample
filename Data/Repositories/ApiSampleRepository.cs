using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiSample.Data.Entities;

namespace ApiSample.Data.Repositories
{
    public class ApiSampleRepository : IDisposable
    {
        public PagedSearchResponseDto<List<PersonSearchResultDto>> SearchPeople(PagedSearchDto dto)
        {
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                SqlParameter pageSize = new SqlParameter("@PageSize", dto.PageSize ?? (object)DBNull.Value)
                {
                    DbType = System.Data.DbType.Int32
                };
                SqlParameter pageNumber = new SqlParameter("@PageNumber", dto.PageNumber ?? (object)DBNull.Value)
                {
                    DbType = System.Data.DbType.Int32
                };
                SqlParameter orderBy = new SqlParameter("@OrderBy", string.IsNullOrEmpty(dto.OrderByColumn) ? (object)DBNull.Value : dto.OrderByColumn);
                SqlParameter orderAsc = new SqlParameter("@OrderAsc", dto.OrderAscending ?? (object)DBNull.Value);
                SqlParameter totalRows = new SqlParameter("@TotalRows", 0)
                {
                    DbType = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Output
                };

                List<PersonSearchResultDto> results = context.Database.SqlQuery<PersonSearchResultDto>("EXEC dbo.GetPeople @PageSize, @PageNumber, @OrderBy, @OrderAsc, @TotalRows OUTPUT",
                    pageSize, pageNumber, orderBy, orderAsc, totalRows).ToList();

                PagedSearchResponseDto<List<PersonSearchResultDto>> response = new PagedSearchResponseDto<List<PersonSearchResultDto>>
                {
                    PageSize = dto.PageSize,
                    PageNumber = dto.PageNumber,
                    OrderByColumn = dto.OrderByColumn,
                    OrderAscending = dto.OrderAscending,
                    TotalRows = (int?)totalRows.Value,
                    Result = results
                };
                return response;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApiSampleRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
