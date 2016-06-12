using System.Collections.Generic;
using System.Linq;
using Instagram.Model.Common;
using Instagram.ViewModel.Feed;
using AutoMapper;
using System;
using Instagram.Service.Common;
using Instagram.Common;
using Instagram.Service.Message;
using Instagram.Model.UnitOfWork;

namespace Instagram.Service.Message
{
    public class MessageService : IMessageService
    {
        private UnitOfWork unitOfWork;

        public MessageService()
        {
            unitOfWork = new UnitOfWork(new DatabaseFactory());
        }
        public MessageService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 1. Lấy tất cả fromuserid gửi tin nhắn đến tôi
        /// 2. Lấy tất cả ToUSerId tôi gửi tin nhắn đến
        /// 3. Lấy thông tin User từ bảng User từ 2 list trên
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MessageWrapperViewModel GetPagingMessage(string userId, int pageIndex, int pageSize)
        {
            List<string> fromUserIds = unitOfWork.MessageRepository.GetMany(e => e.FromUserId == userId).Select(e => e.ToUserId).Distinct().ToList();
            List<string> toUserIds = unitOfWork.MessageRepository.GetMany(e => e.ToUserId == userId).Select(e => e.FromUserId).Distinct().ToList();
            IEnumerable<Model.EDM.User> users = unitOfWork.UserRepository.GetWithInclude(e => fromUserIds.Contains(e.UserId) || toUserIds.Contains(e.UserId));
            if (users != null)
            {
                users = users.Skip(pageIndex).Take(pageSize).ToList();
            }
            MessageWrapperViewModel messageWrapperVM = new MessageWrapperViewModel();
            if (users != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    cfg.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    cfg.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    cfg.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    cfg.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    cfg.CreateMap<Model.EDM.File, FileViewModel>();
                    cfg.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                messageWrapperVM.Users = mapper.Map<IEnumerable<UserViewModel>>(users);
                string firstUserId = users.FirstOrDefault().UserId;
                IEnumerable<Model.EDM.Message> Messages = unitOfWork.MessageRepository.GetWithInclude(e => (e.FromUserId == firstUserId && e.ToUserId == userId) || (e.FromUserId == userId && e.ToUserId == firstUserId), "User", "User1").OrderBy(e => e.CreateDate); config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Model.EDM.Message, MessageViewModel>(); cfg.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = unitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    cfg.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    cfg.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    cfg.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    cfg.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    cfg.CreateMap<Model.EDM.File, FileViewModel>();
                    cfg.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                    cfg.CreateMap<Model.EDM.MessageType, MessageTypeViewModel>();
                    cfg.CreateMap<Model.EDM.MessageStatusType, MessageStatusTypeViewModel>();
                });
                mapper = config.CreateMapper();
                messageWrapperVM.Messages = mapper.Map<IEnumerable<MessageViewModel>>(Messages);
            }
            return messageWrapperVM;
        }

        public PrivateMessageViewModel GetMessageById(long messageId)
        {
            PrivateMessageViewModel privateMessage = null;
            Model.EDM.Message message = unitOfWork.MessageRepository.GetWithInclude(e => e.MessageId == messageId, "User", "User1").FirstOrDefault();
            if (message != null)
            {
                privateMessage = new PrivateMessageViewModel(message.MessageId, unitOfWork.AspNetUserRepository.GetBy(e => e.Id == message.User.UserId).UserName, message.User.UserId, message.User1.UserId, message.User.FullName, ImageCommon.GetAvatarLink(message.User.UserId, message.User.FileTypeId, message.User.FileType).Replace("~", ""), message.Body, message.CreateDate);
            }
            return privateMessage;
        }


        public long SendMessage(string userId, string toUserId, string message)
        {
            Model.EDM.Message newMessage = new Model.EDM.Message() { Body = message, FromUserId = userId, ToUserId = toUserId, CreateDate = DateTime.Now, MessageStatusTypeId = (int)MessageStatusTypes.Unread, MessageTypeId = (int)MessageTypes.Message };
            unitOfWork.MessageRepository.Add(newMessage);
            unitOfWork.Commit();
            return newMessage.MessageId;
        }

        public IEnumerable<MessageViewModel> GetMessageByUserId(string currentUserId, string userId)
        {
            IEnumerable<MessageViewModel> MessageViewModels = new List<MessageViewModel>();
            IEnumerable<Model.EDM.Message> Messages = unitOfWork.MessageRepository.GetWithInclude(e => (e.FromUserId == currentUserId && e.ToUserId == userId) || (e.FromUserId == userId && e.ToUserId == currentUserId), "User", "User1").OrderBy(e => e.CreateDate);
            if (Messages != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Model.EDM.Message, MessageViewModel>(); cfg.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = unitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    cfg.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    cfg.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    cfg.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    cfg.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    cfg.CreateMap<Model.EDM.File, FileViewModel>();
                    cfg.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                    cfg.CreateMap<Model.EDM.MessageType, MessageTypeViewModel>();
                    cfg.CreateMap<Model.EDM.MessageStatusType, MessageStatusTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                MessageViewModels = mapper.Map<IEnumerable<MessageViewModel>>(Messages);
            }
            return MessageViewModels;
        }

    }
}
