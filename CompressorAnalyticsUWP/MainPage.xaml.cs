using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CompressorAnalyticsUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            SerialHelper serialHelper = new SerialHelper(this);

            // connect to arduino and start listening to data
            serialHelper.ConnectToArduino();

        }

        public void updateArduinoID(string msg)
        {
            this.arduinoSerialId.Text = msg;
        }
        public void updateArduinoStatus(string msg)
        {
            this.arduinoSerialStatus.Text = msg;
        }
        public void updateArduinoData(string msg)
        {
            this.arduinoSerialData.Text = msg;
        }
    }
}
