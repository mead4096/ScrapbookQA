using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ScrapbookQA
{
    class DataPool
    {
        //Class scope to give a more global timer and more accurately random value
        Random rand = new Random();

        string[] MsgArray;
        int MsgArrayNextRandNdx;

        string[] ImgArray;
        int ImgArrayNextRandNdx;

        QA[] QAArray;
        int QAArrayNextRandNdx;

        public DataPool() {}

        //Returns a random message
        public string GetMsg()
        {
            Shuffle(ref MsgArray, ref MsgArrayNextRandNdx);

            return MsgArray[MsgArrayNextRandNdx];
        }

        //Returns the path to a random image
        public string GetImg()
        {
            Shuffle(ref ImgArray, ref ImgArrayNextRandNdx);

            return ImgArray[ImgArrayNextRandNdx];
        }

        //Returns a random QA struct
        public QA GetQA()
        {
            Shuffle(ref QAArray, ref QAArrayNextRandNdx);
            QAArray[QAArrayNextRandNdx].ShuffleAnswers();

            return QAArray[QAArrayNextRandNdx];
        }
        
        public void LoadData(string fileName)
        {
            //Load XML file
            XDocument xmlDoc;
            try
            {
                //Load XML file
                xmlDoc = XDocument.Load(fileName);

                //Load messages
                MsgArray = xmlDoc.Root.Elements("msgscreen").Elements("msg").Select(element => element.Value).ToArray();

                //Check # of messages
                int MsgIndexRange = MsgArray.Count() - 1;
                if (MsgIndexRange < 1) //Unsuccessful if fewer than 2 entries in xml file
                    throw new IOException("Fewer than two messages exist!");
                else
                    MsgArrayNextRandNdx = MsgIndexRange;

                //Load images
                ImgArray = Directory.GetFiles(@"..\Pictures");

                //Check # of images
                int ImgIndexRange = ImgArray.Count() - 1;
                if (ImgIndexRange < 1) //Unsuccessful if fewer than 2 pictures available
                    throw new IOException("Fewer than two images exist!");
                else
                    ImgArrayNextRandNdx = ImgIndexRange;

                //Load questions and answers
                int QASize = xmlDoc.Root.Elements("qascreen").Elements("qa").Count();
                QAArray = new QA[QASize];

                //Check # of QAs
                int QAArrayIndexRange = QAArray.Count() - 1;
                if (QAArrayIndexRange < 1) //Unsuccessful if fewer than 2 pictures available
                    throw new IOException("Fewer than two question/answers exist!");
                else
                    QAArrayNextRandNdx = QAArrayIndexRange;

                int i = 0;
                foreach (XElement qa in xmlDoc.Root.Elements("qascreen").Elements("qa"))
                {
                    string question = qa.Element("question").Value.ToString();
                    string answer = qa.Element("answer").Value.ToString();
                    string[] decoys = qa.Elements("decoy").Select(element => element.Value).ToArray();

                    if (decoys.Count() != 3)
                        throw new IOException("Exactly three decoy answers must exist for every QA!");

                    QAArray[i] = new QA(question, answer, decoys);

                    i++;
                }
            }
            catch (IOException e)
            {
                throw new IOException(e.ToString());
            }
        }

        //Shuffles an array using the Knuth Shuffle for efficieny and redundancy protection
        //Stores the randomly chosen element in the nextNdx reference
        private void Shuffle<T>(ref T[] array, ref int nextNdx)
        {
            //Set next index and check for end of array reached
            nextNdx++;
            if (nextNdx >= array.Count() - 1)
                nextNdx = 0;
            
            //Set indexes of random entry to be swapped
            int randNdx = rand.Next(nextNdx, array.Count());

            //Swap entries
            T oldEntry = array[nextNdx];
            T newEntry = array[randNdx];
            array[nextNdx] = newEntry;
            array[randNdx] = oldEntry;
        }
    }
}