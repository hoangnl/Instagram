using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instagram.Model.EDM;
using Instagram.Model.Common;
using Instagram.ViewModel.Feed;
using AutoMapper;
using Instagram.Service.Common;

namespace Instagram.Service.Feed
{
    public class FeedService : IFeedService
    {
        private readonly Model.UnitOfWork.IUnitOfWork UnitOfWork;

        public FeedService()
        {
            UnitOfWork = new Model.UnitOfWork.UnitOfWork(new DatabaseFactory());
        }
        public IEnumerable<FeedViewModel> GetAllFeeds()
        {
            IEnumerable<Model.EDM.Feed> newsFeed = UnitOfWork.FeedRepository.
                GetWithInclude(e => 1 == 1, "User", "FeedComments", "FeedLikes", "Files.FileType", "Files.FileFolder").OrderByDescending(p => p.CreatedTime).Take(10);
            IEnumerable<FeedViewModel> feeds = new List<FeedViewModel>();
            if (newsFeed.Count() > 0)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                feeds = mapper.Map<IEnumerable<FeedViewModel>>(newsFeed);
            }
            return feeds;
        }

        public IEnumerable<FeedViewModel> GetFeedsPaging(string userId, int pageIndex, int pageSize)
        {
            List<string> userIdList = UnitOfWork.UserFollowRepository.GetMany(e => e.UserFollowId == userId).Select(e => e.UserId).ToList();
            IEnumerable<Model.EDM.Feed> newsFeed = UnitOfWork.FeedRepository.
    GetWithInclude(e => e.UserId == userId || userIdList.Contains(e.UserId), "User", "FeedComments", "FeedLikes", "Files.FileType", "Files.FileFolder").OrderByDescending(p => p.CreatedTime).Skip(pageSize * pageIndex).Take(pageSize);
            IEnumerable<FeedViewModel> feeds = new List<FeedViewModel>();
            if (newsFeed.Count() > 0)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>().AfterMap((s, d) => d.FeedLikeSummary = new FeedLikeSummary() { FeedId = s.FeedId, Liked = s.FeedLikes.Any(e => e.UserId == userId), TotalLike = s.FeedLikes.Count() });
                    c.CreateMap<Model.EDM.User, UserViewModel>().AfterMap((s, d) =>
                    {
                        d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName;
                        d.Avartar = ImageCommon.GetAvatarLink(s.UserId, s.FileTypeId, s.FileType);
                    });
                    c.CreateMap<Model.EDM.FeedComment, FeedCommentViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                feeds = mapper.Map<IEnumerable<FeedViewModel>>(newsFeed);
            }
            return feeds;
        }

        public int GetFeedTotalLike(long feedId)
        {
            return UnitOfWork.FeedLikeRepository.GetMany(e => e.FeedId == feedId).Count();
        }

        public bool LikeFeed(string userId, long feedId)
        {
            FeedLike feedLike = new FeedLike();
            feedLike.UserId = userId;
            feedLike.FeedId = feedId;
            UnitOfWork.FeedLikeRepository.Add(feedLike);
            bool likeResult = UnitOfWork.Commit();
            return likeResult;
        }

        public bool UnlikeFeed(string userId, long feedId)
        {
            UnitOfWork.FeedLikeRepository.Delete(e => e.UserId == userId && e.FeedId == feedId);
            bool likeResult = UnitOfWork.Commit();
            return likeResult;
        }

        public bool SaveFeedAndFile(FeedViewModel feedViewModel, IEnumerable<FileViewModel> fileViewModels)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<FeedViewModel, Model.EDM.Feed>();
                c.CreateMap<UserViewModel, Model.EDM.User>();
                c.CreateMap<FileViewModel, Model.EDM.File>();
                c.CreateMap<FileTypeViewModel, Model.EDM.FileType>();
                c.CreateMap<FileFolderViewModel, Model.EDM.FileFolder>();
            });
            var mapper = config.CreateMapper();
            Model.EDM.Feed feed = mapper.Map<Model.EDM.Feed>(feedViewModel);
            IEnumerable<Model.EDM.File> files = mapper.Map<IEnumerable<Model.EDM.File>>(fileViewModels);
            feed.CreatedTime = DateTime.Now;
            UnitOfWork.FeedRepository.Add(feed);
            foreach (var file in files)
            {
                file.CreatedDate = DateTime.Now;
                file.Feed = feed;
                UnitOfWork.FileRepository.Add(file);
            }
            var saveResult = UnitOfWork.Commit();
            return saveResult;

        }

        public FeedCommentViewModel Comment(long feedId, string userId, string content)
        {
            FeedComment feedComment = new FeedComment();
            feedComment.UserId = userId;
            feedComment.FeedId = feedId;
            feedComment.Content = content;
            feedComment.CreatedTime = DateTime.Now;
            UnitOfWork.FeedCommentRepository.Add(feedComment);
            bool saveResult = UnitOfWork.Commit();
            var feedCommentViewModel = new FeedCommentViewModel();
            if (saveResult)
            {
                feedCommentViewModel = GetFeedCommentViewModelById(feedComment.FeedCommentId);
            }
            return feedCommentViewModel;
        }

        private FeedCommentViewModel GetFeedCommentViewModelById(long feedCommentId)
        {
            FeedComment feedComment = UnitOfWork.FeedCommentRepository.GetWithInclude(e => e.FeedCommentId == feedCommentId, "User", "FeedCommentLikes").FirstOrDefault();
            FeedCommentViewModel feedCommentViewModel = null;
            if (feedComment != null)
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<FeedComment, FeedCommentViewModel>();
                    c.CreateMap<User, UserViewModel>().AfterMap((s, d) => d.UserName = UnitOfWork.AspNetUserRepository.GetBy(e => e.Id == s.UserId).UserName); ;
                    c.CreateMap<Model.EDM.Feed, FeedViewModel>();
                    c.CreateMap<Model.EDM.FeedCommentLike, FeedCommentLikeViewModel>();
                    c.CreateMap<Model.EDM.FeedLike, FeedLikeViewModel>();
                    c.CreateMap<Model.EDM.File, FileViewModel>().AfterMap((s, d) => d.PhotoLink = string.Format("~/" + s.FileFolder.Path + "/{0}/{1}", string.Concat(s.CreatedDate.Year.ToString(), s.CreatedDate.Month.ToString()), s.FileName.ToString() + "_O." + s.FileType.Name));
                    c.CreateMap<Model.EDM.FileType, FileTypeViewModel>();
                });
                var mapper = config.CreateMapper();
                feedCommentViewModel = mapper.Map<FeedCommentViewModel>(feedComment);
            }
            return feedCommentViewModel;
        }

        public bool DeleteComment(long commentId)
        {
            UnitOfWork.FeedCommentRepository.Delete(e => e.FeedCommentId == commentId);
            bool likeResult = UnitOfWork.Commit();
            return likeResult;
        }
    }
}
