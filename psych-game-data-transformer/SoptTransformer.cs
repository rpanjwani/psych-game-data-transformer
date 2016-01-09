using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace psych_game_data_transformer
{
    class SoptTransformer
    {
        public const string OutputFileName = "SOPT_Outcomes.csv";
        private int maxErrorCount = 0;

        public void Transform(string inputFolderpath, string outputFolderPath)
        {
            if(Directory.Exists(inputFolderpath) && Directory.Exists(outputFolderPath))
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
            foreach(var file in files.Where(x => x.EndsWith("items.txt")).OrderByDescending(x => x))
            {
                var subId = getSubjectId(file);
                var numItems = getNumItems(file);
                var errorList = getFileErrorList(file);
                rowContent.Append(getTransformedFileData(subId, numItems, errorList));
                setMaxErrors(errorList.Count());
            }
            var content = getHeader(maxErrorCount).Append(rowContent).ToString();
            File.WriteAllText(outputFolderPath + "\\" + OutputFileName, content);
        }

        private void setMaxErrors(int errorCount)
        {
            if (maxErrorCount < errorCount)
                maxErrorCount = errorCount;
        }
       
        private string getTransformedFileData(string subId, string numItems, string[] errorList)
        {
            string outputLine = string.Format("{0},{1},", subId, numItems);
            outputLine += String.Join(",", errorList);
            outputLine += "\n";
            return outputLine;
        }

        private StringBuilder getHeader(int errorCount)
        {
            var header = new StringBuilder();
            header.Append("Subid,Items,");
            for(var i=1; i<=errorCount; i++)
            {
                header.Append("Error" + i + ",");
            }
            header.Append("\n");
            return header;
        }

        private string[] getFileErrorList(string inputFilePath)
        {
            var errorList = new List<string>();
            foreach (var line in File.ReadAllLines(inputFilePath))
            {
                if (line.StartsWith("No. Errors"))
                {
                    errorList.Add(line.Split('=')[1].Trim());
                }
            }
            return errorList.ToArray();
        }

        private string getNumItems(string inputFilePath)
        {
            return inputFilePath.Split('.')[1].Split('-')[0];
        }

        private string getSubjectId(string inputFilePath)
        {
            return inputFilePath.Split('.')[0].Split('\\').Last();
        }
    }
}
