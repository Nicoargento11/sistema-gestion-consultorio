namespace SGC.Entidades.Reportes;

// Fila minima del resultado de sp_ObtenerTurnosPorMedicoYFecha: solo interesa el
// Id (el procedimiento devuelve todas las columnas de Turnos, EF ignora el resto).
public class TurnoIdFila
{
    public int Id { get; set; }
}
