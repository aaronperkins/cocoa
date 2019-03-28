using Cocoa.Hal.Models;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cocoa.Hal
{
    public class Driver : IDisposable
    {
        protected const int READ_TIMEOUT = 1000;
        protected const int WRITE_TIMEOUT = 1000;
        protected static SemaphoreSlim _syncPose = new SemaphoreSlim(1, 1);
        protected static SemaphoreSlim _syncWrite = new SemaphoreSlim(1, 1);
        protected readonly int _baudRate;
        protected readonly SerialPort _port;
        protected readonly string _portName;
        protected readonly StringBuilder _receiveBuffer;
        protected Pose _currentPose = new Pose();
        protected Pose _lastFinishedPose = new Pose();
        protected System.Timers.Timer _statusTimer;

        public Driver(string portName, int baudRate)
        {
            _portName = portName;
            _baudRate = baudRate;
            _port = new SerialPort();
            _port.RtsEnable = true;
            _port.ReadTimeout = READ_TIMEOUT;
            _receiveBuffer = new StringBuilder();
            _port.DataReceived += PortDataReceived;
            _statusTimer = new System.Timers.Timer(100);
            _statusTimer.Elapsed += StatusTimerElapsed;
            _statusTimer.Enabled = true;
        }

        public Pose CurrentPose
        {
            get
            {
                return _currentPose;
            }
        }

        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }

        public void Close()
        {
            if (_port.IsOpen)
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
            if (!_port.IsOpen)
            {
                _port.PortName = _portName;
                _port.BaudRate = _baudRate;

                _port.Open();

                Task task = Task.Run(() => PollCurrentPose());
                task.Wait();
            }
        }

        public async Task Relax()
        {
            await WriteData("<R>)");
        }

        public async Task<Pose> SetPose(Pose pose)
        {
            await _syncPose.WaitAsync();

            try
            {
                int newPoseId = pose.Id;
                if (_lastFinishedPose.Id == newPoseId)
                    newPoseId++;

                var data = pose.ToString();
                await WriteData(data);

                var timer = new Stopwatch();
                timer.Start();
                while (newPoseId != _lastFinishedPose.Id)
                {
                    if (timer.ElapsedMilliseconds > pose.TotalTime + 2500)
                    {
                        return null;
                    }

                    await Task.Yield();
                }

                return _lastFinishedPose;
            }
            finally
            {
                _syncPose.Release();
            }
        }

        protected void ParseMessage(string message)
        {
            string instruction = message.Substring(1, 1);
            switch (instruction)
            {
                case "P":
                    _currentPose = new Pose(message);
                    break;

                case "F":
                    _currentPose = new Pose(message);
                    _lastFinishedPose = _currentPose;
                    break;
            }
        }

        protected async Task PollCurrentPose()
        {
            if (_port.IsOpen)
            {
                await WriteData("<C>)");
            }
        }

        protected void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
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

        protected void StatusTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task task = Task.Run(() => PollCurrentPose());
            task.Wait();
        }

        protected async Task<bool> WriteData(string data)
        {
            await _syncWrite.WaitAsync();
            try
            {
                var dataBytes = Encoding.ASCII.GetBytes(data);
                _port.Write(data);

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