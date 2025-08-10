using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ExerciseTrackingAnalytics.Security.DataProtection
{
    public class KeyRing : ILookupProtectorKeyRing
    {
        private readonly string _applicationDataProtectionKey;

        public KeyRing(IOptions<DataProtectionKeyRingOptions> options)
        {
            _applicationDataProtectionKey = options.Value.MasterKey;
        }

        public const string ApplicationDataProtectionKeyName = "Dash Exercise Tracking Analytics";

        public string this[string keyId] =>
            keyId == ApplicationDataProtectionKeyName
                ? _applicationDataProtectionKey
                : throw new InvalidOperationException($"Could not find key '{keyId}'");

        public string CurrentKeyId => ApplicationDataProtectionKeyName;

        public IEnumerable<string> GetAllKeyIds()
        {
            return new string[] { ApplicationDataProtectionKeyName };
        }
    }
}
