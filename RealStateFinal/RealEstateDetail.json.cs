using System;
using System.Collections.Generic;

using Starcounter;

namespace RealStateFinal
{
    partial class RealEstateDetail : Json
    {
        protected override void OnData()
        {
            base.OnData();
            AssignSavedValue();
        }

        private void AssignSavedValue()
        {
            var realEstateOffice = Data as RealEstateOffice;
            RealEstateName = realEstateOffice?.OfficeName;
            Street = realEstateOffice?.Street;
            if (realEstateOffice?.Number != null) Number = realEstateOffice?.Number;
            if (realEstateOffice?.ZipCode != null) ZipCode = (long) realEstateOffice?.ZipCode;
            City = realEstateOffice?.City;
            Country = realEstateOffice?.Country;

            PopulateHouseList(realEstateOffice?.HouseInfos);
        }

        private void PopulateHouseList(IEnumerable<HouseInformation> houseInfos)
        {
            foreach (var houseInformation in houseInfos)
                //var houseInfosElementJson = new HouseInfosElementJson
                //{
                //    Date = houseInformation.Date.ToShortDateString(),
                //    Address = houseInformation.Number + ", " + houseInformation.Street + ", " + houseInformation.City + houseInformation.Country,
                //    Price = houseInformation.Price,
                //    Commission = houseInformation.Commission
                //};
                AddHouseList(houseInformation);
        }

        private void AddHouseList(HouseInformation houseInformation)
        {
            var houseInfosElementJson = HouseList.Add();
            houseInfosElementJson.Date = houseInformation.Date.ToShortDateString();
            houseInfosElementJson.Address = houseInformation.Number + ", " + houseInformation.Street + ", " +
                                            houseInformation.City + houseInformation.Country;
            houseInfosElementJson.Price = houseInformation.Price;
            houseInfosElementJson.Commission = houseInformation.Commission;
        }

        private void AssaingnValueToRepo()
        {
            var realEstateOffice = Data as RealEstateOffice;
            if (realEstateOffice != null)
            {
                realEstateOffice.OfficeName = RealEstateName;
                realEstateOffice.Street = Street;
                realEstateOffice.Number = Number;
                realEstateOffice.ZipCode = ZipCode is int ? (int) ZipCode : 0;
                realEstateOffice.City = City;
                realEstateOffice.Country = Country;
            }
        }

        private void Handle(Input.CancelTrigger action)
        {
            AssignSavedValue();
        }

        private void Handle(Input.SaveOffice action)
        {
            AssaingnValueToRepo();
            Transaction.Commit();
        }

        private void Handle(Input.SaveHome action)
        {
            AssignHomeValueToRepo();
        }

        private void AssignHomeValueToRepo()
        {
            var houseInformation = new HouseInformation();
            houseInformation.Date = DateTime.Parse(SaleDate);
            houseInformation.City = HouseCity;
            houseInformation.Country = HouseCountry;
            houseInformation.Street = Street;
            houseInformation.Price = HousePrice;
            houseInformation.Commission = HouseCommission;
            houseInformation.ZipCode = (int) ZipCode;
            houseInformation.Number = (int) HouseNumber;
            var realEstateOffice = Data as RealEstateOffice;
            houseInformation.RealEstateOffice = realEstateOffice;

            AddHouseList(houseInformation);

            Transaction.Commit();
        }
    }
}