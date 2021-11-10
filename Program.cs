using System;

namespace Generador
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje l = new Lenguaje("C:\\archivos\\c.gram"))
                {
                    //instanciamos nuestra clase
                    /*
                    while(!l.finArchivo())
                    {
                        l.nextToken();
                    }*/
                    l.gramatica();
                    //l.Programa();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            Console.ReadKey();
        }
    }
}
