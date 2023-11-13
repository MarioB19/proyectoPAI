namespace Backend;

using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Security.Cryptography;

public static class Utilidades
{



#region Contrasena
	// Función para comparación en tiempo constante
	public static bool ComparacionEnTiempoConstante(byte[] a, byte[] b)
	{
	    if (a.Length != b.Length)
	        return false;
	
	    int result = 0;
	    for (int i = 0; i < a.Length; i++)
	    {
	        result |= a[i] ^ b[i];
	    }
	    return result == 0;
	}

	public static string EncriptarContrasenaSHA256(string contrasena)
	{
	    using (SHA256 sha256 = SHA256.Create())
	    {
	        byte[] contrasenaBytes = Encoding.UTF8.GetBytes(contrasena);
	        byte[] hashBytes = sha256.ComputeHash(contrasenaBytes);
	
	        StringBuilder sb = new StringBuilder();
	        for (int i = 0; i < hashBytes.Length; i++)
	        {
	            sb.Append(hashBytes[i].ToString("x2")); // Convierte los bytes en formato hexadecimal
	        }
	
	        return sb.ToString();
	    }
	}
		
	public static bool VerificarContrasenaSHA256(string contrasena, string hashAlmacenado)
	{
	    string hashEntrante = EncriptarContrasenaSHA256(contrasena);
	    return string.Equals(hashAlmacenado, hashEntrante, StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool VerificarContrasenaSHA256(byte[] contrasena, string hashAlmacenado)
	{
	    string contrasenaHash = Encoding.UTF8.GetString(contrasena);
	    string hashEntrante = EncriptarContrasenaSHA256(contrasenaHash);
	    return string.Equals(hashAlmacenado, hashEntrante, StringComparison.OrdinalIgnoreCase);
	}

	public static bool VerificarContrasenaSHA256(string contrasena, byte[] hashAlmacenado)
	{
	    string hashEntrante = EncriptarContrasenaSHA256(contrasena);
	    byte[] hashEntranteBytes = Encoding.UTF8.GetBytes(hashEntrante);
	    
	    return VerificarContrasenaSHA256(hashAlmacenado, hashEntranteBytes);
	}

	public static bool VerificarContrasenaSHA256(byte[] contrasena, byte[] hashAlmacenado)
	{
		return ComparacionEnTiempoConstante(contrasena, hashAlmacenado);
	}
#endregion

#region Registro/Nómina

public static bool VerificarRegistroNomina(string Registro){
        Regex regex = new Regex("^[0-9]{8}$");
        return regex.IsMatch(Registro);
}

#endregion

#region Nombre

public static bool VerificarNombre(string Nombre){
		RegexOptions options = RegexOptions.IgnoreCase;
        Regex regex = new Regex(@"^[\ áéíóúñ'a-z]+$", options);
        return regex.IsMatch(Nombre);
}
#endregion
public static bool VerificarTipoDePrestamo(string tipo_prestamo){
if(tipo_prestamo=="1"||tipo_prestamo=="2"){
	return true;
}
else {
	return false;
}

}
public static bool Verificarfecha(string tipo_prestamo){
 string formatoFechaHora = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$";

    // Verificar si la cadena cumple con el patrón de fecha y hora
    if (Regex.IsMatch(tipo_prestamo, formatoFechaHora))
    {
        // Intentar convertir la cadena a DateTime para asegurarse de que es una fecha y hora válida
        if (DateTime.TryParseExact(tipo_prestamo, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _))
        {
            return true; // La cadena tiene el formato y es una fecha y hora válida
        }
    }

    return false;

}
public static bool VerificarEstadoDePrestamo(string estado_prestamo){
if(estado_prestamo=="1"||estado_prestamo=="2"||estado_prestamo=="3"){
	return true;
}
else {
	return false;
}

}


public static string ConvertirBytesAString(byte[] arregloBytes)
{
    string resultado = System.Text.Encoding.UTF8.GetString(arregloBytes);
    return resultado;
}

}

