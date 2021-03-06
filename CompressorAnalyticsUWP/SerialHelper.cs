﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace CompressorAnalyticsUWP
{
    public class SerialHelper
    {
        /// <summary>
        /// Private variables
        /// </summary>
        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;


        private ObservableCollection<DeviceInformation> listOfDevices;
        private CancellationTokenSource ReadCancellationTokenSource;

        private string arduinoSerialName = "Silicon Labs CP210x USB to UART Bridge (COM6)";

        public string deviceID;
        MainPage p;


        public SerialHelper(MainPage p)
        {
            // populate list of available ports
            listOfDevices = new ObservableCollection<DeviceInformation>();
            this.p = p;
            //
        }

        public async void ConnectToArduino()
        {
            try
            {
                string deviceSelector = SerialDevice.GetDeviceSelector();
                var devices = await DeviceInformation.FindAllAsync(deviceSelector);
                for (int i = 0; i < devices.Count; i++)
                {
                    listOfDevices.Add(devices[i]);
                }
                // update device id
                deviceID = listOfDevices.Where(x => x.Name == arduinoSerialName).Select(x => x.Id).FirstOrDefault();
                p.addLineToConsole("Serial Device ID: "+deviceID);

                // get serial port and confirm on screen
                serialPort = await SerialDevice.FromIdAsync(deviceID);


                if (serialPort == null) return;
                p.addLineToConsole("Serial Status OK");

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();

                p.addLineToConsole("Listening for serial data");
                Listen();

                return;
            }

            catch (Exception ex)
            {
                p.addLineToConsole(ex.Message);
                return;
            }




        }

        public async void Listen()
        {
            try
            {

                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException tce)
            {
                CloseDevice();
                p.updateDataLog("Reading task was cancelled, closing device and cleaning up. Error: "+tce.Message);
            }
            catch (Exception ex)
            {
                p.updateDataLog(ex.Message);
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            using (var childCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                // Create a task object to wait for data on the serialPort.InputStream
                loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(childCancellationTokenSource.Token);

                // Launch the task and wait
                UInt32 bytesRead = await loadAsyncTask;
                if (bytesRead > 0)
                {
                    string message = dataReaderObject.ReadString(bytesRead);
                    p.updateDataLog(message);
                    dynamic d = JsonConvert.DeserializeObject(message);
                    d.timeStamp = DateTime.Now;
                    message = JsonConvert.SerializeObject(d);
                    //SystemData data = JsonConvert.DeserializeObject<SystemData>(message);
                    //data.timeStamp = DateTime.Now;
                    //message = JsonConvert.SerializeObject(data);
                    p.updateDataLog(message);
                    CloudHelper.SendDeviceToCloudMessagesAsync(message);
                }
            }

        }


        /// <summary>
        /// CloseDevice:
        /// - Disposes SerialDevice object
        /// - Clears the enumerated device Id list
        /// </summary>
        private void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;

            listOfDevices.Clear();
        }
    }
}
