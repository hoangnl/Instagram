//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Instagram.Model.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Feeds = new HashSet<Feed>();
            this.FeedComments = new HashSet<FeedComment>();
            this.FeedCommentLikes = new HashSet<FeedCommentLike>();
            this.FeedLikes = new HashSet<FeedLike>();
            this.UserFollows = new HashSet<UserFollow>();
            this.UserFollows1 = new HashSet<UserFollow>();
        }
    
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Avartar { get; set; }
        public string Bio { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public int Gender { get; set; }
        public byte[] Timestamp { get; set; }
        public bool AccountDisabled { get; set; }
        public Nullable<int> FileTypeId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feed> Feeds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeedComment> FeedComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeedCommentLike> FeedCommentLikes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeedLike> FeedLikes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFollow> UserFollows { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFollow> UserFollows1 { get; set; }
        public virtual FileType FileType { get; set; }
    }
}
