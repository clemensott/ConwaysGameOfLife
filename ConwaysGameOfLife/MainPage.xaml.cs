using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace ConwaysGameOfLife
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string start = "Start", pause = "Pause";

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

            abbPlayPause.Label = start;
            abbPlayPause.Icon = new SymbolIcon(Symbol.Play);
        }

        private async void AbbPlayPause_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton abb = (AppBarButton)sender;

            if (abb.Label == start) EnableTimer();
            else await DisableTimer();
        }

        private void EnableTimer()
        {
            genCount = 0;
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            timer.Start();
            abbPlayPause.Label = pause;
            abbPlayPause.Icon = new SymbolIcon(Symbol.Pause);
        }

        private async Task DisableTimer()
        {
            sw.Stop();

            timer.Stop();
            abbPlayPause.Label = start;
            abbPlayPause.Icon = new SymbolIcon(Symbol.Play);

            string message = string.Format("Count: {0,4}; Time: {1}\nAvg: {2} Gen/s",
                genCount, sw.Elapsed.TotalSeconds, genCount / sw.Elapsed.TotalSeconds);
            await new Windows.UI.Popups.MessageDialog(message).ShowAsync();
        }

        private void AbbNextStep_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            abbPlayPause.Label = start;

            grid.SetNextGeneration();
            ccDraw.Invalidate();
        }

        private void AbbClear_Click(object sender, RoutedEventArgs e)
        {
            grid.Clear();
            ccDraw.Invalidate();
        }

        private async void AbbLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid = await Grid.Load();
                DataContext = grid;

                grid.SetCellSize(slvGrid.RenderSize);

                ccDraw.Width = slvGrid.ActualWidth;
                ccDraw.Height = slvGrid.ActualHeight;

                sldColumns.Value = grid.Columns;
                sldRows.Value = grid.Rows;

                slvGrid.ChangeView(0, 0, 1);

                ccDraw.Invalidate();
            }
            catch { }
        }

        private async void AbbSave_Click(object sender, RoutedEventArgs e)
        {
            await grid.Save();
        }

        private void CcDraw_Draw(CanvasControl sender, CanvasDrawEventArgs args)
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

        private void CcEmpty_Tapped(object sender, TappedRoutedEventArgs e)
        {
            grid.Toggle(e.GetPosition(sender as UIElement));

            ccDraw.Invalidate();
        }

        private void SlvGrid_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            grid.SetOffsetsAndZoom(e.NextView.HorizontalOffset,
                e.NextView.VerticalOffset, e.NextView.ZoomFactor);

            ccDraw.Invalidate();
        }

        private void AbbRandom_Click(object sender, RoutedEventArgs e)
        {
            grid.SetCellsRandom();
            ccDraw.Invalidate();
        }

        private void SlvGrid_DirectManipulationStarted(object sender, object e)
        {
            isUpdatingAnyway = true;
        }

        private void SlvGrid_DirectManipulationCompleted(object sender, object e)
        {
            isUpdatingAnyway = false;
        }

        private void UpdateView(object sender, RoutedEventArgs e)
        {
            ccDraw.Invalidate();
        }

        private void AbbFullscreen_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            gidSliders.Visibility = Visibility.Collapsed;
        }

        private void AbbFullscreen_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            gidSliders.Visibility = Visibility.Visible;
        }

        private void SlvGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.SetCellSize(e.NewSize);

            ccEmpty.Width = grid.Width;
            ccEmpty.Height = grid.Height;
            ccDraw.Invalidate();
        }
    }
}
