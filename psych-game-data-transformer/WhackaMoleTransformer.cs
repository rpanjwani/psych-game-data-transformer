using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psych_game_data_transformer
{
    public class WhackaMoleTransformer
    {
        public const string OutputFileName = "Whackamole_Outcomes.csv";

        public void Transform(string inputFolderpath, string outputFolderPath)
        {
            if (Directory.Exists(inputFolderpath) && Directory.Exists(outputFolderPath))
            {
                processDirectory(inputFolderpath, outputFolderPath);
            }
            else
            {
                throw new Exception("Can't process. Please check the input and output paths.");
            }
        }

        private void processDirectory(string inputFolderpath, string outputFolderPath)
        {
            var files = Directory.GetFiles(inputFolderpath);
            var rowContent = new StringBuilder();
            foreach (var file in files.Where(x => x.EndsWith(".txt")).OrderByDescending(x => x))
            {
                var subId = getSubjectId(file);
                var sessionId = getSessionId(file);
                rowContent.Append(getTransformedFileData(subId, sessionId, file));
            }
            var content = getHeader().Append(rowContent).ToString();
            File.WriteAllText(outputFolderPath + "\\" + OutputFileName, content);
        }

        private string getSubjectId(string inputFilePath)
        {
            return inputFilePath.Split('-')[1];
        }

        private string getSessionId(string inputFilePath)
        {
            return inputFilePath.Split('-')[2].Split('.')[0];
        }


        private string getTransformedFileData(string subId, string sessionId, string inputFilePath)
        {
            var outData = new StringBuilder();
            var lines = File.ReadAllLines(inputFilePath);
            
            for (var i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("Procedure: go") || lines[i].Contains("Procedure: ng"))
                {
                    var condition = lines[i].Split(':')[1].Trim();
                    while (!lines[i].Contains(".ACC")) i++;
                    var accuracy = lines[i].Split(':')[1].Trim();
                    i++;
                    var rt = lines[i].Split(':')[1].Trim();

                    var outputLine = string.Format("{0},{1},{2},{3},{4}\n",
                        subId, sessionId, condition, accuracy, rt);

                    outData.Append(outputLine);
                }
            }

            return outData.ToString();
        }

        private StringBuilder getHeader()
        {
            return new StringBuilder("Subid, Session, Condition, Accuracy, RT\n");
        }
    }
}
