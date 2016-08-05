using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Model
{
    /// <summary>
    /// Enum <c>CIResponseCode</c>
    /// <remarks>Enum for response http code,for error process</remarks>
    /// </summary>
    public enum CIResponseCode
    {
        NoServerError = 0,
        /// <summary>
        /// member <c>Unauthorized</c>
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// member <c>ArgumentError</c>422
        /// </summary>
        ArgumentError=422,

        ServerError = 500,
    }

    /// <summary>
    /// Enum <c>CIRequest</c>
    /// Request enum defines here
    /// </summary>
    public enum CIRequest
    {
        /// <summary>
        /// member <c>MAXKPIENTRYCOUNT</c>
        /// <remarks>define the max kpientry count</remarks>
        /// </summary>
        MAXKPIENTRYCOUNT = 500,
    }
}
