using System.Net;
using ExerciseTrackingAnalytics.Extensions;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public class BusinessLogicResponse
    {
        public bool IsSuccessful
        {
            get { return ErrorType == ErrorType.None && Exception == null; }
        }

        public ErrorType ErrorType { get; set; } = ErrorType.None;

        public Exception? Exception { get; set; }

        public string? ErrorMessage
        {
            get { return ErrorMessages?.FirstOrDefault() ?? Exception?.Message; }
        }

        private List<string>? _errorMessages;
        public IReadOnlyCollection<string>? ErrorMessages
        {
            get { return _errorMessages?.AsReadOnly(); }
        }

        public BusinessLogicResponse() { }

        public BusinessLogicResponse(ErrorType errorType)
        {
            ErrorType = errorType;
        }

        public BusinessLogicResponse(ErrorType errorType, Exception exception)
        {
            ErrorType = errorType;
            Exception = exception;
        }

        public BusinessLogicResponse(ErrorType errorType, string errorMessage)
        {
            ErrorType = errorType;

            _errorMessages = new List<string>() { errorMessage };
        }

        public BusinessLogicResponse(ErrorType errorType, IEnumerable<string>? errorMessages)
        {
            ErrorType = errorType;

            if (errorMessages.HasAny())
            {
                _errorMessages = new List<string>();

                // We've checked on Line 53 (using `HasAny()` extension method) and are assured that `errorMessages` is not null and not empty at this point.
#pragma warning disable CS8604 // Possible null reference argument.
                _errorMessages.AddRange(errorMessages);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
    }

    public class BusinessLogicResponse<TEntity> : BusinessLogicResponse
    {
        public TEntity? Data { get; set; }

        public BusinessLogicResponse() : base()
        {
            Data = default;
        }

        public BusinessLogicResponse(BusinessLogicResponse src)
            : base(src.ErrorType, src.ErrorMessages)
        {
            Exception = src.Exception;
        }

        public BusinessLogicResponse(TEntity data) : base()
        {
            Data = data;
        }

        public BusinessLogicResponse(TEntity data, ErrorType errorType)
            : base(errorType)
        {
            Data = data;
        }

        public BusinessLogicResponse(TEntity data, ErrorType errorType, Exception exception)
            : base(errorType, exception)
        {
            Data = data;
        }

        public BusinessLogicResponse(TEntity data, ErrorType errorType, string errorMessage)
            : base(errorType, errorMessage)
        {
            Data = data;
        }

        public BusinessLogicResponse(TEntity data, ErrorType errorType, IEnumerable<string> errorMessages)
            : base(errorType, errorMessages)
        {
            Data = data;
        }

        public BusinessLogicResponse(ErrorType errorType) : base(errorType)
        {
        }

        public BusinessLogicResponse(ErrorType errorType, Exception exception) : base(errorType, exception)
        {
        }

        public BusinessLogicResponse(ErrorType errorType, string errorMessage) : base(errorType, errorMessage)
        {
        }

        public BusinessLogicResponse(ErrorType errorType, IEnumerable<string> errorMessages) : base(errorType, errorMessages)
        {
        }
    }
}
