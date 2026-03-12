using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Application.Services;
using VetConsoleApp.Domain.Validators;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

var data = InMemoryData.Load();
ICitaService citaService = new CitaService(data);
IServicioCatalogo servicioCatalogo = new ServicioCatalogo(data);
IPagoService pagoService = new PagoService(data, citaService);
IVeterinarioService veterinarioService = new VeterinarioService(data, citaService);
var reporter = new DataReporter(data);

while (true)
{
    Console.WriteLine("=== Veterinaria - Gestión de Citas ===");
    Console.WriteLine("1) Listar todo");
    Console.WriteLine("2) Listar citas");
    Console.WriteLine("3) Crear cita");
    Console.WriteLine("4) Cambiar estado de una cita");
    Console.WriteLine("5) Listar pacientes");
    Console.WriteLine("6) Registrar pago");
    Console.WriteLine("7) Asignar veterinario a cita");
    Console.WriteLine("0) Salir");
    Console.Write("Selecciona opción: ");

    var option = Console.ReadLine();
    Console.WriteLine();

    try
    {
        switch (option)
        {
            case "1":
                reporter.PrintEverything();
                break;
            case "2":
                reporter.PrintCitas();
                break;
            case "3":
                CrearCita();
                break;
            case "4":
                CambiarEstadoCita();
                break;
            case "5":
                reporter.PrintPacientes();
                break;
            case "6":
                RegistrarPago();
                break;
            case "7":
                AsignarVeterinario();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Opción inválida.\n");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}

void CrearCita()
{
    Console.WriteLine("=== Crear Cita ===");
    var usuario = ElegirUsuario();
    if (usuario is null)
        return;

    var paciente = ElegirPaciente(usuario.IdUsuario);
    if (paciente is null)
        return;

    var servicio = ElegirServicio();
    if (servicio is null)
        return;

    var fecha = ReadDate("Fecha y hora (yyyy-MM-dd HH:mm): ");

    var cita = citaService.Crear(usuario.IdUsuario, paciente.IdPaciente, servicio.IdServicio, fecha);
    Console.WriteLine($"Cita {cita.IdCita} creada con estado '{cita.Estado}'.\n");
}

void CambiarEstadoCita()
{
    Console.WriteLine("=== Cambiar Estado de Cita ===");
    var cita = ElegirCita();
    if (cita is null)
    {
        return;
    }

    Console.WriteLine($"Estados válidos: {string.Join(" | ", EstadoCitaValidator.EstadosPermitidos())}");
    Console.Write("Nuevo estado: ");
    var estado = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    citaService.CambiarEstado(cita.IdCita, estado);
    Console.WriteLine("Estado actualizado.\n");
}

void RegistrarPago()
{
    Console.WriteLine("=== Registrar Pago ===");
    var cita = ElegirCita();
    if (cita is null)
        return;

    Console.Write("Método de pago: ");
    var metodo = (Console.ReadLine() ?? "").Trim();
    var monto = ReadDecimal("Monto bruto: ");
    var descuento = ReadDecimal("Descuento (0 si no aplica): ");

    var pago = pagoService.RegistrarPago(cita.IdCita, metodo, monto, descuento);
    Console.WriteLine($"Pago {pago.IdPago} registrado por ${pago.MontoNeto:N0}.\n");
}

void AsignarVeterinario()
{
    Console.WriteLine("=== Asignar Veterinario ===");
    var cita = ElegirCita();
    if (cita is null)
        return;

    var servicio = servicioCatalogo.ObtenerPorId(cita.IdServicio);
    var especialidad = servicio?.EspecialidadRequerida ?? "medicina_general";
    var disponibles = veterinarioService.Disponibles(cita.FechaCita, TimeSpan.FromMinutes(cita.DuracionMinutos), especialidad).ToList();
    if (!disponibles.Any())
    {
        Console.WriteLine("No hay veterinarios disponibles con la especialidad requerida.\n");
        return;
    }

    Console.WriteLine($"Especialidad requerida: {especialidad}");
    var vet = ElegirDeLista(disponibles, v => $"{v.Nombre} | Esp: {string.Join(",", v.Especialidades)} | Agenda: {v.Agenda.Count} citas");
    if (vet is null)
        return;

    veterinarioService.AsignarACita(cita.IdCita, vet.IdVeterinario);
    Console.WriteLine($"Veterinario {vet.Nombre} asignado a la cita {cita.IdCita}.\n");
}

Usuario? ElegirUsuario()
{
    Console.WriteLine("Elige usuario/cliente:");
    return ElegirDeLista(data.Usuarios.OrderBy(u => u.IdUsuario).ToList(),
        u => $"{u.Nombre} {u.Apellido} | {u.Email} | Tel: {u.Telefono}");
}

Paciente? ElegirPaciente(int idUsuario)
{
    var pacientes = data.Pacientes
        .OrderBy(p => p.IdPaciente)
        .Where(p => p.IdUsuario == idUsuario)
        .ToList();

    if (pacientes.Count == 0)
    {
        Console.WriteLine("Ese usuario no tiene pacientes registrados.\n");
        return null;
    }

    Console.WriteLine("Elige paciente:");
    return ElegirDeLista(pacientes, p => $"{p.Nombre} ({p.Especie}) Raza: {p.Raza} Edad: {p.Edad} Peso: {p.Peso}");
}

Servicio? ElegirServicio()
{
    Console.WriteLine("Elige servicio:");
    return ElegirDeLista(servicioCatalogo.ObtenerTodos().OrderBy(s => s.IdServicio).ToList(),
        s => $"{s.Nombre} | ${s.Precio:N0} | {s.Descripcion}");
}

Cita? ElegirCita()
{
    Console.WriteLine("Elige cita:");
    var citas = citaService.ObtenerTodas().OrderBy(c => c.IdCita).ToList();
    return ElegirDeLista(citas,
        c =>
        {
            var paciente = data.Pacientes.FirstOrDefault(p => p.IdPaciente == c.IdPaciente)?.Nombre ?? $"Paciente {c.IdPaciente}";
            var servicio = data.Servicios.FirstOrDefault(s => s.IdServicio == c.IdServicio)?.Nombre ?? $"Serv {c.IdServicio}";
            var usuario = data.Usuarios.FirstOrDefault(u => u.IdUsuario == c.IdUsuario)?.Nombre ?? $"Usuario {c.IdUsuario}";
            return $"{c.IdCita} | {paciente} | {servicio} | {usuario} | {c.FechaCita:yyyy-MM-dd HH:mm} | {c.Estado}";
        });
}

T? ElegirDeLista<T>(IReadOnlyList<T> items, Func<T, string> label)
{
    for (int i = 0; i < items.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {label(items[i])}");
    }
    Console.Write("Opción (número) o vacío para cancelar: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Operación cancelada.\n");
        return default;
    }
    if (int.TryParse(input, out var idx) && idx >= 1 && idx <= items.Count)
    {
        Console.WriteLine();
        return items[idx - 1];
    }
    Console.WriteLine("Opción inválida.\n");
    return default;
}

DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();
        if (DateTime.TryParse(txt, out var value))
            return value;
        Console.WriteLine("Formato de fecha/hora inválido.");
    }
}

decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();
        if (decimal.TryParse(txt, out var value))
            return value;
        Console.WriteLine("Valor inválido.");
    }
}
