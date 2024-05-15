using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ConnectFourGUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    const int circleSize = 80;

    private ConnectFourBoard board;
    private DispatcherTimer animationTimer;
    private bool inputLock;

    private Side currentSide;
    private System.Windows.Controls.Image currentChip;
    private int currentColumn;

    private BitmapImage blueChipImage;
    private BitmapImage yellowChipImage;

    public MainWindow()
    {
      InitializeComponent();
      NewGame();
      //blueChipImage = Properties.Resources.blue_chip;
      blueChipImage = ConvertBitmapToBitmapImage(Resource1.blue_chip);
      //yellowChipImage = Properties.Resources.yellow_chip;
      yellowChipImage = ConvertBitmapToBitmapImage(Resource1.yellow_chip);
    }

    public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
    {
      using (MemoryStream memory = new MemoryStream())
      {
        bitmap.Save(memory, ImageFormat.Png);
        memory.Position = 0;
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
      }
    }
    private void NewGame()
    {
      inputLock = true;
      board = new ConnectFourBoard(6, 7);
      currentSide = Side.Yellow;
      animationTimer = new DispatcherTimer();
      animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
      animationTimer.Start();
      GameCanvas.Children.Clear();
      DrawBackground();
      inputLock = false;
      EnableAllInsertButtons();
    }

    private void InsertButton_Click(int column)
    {
      if (inputLock == false)
      {
        bool success = board.Insert(currentSide, column);
        if (success)
        {
          currentColumn = column;
          DrawChip(currentSide, column);
          AfterTurn();
        }
      }

    }

    #region InsertButton_Click Methods
    private void InsertButton0_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(0);
    }

    private void InsertButton1_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(1);
    }

    private void InsertButton2_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(2);
    }

    private void InsertButton3_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(3);
    }

    private void InsertButton4_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(4);
    }

    private void InsertButton5_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(5);
    }

    private void InsertButton6_Click(object sender, RoutedEventArgs e)
    {
      InsertButton_Click(6);
    }
    #endregion

    private void DrawBackground()
    {
      for (int row = 0; row < board.GameBoard.GetLength(0); row++)
      {
        for (int column = 0; column < board.GameBoard.GetLength(1); column++)
        {
          System.Windows.Shapes.Rectangle square = new System.Windows.Shapes.Rectangle();
          square.Height = circleSize;
          square.Width = circleSize;
          square.Fill = (column % 2 == 0) ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightGray;
          Canvas.SetBottom(square, circleSize * row);
          Canvas.SetRight(square, circleSize * column);
          GameCanvas.Children.Add(square);
        }
      }
    }

    private void DrawChip(Side side, int col)
    {
      inputLock = true;

      System.Windows.Controls.Image chip = new System.Windows.Controls.Image();
      chip.Height = circleSize;
      chip.Width = circleSize;
      chip.Stretch = Stretch.Fill;

      // Set the source of the image based on the side
      if (side == Side.Yellow)
      {
        chip.Source = yellowChipImage;
      }
      else
      {
        chip.Source = blueChipImage;
      }

      Canvas.SetTop(chip, 0);
      Canvas.SetLeft(chip, col * circleSize);
      GameCanvas.Children.Add(chip);
      currentChip = chip;
      animationTimer.Tick += DropChipAnimation;
    }

    private void DropChipAnimation(object sender, EventArgs e)
    {
      int dropLength = circleSize * (board.GameBoard.GetLength(1) - 1 - board.PiecesInCol(currentColumn));
      int dropRate = 40;
      if (Canvas.GetTop(currentChip) < dropLength)
      {
        Canvas.SetTop(currentChip, Canvas.GetTop(currentChip) + dropRate);
      }
      else
      {
        animationTimer.Tick -= DropChipAnimation;
        inputLock = false;
      }

    }

    private void AfterTurn()
    {
      Side winner = board.Winner();

      if (winner != Side.None)
      {
        StatusText.Text = String.Format("{0} player Wins!", currentSide);
        DisableAllInsertButtons();
      }
      else if (board.Tied())
      {
        StatusText.Text = "Tied game!";
        DisableAllInsertButtons();
      }
      else
      {
        currentSide = (currentSide == Side.Blue) ? Side.Yellow : Side.Blue;
        StatusText.Text = String.Format("{0}'s Turn", currentSide);
      }
    }

    private void DisableAllInsertButtons()
    {
      InsertButton0.IsEnabled = false;
      InsertButton1.IsEnabled = false;
      InsertButton2.IsEnabled = false;
      InsertButton3.IsEnabled = false;
      InsertButton4.IsEnabled = false;
      InsertButton5.IsEnabled = false;
      InsertButton6.IsEnabled = false;
    }

    private void EnableAllInsertButtons()
    {
      InsertButton0.IsEnabled = true;
      InsertButton1.IsEnabled = true;
      InsertButton2.IsEnabled = true;
      InsertButton3.IsEnabled = true;
      InsertButton4.IsEnabled = true;
      InsertButton5.IsEnabled = true;
      InsertButton6.IsEnabled = true;
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
      NewGame();
    }
  }
}
