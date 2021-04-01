using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReader.Core
{
    public class ParsingArgs
    {
        public string TemplateFile { get; set; }
        public string ResultPath { get; set; }
        public DoWorkEventArgs WorkerArgs { get; set; }
        public int VolumeSize { get; set; }
        public int DigitNum { get; set; }

        public ParsingArgs(string templateFile, string resultPath, DoWorkEventArgs workerArgs, int volumeSize, int digitNum)
        {
            this.TemplateFile = templateFile;
            this.ResultPath = resultPath;
            this.WorkerArgs = workerArgs;
            this.VolumeSize = volumeSize;
            this.DigitNum = digitNum;
        }
    }
}
