using System;
using System.Linq;
using BGoodMusic.Models;

namespace BGoodMusic.EFDAL.Interfaces
{
    public interface IBGoodMusicRepository : IDisposable
    {
        Guid AddNewUserInfo(string userIdentifier, string protectedToken);
        IQueryable<Rehearsal> GetRehearsals();
        UserInfo GetUserInfoItem(Guid id);
        IQueryable<UserInfo> GetUserInfoItems();
        void RemoveUserInfoItem(Guid id);
        int SaveChanges();
    }
}
