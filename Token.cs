namespace Generador
{
    class Token : Error
    {
        public enum clasificaciones
        {
            snt, st, flechita, fin_produccion, cerradura_epsilon, parentesis_izquierdo, parentesis_derecho, 
            or, corchete_izquierdo, corchete_derecho,
            /*
                snt -> L+
                st -> L+ | clasificaciones.tipo | caracter
                flechita -> ->
                cerradura_epsilon -> \?
                parentesis_derecho -> \(
                parentesis_izquierdo -> \)
                or -> \|
                corchete_izquierdo -> \[
                corchete_derecho -> \]
            */
        }
        private string contenido;
        private clasificaciones clasificacion;

        protected bool esClasificacion(string clasificacion)
        {
            switch (clasificacion)
            {
                case "identificador":
                case "numero":
                case "asignacion":
                case "inicializacion":
                case "fin_sentencia":
                case "operador_logico":
                case "operador_relacional":
                case "operador_termino":
                case "operador_factor":
                case "incremento_termino":
                case "incremento_factor":
                case "cadena":
                case "operador_ternario":
                case "caracter":
                case "tipo_dato":
                case "zona":
                case "condicion":
                case "ciclo":
                case "inicio_bloque":
                case "fin_bloque":
                case "flujo_entrada":
                case "flujo_salida":
                    return true;
            }
            return false;
        }

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