using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtiquetadoImagenes
{
    /// <summary>
    /// Etiquetas.
    /// </summary>
    public class Etiquetas
    {
        Nodo[] nodos;

        public Nodo[] Nodos
        {
            get
            {
                return nodos;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MarshalAnalyse.Etiquetas"/> class.
        /// </summary>
        public Etiquetas()
        {
            nodos       = new Nodo[1];
            nodos[0]    = new Nodo(0);
        }

        /// <summary>
        /// Gets the <see cref="T:MarshalAnalyse.Etiquetas"/> with the specified no_etiqueta.
        /// </summary>
        /// <param name="no_etiqueta">No etiqueta.</param>
        public Nodo this[int no_etiqueta]
        {
            get
            {
                if (no_etiqueta > nodos.Length - 1)
                    return null;
                return nodos[no_etiqueta];
            }
        }

        /// <summary>
        /// Agregars the etiqueta.
        /// </summary>
        /// <param name="nodo">Nodo.</param>
        public void AgregarEtiqueta(Nodo nodo)
        {
            Array.Resize(ref nodos, nodos.Length + 1);
            nodos[nodos.Length - 1] = nodo;
        }

        public void AbsorverEtiquetas()
        {
            foreach (Nodo nodo in nodos)
                nodo.AbsorverNodos();

            List<Nodo> listar_nodos = nodos.AsEnumerable().ToList();
            listar_nodos.RemoveAll(nodo => !nodo.EsPadre);
            nodos = listar_nodos.ToArray();
        }
    }
}
