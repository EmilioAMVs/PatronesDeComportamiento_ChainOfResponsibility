using System;
using System.Collections.Generic;

namespace PatronesDeDiseño.ChainOfResponsibility.Conceptual
{
    // La interfaz Manejador declara un método para construir la cadena de
    // manejadores. También declara un método para ejecutar una solicitud.
    public interface IManejador
    {
        IManejador EstablecerSiguiente(IManejador manejador);

        object Manejar(object solicitud);
    }

    // El comportamiento de encadenamiento predeterminado se puede implementar dentro de una clase de manejador base.
    abstract class ManejadorAbstracto : IManejador
    {
        private IManejador _siguienteManejador;

        public IManejador EstablecerSiguiente(IManejador manejador)
        {
            this._siguienteManejador = manejador;

            // Devolver un manejador desde aquí nos permitirá vincular manejadores de una manera conveniente como esta:
            // mono.EstablecerSiguiente(ardilla).EstablecerSiguiente(perro);
            return manejador;
        }

        public virtual object Manejar(object solicitud)
        {
            if (this._siguienteManejador != null)
            {
                return this._siguienteManejador.Manejar(solicitud);
            }
            else
            {
                return null;
            }
        }
    }

    class ManejadorMono : ManejadorAbstracto
    {
        public override object Manejar(object solicitud)
        {
            if ((solicitud as string) == "Banana")
            {
                return $"Mono: Me comeré la {solicitud.ToString()}.\n\n";
            }
            else
            {
                return base.Manejar(solicitud);
            }
        }
    }

    class ManejadorArdilla : ManejadorAbstracto
    {
        public override object Manejar(object solicitud)
        {
            if (solicitud.ToString() == "Nuez")
            {
                return $"Ardilla: Me comeré la {solicitud.ToString()}.\n\n";
            }
            else
            {
                return base.Manejar(solicitud);
            }
        }
    }

    class ManejadorPerro : ManejadorAbstracto
    {
        public override object Manejar(object solicitud)
        {
            if (solicitud.ToString() == "Albóndiga")
            {
                return $"Perro: Me comeré la {solicitud.ToString()}.\n\n";
            }
            else
            {
                return base.Manejar(solicitud);
            }
        }
    }
    class Cliente
    {
        // El código del cliente suele estar adaptado para trabajar con un solo manejador\.
        // En la mayoría de los casos, ni siquiera es consciente de que el manejador forma parte de una cadena\.

        public static void CodigoCliente(ManejadorAbstracto manejador)
        {
            foreach (var comida in new List<string> { "Nuez", "Banana", "Taza de café" })
            {
                Console.WriteLine($"Cliente: ¿Quién quiere una {comida}?");

                var resultado = manejador.Manejar(comida);

                if (resultado != null)
                {
                    Console.Write($"{resultado}");
                }
                else
                {
                    Console.WriteLine($"{comida} se quedó sin comer.\n");
                }
            }
        }
    }

    class Programa
    {
        static void Main(string[] args)
        {
            // La otra parte del código del cliente construye la cadena real.
            var mono = new ManejadorMono();
            var ardilla = new ManejadorArdilla();
            var perro = new ManejadorPerro();

            mono.EstablecerSiguiente(ardilla).EstablecerSiguiente(perro);

            // El cliente debe poder enviar una solicitud a cualquier manejador, no
            // solo al primero de la cadena.
            Console.WriteLine("Cadena: Mono > Ardilla > Perro\n");
            Cliente.CodigoCliente(ardilla);
        }
    }
}