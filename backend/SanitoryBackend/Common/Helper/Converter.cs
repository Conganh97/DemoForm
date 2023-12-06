using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Converter
    {
        public static readonly DateFormatHandling JSONDateFormatHandling = DateFormatHandling.IsoDateFormat;
        public static readonly DateTimeZoneHandling JSONDateTimeZoneHandling = DateTimeZoneHandling.Utc;
        public static readonly string JSONDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
        public static readonly NullValueHandling JSONNullValueHandling = NullValueHandling.Ignore;
        public static readonly ReferenceLoopHandling JSONReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }
}
