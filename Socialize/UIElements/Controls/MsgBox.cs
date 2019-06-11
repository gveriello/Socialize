using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UnifyMe.UIElements.Controls
{
    public static class MsgBox
    {
        public static void Show(string title, string body)
        {
            MessageBox.Show(body, title);
        }
    }
}
