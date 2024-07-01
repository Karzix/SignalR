using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Consumer.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext()
        {

        }
        public ProductDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-TMN6UG6G;Initial Catalog=Product;Integrated Security=True;Trust Server Certificate=True");
            }


        }
    }
}
