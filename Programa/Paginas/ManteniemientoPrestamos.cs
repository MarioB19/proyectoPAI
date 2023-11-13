using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Spectre.Console;
using Backend;
using Entidades;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

using Programa.Paginas;



public class PrestamosEstudiantes : PaginaBase
{
    public Estudiante EstudianteActual { get; set; }
    public Usuario UsuarioActual { get; set; }



    public int IdUsuarioActual { get; set; }
    public bool UsuarioNuevo { get; set; }

    public PrestamosEstudiantes(IConfiguration configuracion) : base("MantenimientoPrestamos", configuracion)
    {
        IdUsuarioActual = -1;
        UsuarioNuevo = true;
        EstudianteActual = null;
        UsuarioActual = null;
    }


    

    //Constructor
    public void MantenimientoPrestamos()
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

        if (UsuarioActual.IdTpoUsuario != 1)
        {
            AnsiConsole.MarkupLine("[bold red]No tienes permisos para acceder a esta interfaz.[/]");
        }
        else
        {
            AgregarPrestamos();
        }
    }

    public void AgregarPrestamos()
    {
        List<Equipo> equiposPrestados = new();
        Equipo equipo = new();
        Salon salon = new();

        Profesor profesor = new();
        string[] fechaInicio = new string[3];
        string[] fechaFin = new string[3];
        DateTime startDate;
        DateTime endDate;
        int cantidad = 0;
        long idEquipo = 0;
        int repetir = 0;
        string opcionSeleccionada = string.Empty;
        long idSalon = 0;
        long idProfesor = 0;


        ListarEquipos();
        do
        {
            AnsiConsole.MarkupLine("\n\n[bold green]Por favor ingrese el ID y la cantidad de equipos[/]");
            do
            {
                Console.Write("ID: ");
                idEquipo = Convert.ToInt32(Console.ReadLine());
                //Realizar una verificación de que el ID ingresado sea válido
                equipo = SQLite.ObtenerEquipoPorId(idEquipo);

            } while (equipo is null);

            //La cantidad de equipos que se van a prestar. Esta cantidad no puede ser mayor a la cantidad disponible.
            do
            {
                Console.Write("Cantidad:");
                cantidad = Convert.ToInt32(Console.ReadLine());

            } while (cantidad > equipo.CntDisponible);

            AnsiConsole.MarkupLine("\n[bold blue]¿Desea ingresar más? [/]");
            List<string> opciones = new();
            opciones.Add("Si");
            opciones.Add("No");

            equiposPrestados.Add(equipo);

            var seleccionPrompt = new SelectionPrompt<string>()
                                        .AddChoices(opciones);

            opcionSeleccionada = AnsiConsole.Prompt(seleccionPrompt);

        } while (opcionSeleccionada == "Si");

        //Agregar el salon donde se dará el  prestamo
        AnsiConsole.MarkupLine("\n[bold green]Ahora seleccione el salón del prestamo[/]");

        AnsiConsole.WriteLine($"{"ID",-10}|{"Nombre",20}|");

        foreach (var salones in SQLite.ObtenerTodosLosSalones())
        {
            //Mostrando salones disponibles
            AnsiConsole.WriteLine($"{salones.Id,-10}|{salones.NmrSalon,20}|");
        }

        do
        {
            Console.Write("ID: ");
            idSalon = Convert.ToInt32(Console.ReadLine());

            salon = SQLite.ObtenerSalonPorId(idSalon);

        } while (salon is null);


        //Agregar porfesor      
        AnsiConsole.MarkupLine("\n[bold green]Ahora seleccione el profesor[/]");

        //Listar profesores de acuerdo a grupo que pertenece el usuario
        AnsiConsole.WriteLine($"{"Id: ",-10}|{"Profesor: ",20}|");



        long? idGrupo = SQLite.ObtenerGrupoPorIdEstudiante(EstudianteActual.Id);


        foreach (var item in SQLite.ObtenerProfesoresPorGrupo(idGrupo))
        {
            string name = SQLite.ObtenerNombreProfesorPorId(item.Id);
            AnsiConsole.WriteLine($"{item.Id,-10}|{name,20}|");
        }

        if (SQLite.ObtenerProfesoresPorGrupo(idGrupo) is null)
        {
            AnsiConsole.MarkupLine("[bold red]No hay profesores disponibles[/]");
            return;
        }

        do
        {
            Console.Write("ID: ");
            idProfesor = Convert.ToInt32(Console.ReadLine());

            profesor = SQLite.ObtenerProfesorPorId(idProfesor);

        } while (profesor is null);



        //Fecha de inicio y fin
        AnsiConsole.MarkupLine("\n\n[bold green] Ingrese la fecha de inicio[/]");
        AnsiConsole.MarkupLine("[bold blue] Año (YYYY) [/]");
        fechaInicio[0] = Console.ReadLine();
        AnsiConsole.MarkupLine("[bold blue] Mes (MM) [/]");
        fechaInicio[1] = Console.ReadLine();
        AnsiConsole.MarkupLine("[bold blue] Dia (DD)) [/]");
        fechaInicio[2] = Console.ReadLine();

        string fechaInicioString = $"{fechaInicio[0]}-{fechaInicio[1]}-{fechaInicio[2]}";


        AnsiConsole.MarkupLine("\n\n[bold green] Ingrese la fecha de fin[/]");
        AnsiConsole.MarkupLine("[bold blue] Año (YYYY) [/]");
        fechaFin[0] = Console.ReadLine();
        AnsiConsole.MarkupLine("[bold blue] Mes (MM) [/]");
        fechaFin[1] = Console.ReadLine();
        AnsiConsole.MarkupLine("[bold blue] Dia (DD)) [/]");
        fechaFin[2] = Console.ReadLine();

        string fechaFinString = $"{fechaInicio[0]}-{fechaInicio[1]}-{fechaInicio[2]}";


        if (DateTime.TryParse(fechaInicioString, out startDate) && DateTime.TryParse(fechaFinString, out endDate))
        {
            if (startDate <= endDate)
            {
                //Agregar prestamo

                Prestamo Prestamo = new();
                
                Prestamo.IdTpoPrestamo=  SQLite.ObtenerEstPrestamoPorUsuario(IdUsuarioActual);
                Prestamo.IdEstPrestamo= SQLite.ObtenerTipoPrestamoPorUsuario(IdUsuarioActual);

                Prestamo.IdUsuario = IdUsuarioActual;
                Prestamo.IdSalon = salon.Id;

                bool status = SQLite.InsertarPrestamo(Prestamo);

                if (status)
                {


                    if (equiposPrestados.Count > 0)
                    {
                        foreach (var item in equiposPrestados)
                        {
                            for (int i = 0; i < item.CntDisponible;i++)          {
                            {
        
                            PrmEquipo equipos = new();
                            equipos.IdPrestamo = SQLite.ObtenerIdUltimoPrestamo();
                            equipos.IdEquipo = item.Id;   //relaciona cada uno de los items

                            SQLite.InsertarEquipo(equipos);
                            }

                        }
                    }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Pi pi pi pi[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red] Error: No se agregó la consulta[/]");
                }


            }
            else
            {
                // La fecha de inicio es posterior a la fecha de fin. Debes manejar este error.
                AnsiConsole.MarkupLine("[bold red] Error: La fecha de inicio es posterior a la fecha de fin [/]");
            }
        }
        else
        {
            // Al menos una de las fechas no es válida. Debes manejar este error.
            AnsiConsole.MarkupLine("[bold red] Error: La fechas son invalidas[/]");
        }
        Console.ReadKey();
    }

    public void ListarEquipos()
    {
        List<Equipo> inventario = SQLite.ObtenerTodosLosEquipos();
        //Se obtiene la lista de todos los equipos existentes 

        if (inventario is not null)
        {
            //Se muestra la lista de equipos disponibles

            AnsiConsole.WriteLine($"{"ID",-10}|{"Nombre",20}|{"Descripcion",20}|{"Cantidad Disponible",20}|");
            foreach (var equipos in inventario)
            {
                AnsiConsole.WriteLine($"{equipos.Id,-10}|{equipos.Nombre,20}|{equipos.Descripcion,20}|{equipos.CntDisponible,20}|");
            }
        }
    }

    public void Consultar()
    {

        UsuarioActual = null;
        EstudianteActual = null;

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


        // Consulta del 
        try
        {
            EstudianteActual = SQLite.ObtenerEstudiantePorIdUsuario(UsuarioActual.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrio un error al consultar el almacenista, por favor intentalo mas tarde");
            Console.WriteLine($"Mensaje de error: {e.Message}");
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}