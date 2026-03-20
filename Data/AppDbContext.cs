using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ================= DbSet =================
        public DbSet<Role> Roles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumPicture> AlbumPictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= ROLE =================
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // ================= MEMBER =================
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.MemberName)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasOne(m => m.Role)
                .WithMany(r => r.Members)
                .HasForeignKey(m => m.RoleID)
                .OnDelete(DeleteBehavior.Restrict); // ❌ không cascade

            // ================= CATEGORY =================
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            // ================= PICTURE =================
            modelBuilder.Entity<Picture>()
                .HasOne(p => p.Author)
                .WithMany(m => m.Pictures)
                .HasForeignKey(p => p.MemberID)
                .OnDelete(DeleteBehavior.Cascade); // ✔ xóa user → xóa ảnh

            modelBuilder.Entity<Picture>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Pictures)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);

            // ================= COMMENT =================
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MemberID)
                .OnDelete(DeleteBehavior.Restrict); // ❌ tránh loop

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Picture)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PictureID)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 FIX

            // ================= LIKE =================
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.MemberID, l.PictureID })
                .IsUnique();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Likes)
                .HasForeignKey(l => l.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Picture)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PictureID)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 FIX

            // ================= REPORT =================
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(m => m.Reports)
                .HasForeignKey(r => r.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Picture)
                .WithMany()
                .HasForeignKey(r => r.PictureID)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 FIX

            // ================= ALBUM =================
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Owner)
                .WithMany(m => m.Albums)
                .HasForeignKey(a => a.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            // ================= MANY-TO-MANY =================
            modelBuilder.Entity<AlbumPicture>()
                .HasKey(ap => new { ap.AlbumID, ap.PictureID });

            modelBuilder.Entity<AlbumPicture>()
                .HasOne(ap => ap.Album)
                .WithMany(a => a.AlbumPictures)
                .HasForeignKey(ap => ap.AlbumID)
                .OnDelete(DeleteBehavior.Cascade); // ✔

            modelBuilder.Entity<AlbumPicture>()
                .HasOne(ap => ap.Picture)
                .WithMany(p => p.AlbumPictures)
                .HasForeignKey(ap => ap.PictureID)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 CRITICAL FIX

            // ================= DEFAULT VALUE =================
            modelBuilder.Entity<Member>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Picture>()
                .Property(p => p.UploadDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Comment>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Report>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}