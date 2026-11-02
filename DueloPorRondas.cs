using System;

namespace TPRobots
{
    public class ResultadoDueloPorRondas
    {
        public Robot? Ganador { get; set; }
        public int VictoriasRobot1 { get; set; }
        public int VictoriasRobot2 { get; set; }
        public int RondasJugadas { get; set; }

        public bool EsEmpate => Ganador == null;
    }

    // Duelo al mejor de 3 rondas. Gana el primero que llega a 2 rondas ganadas.
    public class DueloPorRondas
    {
        private const int MaximoRondas = 3;
        private const int RondasParaGanar = 2;

        public ResultadoDueloPorRondas Jugar(Robot robot1, Robot robot2)
        {
            var resultado = new ResultadoDueloPorRondas();

            while (resultado.RondasJugadas < MaximoRondas && resultado.Ganador == null)
            {
                int puntaje1 = robot1.EstrategiaCombate.CalcularPuntaje(robot1, robot2);
                int puntaje2 = robot2.EstrategiaCombate.CalcularPuntaje(robot2, robot1);
                resultado.RondasJugadas++;

                Console.WriteLine($"Ronda {resultado.RondasJugadas}: {robot1.Nombre} {puntaje1} - {puntaje2} {robot2.Nombre}");

                if (puntaje1 > puntaje2)
                    resultado.VictoriasRobot1++;
                else if (puntaje2 > puntaje1)
                    resultado.VictoriasRobot2++;

                if (resultado.VictoriasRobot1 == RondasParaGanar)
                    resultado.Ganador = robot1;
                else if (resultado.VictoriasRobot2 == RondasParaGanar)
                    resultado.Ganador = robot2;
            }

            // Si se jugaron las 3 rondas sin que nadie llegue a 2,
            // gana el que tenga más victorias (por ejemplo 1 a 0 con dos empates).
            if (resultado.Ganador == null)
            {
                if (resultado.VictoriasRobot1 > resultado.VictoriasRobot2)
                    resultado.Ganador = robot1;
                else if (resultado.VictoriasRobot2 > resultado.VictoriasRobot1)
                    resultado.Ganador = robot2;
            }

            Desgastar(robot1);
            Desgastar(robot2);

            return resultado;
        }

        private static void Desgastar(Robot robot)
        {
            robot.Energia = Math.Max(0, robot.Energia - 15);
            robot.ActualizarEstado();
        }
    }
}
