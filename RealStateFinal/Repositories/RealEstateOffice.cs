using System.Collections.Generic;
using System.Linq;
using Starcounter;

namespace RealStateFinal
{
    [Database]
    public class RealEstateOffice
    {
        public string City;

        public string Country;

        public string Number;
        public string OfficeName;


        public RealEstateInfo RealEstateInfo;

        public string Street;

        public int ZipCode;

        public IEnumerable<HouseInformation> HouseInfos
        {
            get
            {
                var houseInfos = Db.SQL<HouseInformation>(
                    $"select h from HouseInformation h where h.RealEstateOffice= ?", this);
                return houseInfos;
            }
        }


        public long TotalHouseSold => HouseInfos.Count();


        public long TotalCommission
        {
            get { return HouseInfos.Sum(x => x.Commission); }
        }


        public long AveargeCommission
        {
            get { return TotalHouseSold == 0 ? TotalHouseSold : HouseInfos.Sum(x => x.Commission) / TotalHouseSold; }
        }
    }
}