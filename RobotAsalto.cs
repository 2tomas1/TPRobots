using System;
using System.Collections.Generic;
using System.Text;

namespace TPRobots
{
   
    public class RobotAsalto : Robot, IDuelo
    {
        public RobotAsalto(string nombre, int energia, int nivelHabilidad)
            : base(nombre, energia, nivelHabilidad)
        {
        }

        public Robot RealizarDuelo(Robot oponente)
        {
            Random random = new Random();

            // Ataca mas fuerte: multiplica su habilidad x2 (es un robot
            // ofensivo) y le suma un golpe de azar grande (0 a 15).
            int puntajePropio = (NivelHabilidad * 2) + random.Next(0, 16);

            // El oponente se defiende con su propia habilidad, más
            // una parte de su energía (Energia / 10), más un azar
            // más chico (0 a 10). No tiene el multiplicador x2 del
            // atacante, por eso el RobotAsalto suele tener ventaja
            // ofensiva pero también más variabilidad.
            int puntajeOponente = oponente.NivelHabilidad + (oponente.Energia / 10) + random.Next(0, 11);

            Console.WriteLine($"-> {Nombre} ataca con fuerza {puntajePropio} contra la defensa de " +
                               $"{oponente.Nombre} ({puntajeOponente}).");

            // Pelear cansa a ambos robots por igual, gasten o no el
            // duelo.
            Energia = Math.Max(0, Energia - 15);
            oponente.Energia = Math.Max(0, oponente.Energia - 15);

            // Gana quien tenga mayor o igual puntaje de ataque vs
            // defensa.
            return puntajePropio >= puntajeOponente ? this : oponente;
        }
    }
}
