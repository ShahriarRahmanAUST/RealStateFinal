using System.Collections.Generic;

using Starcounter;

namespace RealStateFinal
{
    partial class MainPage : Json
    {
        protected override void OnData()
        {
            base.OnData();

            base.OnData();
            var corporations = this.Data as IEnumerable<RealEstateInfo>;
            this.Corporations.Clear();
            foreach (var corporation in corporations)
            {
                var corp = this.Corporations.Add();
                corp.Name = corporation.RealEstateName;
                corp.Link = "/RealSateFinal/realState/" + corporation.GetObjectID();
            }

        }

        private void Handle(Input.CreateCorporationTrigger action)
        {
            Db.Scope(() =>
            {
                var company = new RealEstateInfo
                {
                    RealEstateName = this.CorporationName
                };
                var companyJson = this.Corporations.Add();
                companyJson.Name = this.CorporationName;
                companyJson.Link = "/RealSateFinal/realState/" + company.GetObjectID();
            });
            this.Transaction.Commit();
            this.CorporationName = "";
        }
    }
}
