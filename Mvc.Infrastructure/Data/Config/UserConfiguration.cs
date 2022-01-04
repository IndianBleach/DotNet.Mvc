using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApp.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
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
