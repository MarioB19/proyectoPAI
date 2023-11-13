namespace Programa;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;

using Backend;
using Programa.Paginas;

class Program
{
    // Configuracion
    private static IConfiguration configuracion;

    public static string connectionString;

    static void Main(string[] args)
    {
        // Configuración del archivo appsettings.json
        configuracion = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Obtener la cadena de conexión del archivo appsettings.json
        connectionString = configuracion.GetConnectionString("ConexionSQLite");

        // Configuración de la inyección de dependencias
        /*
        var serviceProvider = new ServiceCollection()
            .AddDbContext<SQLite>(options =>
                options.UseSqlite(connectionString))
            .BuildServiceProvider();
        */
        
        // Mostrar Inicio
        Inicio inicio;
        inicio = new Inicio(configuracion);
        do
        {
            inicio.Mostrar();
        } while (inicio.Ciclo);
    }
}
