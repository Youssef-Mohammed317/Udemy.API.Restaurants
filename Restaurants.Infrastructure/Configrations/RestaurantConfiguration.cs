using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Configrations;

internal class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.CategoryId).IsRequired();
        builder.Property(p => p.HasDelivery).IsRequired().HasDefaultValue(false);
        builder.Property(p => p.ContactEmail).IsRequired(false);
        builder.Property(p => p.ContactNumber).IsRequired(false);

        builder.OwnsOne(p => p.Address, a =>
        {
            a.Property(p => p.City).IsRequired(false);
            a.Property(p => p.Street).IsRequired(false);
            a.Property(p => p.PostalCode).IsRequired(false);
        });

        builder.HasMany(p => p.Dishes)
            .WithOne(p => p.Restaurant)
            .HasForeignKey(p => p.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Category)
            .WithMany(p => p.Restaurants)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Owner)
            .WithMany(p => p.OwnedRestaurants)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
