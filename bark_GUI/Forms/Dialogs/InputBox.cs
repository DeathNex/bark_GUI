using System.Windows.Forms;

namespace bark_GUI.Forms.Dialogs
{
    public static class InputBox
    {
        public static string ShowDialog(string text, string caption, string initalValue="")
        {
            Label textLabel = new Label() { Left = 30, Top = 5, Width = 200, Text = text };
            TextBox textBox = new TextBox() { Left = 30, Top = 35, Width = 200, Text = initalValue };
            Button confirmation = new Button() { Text = "Ok", Left = 105, Width = 70, Top = 80 };
            Button cancel = new Button() { Text = "Cancel", Left = 180, Width = 70, Top = 80 };
            Form prompt = new Form {Text = caption, ShowIcon = false, AcceptButton = confirmation, CancelButton = cancel,
                AutoSize = true, MaximizeBox = false, MinimizeBox = false, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                SizeGripStyle = SizeGripStyle.Hide, ShowInTaskbar = false, StartPosition = FormStartPosition.CenterParent};

            confirmation.Click += (sender, e) =>
                {
                    prompt.DialogResult = DialogResult.OK;
                    prompt.Close();
                };
            cancel.Click += (sender, e) =>
                {
                    prompt.DialogResult = DialogResult.Cancel;
                    prompt.Close();
                };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);

            var result = prompt.ShowDialog();

            return result == DialogResult.OK ? textBox.Text.Trim() : null;
        }
    }
}
