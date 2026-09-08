using System;
using System.Collections.Generic;
using TPRobots;

Console.WriteLine("===== TORNEO DE ROBOTS 2.0 =====");
Console.WriteLine();

// ----- Armado inicial -----
// La conexión a SQL Server ya está lista en RobotRepository.cs
// (y el script CrearBaseDeDatos.sql para crear la base en SSMS),
// pero por ahora la dejamos APAGADA para poder probar todo el
// simulador por consola sin depender de que la base exista.
//
// Cuando tengas la base creada y conectada, cambiá esta única
// línea a "true" y el programa empieza a persistir todo solo,
// sin tocar nada más.
const bool PersistenciaActiva = false;

RobotRepository? repositorio = PersistenciaActiva ? new RobotRepository() : null;

// GestorDeDuelos es el "sujeto" del Observer. Acá se registran los
// componentes que reaccionan cuando termina un duelo. El de base
// de datos solo se suma si la persistencia está activa.
GestorDeDuelos gestorDeDuelos = new GestorDeDuelos();
ObservadorRanking ranking = new ObservadorRanking();
gestorDeDuelos.Suscribir(new ObservadorConsola());
gestorDeDuelos.Suscribir(ranking);
if (repositorio != null) gestorDeDuelos.Suscribir(new ObservadorBaseDeDatos(repositorio));

// Robots de ejemplo para arrancar. Se crean con la fábrica
// (Factory Method), no con "new RobotAsalto(...)" como antes.
List<Robot> robots = new()
{
    RobotFactory.Crear(TipoRobot.Asalto, "Titán", 60, 15),
    RobotFactory.Crear(TipoRobot.Defensivo, "Centinela", 60, 15)
};
if (repositorio != null) foreach (var r in robots) repositorio.Guardar(r);

bool salir = false;

while (!salir)
{
    MostrarEstado(robots);
    MostrarMenu();

    string? entrada = Console.ReadLine();
    if (!int.TryParse(entrada, out int opcion))
    {
        Console.WriteLine("Opción inválida, ingresá un número.");
        Console.WriteLine();
        continue;
    }

    switch (opcion)
    {
        case 1:
            DueloMenu(robots, gestorDeDuelos);
            break;
        case 2:
            EntrenarMenu(robots, repositorio);
            break;
        case 3:
            DescansarMenu(robots, repositorio);
            break;
        case 4:
            CambiarEstrategiaMenu(robots);
            break;
        case 5:
            CrearRobotMenu(robots, repositorio);
            break;
        case 6:
            ranking.MostrarRanking();
            break;
        case 7:
            salir = true;
            Console.WriteLine("Saliendo del simulador. ¡Hasta la próxima!");
            break;
        default:
            Console.WriteLine("Opción no reconocida. Elegí una del 1 al 7.");
            break;
    }

    Console.WriteLine();
}

// ---------------- Funciones del menú ----------------

static void MostrarEstado(List<Robot> robots)
{
    Console.WriteLine("---------------------------------------------");
    foreach (var r in robots) Console.WriteLine(r);
    Console.WriteLine("---------------------------------------------");
}

static void MostrarMenu()
{
    Console.WriteLine("1 - Duelo");
    Console.WriteLine("2 - Entrenar un robot");
    Console.WriteLine("3 - Descansar un robot");
    Console.WriteLine("4 - Cambiar estrategia de combate de un robot");
    Console.WriteLine("5 - Crear un robot nuevo");
    Console.WriteLine("6 - Ver ranking de victorias");
    Console.WriteLine("7 - Salir");
    Console.Write("Elegí una opción: ");
}

static Robot? ElegirRobot(List<Robot> robots, string mensaje)
{
    Console.WriteLine(mensaje);
    for (int i = 0; i < robots.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {robots[i].Nombre}");
    }

    if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= robots.Count)
    {
        return robots[indice - 1];
    }

    Console.WriteLine("Selección inválida.");
    return null;
}

static void DueloMenu(List<Robot> robots, GestorDeDuelos gestor)
{
    if (robots.Count < 2)
    {
        Console.WriteLine("Hacen falta al menos dos robots para pelear.");
        return;
    }

    Robot? r1 = ElegirRobot(robots, "Elegí el primer robot:");
    if (r1 == null) return;

    Robot? r2 = ElegirRobot(robots, "Elegí el robot rival:");
    if (r2 == null || r2 == r1)
    {
        Console.WriteLine("Tenés que elegir dos robots distintos.");
        return;
    }

    gestor.EjecutarDuelo(r1, r2);
}

static void EntrenarMenu(List<Robot> robots, RobotRepository? repositorio)
{
    Robot? elegido = ElegirRobot(robots, "¿Qué robot va a entrenar?");
    if (elegido == null) return;

    Console.WriteLine(elegido.Entrenar());
    repositorio?.Actualizar(elegido);
}

static void DescansarMenu(List<Robot> robots, RobotRepository? repositorio)
{
    Robot? elegido = ElegirRobot(robots, "¿Qué robot va a descansar?");
    if (elegido == null) return;

    Console.WriteLine(elegido.Descansar());
    repositorio?.Actualizar(elegido);
}

static void CambiarEstrategiaMenu(List<Robot> robots)
{
    Robot? elegido = ElegirRobot(robots, "¿A qué robot le cambiás la estrategia de combate?");
    if (elegido == null) return;

    Console.WriteLine("1) Agresiva   2) Defensiva   3) Precisión");
    string? opcion = Console.ReadLine();

    IEstrategiaCombate nueva = opcion switch
    {
        "1" => new EstrategiaAgresiva(),
        "2" => new EstrategiaDefensiva(),
        "3" => new EstrategiaPrecision(),
        _ => elegido.EstrategiaCombate
    };

    elegido.CambiarEstrategia(nueva);
}

static void CrearRobotMenu(List<Robot> robots, RobotRepository? repositorio)
{
    Console.Write("Nombre del nuevo robot: ");
    string nombre = Console.ReadLine() ?? "SinNombre";

    Console.WriteLine("Tipo: 1) Asalto   2) Defensivo   3) Francotirador");
    string? opcionTipo = Console.ReadLine();
    TipoRobot tipo = opcionTipo switch
    {
        "2" => TipoRobot.Defensivo,
        "3" => TipoRobot.Francotirador,
        _ => TipoRobot.Asalto
    };

    Robot nuevo = RobotFactory.Crear(tipo, nombre, 60, 15);
    robots.Add(nuevo);
    repositorio?.Guardar(nuevo);
    Console.WriteLine($"Se creó {nuevo.Nombre} ({nuevo.Tipo}).");
}
