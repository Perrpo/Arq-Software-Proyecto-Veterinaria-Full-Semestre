using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Interfaces;

public interface IVeterinarioService
{
    IReadOnlyCollection<Veterinario> ObtenerTodos();
    Veterinario? ObtenerPorId(int id);
    Veterinario AsignarACita(int idCita, int? idVeterinarioPreferido = null);
    IReadOnlyCollection<Veterinario> Disponibles(DateTime inicio, TimeSpan duracion, string especialidad);
}
