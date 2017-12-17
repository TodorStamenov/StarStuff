namespace StarStuff.Test.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using StarStuff.Data.Models;

    public class UserManagerMock
    {
        public static Mock<UserManager<User>> New
        {
            get
            {
                return new Mock<UserManager<User>>(
                    Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            }
        }
    }
}