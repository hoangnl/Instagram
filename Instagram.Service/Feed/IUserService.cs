using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instagram.Model.EDM;
using Instagram.ViewModel.Feed;
using Instagram.ViewModel.User;

namespace Instagram.Service.Feed
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> SearchUser(string searchTerm);
        void AddUserInfo(UserViewModel userViewModel);
        UserViewModel GetUserInfo(string userId);
        bool HasNotFollower(string userId);
        IEnumerable<UserViewModel> GetSuggetions(string userId);
        bool Follow(string userId, string userFollowId);
        bool Unfollow(string userId, string userFollowId);
        IEnumerable<FeedLikeViewModel> GetLikedUserList(long feedId);
        IEnumerable<UserViewModel> Autocomplete(string searchTerm);
        UserProfileViewModel GetUserProfileByUserId(string id, string loginUserId);
        void SaveUserProfile(UserProfileViewModel userProfileViewModel);
    }
}
