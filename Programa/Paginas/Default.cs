namespace Programa.Paginas;

using Backend;
using Entidades;

using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System;
using System.Linq.Expressions;

public class Default : PaginaBase
{
	public int IdUsuarioActual { get; set; }

	public int IdTpoUsuarioActual { get; set; }

	public Usuario UsuarioActual { get; set; }

	public Default(IConfiguration configuracion) : base("Default", configuracion)
	{
		IdUsuarioActual = -1;
		if (IdTpoUsuarioActual != null && IdTpoUsuarioActual != 0)
		{
			base.IdTpoUsuarioActual = this.IdTpoUsuarioActual;
		}
	}

	/*
	TODO: Completar funcion
	? Metodo encargado de mostrar la pagina
	*/
	public override void Mostrar()
	{
		MostrarMenu();
	}

	/*
    TODO: Realizar la validacion de las interfaces en base al tipo de usuario actual
    ? Metodo encargado de mostrar las pantallas disponibles para el usuario en base a su tipo
	*/
	public void MostrarMenu()
	{
		Console.Clear();
		// Consulta el usuario actual

		if (!ObtenerUsuarioActual())
		{
			return;
		}
		Console.WriteLine($"Bienvenido {UsuarioActual.Nombre}, ");

		try
		{
			// Consulta las interfaces a las cuales tiene acceso en base a su tipo de usuario
			List<Interfaz> interfaces = SQLite.ObtenerInterfacesPorTpoUsuario(IdTpoUsuarioActual);

			if (interfaces.Count != 0)
			{
				AnsiConsole.MarkupLine("[bold yellow]Por favor selecciona una opción[/]");
			}
			else
			{
				AnsiConsole.WriteLine("Lo sentimos, pero por el momento no tienes el permiso de accesar a ninguna interfaz");
			}

			var opciones = interfaces.Select(i => i.Nombre).ToList();
			opciones.Add("Cerrar Sesion"); //Todos los usuarios tienen la oportunidad de cerrar sesión.

			var seleccionPrompt = new SelectionPrompt<string>()
				//.Title("Opciones")
				.AddChoices(opciones);

			var opcionSeleccionada = AnsiConsole.Prompt(seleccionPrompt);

			try
			{
				if (opcionSeleccionada == "Cerrar Sesion")
				{
					Console.WriteLine("Saliendo del sistema...");
					Console.WriteLine("Presiona cualquier tecla para continuar...");
					Console.ReadKey();
				}
				else
				{
					Console.WriteLine(opcionSeleccionada);
					Console.ReadKey();
					switch (opcionSeleccionada)
					{
						
						case "MantenimientoAlmacenistas":
							MantenimientoAlmacenistas mantenimientoAlmacenistas = new MantenimientoAlmacenistas(base.configuracion);
							mantenimientoAlmacenistas.IdUsuarioActual = IdUsuarioActual;
							mantenimientoAlmacenistas.Mostrar();
							break;
						case "MantenimientoPrestamos":
							MantenimientoPrestamos mantenimientoPrestamos = new MantenimientoPrestamos(base.configuracion);
							mantenimientoPrestamos.IdUsuarioActual = IdUsuarioActual;
							mantenimientoPrestamos.Mostrar();
							break;
						case "MantenimientoCoordinadores":
							MantenimientoCoordinadores mantenimientoCoordinadores = new MantenimientoCoordinadores(base.configuracion);
							mantenimientoCoordinadores.IdUsuarioActual = IdUsuarioActual;
							mantenimientoCoordinadores.Mostrar();
							break;
						case "MantenimientoProfesores":
							MantenimientoProfesores mantenimientoProfesores = new MantenimientoProfesores(base.configuracion);
							mantenimientoProfesores.IdUsuarioActual = IdUsuarioActual;
							mantenimientoProfesores.Mostrar();
							break;
						case "MantenimientoMantenimientos":
							MantenimientoMantenimientos mantenimientoMantenimientos = new MantenimientoMantenimientos(base.configuracion);
							mantenimientoMantenimientos.IdUsuarioActual = IdUsuarioActual;
							mantenimientoMantenimientos.Mostrar();
							break;
						case "MantenimientoEstudiantes":
							MantenimientoEstudiantes mantenimientoEstudiantes = new MantenimientoEstudiantes(base.configuracion);
							mantenimientoEstudiantes.IdUsuarioActual = IdUsuarioActual;
							mantenimientoEstudiantes.Mostrar();
							break;
						default:
							break;
					}
					string nombreClase = opcionSeleccionada;
					Type tipoClase = Type.GetType(nombreClase);



					if (tipoClase != null)
					{
						Console.WriteLine(tipoClase.Name);
						Console.ReadKey();

						dynamic pagina = Activator.CreateInstance(tipoClase, base.configuracion);
						pagina.IdUsuarioActual = IdUsuarioActual;
						pagina.Mostrar();
					}
					else
					{
						AnsiConsole.MarkupLine("[red]Opción no válida.[/]");
						Console.WriteLine("Presiona cualquier tecla para continuar...");
						Console.ReadKey();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
				Console.WriteLine("Stack trace: " + e.StackTrace);
				Console.WriteLine("Excepción datos: " + e.Data);
				if (e.InnerException != null)
				{
					Console.WriteLine("Excepción interna: " + e.InnerException.Message);
					Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
				}

				Console.ReadKey();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
				Console.WriteLine("Stack trace: " + e.StackTrace);
				Console.WriteLine("Excepción datos: " + e.Data);
				if (e.InnerException != null)
				{
					Console.WriteLine("Excepción interna: " + e.InnerException.Message);
					Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
				}

				Console.ReadKey();
		}
	}

	//Verifica si el usuario existe
	public bool ObtenerUsuarioActual()
	{
		bool resultado = false;

		try
		{
			UsuarioActual = SQLite.ObtenerUsuarioPorId(IdUsuarioActual);
			resultado = true;
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al consultar el usuario actual, porfavor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar");
			Console.ReadKey();
		}

		return resultado;
	}
}
