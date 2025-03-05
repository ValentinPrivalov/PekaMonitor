using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
using System.Windows.Threading;
using OxyPlot.Wpf;
using OxyPlot;
using System.Windows.Interop;
using Microsoft.Win32;

namespace PekaMonitor
{
    public partial class MainWindow : Window
    {
        private HardwareMonitor hardwareMonitor;
        private HardwareViewModel viewModel;
        private DispatcherTimer timer;
        private DateTime lastUpdate = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();
            hardwareMonitor = new HardwareMonitor();
            viewModel = new HardwareViewModel();
            DataContext = viewModel;

            Loaded += MainWindow_Loaded;

            timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Title = $"PekaMonitor v.{version.Major}.{version.Minor}.{version.Build}";
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            //DebugUpdateInterval();
            hardwareMonitor.GatherData();
            HardwareData hardwareData = hardwareMonitor.GetHardwareData();
            viewModel.UpdateValues(hardwareData);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsDarkTheme();
            hardwareMonitor.GatherData();
            HardwareData hardwareData = hardwareMonitor.GetHardwareData();

            CpuName.Text = hardwareData.cpuName;
            MotherBoardName.Text = hardwareData.motherboardName;
            GpuName.Text = hardwareData.gpuName;
            Storage1Name.Text = hardwareData.storage1Name;

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseEnter(OxyPlot.PlotCommands.HoverSnapTrack);

            CpuLoadView.Model = viewModel.CpuLoadDiagram.GetPlotModel();
            CpuTempView.Model = viewModel.CpuTempDiagram.GetPlotModel();
            CpuFanView.Model = viewModel.CpuFanDiagram.GetPlotModel();
            MemoryLoadView.Model = viewModel.MemoryLoadDiagram.GetPlotModel();
            CpuTDPView.Model = viewModel.CpuTDPDiagram.GetPlotModel();
            CpuLoadView.Controller = controller;
            CpuTempView.Controller = controller;
            CpuFanView.Controller = controller;
            MemoryLoadView.Controller = controller;
            CpuTDPView.Controller = controller;

            GpuLoadView.Model = viewModel.GpuLoadDiagram.GetPlotModel();
            GpuTempView.Model = viewModel.GpuTempDiagram.GetPlotModel();
            GpuFanView.Model = viewModel.GpuFanDiagram.GetPlotModel();
            GpuMemoryLoadView.Model = viewModel.GpuMemoryLoadDiagram.GetPlotModel();
            GpuTDPView.Model = viewModel.GpuTDPDiagram.GetPlotModel();
            GpuLoadView.Controller = controller;
            GpuTempView.Controller = controller;
            GpuFanView.Controller = controller;
            GpuMemoryLoadView.Controller = controller;
            GpuTDPView.Controller = controller;

            Storage1TempView.Model = viewModel.Storage1TempDiagram.GetPlotModel();
            Storage1TempView.Controller = controller;
        }

        protected override void OnClosed(System.EventArgs e)
        {
            timer.Stop();
            hardwareMonitor.Close();
            base.OnClosed(e);
        }

        private void DebugUpdateInterval()
        {
            var now = DateTime.Now;
            if (lastUpdate != DateTime.MinValue)
            {
                var interval = (now - lastUpdate).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine($"Interval between updates: {interval} ms");
            }
            lastUpdate = now;
        }

        private void IsDarkTheme()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            //using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            //return value is int intValue && intValue == 0; // 0 = Dark, 1 = Light
            System.Diagnostics.Debug.WriteLine($"Theme: {value}");
        }
    }
}
