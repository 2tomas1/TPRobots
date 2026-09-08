namespace TPRobots
{
    public enum TipoRobot
    {
        Asalto,
        Defensivo,
        Francotirador
    }

    // PATRÓN FACTORY METHOD.
    // Es la única clase del programa que conoce la relación entre
    // "tipo de robot" y su estrategia de combate por defecto.
    public static class RobotFactory
    {
        public static Robot Crear(TipoRobot tipo, string nombre, int energia, int nivelHabilidad)
        {
            Robot robot = new Robot(nombre, energia, nivelHabilidad, tipo);
            robot.CambiarEstrategia(EstrategiaPorDefecto(tipo));
            return robot;
        }

        private static IEstrategiaCombate EstrategiaPorDefecto(TipoRobot tipo)
        {
            return tipo switch
            {
                TipoRobot.Asalto => new EstrategiaAgresiva(),
                TipoRobot.Defensivo => new EstrategiaDefensiva(),
                TipoRobot.Francotirador => new EstrategiaPrecision(),
                _ => new EstrategiaPrecision()
            };
        }
    }
}
