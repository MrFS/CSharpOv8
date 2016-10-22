using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSharpOv8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "C# Øving 8 - Joachim F. Stamnes";

            Commands.Om.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            Commands.EndreFarge.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            Commands.VisSmiley.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            Commands.VisHus.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));
            Commands.VisBat.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(Commands.Om, Om_Click));
            CommandBindings.Add(new CommandBinding(Commands.EndreFarge, Velg_Farge));
            CommandBindings.Add(new CommandBinding(Commands.VisSmiley, TegnSmiley));
            CommandBindings.Add(new CommandBinding(Commands.VisHus, TegnHus));
            CommandBindings.Add(new CommandBinding(Commands.VisBat, TegnBat));

            this.DataContext = Farge.Farger;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            string msg = "Vil du virkelig avslutte?";
            MessageBoxResult result =
                MessageBox.Show(
                msg,
                Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                // Avbryt avslutning
                e.Cancel = true;
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Om_Click(object sender, RoutedEventArgs e)
        {
            string msg = "C# Øving 8 \nLaget av Joachim F. Stamnes 2BADR IIE NTNU\n" +
                         "Grafiske elementer (båt, hus) laget i illustrator og eksportert til XAML\n" + 
                         "Snarveier fungerer og ved tegning av båt får brukeren velge mellom heldekkende eller gradient,\n" + 
                         "ved tegning av hus får bruker velge mellom et eget bilde eller heldekkende farge\nFarge velges under Tegn->Velg Farge (Ctrl+F)";
            MessageBox.Show(msg, "Om Øving 8", MessageBoxButton.OK);
        }

        private void Velg_Farge(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dg = new System.Windows.Forms.ColorDialog();

            if (dg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Farge.Farger = Color.FromArgb(dg.Color.A, dg.Color.R, dg.Color.G, dg.Color.B);
            }
        }

        private void TegnSmiley(object sender, RoutedEventArgs e)
        {
            if (BatCanvas.Visibility == Visibility.Visible || HusCanvas.Visibility == Visibility.Visible)
            {
                BatCanvas.Visibility = Visibility.Hidden;
                HusCanvas.Visibility = Visibility.Hidden;
            }

            Smiley.Fill = new SolidColorBrush(Farge.Farger);

            SmileyCanvas.Visibility = Visibility.Visible;
        }

        private void TegnHus(object sender, RoutedEventArgs e)
        {
            if (SmileyCanvas.Visibility == Visibility.Visible || BatCanvas.Visibility == Visibility.Visible)
            {
                SmileyCanvas.Visibility = Visibility.Hidden;
                BatCanvas.Visibility = Visibility.Hidden;
            }

            MessageBoxResult results = MessageBox.Show("Vil du bruke en bildefil som 'farge' isteden?",
                                                       "Åpne bildefil?",
                                                       MessageBoxButton.YesNo);
            if (results == MessageBoxResult.Yes)
            {
                ImageBrush imgBr = new ImageBrush();
                Stream bild = null;

                System.Windows.Forms.OpenFileDialog fdg = new System.Windows.Forms.OpenFileDialog();

                fdg.Filter = "Bildefiler (*.jpg)|*.jpg|Alle Filer (*.*)|*.*";
                fdg.FilterIndex = 1;

                if (fdg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if ((bild = fdg.OpenFile()) != null)
                        {
                            imgBr.ImageSource = new BitmapImage((new Uri(fdg.FileName, UriKind.Relative)));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally { bild.Dispose(); }
                }

                HusVegg.Fill = imgBr;
                HusCanvas.Visibility = Visibility.Visible;
            }
            else
            {
                HusVegg.Fill = new SolidColorBrush(Farge.Farger);

                HusCanvas.Visibility = Visibility.Visible;
            }
        }

        private void TegnBat(object sender, RoutedEventArgs e)
        {
            if (SmileyCanvas.Visibility == Visibility.Visible || HusCanvas.Visibility == Visibility.Visible)
            {
                SmileyCanvas.Visibility = Visibility.Hidden;
                HusCanvas.Visibility = Visibility.Hidden;
            }

            MessageBoxResult results = MessageBox.Show("Båt med gradient (tar utgangspunkt i fargen du har valgt)?",
                                                       "Vil du ha gradient på?",
                                                       MessageBoxButton.YesNo);
            if (results == MessageBoxResult.Yes)
            {
                Random rnd = new Random();

                RadialGradientBrush rd = new RadialGradientBrush();

                rd.GradientStops.Add(new GradientStop(Farge.Farger, rnd.NextDouble()));
                rd.GradientStops.Add(new GradientStop(Color.FromArgb((byte)rnd.Next(0, 255),
                                                                     (byte)rnd.Next(0, 255),
                                                                     (byte)rnd.Next(0, 255),
                                                                     (byte)rnd.Next(0, 255)), rnd.NextDouble()));

                BatBauge.Fill = rd;
                HSeil.Fill = rd;
                VSeil.Fill = rd;
            }
            else
            {
                BatBauge.Fill = new SolidColorBrush(Farge.Farger);
                HSeil.Fill = new SolidColorBrush(Farge.Farger);
                VSeil.Fill = new SolidColorBrush(Farge.Farger);
            }

            BatCanvas.Visibility = Visibility.Visible;
        }
    }
}