using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class Country
    {
        public Country()
        {
            Addresses = new HashSet<Address>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
