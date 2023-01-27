namespace DisprzTraining.Model.Result
{
    public class AppointmentErrorResponse
    {
        public static ErrorResponse ConflictResponse = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Conflict occured between different meetings",
            errorCode = "AC_001"
        };
        public static ErrorResponse PastTiming = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Appointment for past time and days are not allowed",
            errorCode = "AC_002"
        };

        public static ErrorResponse EndTimeLessThanStartTime = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Appointment end time should be greater than start time",
            errorCode = "AC_003"
        };

        public static ErrorResponse InputTimeNull = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Appointment input time should not be null",
            errorCode = "AC_004"
        };

        public static ErrorResponse SameTiming = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Input start time and end time should not be same",
            errorCode = "AC_005"
        };
        public static ErrorResponse DataNotFound = new ErrorResponse()
        {
            language = "en",
            errorMessage = "No Appointment found with the given Id",
            errorCode = "AC_006"
        };

        public static ErrorResponse TimeRange = new ErrorResponse()
        {
            language = "en",
            errorMessage = "StartTime and End Time should fall within Current Date",
            errorCode = "AC_007"
        };
    }
}