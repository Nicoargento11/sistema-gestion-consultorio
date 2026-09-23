using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class AgendaMedicoService
{
    public List<AgendaMedico> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo)
            .Include(a => a.Medico)
            .ToList();
    }

    public List<AgendaMedico> ObtenerPorMedico(int medicoId)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == medicoId)
            .Include(a => a.Medico)
            .ToList();
    }

    public AgendaMedico? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico.Include(a => a.Medico).FirstOrDefault(a => a.Id == id);
    }

    public List<AgendaMedico> ObtenerPorMedicoYDia(int medicoId, DayOfWeek dia)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == medicoId && a.DiaSemana == dia)
            .ToList();
    }

    public bool MedicoAtiende(int medicoId, DayOfWeek dia, TimeOnly horaInicioTurno, int duracionMinutos)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;
        TimeOnly horaFinTurno = horaInicioTurno.AddMinutes(duracionMinutos);

        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico.Any(a =>
            a.Activo && a.MedicoId == medicoId && a.DiaSemana == dia &&
            horaInicioTurno >= a.HoraInicio && horaFinTurno <= a.HoraFin);
    }

    public void Agregar(AgendaMedico agenda)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = ValidarDatosBasicos(agenda, contexto);
        ValidarSuperposicion(agenda, idAExcluir: null, contexto);

        agenda.Id = 0;
        agenda.Activo = true;
        agenda.Medico = medico;
        contexto.AgendasMedico.Add(agenda);
        contexto.SaveChanges();
    }

    public void Modificar(AgendaMedico agenda)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = ValidarDatosBasicos(agenda, contexto);

        var existente = contexto.AgendasMedico.FirstOrDefault(a => a.Id == agenda.Id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta modificar no existe.");

        ValidarSuperposicion(agenda, idAExcluir: existente.Id, contexto);

        existente.MedicoId = agenda.MedicoId;
        existente.Medico = medico;
        existente.DiaSemana = agenda.DiaSemana;
        existente.HoraInicio = agenda.HoraInicio;
        existente.HoraFin = agenda.HoraFin;
        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var agenda = contexto.AgendasMedico.FirstOrDefault(a => a.Id == id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta eliminar no existe.");

        agenda.Activo = false;
        contexto.SaveChanges();
    }

    private void ValidarSuperposicion(AgendaMedico agenda, int? idAExcluir, SGCContext contexto)
    {
        var franjasDelDia = contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == agenda.MedicoId && a.DiaSemana == agenda.DiaSemana && a.Id != (idAExcluir ?? 0))
            .ToList();

        foreach (var a in franjasDelDia)
        {
            if (a.HoraInicio < agenda.HoraFin && agenda.HoraInicio < a.HoraFin)
            {
                throw new InvalidOperationException("El rango horario seleccionado se superpone con otro registro de agenda para el mismo medico y dia de semana.");
            }
        }
    }

    private Medico ValidarDatosBasicos(AgendaMedico agenda, SGCContext contexto)
    {
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == agenda.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un medico valido.");

        if (agenda.HoraFin <= agenda.HoraInicio)
            throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");

        return medico;
    }
}
