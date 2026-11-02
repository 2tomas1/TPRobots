using System;
using Microsoft.EntityFrameworkCore;

namespace TPRobots
{
    
    public class TorneoRobotsDbContext : DbContext
    {
        public DbSet<RobotEntity> Robots => Set<RobotEntity>();
        public DbSet<ResultadoDueloEntity> ResultadosDuelos => Set<ResultadoDueloEntity>();

        private readonly string? _cadenaConexion;

        public TorneoRobotsDbContext(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        // Lo usan los tests para pasarle una base SQLite en memoria.
        public TorneoRobotsDbContext(DbContextOptions<TorneoRobotsDbContext> opciones) : base(opciones)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_cadenaConexion!);
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
