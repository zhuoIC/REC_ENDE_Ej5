using System.Windows;

namespace REC_ENDE_Ej5
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window unaVentana = new Registro();
            this.Close();
            unaVentana.Show();
        }
    }
}
