using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Domain.Validators;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

namespace VetConsoleApp.Application.Services;

/// <summary>
/// Servicio de dominio para administrar el ciclo de vida de las citas.
/// </summary>
public class CitaService : ICitaService
{
    private readonly InMemoryData _data;

    public CitaService(InMemoryData data)
    {
        _data = data;
    }

    public IReadOnlyCollection<Cita> ObtenerTodas() => _data.Citas.AsReadOnly();

    public Cita? ObtenerPorId(int idCita) => _data.Citas.FirstOrDefault(c => c.IdCita == idCita);

    public Cita Crear(int idUsuario, int idPaciente, int idServicio, DateTime fecha)
    {
        if (!_data.Usuarios.Any(u => u.IdUsuario == idUsuario))
            throw new ArgumentException($"Usuario {idUsuario} no existe", nameof(idUsuario));

        if (!_data.Pacientes.Any(p => p.IdPaciente == idPaciente && p.IdUsuario == idUsuario))
            throw new ArgumentException($"Paciente {idPaciente} no pertenece al usuario {idUsuario}", nameof(idPaciente));

        if (!_data.Servicios.Any(s => s.IdServicio == idServicio))
            throw new ArgumentException($"Servicio {idServicio} no existe", nameof(idServicio));

        var nextId = _data.Citas.Count == 0 ? 1 : _data.Citas.Max(c => c.IdCita) + 1;

        var cita = new Cita(nextId, idUsuario, idPaciente, idServicio, fecha, EstadoCitaValidator.Normalizar("pendiente"));

        _data.Citas.Add(cita);
        return cita;
    }

    public void CambiarEstado(int idCita, string nuevoEstado)
    {
        if (!EstadoCitaValidator.EsValido(nuevoEstado))
            throw new ArgumentException($"Estado inválido. Usa: {string.Join(", ", EstadoCitaValidator.EstadosPermitidos())}");

        var cita = ObtenerPorId(idCita) ?? throw new InvalidOperationException($"Cita {idCita} no encontrada");
        cita.CambiarEstado(EstadoCitaValidator.Normalizar(nuevoEstado));
    }
}
