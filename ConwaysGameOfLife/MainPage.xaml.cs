using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace ConwaysGameOfLife
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string start = "Start", stop = "Stop";

        private volatile bool isUpdatingAnyway;
        private Grid grid;
        private DispatcherTimer timer;

        private int genCount;
        private System.Diagnostics.Stopwatch sw;

        public MainPage()
        {
            grid = new Grid(50, 50);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / 10.0);
            timer.Tick += Timer_Tick;

            this.InitializeComponent();

            DataContext = grid;
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (btnStartStop.Content as string == start) EnableTimer();
            else DisableTimer();
        }

        private void EnableTimer()
        {
            genCount = 0;
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            timer.Start();
            btnStartStop.Content = stop;
        }

        private void DisableTimer()
        {
            sw.Stop();

            timer.Stop();
            btnStartStop.Content = start;

            string message = string.Format("Count: {0,4}; Time: {1}\nAvg: {2} Gen/s",
                genCount, sw.Elapsed.TotalSeconds, genCount / sw.Elapsed.TotalSeconds);
            new Windows.UI.Popups.MessageDialog(message).ShowAsync();
        }

        private void BtnStep_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            btnStartStop.Content = start;

            grid.SetNextGeneration();
            ccDraw.Invalidate();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            grid.Clear();
            ccDraw.Invalidate();
        }

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid = await Grid.Load();
                DataContext = grid;

                grid.SetCellSize(slvGrid.RenderSize);

                ccDraw.Width = slvGrid.ActualWidth;
                ccDraw.Height = slvGrid.ActualHeight;

                slvGrid.ZoomToFactor(1);
                slvGrid.ScrollToHorizontalOffset(0);
                slvGrid.ScrollToVerticalOffset(0);

                ccDraw.Invalidate();
            }
            catch { }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            grid.Save();
        }

        private void ccDraw_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            grid.Draw(args.DrawingSession);
        }

        private void SldTimer_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000 / e.NewValue);
        }

        private void SldColumns_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            grid.Columns = Convert.ToInt32(e.NewValue);

            if (ccDraw == null) return;
            ccDraw.Invalidate();
        }

        private void SldRows_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            grid.Rows = Convert.ToInt32(e.NewValue);

            if (ccDraw == null) return;
            ccDraw.Invalidate();
        }

        private void Timer_Tick(object sender, object e)
        {
            grid.SetNextGeneration();
            genCount++;

            if (!isUpdatingAnyway) ccDraw.Invalidate();
        }

        private void ccEmpty_Tapped(object sender, TappedRoutedEventArgs e)
        {
            grid.Toggle(e.GetPosition(sender as UIElement));

            ccDraw.Invalidate();
        }

        private void slvGrid_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            grid.SetOffsetsAndZoom(e.NextView.HorizontalOffset,
                e.NextView.VerticalOffset, e.NextView.ZoomFactor);

            ccDraw.Invalidate();
        }

        private void CbxShowGrid_Checked(object sender, RoutedEventArgs e)
        {
            grid.ShowGrid = true;
            ccDraw.Invalidate();
        }

        private void CbxShowGrid_Unchecked(object sender, RoutedEventArgs e)
        {
            grid.ShowGrid = false;
            ccDraw.Invalidate();
        }

        private void CbxWithCellMargin_Checked(object sender, RoutedEventArgs e)
        {
            grid.WithCellMargin = true;
            ccDraw.Invalidate();
        }

        private void CbxWithCellMargin_Unchecked(object sender, RoutedEventArgs e)
        {
            grid.WithCellMargin = false;
            ccDraw.Invalidate();
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            grid.SetCellsRandom();
            ccDraw.Invalidate();
        }

        private void slvGrid_DirectManipulationStarted(object sender, object e)
        {
            isUpdatingAnyway = true;
        }

        private void slvGrid_DirectManipulationCompleted(object sender, object e)
        {
            isUpdatingAnyway = false;
        }

        private void UpdateView(object sender, RoutedEventArgs e)
        {
            ccDraw.Invalidate();
        }

        private void slvGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.SetCellSize(e.NewSize);

            ccEmpty.Width = grid.Width;
            ccEmpty.Height = grid.Height;
            ccDraw.Invalidate();
        }
    }
}
