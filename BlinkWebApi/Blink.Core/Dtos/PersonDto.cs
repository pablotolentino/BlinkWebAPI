using System;
using System.Collections.Generic;

namespace Blink.Core.Dtos
{
    public class PersonDto
    {
        public long PersonId { get; set; }
        public Guid? TypeUserId { get; set; }
        public Guid? TypePersonId { get; set; }
        public Guid? GenderId { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Rfc { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Password { get; set; }
        public List<AddressDto> Address { get; set; }
        public List<AffiliateDto> Affiliate { get; set; }
    }
}
