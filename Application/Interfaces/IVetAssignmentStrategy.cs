using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Interfaces;

public interface IVetAssignmentStrategy
{
    Veterinario Seleccionar(Cita cita, IReadOnlyCollection<Veterinario> candidatos);
}
