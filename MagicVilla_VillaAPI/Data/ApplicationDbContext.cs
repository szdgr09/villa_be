using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        public DbSet<Villa> Villas { get; set; }

        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "This is a sample Royal Villa located somewhere in the atlantic Ocean",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Calamari Villa",
                    Details = "Indian Resort",
                    ImageUrl = "",
                    Occupancy = 10,
                    Rate = 300,
                    Sqft = 600,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Philippine Manila Master Villa",
                    Details = "Makati Circuit",
                    ImageUrl = "",
                    Occupancy = 50,
                    Rate = 200000,
                    Sqft = 700,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
