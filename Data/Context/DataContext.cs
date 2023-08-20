using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Models;

namespace tcc_mypet_back.Data.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<FavoritePet> FavoritePets { get; set; }
        public DbSet<ReportedPet> ReportedPets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<ReportedProduct> ReportedProducts { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<UserPetChat> UserPetChats { get; set; }
        public DbSet<UserProductChat> UserProductChats { get; set; }
        public DbSet<UserPetChatSession> UserPetChatSessions { get; set; }
        public DbSet<UserProductChatSession> UserProductChatSessions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AnimalType
            modelBuilder.Entity<AnimalType>()
                .HasOne(at => at.Administrator)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Breed
            modelBuilder.Entity<Breed>()
                .HasOne(b => b.AnimalType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Breed>()
                .HasOne(b => b.Administrator)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Pet
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Characteristic)
                .WithMany();
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Breed)
                .WithMany();
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Size)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.User)
                .WithMany();

            // Size
            modelBuilder.Entity<Size>()
                .HasOne(s => s.Administrator)
                .WithMany();

            // PetImage
            modelBuilder.Entity<PetImage>()
                .HasOne(pi => pi.Pet)
                .WithMany();

            // ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany();

            // UserImage
            modelBuilder.Entity<UserImage>()
                .HasOne(ui => ui.User)
                .WithMany();

            // Characteristic
            modelBuilder.Entity<Characteristic>()
                .HasOne(f => f.Administrator)
                .WithMany();

            // UserPetChat
            modelBuilder.Entity<UserPetChat>()
                .HasOne(upc => upc.SenderUser)
                .WithMany();
            modelBuilder.Entity<UserPetChat>()
                .HasOne(upc => upc.UserPetChatSession)
                .WithMany();
            modelBuilder.Entity<UserPetChat>()
                .HasOne(upc => upc.SenderUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserPetChat>()
                .HasOne(upc => upc.UserPetChatSession)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            // UserProductChat
            modelBuilder.Entity<UserProductChat>()
                .HasOne(upc => upc.SenderUser)
                .WithMany();
            modelBuilder.Entity<UserProductChat>()
                .HasOne(upc => upc.UserProductChatSession)
                .WithMany();
            modelBuilder.Entity<UserProductChat>()
                .HasOne(upc => upc.SenderUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserProductChat>()
                .HasOne(upc => upc.UserProductChatSession)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            // UserPetChatSession
            modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.Pet)
                .WithMany();
             modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.User2)
                .WithMany();
            modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.User1)
                .WithMany();

            // UserProductChatSession
            modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.Product)
                .WithMany();
             modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.User2)
                .WithMany();
            modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.User1)
                .WithMany();

            // UserPetChatSession
            modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.Pet)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.User2)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<UserPetChatSession>()
                .HasOne(upc => upc.User1)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // UserPetChatSession
            modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.User1)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserProductChatSession>()
                .HasOne(upc => upc.User2)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // FavoriteProducts
            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(fp => fp.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // FavoritePets
            modelBuilder.Entity<FavoritePet>()
                .HasOne(fp => fp.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FavoritePet>()
                .HasOne(fp => fp.Pet)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var deleteAtProperty = entityType.FindProperty("DeleteAt");
                if (deleteAtProperty != null && deleteAtProperty.ClrType == typeof(DateTime?))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var propertyAccess = Expression.PropertyOrField(parameter, "DeleteAt");
                    var nullConstant = Expression.Constant(null, typeof(DateTime?));
                    var comparison = Expression.Equal(propertyAccess, nullConstant);
                    var lambda = Expression.Lambda(comparison, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

        }
    }
}