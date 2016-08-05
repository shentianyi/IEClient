using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Exception
{
    class CiServerErrorException : ClearInsightException
    {
                /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        public CiServerErrorException()
        { }

        /// <summary>
        /// CIArgumentErrorException
        /// </summary>
        /// <param name="message">Error message</param>
        public CiServerErrorException(string message)
            : base(message)
        { }
    }
}
