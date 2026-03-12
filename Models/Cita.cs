namespace VetConsoleApp.Models;

public class Cita
{
    public int IdCita { get; private set; }
    public int IdUsuario { get; private set; }
    public int IdPaciente { get; private set; }
    public int IdServicio { get; private set; }
    public DateTime FechaCita { get; private set; }
    public int DuracionMinutos { get; private set; }
    public int? IdVeterinario { get; private set; }
    public string Estado { get; private set; } = string.Empty;

    public Cita(int idCita, int idUsuario, int idPaciente, int idServicio, DateTime fechaCita, string estado, int duracionMinutos = 60, int? idVeterinario = null)
    {
        IdCita = idCita;
        IdUsuario = idUsuario;
        IdPaciente = idPaciente;
        IdServicio = idServicio;
        FechaCita = fechaCita;
        DuracionMinutos = duracionMinutos;
        IdVeterinario = idVeterinario;
        Estado = estado;
    }

    public void CambiarEstado(string nuevoEstado) => Estado = nuevoEstado;
    public void AsignarVeterinario(int idVeterinario) => IdVeterinario = idVeterinario;
}
