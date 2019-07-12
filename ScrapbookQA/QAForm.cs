using System;
using System.Windows.Forms;

namespace ScrapbookQA
{
    public partial class QAForm : Form
    {
        public bool GuessedCorrect { get; set; }
        public string CorrectAnswer { get; set; }

        public QAForm()
        {
            InitializeComponent();
            this.Text = "QA";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //Display question and answers
        public void SetQA(string question, string[] answers, int correctAnswerIndex)
        {
            this.CorrectAnswer = answers[correctAnswerIndex];

            txtBxMsg.Text = question;

            radioButton1.Text = answers[0];
            radioButton3.Text = answers[1];
            radioButton4.Text = answers[2];
            radioButton2.Text = answers[3];
        }
        
        // Handles multiple choice selected events. Shows a new msg if correct, otherwise exits application
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked) //CheckedChanged fires twice per change. Only use new checked btn
            {
                if (rb.Text == CorrectAnswer) //Accuracy test
                {
                    GuessedCorrect = true;
                }
                else
                {
                    GuessedCorrect = false;
                    MessageBox.Show(this, "Incorrect! Try again next time!");
                }

                this.DialogResult = DialogResult.Cancel;
                rb.Checked = false;
            }
        }

        private void Form1_Close(object sender, FormClosingEventArgs e)
        {
        }
    }
}
