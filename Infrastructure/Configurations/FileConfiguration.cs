using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<Domain.File>
{
    public void Configure(EntityTypeBuilder<Domain.File> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.OriginalName)
            .IsRequired();
        
        builder
            .Property(x => x.Path)
            .IsRequired();

        builder
            .Property(x => x.Size)
            .IsRequired();
        
        builder
            .Property(x => x.ContentType)
            .IsRequired();

        builder
            .HasOne(x => x.Transfer)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
