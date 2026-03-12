using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

namespace VetConsoleApp.Application.Services;

/// <summary>
/// Gestiona el proceso de pago asociado a una cita.
/// </summary>
public class PagoService : IPagoService
{
    private readonly InMemoryData _data;
    private readonly ICitaService _citaService;

    public PagoService(InMemoryData data, ICitaService citaService)
    {
        _data = data;
        _citaService = citaService;
    }

    public IReadOnlyCollection<Pago> ObtenerTodos() => _data.Pagos.AsReadOnly();

    public IReadOnlyCollection<Pago> ObtenerPorCita(int idCita) =>
        _data.Pagos.Where(p => p.IdCita == idCita).ToList().AsReadOnly();

    public Pago RegistrarPago(int idCita, string metodoPago, decimal monto, decimal descuento = 0, DateTime? fechaPago = null)
    {
        var cita = _citaService.ObtenerPorId(idCita) ?? throw new InvalidOperationException($"Cita {idCita} no existe");
        var pagoId = _data.Pagos.Count == 0 ? 1 : _data.Pagos.Max(p => p.IdPago) + 1;

        var pago = new Pago(pagoId, cita.IdCita, metodoPago, monto, descuento, fechaPago ?? DateTime.Now, "pagado");

        _data.Pagos.Add(pago);
        return pago;
    }
}
