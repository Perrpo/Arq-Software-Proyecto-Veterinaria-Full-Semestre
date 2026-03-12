using VetConsoleApp.Application.Interfaces;
using VetConsoleApp.Application.Services;
using VetConsoleApp.Domain.Validators;
using VetConsoleApp.Models;
using VetConsoleApp.Services;

var data = InMemoryData.Load();
ICitaService citaService = new CitaService(data);
IServicioCatalogo servicioCatalogo = new ServicioCatalogo(data);
IPagoService pagoService = new PagoService(data, citaService);
IVetAssignmentStrategy vetStrategy = new MenorCargaStrategy();
IVeterinarioService veterinarioService = new VeterinarioService(data, citaService, vetStrategy);
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
    Console.WriteLine("8) Buscar cita");
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
            case "8":
                BuscarCita();
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
    var usuario = BuscarUsuarioPorNombre();
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
    var pagoFecha = ReadDate("Fecha de pago (enter para ahora): ", allowEmptyNow: true);

    var pago = pagoService.RegistrarPago(cita.IdCita, metodo, monto, descuento, pagoFecha);
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
    Console.WriteLine("0) Selección automática (estrategia)");
    for (int i = 0; i < disponibles.Count; i++)
    {
        var v = disponibles[i];
        Console.WriteLine($"{i + 1}) {v.Nombre} | Esp: {string.Join(",", v.Especialidades)} | Agenda: {v.Agenda.Count} citas");
    }
    Console.Write("Opción (0=auto): ");
    var input = Console.ReadLine();
    int? preferido = null;
    if (int.TryParse(input, out var idx))
    {
        if (idx == 0)
            preferido = null;
        else if (idx >= 1 && idx <= disponibles.Count)
            preferido = disponibles[idx - 1].IdVeterinario;
        else
        {
            Console.WriteLine("Opción inválida.\n");
            return;
        }
    }
    else
    {
        Console.WriteLine("Opción inválida.\n");
        return;
    }

    var asignado = veterinarioService.AsignarACita(cita.IdCita, preferido);
    Console.WriteLine($"Veterinario {asignado.Nombre} asignado a la cita {cita.IdCita}.\n");
}

void BuscarCita()
{
    Console.WriteLine("=== Buscar Cita ===");
    Console.Write("Ingresa ID de cita o nombre del paciente: ");
    var filtro = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(filtro))
    {
        Console.WriteLine("Operación cancelada.\n");
        return;
    }

    List<Cita> coincidencias;
    if (int.TryParse(filtro, out var idCita))
    {
        var cita = citaService.ObtenerPorId(idCita);
        coincidencias = cita is null ? new() : new() { cita };
    }
    else
    {
        var lower = filtro.ToLowerInvariant();
        var pacientesIds = data.Pacientes
            .Where(p => p.Nombre.ToLowerInvariant().Contains(lower))
            .Select(p => p.IdPaciente)
            .ToHashSet();
        coincidencias = citaService.ObtenerTodas()
            .Where(c => pacientesIds.Contains(c.IdPaciente))
            .OrderBy(c => c.FechaCita)
            .ToList();
    }

    if (coincidencias.Count == 0)
    {
        Console.WriteLine("No se encontraron citas.\n");
        return;
    }

    if (coincidencias.Count > 1)
    {
        Console.WriteLine("Se encontraron varias, elige una:");
        var seleccion = ElegirDeLista(coincidencias, c =>
        {
            var paciente = data.Pacientes.FirstOrDefault(p => p.IdPaciente == c.IdPaciente)?.Nombre ?? $"Paciente {c.IdPaciente}";
            return $"{c.IdCita} | {paciente} | {c.FechaCita:yyyy-MM-dd HH:mm} | {c.Estado}";
        });
        if (seleccion is null)
            return;
        ImprimirCitaDetallada(seleccion);
    }
    else
    {
        ImprimirCitaDetallada(coincidencias[0]);
    }
}

void ImprimirCitaDetallada(Cita c)
{
    var paciente = data.Pacientes.FirstOrDefault(p => p.IdPaciente == c.IdPaciente)?.Nombre ?? $"Paciente {c.IdPaciente}";
    var servicio = data.Servicios.FirstOrDefault(s => s.IdServicio == c.IdServicio)?.Nombre ?? $"Servicio {c.IdServicio}";
    var usuario = data.Usuarios.FirstOrDefault(u => u.IdUsuario == c.IdUsuario)?.Nombre ?? $"Usuario {c.IdUsuario}";
    var vet = c.IdVeterinario.HasValue ? data.Veterinarios.FirstOrDefault(v => v.IdVeterinario == c.IdVeterinario.Value)?.Nombre ?? $"Vet {c.IdVeterinario}" : "Sin asignar";
    Console.WriteLine($"Cita {c.IdCita}");
    Console.WriteLine($"Paciente: {paciente}");
    Console.WriteLine($"Servicio: {servicio}");
    Console.WriteLine($"Cliente: {usuario}");
    Console.WriteLine($"Veterinario: {vet}");
    Console.WriteLine($"Fecha: {c.FechaCita:yyyy-MM-dd HH:mm}");
    Console.WriteLine($"Estado: {c.Estado}");
    Console.WriteLine();
}

Usuario? BuscarUsuarioPorNombre()
{
    Console.Write("Nombre o email de usuario: ");
    var filtro = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
    if (string.IsNullOrWhiteSpace(filtro))
    {
        Console.WriteLine("Operación cancelada.\n");
        return null;
    }

    var coincidencias = data.Usuarios
        .Where(u => u.Nombre.ToLowerInvariant().Contains(filtro)
                 || u.Apellido.ToLowerInvariant().Contains(filtro)
                 || u.Email.ToLowerInvariant().Contains(filtro))
        .OrderBy(u => u.IdUsuario)
        .ToList();

    if (coincidencias.Count == 0)
    {
        Console.WriteLine("No se encontró usuario.\n");
        return null;
    }
    if (coincidencias.Count == 1)
    {
        Console.WriteLine($"Seleccionado: {coincidencias[0].Nombre} {coincidencias[0].Apellido}\n");
        return coincidencias[0];
    }

    Console.WriteLine("Se encontraron varios, elige uno:");
    return ElegirDeLista(coincidencias, u => $"{u.Nombre} {u.Apellido} | {u.Email} | Tel: {u.Telefono}");
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
            var vet = c.IdVeterinario.HasValue ? data.Veterinarios.FirstOrDefault(v => v.IdVeterinario == c.IdVeterinario.Value)?.Nombre ?? $"Vet {c.IdVeterinario}" : "Sin vet";
            return $"{c.IdCita} | {paciente} | {servicio} | Cliente: {usuario} | Vet: {vet} | {c.FechaCita:yyyy-MM-dd HH:mm} | {c.Estado}";
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

DateTime ReadDate(string prompt, bool allowEmptyNow = false)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();
        if (allowEmptyNow && string.IsNullOrWhiteSpace(txt))
            return DateTime.Now;
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
