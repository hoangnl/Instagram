using Instagram.Controllers;
using Instagram.Helpers;
using Instagram.Service.Feed;
using Instagram.ViewModel.User;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Security.Principal;
using System.Web.Mvc;

namespace Instagram.Test
{
    [TestFixture]
    public class ProfileControllerTest
    {
        [Test]
        [Category("Test1")]
        public void Edit_ValidLogginUser_ValidEditView()
        {
            //Arrange
            IUserHelper UserHelper = Substitute.For<IUserHelper>();
            IUserService UserService = Substitute.For<IUserService>();
            UserHelper.GetCurrentUserIdFromClaim(Arg.Any<IPrincipal>()).Returns("hoangnl");
            UserService.GetUserProfileByUserId(Arg.Any<string>(), Arg.Any<string>()).Returns(new UserProfileViewModel() { UserName = "hoangnl" });
            ProfileController controller = new ProfileController(UserHelper, UserService, null);
            //Act
            ViewResult editView = controller.Edit() as ViewResult;
            //Assert
            Assert.IsTrue((editView.Model as UserProfileViewModel).UserName == "hoangnl");
        }

    }
}
