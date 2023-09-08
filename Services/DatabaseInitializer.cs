using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Models;

namespace tcc_mypet_back.Services
{
    public static class DatabaseInitializer
    {
        public static void Initialize(DataContext context) // Assumindo que o nome do seu contexto seja MyPetDataContext
        {
            if (!context.Database.CanConnect())
            {
                context.Database.Migrate();
            }

            if (context.AnimalTypes.Any())
            {
                return; // O banco de dados já foi inicializado
            }

            // Cria exemplos para Administrators
            for (int i = 1; i <= 5; i++)
            {
                var admin = new Administrator { 
                    Name = $"Admin{i}", 
                    Email = $"admin{i}@email.com", 
                    Password = $"pass{i}", 
                    Cellphone = $"12345678{i}0", 
                    CellphoneCode = i 
                };
                context.Administrators.Add(admin);
            }
            context.SaveChanges();

            // Cria exemplos para AnimalType
            for (int i = 1; i <= 5; i++)
            {
                var type = new AnimalType { 
                    Name = $"Type{i}", 
                    AdministratorId = i 
                };
                context.AnimalTypes.Add(type);
            }
            context.SaveChanges();

            // Cria exemplos para Breed
            for (int i = 1; i <= 5; i++)
            {
                var breed = new Breed { 
                    Name = $"Breed{i}", 
                    AdministratorId = i, 
                    AnimalTypeId = i 
                };
                context.Breeds.Add(breed);
            }
            context.SaveChanges();

            // Cria exemplos para Characteristic
            for (int i = 1; i <= 5; i++)
            {
                var characteristic = new Characteristic { 
                    Name = $"Characteristic{i}", 
                    AdministratorId = i 
                };
                context.Characteristics.Add(characteristic);
            }
            context.SaveChanges();

            // Cria exemplos para Size
            for (int i = 1; i <= 5; i++)
            {
                var size = new Size { 
                    Name = $"Size{i}", 
                    AdministratorId = i 
                };
                context.Sizes.Add(size);
            }
            context.SaveChanges();
        }
    }
}