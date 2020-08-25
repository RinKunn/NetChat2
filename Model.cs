using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2
{
    public class Model
    {
        private string _filename;

        public Model(string filename)
        {
            this._filename = !string.IsNullOrWhiteSpace(filename) ? filename : throw new ArgumentNullException(nameof(filename));
        }


    }
}
