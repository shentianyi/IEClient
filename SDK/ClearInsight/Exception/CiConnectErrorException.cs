using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    class CiConnectErrorException : ClearInsightException
    {
        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        public CiConnectErrorException()
        { }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="message">Error message</param>
        public CiConnectErrorException(string message)
            : base(message)
        { }
    }
}
