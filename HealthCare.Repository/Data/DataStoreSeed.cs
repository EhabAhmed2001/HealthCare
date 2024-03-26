using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthCare.Core.Entities.Data;
using HealthCare.Repository.Data;
using Microsoft.EntityFrameworkCore;


namespace Talabat.Repository.Data
{
	public class DataStoreSeed 
	{
		public static async Task SeedAsync (HealthCareContext dbContext)
		{
			var ServicePath = "../HealthCare.Repository/Data/DataSeed/Locations.json";

			await TransferData<Service>(ServicePath, dbContext);


			///if(!dbContext.ProductBrands.Any())
			///{
			///	var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
			///	var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
			///	if(Brands?.Count > 0)
			///	{
			///		foreach(var Brand in Brands)
			///			await dbContext.Set<ProductBrand>().AddAsync(Brand);
			///		await dbContext.SaveChangesAsync();
			///	}
			///}
		}

		private static async Task TransferData <T>(string DataPath, HealthCareContext dbContext) where T : AppEntity
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
