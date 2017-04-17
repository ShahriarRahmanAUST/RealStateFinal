using System;
using System.Collections.Generic;
using System.Linq;

using Starcounter;

namespace RealStateFinal
{
    partial class RealStateHome : Json
    {
        protected override void OnData()
        {
            base.OnData();
            var data = this.Data as RealEstateInfo;
            this.CompanyName = data?.RealEstateName;
            var offices = data?.RealEstateDetails;
            this.SetOfficcesIntoView(offices);
        }

        private void SetOfficcesIntoView(IEnumerable<RealEstateOffice> offices)
        {
            foreach (var franchiseOffice in offices)
            {
                SetOfficeIntoView(franchiseOffice);
            }
        }

        private void SetOfficeIntoView(RealEstateOffice franchiseOffice)
        {
            var office = this.Offices.Add();
            office.OfficeName = franchiseOffice.OfficeName;
            office.TotalSoldHouse = franchiseOffice.TotalHouseSold;
            office.TotalCommission = franchiseOffice.TotalCommission;
            office.AverageCommission = franchiseOffice.AveargeCommission;
            office.Link = "/RealSateFinal/RealEstateDetail/" + franchiseOffice.GetObjectID();
        }

        private void Handle(Input.CreateOffice action)
        {
            action.Cancel();
            this.OpenModal = true;
        }
        
        private void Handle(Input.CancelTrigger action)
        {
            this.OpenModal = false;
        }
        
        private void Handle(Input.SortHomeSaleTrigger action)
        {
            PopulateSortedOffice(x => x.TotalHouseSold);
        }

        private void Handle(Input.SortTotalCommissionTrigger action)
        {
            PopulateSortedOffice(x => x.TotalCommission);
        }

        private void Handle(Input.SortAverageCommissionTrigger action)
        {
            PopulateSortedOffice(x => x.AveargeCommission);
        }

        private void PopulateSortedOffice(Func<RealEstateOffice, long> sortCriteria)
        {
            var data = this.Data as RealEstateInfo;
            var realEstateOffices = data?.RealEstateDetails.OrderByDescending(sortCriteria);
            this.Offices.Clear();
            SetOfficcesIntoView(realEstateOffices);
        }

        private void Handle(Input.CreateTrigger action)
        {
            Db.Scope(() =>
            {
                var newOfficeData = new RealEstateOffice()
                {
                    RealEstateInfo = this.Data as RealEstateInfo,
                    OfficeName = this.NewOffice.OfficeName,
                    
                    
                };
                var newOffice = this.Offices.Add();
                newOffice.OfficeName = this.NewOffice.OfficeName;
                newOffice.Link = "/RealSateFinal/RealEstateDetail/" + newOfficeData.GetObjectID();
            });

            this.Transaction.Commit();
            this.OpenModal = false;
        }
    }
}
