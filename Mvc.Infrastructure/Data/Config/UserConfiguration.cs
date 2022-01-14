using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApp.Infrastructure.Data.Configuration
{
    /*
    public class TopicConfiguration : IEntityTypeConfiguration<IdeaTopic>
    {
        public void Configure(EntityTypeBuilder<IdeaTopic> builder)
        {
            builder.HasMany(x => x.Comments).WithOne(x => x.Topic)
                .OnDelete(DeleteBehavior.NoAction);           
        }
    }

    
    public class TopicCommentConfiguration : IEntityTypeConfiguration<TopicComment>
    {
        public void Configure(EntityTypeBuilder<TopicComment> builder)
        {
            builder.HasOne(x => x.Topic).WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    */

    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            /*
            builder.HasMany(x => x.IdeaTopics).WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(x => x.IdeaMemberRoles).WithOne(x => x.User)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(x => x.TopicComments).WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.ClientCascade);
            */
            //

            builder.HasMany(x => x.Following).WithOne(x => x.Following)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Followers).WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AuthorInvites).WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Invites).WithOne(x => x.InvitedUser)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
