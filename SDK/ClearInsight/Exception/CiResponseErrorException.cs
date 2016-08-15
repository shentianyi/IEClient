using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    /// <summary>
    /// Class<c>CIArgumentErrorException</c>
    /// Argument Error Exception
    /// </summary>
  public  class CiResponseErrorException : ClearInsightException
    {
        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        public CiResponseErrorException()
        { }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="message">Error message</param>
        public CiResponseErrorException(string message) : base(message)
        { }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">System Exception</param>
        public CiResponseErrorException(string message, System.Exception innerException) 
            :base (message,innerException)
        {
            
        }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
        /// <param name="context">System.Runtime.Serialization.StreamingContext</param>
        protected CiResponseErrorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
