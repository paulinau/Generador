using System;
using System.IO;

namespace Generador
{
    class Error : Exception
    {
        public Error()
        {

        }
        public Error(StreamWriter bitacora, string error) : base(error)
        {
            //string mensaje = error + " linea "  + linea + " caracter " + caracter;
            bitacora.WriteLine(error);
        }
    }
}