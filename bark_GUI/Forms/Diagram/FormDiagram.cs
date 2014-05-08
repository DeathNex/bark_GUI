using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace bark_GUI
{
    public partial class FormDiagram : Form
    {
        public const int MaxNumberOfYAxis = 10;


        private static int _diagramID = 0;

        // Line colors.
        private List<Color> _lineColors;

        // All the input data converted to 2D array.
        private string[][] _dataArray;
        private int _rows;
        private int _columns;
        private int _noOfSelectedYAxis;

        // The indexes of the selected axis' in the dataArray.
        private int _indexOfXAxis;
        private int[] _indexOfYAxis;

        // All the points (as number, not string) of the selected Axis'.
        double[] _xPoints;
        double[][] _yPoints;


        // Constructor
        public FormDiagram(string diagramTitle, string xAxis, string[] yAxis, string[] inputData)
        {
            InitializeComponent();

            // Get desired values from given data.
            var yAxisGraphTitle = yAxis.Length == 1 ? yAxis[0] : yAxis.Aggregate("", (current, yAxisItem) => current + (yAxisItem + '\n'));

            _rows = inputData.Length;
            _columns = inputData[0].Split('\t').Length;
            _noOfSelectedYAxis = yAxis.Length;

            // Check maximum number of Y-Axis'.
            Debug.Assert(_noOfSelectedYAxis <= MaxNumberOfYAxis, "Too many Y-Axis' were selected.\n Please try again with less.");

            // Convert the string lines of data to a two dimensional array of data.
            CreateDataArray(inputData);

            // Find the selected (by user) axis' indexes (Column-J) in the data array.
            SetIndexesOfSelectedAxis(xAxis, yAxis);

            // Check index of X-Axis.
            Debug.Assert(_indexOfXAxis >= 0, "Error! Cannot handle data from .dat file.\n" +
                                            "Index of X Axis in the dataArray was not set.\n" +
                                            "The name " + xAxis + " was not found in the first row of the .dat file.");

            // Create the data points as numbers.
            SetDataPoints();

            // Create the colors for visual separation of lines.
            CreateColors();

            // Set diagram parameters.
            diagram.GraphPane = new GraphPane();

            diagram.GraphPane.Title.Text = diagramTitle + ' ' + (++_diagramID);
            diagram.GraphPane.XAxis.Title.Text = xAxis;
            diagram.GraphPane.YAxis.Title.Text = yAxisGraphTitle;

            // Create lines.
            for (int k = 0; k < _noOfSelectedYAxis; k++)
            {
                diagram.GraphPane.AddCurve(yAxis[k], _xPoints, _yPoints[k], _lineColors[k], SymbolType.None);
            }

            diagram.RestoreScale(diagram.GraphPane);
            diagram.GraphPane.ZoomStack.Clear();
        }

        #region Private Initialization Methods
        private void CreateDataArray(string[] inputData)
        {
            _dataArray = new string[_rows][];

            // Create dataArray & get indexes of selected axis'.
            for (int i = 0; i < _rows; i++)
            {
                _dataArray[i] = new string[_columns];

                var columnStartIndex = 0;
                for (int j = 0; j < _columns; j++)
                {
                    // If it's not the last column, the data is separated with a tab (\t) from the next column data.
                    var columnEndIndex = j < _columns - 1 ? inputData[i].IndexOf('\t', columnStartIndex) : inputData[i].Length - 1;

                    // Get the part from the data that interests us (current row and column)
                    _dataArray[i][j] = inputData[i].Substring(columnStartIndex, (columnEndIndex - columnStartIndex) + 1).Trim();

                    // Set the string's column further.
                    columnStartIndex = columnEndIndex + 1;
                }
            }
        }

        private void SetIndexesOfSelectedAxis(string xAxis, string[] yAxis)
        {
            // Indexes (column-J) in the dataArray of the selected Axis' (by user).
            _indexOfXAxis = -1;
            _indexOfYAxis = new int[_noOfSelectedYAxis];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    // Get the indexes of the selected (by user) axis'. (first row = titles)
                    if (i == 0)
                    {
                        if (_dataArray[i][j] == xAxis)
                            _indexOfXAxis = j;

                        for (int k = 0; k < _noOfSelectedYAxis; k++)
                        {
                            if (_dataArray[i][j] == yAxis[k])
                                _indexOfYAxis[k] = j;
                        }
                    }
                }
            }
        }

        private void SetDataPoints()
        {
            var rowsWithNumbers = _rows - 1;
            // All the points (as number, not string) of the selected Axis'.
            _xPoints = new double[rowsWithNumbers];
            _yPoints = new double[_noOfSelectedYAxis][];


            // Get the selected axis' point values.

            // Fill (double) xPoints from (string) dataArray.
            for (int i = 1; i < _rows; i++)
            {
                var isNotNumber = !double.TryParse(_dataArray[i][_indexOfXAxis], out _xPoints[i - 1]);

                // Check.
                if (isNotNumber)
                {
                    MessageBox.Show(string.Format("Error! Cannot convert X-Axis value to double.\n" +
                                                  "File .dat has invalid data on row {0} and column {1}.\n\nData:\n{2}",
                                                  i, _indexOfXAxis, _dataArray[i][_indexOfXAxis]));
                    Close();
                }

            }

            // Fill (double) yPoints from (string) dataArray.
            for (int k = 0; k < _noOfSelectedYAxis; k++)
            {
                _yPoints[k] = new double[rowsWithNumbers];

                // For every Y-Axis iterate the dataArray by rows to get all the desired data in number.
                for (int i = 1; i < _rows; i++)
                {
                    var isNotNumber = !double.TryParse(_dataArray[i][_indexOfYAxis[k]], out _yPoints[k][i - 1]);

                    // Check.
                    if (isNotNumber)
                    {
                        MessageBox.Show(string.Format("Error! Cannot convert Y-Axis value to double.\n" +
                                                      "File .dat has invalid data for Y-Axis on row {0} and column {1}({2}).\n\n" +
                                                      "Data:\n{3}", i, _indexOfYAxis[k], k, _dataArray[i][_indexOfYAxis[k]]));
                        Close();
                    }

                }
            }
        }

        private void CreateColors()
        {
            // Basic 10 Colors
            _lineColors = new List<Color>
                {
                    Color.Blue,
                    Color.Red,
                    Color.Green,
                    Color.Gold,
                    Color.Orchid,
                    Color.Cyan,
                    Color.Lime,
                    Color.Brown,
                    Color.DarkOrange,
                    Color.HotPink
                };

            // Additional Random Colors. (if required)
            var randomGen = new Random();
            KnownColor[] colorNames = (KnownColor[])Enum.GetValues(typeof(KnownColor));

            while (_lineColors.Count < _noOfSelectedYAxis)
            {
                var newRandomColorsArray = colorNames.Where(c => !_lineColors.Contains(Color.FromKnownColor(c))).ToArray();
                var newRandomColor = Color.FromKnownColor(newRandomColorsArray[randomGen.Next(newRandomColorsArray.Length)]);

                _lineColors.Add(newRandomColor);
            }
        }
        #endregion

        private void FormDiagram_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
    }
}
