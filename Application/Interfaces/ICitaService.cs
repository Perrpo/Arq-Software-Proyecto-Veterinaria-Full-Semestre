using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Interfaces;

public interface ICitaService
{
    IReadOnlyCollection<Cita> ObtenerTodas();
    Cita? ObtenerPorId(int idCita);
    Cita Crear(int idUsuario, int idPaciente, int idServicio, DateTime fecha);
    void CambiarEstado(int idCita, string nuevoEstado);
}
