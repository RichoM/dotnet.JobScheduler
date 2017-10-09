using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobScheduler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log("Welcome!");
            Scheduler.Start();
        }

        private void Log(string str)
        {
            Invoke((Action)delegate
            {
                logTextBox.AppendText(string.Format("{0}) ", DateTime.Now));
                logTextBox.AppendText(str);
                logTextBox.AppendText(Environment.NewLine);
            });
        }

        private void Log(string formatString, params object[] args)
        {
            Log(string.Format(formatString, args));
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Text = string.Format("Job Scheduler ({0}, {1} jobs)",
                Scheduler.IsRunning ? "Running..." : "Stopped", Scheduler.JobCount);
        }

        private void inFutureButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds into the future?", "1000"));
            Log("Before!");
            Scheduler.ExecuteAfter(ms.Milliseconds(), () =>
            {
                Log("Future!");
            });
            Log("After!");
        }

        char[] letters = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ".ToCharArray();
        private void loopButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            char letter = letters[0];
            letters = letters.Skip(1).ToArray();
            Scheduler.Loop(() =>
            {
                Log("{0} - {1}", letter, ++i);
            });
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Scheduler.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Scheduler.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Scheduler.Flush();
        }

        private void loopEveryButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds?", "1000"));

            int i = 0;
            char letter = letters[0];
            letters = letters.Skip(1).ToArray();
            Scheduler.LoopEvery(ms.Milliseconds(), () =>
            {
                Log("{0}{0} - {1}", letter, ++i);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int times = int.Parse(InputBox.Ask("Times?", "3"));
            int delay = int.Parse(InputBox.Ask("Delay ms?", "1000"));
            double successRate = double.Parse(InputBox.Ask("Success rate?", "0.5"));

            Random rnd = new Random();
            int i = 0;
            Log("Starting retry (times: {0}, delay: {1} ms, successRate: {2})", 
                times, delay, successRate);
            Scheduler.Retry(times, delay.Milliseconds(), () =>
            {
                try
                {
                    Log("Attempt: {0}", ++i);
                    if (rnd.NextDouble() < successRate)
                    {
                        Log("SUCCESS!");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Log("FAIL :(");
                    throw;
                }
            });
        }
    }
}
