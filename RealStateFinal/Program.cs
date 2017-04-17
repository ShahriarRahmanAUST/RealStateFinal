using RealEstate;
using Starcounter;

namespace RealStateFinal
{
    class Program
    {

        static void Main()
        {

            //Db.Transact(() =>
            //{
            //    var anyone = Db.SQL<RealEstateOffice>("SELECT s FROM RealEstateOffice s").First;
            //    if (anyone != null)
            //    {
            //        new HouseInformation()
            //        {
            //            Street = "St1",
            //            Number = 1,
            //            Country = "Sweden",
            //            Price = 10,
            //            Commission = 2,
            //            Date = DateTime.Parse("2017-4-15"),
            //            RealEstateOffice = anyone

            //        };
            //    }
            //});


            Handle.GET("/RealStateFinal/master", () =>
            {
                return Db.Scope(() =>
                {
                    if (Session.Current != null && Session.Current.Data != null)
                    {
                        return Session.Current.Data;
                    }
                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }

                    var master = new MasterPage();
                    var dashboardPage = new MainPage();
                    var corporateCompany = Db.SQL<RealEstateInfo>("SELECT s FROM RealEstateInfo s");

                    if (corporateCompany != null)
                    {
                        dashboardPage.Data = corporateCompany;
                    }
                    master.CurrentPage = dashboardPage;
                    master.Session = Session.Current;
                    return master;
                });
            });


            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            var realEstateInfo = Db.SQL<RealEstateInfo>("SELECT s FROM RealEstateInfo s");


            Handle.GET("/RealStateFinal/partial/mainpage", () => new MainPage());

            Handle.GET("/RealStateFinal", () =>
            {
                return Db.Scope(() =>
                {
                    var companies = Db.SQL<RealEstateInfo>("SELECT s FROM RealEstateInfo s");
                    return WrapPage<MainPage>("/RealStateFinal/partial/mainpage", companies);
                });

            });


            Handle.GET("/RealSateFinal/partial/realState", () => new RealStateHome());

            Handle.GET("/RealSateFinal/realState/{?}", (string id) =>
            {
                return Db.Scope(() =>
                {
                    var company = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id));
                    return WrapPage<RealStateHome>("/RealSateFinal/partial/realState", company);
                });
            });


            Handle.GET("/RealSateFinal/partial/RealEstateDetail", () => new RealEstateDetail());


            Handle.GET("/RealSateFinal/RealEstateDetail/{?}", (string id) =>
            {
                return Db.Scope(() =>
                {
                    var realEstateDetail = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id));
                    return WrapPage<RealEstateDetail>("/RealSateFinal/partial/RealEstateDetail", realEstateDetail);
                });
            });





        }

        private static Json WrapPage<T>(string partialPath, object data = null) where T : Json
        {
            var masterPage = (MasterPage)Self.GET("/RealStateFinal/master");

            if (masterPage.CurrentPage == null || masterPage.CurrentPage.GetType() != typeof(T))
            {
                masterPage.CurrentPage = Self.GET(partialPath);
            }

            if (data != null)
            {
                masterPage.CurrentPage.Data = data;
            }
            return masterPage;
        }
    }
}