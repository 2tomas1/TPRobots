using System;
using System.Collections.Generic;
using System.Text;

namespace TPRobots
{
    public class RobotDefensivo : Robot, IDuelo
    {
        public RobotDefensivo(string nombre, int energia, int nivelHabilidad)
            : base(nombre, energia, nivelHabilidad)
        {
        }

        public Robot RealizarDuelo(Robot oponente)
        {
            Random random = new Random();

            // Usa su energía como escudo, con menos peso del azar
            int puntajePropio = NivelHabilidad + (Energia / 5) + random.Next(0, 11);

            // El oponente ataca con su habilidad + un golpe de azar más grande
            int puntajeOponente = oponente.NivelHabilidad + random.Next(0, 16);

            Console.WriteLine($"-> {Nombre} se defiende con {puntajePropio} puntos frente al ataque de " +
                               $"{oponente.Nombre} ({puntajeOponente}).");

            Energia = Math.Max(0, Energia - 10);
            oponente.Energia = Math.Max(0, oponente.Energia - 10);

            return puntajePropio >= puntajeOponente ? this : oponente;
        }
    }
}
