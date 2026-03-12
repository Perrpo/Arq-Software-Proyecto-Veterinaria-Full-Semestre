namespace VetConsoleApp.Models;

public class AgendaSlot
{
    public int IdCita { get; }
    public DateTime Inicio { get; }
    public TimeSpan Duracion { get; }
    public DateTime Fin => Inicio + Duracion;

    public AgendaSlot(int idCita, DateTime inicio, TimeSpan duracion)
    {
        IdCita = idCita;
        Inicio = inicio;
        Duracion = duracion;
    }

    public bool SeSolapaCon(DateTime inicio, TimeSpan duracion)
    {
        var nuevoFin = inicio + duracion;
        return inicio < Fin && nuevoFin > Inicio;
    }
}
