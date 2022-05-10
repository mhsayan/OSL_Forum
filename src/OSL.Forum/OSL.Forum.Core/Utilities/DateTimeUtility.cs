using System;

namespace OSL.Forum.Core.Utilities
{
    public class DateTimeUtility : IDateTimeUtility
    {
        public DateTimeUtility()
        {

        }
        
        public DateTime Now => DateTime.Now;
    }
}
