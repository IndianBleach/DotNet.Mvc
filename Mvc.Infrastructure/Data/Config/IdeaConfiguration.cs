using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Data.Config
{
    public class IdeaBoxConfiguration : IEntityTypeConfiguration<IdeaBox>
    {
        public void Configure(EntityTypeBuilder<IdeaBox> builder)
        {
            builder.HasMany(x => x.Goals).WithOne(x => x.Box)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class BoxGoalConfiguration : IEntityTypeConfiguration<BoxGoal>
    {
        public void Configure(EntityTypeBuilder<BoxGoal> builder)
        {
            builder.HasOne(x => x.Box).WithMany(x => x.Goals)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
    {
        public void Configure(EntityTypeBuilder<Idea> builder)
        {
            builder.HasOne(x => x.Avatar).WithMany(x => x.Ideas);
        }
    }


}
