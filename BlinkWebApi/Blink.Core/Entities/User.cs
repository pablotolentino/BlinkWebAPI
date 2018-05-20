using System;

namespace Blink.Core.Entities
{
    public partial class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MaternalSurname { get; set; }
        public string PaternalSurname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Username { get; set; }
        public string TypeUser { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public long? LinkedByAffiliateId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsPhysical { get; set; }
    }
}
