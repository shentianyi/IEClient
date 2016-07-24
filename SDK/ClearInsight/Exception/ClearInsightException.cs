using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    /// <summary>
    /// Class <c>ClearInsightException</c>
    /// Base ClearInsight Exception
    /// </summary>
    [Serializable]
    class ClearInsightException : ApplicationException
    {
        /// <summary>
        /// ClearInsightException
        /// </summary>
        public ClearInsightException()
        { }

        /// <summary>
        /// ClearInsightException
        /// </summary>
        /// <param name="message">Error Message</param>
        public ClearInsightException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// ClearInsightException
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">System Exception</param>
        public ClearInsightException(string message, System.Exception innerException)
            :base(message,innerException)
        {
            
        }

        /// <summary>
        /// ClearInsigheException
        /// </summary>
        /// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
        /// <param name="context">System.Runtime.Serialization.StreamingContext</param>
        protected ClearInsightException(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
