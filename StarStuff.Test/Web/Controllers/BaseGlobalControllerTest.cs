namespace StarStuff.Test.Web.Controllers
{
    using StarStuff.Web.Infrastructure.Helpers;
    using Xunit;

    public class BaseGlobalControllerTest : BaseTest
    {
        protected void AssertPages(BasePageViewModel expected, BasePageViewModel actual)
        {
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.NextPage, actual.NextPage);
            Assert.Equal(expected.PrevPage, actual.PrevPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
        }
    }
}