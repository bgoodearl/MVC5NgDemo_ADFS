using System.Linq;
using BGoodMusic.Models;

namespace BGoodMusic.EFDAL.Interfaces
{
    public interface IBGoodMusicRepository
    {
        IQueryable<Rehearsal> GetRehearsals();
        int SaveChanges();
    }
}
