using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TPRobots.Tests.Integracion
{
    // Usan una base SQLite en memoria que se crea antes de cada test y
    // desaparece al cerrar la conexión. No hace falta SQL Server y no se toca la base real.
    public class RobotRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _conexion;
        private readonly DbContextOptions<TorneoRobotsDbContext> _opciones;

        public RobotRepositoryTests()
        {
            _conexion = new SqliteConnection("DataSource=:memory:");
            _conexion.Open();

            _opciones = new DbContextOptionsBuilder<TorneoRobotsDbContext>()
                .UseSqlite(_conexion)
                .Options;

            using var contexto = new TorneoRobotsDbContext(_opciones);
            contexto.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _conexion.Dispose();
        }

        [Fact]
        public void ObtenerTodos_BaseConDosRobots_DevuelveExactamenteEsosDos()
        {
            using (var contexto = new TorneoRobotsDbContext(_opciones))
            {
                contexto.Robots.Add(new RobotEntity { Nombre = "Titán", Tipo = "Asalto", Energia = 60, NivelHabilidad = 15, Estado = "Activo" });
                contexto.Robots.Add(new RobotEntity { Nombre = "Centinela", Tipo = "Defensivo", Energia = 40, NivelHabilidad = 12, Estado = "Cansado" });
                contexto.SaveChanges();
            }
            var repositorio = new RobotRepository(_opciones);

            List<Robot> robots = repositorio.ObtenerTodos();

            Assert.Equal(2, robots.Count);
            Assert.Contains(robots, r => r.Nombre == "Titán" && r.Tipo == TipoRobot.Asalto);
            Assert.Contains(robots, r => r.Nombre == "Centinela" && r.Tipo == TipoRobot.Defensivo);
        }

        [Fact]
        public void Guardar_RobotNuevo_QuedaPersistidoEnLaBase()
        {
            var repositorio = new RobotRepository(_opciones);
            Robot robot = RobotFactory.Crear(TipoRobot.Francotirador, "Halcón", 70, 20);

            repositorio.Guardar(robot);

            using var contexto = new TorneoRobotsDbContext(_opciones);
            RobotEntity? guardado = contexto.Robots.Find(robot.Id);
            Assert.NotNull(guardado);
            Assert.Equal("Halcón", guardado.Nombre);
            Assert.Equal("Francotirador", guardado.Tipo);
            Assert.Equal(70, guardado.Energia);
            Assert.Equal(20, guardado.NivelHabilidad);
        }
    }
}
