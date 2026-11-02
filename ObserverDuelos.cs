using System;
using System.Collections.Generic;
using System.Linq;

namespace TPRobots
{
    // PATRÓN OBSERVER.
    // Quien termina un duelo (GestorDeDuelos) no sabe ni le importa
    // qué hace cada observador con el resultado. Solo avisa.
    public interface IObservadorDuelo
    {
        void ActualizarResultado(ResultadoDuelo resultado);
    }


    public class ResultadoDuelo
    {
        public Robot Ganador { get; }
        public Robot Perdedor { get; }
        public DateTime Fecha { get; }

        public ResultadoDuelo(Robot ganador, Robot perdedor)
        {
            Ganador = ganador;
            Perdedor = perdedor;
            Fecha = DateTime.Now;
        }
    }

    //solo muestra el resultado por pantalla. 
    public class ObservadorConsola : IObservadorDuelo
    {
        public void ActualizarResultado(ResultadoDuelo resultado)
        {
            Console.WriteLine($"[Consola] Duelo del {resultado.Fecha:HH:mm:ss} -> " +
                               $"Ganó {resultado.Ganador.Nombre}, perdió {resultado.Perdedor.Nombre}.");
        }
    }

    // Lleva la cuenta de victorias por robot.
    public class ObservadorRanking : IObservadorDuelo
    {
        private readonly Dictionary<string, int> _victorias = new();

        public void ActualizarResultado(ResultadoDuelo resultado)
        {
            string nombre = resultado.Ganador.Nombre;
            _victorias[nombre] = _victorias.GetValueOrDefault(nombre) + 1;
        }

        public void MostrarRanking()
        {
            Console.WriteLine("----- RANKING (victorias) -----");
            if (_victorias.Count == 0)
            {
                Console.WriteLine("Todavía no hay duelos registrados.");
                return;
            }

            foreach (var par in _victorias.OrderByDescending(p => p.Value))
            {
                Console.WriteLine($"{par.Key}: {par.Value} victoria(s)");
            }
        }
    }

    // El observador que toca la base de datos. Guarda
    // el resultado del duelo y deja actualizados en la tabla Robots
    // la energía/nivel/estado de ambos.
    public class ObservadorBaseDeDatos : IObservadorDuelo
    {
        private readonly RobotRepository _repositorio;

        public ObservadorBaseDeDatos(RobotRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public void ActualizarResultado(ResultadoDuelo resultado)
        {
            _repositorio.GuardarResultadoDuelo(resultado);
            _repositorio.Actualizar(resultado.Ganador);
            _repositorio.Actualizar(resultado.Perdedor);
        }
    }

    
    public class GestorDeDuelos
    {
        private readonly List<IObservadorDuelo> _observadores = new();

        public void Suscribir(IObservadorDuelo observador) => _observadores.Add(observador);

        public void Desuscribir(IObservadorDuelo observador) => _observadores.Remove(observador);

        public void EjecutarDuelo(Robot robot1, Robot robot2)
        {
            if (!AmbosPuedenPelear(robot1, robot2)) return;

            Robot ganador = robot1.RealizarDuelo(robot2);
            Robot perdedor = ganador == robot1 ? robot2 : robot1;

            Console.WriteLine($"Ganador del duelo: {ganador.Nombre}");

            Notificar(new ResultadoDuelo(ganador, perdedor));
        }

        public ResultadoDueloPorRondas? EjecutarDueloPorRondas(Robot robot1, Robot robot2)
        {
            if (!AmbosPuedenPelear(robot1, robot2)) return null;

            ResultadoDueloPorRondas resultado = new DueloPorRondas().Jugar(robot1, robot2);

            if (resultado.Ganador == null)
            {
                Console.WriteLine($"El duelo por rondas terminó en empate ({resultado.VictoriasRobot1} a {resultado.VictoriasRobot2}).");
                return resultado;
            }

            Robot perdedor = resultado.Ganador == robot1 ? robot2 : robot1;
            Console.WriteLine($"Ganador del duelo por rondas: {resultado.Ganador.Nombre} " +
                              $"({resultado.VictoriasRobot1} a {resultado.VictoriasRobot2}).");

            Notificar(new ResultadoDuelo(resultado.Ganador, perdedor));
            return resultado;
        }

        private bool AmbosPuedenPelear(Robot robot1, Robot robot2)
        {
            if (!robot1.PuedePelear)
            {
                Console.WriteLine($"{robot1.Nombre} no puede pelear (estado: {robot1.EstadoActual.Nombre}).");
                return false;
            }

            if (!robot2.PuedePelear)
            {
                Console.WriteLine($"{robot2.Nombre} no puede pelear (estado: {robot2.EstadoActual.Nombre}).");
                return false;
            }

            return true;
        }

        private void Notificar(ResultadoDuelo resultado)
        {
            foreach (var observador in _observadores)
            {
                observador.ActualizarResultado(resultado);
            }
        }
    }
}
