namespace Programa.Paginas;

using Backend;
using Entidades;

using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System;
using Spectre.Console;

public class Inicio : PaginaBase
{
	// Variable encargada de manejar el ciclo del sistema
	public bool Ciclo { get; set; }

	public Inicio(IConfiguration configuracion) : base("Inicio", configuracion)
    {
    	Ciclo = true;
    }

	/*
	? Metodo encargado de mostrar la pagina
	*/
    public override void Mostrar()
    {
    	MostrarOpciones();
    }

   public void MostrarOpciones()
   {
       AnsiConsole.Clear();
       var bienvenida = new FigletText("Bienvenido al sistema de administración del CETI")
           .Color(Color.Yellow)
           .Centered();
   
       AnsiConsole.Render(bienvenida);
       AnsiConsole.WriteLine();
   
       AnsiConsole.MarkupLine("[bold yellow]Por favor selecciona una opción[/]");
       var opciones = new[] { "Iniciar Sesión", "Crear Cuenta", "Recuperar Contraseña", "Salir" };
       var seleccionPrompt = new SelectionPrompt<string>()
           //.Title("Opciones")
           .AddChoices(opciones);
   
       var opcionSeleccionada = AnsiConsole.Prompt(seleccionPrompt);
   
       switch (opcionSeleccionada)
       {
           case "Iniciar Sesión":
               MostrarPantallaLogin();
               break;
           case "Crear Cuenta":
               MostrarPantallaCrearCuenta();
               break;
           case "Recuperar Contraseña":
               MostrarPantallaRecuperarContrasena();
               break;
           case "Salir":
               Ciclo = false;
               break;
           default:
               AnsiConsole.MarkupLine("[red]Opción no válida.[/]");
               break;
       }
   }
                                                                                      
    public int PedirTipoUsuario(bool usuarioExistente = false)
    {
        AnsiConsole.Clear();
        string pregunta = usuarioExistente ? "¿Qué tipo de usuario eres?" : "¿Qué tipo de usuario quieres crear?";
        var opciones = new List<string> { "Almacenista", "Estudiante", "Profesor"};
        if (usuarioExistente)
        {
            opciones.Add("Coordinador");
        }
    
        var seleccionPrompt = new SelectionPrompt<string>()
            .Title(pregunta)
            .AddChoices(opciones);
    
        var opcionSeleccionada = AnsiConsole.Prompt(seleccionPrompt);
    
        int resultado = opciones.IndexOf(opcionSeleccionada) + 1;
    
        return resultado;
    }
    
    public string PedirRegistro()
    {
    	string registro = string.Empty;
    
        Console.Clear();
        Console.WriteLine("Bienvenido al sistema de administración del CETI");
        Console.WriteLine("Por favor ingresa tus datos a continuación");
        Console.Write("\nRegistro/Nómina: ");
        registro = Console.ReadLine();
    
        while (!Utilidades.VerificarRegistroNomina(registro))
        {
            Console.Clear();
            Console.WriteLine("Registro/Nómina no válidos, inténtalo de nuevo");
            Console.Write("\nRegistro/Nómina: ");
            registro = Console.ReadLine();
        };

		return registro;
        
    }

    public int BuscarUsuario(string registro)
  	{
  	    int resultado = 0;
  	    Usuario usuarioActual = null;
  	
  	    try
  	    {
  	        usuarioActual = SQLite.ObtenerUsuarioPorRegistro(registro);
  	    }
  	    catch (Exception e)
  	    {
  	        Console.WriteLine("Ocurrió un error al consultar el usuario, por favor inténtalo más tarde");
  	        Console.WriteLine($"Mensaje de error: {e.Message}");
  	        Console.WriteLine("\nPresiona cualquier tecla para continuar");
  	        Console.ReadKey();
  	    }

  	    if(!((int)usuarioActual.IdTpoUsuario == (int)Enumeradores.tps_usuarios.Coordinador))
  	    {
  	    	resultado = (int)usuarioActual.Id;
  	    }
  	
  	    return resultado;
  	}

    public void MostrarPantallaLogin()
    {
    	Login login = new Login(base.configuracion);
       	login.Mostrar();
    }

    public void MostrarPantallaRecuperarContrasena()
	{
		bool usuarioExistente = true;
		int IdTpoUsuarioActual = PedirTipoUsuario(usuarioExistente);
		string registro = PedirRegistro();
		int IdUsuarioActual = BuscarUsuario(registro);

		if(IdUsuarioActual == 0)
		{
			return;
		}

		switch (IdTpoUsuarioActual)
		{
			case 1:
				MantenimientoAlmacenistas mntAlmacenista = new MantenimientoAlmacenistas(base.configuracion);
				mntAlmacenista.IdUsuarioActual = IdUsuarioActual;
				mntAlmacenista._RecuperarContrasena = true;
				mntAlmacenista.Mostrar();
				break;
			case 2:
				MantenimientoEstudiantes mntEstudiante = new MantenimientoEstudiantes(base.configuracion);
				mntEstudiante.IdUsuarioActual = IdUsuarioActual;
				mntEstudiante._RecuperarContrasena = true;
				mntEstudiante.Mostrar();
				break;
			case 3:
				MantenimientoProfesores mntProfesor = new MantenimientoProfesores(base.configuracion);
				mntProfesor.IdUsuarioActual = IdUsuarioActual;
				mntProfesor._RecuperarContrasena = true;
				mntProfesor.Mostrar();
				break;
			case 4:
				MantenimientoCoordinadores mntCoordinador = new MantenimientoCoordinadores(base.configuracion);
				mntCoordinador.Mostrar();
				break;
			default:
				break;
		}
	}

	public void MostrarPantallaCrearCuenta()
	{
		bool usuarioExistente = false;
		int tpoUsuario = PedirTipoUsuario(usuarioExistente);
		switch (tpoUsuario)
		{
			case 1:
				MantenimientoAlmacenistas mntAlmacenista = new MantenimientoAlmacenistas(base.configuracion);
				// mntAlmacenista.IdUsuarioActual = (int)Enumeradores.tps_usuarios.Default;
				mntAlmacenista.Mostrar();
				break;
			case 2:
				MantenimientoEstudiantes mntEstudiante = new MantenimientoEstudiantes(base.configuracion);
				// mntEstudiante.IdUsuarioActual = (int)Enumeradores.tps_usuarios.Default;
				mntEstudiante.Mostrar();
				break;
			case 3:
				MantenimientoProfesores mntProfesor = new MantenimientoProfesores(base.configuracion);
				// mntProfesor.IdUsuarioActual = (int)Enumeradores.tps_usuarios.Default;
				mntProfesor.Mostrar();
				break;
				/*
			case 4:
				MantenimientoCoordinadores mntCoordinador = new MantenimientoCoordinadores(base.configuracion);
				mntCoordinador.Mostrar();
				break;
				*/
			default:
				break;
		}
	}

}
