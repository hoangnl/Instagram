using Instagram.Controllers;
using Instagram.Helpers;
using Instagram.Service.Feed;
using Instagram.Service.Message;
using Instagram.ViewModel.Feed;
using Instagram.ViewModel.User;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Security.Principal;
using System.Web.Mvc;

namespace Instagram.Test
{
    [TestFixture]
    public class MessageControllerTest
    {
        [Test]
        public void Index_ValidInput_ValidIndexView()
        {
            //Arrange
            IMessageService MessageService = Substitute.For<IMessageService>();
            IUserHelper UserHelper = Substitute.For<IUserHelper>();

            MessageService.GetPagingMessage(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new MessageWrapperViewModel());
            UserHelper.GetCurrentUserIdFromClaim(Arg.Any<IPrincipal>()).Returns("hoangnl");

            MessageController controller = new MessageController(MessageService, UserHelper);
            //Act
            ViewResult editView = controller.Index() as ViewResult;
            //Assert
            Assert.IsEmpty(editView.ViewName);
            Assert.IsTrue((editView.Model as MessageWrapperViewModel) != null);
        }

    }
}
