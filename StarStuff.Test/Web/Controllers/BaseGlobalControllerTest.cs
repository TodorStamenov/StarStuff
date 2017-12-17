namespace StarStuff.Test.Web.Controllers
{
    using FluentAssertions;
    using StarStuff.Web.Infrastructure.Helpers;

    public class BaseGlobalControllerTest
    {
        protected void AssertPages(BasePageViewModel expected, BasePageViewModel actual)
        {
            actual.CurrentPage.Should().Be(expected.CurrentPage);
            actual.NextPage.Should().Be(expected.NextPage);
            actual.PrevPage.Should().Be(expected.PrevPage);
            actual.TotalPages.Should().Be(expected.TotalPages);
        }
    }
}