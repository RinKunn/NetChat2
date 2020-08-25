using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2
{
    public interface ISetting
    {
        string DataFilePath { get; }
    }

    public class Configuration : ISetting
    {
        private string _dataFilePath;

        public string DataFilePath => _dataFilePath;

        public void Init()
        {

        }
    }
}
