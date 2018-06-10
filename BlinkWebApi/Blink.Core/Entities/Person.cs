using System;
using System.Collections.Generic;

namespace Blink.Core.Entities
{
    public partial class Person
    {
        public Person()
        {
            Address = new HashSet<Address>();
            BinnacleAddress = new HashSet<BinnacleAddress>();
        }

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

        public Affiliate Affiliate { get; set; }
        public ICollection<Address> Address { get; set; }
        public ICollection<BinnacleAddress> BinnacleAddress { get; set; }
    }
}
