using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobScheduling
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
            JobScheduler.Start();
        }

        private void Log(string str)
        {
            Invoke((Action)delegate
            {
                logTextBox.AppendText(string.Format("{0} /// ", DateTime.Now.ToString("o")));
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
                JobScheduler.IsRunning ? "Running..." : "Stopped", JobScheduler.JobCount);
        }

        private void inFutureButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds into the future?", "1000"));
            Log("Before!");
            var promise = JobScheduler
                .ExecuteAfter(ms.Milliseconds(), () => Log("Future!"))
                .Then(() => Log("FINISHED 1!"))
                .Then(() => Log("FINISHED 2!"));
            Log("After!");
            Thread.Sleep(ms * 2);
            promise.Then(() => Log("FINISHED 3!"));
        }

        char[] letters = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ".ToCharArray();
        private void loopButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            char letter = letters[0];
            letters = letters.Skip(1).ToArray();
            JobScheduler.Loop(() =>
            {
                Log("{0} - {1}", letter, ++i);
            });
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            JobScheduler.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            JobScheduler.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JobScheduler.Flush();
        }

        private void loopEveryButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds?", "1000"));

            int i = 0;
            char letter = letters[0];
            letters = letters.Skip(1).ToArray();
            JobScheduler.LoopEvery(ms.Milliseconds(), () =>
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
            JobScheduler.Retry(times, delay.Milliseconds(), () =>
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
            }).Then(()=>
            {
                Log("FINISHED!");
            });
        }
    }
}
