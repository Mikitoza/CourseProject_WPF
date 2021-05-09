namespace CourseProject.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TRACKS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRACKS()
        {
            PLAYLISTS = new HashSet<PLAYLISTS>();
            USERS1 = new HashSet<USERS>();
        }

        public int? id_user { get; set; }

        public int? genre_id { get; set; }

        public int? album_id { get; set; }

        [Key]
        public int track_id { get; set; }

        [Required]
        [StringLength(255)]
        public string track_name { get; set; }

        public virtual ALBUMS ALBUMS { get; set; }

        public virtual GENRES GENRES { get; set; }

        public virtual USERS USERS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYLISTS> PLAYLISTS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USERS> USERS1 { get; set; }
        public TRACKS(string name, int id_user, int album_id, int genre_id)
        {
            this.track_name = name;
            this.id_user = id_user;
            this.album_id = album_id;
            this.genre_id = genre_id;
        }

    }
}
