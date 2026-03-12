using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Services;

/// <summary>
/// Elige al veterinario cuya última cita termine más cerca del inicio solicitado.
/// </summary>
public class ProximidadHoraStrategy : IVetAssignmentStrategy
{
    public Veterinario Seleccionar(Cita cita, IReadOnlyCollection<Veterinario> candidatos)
    {
        if (candidatos.Count == 0) throw new InvalidOperationException("No hay candidatos disponibles.");

        DateTime inicio = cita.FechaCita;
        return candidatos
            .OrderBy(v =>
            {
                var ultimaFin = v.Agenda.Any() ? v.Agenda.Max(s => s.Fin) : DateTime.MinValue;
                return Math.Abs((ultimaFin - inicio).Ticks);
            })
            .ThenBy(v => v.IdVeterinario)
            .First();
    }
}
