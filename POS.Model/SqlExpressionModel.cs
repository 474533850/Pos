using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class SqlExpressionModel
    {
        public string CmdText { get; set; }

        public object Parameters { get; set; }
    }
}
