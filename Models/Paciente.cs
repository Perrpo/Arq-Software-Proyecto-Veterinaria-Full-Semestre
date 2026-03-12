namespace VetConsoleApp.Models;

public class Paciente
{
    public int IdPaciente { get; set; }
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public int Edad { get; set; }
    public decimal Peso { get; set; }
}
