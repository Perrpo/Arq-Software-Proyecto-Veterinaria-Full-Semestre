using System.Globalization;
using VetConsoleApp.Models;

namespace VetConsoleApp.Services;

public class DataReporter
{
    private static readonly CultureInfo _culture = new("es-CO");
    private readonly InMemoryData _data;

    public DataReporter(InMemoryData data)
    {
        _data = data;
    }

    public void PrintEverything()
    {
        PrintUsuarios();
        PrintServicios();
        PrintPacientes();
        PrintVeterinarios();
        PrintCitas();
        PrintPagos();
        PrintExamenes();
    }

    public void PrintUsuarios()
    {
        Console.WriteLine("=== Usuarios ===");
        foreach (var u in _data.Usuarios.OrderBy(u => u.IdUsuario))
        {
            Console.WriteLine($"{u.IdUsuario} | {u.Nombre} {u.Apellido} | {u.Email} | Rol: {u.Rol} | Tel: {u.Telefono} | Dir: {u.Direccion} | Registro: {u.FechaRegistro:yyyy-MM-dd HH:mm:ss}");
        }
        Console.WriteLine();
    }

    public void PrintServicios()
    {
        Console.WriteLine("=== Servicios ===");
        foreach (var s in _data.Servicios.OrderBy(s => s.IdServicio))
        {
            Console.WriteLine($"{s.IdServicio} | {s.Nombre} | ${s.Precio.ToString("N0", _culture)} | {s.Descripcion}");
        }
        Console.WriteLine();
    }

    public void PrintPacientes()
    {
        Console.WriteLine("=== Pacientes ===");
        foreach (var p in _data.Pacientes.OrderBy(p => p.IdPaciente))
        {
            Console.WriteLine($"{p.IdPaciente} | Usuario: {p.IdUsuario} | {p.Nombre} ({p.Especie}) Raza: {p.Raza} Edad: {p.Edad} Peso: {p.Peso}");
        }
        Console.WriteLine();
    }

    public void PrintCitas()
    {
        Console.WriteLine("=== Citas ===");
        foreach (var c in _data.Citas.OrderBy(c => c.IdCita))
        {
            var vet = c.IdVeterinario.HasValue
                ? _data.Veterinarios.FirstOrDefault(v => v.IdVeterinario == c.IdVeterinario.Value)?.Nombre ?? $"Vet {c.IdVeterinario}"
                : "Sin asignar";
            var paciente = _data.Pacientes.FirstOrDefault(p => p.IdPaciente == c.IdPaciente)?.Nombre ?? $"Paciente {c.IdPaciente}";
            var servicio = _data.Servicios.FirstOrDefault(s => s.IdServicio == c.IdServicio)?.Nombre ?? $"Servicio {c.IdServicio}";
            var usuario = _data.Usuarios.FirstOrDefault(u => u.IdUsuario == c.IdUsuario)?.Nombre ?? $"Usuario {c.IdUsuario}";
            Console.WriteLine($"{c.IdCita} | {paciente} | {servicio} | Cliente: {usuario} | Vet: {vet} | {c.FechaCita:yyyy-MM-dd HH:mm:ss} | Estado: {c.Estado}");
        }
        Console.WriteLine();
    }

    public void PrintPagos()
    {
        Console.WriteLine("=== Pagos ===");
        foreach (var p in _data.Pagos.OrderBy(p => p.IdPago))
        {
            var cita = _data.Citas.FirstOrDefault(c => c.IdCita == p.IdCita);
            var paciente = cita is null ? $"Cita {p.IdCita}" : _data.Pacientes.FirstOrDefault(pa => pa.IdPaciente == cita.IdPaciente)?.Nombre ?? $"Paciente {cita.IdPaciente}";
            Console.WriteLine($"{p.IdPago} | Cita: {p.IdCita} ({paciente}) | {p.MetodoPago} | Bruto: ${p.Monto.ToString("N0", _culture)} | Desc: ${p.Descuento.ToString("N0", _culture)} | Neto: ${p.MontoNeto.ToString("N0", _culture)} | {p.FechaPago:yyyy-MM-dd HH:mm:ss} | Estado: {p.Estado}");
        }
        Console.WriteLine();
    }

    public void PrintExamenes()
    {
        Console.WriteLine("=== Examenes ===");
        foreach (var e in _data.Examenes.OrderBy(e => e.IdExamen))
        {
            var obs = string.IsNullOrWhiteSpace(e.Observaciones) ? "-" : e.Observaciones;
            var res = string.IsNullOrWhiteSpace(e.Resultado) ? "-" : e.Resultado;
            var paciente = _data.Pacientes.FirstOrDefault(p => p.IdPaciente == e.IdPaciente)?.Nombre ?? $"Paciente {e.IdPaciente}";
            Console.WriteLine($"{e.IdExamen} | Paciente: {paciente} | {e.TipoExamen} | {e.FechaExamen:yyyy-MM-dd HH:mm:ss} | Estado: {e.Estado} | Resultado: {res} | Obs: {obs}");
        }
        Console.WriteLine();
    }

    public void PrintVeterinarios()
    {
        Console.WriteLine("=== Veterinarios ===");
        foreach (var v in _data.Veterinarios.OrderBy(v => v.IdVeterinario))
        {
            Console.WriteLine($"{v.IdVeterinario} | {v.Nombre} | Esp: {string.Join(",", v.Especialidades)} | Citas agendadas: {v.Agenda.Count}");
        }
        Console.WriteLine();
    }
}
