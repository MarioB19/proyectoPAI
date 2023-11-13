namespace Programa.Paginas;

using Backend;
using Entidades;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;


public class MantenimientoEstudiantes : PaginaBase
{
    // Bandera que determina si solo se cambia la contrasena del estudiante o todos sus atributos
    // - True: Solo cambia la contrasena
    // - False: Cambiar todos los atributos
    public bool _RecuperarContrasena { get; set; }

    public Estudiante EstudianteActual { get; set; }
    public Usuario UsuarioActual { get; set; }

    public int IdUsuarioActual { get; set; }
    public bool UsuarioNuevo { get; set; }

    public MantenimientoEstudiantes(IConfiguration configuracion) : base("MantenimientoEstudiantes", configuracion)
    {
        IdUsuarioActual = -1;
        UsuarioNuevo = true;
        _RecuperarContrasena = false;
        EstudianteActual = null;
        UsuarioActual = null;
    }

    public override void Mostrar()
    {

        Console.WriteLine("Pagina de estudiantes");
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
            // Consultar();
            if (EstudianteActual == null)
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
        Console.ReadKey();

    }

    // Metodo que te permite cambiar la contrasena del estudiante
    public void RecuperarContrasena()
    {
        Console.Clear();
        MostrarEstudiante();

        Console.WriteLine("\nIngresa la nueva contrasena: ");
        string nuevaContrasena = Console.ReadLine();

        if (UsuarioActual != null)
        {
            string nuevaContrasenaEncriptada = Utilidades.EncriptarContrasenaSHA256(nuevaContrasena);
            UsuarioActual.Contrasena = Encoding.UTF8.GetBytes(nuevaContrasenaEncriptada);
        }

        bool usuarioActualizado = SQLite.ActualizarUsuario(UsuarioActual);
        bool estudianteActualizado = SQLite.ActualizarEstudiante(EstudianteActual);

        if (!usuarioActualizado || !estudianteActualizado)
        {

            Console.WriteLine("Error al actualizar la contrasena del estudiante. Verifica los datos y vuelve a intentarlo.");
            return;
        }

        Console.WriteLine("Contrasena actualizada con exito.");
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Metodo que consulta al estudiante y su usuario relacionado
    public void Consultar()
    {
        UsuarioActual = null;
        EstudianteActual = null;

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

        try
        {
            EstudianteActual = SQLite.ObtenerEstudiantePorIdUsuario(UsuarioActual.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrio un error al consultar el estudiante, por favor intentalo mas tarde");
            Console.WriteLine($"Mensaje de error: {e.Message}");
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    // Método para agregar un nuevo estudiante
    public void Agregar()
    {
        bool usuarioAgregado = false;
        bool estudianteAgregado = false;

        Console.Clear();
        Console.WriteLine("Agregando un nuevo Estudiante");

        // Solicitar los datos      
        Console.Write("Ingresa el nombre del usuario: ");
        string nuevoNombre = Console.ReadLine();

        while (!Utilidades.VerificarNombre(nuevoNombre))
        {
            Console.Clear();
            Console.WriteLine("Nombre no válido, inténtalo de nuevo");
            Console.Write("\nIngresa el nombre del usuario: ");
            nuevoNombre = Console.ReadLine();
        };

        Console.Write("Ingresa el apellido paterno del usuario: ");
        string nuevoAplPaterno = Console.ReadLine();

        while (!Utilidades.VerificarNombre(nuevoAplPaterno))
        {
            Console.Clear();
            Console.WriteLine("Apellido paterno no válido, inténtalo de nuevo");
            Console.Write("\nIngresa el apellido paterno del usuario: ");
            nuevoAplPaterno = Console.ReadLine();
        };

        Console.Write("Ingresa el apellido materno del usuario: ");
        string nuevoAplMaterno = Console.ReadLine();

        while (!Utilidades.VerificarNombre(nuevoAplMaterno))
        {
            Console.Clear();
            Console.WriteLine("Apellido materno no válido, inténtalo de nuevo");
            Console.Write("\nIngresa el apellido materno del usuario: ");
            nuevoAplMaterno = Console.ReadLine();
        };

        Console.Write("Ingresa el registro del estudiante: ");
        string nuevoRegistro = Console.ReadLine();

        while (!Utilidades.VerificarRegistroNomina(nuevoRegistro))
        {
            Console.Clear();
            Console.WriteLine("Registro no válido, inténtalo de nuevo");
            Console.Write("\nIngresa el registro del estudiante: ");
            nuevoRegistro = Console.ReadLine();
        };

        Console.Write("Ingresa la contraseña del usuario: ");
        string contrasena = Console.ReadLine();
        string contrasenaEncriptada = Utilidades.EncriptarContrasenaSHA256(contrasena);

        Console.WriteLine("Grupos disponibles: ");

        List<Grupo> grupos = SQLite.ObtenerTodosLosGrupos();

        int idGrupoSeleccionado = 0;
        bool entradaValida = false;


        do
        {
            foreach (Grupo grupo in grupos)
            {
                Console.WriteLine($"ID: {grupo.Id} - Nombre: {grupo.Nombre}");
            }

            Console.WriteLine("\nIngresa el ID del grupo al que pertenecs: ");

            try
            {
                idGrupoSeleccionado = Convert.ToInt32(Console.ReadLine());
                entradaValida = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Por favor, ingresa un número válido.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("El número ingresado es demasiado grande.");
            }

        } while (!entradaValida || !grupos.Any(grupo => grupo.Id == idGrupoSeleccionado));






        try
        {
            // Crear una nueva instancia de Usuario
            UsuarioActual = new Usuario
            {
                IdTpoUsuario = (long)Enumeradores.tps_usuarios.Estudiante, // Estudiante
                IdEstUsuario = (long)Enumeradores.est_usuarios.Activo, // Activo
                Nombre = nuevoNombre,
                AplPaterno = nuevoAplPaterno,
                AplMaterno = nuevoAplMaterno,
                Contrasena = Encoding.UTF8.GetBytes(contrasenaEncriptada),
                FchCreacion = DateTime.Now
            };

            usuarioAgregado = SQLite.InsertarUsuario(UsuarioActual);

            if (usuarioAgregado)
            {
                // Crear una nueva instancia de Estudiante
                EstudianteActual = new Estudiante
                {
                    IdUsuario = UsuarioActual.Id,
                    Registro = long.Parse(nuevoRegistro),
                    IdGrupo = idGrupoSeleccionado, //Prueba
                    FchCreacion = DateTime.Now

                };


                // Guardar el nuevo usuario y estudiante en la base de datos
                estudianteAgregado = SQLite.InsertarEstudiante(EstudianteActual);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error al agregar el nuevo estudiante, por favor inténtalo más tarde");
            Console.WriteLine($"Mensaje de error: {e.Message}");
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        if (!usuarioAgregado)
        {
            Console.WriteLine("Error al agregar el usuario. Verifica los datos y vuelve a intentarlo.");
            return;
        }

        if (!estudianteAgregado)
        {
            Console.WriteLine("Error al agregar el estudiante. Verifica los datos y vuelve a intentarlo.");
            return;
        }

        Console.WriteLine("Estudiante agregado exitosamente.");
        Console.WriteLine("Presiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Método para actualizar un estudiante
    public void Actualizar()
    {
        MostrarEstudiante(); // Muestra los datos actuales

        // Solicitar los nuevos valores del estudiante

        Console.Write("Ingresa el nuevo registro del estudiante: ");
        string nuevoRegistro = Console.ReadLine();

        while (!Utilidades.VerificarRegistroNomina(nuevoRegistro))
        {
            Console.Clear();
            Console.WriteLine("Registro no válido, inténtalo de nuevo");
            Console.Write("\nIngresa el nuevo registro del estudiante: ");
            nuevoRegistro = Console.ReadLine();
        };

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
            Console.Write("\nIngresa nuevo el apellido materno del usuario: ");
            nuevoAplMaterno = Console.ReadLine();
        };


        try
        {
            // Actualizar el estudiante
            EstudianteActual.Registro = long.Parse(nuevoRegistro);
            UsuarioActual.Nombre = nuevoNombre;
            UsuarioActual.AplPaterno = nuevoAplPaterno;
            UsuarioActual.AplMaterno = nuevoAplMaterno;
            UsuarioActual.FchModificacion = DateTime.Now;

            // Guardar los cambios en la base de datos
            SQLite.SaveChanges();

            Console.WriteLine("Cambios guardados exitosamente.");
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error al actualizar el estudiante, por favor inténtalo más tarde");
            Console.WriteLine($"Mensaje de error: {e.Message}");
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }

    // Método para mostrar la información del estudiante y su usuario relacionado
    public void MostrarEstudiante()
    {
        Console.Clear();
        Console.WriteLine("Datos del Estudiante");
        Console.WriteLine($"Nombre completo: {UsuarioActual.Nombre} {UsuarioActual.AplPaterno} {UsuarioActual.AplMaterno}");
        Console.WriteLine($"Registro: {EstudianteActual.Registro}");
        Console.WriteLine($"Fecha de creación: {EstudianteActual.FchCreacion}");

        if (EstudianteActual.FchModificacion != null)
        {
            Console.WriteLine($"Ultima fecha de modificación: {EstudianteActual.FchModificacion}");
        }
        else
        {
            Console.WriteLine("Ultima fecha de modificación: No disponible");
        }
    }

}
