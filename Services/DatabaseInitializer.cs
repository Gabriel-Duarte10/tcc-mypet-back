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
        }
    }
}