using System;
using System.Linq;

namespace ScrapbookQA
{
    class QA
    {
        Random rand = new Random();

        string question;
        
        int correctIndex;
        string[] answers;
        int answersArrayNextNdx;

        public QA(string question, string answer, string[] decoys)
        {
            this.question = question;

            //Initialize answers array with answer and decoys parameters
            int answersCount = decoys.Count() + 1;
            this.answers = new string[answersCount];
            this.answers[0] = answer;
            decoys.CopyTo(answers, 1);

            //Correct answer placed in index 0 by default
            correctIndex = 0;
            answersArrayNextNdx = answers.Count();
        }

        public string getQuestion() { return this.question; }
        public int getCorrectIndex() { return this.correctIndex; }
        public string[] getAnswers() { return this.answers; }

        //Shuffles all answers using the Knuth Shuffle for efficieny and redundancy protection
        public void ShuffleAnswers()
        {
            for (int i = 0; i < answers.Count(); i++)
            {
                //Set next index and check for end of array reached
                answersArrayNextNdx++;
                if (answersArrayNextNdx >= answers.Count() - 1)
                {
                    answersArrayNextNdx = 0;
                }

                //Set indexes of random entry to be swapped
                int randNdx = rand.Next(answersArrayNextNdx, answers.Count());

                //Swap entries
                string oldEntry = answers[answersArrayNextNdx];
                string newEntry = answers[randNdx];
                answers[answersArrayNextNdx] = newEntry;
                answers[randNdx] = oldEntry;

                //Change index for correct answer if it was swapped
                if (correctIndex == answersArrayNextNdx)
                    correctIndex = randNdx;
                else if (correctIndex == randNdx)
                    correctIndex = answersArrayNextNdx;
            }
        }
    }
}
