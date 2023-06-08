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
