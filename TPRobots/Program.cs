using TPRobots;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Bienvenido al Torneo de Robots =====\n");

        Robot miRobot = CrearRobotJugador();

        bool salir = false;

        while (!salir)
        {
            MostrarMenu(miRobot);

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    miRobot.Entrenar();
                    break;

                case "2":
                    miRobot.Descansar();
                    break;

                case "3":
                    RealizarDueloContraRival(miRobot);
                    break;

                case "4":
                    Console.WriteLine("\n" + miRobot.ToString() + "\n");
                    break;

                case "0":
                    salir = true;
                    Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");
                    break;

                default:
                    Console.WriteLine("Opción inválida. Probá de nuevo.\n");
                    break;
            }
        }
    }

    static Robot CrearRobotJugador()
    {
        Console.Write("Ingresá el nombre de tu robot: ");
        string nombre = Console.ReadLine() ?? "SinNombre";

        Console.WriteLine("Elegí el tipo de robot:");
        Console.WriteLine("1_ Robot de Asalto (más ofensivo, más azar)");
        Console.WriteLine("2_ Robot Defensivo (más constante, depende de la energía)");
        Console.Write("Opción: ");
        string? tipo = Console.ReadLine();

        int energiaInicial = 100;
        int nivelInicial = 10;

        Robot robotCreado;

        if (tipo == "2")
            robotCreado = new RobotDefensivo(nombre, energiaInicial, nivelInicial);
        else
            robotCreado = new RobotAsalto(nombre, energiaInicial, nivelInicial); // por defecto, de asalto

        Console.WriteLine($"\n¡Robot creado! {robotCreado}\n");
        return robotCreado;
    }

    static void MostrarMenu(Robot robot)
    {
        Console.WriteLine("----- Concurso de Robots -----");
        Console.WriteLine($"Tu robot: {robot.Nombre} (Energía: {robot.Energia}, Nivel: {robot.NivelHabilidad})");
        Console.WriteLine("1_ Presione 1 para entrenar y aumentar el nivel de tu robot");
        Console.WriteLine("2_ Presione 2 para descansar y recuperar la energía de tu robot");
        Console.WriteLine("3_ Presione 3 para tener un duelo con un robot rival");
        Console.WriteLine("4_ Presione 4 para ver el estado de tu robot");
        Console.WriteLine("0_ Presione 0 para salir");
        Console.Write("Elegí una opción: ");
    }

    static void RealizarDueloContraRival(Robot miRobot)
    {
        if (!miRobot.PuedePelear())
        {
            Console.WriteLine($"\n{miRobot.Nombre} no tiene suficiente energía para pelear. " +
                               "Hacelo descansar primero.\n");
            return;
        }

        Robot rival = GenerarRobotRival();
        Console.WriteLine($"\nApareció un rival: {rival}\n");

        // miRobot está tipado como Robot (clase base), pero en la práctica
        // siempre implementa IDuelo, por eso lo casteo.
        IDuelo robotQuePelea = (IDuelo)miRobot;
        Robot ganador = robotQuePelea.RealizarDuelo(rival);

        Console.WriteLine();
        if (ganador == miRobot)
            Console.WriteLine($"¡Ganaste! {miRobot.Nombre} venció a {rival.Nombre}.\n");
        else
            Console.WriteLine($"Perdiste... {rival.Nombre} venció a {miRobot.Nombre}.\n");
    }

    static Robot GenerarRobotRival()
    {
        Random random = new Random();
        string[] nombresPosibles = { "Titanio-X", "Chatarrero", "Rayo-9", "MegaBot", "Centinela" };

        string nombreRival = nombresPosibles[random.Next(nombresPosibles.Length)];
        int energiaRival = random.Next(60, 101);
        int nivelRival = random.Next(5, 21);

        if (random.Next(0, 2) == 0)
            return new RobotAsalto(nombreRival, energiaRival, nivelRival);
        else
            return new RobotDefensivo(nombreRival, energiaRival, nivelRival);
    }
}

