using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace bark_GUI.Custom_Controls
{
    public partial class control_variable_table : custom_control
    {
        //textBox_TextChanged() Method works only for arrayColumns = 2 at 'Insert new value' part


        /* PRIVATE VARIABLES & PROPERTIES */
        bool filling;
        int arrayRows = 300;
        int arrayColumns = 2;
        int minArrayRows = 1;

        int last_i = 0;
        int last_j_val = 0;
        int last_j
        {
            set
            {
                if (value > arrayColumns - 1) last_j_val = 0;
                else if (value < 0) last_j_val = arrayColumns - 1;
                else last_j_val = value;
            }
            get { return last_j_val; }
        }

        TextBox[,] textBoxArray;

        //Constructor
        public control_variable_table()
        {
            InitializeComponent();

            //Initialize tables text boxes
            textBoxArray = new TextBox[arrayRows, arrayColumns];

            for (int i = 0; i < minArrayRows; i++)
                for (int j = 0; j < arrayColumns; j++)
                    _add();
        }






        /* PUBLIC UTILITY METHODS */







        /// <summary> Fills the table with the correct values using '\n' & ' ' as seperators. </summary>
        /// <param name="data">Raw data string. The string will be automatically trimmed before use.</param>
        public void Fill(string data)
        {
            filling = true;
            _reset();

            string tmp = "";
            int reader = 0;
            int valuesRead = 0;
            int sizeOfData = 0;

            if (string.IsNullOrEmpty(data))
                return;

            sizeOfData = _countBreakers(data);

            //Fill the first (premade) text boxes
            for (int i = 0; i < minArrayRows; i++)
                for (int j = 0; j < arrayColumns; j++)
                {
                    while (reader < data.Length && !_isABreaker(data[reader]))
                    {
                        textBoxArray[i, j].Text += data[reader];
                        reader++;
                    }
                    if (++valuesRead > sizeOfData)
                    {
                        filling = false;
                        return;
                    }
                    reader++;
                }

            //Create the rest text boxes
            string data2 = data.Remove(0, reader);

            foreach (char c in data2)
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
            for (int j = 0; j < arrayColumns; j++)
                _add();

            filling = false;
        }












        /* PRIVATE UTILITY METHODS */








        /// <summary> Adds a text box with value. </summary>
        private void _add(string value)
        {
            if (last_j >= arrayColumns - 1)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            textBoxArray[last_i, last_j] = new TextBox();
            table.Controls.Add(textBoxArray[last_i, last_j]);
            textBoxArray[last_i, last_j].Text = value;
            textBoxArray[last_i, last_j].Dock = DockStyle.Fill;
            textBoxArray[last_i, last_j].TextChanged += new EventHandler(this.textBox_TextChanged);
            textBoxArray[last_i, last_j].KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);


            if (last_j < arrayColumns - 1)
                last_j++;
            else
            {
                last_i++;
                last_j = 0;
            }
        }
        /// <summary> Adds an empty text box. </summary>
        private void _add() { _add(""); }

        /// <summary> Increases the number of text box rows by 1 when the last textbox is filled. </summary>
        private void _expand()
        {
            if (!filling && !_lastRowIsEmpty())
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                for (int j = 0; j < arrayColumns; j++)
                    _add();
            }
        }

        /// <summary> Checks if the last row is empty or not. </summary>
        /// <returns> True if the last row has no strings. </returns>
        private bool _lastRowIsEmpty()
        {
            for (int j = arrayColumns - 1; j >= 0; j--)
                if (!string.IsNullOrEmpty(textBoxArray[last_i - 1, j].Text)) return false;
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


        private void textBox_TextChanged(object sender, EventArgs e)
        {
            _expand();
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
                            if (i % arrayColumns > 0)
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

        private void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V && _hasTabsOrEnters(Clipboard.GetText()))
            {
                e.SuppressKeyPress = true;
                Fill(_trimVariableTable(Clipboard.GetText()));
            }
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
            last_i = 0;
            last_j_val = 0;

            //Initialize tables text boxes
            textBoxArray = new TextBox[arrayRows, arrayColumns];

            for (int i = 0; i < minArrayRows; i++)
                for (int j = 0; j < arrayColumns; j++)
                    _add();
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





        /*
        /// <summary> Decrease the number of text box rows until there is only 1 empty row. </summary>
        private void _supress()
        {
            bool delete = true;
            for (int i = last_i-1; i >= 0; i--)
            {
                delete = true;
                for (int j = last_j - 1; j >= 0; j--)
                    if (!_lastRowIsEmpty())) delete = false;
                if (!delete) break;
                else _removeRow();
            }
            _expand();
        }
        private void _removeRow()
        {
            for (int j = last_j - 1; j >= 0; j--)
                tableLayoutPanel1.Controls.Remove(textBoxArray[last_i - 1, j]);
            tableLayoutPanel1.RowStyles.RemoveAt(last_i - 1);
            last_i--;
            last_j = 0;
        }*/
    }
}
