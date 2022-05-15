using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtiquetadoImagenes
{
    /// <summary>
    /// Nodo.
    /// </summary>
    public class Nodo
    {
        /// <summary>
        /// Gets the valor.
        /// </summary>
        /// <value>The valor.</value>
        public short Valor { get; private set; }

        /// <summary>
        /// Gets the nodos.
        /// </summary>
        /// <value>The nodos.</value>
        public Queue<Nodo> Nodos { get; private set; }

        /// <summary>
        /// Gets or sets the raiz.
        /// </summary>
        /// <value>The raiz.</value>
        public Nodo Raiz { get; set; }

        /// <summary>
        /// Gets the pixeles.
        /// </summary>
        /// <value>The pixeles.</value>
        public Queue<short[]> Pixeles { get; private set; }

        /// <summary>
        /// Gets the momentos.
        /// </summary>
        /// <value>The momentos.</value>
        public MomentoHu Momentos { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MarshalAnalyse.Nodo"/> es padre.
        /// </summary>
        /// <value><c>true</c> if es padre; otherwise, <c>false</c>.</value>
        public bool EsPadre
        {
            get
            {
                return Raiz == null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MarshalAnalyse.Nodo"/> class.
        /// </summary>
        /// <param name="valor">Valor.</param>
        public Nodo(short valor)
        {
            Valor      = valor;
            Nodos      = new Queue<Nodo>();
            Pixeles    = new Queue<short[]>();
        }

        /// <summary>
        /// Absorvers the nodos.
        /// </summary>
        public void AbsorverNodos()
        {
            if (!EsPadre)
                return;

            // Seleccionamos todos los sub-nodos dentro de éste
            Queue<Nodo> nodos_seleccionado = Nodos;
            while (nodos_seleccionado.Count != 0)
            {
                // Extraemos de uno en uno
                Nodo nd_slc = nodos_seleccionado.Dequeue();

                // Y anexamos todos sus pixeles, uno por uno
                while (nd_slc.Pixeles.Count != 0)
                    AgregarPixel(nd_slc.Pixeles.Dequeue());

                // Si el nodo extraido es padre de otros nodos, agregamos cada uno de ellos a espera
                while (nd_slc.Nodos.Count != 0)
                    nodos_seleccionado.Enqueue(nd_slc.Nodos.Dequeue());
            }
        }

        /// <summary>
        /// Agregars the pixel.
        /// </summary>
        /// <param name="pxl">Pxl.</param>
        public void AgregarPixel(short[] pxl)
        {
            // La condición está de más, pero más vale prevenir
            if (pxl.Length == 2)
                Pixeles.Enqueue(pxl);
        }

        /// <summary>
        /// Agregars the nodo.
        /// </summary>
        /// <param name="nodo">Nodo.</param>
        public void AgregarNodo(Nodo nodo)
        {
            Nodos.Enqueue(nodo);
        }

        /// <summary>
        /// Obtens the momentos.
        /// </summary>
        /// <returns>The momentos.</returns>
        public MomentoHu ObtenMomentos()
        {
            if (Pixeles.Count == 0)
                return null;

            Momentos = new MomentoHu(Pixeles.ToArray());
            return Momentos;
        }
    }
}
