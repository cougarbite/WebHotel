using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using WebHotels.WebUI.Models.WebHotelsDB;

namespace WebHotels.WebUI.Data
{
    public partial class WebHotelsDBContext : DbContext
    {
        public WebHotelsDBContext()
        {
        }

        public WebHotelsDBContext(DbContextOptions<WebHotelsDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<WebHotels.WebUI.Models.WebHotelsDB.Client> Clients { get; set; }

        public DbSet<WebHotels.WebUI.Models.WebHotelsDB.Employee> Employees { get; set; }
    }
}