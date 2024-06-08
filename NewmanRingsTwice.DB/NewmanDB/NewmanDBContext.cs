using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NewmanRingsTwice.DB.NewmanDB
{
    public partial class NewmanDBContext : DbContext
    {
        public NewmanDBContext()
        {
        }

        public NewmanDBContext(DbContextOptions<NewmanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CcMailAddress> CcMailAddresses { get; set; } = null!;
        public virtual DbSet<ImportanceType> ImportanceTypes { get; set; } = null!;
        public virtual DbSet<Mail> Mail { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=localhost\\SQLEXPRESS;initial catalog=NewmanDB;Trusted_Connection=True;multipleactiveresultsets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CcMailAddress>(entity =>
            {
                entity.ToTable("CcMailAddress");

                entity.Property(e => e.CcMailAddressId).HasColumnName("CcMailAddressID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.MailId).HasColumnName("MailID");

                entity.HasOne(d => d.Mail)
                    .WithMany(p => p.CcMailAddresses)
                    .HasForeignKey(d => d.MailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CcMailAddress_Mail");
            });

            modelBuilder.Entity<ImportanceType>(entity =>
            {
                entity.ToTable("ImportanceType");

                entity.Property(e => e.ImportanceTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ImportanceTypeID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Mail>(entity =>
            {
                entity.Property(e => e.MailId).HasColumnName("MailID");

                entity.Property(e => e.DatetimeCreated).HasColumnType("datetime");

                entity.Property(e => e.FromMail).HasMaxLength(50);

                entity.Property(e => e.ImportanceTypeId).HasColumnName("ImportanceTypeID");

                entity.Property(e => e.MailContent).HasMaxLength(1000);

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.ToMail).HasMaxLength(50);

                entity.HasOne(d => d.ImportanceType)
                    .WithMany(p => p.Mail)
                    .HasForeignKey(d => d.ImportanceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mail_ImportanceType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
