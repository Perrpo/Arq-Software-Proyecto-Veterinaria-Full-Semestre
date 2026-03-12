using VetConsoleApp.Models;

namespace VetConsoleApp.Services;

public class InMemoryData
{
    public required List<Usuario> Usuarios { get; init; }
    public required List<Servicio> Servicios { get; init; }
    public required List<Paciente> Pacientes { get; init; }
    public required List<Cita> Citas { get; init; }
    public required List<Pago> Pagos { get; init; }
    public required List<Examen> Examenes { get; init; }
    public required List<Veterinario> Veterinarios { get; init; }

    public static InMemoryData Load() => new()
    {
        Usuarios = new()
        {
            new Usuario { IdUsuario = 1, Nombre = "Admin", Apellido = "Sistema", Email = "admin@vetcare.com", Telefono = "+57 300 000 0000", Direccion = "Oficina Central", Rol = "admin", FechaRegistro = DateTime.Parse("2025-09-03 02:27:55") },
            new Usuario { IdUsuario = 2, Nombre = "María", Apellido = "González", Email = "maria.gonzalez@email.com", Telefono = "+57 300 123 456", Direccion = "Calle Mayor 123, Bogotá", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-03 02:27:55") },
            new Usuario { IdUsuario = 3, Nombre = "Carlos", Apellido = "Rodríguez", Email = "carlos.rodriguez@veterinaria.com", Telefono = "+57 300 789 018", Direccion = "Avenida Veterinaria 45, Bogotá", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-03 02:27:55") },
            new Usuario { IdUsuario = 4, Nombre = "Ana", Apellido = "López", Email = "ana.lopez@email.com", Telefono = "+57 300 345 678", Direccion = "Plaza Central 8, Bogotá", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-03 02:27:55") },
            new Usuario { IdUsuario = 5, Nombre = "Elena", Apellido = "Martín", Email = "elena.martin@veterinaria.com", Telefono = "+57 300 555 444", Direccion = "Consulta 2, Bogotá", Rol = "veterinario", FechaRegistro = DateTime.Parse("2025-09-03 02:27:55") },
            new Usuario { IdUsuario = 6, Nombre = "Nicole", Apellido = "Yuqui", Email = "nicole@gmail.com", Telefono = "3123821739", Direccion = "Mi casa", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-03 17:55:18") },
            new Usuario { IdUsuario = 17, Nombre = "Andres", Apellido = "Felipe", Email = "andresfelipe@gmail.com", Telefono = "3503374876", Direccion = "Al costado de mi vecino", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-03 20:22:37") },
            new Usuario { IdUsuario = 18, Nombre = "Jose", Apellido = "Velez", Email = "josevelez@gmail.com", Telefono = "3059305930", Direccion = "Al costado de mi vecino", Rol = "cliente", FechaRegistro = DateTime.Parse("2025-09-04 17:54:43") },
            new Usuario { IdUsuario = 19, Nombre = "Andres", Apellido = "Nunez", Email = "andres@gmail.com", Telefono = "389271983271", Direccion = "meedellin", Rol = "cliente", FechaRegistro = DateTime.Parse("2026-02-14 19:39:56") }
        },
        Servicios = new()
        {
            new Servicio { IdServicio = 1, Nombre = "Consulta General", Descripcion = "Revisión médica general del paciente", Precio = 180000, EspecialidadRequerida = "medicina_general" },
            new Servicio { IdServicio = 2, Nombre = "Vacunación", Descripcion = "Administración de vacunas", Precio = 120000, EspecialidadRequerida = "medicina_general" },
            new Servicio { IdServicio = 3, Nombre = "Cirugía Menor", Descripcion = "Procedimientos quirúrgicos menores", Precio = 300000, EspecialidadRequerida = "cirugia" },
            new Servicio { IdServicio = 4, Nombre = "Análisis de Sangre", Descripcion = "Extracción y análisis de sangre", Precio = 200000, EspecialidadRequerida = "medicina_general" },
            new Servicio { IdServicio = 5, Nombre = "Radiografía", Descripcion = "Radiografía digital para diagnóstico", Precio = 250000, EspecialidadRequerida = "medicina_general" },
            new Servicio { IdServicio = 6, Nombre = "Limpieza Dental", Descripcion = "Limpieza profesional de dientes", Precio = 220000, EspecialidadRequerida = "medicina_general" },
            new Servicio { IdServicio = 7, Nombre = "Urgencias", Descripcion = "Atención médica urgente", Precio = 350000, EspecialidadRequerida = "urgencias" },
            new Servicio { IdServicio = 8, Nombre = "Peluquería Canina", Descripcion = "Servicio de peluquería y estética", Precio = 160000, EspecialidadRequerida = "medicina_general" }
        },
        Pacientes = new()
        {
            new Paciente { IdPaciente = 1, IdUsuario = 2, Nombre = "Luna", Especie = "Perro", Raza = "Golden Retriever", Edad = 3, Peso = 28.50m },
            new Paciente { IdPaciente = 2, IdUsuario = 2, Nombre = "Miau", Especie = "Gato", Raza = "Persa", Edad = 2, Peso = 4.20m },
            new Paciente { IdPaciente = 3, IdUsuario = 4, Nombre = "Max", Especie = "Perro", Raza = "Pastor Alemán", Edad = 5, Peso = 35.00m },
            new Paciente { IdPaciente = 4, IdUsuario = 4, Nombre = "Coco", Especie = "Conejo", Raza = "Holandés", Edad = 1, Peso = 1.80m },
            new Paciente { IdPaciente = 5, IdUsuario = 2, Nombre = "Pipo", Especie = "Ave", Raza = "Canario", Edad = 2, Peso = 0.25m },
            new Paciente { IdPaciente = 6, IdUsuario = 6, Nombre = "Oreo", Especie = "Perro", Raza = "Chandoso", Edad = 3, Peso = 60.02m },
            new Paciente { IdPaciente = 7, IdUsuario = 17, Nombre = "Pumba", Especie = "Gato", Raza = "Otro", Edad = 1, Peso = 5.00m },
            new Paciente { IdPaciente = 8, IdUsuario = 5, Nombre = "Max", Especie = "Gato", Raza = "Criollo", Edad = 2, Peso = 300.00m },
            new Paciente { IdPaciente = 9, IdUsuario = 18, Nombre = "Tomas", Especie = "Gato", Raza = "Persa", Edad = 1, Peso = 5.00m }
        },
        Citas = new()
        {
            new Cita(1, 2, 1, 1, DateTime.Parse("2024-09-15 20:30:00"), "confirmada"),
            new Cita(2, 4, 3, 2, DateTime.Parse("2024-09-15 14:00:00"), "pendiente"),
            new Cita(3, 2, 2, 6, DateTime.Parse("2024-09-16 09:00:00"), "completada"),
            new Cita(4, 4, 4, 1, DateTime.Parse("2024-09-16 11:30:00"), "confirmada"),
            new Cita(5, 2, 1, 7, DateTime.Parse("2024-09-14 20:00:00"), "cancelada"),
            new Cita(6, 6, 6, 8, DateTime.Parse("2025-09-11 01:30:00"), "cancelada"),
            new Cita(7, 17, 7, 4, DateTime.Parse("2025-09-18 15:00:00"), "completada"),
            new Cita(8, 17, 7, 1, DateTime.Parse("2025-09-13 00:00:00"), "cancelada")
        },
        Pagos = new()
        {
            new Pago(1, 1, "tarjeta_credito", 180000m, 0, DateTime.Parse("2025-09-03 02:27:55"), "pagado"),
            new Pago(2, 2, "efectivo", 120000m, 0, DateTime.Parse("2025-09-03 02:27:55"), "pagado"),
            new Pago(3, 3, "transferencia", 60000m, 0, DateTime.Parse("2025-09-03 02:27:55"), "pendiente"),
            new Pago(4, 4, "tarjeta_credito", 120000m, 0, DateTime.Parse("2025-09-03 02:27:55"), "pagado"),
            new Pago(5, 5, "efectivo", 95000m, 0, DateTime.Parse("2025-09-03 02:27:55"), "pagado"),
            new Pago(6, 6, "efectivo", 60000m, 0, DateTime.Parse("2025-09-04 00:08:06"), "pagado"),
            new Pago(7, 7, "tarjeta_credito", 200000m, 0, DateTime.Parse("2025-09-04 08:53:39"), "pagado"),
            new Pago(8, 7, "efectivo", 80000m, 0, DateTime.Parse("2025-09-04 22:53:58"), "pagado")
        },
        Examenes = new()
        {
            new Examen { IdExamen = 1, IdPaciente = 7, TipoExamen = "Análisis de Sangre", FechaExamen = DateTime.Parse("2025-09-03 22:44:09"), Resultado = null, Observaciones = "Lo pidio el veterinario", Estado = "pendiente" }
        },
        Veterinarios = new()
        {
            new Veterinario(1, "Dra. Elena Martín", new[] { "medicina_general", "cirugia" }),
            new Veterinario(2, "Dr. Carlos Rodríguez", new[] { "medicina_general", "dermatologia" }),
            new Veterinario(3, "Dra. Ana López", new[] { "urgencias" }),
            new Veterinario(4, "Dr. Javier Pérez", new[] { "cirugia" }),
            new Veterinario(5, "Dra. Sofía Mejía", new[] { "urgencias", "medicina_general" }),
            new Veterinario(6, "Dr. Miguel Torres", new[] { "dermatologia" }),
            new Veterinario(7, "Dra. Paula Ríos", new[] { "medicina_general" }),
            new Veterinario(8, "Dr. Luis Gómez", new[] { "medicina_general", "dermatologia" })
        }
    };
}
