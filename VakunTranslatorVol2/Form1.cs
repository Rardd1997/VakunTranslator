using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VakunTranslatorVol2.Analyzers;

namespace VakunTranslatorVol2
{
    public partial class Form1 : Form, IView
    {
        public event Action<string> AnalyzeRequired;
        public event Action NewFileClick;
        public event Action OpenFileClick;
        public event Action<string> SaveFileClick;
        public event Action<string> SaveFileAsClick;

        public Form1()
        {
            InitializeComponent();
            menuStrip1.Renderer = new CustomToolStripRenderer();
            sourceBox.SelectionTabs = Enumerable.Range(1, 10).Select(x => x * 14).ToArray();
            analyzerWindow.LexemSelected += AnalyzerWindow_LexemSelected;
        }

        public void ClearConsole()
        {
            console.Clear();
        }
        public void DisplayConstants<T>(IEnumerable<T> source)
        {
            analyzerWindow.DisplayConstants(source);
        }
        public void DisplayIdentificators<T>(IEnumerable<T> source)
        {
            analyzerWindow.DisplayIdentificators(source);
        }
        public void DisplayLexemes<T>(IEnumerable<T> source)
        {
            analyzerWindow.DisplayLexemes(source);
        }
        public void HighlightSourceCode(IEnumerable<Lexeme> source)
        {
            colorizer?.Highlight(sourceBox, source);
        }
        public void WriteConsole(IEnumerable<string> lines)
        {
            foreach(var line in lines)
            {
                WriteConsole(line);
            }
        }
        public void WriteConsole(string message)
        {
            console.AppendText(message);
            console.AppendText(Environment.NewLine);
        }
        public void SetSourceCode(string text)
        {
            sourceBox.Text = text;
        }
        public void HideConsole()
        {
            tableLayoutPanel1.RowStyles[1].Height = 0;
        }
        public void ShowConsole()
        {
            tableLayoutPanel1.RowStyles[1].Height = CONSOLE_HEIGHT;
        }
        public void SetColorizer(Colorizer<Lexeme> colorizer)
        {
            this.colorizer = colorizer;
        }
        public void SetPDAOutput(List<PDASyntaxAnalyzer.UsedRule> rules)
        {
            pdaWindow.SetRules(rules);
        }

        private void sourceBox_TextChanged(object sender, EventArgs e)
        {
            var source = (sender as RichTextBox).Text;
            if(!lastSource.Equals(source))
            {
                lastSource = source;
                AnalyzeRequired?.Invoke(source);
            }
        }
        private void lexicalAnlyzerOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analyzerWindow.Show();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckLastSourceCode())
            {
                NewFileClick?.Invoke();
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckLastSourceCode())
            {
                OpenFileClick?.Invoke();
                openFileSource = lastSource;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileClick?.Invoke(sourceBox.Text);
            openFileSource = lastSource;
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAsClick?.Invoke(sourceBox.Text);
        }
        private void AnalyzerWindow_LexemSelected(string body, int line)
        {
            var start = sourceBox.GetFirstCharIndexFromLine(line);
            var end = start + sourceBox.Lines[line].Length;

            sourceBox.Find(body, start, end, RichTextBoxFinds.WholeWord);
            Focus();
        }
        private void pDAAnalyzerOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pdaWindow.ShowDialog();
        }
        private bool CheckLastSourceCode()
        {
            if (openFileSource != sourceBox.Text)
            {
                if (MessageBox.Show("Some changes were not saved. Continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }
        private void checkGrammarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private const int CONSOLE_HEIGHT = 250;
        private string lastSource = string.Empty;
        private string openFileSource = string.Empty;
        private Colorizer<Lexeme> colorizer;
        private AnalyzerWindow analyzerWindow = new AnalyzerWindow();
        private PDAOutputForm pdaWindow = new PDAOutputForm();
        private GrammarCheckForm grammarCheck = new GrammarCheckForm();
    }
}
