using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearInsight.Model;
using ClearInsight.Exception;

namespace ClearInsight.Validation
{
    /// <summary>
    /// Validator <c>KpiEntryValidator</c>
    /// </summary>
    public class KpiEntryValidator
    {
        /// <summary>
        /// Constructor<c>KpiEntryValidator</c>
        /// </summary>
        public KpiEntryValidator()
        { }

        /// <summary>
        /// function <c>validate</c>
        /// </summary>
        /// <param name="entries">List KpiEntry</param>
        public void validate(List<KpiEntry> entries)
        {
            foreach(KpiEntry entry in entries)
            {
                if (!IsIntegral(entry))
                {
                    throw new CiPropertyMissingException("Property Missing,please set all the property!");
                }
            }
        }

        /// <summary>
        /// function <c>IsIntegral</c>
        /// Check if all the properties of kpientry are seted
        /// </summary>
        /// <param name="entry">KpiEntry</param>
        /// <returns>bool</returns>
        private bool IsIntegral(KpiEntry entry)
        {
            foreach (var prop in entry.GetType().GetProperties())
            {
                if (prop.GetValue(entry, null) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
