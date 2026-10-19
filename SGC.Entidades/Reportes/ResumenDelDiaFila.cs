namespace SGC.Entidades.Reportes;

// La fila unica de sp_ResumenDelDia: contadores del dia por estado, de la
// semana (lunes a domingo) y del mes de la fecha consultada.
public class ResumenDelDiaFila
{
    public int Pendientes { get; set; }
    public int Confirmados { get; set; }
    public int Asistieron { get; set; }
    public int Ausentes { get; set; }
    public int Cancelados { get; set; }
    public int TurnosSemana { get; set; }
    public int AusentesMes { get; set; }
    public decimal RecaudacionMes { get; set; }
    public int MedicosConAgendaHoy { get; set; }

    public int TotalDia => Pendientes + Confirmados + Asistieron + Ausentes + Cancelados;

    // Suma otro resumen (se usa para unir los resultados de varios medicos,
    // porque el procedimiento recibe un solo MedicoId).
    public void Sumar(ResumenDelDiaFila otro)
    {
        Pendientes += otro.Pendientes;
        Confirmados += otro.Confirmados;
        Asistieron += otro.Asistieron;
        Ausentes += otro.Ausentes;
        Cancelados += otro.Cancelados;
        TurnosSemana += otro.TurnosSemana;
        AusentesMes += otro.AusentesMes;
        RecaudacionMes += otro.RecaudacionMes;
        MedicosConAgendaHoy += otro.MedicosConAgendaHoy;
    }
}
