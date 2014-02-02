using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProxySwarm.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for BigIndicator.xaml
    /// </summary>
    public partial class BigIndicatorControl : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText",
          typeof(string),
          typeof(BigIndicatorControl),
          new FrameworkPropertyMetadata(string.Empty,
              FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IndicatorTextProperty = DependencyProperty.Register(
          "IndicatorText",
          typeof(string),
          typeof(BigIndicatorControl),
          new FrameworkPropertyMetadata(string.Empty,
              FrameworkPropertyMetadataOptions.AffectsRender));

        public BigIndicatorControl()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public string IndicatorText
        {
            get { return (string)GetValue(IndicatorTextProperty); }
            set { SetValue(IndicatorTextProperty, value); }
        }
    }
}
