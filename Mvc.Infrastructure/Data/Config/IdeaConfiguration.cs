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
    
    public class TopicCommentConfiguration : IEntityTypeConfiguration<TopicComment>
    {
        public void Configure(EntityTypeBuilder<TopicComment> builder)
        {
            builder.HasOne(x => x.Topic).WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
    

    public class IdeaTopicConfiguration : IEntityTypeConfiguration<IdeaTopic>
    {
        public void Configure(EntityTypeBuilder<IdeaTopic> builder)
        {
            builder.HasMany(x => x.Comments).WithOne(x => x.Topic)
                .OnDelete(DeleteBehavior.ClientCascade);            
        }
    }

    public class IdeaBoxConfiguration : IEntityTypeConfiguration<IdeaBox>
    {
        public void Configure(EntityTypeBuilder<IdeaBox> builder)
        {
            builder.HasMany(x => x.Goals).WithOne(x => x.Box)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }

    /*
    public class BoxGoalConfiguration : IEntityTypeConfiguration<BoxGoal>
    {
        public void Configure(EntityTypeBuilder<BoxGoal> builder)
        {
            builder.HasOne(x => x.Box).WithMany(x => x.Goals)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
    */
    

    public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
    {
        public void Configure(EntityTypeBuilder<Idea> builder)
        {
            builder.HasOne(x => x.Avatar).WithMany(x => x.Ideas);

            builder.HasMany(x => x.Topics).WithOne(x => x.Idea)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }


}
