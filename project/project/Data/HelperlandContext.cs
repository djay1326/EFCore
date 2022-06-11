using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using project.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace project.Data
{
    public partial class HelperlandContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HelperlandContext()
        {
        }

        public HelperlandContext(DbContextOptions<HelperlandContext> options)
            : base(options)
        {
        }

        
        public virtual DbSet<leave> leave { get; set; }
        public virtual DbSet<salary> salary { get; set; }

        public virtual DbSet<holidays> holidays { get; set; }
        //public virtual DbSet<User> Userr { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=DESKTOP-H353LBS; database=Helperlandd; trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            //modelBuilder.Entity<Contactus>(entity =>
            //{
            //    entity.ToTable("contactus");

            //    entity.Property(e => e.ContactusId).HasColumnName("contactusID");

            //    entity.Property(e => e.Email)
            //        .IsRequired()
            //        .HasColumnName("email")
            //        .HasMaxLength(30);

            //    entity.Property(e => e.FirstName)
            //        .IsRequired()
            //        .HasColumnName("firstName")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.LastName)
            //        .IsRequired()
            //        .HasColumnName("lastName")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.MobileNo)
            //        .IsRequired()
            //        .HasColumnName("mobileNo")
            //        .HasMaxLength(20);

            //    entity.Property(e => e.Msg)
            //        .IsRequired()
            //        .HasColumnName("msg")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.SubjectType)
            //        .IsRequired()
            //        .HasColumnName("subjectType")
            //        .HasMaxLength(100);
            //});

            //modelBuilder.Entity<User>(entity =>
            //{


            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Email)
            //        .IsRequired()
            //        .HasMaxLength(100);

                

                
            //});

            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
            
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
