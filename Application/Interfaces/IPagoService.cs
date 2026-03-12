using VetConsoleApp.Models;

namespace VetConsoleApp.Application.Interfaces;

public interface IPagoService
{
    IReadOnlyCollection<Pago> ObtenerTodos();
    IReadOnlyCollection<Pago> ObtenerPorCita(int idCita);
    Pago RegistrarPago(int idCita, string metodoPago, decimal monto, decimal descuento = 0, DateTime? fechaPago = null);
}
