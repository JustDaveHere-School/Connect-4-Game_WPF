using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ConnectFourGUI
{
  /// <summary>
  /// Interaction logic for TieWindow.xaml
  /// </summary>
  public partial class TieWindow : Window
  {
    public TieWindow()
    {
      InitializeComponent();
      // Set the source of the image to the tie picture
      WinnerImage.Source = new BitmapImage(new Uri("C:\\Users\\david\\OneDrive - sluz\\Desktop\\D23a\\Informatik\\IPT2.1\\Connect-4 Game (WPF)\\ConnectFourGUI\\Chips\\tie.png"));
    }
  }
}
