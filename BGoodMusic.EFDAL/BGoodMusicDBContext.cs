using System.Data.Entity;
using System.Linq;
using BGoodMusic.Models;
using BGoodMusic.EFDAL.Interfaces;

namespace BGoodMusic.EFDAL
{
    public class BGoodMusicDBContext : DbContext, IBGoodMusicRepository
    {
        public DbSet<Rehearsal> Rehearsals { get; set; }

        //***************************
        #region IBGoodMusicRepository

        public IQueryable<Rehearsal> GetRehearsals() { return Rehearsals; }

        #endregion IBGoodMusicRepository

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rehearsal>()
                .ToTable("bgm_Rehearsals");
        }
    }
}
