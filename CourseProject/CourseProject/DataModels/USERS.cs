namespace CourseProject.DataModels
{
    using CourseProject.DataModels.ENUM;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Security.Cryptography;
    using System.Text;

    public partial class USERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USERS()
        {
            ALBUMS = new HashSet<ALBUMS>();
            PLAYLISTS = new HashSet<PLAYLISTS>();
            TRACKS = new HashSet<TRACKS>();
            TRACKS1 = new HashSet<TRACKS>();
        }

        [Key]
        public int id_user { get; set; }

        [Required]
        [StringLength(255)]
        public string user_login { get; set; }

        [StringLength(255)]
        public string user_nickname { get; set; }

        [Required]
        [StringLength(255)]
        public string user_password { get; set; }

        public int? user_role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALBUMS> ALBUMS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYLISTS> PLAYLISTS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRACKS> TRACKS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRACKS> TRACKS1 { get; set; }

        public static string getHash(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return "Error";
            }
            else
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
        public USERS(string login, string password,bool IsSinger)
        {
            this.user_login = login;
            this.user_password = password;
            this.user_nickname = login;
            if (!IsSinger)
            {
                this.user_role = (int)UserRole.User;
            }
            else
            {
                this.user_role = (int)UserRole.Singer;
            }
        }
    }
}
