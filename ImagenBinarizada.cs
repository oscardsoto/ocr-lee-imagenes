using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace EtiquetadoImagenes
{
    public class ImagenBinarizada
    {
        float RED_CONST = 0.3f,
              GRN_CONST = 0.59f,
              BLU_CONST = 0.11f;

        private BitmapData data { get; set; }
        private Bitmap img_rgb { get; set; }

        /// <summary>
        /// Array unidireccional que representa la imagen binarizada con 0 y 1.
        /// </summary>
        public short[] BinariosObjetos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public float Umbral { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="umbral"></param>
        /// <param name="binariza"></param>
        public ImagenBinarizada(Image image, float umbral, bool binarizar = false)
        {
            Umbral                  = umbral;
            img_rgb                 = new Bitmap(image);
            Rectangle img_contenido = new Rectangle(0, 0, img_rgb.Width, img_rgb.Height);
            data                    = img_rgb.LockBits(img_contenido, ImageLockMode.ReadWrite, img_rgb.PixelFormat);
            BinariosObjetos         = new short[img_rgb.Width * img_rgb.Height];

            if (binarizar)
                Binarizar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invertir"></param>
        /// <returns></returns>
        public Bitmap Binarizar(bool invertir = false)
        {
            IntPtr puntero      = data.Scan0;
            int bytes           = data.Stride * data.Height;
            byte[] rgb_valores  = new byte[bytes];

            Marshal.Copy(puntero, rgb_valores, 0, bytes);

            int iterador = 0;
            for (int i = 0; i < bytes; i += 4)
            {
                float rgb = (rgb_valores[i] * RED_CONST) + 
                            (rgb_valores[i + 1] * GRN_CONST) + 
                            (rgb_valores[i + 2] * BLU_CONST);

                if ((rgb > Umbral && invertir) || (rgb < Umbral && !invertir))
                {
                    BinariosObjetos[iterador] = 0;
                    rgb_valores[i] = rgb_valores[i + 1] = rgb_valores[i + 2] = 0;
                    iterador++;
                    continue;
                }

                BinariosObjetos[iterador] = 1;
                rgb_valores[i] = rgb_valores[i + 1] = rgb_valores[i + 2] = 255;
                iterador++;
            }

            Marshal.Copy(rgb_valores, 0, puntero, bytes);
            img_rgb.UnlockBits(data);
            return img_rgb;
        }
    }
}
