using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudentInfoSystem.Api.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentInfoSystem.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{
		}

		public DbSet<StudentEntity> Students { get; set; }


        //OnModelCreating represents how the model is configured;

        //How the tables will be created,
        //How the relationships (foreign keys, one-to-many) will be established,
        //You configure the property rules (HasMaxLength, HasKey, HasCheckConstraint, etc.)

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
			modelbuilder.ApplyConfiguration(new StudentEntityConfiguration());

            // Bu satır EF Core’un kendi temel ayarlarını da uygular
            base.OnModelCreating(modelbuilder);
        }
    }
}

