using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests.ExtensionMethods
{
    [ExcludeFromCodeCoverage]
    public static class AssertHelper
    {
        public static void Throws<T>(Action action, string expectedMessage) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T exc)
            {
                Assert.AreEqual(expectedMessage, exc.Message);

                return;
            }

            Assert.Fail("Exception of type {0} should be thrown.", typeof(T));
        }
    }
}