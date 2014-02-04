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
    /// Interaction logic for PlayPauseControl.xaml
    /// </summary>
    public partial class PlayPauseControl : UserControl
    {
        public static readonly RoutedEvent PlayEvent = EventManager.RegisterRoutedEvent(
            "Play", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlayPauseControl));

        public static readonly RoutedEvent StopEvent = EventManager.RegisterRoutedEvent(
            "Stop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlayPauseControl));

        private static readonly DependencyPropertyKey IsPlayingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsPlaying",
            typeof(bool),
            typeof(PlayPauseControl),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnIsPlayingChanged)));

        public static readonly DependencyProperty IsPlayingProperty = IsPlayingPropertyKey.DependencyProperty;

        private static void OnIsPlayingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (PlayPauseControl)o;
            var eventArgs = new RoutedEventArgs(ctrl.IsPlaying ? PlayPauseControl.PlayEvent : PlayPauseControl.StopEvent);
            ctrl.RaiseEvent(eventArgs);
        }

        public static readonly DependencyProperty PlayPauseCommandProperty = DependencyProperty.Register(
          "PlayPauseCommand",
          typeof(ICommand),
          typeof(PlayPauseControl),
          new FrameworkPropertyMetadata(null, OnPlayPauseCommandChanged));

        private static void OnPlayPauseCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (PlayPauseControl)o;
            ctrl.PlayPauseButton.Command = ctrl.PlayPauseCommand;
        }

        private void PlayPauseControl_Click(object sender, RoutedEventArgs e)
        {
            this.IsPlaying = !this.IsPlaying;
        }

        public PlayPauseControl()
        {
            InitializeComponent();
        }

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            protected set { SetValue(IsPlayingPropertyKey, value); }
        }

        public event RoutedEventHandler Play
        {
            add { AddHandler(PlayEvent, value); }
            remove { RemoveHandler(PlayEvent, value); }
        }

        public event RoutedEventHandler Stop
        {
            add { AddHandler(StopEvent, value); }
            remove { RemoveHandler(StopEvent, value); }
        }

        public ICommand PlayPauseCommand
        {
            get { return (ICommand)GetValue(PlayPauseCommandProperty); }
            set { SetValue(PlayPauseCommandProperty, value); }
        }
    }
}
