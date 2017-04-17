using System.Collections.Generic;
using Starcounter;

namespace RealStateFinal
{
    [Database]
    public class RealEstateInfo
    {
        public string RealEstateName;

        public IEnumerable<RealEstateOffice> RealEstateDetails
        {
            get
            {
                var offices = Db.SQL<RealEstateOffice>($"select f from RealEstateOffice f where f.RealEstateInfo= ?", this);
                return offices;
            }
        }
    }
}