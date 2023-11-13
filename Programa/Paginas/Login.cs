namespace Programa.Paginas;

using Backend;
using Entidades;

using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;

public class Login : PaginaBase
{

	public int IdUsuarioActual { get; set; }

	public int IdTpoUsuarioActual { get; set; }
	
    public Login(IConfiguration configuracion) : base("Login", configuracion)
    {
        IdUsuarioActual = 0;
    }

	/*
	? Metodo encargado de mostrar la pagina
	*/
    public override void Mostrar()
    {
        MostrarLogin();
    }

	/* 
	? Metodo encargado de pedir al usuario las credenciales para el login (Registro/Nomina y Contrasena)
	? Realiza las validaciones de los campos
	*/
    public void MostrarLogin()
    {
    	string registro = string.Empty;
        string contrasena = string.Empty;
    
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
    
        Console.Write("Contraseña: ");
        contrasena = Console.ReadLine();
    
        // En caso de encontrar un usuario se le manda a la pantalla 'Default'
        if (BuscarUsuario(registro, contrasena))
        {
            MostrarPantallaDefault();
        }
    }

    /*
	? Metodo para buscar el usuario, primero por el registro y luego por la contrasena
	? En caso de que no se encuentre ningun usuario u ocurra algun error devuelve: false
	? De lo contrario devuelve: true
    */
	public bool BuscarUsuario(string registro, string contrasena)
	{
	    bool resultado = false;
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
	
	    if (usuarioActual == null)
	    {
	        MostrarLogin();
	    }

		/*

		Console.WriteLine(usuarioActual.Nombre);
		Console.WriteLine(contrasena);
	
		Console.WriteLine(Utilidades.EncriptarContrasenaSHA256(contrasena));

		Console.WriteLine($"Contra convert {Utilidades.ConvertirBytesAString(usuarioActual.Contrasena)}");

		Console.WriteLine(Utilidades.EncriptarContrasenaSHA256(contrasena) == Utilidades.ConvertirBytesAString(usuarioActual.Contrasena));
		
*/
		

		if(Utilidades.EncriptarContrasenaSHA256(contrasena) == Utilidades.ConvertirBytesAString(usuarioActual.Contrasena))
		{
			
			IdUsuarioActual = (int)usuarioActual.Id;
			IdTpoUsuarioActual = (int)usuarioActual.IdTpoUsuario;
		    resultado = true;
		}

	
	    return resultado;
	}

	// Pasar a la siguiente pagina
	public void MostrarPantallaDefault()
	{

		
	    Default PaginaDefault = new Default(base.configuracion);
	    PaginaDefault.IdUsuarioActual = IdUsuarioActual;
	    PaginaDefault.IdTpoUsuarioActual = IdTpoUsuarioActual;
	    PaginaDefault.Mostrar();
	}
    
}
