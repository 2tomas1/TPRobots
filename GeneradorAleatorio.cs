using System;

namespace TPRobots
{
    // Se separa el Random en una interfaz para poder usar un valor
    // controlado en los tests.
    public interface IGeneradorAleatorio
    {
        int Siguiente(int minimo, int maximo);
    }

    public class GeneradorAleatorio : IGeneradorAleatorio
    {
        private readonly Random _random = new Random();

        public int Siguiente(int minimo, int maximo)
        {
            return _random.Next(minimo, maximo);
        }
    }
}
