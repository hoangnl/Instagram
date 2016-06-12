using Instagram.Controllers;
using Instagram.Helpers;
using Instagram.Model.Common;
using Instagram.Model.Repository;
using Instagram.Model.UnitOfWork;
using Instagram.Service.Message;
using Instagram.ViewModel.Feed;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Mvc;

namespace Instagram.Test
{
    [TestFixture]
    public class MessageSerivceTest
    {
        [Test]
        public void GetPagingMessage_ValidInput_ReturnValidMessageWrapperViewModel()
        {
            //Arrange
            IDatabaseFactory databaseFactory = Substitute.For<IDatabaseFactory>();
            UnitOfWork unitOfWork = new UnitOfWork(databaseFactory);
            IMessageRepository messageRepository = Substitute.For<IMessageRepository>();
            unitOfWork.MessageRepository = messageRepository;
            messageRepository.GetMany(Arg.Any<Expression<Func<Model.EDM.Message, bool>>>()).Returns(new List<Model.EDM.Message>() {
                new Model.EDM.Message() { FromUserId = "123", ToUserId ="456" }
            });
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            unitOfWork.UserRepository = userRepository;
            userRepository.GetWithInclude(Arg.Any<Expression<Func<Model.EDM.User, bool>>>()).Returns(new List<Model.EDM.User> {
                new Model.EDM.User() { UserId ="123" },
                new Model.EDM.User() { UserId ="456" }
            });
            MessageService messageSerivce = new MessageService(unitOfWork);

            //Act
            var messageWrapper = messageSerivce.GetPagingMessage("123", 0, 5);
            var userViewModels = messageWrapper.Users as List<UserViewModel>;
            //Assert
            Assert.IsTrue(userViewModels[0].UserId == "123" && userViewModels[1].UserId == "456");

        }

        [Test]
        public void GetPagingMessage_ValidInput_ReturnEmptyMessageWrapperViewModel()
        {
            //Arrange
            IDatabaseFactory databaseFactory = Substitute.For<IDatabaseFactory>();
            UnitOfWork unitOfWork = new UnitOfWork(databaseFactory);
            IMessageRepository messageRepository = Substitute.For<IMessageRepository>();
            unitOfWork.MessageRepository = messageRepository;
            messageRepository.GetMany(Arg.Any<Expression<Func<Model.EDM.Message, bool>>>()).Returns(new List<Model.EDM.Message>() {
                new Model.EDM.Message() { FromUserId = "123", ToUserId ="456" }
            });
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            unitOfWork.UserRepository = userRepository;
            userRepository.GetWithInclude(Arg.Any<Expression<Func<Model.EDM.User, bool>>>()).Returns(x => null);
            MessageService messageSerivce = new MessageService(unitOfWork);

            //Act
            var messageWrapper = messageSerivce.GetPagingMessage("123", 0, 5);
            //Assert
            Assert.IsNull(messageWrapper.Users);

        }
    }
}
