using System;
using System.Collections.Generic;
using System.Text;

// Requerimiento 1: Agregar comentarios de linea y multilinea a nivel lexico (tendriamos que modificar la matriz)
// Requerimiento 2: El proyecto se debe de llamar igual que el lenguaje (namespace C)
// Requerimiento 3: Indentar el codigo generado (tip: hacer una funcion para escribir y recibir como parametro cuantos tabuladores)
//                  Escribe(int numeroTabs, string instruccion) cada que se habra una llave se suma uno al numero de tabs (a lo mejor manejarlo como atributo)
// Requerimiento 4: En la cerradura epsilon considerar getClasificacion y getContenido
// Requerimiento 5: Implementar el operador OR (modificar la matriz)

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
            {
                lenguaje.WriteLine("            "+getContenido()+"();");
                match(clasificaciones.snt);
            }
            else if (getClasificacion() == clasificaciones.st)
            {
                if(esClasificacion(getContenido()))
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido()+");");
                else
                    lenguaje.WriteLine("            "+"match(\""+getContenido()+"\");");
                match(clasificaciones.st);
            }
            else if(getClasificacion() == clasificaciones.parentesis_izquierdo)
            {
                match(clasificaciones.parentesis_izquierdo);
                lenguaje.WriteLine("            "+"if (getContenido() == \"" + getContenido() + "\")");
                lenguaje.WriteLine("            {");

                if(esClasificacion(getContenido()))
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido()+");");
                else
                    lenguaje.WriteLine("            "+"match(\""+getContenido()+"\");");
                match(clasificaciones.st);

                if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st)
                    ListaSimbolos();

                match(clasificaciones.parentesis_derecho);
                lenguaje.WriteLine("            }");

                match(clasificaciones.cerradura_epsilon);
            }
            else if (getClasificacion() == clasificaciones.corchete_izquierdo)
            {
                match(clasificaciones.corchete_izquierdo);
                lenguaje.WriteLine("            "+"if (getContenido() == \"" + getContenido() + "\")");
                lenguaje.WriteLine("            {");

                if(esClasificacion(getContenido()))
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido()+");");
                else
                    lenguaje.WriteLine("            "+"match(\""+getContenido()+"\");");
                match(clasificaciones.st);

                if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st)
                    ListaORs();

                match(clasificaciones.corchete_derecho);
                lenguaje.WriteLine("            }");
            }

            if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st || getClasificacion() == clasificaciones.parentesis_izquierdo)
                ListaSimbolos();
        }

        // ListaORs -> st (| ListaORs)?
        private void ListaORs()
        {
            // Generar "else ifs" y el ultimo simbolo debe ser "else"

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