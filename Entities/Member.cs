using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCorePgExercises.Entities
{
    public class Member
    {
        public int MemId { set; get; }

        public string Surname { set; get; }

        public string FirstName { set; get; }

        public string Address { set; get; }

        public int ZipCode { set; get; }

        public string Telephone { set; get; }

        public virtual ICollection<Member> Children { get; set; }
        public virtual Member Recommender { set; get; }
        public int? RecommendedBy { set; get; }

        public DateTime JoinDate { set; get; }

        public virtual ICollection<Booking> Bookings { set; get; }
    }

    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(member => member.MemId);
            builder.Property(member => member.MemId).IsRequired().UseIdentityColumn(seed: 0, increment: 1);

            builder.Property(member => member.Surname).HasMaxLength(200).IsRequired();

            builder.Property(member => member.FirstName).HasMaxLength(200).IsRequired();

            builder.Property(member => member.Address).HasMaxLength(300).IsRequired();

            builder.Property(member => member.ZipCode).IsRequired();

            builder.Property(member => member.Telephone).HasMaxLength(20).IsRequired();

            builder.HasIndex(member => member.RecommendedBy);
            builder.HasOne(member => member.Recommender)
                    .WithMany(member => member.Children)
                    .HasForeignKey(member => member.RecommendedBy);

            builder.Property(member => member.JoinDate).IsRequired();

            builder.HasIndex(member => member.JoinDate).HasDatabaseName("IX_JoinDate");
            builder.HasIndex(member => member.RecommendedBy).HasDatabaseName("IX_RecommendedBy");
        }
    }
}