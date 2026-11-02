namespace TPRobots.Tests.Fakes
{
    // Devuelve siempre los valores que le pasamos, en orden.
    public class GeneradorFalso : IGeneradorAleatorio
    {
        private readonly int[] _valores;
        private int _posicion;

        public GeneradorFalso(params int[] valores)
        {
            _valores = valores;
        }

        public int Siguiente(int minimo, int maximo)
        {
            int valor = _valores[_posicion % _valores.Length];
            _posicion++;
            return valor;
        }
    }
}
