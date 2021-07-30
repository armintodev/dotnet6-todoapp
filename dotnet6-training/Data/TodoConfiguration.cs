using dotnet6_training.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet6_training.Data
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Title)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
