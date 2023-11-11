using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace ExerciseTrackingAnalytics.Tests.Security.DataProtection
{
    public class DataProtectionFixture
    {
        private readonly ITestOutputHelper output;

        public DataProtectionFixture(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Can_Protect_And_Unprotect()
        {
            var plainText = "The quick, brown fox jumps over the lazy dog.";
            output.WriteLine($"Original plaintext: {plainText}");

            // add data protection services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            var dataProtectionProvider = services.GetRequiredService<IDataProtectionProvider>();
            var purpose = "DataProtectionUnitTestPurpose";
            var dataProtector = dataProtectionProvider.CreateProtector(purpose);

            var cipherText = dataProtector.Protect(plainText);
            output.WriteLine($"Encrypted ciphertext: {cipherText}");

            var recoveredPlainText = dataProtector.Unprotect(cipherText);
            output.WriteLine($"Recovered plaintext: {recoveredPlainText}");
            Assert.Equal(plainText, recoveredPlainText);
        }
    }
}