using System;

namespace OSL.Forum.Core.Utilities
{
    public class DateTimeUtility : IDateTimeUtility
    {
        private static DateTimeUtility _dateTimeUtility;

        private DateTimeUtility()
        {

        }

        public static DateTimeUtility Create()
        {
            if (_dateTimeUtility == null)
            {
                _dateTimeUtility = new DateTimeUtility();
            }

            return _dateTimeUtility;
        }

        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
