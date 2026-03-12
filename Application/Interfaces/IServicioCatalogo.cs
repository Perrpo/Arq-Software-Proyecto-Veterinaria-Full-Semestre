using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Interfaces;

public interface IServicioCatalogo
{
    IReadOnlyCollection<Servicio> ObtenerTodos();
    Servicio? ObtenerPorId(int idServicio);
}
