using System;

namespace Blink.Core.Dtos
{
    public class UserDto
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
        public long? LinkedByAffiliateId { get; set; }
        public bool? IsPhysical { get; set; }
        public string Password { get; set; }
    }
}
