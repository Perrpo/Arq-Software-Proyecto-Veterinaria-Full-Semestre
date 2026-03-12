using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

namespace VetConsoleApp.Application.Services;

public class ServicioCatalogo : IServicioCatalogo
{
    private readonly InMemoryData _data;

    public ServicioCatalogo(InMemoryData data)
    {
        _data = data;
    }

    public IReadOnlyCollection<Servicio> ObtenerTodos() => _data.Servicios.AsReadOnly();

    public Servicio? ObtenerPorId(int idServicio) => _data.Servicios.FirstOrDefault(s => s.IdServicio == idServicio);
}
