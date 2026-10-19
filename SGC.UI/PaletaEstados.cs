using SGC.Entidades;

namespace SGC.UI;

// Colores y textos por EstadoTurno, compartidos por la grilla de FormTurnos
// (fondo suave de fila / contadores) y la pantalla de inicio (tarjetas), para
// que un mismo estado se vea igual en todas las pantallas.
public static class PaletaEstados
{
    // Fondos suaves: legibles con texto oscuro y coherentes con la
    // disponibilidad de la agenda (verde claro / rojo claro).
    public static Color Fondo(EstadoTurno estado) => estado switch
    {
        EstadoTurno.Pendiente => Color.FromArgb(255, 244, 214),
        EstadoTurno.Confirmado => Color.FromArgb(222, 236, 252),
        EstadoTurno.Asistio => Color.FromArgb(220, 245, 225),
        EstadoTurno.Ausente => Color.FromArgb(250, 220, 220),
        EstadoTurno.Cancelado => Color.FromArgb(236, 238, 242),
        _ => Color.White
    };

    // Color fuerte del mismo estado (numeros grandes y franjas de tarjeta).
    public static Color Acento(EstadoTurno estado) => estado switch
    {
        EstadoTurno.Pendiente => Color.FromArgb(196, 130, 10),
        EstadoTurno.Confirmado => Color.FromArgb(46, 134, 222),
        EstadoTurno.Asistio => Color.FromArgb(39, 174, 96),
        EstadoTurno.Ausente => Color.FromArgb(231, 76, 60),
        EstadoTurno.Cancelado => Color.FromArgb(110, 120, 135),
        _ => Color.FromArgb(27, 42, 74)
    };

    public static string Texto(EstadoTurno estado) => estado switch
    {
        EstadoTurno.Pendiente => "Pendiente",
        EstadoTurno.Confirmado => "Confirmado",
        EstadoTurno.Asistio => "Asistió",
        EstadoTurno.Ausente => "Ausente",
        EstadoTurno.Cancelado => "Cancelado",
        _ => estado.ToString()
    };
}
