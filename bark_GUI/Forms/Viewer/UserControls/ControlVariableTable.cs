using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using bark_GUI.Structure.Items;

namespace bark_GUI.CustomControls
{
    public partial class ControlVariableTable : UserControl
    {
        // Constraint: textBox_TextChanged() Method works only for arrayColumns = 2 at 'Insert new value' part

        // Public Variables

        public ValueValidator Validator;

        public bool IsRequired;


        // Private Variables

        private const int ArrayRowsMax = 300;

        private const int ArrayRowsMin = 1;

        private const int ArrayColumns = 2;

        private TextBox[,] _textBoxArray;

        private bool _filling;

        private int _lastIndex;

        private int _lastJValue;

        private int LastJ
        {
            set
            {
                if (value > ArrayColumns - 1) _lastJValue = 0;
                else if (value < 0) _lastJValue = ArrayColumns - 1;
                else _lastJValue = value;
            }
            get { return _lastJValue; }
        }


        #region Constructor
        public ControlVariableTable()
        {
            InitializeComponent();

            // Initialize tables text boxes
            _textBoxArray = new TextBox[ArrayRowsMax, ArrayColumns];

            for (var i = 0; i < ArrayRowsMin; i++)
                for (var j = 0; j < ArrayColumns; j++)
                    _addEmpty();

            // Red colors if empty text boxes & required.
            textBox_TextChanged(null, null);
        }
        #endregion

        #region Public Methods
        /// <summary> Fills the table with the correct values using '\n' & ' ' as seperators. </summary>
        /// <param name="data">Raw data string. The string will be automatically trimmed before use.</param>
        public void Fill(string data)
        {
            _filling = true;
            _reset();

            var tmp = "";
            var reader = 0;
            var valuesRead = 0;
            var sizeOfData = 0;

            if (string.IsNullOrEmpty(data))
                return;

            sizeOfData = _countBreakers(data);

            //Fill the first (premade) text boxes
            for (var i = 0; i < ArrayRowsMin; i++)
                for (var j = 0; j < ArrayColumns; j++)
                {
                    while (reader < data.Length && !_isABreaker(data[reader]))
                    {
                        _textBoxArray[i, j].Text += data[reader];
                        reader++;
                    }
                    if (++valuesRead > sizeOfData)
                    {
                        _filling = false;
                        return;
                    }
                    reader++;
                }

            //Create the rest text boxes
            string data2 = data.Remove(0, reader);

            foreach (var c in data2)
            {
                if (_isABreaker(c))
                {
                    _add(tmp);
                    tmp = "";
                }
                else
                    tmp += c;
            }
            _add(tmp);

            //Expand for edit
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            for (var j = 0; j < ArrayColumns; j++)
                _addEmpty();

            _filling = false;
        }

        public bool HasValue()
        {
            var boxHasValue = false;

            // Iterate the text boxes to check if any of them have a value.
            foreach (var textBox in _textBoxArray)
            {
                // If a text box is null, means there are no more text boxes left to check.
                if (textBox == null)
                    return false;

                // 
                if (!string.IsNullOrEmpty(textBox.Text.Trim()))
                    boxHasValue = true;

                if (boxHasValue) break;
            }

            return boxHasValue;
        }

        public string GetValue()
        {
            var stop = false;
            var result = "";
            var rows = _textBoxArray.Length / ArrayColumns + (_textBoxArray.Length % ArrayColumns > 0 ? 1 : 0);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < ArrayColumns; j++)
                {
                    if (_textBoxArray[i, j] == null) { stop = true; break; }

                    result += _textBoxArray[i, j].Text.Trim() + '\t';
                }
                result = result.TrimEnd('\t');

                if (stop) break;
                result += '\n';
            }

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary> Adds a text box with value. </summary>
        private void _add(string value)
        {
            // TODO Fix RowStyle breaking at row 13+... Try something with the RowCount?
            if (value == string.Empty)
                return;

            value = Regex.Replace(value, @"[\n\r\t\s]", "");

            if (value == string.Empty)
                return;
            if (LastJ > ArrayColumns - 1)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _textBoxArray[_lastIndex, LastJ] = new TextBox();
            table.Controls.Add(_textBoxArray[_lastIndex, LastJ]);
            _textBoxArray[_lastIndex, LastJ].Text = value;
            _textBoxArray[_lastIndex, LastJ].Dock = DockStyle.Fill;
            _textBoxArray[_lastIndex, LastJ].TextChanged += new EventHandler(this.textBox_TextChanged);
            _textBoxArray[_lastIndex, LastJ].KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);


            if (LastJ < ArrayColumns - 1)
                LastJ++;
            else
            {
                _lastIndex++;
                LastJ = 0;
            }
        }
        /// <summary> Adds an empty text box. </summary>
        private void _addEmpty()
        {
            if (LastJ > ArrayColumns - 1)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _textBoxArray[_lastIndex, LastJ] = new TextBox();
            table.Controls.Add(_textBoxArray[_lastIndex, LastJ]);
            _textBoxArray[_lastIndex, LastJ].Text = "";
            _textBoxArray[_lastIndex, LastJ].Dock = DockStyle.Fill;
            _textBoxArray[_lastIndex, LastJ].TextChanged += textBox_TextChanged;
            _textBoxArray[_lastIndex, LastJ].KeyDown += textBox_KeyDown;


            if (LastJ < ArrayColumns - 1)
                LastJ++;
            else
            {
                _lastIndex++;
                LastJ = 0;
            }
        }

        /// <summary> Increases the number of text box rows by 1 when the last textbox is filled. </summary>
        private void _expand()
        {
            if (!_filling && !_lastRowIsEmpty())
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                for (int j = 0; j < ArrayColumns; j++)
                    _addEmpty();
            }
        }

        /// <summary> Decrease the number of text box rows until there is only 1 empty row. </summary>
        private void _supress()
        {
            bool stopDeleting = false;
            for (int i = _lastIndex - 1; i > 0; i--)
            {
                for (int j = ArrayColumns - 1; j >= 0; j--)
                    if (!_lastRowIsEmpty())
                        stopDeleting = true;

                if (stopDeleting)
                    break;

                _removeRow();
            }
        }

        private void _removeRow()
        {
            for (int j = ArrayColumns - 1; j >= 0; j--)
                table.Controls.Remove(_textBoxArray[_lastIndex - 1, j]);
            table.RowStyles.RemoveAt(_lastIndex - 1);
            _lastIndex--;
            LastJ = 0;
        }

        /// <summary> Checks if the last row is empty or not. </summary>
        /// <returns> True if the last row has no strings. </returns>
        private bool _lastRowIsEmpty()
        {
            for (int j = ArrayColumns - 1; j >= 0; j--)
                if (!string.IsNullOrEmpty(_textBoxArray[_lastIndex - 1, j].Text)) return false;
            return true;
        }

        private bool _isABreaker(char c)
        {
            switch (c)
            {
                case '\n':
                    return true;
                case '\r':
                    return true;
                case ' ':
                    return true;
                default:
                    return false;
            }
        }

        private int _countBreakers(string data)
        {
            int counter = 0;
            foreach (char c in data)
                switch (c)
                {
                    case '\n':
                        counter++;
                        break;
                    case '\r':
                        break;
                    case ' ':
                        counter++;
                        break;
                }
            return counter;
        }

        private bool _hasTabsOrEnters(string s)
        {
            foreach (char c in s)
                if (c == '\n' || c == '\t')
                    return true;
            return false;
        }

        private void _reset()
        {
            table.Controls.Clear();
            _lastIndex = 0;
            _lastJValue = 0;

            //Initialize tables text boxes
            _textBoxArray = new TextBox[ArrayRowsMax, ArrayColumns];
        }

        private string _trimVariableTable(string data)
        {
            data = data.Replace('\t', ' ');
            data = data.Trim(new char[4] { ' ', '\r', '\n', '\t' });
            char prev = data[0];
            int i = 1;

            while (i < data.Length)
            {
                char c = data[i];
                prev = data[i - 1];
                if (_isABreaker(c) && _isABreaker(prev))
                {
                    if (prev != '\n')
                        data = data.Remove(i - 1, 1);
                    else
                        data = data.Remove(i, 1);
                    i--;
                }
                i++;
            }
            return data;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            var arrayIsValid = true;
            var emptyTextBoxes = 0;

            _supress();

            _expand();

            // Validation
            foreach (var textBox in _textBoxArray)
            {
                if (textBox == null) continue;

                var value = textBox.Text.Trim();
                var isValid = true;

                textBox.ResetBackColor();
                textBox.ResetForeColor();

                // Item Required Validation
                if (string.IsNullOrEmpty(value) && IsRequired)
                {
                    textBox.BackColor = Color.Tomato;
                    emptyTextBoxes++;
                    arrayIsValid = false;
                    continue;
                }

                // SimpleType Validation
                if (Validator != null)
                    isValid = Validator(value);

                if (!isValid)
                {
                    textBox.ForeColor = Color.Red;
                    arrayIsValid = false;
                }
            }

            // Check if there are just 2 empty text boxes, then the array is valid.
            if (emptyTextBoxes == 2)
            {
                foreach (var textBox in _textBoxArray)
                    if (textBox != null)
                        textBox.ResetBackColor();

                arrayIsValid = true;
            }
            else
            {
                // Reset the color of the last 2 text boxes.
                if (_textBoxArray[_lastIndex - 1, 0] != null)
                    _textBoxArray[_lastIndex - 1, 0].ResetBackColor();
                if (_textBoxArray[_lastIndex - 1, 1] != null)
                    _textBoxArray[_lastIndex - 1, 1].ResetBackColor();
            }

            // Check if the text boxes are not valid, don't save anything.
            if (!arrayIsValid) return;


            if (Tag != null)
            {
                string s;
                string prefix = null;
                string suffix = null;
                string value = null;
                string additions = "";
                TextBox t = sender as TextBox;
                string newValue = t.Text.Trim();
                int tIndex = t.Parent.Controls.GetChildIndex(t);
                int breakCounter = 0;
                int oldValueStart = 0;
                int oldValueLength = 0;

                //Get the correct strings
                s = (Tag as XmlNode).FirstChild.FirstChild.Value;

                //Get the position of the desired old value in the XmlNode
                foreach (char c in s)
                {
                    if (breakCounter >= tIndex)
                    {
                        if (_isABreaker(c))
                            break;
                        oldValueLength++;
                    }
                    else
                    {
                        if (_isABreaker(c))
                            breakCounter++;
                        oldValueStart++;
                    }
                }

                /* Wrong Cases
                 * oldValueStart = 0 [no prefix]
                 * oldValueLength = 0 [Add, Add*]
                 * oldValueStart = max [Add, Add*] [no suffix]
                 * 
                 */
                bool newValueIsEmpty = newValue == string.Empty;
                bool isAtTheEnd = (oldValueStart >= s.Length - 1) || ((oldValueStart >= s.Length - 2) && (s.EndsWith("\n") || s.EndsWith(" ")));
                bool oldValueExists = oldValueLength > 0;
                bool itemIsAtFirstPos = oldValueStart <= 0;

                //Don't break the table structure by placing zeros
                if (newValueIsEmpty && !isAtTheEnd)
                    newValue = "0";

                //Break the string to the desired parts
                if (!itemIsAtFirstPos)
                    prefix = s.Substring(0, oldValueStart);
                if (oldValueExists)
                    value = s.Substring(oldValueStart, oldValueLength);
                if (!isAtTheEnd)
                    suffix = s.Substring(oldValueStart + oldValueLength);
                else
                {
                    if (!newValueIsEmpty)
                    {
                        for (int i = 0; i < tIndex - breakCounter; i++)
                            if (i % ArrayColumns > 0)
                                additions += "\n0";
                            else
                                additions += " 0";
                        if (additions.Length > 1)
                            additions = additions.Remove(additions.Length - 1);
                    }
                    else
                        suffix = null;
                }

                //Reassemble with the new value
                s = string.Format(prefix + additions + newValue + suffix);


                /* OLD WAY DELETEME
                //Clear the old value
                if (oldValueLength > 0)
                    s = s.Remove(oldValueStart + (oldValueLength - 1));

                //Insert the new value
                if (oldValueStart == s.Length) //if the value is at the end of the string
                {
                    if (s.LastIndexOf('\n') > s.LastIndexOf(' ')) //Add a row/column
                        s = string.Format(s + ' ' + newValue);
                    else
                        s = string.Format(s + '\n' + newValue);
                }
                else
                    s = s.Insert(oldValueStart, newValue);*/

                //Save the result on the XmlNode value
                (Tag as XmlNode).FirstChild.FirstChild.Value = s;
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V && _hasTabsOrEnters(Clipboard.GetText()))
            {
                e.SuppressKeyPress = true;
                Fill(_trimVariableTable(Clipboard.GetText()));
            }
        }
        #endregion

    }
}
