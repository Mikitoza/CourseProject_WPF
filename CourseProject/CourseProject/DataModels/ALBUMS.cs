namespace CourseProject.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ALBUMS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALBUMS()
        {
            TRACKS = new HashSet<TRACKS>();
        }

        public int? id_user { get; set; }

        public int? genre_id { get; set; }

        [Key]
        public int album_id { get; set; }

        [Required]
        [StringLength(255)]
        public string albums_name { get; set; }

        public virtual GENRES GENRES { get; set; }

        public virtual USERS USERS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRACKS> TRACKS { get; set; }

        public ALBUMS(int id_user, int genre_id, string albums_name)
        {
            this.id_user = id_user;
            this.genre_id = genre_id;
            this.albums_name = albums_name;
        }
    }
}
