using System.Net;
using ExerciseTrackingAnalytics.BusinessLogic;

namespace ExerciseTrackingAnalytics.Extensions
{
    public static class BusinessLogicResponseExtensions
    {
        public static HttpStatusCode GetHttpStatus(this BusinessLogicResponse bizLogicResponse)
        {
            if (bizLogicResponse == null)
                return HttpStatusCode.InternalServerError;

            if (bizLogicResponse.IsSuccessful)
                return HttpStatusCode.OK;

            switch (bizLogicResponse.ErrorType)
            {
                case ErrorType.Conflict:
                    return HttpStatusCode.Conflict;

                case ErrorType.RequestedEntityNotFound:
                    return HttpStatusCode.NotFound;

                case ErrorType.UserNotAuthorized:
                    return HttpStatusCode.Forbidden;

                case ErrorType.RequestNotValid:
                    return HttpStatusCode.BadRequest;

                case ErrorType.RuntimeException:
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
