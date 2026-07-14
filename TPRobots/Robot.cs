using System;
using System.Collections.Generic;
using System.Text;

namespace TPRobots
{
    public class Robot
    {
        public string Nombre { get; set; }
        public int Energia { get; set; }
        public int NivelHabilidad { get; set; }

        private const int EnergiaMaxima = 100;
        private const int EnergiaMinimaParaPelear = 20;

        public Robot(string nombre, int energia, int nivelHabilidad)
        {
            Nombre = nombre;
            Energia = energia;
            NivelHabilidad = nivelHabilidad;
        }

        // Aumenta el nivel de habilidad del robot, pero gasta energía
        public void Entrenar()
        {
            int aumento = new Random().Next(1, 6); // sube entre 1 y 5 puntos
            NivelHabilidad += aumento;

            Energia -= 10;
            if (Energia < 0) Energia = 0;

            Console.WriteLine($"{Nombre} entrenó y ahora tiene nivel de habilidad {NivelHabilidad} " +
                               $"(energía restante: {Energia}).");
        }

        // Recupera energía para el próximo duelo
        public void Descansar()
        {
            int recuperacion = new Random().Next(10, 26); // recupera entre 10 y 25
            Energia += recuperacion;

            if (Energia > EnergiaMaxima) Energia = EnergiaMaxima;

            Console.WriteLine($"{Nombre} descansó y recuperó energía. Energía actual: {Energia}.");
        }

        public bool PuedePelear()
        {
            return Energia >= EnergiaMinimaParaPelear;
        }

        public override string ToString()
        {
            return $"{Nombre} | Tipo: {this.GetType().Name} | Energía: {Energia} | Nivel: {NivelHabilidad}";
        }
    }

}
