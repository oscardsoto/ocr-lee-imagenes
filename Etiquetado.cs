using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtiquetadoImagenes
{

    public static class Etiquetado
    {
        public static Etiquetas EtiquetarImagen(short[] img_binarizada, int img_largo)
        {
            short valor_pos, etiquetado = 0;

            List<short> conexiones;
            Etiquetas etiquetas = new Etiquetas();
            short[] pixel       = new short[2];

            for (int i = 0; i < img_binarizada.Length; i++)
            {
                valor_pos   = short.MaxValue;
                conexiones  = new List<short>();

                if (img_binarizada[i] == 0)
                    continue;

                /* 
                 * Condiciones para los pixeles vecinos:
                 * [X, X, X]
                 * [X, P, _]
                 *                             Izq       Arriba-Izq        Arriba        Arriba-Der        */
                int[] posiciones = new int[] { i - 1, i - img_largo - 1, i - img_largo, i - img_largo + 1 };
                foreach (int pos in posiciones)
                {
                    if (pos < 0)
                        continue;

                    short valor_array = img_binarizada[pos];
                    if (valor_array == 0)
                        continue;

                    if (valor_pos > valor_array)
                        valor_pos = valor_array;
                    if (!conexiones.Contains(valor_array))
                        conexiones.Add(valor_array);
                }

                /* pixel[0] = x
                 * pixel[1] = y
                 */
                pixel[0] = (short)(i % img_largo);
                pixel[1] = (short)(i / img_largo);

                // Si la posiciòn no tiene vecinos
                if (valor_pos == short.MaxValue)
                {
                    etiquetado++;
                    Nodo nuevo = new Nodo(etiquetado);
                    nuevo.AgregarPixel(pixel);

                    etiquetas.AgregarEtiqueta(nuevo);
                    img_binarizada[i] = etiquetado;
                    continue;
                }

                // En caso de que los tenga
                img_binarizada[i] = valor_pos;
                etiquetas[valor_pos].AgregarPixel(pixel);

                if (conexiones.Count != 1)
                    continue;

                foreach (short conx in conexiones)
                {
                    if (conx <= valor_pos)
                        continue;

                    Nodo en_posicion = etiquetas[valor_pos];
                    Nodo a_conectar = etiquetas[conx];

                    while (!en_posicion.EsPadre)
                        en_posicion = en_posicion.Raiz;

                    while (!a_conectar.EsPadre)
                        a_conectar = a_conectar.Raiz;

                    if (en_posicion.Valor == a_conectar.Valor)
                        continue;

                    a_conectar.Raiz = en_posicion;
                    en_posicion.AgregarNodo(a_conectar);
                }
            }

            return etiquetas;
        }
    }
}
