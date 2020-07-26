using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePgExercises.Entities
{
    public class Facility
    {
        public int FacId { set; get; }

        public string Name { set; get; }

        public decimal MemberCost { set; get; }

        public decimal GuestCost { set; get; }

        public decimal InitialOutlay { set; get; }

        public decimal MonthlyMaintenance { set; get; }

        public virtual ICollection<Booking> Bookings { set; get; }
    }

    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasKey(facility => facility.FacId);
            builder.Property(facility => facility.FacId).IsRequired().UseIdentityColumn(seed: 0, increment: 1);

            builder.Property(facility => facility.Name).HasMaxLength(100).IsRequired();

            builder.Property(facility => facility.MemberCost).IsRequired().HasColumnType("decimal(18, 6)");

            builder.Property(facility => facility.GuestCost).IsRequired().HasColumnType("decimal(18, 6)");

            builder.Property(facility => facility.InitialOutlay).IsRequired().HasColumnType("decimal(18, 6)");

            builder.Property(facility => facility.MonthlyMaintenance).IsRequired().HasColumnType("decimal(18, 6)");
        }
    }
}