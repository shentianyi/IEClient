using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Model
{
    /// <summary>
    /// Class <c>CIResponse</c>
    /// Model for ClearInsightAPI response
    /// </summary>
    public class CIResponse
    {
        /// <summary>
        /// Property <c>Code</c>
        /// HttpStatusCode
        /// </summary>
        public int Code { get; set; }
        ///public int result { get; set; }

        /// <summary>
        /// Property <c>Content</c>
        /// Content for error message
        /// </summary>
        public string Content { get; set; }
        ///public string data { get; set; }

        /// <summary>
        /// Function <c>ToString</c>
        /// return string with code and content
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "StatusCode:" + Code + ", Content:" + Content;
        }
    }
}
