using System;
using System.Data.Entity;
using System.Linq;
using BGoodMusic.Models;
using BGoodMusic.EFDAL.Interfaces;

namespace BGoodMusic.EFDAL
{
    public class BGoodMusicDBContext : DbContext, IBGoodMusicRepository
    {
        public DbSet<Rehearsal> Rehearsals { get; set; }
        public DbSet<UserInfo> UserInfoItems { get; set; }

        //***************************
        #region IBGoodMusicRepository

        public Guid AddNewUserInfo(string userIdentifier, string protectedToken)
        {
            if (!string.IsNullOrWhiteSpace(userIdentifier) && !string.IsNullOrWhiteSpace(protectedToken))
            {
                UserInfo userInfo = new UserInfo
                {
                    CreationTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Token = protectedToken,
                    UserIdentifier = userIdentifier
                };
                UserInfoItems.Add(userInfo);
                SaveChanges();
                return userInfo.Id;
            }
            return Guid.Empty;
        }

        public IQueryable<Rehearsal> GetRehearsals() { return Rehearsals; }
        public UserInfo GetUserInfoItem(Guid id)
        {
            var userInfo = UserInfoItems.Where(ui => ui.Id.Equals(id)).FirstOrDefault();
            return userInfo;
        }
        public IQueryable<UserInfo> GetUserInfoItems() { return UserInfoItems; }

        public void RemoveUserInfoItem(Guid id)
        {
            var uiItem = UserInfoItems.Where(ui => ui.Id.Equals(id)).FirstOrDefault();
            if (uiItem != null)
            {
                UserInfoItems.Remove(uiItem);
                SaveChanges();
            }
        }

        #endregion IBGoodMusicRepository

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rehearsal>()
                .ToTable("bgm_Rehearsals");

            modelBuilder.Entity<UserInfo>()
                .ToTable("bgm_UserInfo");
        }
    }
}
