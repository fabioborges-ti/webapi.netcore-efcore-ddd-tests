using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Api.Service.Test.Helpers
{
    public static class TestLogHelpers
    {
        public static void VerifyLogger<T>(Mock<ILogger<T>> logger, LogLevel loggedLevel, Exception exception, Times times, string stringInLogMessage = null, bool strictEqual = false)
        {
            logger.Verify(l => 
                l.Log(loggedLevel, 
                    It.IsAny<EventId>(), 
                    It.Is<It.IsAnyType>((object v, Type _) => CompareLogMessage(v, stringInLogMessage, strictEqual)), 
                    exception, (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), times);
        }

        private static bool CompareLogMessage(object v, string stringInLogMessage, bool strictEqual)
        {
            if (v == null) return false;
            if (stringInLogMessage == null) return true;

            var actualLoggedMessage = v.ToString();

            if (string.IsNullOrEmpty(actualLoggedMessage)) return false;

            return strictEqual ? actualLoggedMessage.Equals(stringInLogMessage) : actualLoggedMessage.Contains(stringInLogMessage);
        }
    }
}
