using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    class CiUnauthorizedErrorException : ClearInsightException
    {
        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        public CiUnauthorizedErrorException()
        { }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="message">Error message</param>
        public CiUnauthorizedErrorException(string message)
            : base(message)
        { }
    }
}
