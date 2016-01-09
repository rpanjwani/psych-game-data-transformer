using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psych_game_data_transformer
{
    class XylophoneTransformer
    {
        public const string OutputFileName = "Xylophone_Outcomes.csv";
        

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
            foreach (var file in files.Where(x => x.EndsWith(".xyl")).OrderByDescending(x => x))
            {
                var subId = getSubjectId(file);
                rowContent.Append(getTransformedFileData(subId, file));
            }
            var content = getHeader().Append(rowContent).ToString();
            File.WriteAllText(outputFolderPath + "\\" + OutputFileName, content);
        }

        private string getSubjectId(string inputFilePath)
        {
            return inputFilePath.Split('.')[0].Split('\\').Last();
        }

        private string getTransformedFileData(string subId, string inputFilePath)
        {
            var outData = new StringBuilder();
            //skip till --- encountered, and then skip 2 and take all lines till "VALID" encountered.
            var lines = File.ReadAllLines(inputFilePath)
                .SkipWhile(x => !x.EndsWith("---\""))
                .Skip(1)
                .TakeWhile(x => !x.StartsWith("VALID"));
            //don't take the last line
            for(var i=0; i<lines.Count()-1; i++)
            {
                var outputLine = new StringBuilder(subId + ",");
                var cols = lines.ElementAt(i).Split(',');
                outputLine
                    .Append(cols[0] + ",")
                    .Append(cols[1].Trim('"') + ",")
                    .Append(cols[4] + ",")
                    .Append(cols[5] + ",")
                    .Append(cols[6].Trim('"'))
                    .Append("\n");
                outData.Append(outputLine);
            }
            
            return outData.ToString();
        }

        private StringBuilder getHeader()
        {
            return new StringBuilder("Subid, Trial, Condition, Score, Latency, Perception\n");
        }
    }
}
