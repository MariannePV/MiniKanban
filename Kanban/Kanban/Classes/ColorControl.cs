using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Kanban
{
    public class ColorControl
    {
        public ColorControl() { }

        public Color SeleccionarColorFinestra() //Obrim una finestra propia de windows per mostrar els colors. Retornem el color seleccionat
        {
            ColorDialog colorDialog = new ColorDialog();
            Color colorWpf = new Color();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color colorSeleccionado = colorDialog.Color;
                colorWpf = Color.FromArgb(colorSeleccionado.A, colorSeleccionado.R, colorSeleccionado.G, colorSeleccionado.B);
             
            }
            return colorWpf;
        }
        public string ConvertirColorAString(Color color) //Funció per pasar un color de tipus Color a String
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public Color ConvertitStringAColor(string colorString) //Funció per pasar un color de tipus String a Color
        {          
            Color color = (Color)ColorConverter.ConvertFromString(colorString);
            return color;
        }

        public string ConvertirColorBrushAString(SolidColorBrush colorBrush) //Funció per pasar un SolidColorBrush a String
        {
            Color color = colorBrush.Color;

            return ConvertirColorAString(color);

        }

        public SolidColorBrush ConvertirStringAColorBrush(string colorString) //Funció per pasar de String a SolidColorBrush
        {
            Color color = ConvertitStringAColor(colorString);

            return new SolidColorBrush(color);
        }

    }
}
