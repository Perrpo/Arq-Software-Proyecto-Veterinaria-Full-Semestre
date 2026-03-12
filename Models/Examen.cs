namespace VetConsoleApp.Models;

public class Examen
{
    public int IdExamen { get; set; }
    public int IdPaciente { get; set; }
    public string TipoExamen { get; set; } = string.Empty;
    public DateTime FechaExamen { get; set; }
    public string? Resultado { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = string.Empty;
}
