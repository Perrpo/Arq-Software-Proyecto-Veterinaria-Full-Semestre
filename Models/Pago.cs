namespace VetConsoleApp.Models;

public class Pago
{
    public int IdPago { get; private set; }
    public int IdCita { get; private set; }
    public string MetodoPago { get; private set; }
    public decimal Monto { get; private set; }
    public decimal Descuento { get; private set; }
    public decimal MontoNeto => Monto - Descuento;
    public DateTime FechaPago { get; private set; }
    public string Estado { get; private set; } = string.Empty;

    public Pago(int idPago, int idCita, string metodoPago, decimal monto, decimal descuento, DateTime fechaPago, string estado)
    {
        IdPago = idPago;
        IdCita = idCita;
        MetodoPago = metodoPago;
        Monto = monto;
        Descuento = descuento;
        FechaPago = fechaPago;
        Estado = estado;
    }

    public void AplicarDescuento(decimal descuento) => Descuento = descuento;
    public void CambiarEstado(string estado) => Estado = estado;
}
