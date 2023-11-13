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

		Console.WriteLine(IdUsuarioActual);
		Console.ReadKey();
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
		Console.WriteLine("Agregando un nuevo Mantenimiento");
		try
		{
			Console.WriteLine($"Ingresa la descripcion del mantenimiento: ");
			
		}
		catch(Exception e)
		{

		}

		Console.WriteLine("\nIngresa la descripción del mantenimiento: ");
		MantenimientoActual.Descripciones = Console.ReadLine();



		Console.WriteLine("Mantenimiento agregado exitosamente.");
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

