using System;
using Starcounter;

namespace RealStateFinal
{
    [Database]
    public class HouseInformation
    {
        public string City;

        public long Commission;

        public string Country;

        public DateTime Date;
        public int Number;

        public long Price;

        //[Transient]
        //public string Address;


        public RealEstateOffice RealEstateOffice;
        public string Street;


        public int ZipCode;
    }
}