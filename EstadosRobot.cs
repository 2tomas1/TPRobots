using System;

namespace TPRobots
{
    // PATRÓN STATE.
    // Cada estado decide qué pasa cuando el robot entrena, descansa
    // o intenta pelear. 

    public interface IEstadoRobot
    {
        string Nombre { get; }
        bool PuedePelear { get; }

        // Devuelven un mensaje para mostrar por consola. Reciben el
        // robot para poder modificar Energia / NivelHabilidad.
        string Entrenar(Robot robot);
        string Descansar(Robot robot);
    }

    // Energía >= 50. El robot funciona a pleno: puede entrenar,
    // descansar y pelear sin restricciones.
    public class EstadoActivo : IEstadoRobot
    {
        public string Nombre => "Activo";
        public bool PuedePelear => true;

        public string Entrenar(Robot robot)
        {
            int aumento = robot.Generador.Siguiente(1, 6);
            robot.NivelHabilidad += aumento;
            robot.Energia = Math.Max(0, robot.Energia - 10);
            robot.ActualizarEstado();
            return $"{robot.Nombre} entrenó y ahora tiene nivel {robot.NivelHabilidad}.";
        }

        public string Descansar(Robot robot)
        {
            robot.Energia = Math.Min(100, robot.Energia + robot.Generador.Siguiente(10, 26));
            robot.ActualizarEstado();
            return $"{robot.Nombre} ya estaba activo, igual descansa un poco más.";
        }
    }

    // 20 <= Energía < 50. Todavía puede pelear, pero entrenar
    // rinde menos (el robot está gastado).
    public class EstadoCansado : IEstadoRobot
    {
        public string Nombre => "Cansado";
        public bool PuedePelear => true;

        public string Entrenar(Robot robot)
        {
            int aumento = robot.Generador.Siguiente(1, 3); // rinde menos que Activo
            robot.NivelHabilidad += aumento;
            robot.Energia = Math.Max(0, robot.Energia - 15);
            robot.ActualizarEstado();
            return $"{robot.Nombre} entrenó con esfuerzo (nivel {robot.NivelHabilidad}), pero quedó más cansado.";
        }

        public string Descansar(Robot robot)
        {
            robot.Energia = Math.Min(100, robot.Energia + robot.Generador.Siguiente(10, 26));
            robot.ActualizarEstado();
            return $"{robot.Nombre} descansó. Energía actual: {robot.Energia}.";
        }
    }

    // 1 <= Energía < 20. Demasiado débil para pelear o entrenar:
    // solo le queda descansar hasta recuperarse.
    public class EstadoDanado : IEstadoRobot
    {
        public string Nombre => "Dañado";
        public bool PuedePelear => false;

        public string Entrenar(Robot robot)
        {
            return $"{robot.Nombre} está dañado y no tiene energía para entrenar. Necesita descansar.";
        }

        public string Descansar(Robot robot)
        {
            robot.Energia = Math.Min(100, robot.Energia + robot.Generador.Siguiente(10, 26));
            robot.ActualizarEstado();
            return $"{robot.Nombre} descansó para recuperarse del daño. Energía actual: {robot.Energia}.";
        }
    }

    // Energía <= 0. Quedó fuera de combate. No entrena ni pelea;
    // solo una reparación de emergencia (descansar) lo puede volver
    // a poner en carrera, y arranca de nuevo con poca energía.
    public class EstadoEliminado : IEstadoRobot
    {
        public string Nombre => "Eliminado";
        public bool PuedePelear => false;

        public string Entrenar(Robot robot)
        {
            return $"{robot.Nombre} está eliminado. No puede entrenar.";
        }

        public string Descansar(Robot robot)
        {
            robot.Energia = Math.Min(100, robot.Energia + robot.Generador.Siguiente(5, 16));
            robot.ActualizarEstado();
            return $"{robot.Nombre} recibió una reparación de emergencia. Energía actual: {robot.Energia}.";
        }
    }
}
