namespace Backend;

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Entidades;
using System.Data.Common;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

public partial class SQLite : ContextoBD
{

	public string ConnectionString { get; set; }

	public string Excepcion { get; set; }

	public SQLite(DbContextOptions<ContextoBD> options) : base(options) { }

	public SQLite(string connectionString) : base(connectionString) { }

	#region Interfaces

	public List<Interfaz> ObtenerTodasLasInterfaces(string sortBy = "")
	{
		List<Interfaz> resultado = new List<Interfaz>();
		Excepcion = string.Empty;

		try
		{
			resultado = Interfaces.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Interfaz>();
		}

		return resultado;
	}

	public List<Interfaz> ObtenerInterfacesPorTpoUsuario(int TpoUsuario)
	{
		List<Interfaz> resultado = new List<Interfaz>();
		Excepcion = string.Empty;


		try
		{

			resultado = AccInterfaces
				.Where(ai => ai.IdTpoUsuarioNavigation.Id == TpoUsuario)
				.Select(ai => ai.IdInterfazNavigation)
				.ToList();

			/*
						foreach(Interfaz interfaz in resultado)
						{
							Console.WriteLine(interfaz.Nombre);
						}
			*/




		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	public bool TieneAccesoAInterfaz(int TpoUsuario, string NombrePagina)
	{

		bool output = false;
		Excepcion = string.Empty;
		List<Interfaz> resultado = new List<Interfaz>();

		try
		{

			resultado = ObtenerInterfacesPorTpoUsuario(TpoUsuario);

			if (resultado.Any(i => i.Nombre == NombrePagina))
			{
				output = true;
			}
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return output;
	}


	#endregion

	#region Catalogos

	public List<T> ListaCatalogo<T>() where T : class
	{
		try
		{
			return Set<T>().ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			return new List<T>();
		}
	}


	#endregion

	#region General

	public Usuario ObtenerUsuarioPorRegistro(string registro)
	{
		Usuario resultado = null;
		Excepcion = string.Empty;
		int idUsuario = 0;

		try
		{
			idUsuario = base.ObtenerIdUsuarioPorRegistro(registro);
			if (idUsuario != 0)
			{

				resultado = ObtenerUsuarioPorId(idUsuario);



			}
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	#endregion

	#region Usuarios

	public bool InsertarUsuario(Usuario usuario)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			usuario.FchCreacion = DateTime.Now;
			Usuarios.Add(usuario);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	public bool ActualizarUsuario(Usuario usuario)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			usuario.FchModificacion = DateTime.Now;
			Usuarios.Update(usuario);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarUsuario(Usuario usuario)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Usuarios.Remove(usuario);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public Usuario ObtenerUsuarioPorId(long id)
	{
		Usuario resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Usuarios.FirstOrDefault(u => u.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	public Usuario ObtenerUsuarioPorContrasena(byte[]? contrasena)
	{
		Usuario resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Usuarios.FirstOrDefault(u => u.Contrasena.SequenceEqual(contrasena));
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	// TODO: Implementar logica de ordenamiento
	public List<Usuario> ObtenerTodosLosUsuarios(string sortBy = "")
	{
		List<Usuario> resultado = new List<Usuario>();
		Excepcion = string.Empty;

		try
		{
			resultado = Usuarios.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Usuario>();
		}

		return resultado;
	}



	//función que determina si el tipo de usuario es profesor o estudiante
	/*
		public bool VerificarTpoUsuarioProfesor(long id)
		{
			bool resultado = false;
			Excepcion = string.Empty;

			try{
				
				Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == id);
				if(usuario != null)
				{
					Profesor profesor = Profesores.FirstOrDefault(p => p.IdUsuario == usuario.Id);
					if(profesor != null)
					{
						resultado = true;
					}
				}
			}
			catch(Exception e)
			{
				Excepcion = e.ToString();


			}
			


						
		}
	
	*/


	#endregion

	#region Estudiantes

	public bool InsertarEstudiante(Estudiante estudiante)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			estudiante.FchCreacion = DateTime.Now;

			Estudiantes.Add(estudiante);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	public bool ActualizarEstudiante(Estudiante estudiante)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			estudiante.FchModificacion = DateTime.Now;
			Estudiantes.Update(estudiante);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarEstudiante(Estudiante estudiante)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Estudiantes.Remove(estudiante);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public Estudiante ObtenerEstudiantePorId(long id)
	{
		Estudiante resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Estudiantes.FirstOrDefault(e => e.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public List<Estudiante> ObtenerTodosLosEstudiantes()
	{
		List<Estudiante> resultado = new List<Estudiante>();
		Excepcion = string.Empty;

		try
		{
			resultado = Estudiantes.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Estudiante>();
		}

		return resultado;
	}

	public Estudiante ObtenerEstudiantePorIdUsuario(long idUsuario)
	{
		Estudiante estudiante = null;
		try
		{
			estudiante = Estudiantes.FirstOrDefault(e => e.IdUsuario == idUsuario);
		}
		catch (Exception e)
		{
			// TODO: Manejar la excepcion
		}
		return estudiante;
	}

	#endregion
	#region Prestamos

	public bool InsertarPrestamo(Prestamo _Prestamo)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			_Prestamo.FchCreacion = DateTime.Now;
			_Prestamo.FchInicio = DateTime.Now;
			Prestamos.Add(_Prestamo);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;

	}


	public bool InsertarEquipo(PrmEquipo _EquiposPrm)
	{

		bool resultado = false;
		Excepcion = string.Empty;

		try
		{


			EntityEntry<PrmEquipo> entity = PrmEquipos.Add(_EquiposPrm);

			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;

	}



	public Prestamo ObtenerPrestamoPorId(long id)
	{
		Prestamo resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Prestamos.FirstOrDefault(a => a.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}


	public long ObtenerIdUltimoPrestamo()
	{

		Prestamo resultado = null;

		try
		{
			resultado = Prestamos.OrderByDescending(a => a.Id).FirstOrDefault();

		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado.Id;

	}


	public Prestamo ObtenerPrestamoPorEstadoDePrestamo(int id)
	{
		Prestamo resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Prestamos.FirstOrDefault(a => a.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool ActualizarPrestamo(Prestamo _Prestamo)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			_Prestamo.FchModificacion = DateTime.Now;
			Prestamos.Update(_Prestamo);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarPrestamo(Prestamo _Prestamo)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Prestamos.Remove(_Prestamo);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public List<Prestamo> ObtenerTodosLosPrestamos()
	{
		List<Prestamo> resultado = new List<Prestamo>();
		Excepcion = string.Empty;

		try
		{
			resultado = Prestamos.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Prestamo>();
		}

		return resultado;
	}

	public Prestamo ObtenerPrestamoPorIdUsuario(long idUsuario)
	{
		Prestamo _Prestamo = null;
		try
		{
			_Prestamo = Prestamos.FirstOrDefault(a => a.IdUsuario == idUsuario);
		}
		catch (Exception e)
		{
			// TODO: Manejar la excepcion
		}
		return _Prestamo;
	}

	#endregion

	#region Almacenistas

	public bool InsertarAlmacenista(Almacenista almacenista)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			almacenista.FchCreacion = DateTime.Now;
			Almacenistas.Add(almacenista);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}

	public Almacenista ObtenerAlmacenistaPorId(long id)
	{
		Almacenista resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Almacenistas.FirstOrDefault(a => a.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool ActualizarAlmacenista(Almacenista almacenista)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			almacenista.FchModificacion = DateTime.Now;
			Almacenistas.Update(almacenista);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarAlmacenista(Almacenista almacenista)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Almacenistas.Remove(almacenista);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public List<Almacenista> ObtenerTodosLosAlmacenistas()
	{
		List<Almacenista> resultado = new List<Almacenista>();
		Excepcion = string.Empty;

		try
		{
			resultado = Almacenistas.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Almacenista>();
		}

		return resultado;
	}

	public Almacenista ObtenerAlmacenistaPorIdUsuario(long idUsuario)
	{
		Almacenista almacenista = null;
		try
		{
			almacenista = Almacenistas.FirstOrDefault(a => a.IdUsuario == idUsuario);
		}
		catch (Exception e)
		{
			// TODO: Manejar la excepcion
		}
		return almacenista;
	}

	#endregion

	#region Profesores

	public bool InsertarProfesor(Profesor profesor)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			profesor.FchCreacion = DateTime.Now;
			Profesores.Add(profesor);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}



	public bool ActualizarProfesor(Profesor profesor)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			profesor.FchModificacion = DateTime.Now;
			Profesores.Update(profesor);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarProfesor(Profesor profesor)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Profesores.Remove(profesor);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public Profesor ObtenerProfesorPorId(long id)
	{
		Profesor resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Profesores.FirstOrDefault(p => p.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public List<Profesor> ObtenerTodosLosProfesores()
	{
		List<Profesor> resultado = new List<Profesor>();
		Excepcion = string.Empty;

		try
		{
			resultado = Profesores.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine(Excepcion);
			resultado = new List<Profesor>();
		}

		return resultado;
	}

	public Profesor ObtenerProfesorPorIdUsuario(long idUsuario)
	{
		Profesor profesor = null;
		try
		{
			profesor = Profesores.FirstOrDefault(p => p.IdUsuario == idUsuario);
		}
		catch (Exception e)
		{
			// TODO: Manejar la excepcion
		}
		return profesor;
	}

	#endregion

	#region  Coordinadores

	public bool InsertarCoordinador(Coordinador coordinador)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			coordinador.FchCreacion = DateTime.Now;
			Coordinadores.Add(coordinador);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool ActualizarCoordinador(Coordinador coordinador)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			coordinador.FchModificacion = DateTime.Now;
			Coordinadores.Update(coordinador);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public bool EliminarCoordinador(Coordinador coordinador)
	{
		bool resultado = false;
		Excepcion = string.Empty;

		try
		{
			Coordinadores.Remove(coordinador);
			SaveChanges();
			resultado = true;
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public Coordinador ObtenerCoordinadorPorId(long id)
	{
		Coordinador resultado = null;
		Excepcion = string.Empty;

		try
		{
			resultado = Coordinadores.FirstOrDefault(c => c.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}

	public List<Coordinador> ObtenerTodosLosCoordinadores()
	{
		List<Coordinador> resultado = new List<Coordinador>();
		Excepcion = string.Empty;

		try
		{
			resultado = Coordinadores.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = new List<Coordinador>();
		}

		return resultado;
	}

	public Coordinador ObtenerCoordinadorPorIdUsuario(long idUsuario)
	{
		Coordinador coordinador = null;
		try
		{

			coordinador = Coordinadores.FirstOrDefault(c => c.IdUsuario == idUsuario);
		}
		catch (Exception e)
		{
			// TODO: Manejar la excepcion
		}
		return coordinador;
	}

	#endregion


	#region equipos

	public List<Equipo> ObtenerTodosLosEquipos()
	{
		List<Equipo> resultado = new();
		Excepcion = string.Empty;

		try
		{
			resultado = Equipos.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			resultado = null;
		}

		return resultado;
	}


	public Equipo ObtenerEquipoPorId(long id)
	{

		Equipo equipo = null;
		Excepcion = string.Empty;

		try
		{

			equipo = Equipos.FirstOrDefault(e => e.Id == id);
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();

		}

		return equipo;


	}

	#endregion


	#region salones

	public List<Salon> ObtenerTodosLosSalones()
	{
		List<Salon> resultado = new();
		Excepcion = string.Empty;

		try
		{
			resultado = Salones.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();

		}

		return resultado;
	}


	public Salon ObtenerSalonPorId(long id)
	{

		Salon salones = null;
		Excepcion = string.Empty;

		try
		{

			salones = Salones.FirstOrDefault(e => e.Id == id);
		}

		catch (Exception e)
		{
			Excepcion = e.ToString();

		}

		return salones;


	}


	#endregion

	#region Grupos


	public List<Grupo> ObtenerTodosLosGrupos()
	{
		List<Grupo> resultado = new();
		Excepcion = string.Empty;

		try
		{
			resultado = Grupos.ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
			Console.WriteLine("Error al guardar cambios en la base de datos: " + e.Message);
			Console.WriteLine("Stack trace: " + e.StackTrace);
			Console.WriteLine("Excepción datos: " + e.Data);
			if (e.InnerException != null)
			{
				Console.WriteLine("Excepción interna: " + e.InnerException.Message);
				Console.WriteLine("Excepción interna stack trace: " + e.InnerException.StackTrace);
			}
		}

		return resultado;
	}


	public long? ObtenerGrupoPorIdEstudiante(long id)
	{
		long? resultado = 0;

		Excepcion = string.Empty;

		try
		{
			var estudiante = Estudiantes.FirstOrDefault(e => e.Id == id);
			if (estudiante != null)
			{
				resultado = estudiante.IdGrupo;
			}
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}


	#endregion

	#region Profesor


	public List<Profesor> ObtenerProfesoresPorGrupo(long? idGrupo)
	{
		List<Profesor> resultado = new();
		Excepcion = string.Empty;

		try
		{
			var idsProfesores = Ensenas.Where(e => e.IdGrupo == idGrupo).Select(e => e.IdProfesor).ToList();
			resultado = Profesores.Where(p => idsProfesores.Contains(p.Id)).ToList();
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}




	public string ObtenerNombreProfesorPorId(long? id)
	{
		string resultado = string.Empty;

		Excepcion = string.Empty;

		try
		{
			var profesor = Profesores.FirstOrDefault(p => p.Id == id);
			if (profesor != null)
			{
				var usuario = Usuarios.FirstOrDefault(u => u.Id == profesor.IdUsuario);
				if (usuario != null)
				{
					resultado = usuario.Nombre;
				}
				else
				{
					resultado = "Usuario no encontrado";
				}
			}
			else
			{
				resultado = "Profesor no encontrado";
			}
		}
		catch (Exception e)
		{
			Excepcion = e.ToString();
		}

		return resultado;
	}


	public long ObtenerEstPrestamoPorUsuario(long? IdUsuarioActual)
	{



		Profesor profesor = Profesores.FirstOrDefault(p => p.IdUsuario == IdUsuarioActual);

		if (profesor != null)
		{
			return (long)Enumeradores.est_prm_prestamos.Pendiente;
		}

		Estudiante estudiante = Estudiantes.FirstOrDefault(a => a.IdUsuario == IdUsuarioActual);
		if (estudiante != null)
		{
			return (long)Enumeradores.est_prm_prestamos.Pre_Aprobado;
		}

		return 0;

	}


	public long ObtenerTipoPrestamoPorUsuario(long? IdUsuarioActual)
	{


		Profesor profesor = Profesores.FirstOrDefault(p => p.IdUsuario == IdUsuarioActual);

		if (profesor != null)
		{
			return (long)Enumeradores.tpo_prm_prestamo.Generado_por_un_profesor;
		}

		Estudiante estudiante = Estudiantes.FirstOrDefault(a => a.IdUsuario == IdUsuarioActual);

		if (estudiante != null)
		{
			return (long)Enumeradores.tpo_prm_prestamo.Generado_por_un_estudiante;
		}

		return 0;

	}













	#endregion
}
