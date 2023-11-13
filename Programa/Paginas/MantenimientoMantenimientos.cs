namespace Programa.Paginas;

using Backend;
using Entidades;

using Microsoft.Extensions.Configuration;
using System;
using System.Text;



public class MantenimientoMantenimientos : PaginaBase
{
    // ? Bandera encargada de manejar si 
    public bool _Aprobar { get; set; }

	public Almacenista AlmacenistaActual { get; set; }

	public Usuario UsuarioActual { get; set; }

    public Mantenimiento MantenimientoActual { get; set; }

    public Equipo EquipoActual { get; set; }

	public int IdUsuarioActual { get; set; }

	public int IdMantenimientoActual{ get; set; }

	public bool MantenimientoNuevo { get; set; }

	public MantenimientoMantenimientos(IConfiguration configuracion) : base("MantenimientoMantenimientos", configuracion)
	{
		IdUsuarioActual = -1;
        //IdTpoMantenimientoActual = -1;
		MantenimientoNuevo = true;
		_Aprobar = false;
		AlmacenistaActual = null;
		UsuarioActual = null;
		IdMantenimientoActual = -1;
	}

    public override void Mostrar()
	{
		MantenimientoNuevo = IdUsuarioActual == -1;
		if (!MantenimientoNuevo)
		{
			ConsultarAlmacenista();
				base.IdTpoUsuarioActual = (int)UsuarioActual.IdTpoUsuario;
		}

	
		if (!base.TieneAccesoAInterfazActual)
		{
			return;
		}
		MantenimientoNuevo = IdMantenimientoActual == -1;
		if (MantenimientoNuevo)
		{
			Agregar();
		}
		else
		{
			_Aprobar = UsuarioActual.IdTpoUsuario == (int)Enumeradores.tps_usuarios.Coordinador && _Aprobar;
			if (_Aprobar)
			{
				//Aprobar();
			}
			else
			{
				Actualizar();
			}
		}
	}

    public void ConsultarAlmacenista()
	{
		UsuarioActual = null;
		AlmacenistaActual = null;

		// Consulta del usuario
		try
		{
			//UsuarioActual = SQLite.ObtenerUsuarioPorId();
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al consultar el usuario, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}

		// Consulta del almacenista
		try
		{
			AlmacenistaActual = SQLite.ObtenerAlmacenistaPorIdUsuario(UsuarioActual.Id);
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al consultar el almacenista, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}

	}

    public void ConsultarMantenimiento()
    {
		MantenimientoActual = null;
		EquipoActual = null;

		// Consulta del mantenimiento
		try
		{
			//MantenimientoActual = SQLite.ObtenerMantenimientoPorId(idMantenimientoActual);
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al consultar el mantenimiento, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}

		// Consulta del equipo
		try
		{
			EquipoActual = SQLite.ObtenerEquipoPorId(MantenimientoActual.IdEquipo);
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al consultar el equipo, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}

    }

	/*
	? Metodo permite actualizar al almacenista
	*/
public void Actualizar()
{
	MostrarMantenimiento();

	 
	//Mostrar equipos disponibles

	/*foreach(var equipo in SQLite.ObtenerEquipos())
	{
		Console.WriteLine($"Id: {equipo.Id} Nombre: {equipo.Nombre}");
	}

	*/

	Console.Write("Ingresa el id del equipo: ");
	Console.WriteLine();

    Console.Write("Ingresa la nueva descripción del mantenimiento: ");
    string nuevaDescripcion = Console.ReadLine();


/*
    while (!Utilidades.VerificarDescripcion(nuevaDescripcion))
    {
        Console.Clear();
        Console.WriteLine("Descripción no válida, inténtalo de nuevo");
        Console.Write("\nIngresa la nueva descripción del mantenimiento: ");
        nuevaDescripcion = Console.ReadLine();
    };
*/
    Console.Write("Ingresa la nueva refacción del mantenimiento: ");
    string nuevaRefaccion = Console.ReadLine();

/*
    while (!Utilidades.VerificarRefaccion(nuevaRefaccion))
    {
        Console.Clear();
        Console.WriteLine("Refacción no válida, inténtalo de nuevo");
        Console.Write("\nIngresa la nueva refacción del mantenimiento: ");
        nuevaRefaccion = Console.ReadLine();
    };

	*/

    try
    {
        // Actualizar el mantenimiento
        MantenimientoActual.FchModificacion = DateTime.Now;
        MantenimientoActual.Descripciones = nuevaDescripcion;
        MantenimientoActual.Refaccion = nuevaRefaccion;

        // Guardar los cambios en la base de datos
        SQLite.SaveChanges();

        Console.WriteLine("Cambios guardados exitosamente.");
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }
    catch (Exception e)
    {
        Console.WriteLine("Ocurrió un error al actualizar el mantenimiento, por favor intenta más tarde");
        Console.WriteLine($"Mensaje de error: {e.Message}");
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }
}



	/*
	? Metodo para pedir la informacion del mantenimiento con su equipo relacionado
	*/
	public void Agregar()
	{
		bool almacenistaAgregado = false;
		bool usuarioAgregado = false;

		Console.Clear();
		Console.WriteLine("Agregando un nuevo Almacenista");

		// Solicitar los datos del usuario
		Console.Write("Ingresa el nombre del usuario: ");
		string nombre = Console.ReadLine();

		while (!Utilidades.VerificarNombre(nombre))
		{
			Console.Clear();
			Console.WriteLine("Nombre no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el nombre del usuario: ");
			nombre = Console.ReadLine();
		};

		Console.Write("Ingresa el apellido paterno del usuario: ");
		string aplPaterno = Console.ReadLine();

		while (!Utilidades.VerificarNombre(aplPaterno))
		{
			Console.Clear();
			Console.WriteLine("Apellido paterno no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el apellido paterno del usuario: ");
			aplPaterno = Console.ReadLine();
		};

		Console.Write("Ingresa el apellido materno del usuario: ");
		string aplMaterno = Console.ReadLine();

		while (!Utilidades.VerificarNombre(aplMaterno))
		{
			Console.Clear();
			Console.WriteLine("Apellido materno no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el apellido materno del usuario: ");
			aplMaterno = Console.ReadLine();
		};

		Console.Write("Ingresa la nomina: ");
		string nomina = Console.ReadLine();

		while (!Utilidades.VerificarRegistroNomina(nomina))
		{
			Console.Clear();
			Console.WriteLine("Nómina no válida, inténtalo de nuevo");
			Console.Write("\nIngresa la nomina: ");
			nomina = Console.ReadLine();
		};

		Console.Write("Ingresa la contraseña del usuario: ");
		string contrasena = Console.ReadLine();
		string contrasenaEncriptada = Utilidades.EncriptarContrasenaSHA256(contrasena);

		try
		{

			UsuarioActual = new Usuario
			{
				IdTpoUsuario = (long)Enumeradores.tps_usuarios.Almacenista,
				IdEstUsuario = (long)Enumeradores.est_usuarios.Activo,
				Nombre = nombre,
				AplPaterno = aplPaterno,
				AplMaterno = aplMaterno,
				Contrasena = Encoding.UTF8.GetBytes(contrasenaEncriptada),
				FchCreacion = DateTime.Now
			};

			// Guardar el nuevo usuario en la base de datos
			usuarioAgregado = SQLite.InsertarUsuario(UsuarioActual);

			if (usuarioAgregado)
			{
				// Crear una nueva instancia de Almacenista y asignar el ID del usuario
				AlmacenistaActual = new Almacenista
				{
					IdUsuario = UsuarioActual.Id,
					Nomina = long.Parse(nomina),
					FchCreacion = DateTime.Now
				};

				// Insertar el almacenista en la base de datos
				almacenistaAgregado = SQLite.InsertarAlmacenista(AlmacenistaActual);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrió un error al agregar el nuevo almacenista, por favor inténtalo más tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}

		if (!almacenistaAgregado)
		{
			Console.WriteLine("Error al agregar el almacenista. Verifica los datos y vuelve a intentarlo.");
			Console.WriteLine("Presiona cualquier tecla para continuar...");
			Console.ReadKey();
			return;
		}

		Console.WriteLine("Almacenista agregado exitosamente.");
		Console.WriteLine("Presiona cualquier tecla para continuar...");
		Console.ReadKey();
	}

    public void MostrarMantenimiento()
    {
        Console.Clear();
        Console.WriteLine("Datos del mantenimiento");
        
        Console.WriteLine($"Tipo de mantenimiento: {Enum.GetName(typeof(Enumeradores.tps_mantenimiento), MantenimientoActual.IdTpoMantenimiento)}");
        Console.WriteLine($"Nombre del equipo: {EquipoActual.Nombre}");
        Console.WriteLine($"Descripcion del equipo: {MantenimientoActual.Descripciones}");
        Console.WriteLine($"Refaccion(es): {MantenimientoActual.Refaccion}");
        if (MantenimientoActual.FchModificacion != null)
		{
			Console.WriteLine($"Ultima fecha de modificación: {MantenimientoActual.FchModificacion}");
		}
		else
		{
			Console.WriteLine("Ultima fecha de modificación: No disponible");
		}
    }


/*
    public int SeleccionarEquipo()
    {

        
    }

	*/



}

