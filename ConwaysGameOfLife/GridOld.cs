using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;

namespace ConwaysGameOfLife
{
    class Grid
    {
        private const float gridLinesThicknessPercent = 0.05F;
        private const double cellMarginPercent = 0.1F;
        private const string filename = "Data.txt";

        private readonly int[] neighborOffsetI = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };
        private readonly int[] neighborOffsetJ = new int[] { -1, -1, -1, 0, 1, 1, 1, 0 };

        private bool[,] gridCur, gridTmp;
        private float zoomFactor;
        private double cellSize, horizontalOffset, verticalOffset;
        private Size slvGridSize;

        private List<Task> tasks;

        public bool ShowGrid { get; set; }

        public bool WithCellMargin { get; set; }

        public int Columns { get { return gridCur.GetLength(0); } set { ResizegridCur(value, Rows); } }

        public int Rows { get { return gridCur.GetLength(1); } set { ResizegridCur(Columns, value); } }

        public double Width { get { return cellSize * Columns; } }

        public double Height { get { return cellSize * Rows; } }

        public Grid(int columns, int rows)
        {
            zoomFactor = 1;
            gridCur = new bool[columns, rows];

            ShowGrid = WithCellMargin = true;
        }

        private void ResizegridCur(int newColumns, int newRows)
        {
            int oldColumns = Columns;
            int oldRows = Rows;

            gridTmp = new bool[newColumns, newRows];

            for (int i = 0; i < newColumns && i < oldColumns; i++)
            {
                for (int j = 0; j < newRows && j < oldRows; j++)
                {
                    gridTmp[i, j] = gridCur[i, j];
                }
            }

            gridCur = gridTmp;

            SetCellSize(slvGridSize);
        }

        public void Clear()
        {
            gridCur = new bool[Columns, Rows];
        }

        public void SetCellsRandom()
        {
            Random ran = new Random();

            int columns = Columns, rows = Rows;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    SetValue(i, j, ran.Next() % 2 == 1);
                }
            }
        }

        public void SetNextGeneration()
        {
            int columns = Columns, rows = Rows;

            gridTmp = new bool[columns, rows];
            tasks = new List<Task>();

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    tasks.Add(SetNextGenaerationForCell(i, j));
                }
            }

            WaitForTasks();

            gridCur = gridTmp;
        }

        private async Task SetNextGenaerationForCell(int i, int j)
        {
            int aliveNeigbours = GetNeighbourCount(i, j);

            if (aliveNeigbours == 3 || (aliveNeigbours == 2 && GetValue(i, j)))
            {
                gridTmp[i, j] = true;
            }
        }

        private void WaitForTasks()
        {
            while (tasks.Count > 0)
            {
                tasks[0].Wait();
                tasks.RemoveAt(0);
            }
        }

        private int GetNeighbourCount(int i, int j)
        {
            int aliveCellsCount = 0;

            for (int k = 0; k < neighborOffsetI.Length; k++)
            {
                aliveCellsCount += GetValue(i + neighborOffsetI[k], j + neighborOffsetJ[k]) ? 1 : 0;
            }

            return aliveCellsCount;
        }

        private bool GetValue(int i, int j)
        {
            i = (i + Columns) % Columns;
            j = (j + Rows) % Rows;

            return gridCur[i, j];
        }

        private void SetValue(int i, int j, bool value)
        {
            gridCur[i, j] = value;
        }

        public void Toggle(Point point)
        {
            int i = Convert.ToInt32(Math.Floor((point.X + horizontalOffset) / cellSize / zoomFactor));
            int j = Convert.ToInt32(Math.Floor((point.Y + verticalOffset) / cellSize / zoomFactor));

            if (i < 0 || j < 0 || i >= gridCur.GetLength(0) || j >= gridCur.GetLength(1)) return;

            SetValue(i, j, !GetValue(i, j));
        }

        public void SetCellSize(Size size)
        {
            slvGridSize = size;
            int columns = Columns, rows = Rows;
            double columnCellSize = size.Width / columns;
            double rowCellSize = size.Height / rows;

            cellSize = columnCellSize < rowCellSize ? columnCellSize : rowCellSize;
        }

        public void SetOffsetsAndZoom(double horizontalOffset, double verticalOffset, float zoomFactor)
        {
            this.horizontalOffset = horizontalOffset;
            this.verticalOffset = verticalOffset;
            this.zoomFactor = zoomFactor;
        }

        public void Draw(CanvasDrawingSession ds)
        {
            int columns = Columns, rows = Rows;
            float linesThickness = gridLinesThicknessPercent * (float)cellSize * zoomFactor;
            Color colorGrid = Color.FromArgb(255, 255, 0, 0), colorCell = Color.FromArgb(255, 0, 0, 0);

            if (ShowGrid) DrawGrid(ds, columns, rows, colorGrid, linesThickness);

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    DrawCell(ds, cellSize, i, j, colorCell, linesThickness);
                }
            }
        }

        private void DrawGrid(CanvasDrawingSession ds, int columns, int rows, Color color, float linesThickness)
        {
            for (int i = 0; i <= Rows; i++)
            {
                float x1 = (float)-horizontalOffset;
                float x2 = (float)(cellSize * columns * zoomFactor - horizontalOffset);
                float y = (float)(cellSize * i * zoomFactor - verticalOffset);

                DrawLine(ds, x1, y, x2, y, color, linesThickness);
            }

            for (int i = 0; i <= columns; i++)
            {
                float x = (float)(cellSize * i * zoomFactor - horizontalOffset);
                float y1 = (float)-verticalOffset;
                float y2 = (float)(cellSize * rows * zoomFactor - verticalOffset);

                DrawLine(ds, x, y1, x, y2, color, linesThickness);
            }
        }

        private async void DrawLine(CanvasDrawingSession ds, float x1,
            float y1, float x2, float y2, Color color, float thickness)
        {
            if (IsLineInView(ref x1, ref y1, ref x2, ref y2)) ds.DrawLine(x1, y1, x2, y2, color, thickness);
        }

        private bool IsLineInView(ref float x1, ref float y1, ref float x2, ref float y2)
        {
            if (x1 < 0 && y1 < 0) return false;

            if (x1 < 0) x1 = 0;
            if (x2 > slvGridSize.Width) x2 = (float)slvGridSize.Width;
            if (y1 < 0) y1 = 0;
            if (y2 > slvGridSize.Height) y2 = (float)slvGridSize.Height;

            return true;
        }

        private async void DrawCell(CanvasDrawingSession ds, double cellSize,
            int i, int j, Color color, float strokeThickness)
        {
            if (!gridCur[i, j]) return;

            double cellMargin = cellMarginPercent * cellSize * (WithCellMargin ? 1 : 0);
            double xPixel = zoomFactor * (i * cellSize + cellMargin) - horizontalOffset;
            double yPixel = zoomFactor * (j * cellSize + cellMargin) - verticalOffset;
            double widthHeightPixel = zoomFactor * (cellSize - 2 * cellMargin);

            Rect rect = new Rect(xPixel, yPixel, widthHeightPixel, widthHeightPixel);

            if (!IsRectInView(rect)) return;

            ds.FillRectangle(rect, color);
        }

        private bool IsRectInView(Rect rect)
        {
            if (IsConerInView(rect.X, rect.Y)) return true;
            if (IsConerInView(rect.X + rect.Width, rect.Y)) return true;
            if (IsConerInView(rect.X, rect.Y + rect.Height)) return true;
            if (IsConerInView(rect.X + rect.Width, rect.Y + rect.Height)) return true;

            return false;
        }

        private bool IsConerInView(double x, double y)
        {
            return !(x < 0 || y < 0 || x > slvGridSize.Width || y > slvGridSize.Height);
        }

        public async void Save()
        {
            string output = "";

            int columns = Columns, rows = Rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    output += GetValue(j, i) ? "1" : "0";
                }

                output += "\n";
            }

            output = output.TrimEnd('\n');

            StorageFile file = await ApplicationData.Current.LocalFolder.
                CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, output);
        }

        public async static Task<Grid> Load()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            string[] lines = (await FileIO.ReadLinesAsync(file)).ToArray();
            Grid grid = new Grid(lines[1].Length, lines.Length);

            int columns = grid.Columns, rows = grid.Rows;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    grid.SetValue(i, j, lines[j][i] == '1');
                }

            }

            return grid;
        }
    }
}
