using Cocoa.Data.Models;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cocoa.Hal
{
    public class Driver : IDisposable
    {
        private const int READ_TIMEOUT = 1000;
        private const int WRITE_TIMEOUT = 1000;
        private static SemaphoreSlim _syncWrite = new SemaphoreSlim(1, 1);
        private readonly SerialPort _port;
        private readonly StringBuilder _receiveBuffer;
        private System.Timers.Timer _statusTimer;

        public Driver(string portName, int baudRate, int updateRate = 33)
        {
            _port = new SerialPort
            {
                BaudRate = baudRate,
                PortName = portName,
                RtsEnable = true,
                ReadTimeout = READ_TIMEOUT
            };
            _receiveBuffer = new StringBuilder();
            _port.DataReceived += PortDataReceived;
            _statusTimer = new System.Timers.Timer(updateRate);
            _statusTimer.Elapsed += StatusTimerElapsed;
            _statusTimer.Enabled = true;
        }

        public Pose CurrentPose { get; private set; } = new Pose();

        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }

        public Pose LastFinishedPose { get; private set; } = new Pose();

        public void Close()
        {
            if (IsOpen)
            {
                _port.Close();
            }
        }

        public void Dispose()
        {
            Close();
            _port.Dispose();
        }

        public void Open()
        {
            if (!IsOpen)
            {
                _port.Open();

                Task task = Task.Run(() => PollCurrentPose());
                task.Wait();
            }
        }

        public async Task Relax()
        {
            await WriteData("<R>)");
        }

        public async Task<bool> SendPose(Pose pose)
        {          
            var data = pose.ToString();
            return await WriteData(data);
        }

        private void ParseMessage(string message)
        {
            string instruction = message.Substring(1, 1);
            switch (instruction)
            {
                case "P":
                    CurrentPose = new Pose(message);
                    break;

                case "F":
                    CurrentPose = new Pose(message);
                    LastFinishedPose = CurrentPose;
                    break;

                case "D":
                    Console.WriteLine(message);
                    break;
            }
        }

        private async Task PollCurrentPose()
        {
            await WriteData("<C>)");
        }

        private void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_receiveBuffer)
            {
                string newData = null;

                try
                {
                    newData = _port.ReadExisting();
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Read timeout!");
                    return;
                }

                _receiveBuffer.Append(newData);

                var receiveString = _receiveBuffer.ToString();
                int start = receiveString.IndexOf('<');
                int end = receiveString.IndexOf('>', start);
                int length = end - start;
                if (start >= 0 && end > 0)
                {
                    var message = receiveString.Substring(start, length + 1);
                    _receiveBuffer.Remove(0, end + 1);
                    ParseMessage(message);
                }
            }
        }

        private void StatusTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task task = Task.Run(() => PollCurrentPose());
            task.Wait();
        }

        private async Task<bool> WriteData(string data)
        {
            if (!IsOpen)
                return false;

            await _syncWrite.WaitAsync();
            try
            {
                var dataBytes = Encoding.ASCII.GetBytes(data);

                var writeTask = _port.BaseStream.WriteAsync(dataBytes, 0, dataBytes.Length);
                if (await Task.WhenAny(writeTask, Task.Delay(WRITE_TIMEOUT)) == writeTask)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Write timeout.");
                    return false;
                }
            }
            finally
            {
                _syncWrite.Release();
            }
        }
    }
}