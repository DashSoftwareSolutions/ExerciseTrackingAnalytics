using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.Security.DataProtection
{
    public class LookupProtector : ILookupProtector
    {
        private IDataProtector _dataProtector;

        public LookupProtector(IDataProtectionProvider provider)
        {
            _dataProtector = provider.CreateProtector(DataProtectionPurpose);
        }

        public string Protect(string keyId, string data)
        {
            return _dataProtector.Protect(data);
        }

        public string Unprotect(string keyId, string data)
        {
            return _dataProtector.Unprotect(data);
        }
    }
}
