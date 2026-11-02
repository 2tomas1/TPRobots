using TPRobots.Tests.Fakes;

namespace TPRobots.Tests.Unitarios
{
    public class RobotTests
    {
        [Fact]
        public void Descansar_Energia50_TerminaConEnergia70()
        {
            Robot robot = RobotFactory.Crear(TipoRobot.Asalto, "Titán", 50, 15);
            robot.Generador = new GeneradorFalso(20);

            robot.Descansar();

            Assert.Equal(70, robot.Energia);
        }

        [Fact]
        public void Descansar_Energia95_NoSupera100()
        {
            Robot robot = RobotFactory.Crear(TipoRobot.Asalto, "Titán", 95, 15);
            robot.Generador = new GeneradorFalso(20);

            robot.Descansar();

            Assert.Equal(100, robot.Energia);
        }

        [Fact]
        public void Entrenar_RobotDanado_NoSubeNivelNiGastaEnergia()
        {
            Robot robot = RobotFactory.Crear(TipoRobot.Asalto, "Titán", 15, 10);
            robot.Generador = new GeneradorFalso(5);

            robot.Entrenar();

            Assert.Equal("Dañado", robot.EstadoActual.Nombre);
            Assert.Equal(10, robot.NivelHabilidad);
            Assert.Equal(15, robot.Energia);
        }
    }
}
