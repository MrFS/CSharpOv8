using System.Windows.Media;

namespace CSharpOv8
{
    //public class Farge : INotifyPropertyChanged
    public static class Farge
    {
        private static Color _color = Color.FromArgb(100, 0, 255, 255);

        public static Color Farger
        {
            get { return _color; }
            set { _color = Color.FromArgb(value.A, value.R, value.G, value.B); }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}