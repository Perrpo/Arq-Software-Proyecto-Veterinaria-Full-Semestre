namespace VetConsoleApp.Domain.Validators;

public static class EstadoCitaValidator
{
    private static readonly HashSet<string> _validos = new(StringComparer.OrdinalIgnoreCase)
    {
        "pendiente",
        "confirmada",
        "completada",
        "cancelada"
    };

    public static bool EsValido(string? estado) => estado is not null && _validos.Contains(estado.Trim());

    public static string Normalizar(string estado) => estado.Trim().ToLowerInvariant();

    public static IEnumerable<string> EstadosPermitidos() => _validos;
}
