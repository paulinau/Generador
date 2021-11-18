using System;
using System.Collections.Generic;
using System.Text;

// ✿
// ✿   Requerimiento 1: Agregar comentarios de linea y multilinea a nivel lexico (tendriamos que modificar la matriz)
// ✿   Requerimiento 2: El proyecto se debe de llamar igual que el lenguaje (namespace C)
// ✿   Requerimiento 3: Indentar el codigo generado (tip: hacer una funcion para escribir y recibir como parametro cuantos tabuladores)
//                  Escribe(int numeroTabs, string instruccion) cada que se habra una llave se suma uno al numero de tabs (a lo mejor manejarlo como atributo)
// ✿   Requerimiento 4: En la cerradura epsilon considerar getClasificacion y getContenido
// Requerimiento 5: Implementar el operador OR (modificar la matriz) (lista de simbolos o una lista de simbolos), se pone entre corchetes
//                  agregar corchete derecho e izquierdo en la matriz ListaORs tiene simbolos terminales separados por comas

/*
    lenguaje -> lenguaje:identificador; { ListaProducciones }
    ListaProducciones -> snt flechita ListaSimbolos fin_produccion ListaProducciones?
    ListaSimbolos -> snt | st ListaSimbolos?
    ListaORs -> st (| ListaORs)?
*/

namespace Generador
{
    class Lenguaje : Sintaxis
    {
        private int num_tabuladores;
        public Lenguaje()
        {
            num_tabuladores = 0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }

        public Lenguaje(string nombre) : base(nombre)
        {
            num_tabuladores = 0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }

        // lenguaje -> lenguaje:identificador; ListaProducciones
        public void gramatica()
        {
            match("lenguaje");
            match(":");
            string nombre_namespace = getContenido();

            if(getClasificacion() == clasificaciones.snt)
                match(clasificaciones.snt);
            else
                match(clasificaciones.st);
            match(";");

            Cabecera(nombre_namespace);
            match("{");
            ListaProducciones();
            match("}");
            Escribe("}");
            Escribe("}");
        }

        // ListaProducciones -> snt flechita ListaSimbolos fin_produccion ListaProducciones?
        private void ListaProducciones()
        {
            Escribe("public void "+getContenido() + "()");
            match(clasificaciones.snt);
            match(clasificaciones.flechita);

            Escribe("{");
            ListaSimbolos();
            Escribe("}");

            match(clasificaciones.fin_produccion);

            if(getClasificacion() == clasificaciones.snt)
                ListaProducciones();
        }

        // ListaSimbolos -> snt | st ListaSimbolos?
        private void ListaSimbolos()
        {
            if(getClasificacion() == clasificaciones.snt)
            {
                Escribe(getContenido()+"();");
                match(clasificaciones.snt);
            }
            else if (getClasificacion() == clasificaciones.st)
            {
                if(esClasificacion(getContenido()))
                    Escribe("match(clasificaciones."+getContenido()+");");
                else
                    Escribe("match(\""+getContenido()+"\");");
                match(clasificaciones.st);
            }
            else if(getClasificacion() == clasificaciones.parentesis_izquierdo)
            {
                match(clasificaciones.parentesis_izquierdo);
                If();

                if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st)
                    ListaSimbolos();

                match(clasificaciones.parentesis_derecho);
                Escribe("}");

                match(clasificaciones.cerradura_epsilon);
            }
            else if (getClasificacion() == clasificaciones.corchete_izquierdo)
            {
                match(clasificaciones.corchete_izquierdo);
                If();

                if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st)
                    ListaORs();

                match(clasificaciones.corchete_derecho);
                Escribe("}");
            }

            if(getClasificacion() == clasificaciones.snt | getClasificacion() == clasificaciones.st || getClasificacion() == clasificaciones.parentesis_izquierdo)
                ListaSimbolos();
        }

        // ListaORs -> st (| ListaORs)?
        private void ListaORs()
        {
            // if(hola == 1 || queso == webos) este o este
            // if(hola == 1 || queso == 6 || numero == 2 || pollo == frito) este o este o este o este
            // generar los ORs
            // Generar "else ifs" y el ultimo simbolo debe ser "else"
            if(getClasificacion() == clasificaciones.st)
            {
                If();
            }
            if(getClasificacion() == clasificaciones.or)
            {
                Escribe("else");
                ListaORs();
            }
        }

        private void Cabecera(string nombre_namespace)
        {
            Escribe("using System;");
            Escribe("using System.Collections.Generic;");
            Escribe("using System.Text;");
            Escribe("");
            Escribe("namespace "+nombre_namespace);
            Escribe("{");
            Escribe("public class Lenguaje: Sintaxis");
            Escribe("{");
            Escribe("public Lenguaje()");
            Escribe("{");
            Escribe("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            Escribe("}");
            Escribe("");
            Escribe("public Lenguaje(string nombre): base(nombre)");
            Escribe("{");
            Escribe("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            Escribe("}");
        }

        private void Escribe(string instruccion)
        {
            if(instruccion == "}")
                num_tabuladores--;

            for(int i=0; i<num_tabuladores; i++)
                lenguaje.Write("\t");

            if(instruccion == "{")
                num_tabuladores++;

            lenguaje.WriteLine(instruccion);
        }

        private void If()
        {
            if(esClasificacion(getContenido()))
                Escribe("if (getClasificacion() == clasificaciones." + getContenido() + ")");
            else 
                Escribe("if (getContenido() == \"" + getContenido() + "\")");
            Escribe("{");

            if(esClasificacion(getContenido()))
                Escribe("match(clasificaciones."+getContenido()+");");
            else
                Escribe("match(\""+getContenido()+"\");");
            match(clasificaciones.st);
        }
    }
}