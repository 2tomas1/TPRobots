using System;
using System.Collections.Generic;
using System.Text;

namespace TPRobots
{
       // ROBOT DE ASALTO (ofensivo, más azar)
       // ==========================================================
       public class RobotAsalto : Robot, IDuelo
        {
            public RobotAsalto(string nombre, int energia, int nivelHabilidad)
                : base(nombre, energia, nivelHabilidad)
            {
            }
            public Robot RealizarDuelo(Robot oponente)
            {
                Random random = new Random();

                // Ataca fuerte: multiplica su habilidad x2 + un golpe de azar grande
                int puntajePropio = (NivelHabilidad * 2) + random.Next(0, 16);

                // El oponente se defiende con su habilidad + parte de su energía
                int puntajeOponente = oponente.NivelHabilidad + (oponente.Energia / 10) + random.Next(0, 11);

                Console.WriteLine($"-> {Nombre} ataca con fuerza {puntajePropio} contra la defensa de " +
                                   $"{oponente.Nombre} ({puntajeOponente}).");

                // Pelear cansa a ambos
                Energia = Math.Max(0, Energia - 15);
                oponente.Energia = Math.Max(0, oponente.Energia - 15);

                return puntajePropio >= puntajeOponente ? this : oponente;
            }
        }
   }
