using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Mag3DView.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var openGLControl = new OpenGLControl();
            Content = openGLControl; // Set OpenGLControl as the content of the main window
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
