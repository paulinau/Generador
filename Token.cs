namespace Generador
{
    class Token : Error
    {
        public enum clasificaciones
        {
            snt, st, flechita, fin_produccion, cerradura_epsilon, parentesis_derecho, parentesis_izquierdo,
            or, 
            /*
                snt -> L+
                st -> L+ | clasificaciones.tipo | caracter
                flechita -> ->
                cerradura_epsilon -> \?
                parentesis_derecho -> \(
                parentesis_izquierdo -> \)
                or -> \|
            */
        }
        private string contenido;
        private clasificaciones clasificacion;

        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }

        public void setClasificacion(clasificaciones clasificacion)
        {
            this.clasificacion = clasificacion;
        }

        public string getContenido()
        {
            return contenido;
        }

        public clasificaciones getClasificacion()
        {
            return clasificacion;
        }
    }
}