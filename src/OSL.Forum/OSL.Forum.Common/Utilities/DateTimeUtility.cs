using System;

namespace OSL.Forum.Common.Utilities
{
    public class DateTimeUtility : IDateTimeUtility
    {
        public DateTimeUtility()
        {

        }
        
        public DateTime Now => DateTime.Now;
    }
}
