namespace StarStuff.Test
{
    using System;
    using Xunit;

    public abstract class BaseTest
    {
        protected void CompareDates(DateTime expected, DateTime actual)
        {
            Assert.Equal(expected.Year, actual.Year);
            Assert.Equal(expected.Month, actual.Month);
            Assert.Equal(expected.Day, actual.Day);
        }

        protected void CompareDatesExact(DateTime expected, DateTime actual)
        {
            this.CompareDates(expected, actual);
            Assert.Equal(expected.Hour, actual.Hour);
            Assert.Equal(expected.Minute, actual.Minute);
        }
    }
}