using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Services;

/// <summary>
/// Elige al veterinario con menor cantidad de slots en agenda.
/// </summary>
public class MenorCargaStrategy : IVetAssignmentStrategy
{
    public Veterinario Seleccionar(Cita cita, IReadOnlyCollection<Veterinario> candidatos)
    {
        if (candidatos.Count == 0) throw new InvalidOperationException("No hay candidatos disponibles.");
        return candidatos
            .OrderBy(v => v.Agenda.Count)
            .ThenBy(v => v.IdVeterinario)
            .First();
    }
}
