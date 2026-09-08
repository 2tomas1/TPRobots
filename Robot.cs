using System;

namespace TPRobots
{
  
    public class Robot
    {
        
        public int Id { get; set; }

        public string Nombre { get; set; }
        public int Energia { get; set; }
        public int NivelHabilidad { get; set; }
        public TipoRobot Tipo { get; set; }

        public IEstrategiaCombate EstrategiaCombate { get; private set; }
        public IEstadoRobot EstadoActual { get; private set; }

       
        internal Robot(string nombre, int energia, int nivelHabilidad, TipoRobot tipo)
        {
            Nombre = nombre;
            Energia = energia;
            NivelHabilidad = nivelHabilidad;
            Tipo = tipo;
            EstrategiaCombate = new EstrategiaPrecision(); // se pisa en la fábrica, ya que es el factory method
            EstadoActual = new EstadoActivo();
            ActualizarEstado();
        }

        // ---------- STRATEGY ----------

        public void CambiarEstrategia(IEstrategiaCombate nuevaEstrategia)
        {
            EstrategiaCombate = nuevaEstrategia;
            Console.WriteLine($"{Nombre} ahora combate con estrategia: {EstrategiaCombate.Nombre}.");
        }

        // ---------- STATE ----------

        // Recalcula en qué estado está el robot según su energía.las acciones
        // en sí (qué pasa si entreno estando cansado, etc.) 
        public void ActualizarEstado()
        {
            if (Energia <= 0) EstadoActual = new EstadoEliminado();
            else if (Energia < 20) EstadoActual = new EstadoDanado();
            else if (Energia < 50) EstadoActual = new EstadoCansado();
            else EstadoActual = new EstadoActivo();
        }

      
        public void ForzarEstado(string nombreEstado)
        {
            EstadoActual = nombreEstado switch
            {
                "Activo" => new EstadoActivo(),
                "Cansado" => new EstadoCansado(),
                "Dañado" => new EstadoDanado(),
                "Eliminado" => new EstadoEliminado(),
                _ => EstadoActual
            };
        }

        public bool PuedePelear => EstadoActual.PuedePelear;

        public string Entrenar() => EstadoActual.Entrenar(this);

        public string Descansar() => EstadoActual.Descansar(this);

        // ---------- DUELO ----------

        // El cálculo del puntaje se delega por completo a la
        // estrategia de cada robot
        public Robot RealizarDuelo(Robot oponente)
        {
            int puntajePropio = EstrategiaCombate.CalcularPuntaje(this, oponente);
            int puntajeOponente = oponente.EstrategiaCombate.CalcularPuntaje(oponente, this);

            Console.WriteLine($"-> {Nombre} ({EstrategiaCombate.Nombre}) saca {puntajePropio} puntos " +
                               $"contra {oponente.Nombre} ({oponente.EstrategiaCombate.Nombre}) que saca {puntajeOponente}.");

            // Pelear cansa a ambos, ganen o pierdan.
            Energia = Math.Max(0, Energia - 15);
            oponente.Energia = Math.Max(0, oponente.Energia - 15);
            ActualizarEstado();
            oponente.ActualizarEstado();

            return puntajePropio >= puntajeOponente ? this : oponente;
        }

        public override string ToString()
        {
            return $"#{Id} {Nombre} | Tipo: {Tipo} | Energía: {Energia} | Nivel: {NivelHabilidad} | " +
                   $"Estado: {EstadoActual.Nombre} | Estrategia: {EstrategiaCombate.Nombre}";
        }
    }
}
