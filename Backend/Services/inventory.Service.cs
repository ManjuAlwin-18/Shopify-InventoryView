using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Example_FrontEnd.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopifySharp;

namespace Example_FrontEnd.Services
{

    public interface IInventoryService
    {
        void SetInventory();
        List<InventoryArticle> GetInventory();
    }

    public class InventoryService : IInventoryService
    {

        private string shopifyUrl = ""; // Add Shopify Url
        private string shopAccessToken = ""; // Add Token

        private readonly IMemoryCache _cache;
        private ProductService productService { get; set; }
        public InventoryService(IMemoryCache cache)
        {
            /* Sets cache */
            _cache = cache;
            _cache.Set("articles", new List<InventoryArticle>());
            
            /* Sets Service to get Products from Inventory in shopify */
            productService = new ProductService(shopifyUrl, shopAccessToken);
        }


        /* Gives list back from cache */
        public List<InventoryArticle> GetInventory()
        {
            return (List<InventoryArticle>) _cache.Get("articles");
        }


        public void SetInventory()
        {
            /* Make http request to shopify with the provided service */
            var productlist = productService.ListAsync().GetAwaiter().GetResult();
            /* Create new List articles */
            var articles = new List<InventoryArticle>();
            /* Get already existing list in memory cache */
            var existingArticles = (List<InventoryArticle>) _cache.Get("articles");

            /* Goes through each product in productlist */
            foreach (var product in productlist.Items)
            {

                string producttitle = product.Title;

                /* Goes through each variable in product */
                foreach (var variant in product.Variants)
                {

                    /* Checks if articles already exist in memory cache 
                     if not, 
                     */
                    if (!existingArticles.Any())
                    {
                        articles.Add(new InventoryArticle
                        {
                            id = variant.Id.ToString(),
                            producttitle = producttitle,
                            varianttitle = variant.Title,
                            amount = Convert.ToInt32(variant.InventoryQuantity),
                            delta = 2
                        });
                    } else
                    {
                        var item = existingArticles.FirstOrDefault(i => i.id == variant.Id.ToString());
                        if (item == null)
                        {
                            articles.Add(new InventoryArticle
                            {
                                id = variant.Id.ToString(),
                                producttitle = producttitle,
                                varianttitle = variant.Title,
                                amount = Convert.ToInt32(variant.InventoryQuantity),
                                delta = 2
                            });
                        } else
                        {
                            int delta = 0;
                            int amount = Convert.ToInt32(variant.InventoryQuantity);


                            if (item.amount > amount) { delta = 1; }
                            else if (item.amount == amount) { delta = 2; }
                            else { delta = 3; }

                            item.amount = amount;
                            item.delta = delta;

                            articles.Add(item);
                        }
                    }                    
                }
            }

            _cache.Set("articles", articles);
        }

    }

}
