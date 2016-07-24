using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    /// <summary>
    /// Class <c>CIRequestTooLong</c>
    /// throw then http request too long
    /// </summary>
    class CiRequestTooLong : ClearInsightException
    {
        /// <summary>
        /// CiRequestTooLong
        /// </summary>
        public CiRequestTooLong()
        { }

        /// <summary>
        /// constructor<c>CIRequestTooLong</c>
        /// </summary>
        /// <param name="message">Error Message</param>
        public CiRequestTooLong(string message):base(message)
        { }

        /// <summary>
        /// constructor<c>CIRequestTooLong</c>
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">System.Exception</param>
        public CiRequestTooLong(string message,System.Exception innerException)
            :base(message,innerException)
        { }

        /// <summary>
        /// CiRequestTooLong
        /// </summary>
        /// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
        /// <param name="context">System.Runtime.Serialization.StreamingContext</param>
        protected CiRequestTooLong(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
