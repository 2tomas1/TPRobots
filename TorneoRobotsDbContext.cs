using System;
using Microsoft.EntityFrameworkCore;

namespace TPRobots
{
    public class TorneoRobotsDbContext : DbContext
    {
        public DbSet<RobotEntity> Robots => Set<RobotEntity>();

        public DbSet<ResultadoDueloEntity> ResultadosDuelos
            => Set<ResultadoDueloEntity>();

        private readonly string? _cadenaConexion;

        // Constructor utilizado por la aplicación.
        public TorneoRobotsDbContext(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        // Constructor utilizado por los tests.
        public TorneoRobotsDbContext(
            DbContextOptions<TorneoRobotsDbContext> opciones)
            : base(opciones)
        {
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_cadenaConexion!);
            }
        }
    }

    public class RobotEntity
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Tipo { get; set; } = "";

        public int Energia { get; set; }

        public int NivelHabilidad { get; set; }

        public string Estado { get; set; } = "";
    }

    public class ResultadoDueloEntity
    {
        public int Id { get; set; }

        public string Ganador { get; set; } = "";

        public string Perdedor { get; set; } = "";

        public DateTime Fecha { get; set; }
    }
}