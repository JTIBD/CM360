using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FMAplication.Domain;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Models;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace FMAplication.Helpers
{
    public class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await WarehouseStockWithPOSM(context);
                await SalesPointStockWithPosm(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex.Message);
            }
        }

        private static async Task WarehouseStockWithPOSM(ApplicationDbContext context)
        {
            var warehouses = await context.WareHouses.ToListAsync();
            var wareHouseStocks = await context.WareHouseStocks.Where(x => x.Status == Status.Active).ToListAsync();
            var posmProductIds =
                await context.POSMProducts.Where(x => x.Status == Status.Active && x.IsJTIProduct).Select(x => x.Id).ToListAsync();

            List<WareHouseStock> newWareHouseStocks = new List<WareHouseStock>();

            foreach (var wareHouse in warehouses)
            {
                var posmStockProducts = wareHouseStocks.Where(x => x.WareHouseId == wareHouse.Id && x.Status == Status.Active)
                    .Select(x => x.PosmProductId).ToList();
                var posmNotInStock = posmProductIds.Where(x => !posmStockProducts.Contains(x)).ToList();
                foreach (var i in posmNotInStock)
                {
                    newWareHouseStocks.Add(new WareHouseStock
                    {
                        PosmProductId = i, WareHouseId = wareHouse.Id, Quantity = 0,
                        CreatedTime = DateTime.UtcNow, ModifiedTime = DateTime.UtcNow
                    });
                }
            }

            await context.WareHouseStocks.AddRangeAsync(newWareHouseStocks);
            await context.SaveChangesAsync();
        }
        private static async Task SalesPointStockWithPosm(ApplicationDbContext context)
        {
            var salesPoints = await context.SalesPoints.ToListAsync();
            var salesPointStocks = await context.SalesPointStocks.Where(x => x.Status == Status.Active).ToListAsync();
            var posmProductIds = await context.POSMProducts.Where(x => x.Status == Status.Active && x.IsJTIProduct).Select(x => x.Id).ToListAsync();

            List<SalesPointStock> newSalesPointStocks= new List<SalesPointStock>();

            foreach (var salesPoint in salesPoints)
            {
                var posmStockProducts = salesPointStocks.Where(x => x.SalesPointId == salesPoint.SalesPointId && x.Status == Status.Active)
                                                                .Select(x => x.POSMProductId).ToList();

                var posmNotInStock = posmProductIds.Where(x => !posmStockProducts.Contains(x)).ToList();
                foreach (var i in posmNotInStock)
                {
                    newSalesPointStocks.Add(new SalesPointStock
                    {
                        POSMProductId = i,
                        SalesPointId = salesPoint.SalesPointId,
                        Quantity = 0,
                        CreatedTime = DateTime.UtcNow,
                        ModifiedTime = DateTime.UtcNow
                    });
                    if (newSalesPointStocks.Any() && newSalesPointStocks.Count>3000)
                    {
                        context.SalesPointStocks.AddRange(newSalesPointStocks);
                        await context.SaveChangesAsync();
                        newSalesPointStocks= new List<SalesPointStock>();
                    }
                }
            }

            if (newSalesPointStocks.Any())
            {
                context.SalesPointStocks.AddRange(newSalesPointStocks);
                await context.SaveChangesAsync();
            }
        }
    }
}
