using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtiquetadoImagenes
{
    /// <summary>
    /// Momento hu.
    /// </summary>
    public class MomentoHu
    {
        /// <summary>
        /// Gets the hu1.
        /// </summary>
        /// <value>The hu1.</value>
        public double HU_1 { get; private set; }

        /// <summary>
        /// Gets the hu2.
        /// </summary>
        /// <value>The hu2.</value>
        public double HU_2 { get; private set; }

        /// <summary>
        /// Gets the hu3.
        /// </summary>
        /// <value>The hu3.</value>
        public double HU_3 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MarshalAnalyse.MomentoHu"/> class.
        /// </summary>
        /// <param name="pixeles">Pixeles.</param>
        public MomentoHu(short[][] pixeles)
        {
            double[,] mPQ       = new double[4, 4];
            double[,] muPQ      = new double[4, 4];
            double[,] ethaPQ    = new double[4, 4];

            #region Explicación
            /*
             * El algoritmo te pide que mPQ tengan las mismas dimensiones y que se rellenen los mismos espacios.           
             * Si mPQ, muPQ y ethaPQ fueran array bidimensionales, los datos llenados serían estos:
             * 
             * mPQ - muPQ       ethaPQ
             * [X, X, X, X]     [_, _, X, X]
             * [X, X, X, _]     [_, X, X, _]
             * [X, X, _, _]     [X, X, _, _]
             * [X, _, _, _]     [X, _, _, _]        
             * 
             * 
             * Para xCentro y para yCentro, se ocuparán estos valores
             * 
             * mPQ
             * [X, X, _, _]
             * [X, _, _, _]
             * [_, _, _, _]
             * [_, _, _, _]
             * 
             * Por lo que [1, 4], [2, 3], [2, 4], [3, 1], [3, 2] y [3, 3] nunca se llenan. Esas iteracciones son innecesarias.
             * Colocando en el segundo for la condición de (q < 4 - p) quitamos esas iteracciones, y la condición de (p + q <= 3)
             * 
             * E incluso, para este ejercicio, las posiciones [0, 0], [0, 1] y [1, 0] de mPQ son las únicas que se usan, asi es que podemos excluirlas
             * sin ningun problema dentro del algoritmo.           
             * 
             * 
             */
            #endregion

            // Extrae los momentos geométricos
            ExtraeMomentosGeometricos(pixeles, mPQ);

            // Extraemos los momentos centrales de "x" y "y"
            // --¿Volver públicos?
            int xCentro = (int)(mPQ[1, 0] / mPQ[0, 0]);
            int yCentro = (int)(mPQ[0, 1] / mPQ[0, 0]);

            // Extraemos todos los momentos centrales
            ExtraeMomentosCentrales(pixeles, muPQ, xCentro, yCentro);

            // Y extraemos los momentos formales centralizados
            ExtraeMomentosCentralesNormalizados(pixeles, ethaPQ, muPQ);

            // Y finalmente sacamos los momentos
            HU_1 = ethaPQ[2, 0] + ethaPQ[0, 2];
            HU_2 = Math.Pow(ethaPQ[2, 0] - ethaPQ[0, 2], 2) + (4 * Math.Pow(ethaPQ[1, 1], 2));
            HU_3 = Math.Pow(ethaPQ[3, 0] - (3 * ethaPQ[1, 2]), 2) + Math.Pow((3 * ethaPQ[2, 1]) - ethaPQ[0, 3], 2);
        }

        /// <summary>
        /// Extraes the momentos geometricos.
        /// </summary>
        /// <param name="pixeles">Pixeles.</param>
        /// <param name="mPQ">M pq.</param>
        private void ExtraeMomentosGeometricos(short[][] pixeles, double[,] mPQ)
        {
            for (int i = 0; i < pixeles.Length; i++)
            {
                short[] pxl = pixeles[i];
                // pxl[0] = x
                // pxl[1] = y

                for (short p = 0; p < 4; p++)
                    for (short q = 0; q < 4 - p; q++)
                        mPQ[p, q] += Math.Pow(pxl[0], p) * Math.Pow(pxl[1], q);
            }
        }

        /// <summary>
        /// Extraes the momentos centrales.
        /// </summary>
        /// <param name="pixeles">Pixeles.</param>
        /// <param name="muPQ">Mu pq.</param>
        /// <param name="xCentro"></param>
        /// <param name="yCentro"></param>
        private void ExtraeMomentosCentrales(short[][] pixeles, double[,] muPQ, int xCentro, int yCentro)
        {
            for (int i = 0; i < pixeles.Length; i++)
            {
                short[] pxl = pixeles[i];
                // pxl[0] = x
                // pxl[1] = y

                for (short p = 0; p < 4; p++)
                    for (short q = 0; q < 4 - p; q++)
                        muPQ[p, q] += Math.Pow(pxl[0] - xCentro, p) * Math.Pow(pxl[1] - yCentro, q);
            }
        }

        /// <summary>
        /// Extraes the momentos centrales normalizados.
        /// </summary>
        /// <param name="pixeles">Pixeles.</param>
        /// <param name="ethaPQ">Etha pq.</param>
        /// <param name="muPQ"></param>
        private void ExtraeMomentosCentralesNormalizados(short[][] pixeles, double[,] ethaPQ, double[,] muPQ)
        {
            for (int i = 0; i < pixeles.Length; i++)
            {
                short[] pxl = pixeles[i];
                // pxl[0] = x
                // pxl[1] = y

                short intervalo = 2;
                for (int p = 0 + intervalo; p < 4; p++)
                {
                    for (int q = 0 + intervalo; q < 4 - p; q++)
                    {
                        double theta = ((p + q) / 2) + 1;
                        ethaPQ[p, q] = muPQ[p, q] / Math.Pow(muPQ[0, 0], theta);
                    }

                    if (intervalo != 0)
                        intervalo--;
                }
            }
        }
    }
}