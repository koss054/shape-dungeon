using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Data;
using System;

namespace ShapeDungeon.Tests
{
    public static class ContextGenerator
    {
        public static AppDbContext Generate()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
