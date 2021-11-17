using System;
using System.IO;

namespace Generador
{
    class Lexico : Token, IDisposable
    {
        private StreamReader archivo;
        protected StreamWriter bitacora;
        protected StreamWriter lenguaje;
        protected int linea, caracter;
        const int F = -1;
        const int E = -2;
        string nombre_archivo;
        int[,] trand =  {  //WS, L, -, >, \, ;, ?, (, ), |,LA, /, *,EF,#10
                            { 0, 1, 2,10, 4,10,10,10,10,10,10,11, 0, F, 0},
                            { F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, 3, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, 5, 6, 7, 8, 9, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},
                            { F, F, F, F, F, F, F, F, F, F, F,12,13, F, F},
                            {12,12,12,12,12,12,12,12,12,12,12,12,12, 0, 0},
                            {13,13,13,13,13,13,13,13,13,13,13,13,14, E,13},
                            {13,13,13,13,13,13,13,13,13,13,13, 0,14, E,13},
                        };

        //Declaramos nuestro constructor
        public Lexico()
        {
            linea = caracter = 1;
            Console.WriteLine("Compilando prueba.gram");
            Console.WriteLine("Iniciando analisis lexico");

            //checar si el archivo existe
            if (File.Exists("C:\\archivos\\prueba.gram"))
            {
                archivo = new StreamReader("C:\\archivos\\prueba.gram");
                bitacora = new StreamWriter("C:\\archivos\\prueba.log");
                lenguaje = new StreamWriter("C:\\archivos\\lenguaje.cs");
                bitacora.AutoFlush = true;
                lenguaje.AutoFlush = true;

                DateTime fechaActual = DateTime.Now;

                bitacora.WriteLine("Archivo: prueba.gram");  //grabamos algo en el archivo
                bitacora.WriteLine("Directorio: C:\\archivos");
                bitacora.WriteLine("Fecha: " + fechaActual.ToString("D") + " Hora: " + fechaActual.ToString("t"));
            }
            else
            {
                throw new Exception("El archivo prueba.gram no existe");
            }
        }

        //sobrecarga de constructores
        public Lexico(string nombre)
        {
            linea = caracter = 1;

            nombre_archivo = Path.GetFileName(nombre);
            Console.WriteLine("Compilando " + nombre_archivo);
            Console.WriteLine("Iniciando analisis lexico");

            string extension = Path.GetExtension(nombre);
            if (extension == ".gram")
            {
                //checar si el archivo existe
                if (File.Exists(nombre))
                {
                    archivo = new StreamReader(nombre);

                    string log = Path.ChangeExtension(nombre, ".log");
                    bitacora = new StreamWriter(log);
                    lenguaje = new StreamWriter("C:\\archivos\\lenguaje.cs");
                    bitacora.AutoFlush = true;
                    lenguaje.AutoFlush = true;

                    DateTime fechaActual = DateTime.Now;
                    string directorio = Path.GetDirectoryName(nombre);

                    bitacora.WriteLine("Archivo: " + nombre_archivo);  //grabamos algo en el archivo
                    bitacora.WriteLine("Directorio: " + directorio);
                    bitacora.WriteLine("Fecha: " + fechaActual.ToString("D") + " Hora: " + fechaActual.ToString("t"));
                }
                else
                {
                    throw new Exception("El archivo " + nombre_archivo + " no existe");
                }
            }
            else
            {
                throw new Exception("No se puede compilar " + nombre_archivo + " puesto que la extension no es .gram");
            }
        }

        //Destructor
        //~ Lexico(){
        public void Dispose()
        {
            Console.WriteLine("\nFinaliza la compilación de " + nombre_archivo);
            CerrarArchivos();   //invoca el metodo para cerrar los archivos
        }

        //Cerramos los archivos
        private void CerrarArchivos()
        {
            archivo.Close();
            bitacora.Close();
            lenguaje.Close();
        }

        public void nextToken()
        {
            char transicion;
            string palabra = "";
            int estado = 0;
            int estado_anterior = 0;

            //mientras esté en estados positivos, permanezco en el automata
            while (estado >= 0)
            {
                estado_anterior = estado;

                transicion = (char)archivo.Peek();
                estado = trand[estado, columna(transicion)];
                clasificar(estado);

                //son caracteres validos
                if (estado >= 0)
                {
                    archivo.Read();
                    caracter++;
                    //Contador de linea y caracteres
                    if (transicion == 10)
                    {
                        linea++;
                        caracter = 1;
                    }
                    if (estado > 0)
                    {
                        palabra += transicion;
                    }
                    else
                    {
                        //se limpia todo lo que se concateno
                        palabra = "";
                    }
                }
            }
            setContenido(palabra);
            
            if (estado == E)
            {
                throw new Error(bitacora, "Error lexico: Se esperaba un cierre de comentario (*/). Linea: " + linea + ", caracter:" + caracter);
            }
            else if (getClasificacion() == clasificaciones.snt)
            {
                if(!char.IsUpper(getContenido()[0]))
                {
                    setClasificacion(clasificaciones.st);
                }
            }
            if (getContenido() != "")
            {
                bitacora.WriteLine("Token = " + getContenido());
                bitacora.WriteLine("Clasificacion = " + getClasificacion());
            }
        }

        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(clasificaciones.snt);
                    break;
                case 2:
                case 4:
                case 10:
                    setClasificacion(clasificaciones.st);
                    break;
                case 3:
                    setClasificacion(clasificaciones.flechita);
                    break;
                case 5:
                    setClasificacion(clasificaciones.fin_produccion);
                    break;
                case 6:
                    setClasificacion(clasificaciones.cerradura_epsilon);
                    break;
                case 7:
                    setClasificacion(clasificaciones.parentesis_izquierdo);
                    break;
                case 8:
                    setClasificacion(clasificaciones.parentesis_derecho);
                    break;
                case 9:
                    setClasificacion(clasificaciones.or);
                    break;
                case 12:
                case 13:
                case 14:
                    break;
            }
        }

        private int columna(char t)
        {
            //WS, L, -, >, \, ;, ?, (, ), |, LA
            if(finArchivo())
            {
                return 13;
            }
            else if(t == 10)
            {
                return 14;
            }
            else if(char.IsWhiteSpace(t))
            {
                return 0;
            }
            else if(char.IsLetter(t))
            {
                return 1;
            }
            else if(t == '-')
            {
                return 2;
            }
            else if(t == '>')
            {
                return 3;
            }
            else if(t == '\\')
            {
                return 4;
            }
            else if(t == ';')
            {
                return 5;
            }
            else if(t == '?')
            {
                return 6;
            }
            else if(t == '(')
            {
                return 7;
            }
            else if(t == ')')
            {
                return 8;
            }
            else if(t == '|')
            {
                return 9;
            }
            else if(t == '/')
            {
                return 11;
            }
            else if(t == '*')
            {
                return 12;
            }
            else
            {
                return 10;
            }
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}