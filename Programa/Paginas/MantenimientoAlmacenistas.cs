namespace Programa.Paginas;

using Backend;
using Entidades;

using Microsoft.Extensions.Configuration;
using System;
using System.Text;

public class MantenimientoAlmacenistas : PaginaBase
{
	// ? Bandera que determina si solo se cambia la contrasena del almacenista o todos sus atributos
	// ? True: Solo cambia la contrasena
	// ? False : Cambiar todos los atributos
	public bool _RecuperarContrasena { get; set; }

	public Almacenista AlmacenistaActual { get; set; }

	public Usuario UsuarioActual { get; set; }

	public int IdUsuarioActual { get; set; }

	public bool UsuarioNuevo { get; set; }

	public MantenimientoAlmacenistas(IConfiguration configuracion) : base("MantenimientoAlmacenistas", configuracion)
	{
		IdUsuarioActual = -1;
		UsuarioNuevo = true;
		_RecuperarContrasena = false;
		AlmacenistaActual = null;
		UsuarioActual = null;
	}

	public override void Mostrar()
	{
		UsuarioNuevo = IdUsuarioActual == -1;
		if (!UsuarioNuevo)
		{
			Consultar();
			base.IdTpoUsuarioActual = (int)UsuarioActual.IdTpoUsuario;
		}
		
		if (!base.TieneAccesoAInterfazActual)
		{
			return;
		}
	

		if (UsuarioNuevo)
		{
			Agregar();
		}
		else
		{
			if (AlmacenistaActual == null)
			{
				return;
			}

			if (_RecuperarContrasena)
			{
				RecuperarContrasena();
			}
			else
			{
				Actualizar();
			}
		}
	}


	/*
	? Metodo que te permite cambiar la contrasena del almacenista
	*/
	public void RecuperarContrasena()
	{
		bool usuarioActualizado = false;
		bool almacenistaActualizado = false;
		MostrarAlmacenista();

		Console.WriteLine("\nIngresa la nueva contraseña: ");
		string nuevaContrasena = Console.ReadLine();

		if (UsuarioActual != null)
		{
			string nuevaContrasenaEncriptada = Utilidades.EncriptarContrasenaSHA256(nuevaContrasena);

			// Actualiza la contraseña en el usuario
			UsuarioActual.Contrasena = Encoding.UTF8.GetBytes(nuevaContrasenaEncriptada);
		}

		try
		{
			// Guarda los cambios en la base de datos
			usuarioActualizado = SQLite.ActualizarUsuario(UsuarioActual);
			almacenistaActualizado = SQLite.ActualizarAlmacenista(AlmacenistaActual);// Para actualizar la fch_modificacion 
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al actualizar la contrasena del almacenista, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}


		if (!usuarioActualizado)
		{
			Console.WriteLine("Error al actualizar la contrasena del usuario. Verifica los datos y vuelve a intentarlo.");
			return;
		}

		if (!almacenistaActualizado)
		{
			Console.WriteLine("Error al actualizar la contrasena del usuario. Verifica los datos y vuelve a intentarlo.");
			return;
		}

		Console.WriteLine("Contraseña actualizada con éxito.");
		Console.WriteLine("\nPresiona cualquier tecla para continuar...");
		Console.ReadKey();
	}

	/*
	? Metodo que consulta al almacenista y su usuario relacionado
	*/
	public void Consultar()
	{
		UsuarioActual = null;
		AlmacenistaActual = null;

		// Consulta del usuario
		try
		{
			UsuarioActual = SQLite.ObtenerUsuarioPorId(IdUsuarioActual);
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

	/*
	? Metodo permite actualizar al almacenista
	*/
	public void Actualizar()
	{
		MostrarAlmacenista(); // Muestra los datos actuales

		// Solicitar los nuevos valores de usuario
		Console.Write("Ingresa el nuevo nombre del usuario: ");
		string nuevoNombre = Console.ReadLine();

		while (!Utilidades.VerificarNombre(nuevoNombre))
		{
			Console.Clear();
			Console.WriteLine("Nombre no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el nuevo nombre del usuario: ");
			nuevoNombre = Console.ReadLine();
		};

		Console.Write("Ingresa el nuevo apellido paterno del usuario: ");
		string nuevoAplPaterno = Console.ReadLine();

		while (!Utilidades.VerificarNombre(nuevoAplPaterno))
		{
			Console.Clear();
			Console.WriteLine("Apellido paterno no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el nuevo apellido paterno del usuario: ");
			nuevoAplPaterno = Console.ReadLine();
		};

		Console.Write("Ingresa el nuevo apellido materno del usuario: ");
		string nuevoAplMaterno = Console.ReadLine();

		while (!Utilidades.VerificarNombre(nuevoAplMaterno))
		{
			Console.Clear();
			Console.WriteLine("Apellido materno no válido, inténtalo de nuevo");
			Console.Write("\nIngresa el nuevo apellido materno del usuario: ");
			nuevoAplMaterno = Console.ReadLine();
		};

		try
		{
			// Solicitar los nuevos valores de almacenista
			if (AlmacenistaActual.IdUsuarioNavigation != null)
			{
				// Actualizar el usuario
				AlmacenistaActual.IdUsuarioNavigation.FchModificacion = DateTime.Now;
				AlmacenistaActual.IdUsuarioNavigation.Nombre = nuevoNombre;
				AlmacenistaActual.IdUsuarioNavigation.AplPaterno = nuevoAplPaterno;
				AlmacenistaActual.IdUsuarioNavigation.AplMaterno = nuevoAplMaterno;
			}

			// Guardar los cambios en la base de datos
			SQLite.SaveChanges();

			Console.WriteLine("Cambios guardados exitosamente.");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}
		catch (Exception e)
		{
			Console.WriteLine("Ocurrio un error al actualizar el almacenista, por favor intentalo mas tarde");
			Console.WriteLine($"Mensaje de error: {e.Message}");
			Console.WriteLine("\nPresiona cualquier tecla para continuar...");
			Console.ReadKey();
		}
	}


	/*
	? Metodo para pedir la info del almacenista, incluyendo la de su usuario relacionado
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



	/*
	? Metodo para q la info del almacenista y su usuario relacionado
	*/
	public void MostrarAlmacenista()
	{
		Console.Clear();
		Console.WriteLine("Datos del Almacenista");
		Console.WriteLine($"Nombre completo: {UsuarioActual.Nombre} {UsuarioActual.AplPaterno} {UsuarioActual.AplMaterno}");
		Console.WriteLine($"Fecha de creación: {UsuarioActual.FchCreacion}");

		if (UsuarioActual.FchModificacion != null)
		{
			Console.WriteLine($"Ultima fecha de modificación: {UsuarioActual.FchModificacion}");
		}
		else
		{
			Console.WriteLine("Ultima fecha de modificación: No disponible");
		}
	}

}
