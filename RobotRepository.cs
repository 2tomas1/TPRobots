using System;
using System.Collections.Generic;
using System.Linq;

namespace TPRobots
{
    
    public class RobotRepository
    {
        
        
        private const string CadenaConexionPorDefecto =
            "Server=localhost\\SQLEXPRESS;Database=TorneoRobots;Trusted_Connection=True;TrustServerCertificate=True;";

        
        private readonly string _cadenaConexion;

        public RobotRepository(string? cadenaConexion = null)
        {
            _cadenaConexion = cadenaConexion ?? CadenaConexionPorDefecto;

            
        }

        
        private TorneoRobotsDbContext NuevoContexto() => new TorneoRobotsDbContext(_cadenaConexion);

       
        public void Guardar(Robot robot)
        {
            using var contexto = NuevoContexto();
            var entidad = AEntidad(robot);
            contexto.Robots.Add(entidad);
            contexto.SaveChanges();
            robot.Id = entidad.Id;
        }

        
        public void Actualizar(Robot robot)
        {
            using var contexto = NuevoContexto();
            var entidad = contexto.Robots.Find(robot.Id);
            if (entidad == null) return;

            entidad.Energia = robot.Energia;
            entidad.NivelHabilidad = robot.NivelHabilidad;
            entidad.Estado = robot.EstadoActual.Nombre;
            contexto.SaveChanges();
        }

        
        public void Eliminar(int id)
        {
            using var contexto = NuevoContexto();
            var entidad = contexto.Robots.Find(id);
            if (entidad == null) return;

            contexto.Robots.Remove(entidad);
            contexto.SaveChanges();
        }

        public void GuardarResultadoDuelo(ResultadoDuelo resultado)
        {
            using var contexto = NuevoContexto();
            contexto.ResultadosDuelos.Add(new ResultadoDueloEntity
            {
                Ganador = resultado.Ganador.Nombre,
                Perdedor = resultado.Perdedor.Nombre,
                Fecha = resultado.Fecha
            });
            contexto.SaveChanges();
        }

        
        public List<Robot> ObtenerTodos()
        {
            using var contexto = NuevoContexto();
            return contexto.Robots.ToList().Select(ARobot).ToList();
        }

        

        private static RobotEntity AEntidad(Robot robot) => new RobotEntity
        {
            Nombre = robot.Nombre,
            Tipo = robot.Tipo.ToString(),
            Energia = robot.Energia,
            NivelHabilidad = robot.NivelHabilidad,
            Estado = robot.EstadoActual.Nombre
        };

        private static Robot ARobot(RobotEntity entidad)
        {
            var tipo = Enum.Parse<TipoRobot>(entidad.Tipo);
            var robot = RobotFactory.Crear(tipo, entidad.Nombre, entidad.Energia, entidad.NivelHabilidad);
            robot.Id = entidad.Id;
            robot.ForzarEstado(entidad.Estado);
            return robot;
        }
    }
}
