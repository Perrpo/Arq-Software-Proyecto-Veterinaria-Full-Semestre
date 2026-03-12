using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Domain.Validators;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

namespace VetConsoleApp.Application.Services;

public class VeterinarioService : IVeterinarioService
{
    private readonly InMemoryData _data;
    private readonly ICitaService _citaService;
    private readonly IVetAssignmentStrategy _strategy;

    public VeterinarioService(InMemoryData data, ICitaService citaService, IVetAssignmentStrategy strategy)
    {
        _data = data;
        _citaService = citaService;
        _strategy = strategy;
    }

    public IReadOnlyCollection<Veterinario> ObtenerTodos() => _data.Veterinarios.AsReadOnly();

    public Veterinario? ObtenerPorId(int id) => _data.Veterinarios.FirstOrDefault(v => v.IdVeterinario == id);

    public IReadOnlyCollection<Veterinario> Disponibles(DateTime inicio, TimeSpan duracion, string especialidad) =>
        _data.Veterinarios
            .Where(v => v.TieneEspecialidad(especialidad) && v.EstaDisponible(inicio, duracion))
            .ToList()
            .AsReadOnly();

    public Veterinario AsignarACita(int idCita, int? idVeterinarioPreferido = null)
    {
        var cita = _citaService.ObtenerPorId(idCita) ?? throw new InvalidOperationException($"Cita {idCita} no existe");
        var servicio = _data.Servicios.FirstOrDefault(s => s.IdServicio == cita.IdServicio)
            ?? throw new InvalidOperationException($"Servicio {cita.IdServicio} no existe");
        var especialidadRequerida = servicio.EspecialidadRequerida;

        var duracion = TimeSpan.FromMinutes(cita.DuracionMinutos);
        var candidatos = Disponibles(cita.FechaCita, duracion, especialidadRequerida);
        if (!candidatos.Any())
            throw new InvalidOperationException("No hay veterinarios disponibles con la especialidad requerida.");

        Veterinario vet;
        if (idVeterinarioPreferido.HasValue)
        {
            vet = candidatos.FirstOrDefault(v => v.IdVeterinario == idVeterinarioPreferido.Value)
                  ?? throw new InvalidOperationException("El veterinario preferido no está disponible en ese horario.");
        }
        else
        {
            vet = _strategy.Seleccionar(cita, candidatos);
        }

        vet.AsignarCita(cita.IdCita, cita.FechaCita, duracion);
        cita.AsignarVeterinario(vet.IdVeterinario);
        cita.CambiarEstado(EstadoCitaValidator.Normalizar("confirmada"));
        return vet;
    }
}
