using System;
using System.Collections.Generic;

namespace Blink.Core.Dtos
{
    public  class AddressDto
    {
        public AddressDto()
        {
            //BinnacleAddress = new HashSet<BinnacleAddress>();
        }

        public long AddressId { get; set; }
        public long? PersonId { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? MunicipalityId { get; set; }
        public long? ColonyId { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string OutdoorNumber { get; set; }
        public string InteriorNumber { get; set; }
        public PersonDto Person { get; set; }
       // public ICollection<BinnacleAddress> BinnacleAddress { get; set; }
    }
}
