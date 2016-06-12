using Instagram.Model.Common;
using Instagram.Model.EDM;
using Instagram.Model.Repository;
using Instagram.Model.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Instagram.Model.UnitOfWork
{
    /// <summary>
    /// Lớp UnitOfWork chung
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Declaration

        /// <summary>
        /// DBContext chung của UoW
        /// </summary>
        private InstagramEntities DbContext;

        /// <summary>
        /// FeedRepository xử lý Entity Feed
        /// </summary>
        private IFeedRepository feedRepository;

        /// <summary>
        /// FeedRepository xử lý Entity Feed
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// FileRepository xử lý Entity File
        /// </summary>
        private IFileRepository fileRepository;

        private IFeedLikeRepository feedLikeRepository;

        private IFeedCommentRepository feedCommentRepository;

        private IUserFollowRepository userFollowRepository;

        private IAspNetUserRepository aspNetUserRepository;

        IMessageRepository messageRepository;

        IMessageTypeRepository messageTypeRepository;

        IMessageStatusTypeRepository messageStatusTypeRepository;
        #endregion

        #region Property

        /// <summary>
        /// DBContext chung của UoW
        /// </summary>
        public InstagramEntities DataContext
        {
            get { return DbContext ?? (DbContext = DatabaseFactory.Get()); }
        }

        /// <summary>
        /// Factory khởi tạo Database
        /// </summary>
        public IDatabaseFactory DatabaseFactory { get; set; }

        /// <summary>
        /// FeedRepository xử lý Entity Feed
        /// </summary>
        public IFeedRepository FeedRepository
        {
            get
            {
                if (feedRepository == null)
                {
                    feedRepository = new FeedRepository(DatabaseFactory);
                }

                return feedRepository;
            }
        }

        public IAspNetUserRepository AspNetUserRepository
        {
            get
            {
                if (aspNetUserRepository == null)
                {
                    aspNetUserRepository = new AspNetUserRepository(DatabaseFactory);
                }

                return aspNetUserRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(DatabaseFactory);
                }

                return userRepository;
            }
            set
            {
                userRepository = value;
            }
        }

        public IFileRepository FileRepository
        {
            get
            {
                if (fileRepository == null)
                {
                    fileRepository = new FileRepository(DatabaseFactory);
                }

                return fileRepository;
            }
        }

        public IFeedLikeRepository FeedLikeRepository
        {
            get
            {
                if (feedLikeRepository == null)
                {
                    feedLikeRepository = new FeedLikeRepository(DatabaseFactory);
                }

                return feedLikeRepository;
            }
        }

        public IUserFollowRepository UserFollowRepository
        {
            get
            {
                if (userFollowRepository == null)
                {
                    userFollowRepository = new UserFollowRepository(DatabaseFactory);
                }

                return userFollowRepository;
            }
        }

        public IFeedCommentRepository FeedCommentRepository
        {
            get
            {
                if (feedCommentRepository == null)
                {
                    feedCommentRepository = new FeedCommentRepository(DatabaseFactory);
                }

                return feedCommentRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    messageRepository = new MessageRepository(DatabaseFactory);
                }

                return messageRepository;
            }
            set
            {
                messageRepository = value;
            }
        }

        public IMessageTypeRepository MessageTypeRepository
        {
            get
            {
                if (messageTypeRepository == null)
                {
                    messageTypeRepository = new MessageTypeRepository(DatabaseFactory);
                }

                return messageTypeRepository;
            }
        }

        public IMessageStatusTypeRepository MessageStatusTypeRepository
        {
            get
            {
                if (messageStatusTypeRepository == null)
                {
                    messageStatusTypeRepository = new MessageStatusTypeRepository(DatabaseFactory);
                }

                return messageStatusTypeRepository;
            }
        }
        #endregion

        #region Constructor

        public UnitOfWork(IDatabaseFactory databaseFactoryArg)
        {
            DatabaseFactory = databaseFactoryArg;
        }

        #endregion

        #region Function

        public bool Commit()
        {
            return DataContext.SaveChanges() > 0;
        }

        #endregion

    }
}
