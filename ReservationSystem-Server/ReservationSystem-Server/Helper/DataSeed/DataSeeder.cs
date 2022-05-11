using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ReservationSystem_Server.Helper.DataSeed;

public class DataSeeder
{
    private ModelBuilder _builder;

    private DataSeeder(ModelBuilder builder)
    {
        _builder = builder;
    }
    
    public static void Seed(ModelBuilder builder)
    {
        new DataSeeder(builder).Seed();
    }

    private void Seed()
    {
        // find all DataSeederAttribute on methods in assembly
        var seeders = GetType().Assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
                .Select(m => new { method = m, attribute = m.GetCustomAttributes(typeof(DataSeederAttribute), false) as DataSeederAttribute[] })
                .Where(ma => ma.attribute?.Length > 0)
                .ToList();
        
        // run each seeder
        foreach (var seeder in seeders)
        {
            DataSeederAttribute? attribute = seeder.attribute?.SingleOrDefault();
            if (attribute == null) continue;

            ParameterInfo parameter = seeder.method.GetParameters().Single() ?? throw new InvalidOperationException("DataSeeder methods must have a single parameter");
            Type parameterType = parameter.ParameterType;
            
            if(!parameterType.IsGenericType && parameterType.GenericTypeArguments.Length != 1)
                throw new InvalidOperationException("DataSeeder methods must have a generic parameter (<>)");
            
            Type genericParameter = parameterType.GetGenericArguments().Single();

            Type genericType = typeof(List<>).MakeGenericType(genericParameter);
            
            if(!genericType.IsAssignableTo(parameterType))
                throw new InvalidOperationException("DataSeeder methods must have a parameter assignable from List<>");
            
            object? instance = Activator.CreateInstance(genericType);
            
            seeder.method.Invoke(null, new [] { instance });
            GetType().GetMethod(nameof(AddData), BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(genericParameter)
                    .Invoke(this, new [] { instance });
        }
    }
    
    private void AddData<T>(IEnumerable<T> data) where T : class
    {
        _builder.Entity<T>().HasData(data);
    }
}