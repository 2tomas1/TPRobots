using System;
using System.Collections.Generic;
using System.Text;

namespace TPRobots
{
    
    internal interface IDuelo
    {
        // Recibe al robot oponente y devuelve el robot ganador
        // del duelo 
        Robot RealizarDuelo(Robot oponente);
    }
}
