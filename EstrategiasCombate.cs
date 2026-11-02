using System;

namespace TPRobots
{
    // PATRÓN STRATEGY.
    
    public interface IEstrategiaCombate
    {
        string Nombre { get; }

        // Calcula el puntaje de ataque/defensa de "propio" en un
        // duelo contra "oponente". Gana quien saque más puntaje.
        int CalcularPuntaje(Robot propio, Robot oponente);
    }

    // Prioriza la habilidad y arriesga con azar alto. Es la de
    // mayor variabilidad: puede sacar puntajes muy altos o muy bajos.
    public class EstrategiaAgresiva : IEstrategiaCombate
    {
        public string Nombre => "Agresiva";

        public int CalcularPuntaje(Robot propio, Robot oponente)
        {
            return (propio.NivelHabilidad * 2) + propio.Generador.Siguiente(0, 16);
        }
    }

    // Se apoya en la energía disponible además de la habilidad,
    // con azar bajo. Es más pareja/predecible que la agresiva.
    public class EstrategiaDefensiva : IEstrategiaCombate
    {
        public string Nombre => "Defensiva";

        public int CalcularPuntaje(Robot propio, Robot oponente)
        {
            return (propio.NivelHabilidad * 2) + (propio.Energia / 20) + propio.Generador.Siguiente(0, 11);
        }
    }

    // Golpe certero: casi no depende del azar, pega en base al
    // nivel de habilidad sin importar la energía. El +4 es el bonus por precisión.
    public class EstrategiaPrecision : IEstrategiaCombate
    {
        public string Nombre => "Precisión";

        public int CalcularPuntaje(Robot propio, Robot oponente)
        {
            return (propio.NivelHabilidad * 2) + 4 + propio.Generador.Siguiente(0, 6);
        }
    }
}
