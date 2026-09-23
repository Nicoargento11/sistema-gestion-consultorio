using System.Security.Cryptography;

namespace SGC.Entidades;

// Vive en Entidades (no en Logica) porque tanto SGC.Logica (UsuarioService)
// como SGC.Datos (DbSeeder) lo necesitan, y Datos no puede referenciar a
// Logica sin generar una dependencia circular entre capas.
//
// PBKDF2 con sal aleatoria por usuario (System.Security.Cryptography, ya
// viene con el runtime, no suma dependencias). El formato guardado es
// "sal_base64.hash_base64" para no necesitar una columna aparte.
public static class PasswordHasher
{
    private const int TamanoSalBytes = 16;
    private const int TamanoHashBytes = 32;
    private const int Iteraciones = 100_000;

    public static string Hashear(string contrasenaPlana)
    {
        byte[] sal = RandomNumberGenerator.GetBytes(TamanoSalBytes);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(contrasenaPlana, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHashBytes);
        return $"{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string contrasenaPlana, string hashAlmacenado)
    {
        var partes = hashAlmacenado.Split('.');
        if (partes.Length != 2)
            return false;

        byte[] sal = Convert.FromBase64String(partes[0]);
        byte[] hashEsperado = Convert.FromBase64String(partes[1]);
        byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(contrasenaPlana, sal, Iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

        return CryptographicOperations.FixedTimeEquals(hashIngresado, hashEsperado);
    }
}
