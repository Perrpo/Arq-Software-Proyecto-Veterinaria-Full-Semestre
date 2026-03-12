namespace VetConsoleApp.Models;

public class Servicio
{
    public int IdServicio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Precio { get; set; }
    public string EspecialidadRequerida { get; set; } = "medicina_general";
}
