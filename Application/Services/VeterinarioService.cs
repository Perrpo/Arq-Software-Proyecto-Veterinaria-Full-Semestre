using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Domain.Validators;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

namespace VetConsoleApp.Application.Services;

public class VeterinarioService : IVeterinarioService
{
    private readonly InMemoryData _data;
    private readonly ICitaService _citaService;

    public VeterinarioService(InMemoryData data, ICitaService citaService)
    {
        _data = data;
        _citaService = citaService;
    }

    public IReadOnlyCollection<Veterinario> ObtenerTodos() => _data.Veterinarios.AsReadOnly();

    public Veterinario? ObtenerPorId(int id) => _data.Veterinarios.FirstOrDefault(v => v.IdVeterinario == id);

    public IReadOnlyCollection<Veterinario> Disponibles(DateTime inicio, TimeSpan duracion, string especialidad) =>
        _data.Veterinarios
            .Where(v => v.TieneEspecialidad(especialidad) && v.EstaDisponible(inicio, duracion))
            .ToList()
            .AsReadOnly();

    public Veterinario AsignarACita(int idCita, int idVeterinario)
    {
        var cita = _citaService.ObtenerPorId(idCita) ?? throw new InvalidOperationException($"Cita {idCita} no existe");
        var servicio = _data.Servicios.FirstOrDefault(s => s.IdServicio == cita.IdServicio)
            ?? throw new InvalidOperationException($"Servicio {cita.IdServicio} no existe");
        var especialidadRequerida = servicio.EspecialidadRequerida;

        var vet = ObtenerPorId(idVeterinario) ?? throw new InvalidOperationException($"Veterinario {idVeterinario} no existe");
        if (!vet.TieneEspecialidad(especialidadRequerida))
            throw new InvalidOperationException($"El veterinario {vet.Nombre} no cubre la especialidad requerida ({especialidadRequerida}).");

        var duracion = TimeSpan.FromMinutes(cita.DuracionMinutos);
        if (!vet.EstaDisponible(cita.FechaCita, duracion))
            throw new InvalidOperationException($"Veterinario {vet.Nombre} ya tiene una cita en ese horario.");

        vet.AsignarCita(cita.IdCita, cita.FechaCita, duracion);
        cita.AsignarVeterinario(vet.IdVeterinario);
        cita.CambiarEstado(EstadoCitaValidator.Normalizar("confirmada"));
        return vet;
    }
}
