using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UnifyMe.Core.Classes;

namespace UnifyMe.UIElements.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        //Grid listOfColors;
        public ColorPicker()
        {
            InitializeComponent();
            this.Loaded += this.ColorPicker_Loaded;
            openColorPicker.Click += this.OpenColorPicker_Click;
        }

        #region ColorSelected
        public static DependencyProperty ColorSelectedProperty =
            DependencyProperty.Register("ColorSelected",
            typeof(ColorInfo),
            typeof(ColorPicker),
            new PropertyMetadata(default(ColorInfo), OnColorSelectedChanged));

        public ColorInfo ColorSelected
        {
            get { return (ColorInfo)GetValue(ColorSelectedProperty); }
            set { SetValue(ColorSelectedProperty, value); }
        }

        private static void OnColorSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorInfo newColor = (ColorInfo)e.NewValue;
            ColorPicker instance = ((ColorPicker)d);
            instance.txtColorName.Text = newColor.ColorName;
            instance.listColors.IsOpen = false;
        }
        #endregion

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            List<ColorInfo> webColors = GetConstants();

            int cellCount = webColors.Count;
            int numCols = cellCount / 10;
            int numRows = (cellCount + 1) / numCols;
            this.InitializeGrid(cellCount);

            for (int i = 0; i < cellCount; ++i)
            {
                int idx = listOfColors.Children.Add(new StackPanel());
                StackPanel stackColor = listOfColors.Children[idx] as StackPanel;

                stackColor.Background = new SolidColorBrush(webColors.ElementAt(i).Color);
                stackColor.Tag = webColors.ElementAt(i).ColorName;
                stackColor.MouseLeftButtonUp += this.StackColor_MouseLeftButtonUp;
                stackColor.SetValue(Grid.RowProperty, i / numCols);
                stackColor.SetValue(Grid.ColumnProperty, i % numCols);
            }
        }

        private void OpenColorPicker_Click(object sender, RoutedEventArgs e)
        {
            listColors.IsOpen = !listColors.IsOpen;
        }

        private void StackColor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel panelSender = sender as StackPanel;
            ColorInfo colorSelected = new ColorInfo(panelSender.Tag.ToString(), ((SolidColorBrush)panelSender.Background).Color);
            this.ColorSelected = colorSelected;
        }

        private void InitializeGrid(int totCells)
        {
            //this.listOfColors = new Grid();
            int cellCount = totCells;
            int numCols = cellCount / 10;
            int numRows = (cellCount + 1) / numCols;
            for (int i = 0; i < numCols; ++i)
                this.listOfColors.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(20)
                });
            for (int i = 0; i < numRows; ++i)
                this.listOfColors.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(20)
                });
        }

        private List<ColorInfo> GetConstants()
        {
            List<ColorInfo> list = new List<ColorInfo>();
            var color_query = from PropertyInfo property in typeof(Colors).GetProperties()
                              orderby property.Name
                              select new ColorInfo(
                              property.Name,
                              (Color)property.GetValue(null, null));

            foreach (ColorInfo color in color_query)
                list.Add(color);
            return list;
        }
    }
}
