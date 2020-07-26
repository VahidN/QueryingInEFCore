using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePgExercises.Entities
{
    public class Booking
    {
        public int BookId { set; get; }

        public int FacId { set; get; }
        public virtual Facility Facility { set; get; }

        public int MemId { set; get; }
        public virtual Member Member { set; get; }

        public DateTime StartTime { set; get; }

        public int Slots { set; get; }
    }

    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(booking => booking.BookId);
            builder.Property(booking => booking.BookId).IsRequired().UseIdentityColumn(seed: 0, increment: 1);

            builder.Property(booking => booking.FacId).IsRequired();
            builder.HasOne(booking => booking.Facility)
                    .WithMany(facility => facility.Bookings)
                    .HasForeignKey(booking => booking.FacId);

            builder.Property(booking => booking.MemId).IsRequired();
            builder.HasOne(booking => booking.Member)
                    .WithMany(member => member.Bookings)
                    .HasForeignKey(booking => booking.MemId);

            builder.Property(booking => booking.StartTime).IsRequired();

            builder.Property(booking => booking.Slots).IsRequired();

            builder.HasIndex(booking => new { booking.MemId, booking.FacId }).HasName("IX_memid_facid");
            builder.HasIndex(booking => new { booking.FacId, booking.StartTime }).HasName("IX_facid_starttime");
            builder.HasIndex(booking => new { booking.MemId, booking.StartTime }).HasName("IX_memid_starttime");
            builder.HasIndex(booking => booking.StartTime).HasName("IX_starttime");
        }
    }
}