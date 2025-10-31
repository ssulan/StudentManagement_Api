using System;
using StudentInfoSystem.Api.Data.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace StudentInfoSystem.Api.Data
{
	public class DbSeed
	{

        public static async Task SeedAsync(AppDbContext dbContext)
        {

            // Seed Fake Data
            try
            {

                if (!await dbContext.Students.AnyAsync())
                {
                    var studentFaker = new Faker<StudentEntity>()
                    .RuleFor(s => s.FirstName, f =>
                    {
                        var firstName = f.Name.FirstName() ?? "Ahmet";
                        return firstName.Length <= 50 ? firstName : firstName.Substring(0, 50);
                    })
                    .RuleFor(s => s.LastName, f =>
                    {
                        var lastName = f.Name.LastName() ?? "Kara";
                        return lastName.Length <= 100 ? lastName : lastName.Substring(0,100);
                    })
                    .RuleFor(s => s.Branch, f => f.PickRandom(new[] { "A", "B", "C", "D", "E" })) // tek karakter
                    .RuleFor(s => s.Number, f =>
                    {
                        var firstDigit = f.Random.Number(1, 9).ToString();
                        var remainingDigits = f.Random.String2(4, "0123456789"); // her zaman bu rakamlardan 4 tane al
                        return firstDigit + remainingDigits; // toplam 5 karakter
                    });


                    var fakeStudents = studentFaker.Generate(20);

                    dbContext.Students.AddRange(fakeStudents);
                    await dbContext.SaveChangesAsync();

                    Console.Clear();
                    Console.WriteLine("Data Created Successfuly");

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Data Already Exist.");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Data Creation Failed.");
                Console.WriteLine(ex.Message);
            }


          

		}
	}
}

