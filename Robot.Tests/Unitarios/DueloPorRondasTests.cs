using TPRobots.Tests.Fakes;

namespace TPRobots.Tests.Unitarios
{
    public class DueloPorRondasTests
    {
        // Robot de asalto con nivel 10: en cada ronda saca 20 + el valor del generador.
        private static Robot CrearRobot(string nombre, params int[] valoresPorRonda)
        {
            Robot robot = RobotFactory.Crear(TipoRobot.Asalto, nombre, 60, 10);
            robot.Generador = new GeneradorFalso(valoresPorRonda);
            return robot;
        }

        [Fact]
        public void Jugar_AGanaLasDosPrimerasRondas_GanaASinJugarTercera()
        {
            Robot robotA = CrearRobot("A", 10, 10, 10);
            Robot robotB = CrearRobot("B", 0, 0, 0);
            var duelo = new DueloPorRondas();

            ResultadoDueloPorRondas resultado = duelo.Jugar(robotA, robotB);

            Assert.Same(robotA, resultado.Ganador);
            Assert.Equal(2, resultado.RondasJugadas);
        }

        [Fact]
        public void Jugar_AGanaBGanaYAGanaLaTercera_GanaA()
        {
            Robot robotA = CrearRobot("A", 10, 0, 10);
            Robot robotB = CrearRobot("B", 0, 10, 0);
            var duelo = new DueloPorRondas();

            ResultadoDueloPorRondas resultado = duelo.Jugar(robotA, robotB);

            Assert.Same(robotA, resultado.Ganador);
            Assert.Equal(3, resultado.RondasJugadas);
        }

        [Fact]
        public void Jugar_TresRondasEmpatadas_TerminaEnEmpate()
        {
            Robot robotA = CrearRobot("A", 5, 5, 5);
            Robot robotB = CrearRobot("B", 5, 5, 5);
            var duelo = new DueloPorRondas();

            ResultadoDueloPorRondas resultado = duelo.Jugar(robotA, robotB);

            Assert.True(resultado.EsEmpate);
            Assert.Equal(3, resultado.RondasJugadas);
        }

        [Fact]
        public void Jugar_UnaRondaCadaUnoYTerceraEmpatada_TerminaEnEmpate()
        {
            Robot robotA = CrearRobot("A", 10, 0, 5);
            Robot robotB = CrearRobot("B", 0, 10, 5);
            var duelo = new DueloPorRondas();

            ResultadoDueloPorRondas resultado = duelo.Jugar(robotA, robotB);

            Assert.True(resultado.EsEmpate);
            Assert.Equal(1, resultado.VictoriasRobot1);
            Assert.Equal(1, resultado.VictoriasRobot2);
        }
    }
}
