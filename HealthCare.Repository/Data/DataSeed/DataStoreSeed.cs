using HealthCare.Core.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Repository.Data.DataSeed
{
    public class DataStoreSeed
    {
        public static async Task SeedAsync(HealthCareContext dbContext)
        {
            var ServicePath = "../HealthCare.Repository/Data/DataSeed/Locations.json";

            await TransferData<Services>(ServicePath, dbContext);

            var HWIdPath = "../HealthCare.Repository/Data/DataSeed/HardwareId.json";

            await TransferData<Hardware>(HWIdPath, dbContext);

        }

        private static async Task TransferData<T>(string DataPath, HealthCareContext dbContext) where T : AppEntity
        {
            if (!dbContext.Set<T>().Any())
            {
                var ItemsData = File.ReadAllText(DataPath);
                var Items = JsonSerializer.Deserialize<List<T>>(ItemsData);
                if (Items?.Count > 0)
                {
                    foreach (var Item in Items)
                        await dbContext.Set<T>().AddAsync(Item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
