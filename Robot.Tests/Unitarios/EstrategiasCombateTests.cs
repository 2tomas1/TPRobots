using TPRobots.Tests.Fakes;

namespace TPRobots.Tests.Unitarios
{
    public class EstrategiasCombateTests
    {
        [Fact]
        public void CalcularPuntajeDuelo_RobotAsalto_DevuelvePuntajeCorrecto()
        {
            Robot asalto = RobotFactory.Crear(TipoRobot.Asalto, "Titán", 60, 15);
            Robot rival = RobotFactory.Crear(TipoRobot.Defensivo, "Centinela", 60, 15);
            asalto.Generador = new GeneradorFalso(5);

            int puntaje = asalto.EstrategiaCombate.CalcularPuntaje(asalto, rival);

            // 15 * 2 + 5
            Assert.Equal(35, puntaje);
        }

        [Fact]
        public void CalcularPuntajeDuelo_RobotDefensivo_DevuelvePuntajeCorrecto()
        {
            Robot defensivo = RobotFactory.Crear(TipoRobot.Defensivo, "Centinela", 60, 15);
            Robot rival = RobotFactory.Crear(TipoRobot.Asalto, "Titán", 60, 15);
            defensivo.Generador = new GeneradorFalso(4);

            int puntaje = defensivo.EstrategiaCombate.CalcularPuntaje(defensivo, rival);

            // 15 * 2 + 60 / 20 + 4
            Assert.Equal(37, puntaje);
        }
    }
}
