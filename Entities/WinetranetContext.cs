using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace winetranet_api.Entities
{
    public partial class WinetranetContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public WinetranetContext()
        {
        }

        public WinetranetContext(DbContextOptions<WinetranetContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("service");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.ToTable("site");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Ville)
                    .HasMaxLength(255)
                    .HasColumnName("ville")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Service, "service");

                entity.HasIndex(e => e.Site, "site");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("firstname")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("lastname")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .HasColumnName("password_hash")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(255)
                    .HasColumnName("password_salt")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasColumnName("phone")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PhoneMobile)
                    .HasMaxLength(255)
                    .HasColumnName("phone_mobile")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Role)
                    .HasColumnType("enum('VISITOR','ADMIN')")
                    .HasColumnName("role")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Service)
                    .HasColumnType("int(11)")
                    .HasColumnName("service")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Site)
                    .HasColumnType("int(11)")
                    .HasColumnName("site")
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.ServiceNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Service)
                    .HasConstraintName("user_ibfk_1");

                entity.HasOne(d => d.SiteNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Site)
                    .HasConstraintName("user_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
