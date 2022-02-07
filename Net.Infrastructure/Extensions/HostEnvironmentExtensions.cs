using Microsoft.Extensions.Hosting;

// ReSharper disable UnusedMember.Global

namespace Net.Infrastructure.Extensions
{
    public static class HostEnvironmentExtensions
    {
        public const string TestModeEnvironment = "TestMode";
        public const string TestEnvironment = "Test";

        /// <summary>
        /// True если EnvironmentName == "TestMode"
        /// </summary>
        public static bool IsTestMode(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(TestModeEnvironment);
        }

        /// <summary>
        /// True если EnvironmentName == "Test"
        /// </summary>
        public static bool IsTest(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(TestEnvironment);
        }
    }
}
