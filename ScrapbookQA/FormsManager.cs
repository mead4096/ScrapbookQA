using System;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Configuration;

namespace ScrapbookQA
{
    public class FormsManager
    {
        //Initialize forms
        ImgForm imgBox = new ImgForm();
        QAForm qaForm = new QAForm();

        Random rand = new Random();

        DataPool dataPool;

        private string emailHeader = ConfigurationManager.AppSettings["MailHead"];
        public StringBuilder EmailMsg;

        public FormsManager()
        {
            //Load data
            dataPool = new DataPool();
            try
            {
                dataPool.LoadData("OutputMsgs.xml");
            }
            catch (IOException e)
            {
                MessageBox.Show("Whoops! The program can not be run at this time.\n\nError: " + e.Message);
                ExitApp(true);
            }

            //Initiate EmailMsg with a body header
            EmailMsg = new StringBuilder(emailHeader);
        }

        public void MainLoop()
        {
            //Msg box seen first by user b4 QA form
            ShowMsg();
        }

        //Randomly chooses to show an img or a msg
        private void ShowMsg()
        {
            int imgOrMsg = rand.Next(2);

            DialogResult result = new DialogResult();
            if (imgOrMsg == 0)
            {
                imgBox.SetPath(dataPool.GetImg());
                result = imgBox.ShowDialog();
            }
            else
            {
                string msg = dataPool.GetMsg();
                result = MessageBox.Show(msg, "Message", MessageBoxButtons.YesNo);
                EmailMsg.Append(msg).Append("\n\n");
            }

            if (result == DialogResult.Yes) //Quit btn selected
            {
                ExitApp();
            }
            else //New message btn selected
            {
                ShowQA();
            }
        }
        
        //Gets a random QA to be displayed. Changes control based on user input
        private void ShowQA()
        {
            QA qa = dataPool.GetQA();

            string question = "Question:" + Environment.NewLine + qa.getQuestion();

            DialogResult result = new DialogResult();

            qaForm.SetQA(question, qa.getAnswers(), qa.getCorrectIndex());
            result = qaForm.ShowDialog();

            if (qaForm.GuessedCorrect)
            {
                qaForm.GuessedCorrect = false;

                EmailMsg.Append("Q: ").Append(qa.getQuestion()).Append("\n").Append("A: ").Append(qaForm.CorrectAnswer).Append("\n\n");

                ShowMsg();
            }
            else
            {
                ExitApp();
            }
        }

        //Attempts to send email while showing error messages; options to retry
        private void SendMail()
        {
            SMTPClient MailClient = new SMTPClient();
            bool Resend = true;

            try
            {
                MailClient.BuildMail(EmailMsg);
            }
            catch (Exception e)
            {
                MessageBox.Show("Whoops! An email could not be sent at this time.\n\nError: " + e.Message);
                Resend = false;
            }

            //Send mail; give user retry option if failed to send
            while (!MailClient.SendMail() && Resend)
            {
                DialogResult Result = MessageBox.Show("Error: Email could not be sent\n\nTry again?", "Mail Send Error", MessageBoxButtons.RetryCancel);

                if (Result == DialogResult.Cancel)
                    Resend = false;
            }
        }

        //Disposes forms to exit main loop and conditionally send mail
        private void ExitApp(bool errors = false)
        {
            //Dispose of forms
            qaForm.Dispose();
            imgBox.Dispose();

            //Send mail if program ran successfully and config value is true
            if (errors == false && ConfigurationManager.AppSettings["SendMail"].ToLower() == "true")
                SendMail();

            //Automatically exits main loop
        }
    }
}
