using System;
using System.Collections.Generic;
using System.Text;

// Requerimiento 1: Agregar comentarios de linea y multilinea a nivel lexico (tendriamos que modificar la matriz)
// Requerimiento 2: El proyecto se debe de llamar igual que el lenguaje (namespace C)

/*
    lenguaje -> lenguaje:identificador; { ListaProducciones }
    ListaProducciones -> snt flechita ListaSimbolos fin_produccion ListaProducciones?
    ListaSimbolos -> snt | st ListaSimbolos?
*/

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

        // lenguaje -> lenguaje:identificador; ListaProducciones
        public void gramatica()
        {
            match("lenguaje");
            match(":");

            if(getClasificacion() == clasificaciones.snt)
                match(clasificaciones.snt);
            else
                match(clasificaciones.st);
            match(";");

            Cabecera();
            match("{");
            ListaProducciones();
            match("}");
            lenguaje.WriteLine("    }");
            lenguaje.WriteLine("}");
        }

        // ListaProducciones -> snt flechita ListaSimbolos fin_produccion ListaProducciones?
        private void ListaProducciones()
        {
            lenguaje.WriteLine("        public void "+getContenido() + "()");
            match(clasificaciones.snt);
            match(clasificaciones.flechita);

            lenguaje.WriteLine("        {");
            ListaSimbolos();
            lenguaje.WriteLine("        }");

            match(clasificaciones.fin_produccion);

            if(getClasificacion() == clasificaciones.snt)
                ListaProducciones();
        }

        // ListaSimbolos -> snt | st ListaSimbolos?
        private void ListaSimbolos()
        {
            if(getClasificacion() == clasificaciones.snt)
                match(clasificaciones.snt);
            else
                match(clasificaciones.st);

            if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st)
                ListaSimbolos();
        }

        private void Cabecera()
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("using System.Text;");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("namespace Generador");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("    public class Lenguaje: Sintaxis");
            lenguaje.WriteLine("    {");
            lenguaje.WriteLine("        public Lenguaje()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            Console.WriteLine(\"Iniciando analisis gramatical.\");");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("        public Lenguaje(string nombre): base(nombre)");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            Console.WriteLine(\"Iniciando analisis gramatical.\");");
            lenguaje.WriteLine("        }");
        }

    }
}