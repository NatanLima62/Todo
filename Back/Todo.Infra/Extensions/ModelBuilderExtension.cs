using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Serilog;
using Todo.Domain.Contracts;

namespace Todo.Infra.Extensions;

public static class ModelBuilderExtension
{
    public static void ApplyEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.GetEntities<IEntity>();

        var props = entities.SelectMany(c => c.GetProperties().ToList());

        foreach (var property in props.Where(c => c.ClrType == typeof(int) && c.Name == "Id"))
        {
            property.IsKey();
        }
    }

    public static void ApplyTrackingConfiguration(this ModelBuilder modelBuilder)
    {
        var propDatas = new[] { "CriadoEm", "AtualizadoEm" };
        var propIds = new[] { "CriadoPor", "AtualizadoPor" };
        var propBools = new[] { "CriadoPorAdmin", "AtualizadoPorAdmin" };

        var entidades = modelBuilder.GetEntities<ITracking>();

        var dataProps = entidades
            .SelectMany(c
                => c.GetProperties().Where(p => p.ClrType == typeof(DateTime) && propDatas.Contains(p.Name)));

        foreach (var prop in dataProps)
        {
            prop.SetColumnType("timestamp");
            prop.SetDefaultValueSql("CURRENT_TIMESTAMP");
        }

        var idProps = entidades
            .SelectMany(c
                => c.GetProperties().Where(p => p.ClrType == typeof(int) && propIds.Contains(p.Name)));

        foreach (var prop in idProps)
        {
            prop.IsNullable = true;
        }

        var boolProps = entidades
            .SelectMany(c
                => c.GetProperties().Where(p => p.ClrType == typeof(bool) && propBools.Contains(p.Name)));

        foreach (var prop in boolProps)
        {
            prop.SetDefaultValue(false);
            prop.IsNullable = false;
        }
    }

    private static List<IMutableEntityType> GetEntities<T>(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(c => c.ClrType.GetInterface(typeof(T).Name) != null).ToList();

        return entities;
    }

    public static void ApplyGlobalFilter<TInterface>(this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newBody, newParam));
        }
    }

    public static ModelBuilder ApplyConfigurationsFromAssemblyWithServiceInjection(this ModelBuilder modelBuilder,
        Assembly assembly, params object[] services)
    {
        // get the method 'ApplyConfiguration()' so we can invoke it against instances when we find them
        var applyConfigurationMethod = typeof(ModelBuilder).GetMethods().Single(e => e.Name == "ApplyConfiguration" &&
            e.ContainsGenericParameters &&
            e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() ==
            typeof(IEntityTypeConfiguration<>));


        // test to find IEntityTypeConfiguration<> classes
        static bool IsEntityTypeConfiguration(Type i) =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);

        // find all appropriate classes, then create an instance and invoke the configure method on them
        assembly.GetConstructableTypes()
            .ToList()
            .ForEach(t => t.GetInterfaces()
                .Where(IsEntityTypeConfiguration)
                .ToList()
                .ForEach(i =>
                {
                    var hasServiceConstructor =
                        t.GetConstructor(services.Select(s => s.GetType()).ToArray()) != null;
                    var hasEmptyConstructor = t.GetConstructor(Type.EmptyTypes) != null;

                    if (hasServiceConstructor)
                    {
                        applyConfigurationMethod
                            .MakeGenericMethod(i.GenericTypeArguments[0])
                            .Invoke(modelBuilder, new[] { Activator.CreateInstance(t, services) });
                        Log.Information("Registering EF Config {type} with {count} injected services {services}",
                            t.Name, services.Length, services);
                    }
                    else if (hasEmptyConstructor)
                    {
                        applyConfigurationMethod
                            .MakeGenericMethod(i.GenericTypeArguments[0])
                            .Invoke(modelBuilder, new[] { Activator.CreateInstance(t) });
                        Log.Information("Registering EF Config {type} without injected services", t.Name);
                    }
                })
            );

        return modelBuilder;
    }

    private static IEnumerable<TypeInfo> GetConstructableTypes(this Assembly assembly)
    {
        return assembly.GetLoadableDefinedTypes()
            .Where(t => t is { IsAbstract: false, IsGenericTypeDefinition: false });
    }

    private static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
    {
        try
        {
            return assembly.DefinedTypes;
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null).Select(IntrospectionExtensions.GetTypeInfo!);
        }
    }
}