namespace Backend;

public static class Enumeradores
{
	public enum tps_usuarios
	{
		Default,
		Almacenista,
		Estudiante,
		Profesor,
		Coordinador
	}


	public enum est_usuarios
	{
		Activo = 1,
		Inactivo
	}

	public enum tps_mantenimiento
	{
		Preventivo = 1,
		Correctivo,
		Predictivo
	}
	public enum tpo_prm_prestamo
	{
		Generado_por_un_estudiante= 1,
		Generado_por_un_profesor
	}
	public enum est_prm_prestamos
	{
		Aprobado = 1,
		Pendiente,
		Rechazado,

		Pre_Aprobado,
		Entregado,
		No_entregado,
		En_prestamo
	}
}
