using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouse
{
    class FormView : Form1, IView
    {
        int[] IView.InputPickets => throw new NotImplementedException();
    }
}
