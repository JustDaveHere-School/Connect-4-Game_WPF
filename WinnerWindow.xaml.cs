using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ConnectFourGUI
{
  public partial class WinnerWindow : Window
  {
    public WinnerWindow()
    {
      InitializeComponent();
    }

    public void SetWinnerImage(string imagePath)
    {
      WinnerImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
    }
  }
}
