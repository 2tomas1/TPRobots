using System;
using TPRobots;


Console.WriteLine("===== SIMULADOR DE DUELO DE ROBOTS =====");
Console.WriteLine();

// Creamos los dos robots que va a manejar el usuario desde el
// menú. Los declaramos como RobotAsalto
RobotAsalto robot1 = new RobotAsalto("Titán", 60, 15);
RobotAsalto robot2 = new RobotAsalto("Centinela", 60, 15);

bool salir = false;

// Bucle principal del menú: se repite hasta que el usuario
// elija la opción de salir.
while (!salir)
{
    MostrarEstado(robot1, robot2);
    MostrarMenu();

    // Leemos la opción como texto y la intentamos convertir a
    // número. TryParse evita que el programa explote si el
    // usuario escribe algo que no es un número.
    string? entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out int opcion))
    {
        Console.WriteLine("Opción inválida, ingresá un número.");
        Console.WriteLine();
        continue; 
    }

    switch (opcion)
    {
        case 1: // ---- DUELO ----
            RealizarDueloMenu(robot1, robot2);
            break;

        case 2: // ---- RECARGAR ENERGÍA ----
            RecargarEnergiaMenu(robot1, robot2);
            break;

        case 3: // ---- SUBIR NIVEL (ENTRENAR) ----
            EntrenarMenu(robot1, robot2);
            break;

        case 4: // ---- SALIR ----
            salir = true;
            Console.WriteLine("Saliendo del simulador. ¡Hasta la próxima!");
            break;

        default: // cualquier número que no sea 1, 2, 3 o 4
            Console.WriteLine("⚠️  Opción no reconocida. Elegí una del 1 al 4.");
            break;
    }

    Console.WriteLine();
}

static void MostrarEstado(Robot r1, Robot r2)
{
    Console.WriteLine("---------------------------------------------");
    Console.WriteLine(r1);
    Console.WriteLine(r2);
    Console.WriteLine("---------------------------------------------");
}

static void MostrarMenu()
{
    Console.WriteLine("1 - Duelo");
    Console.WriteLine("2 - Recargar energía");
    Console.WriteLine("3 - Subir nivel (entrenar)");
    Console.WriteLine("4 - Salir");
    Console.Write("Elegí una opción: ");
}

// Le pregunta al usuario cuál de los dos robots quiere elegir,
static RobotAsalto ElegirRobot(RobotAsalto r1, RobotAsalto r2)
{
    Console.WriteLine($"¿Sobre cuál robot querés actuar? 1) {r1.Nombre}   2) {r2.Nombre}");
    string? entrada = Console.ReadLine();

    if (entrada == "2") return r2;
    return r1;
}

// Caso 1 del switch: gestiona el duelo entre los dos robots.
static void RealizarDueloMenu(RobotAsalto r1, RobotAsalto r2)
{
    // Antes de pelear, chequeamos que ambos tengan energía
    // suficiente usando PuedePelear() (definido en Robot).
    if (!r1.PuedePelear())
    {
        Console.WriteLine($"{r1.Nombre} no tiene energía suficiente para pelear (recargá antes).");
        return;
    }

    if (!r2.PuedePelear())
    {
        Console.WriteLine($"{r2.Nombre} no tiene energía suficiente para pelear (recargá antes).");
        return;
    }

    
    Random random = new Random();
    RobotAsalto atacante = random.Next(2) == 0 ? r1 : r2;
    RobotAsalto defensor = atacante == r1 ? r2 : r1;

    Console.WriteLine($"(Este duelo: ataca {atacante.Nombre}, defiende {defensor.Nombre})");

    // RealizarDuelo() viene de la interfaz IDuelo, implementada
    // por RobotAsalto
    Robot ganador = atacante.RealizarDuelo(defensor);
    Console.WriteLine($"Ganador del duelo: {ganador.Nombre}");
}

// Caso 2 del switch: recarga energía (Descansar) sobre el robot
// elegido por el usuario.
static void RecargarEnergiaMenu(RobotAsalto r1, RobotAsalto r2)
{
    RobotAsalto elegido = ElegirRobot(r1, r2);
    elegido.Descansar();
}

// Caso 3 del switch: sube el nivel de habilidad (Entrenar) del
// robot elegido por el usuario.
static void EntrenarMenu(RobotAsalto r1, RobotAsalto r2)
{
    RobotAsalto elegido = ElegirRobot(r1, r2);
    elegido.Entrenar();
}
