using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // ❌ Không dùng EnsureCreated nếu đã dùng Migration
            // context.Database.EnsureCreated();

            // Nếu đã có data → bỏ qua
            if (context.Roles.Any()) return;

            // ================= ROLE =================
            var adminRole = new Role
            {
                RoleName = "Admin",
                Description = "System Administrator",
                Status = Status.Active
            };

            var modRole = new Role
            {
                RoleName = "Moderator",
                Description = "Content Moderator",
                Status = Status.Active
            };

            var memberRole = new Role
            {
                RoleName = "Member",
                Description = "Normal User",
                Status = Status.Active
            };

            context.Roles.AddRange(adminRole, modRole, memberRole);
            context.SaveChanges();

            // ================= MEMBER =================
            var admin = new Member
            {
                MemberName = "admin",
                Email = "admin@gmail.com",
                Password = "123", // 👉 sau này hash nhé
                RoleID = adminRole.RoleID,
                FullName = "System Admin",
                Bio = "Administrator account",
                AvatarURL = "/images/default-avatar.png",
                Status = Status.Active
            };

            var user1 = new Member
            {
                MemberName = "user1",
                Email = "user1@gmail.com",
                Password = "123",
                RoleID = memberRole.RoleID,
                FullName = "User One",
                Bio = "Love photography",
                AvatarURL = "/images/default-avatar.png",
                Status = Status.Active
            };

            var user2 = new Member
            {
                MemberName = "user2",
                Email = "user2@gmail.com",
                Password = "123",
                RoleID = memberRole.RoleID,
                FullName = "User Two",
                Bio = "Tech lover",
                AvatarURL = "/images/default-avatar.png",
                Status = Status.Active
            };

            context.Members.AddRange(admin, user1, user2);
            context.SaveChanges();

            // ================= CATEGORY =================
            var nature = new Category
            {
                CategoryName = "Nature",
                Status = Status.Active
            };

            var tech = new Category
            {
                CategoryName = "Technology",
                Status = Status.Active
            };

            var art = new Category
            {
                CategoryName = "Art",
                Status = Status.Active
            };

            context.Categories.AddRange(nature, tech, art);
            context.SaveChanges();

            // ================= PICTURE =================
            var pic1 = new Picture
            {
                Title = "Forest",
                Description = "Morning forest",
                ImageURL = "/uploads/forest.jpg",
                FileSize = 120000,
                Status = Status.Public,
                MemberID = user1.MemberID,
                CategoryID = nature.CategoryID
            };

            var pic2 = new Picture
            {
                Title = "Keyboard",
                Description = "Mechanical keyboard setup",
                ImageURL = "/uploads/keyboard.jpg",
                FileSize = 200000,
                Status = Status.Pending,
                MemberID = user2.MemberID,
                CategoryID = tech.CategoryID
            };

            context.Pictures.AddRange(pic1, pic2);
            context.SaveChanges();

            // ================= ALBUM =================
            var album = new Album
            {
                AlbumName = "My Trip",
                Description = "Summer memories",
                PrivacyMode = "Public",
                MemberID = user1.MemberID
            };

            context.Albums.Add(album);
            context.SaveChanges();

            // ================= ALBUM-PICTURE =================
            context.AlbumPictures.Add(new AlbumPicture
            {
                AlbumID = album.AlbumID,
                PictureID = pic1.PictureID
            });

            // ================= COMMENT =================
            context.Comments.Add(new Comment
            {
                Content = "Nice picture!",
                MemberID = user2.MemberID,
                PictureID = pic1.PictureID
            });

            // ================= LIKE =================
            context.Likes.Add(new Like
            {
                MemberID = user2.MemberID,
                PictureID = pic1.PictureID
            });

            // ================= REPORT =================
            context.Reports.Add(new Report
            {
                Description = "Inappropriate content",
                MemberID = user2.MemberID,
                PictureID = pic2.PictureID,
                Status = Status.Pending
            });

            context.SaveChanges();
        }
    }
}