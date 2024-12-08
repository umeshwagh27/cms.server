using cms.data.Data;
using cms.data.Model;
namespace cms.server.Utility
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(CMSDbContext db)
        {
            SeedCar(db);
        }

        private static void SeedCar(CMSDbContext db)
        {
            bool isCarExist = db.Cars.Any(x => x.Brand == "Audi" || x.Brand == "Jaguar" || x.Brand.Trim() == "Landrover" && x.Brand == "Renault");
            if (!isCarExist)
            {
                var accessControls = new List<Car>
                {
                    new Car{Brand="Audi",Description="Audi Desciption",ModelCode="Audi",ModelName="Audi",DateofManufacturing= DateTime.Now },
                    new Car{Brand="Jaguar",Description="Jaguar",ModelCode="Jaguar",ModelName="Jaguar",DateofManufacturing= DateTime.Now },
                    new Car{Brand="Land rover",Description="Land rover",ModelCode="Land rover",ModelName="Land rover",DateofManufacturing= DateTime.Now },
                    new Car{Brand="Renault",Description="Renault",ModelCode="M#Renault",ModelName="M#Renault",DateofManufacturing= DateTime.Now },
                };
                db.Cars.AddRange(accessControls);
                db.SaveChanges();
            }
        }
    }
}