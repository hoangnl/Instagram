using System.Collections.Generic;
using System.Linq;
using Instagram.Model.Common;
using Instagram.ViewModel.Feed;
using AutoMapper;
using System;
using Instagram.ViewModel.User;
using Instagram.Service.Common;
using Instagram.Common;

namespace Instagram.Service.Feed
{
    public class UserService : IUserService
    {
        private readonly Model.UnitOfWork.IUnitOfWork UnitOfWork;

        public UserService()
        {
            UnitOfWork = new Model.UnitOfWork.UnitOfWork(new DatabaseFactory());
        }

        public void AddUserInfo(UserViewModel userViewModel)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<UserViewModel, Model.EDM.User>();
            });
            var mapper = config.CreateMapper();
            var user = mapper.Map<Model.EDM.User>(userViewModel);
            UnitOfWork.UserRepository.Add(user);
            UnitOfWork.Commit();
        }

        public IEnumerable<UserViewModel> Autocomplete(string searchTerm)
        {
            IEnumerable<UserViewModel> userViewModels = new List<UserViewModel>();
            IEnumerable<Model.EDM.User> userList = UnitOfWork.UserRepository.SearchUser(searchTerm).Take(5);
            if (userList.Count() > 0)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                userViewModels = mapper.Map<IEnumerable<UserViewModel>>(userList);
            }
            return userViewModels;
        }

        public bool Follow(string userId, string userFollowId)
        {
            var userFollow = new Model.EDM.UserFollow()
            {
                UserId = userId,
                UserFollowId = userFollowId,
                FollowDate = DateTime.Now
            };
            UnitOfWork.UserFollowRepository.Add(userFollow);
            return UnitOfWork.Commit();
        }

        public IEnumerable<FeedLikeViewModel> GetLikedUserList(long feedId)
        {
            //IEnumerable<UserViewModel> userViewModels = new List<UserViewModel>();
            var feedLikeViewModel = new List<FeedLikeViewModel>();
            var feedLike = UnitOfWork.FeedLikeRepository.GetWithInclude(p => p.FeedId == feedId, "User");

            //var users = feed.Users;
            if (feedLike != null)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                feedLikeViewModel = mapper.Map<List<FeedLikeViewModel>>(feedLike);
            }
            return feedLikeViewModel;
        }

        public IEnumerable<UserViewModel> GetSuggetions(string userId)
        {
            IEnumerable<UserViewModel> userViewModels = new List<UserViewModel>();
            var users = UnitOfWork.UserRepository.GetWithInclude(e => e.UserId != userId && e.Feeds.Count > 0, "Feeds", "Feeds.Files", "Feeds.Files.FileType", "Feeds.Files.FileFolder").OrderByDescending(e => e.Feeds.Count()).Take(10);
            if (users.Count() > 0)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();

                });
                var mapper = config.CreateMapper();
                userViewModels = mapper.Map<IEnumerable<UserViewModel>>(users);
            }
            return userViewModels;
        }

        public UserViewModel GetUserInfo(string userId)
        {
            var userViewModel = new UserViewModel();
            var user = UnitOfWork.UserRepository.GetById(userId);
            if (user != null)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>();
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                userViewModel = mapper.Map<UserViewModel>(user);
            }
            return userViewModel;
        }

        public UserProfileViewModel GetUserProfileByUserId(string id, string loginUserId)
        {
            var userProfile = new UserProfileViewModel();
            var user = UnitOfWork.UserRepository.GetWithInclude(e => e.UserId == id, "Feeds", "UserFollows", "UserFollows1").FirstOrDefault();
            if (user != null)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.User, UserProfileViewModel>().AfterMap((s, d) =>
                    {
                        d.PostNo = s.Feeds.Count();
                        d.FollowerNo = s.UserFollows.Count();
                        d.FollowingNo = s.UserFollows1.Count();
                        var aspUser = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId);
                        d.UserName = aspUser.UserName;
                        d.Email = aspUser.Email;
                        d.GenderEnum = (GenderEnum)s.Gender;
                        d.Avartar = ImageCommon.GetAvatarLink(id, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        //d.Following = false;
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                    });
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                userProfile = mapper.Map<UserProfileViewModel>(user);
            }
            return userProfile;
        }

        public bool HasNotFollower(string userId)
        {
            bool result = UnitOfWork.UserFollowRepository.GetMany(e => e.UserFollowId == userId).Count() == 0;
            return result;
        }

        public void SaveAvatar(UserViewModel userViewModel)
        {

            var user = UnitOfWork.UserRepository.GetById(userViewModel.UserId);
            if (user != null)
            {
                user.FileTypeId = userViewModel.FileTypeId;
                user.Avartar = userViewModel.Avartar;
                UnitOfWork.UserRepository.Update(user);
                UnitOfWork.Commit();
            }
        }

        public UserProfileViewModel SaveUserProfile(UserProfileViewModel userProfileViewModel)
        {
            var user = UnitOfWork.UserRepository.GetById(userProfileViewModel.UserId);
            if (user != null)
            {
                user.FullName = userProfileViewModel.FullName;
                user.Bio = userProfileViewModel.Bio;
                user.Website = userProfileViewModel.Website;
                user.PhoneNo = userProfileViewModel.PhoneNo;
                user.Gender = (int)userProfileViewModel.GenderEnum;
                UnitOfWork.UserRepository.Update(user);
                if (UnitOfWork.Commit())
                {
                    userProfileViewModel = GetUserProfileByUserId(userProfileViewModel.UserId, userProfileViewModel.UserId);
                }
            }
            return userProfileViewModel;
        }

        public IEnumerable<UserViewModel> SearchUser(string searchTerm)
        {
            IEnumerable<UserViewModel> userViewModels = new List<UserViewModel>();
            IEnumerable<Model.EDM.User> userList = UnitOfWork.UserRepository.SearchUser(searchTerm);
            if (userList.Count() > 0)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.User, UserViewModel>();
                });
                var mapper = config.CreateMapper();
                userViewModels = mapper.Map<IEnumerable<UserViewModel>>(userList);
            }
            return userViewModels;
        }

        public bool Unfollow(string userId, string userFollowId)
        {
            UnitOfWork.UserFollowRepository.Delete(e => e.UserId == userId && e.UserFollowId == userFollowId);
            return UnitOfWork.Commit();
        }
    }
}
