using System;
using System.Collections.Generic;
using System.Text;

namespace Generador
{
    class Lenguaje : Sintaxis
    {
        public Lenguaje()
        {
            Console.WriteLine("Iniciando analisis gramatical.");
        }

        public Lenguaje(string nombre) : base(nombre)
        {
            Console.WriteLine("Iniciando analisis gramatical.");
        }

    }
}