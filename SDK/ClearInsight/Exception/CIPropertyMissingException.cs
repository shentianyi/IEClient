using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    /// <summary>
    /// Class <c>CIPropertyMissingException</c>
    /// </summary>
    class CiPropertyMissingException : ClearInsightException
    {
        /// <summary>
        /// CiPropertyMissingException
        /// </summary>
        public CiPropertyMissingException()
        { }
        /// <summary>
        /// CIPropertyMissingException
        /// </summary>
        /// <param name="message">Error Message</param>
        public CiPropertyMissingException(string message) :base(message)
        { }

        /// <summary>
        /// CIPropertyMissingException
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">System Exception</param>
        public CiPropertyMissingException(string message,System.Exception innerException)
            :base(message,innerException)
        { }

        /// <summary>
        /// CiPropertyMissingException
        /// </summary>
        /// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
        /// <param name="context">System.Runtime.Serialization.StreamingContext</param>
        protected CiPropertyMissingException(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
