using System;
using System.IO;
using System.Windows.Forms;

namespace VakunTranslatorVol2
{
    public class FileManager
    {
        public string Text { get; private set; }
        private string fileName = null;

        public bool OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
                Text = File.ReadAllText(fileName);
                return true;
            }
            else
            {
                fileName = null;
                return false;
            }
        }
        public bool CreatFileDialog()
        {
            string defaultExt = ".txt";

            var dialog = new SaveFileDialog();
            dialog.DefaultExt = defaultExt;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
                dialog.OpenFile().Close();
                return true;
            }
            else
            {
                fileName = null;
                return false;
            }
        }
        public bool SaveFileDialog(string _text)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    File.WriteAllText(fileName, _text);
                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Your file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileName = null;
                return false;
            }
        }
        public bool SaveAsFileDialog(string _text)
        {
            var dialog = new SaveFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileName = dialog.FileName;
                    dialog.OpenFile().Close();
                    File.WriteAllText(fileName, _text);
                    return true;
                }
                catch (IOException)
                {
                    MessageBox.Show("We encountered an error when saving a file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                fileName = null;
                return false;
            }
        }
    }
}
