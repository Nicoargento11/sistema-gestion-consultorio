using SGC.Entidades;

namespace SGC.Logica;

public enum EstadoExcepcion
{
    Vigente,
    Proxima,
    Pasada
}

// Fila del listado de excepciones de agenda: la excepcion mas datos
// calculados (estado respecto de hoy y turnos que todavia la contradicen).
public class ExcepcionFila
{
    public required ExcepcionAgenda Excepcion { get; init; }
    public EstadoExcepcion Estado { get; init; }

    // Turnos activos y no cancelados del medico que se superponen con la
    // excepcion. Si la excepcion ya cancelo los turnos al cargarse, aca
    // quedan solo los que siguen activos.
    public int TurnosAfectados { get; init; }

    public int Id => Excepcion.Id;
    public int MedicoId => Excepcion.MedicoId;
    public string MedicoNombre => Excepcion.MedicoNombre;
    public DateOnly Fecha => Excepcion.Fecha;
    public TipoExcepcionAgenda Tipo => Excepcion.Tipo;
    public string DescripcionTexto => Excepcion.DescripcionTexto;
    public string Motivo => Excepcion.Motivo;

    public string EstadoTexto => Estado switch
    {
        EstadoExcepcion.Vigente => "Vigente",
        EstadoExcepcion.Proxima => "Próxima",
        _ => "Pasada"
    };

    public static EstadoExcepcion CalcularEstado(DateOnly fecha, DateOnly hoy)
    {
        if (fecha == hoy) return EstadoExcepcion.Vigente;
        return fecha > hoy ? EstadoExcepcion.Proxima : EstadoExcepcion.Pasada;
    }
}
