namespace VetConsoleApp.Models;

public class Veterinario
{
    public int IdVeterinario { get; }
    public string Nombre { get; }
    public IReadOnlyCollection<string> Especialidades => _especialidades.AsReadOnly();
    public IReadOnlyCollection<AgendaSlot> Agenda => _agenda.AsReadOnly();

    private readonly List<string> _especialidades;
    private readonly List<AgendaSlot> _agenda;

    public Veterinario(int idVeterinario, string nombre, IEnumerable<string> especialidades)
    {
        IdVeterinario = idVeterinario;
        Nombre = nombre;
        _especialidades = especialidades.Select(e => e.Trim().ToLowerInvariant()).Distinct().ToList();
        _agenda = new List<AgendaSlot>();
    }

    public bool TieneEspecialidad(string especialidad) =>
        _especialidades.Contains(especialidad.Trim().ToLowerInvariant());

    public bool EstaDisponible(DateTime inicio, TimeSpan duracion) =>
        !_agenda.Any(s => s.SeSolapaCon(inicio, duracion));

    public void AsignarCita(int idCita, DateTime inicio, TimeSpan duracion)
    {
        if (!EstaDisponible(inicio, duracion))
            throw new InvalidOperationException($"Veterinario {IdVeterinario} no está disponible en ese horario.");

        _agenda.Add(new AgendaSlot(idCita, inicio, duracion));
    }
}
