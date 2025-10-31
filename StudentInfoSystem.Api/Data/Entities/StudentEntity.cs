using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentInfoSystem.Api.Data.Entities
{
	public class StudentEntity
	{
		public int Id { get; set; }

		public string Number { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Branch { get; set; } = null!;

    }

	//Fluent Api - Configuration of StudentEntity
	public class StudentEntityConfiguration : IEntityTypeConfiguration<StudentEntity>
	{
		public void Configure(EntityTypeBuilder<StudentEntity> builder)
		{
			//Id 
			builder.HasKey(s => s.Id); //Primary Key definition

			builder.Property(s => s.Id)
				.IsRequired()
                .ValueGeneratedOnAdd(); //Id auto-increment

			//Number
			builder.Property(s => s.Number)
				.IsRequired()
				.HasMaxLength(5)
				.IsFixedLength();

			//Unique Index for Number
			builder.HasIndex(s => s.Number).IsUnique();

            //Name
            builder.Property(s => s.FirstName)
				.IsRequired()
				.HasMaxLength(50); //max length of property

			//LastName
			builder.Property(s => s.LastName)
				.IsRequired()
				.HasMaxLength(100);

			//Branch
			builder.Property(s => s.Branch)
				.IsRequired()
				.HasMaxLength(1);

				
		}
	}
}

