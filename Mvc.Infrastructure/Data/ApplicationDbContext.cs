using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaActivity> IdeaActivity { get; set; }
        public DbSet<IdeaBox> IdeaBoxes { get; set; }
        public DbSet<IdeaBoxLike> IdeaBoxLikes { get; set; }
        public DbSet<IdeaInvite> IdeaInvites { get; set; }
        public DbSet<IdeaMemberRole> IdeaMemberRoles { get; set; }
        public DbSet<IdeaStar> IdeaStars { get; set; }
        public DbSet<IdeaStatus> IdeaStatuses { get; set; }
        public DbSet<IdeaTopic> IdeaTopics { get; set; }
        public DbSet<IdeaStatusImage> ImagesIdeaStatus { get; set; }

        public DbSet<BaseDiscussMessage> DiscussMessages { get; set; }
        public DbSet<BoxDiscuss> IdeaBoxDiscuss { get; set; }
        public DbSet<TopicDiscuss> IdeaTopicDiscuss { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Follower> Follows { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
