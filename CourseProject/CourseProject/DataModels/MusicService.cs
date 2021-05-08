using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CourseProject.DataModels
{
    public partial class MusicService : DbContext
    {
        public MusicService()
            : base("name=MusicService")
        {
        }

        public virtual DbSet<ALBUMS> ALBUMS { get; set; }
        public virtual DbSet<GENRES> GENRES { get; set; }
        public virtual DbSet<PLAYLISTS> PLAYLISTS { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TRACKS> TRACKS { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PLAYLISTS>()
                .HasMany(e => e.TRACKS)
                .WithMany(e => e.PLAYLISTS)
                .Map(m => m.ToTable("PLAYLIST_ALL").MapLeftKey("playlists_id").MapRightKey("tracks_id"));

            modelBuilder.Entity<TRACKS>()
                .HasMany(e => e.USERS1)
                .WithMany(e => e.TRACKS1)
                .Map(m => m.ToTable("USERTRACKS_ALL").MapLeftKey("tracks_id").MapRightKey("id_user"));

            modelBuilder.Entity<USERS>()
                .HasMany(e => e.TRACKS)
                .WithOptional(e => e.USERS)
                .HasForeignKey(e => e.id_user);
        }
    }
}
