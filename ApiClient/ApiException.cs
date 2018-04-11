using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiClient
{
    public sealed class ApiException<TRequest> : Exception
    {
        #region Private Variables
        TRequest _Request;
        #endregion

        #region Constructors
        public ApiException(TRequest request, HttpRequestException ex) : base(ex.Message, ex)
        {
            _Request = request;
        }
        #endregion

        #region Public Properties
        public TRequest Request
        {
            get
            {
                return _Request;
            }
        }
        #endregion
    }
}
