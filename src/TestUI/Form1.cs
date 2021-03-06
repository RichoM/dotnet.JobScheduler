﻿using System;
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
            JobScheduler.OnError += JobScheduler_OnError;
        }

        private void JobScheduler_OnError(Exception ex)
        {
            Log("ERROR:");
            Log(ex.ToString());
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
            Text = string.Format("Job Scheduler ({0}, {1} jobs, interval: {2} ms)",
                JobScheduler.IsRunning ? "Running..." : "Stopped", 
                JobScheduler.JobCount,
                JobScheduler.Interval);
        }

        private void inFutureButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds into the future?", "1000"));
            Log("Before!");
            Task.Run(() =>
            {
                var promise = JobScheduler
                    .ExecuteAfter(ms.Milliseconds(), () => Log("Future!"))
                    .Then(() => Log("FINISHED 1!"))
                    .Then(() => Log("FINISHED 2!"));
                Log("After!");
                Task.Delay(ms * 3).Wait();
                promise.Then(() => Log("FINISHED 3!"));
            });
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

        private void inFutureReturnButton_Click(object sender, EventArgs e)
        {
            int ms = int.Parse(InputBox.Ask("Milliseconds into the future?", "1000"));
            Log("Before!");
            var promise = JobScheduler
                .ExecuteAfter(ms.Milliseconds(), () =>
                {
                    Log("Future!");
                    return Environment.TickCount;
                })
                .Then(ign => Log("FINISHED 1!"))
                .Then(ign => Log("FINISHED 2!"));
            Log("After!");
            Thread.Sleep(ms * 2);
            promise.Then(ticks => Log("FINISHED 3! {0}", ticks));
        }

        private void retryReturnButton_Click(object sender, EventArgs e)
        {
            int times = int.Parse(InputBox.Ask("Times?", "3"));
            int delay = int.Parse(InputBox.Ask("Delay ms?", "1000"));
            double successRate = double.Parse(InputBox.Ask("Success rate?", "0.5"));

            Random rnd = new Random();
            int i = 0;
            Log("Starting retry (times: {0}, delay: {1} ms, successRate: {2})",
                times, delay, successRate);
            var promise = JobScheduler.Retry(times, delay.Milliseconds(), () =>
            {
                try
                {
                    Log("Attempt: {0}", ++i);
                    var randomValue = rnd.NextDouble();
                    if (randomValue < successRate)
                    {
                        Log("SUCCESS!");
                        return randomValue;
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
            promise.Then(val =>
            {
                Log("VALUE: {0}", val);
            });
            promise.Then(() =>
            {
                Log("FINISHED!");
            });
            promise.Then(() =>
            {
                Console.Beep();
            });
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int total = 100;
            int count = 0;
            int successes = 0;
            int errors = 0;
            for (int i = 0; i < total; i++)
            {
                int index = i;
                Log("{0}) Starting...", index);
                int attempts = 0;
                var promise = JobScheduler.Retry(3, 1.Seconds(), () =>
                {
                    try
                    {
                        Log("{0}) Attempt: {1}", index, ++attempts);
                        var randomValue = rnd.NextDouble();
                        Thread.Sleep((int)(randomValue * 500));
                        if (randomValue < 0.05)
                        {
                            Log("{0}) SUCCESS!", index);
                            return true;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        Log("{0}) FAIL :(", index);
                        throw;
                    }
                });
                promise.Then(val =>
                {
                    Log("{0}) VALUE: {1}", index, val);
                    if (val) { successes++; } else { errors++; }

                    try
                    {
                        if (val)
                        {
                            Thread.Sleep((int)(rnd.NextDouble() * 500));
                        }
                        else if (rnd.NextDouble() > 0.5)
                        {
                            throw new Exception("TU VIEJA!");
                        }
                    }
                    finally
                    {
                        count++;
                        if (count >= total)
                        {
                            Log("SUCCESSES: {0}, ERRORS: {1}", successes, errors);
                        }
                    }
                });
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int begin = Environment.TickCount;
            int action = 0;
            int end = 0;
            JobScheduler.Retry(1, 10.Seconds(), () =>
            {
                action = Environment.TickCount;
                throw new Exception("FAIL!");
            }).Then(() =>
            {
                end = Environment.TickCount;
            }).Then(()=>
            {
                Log("BEGIN:  {0}", begin);
                Log("ACTION: {0}", action);
                Log("END:    {0}", end);
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int minutes = int.Parse(InputBox.Ask("Minutes?", "1"));
            Random rnd = new Random();
            int total = 100;
            var promises = Enumerable.Range(0, total).Select(index =>
            {
                Log("{0}) Starting...", index);
                int attempts = 0;
                var promise = JobScheduler.Retry(3, minutes.Minutes(), () =>
                {
                    try
                    {
                        Log("{0}) Attempt: {1}", index, ++attempts);
                        var randomValue = rnd.NextDouble();
                        Thread.Sleep((int)(randomValue * 500));
                        if (randomValue < 0.05)
                        {
                            Log("{0}) SUCCESS!", index);
                            return true;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        Log("{0}) FAIL :(", index);
                        throw;
                    }
                });
                promise.Then(val =>
                {
                    Log("{0}) VALUE: {1}", index, val);
                    if (val)
                    {
                        Thread.Sleep((int)(rnd.NextDouble() * 500));
                    }
                    else if (rnd.NextDouble() > 0.5)
                    {
                        throw new Exception("TU VIEJA!");
                    }
                });
                return promise;
            });
            Promise.All(promises).Then(results =>
            {
                int successes = results.Count(each => each == true);
                int errors = results.Count(each => each == false);
                Log("SUCCESSES: {0}, ERRORS: {1}", successes, errors);
            });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            int begin = Environment.TickCount;
            Log("BEGIN AT {0}", begin);
            int count = 0;
            var promises = Enumerable.Range(0, 1000).Select(index =>
            {
                var delay = rnd.Next(60).Seconds();
                return JobScheduler.ExecuteAfter(delay, () =>
                {
                    Interlocked.Increment(ref count);
                    Log("{2}) RUN: {0} (delay: {1})", index, delay,
                        Interlocked.CompareExchange(ref count, 0, 0));
                });
            });
            Promise.All(promises).Then(() =>
            {
                int end = Environment.TickCount;
                Log("END AT {0}", end);
                Log("TIME TO RUN: {0} ms", end - begin);
            });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            int begin = Environment.TickCount;
            var rnd = new Random();
            timer.Elapsed += (ign1, ign2) =>
            {
                int tick = Environment.TickCount;
                Log("Interval: {0}, Tick: {1}", timer.Interval, tick - begin);

                if (timer.Interval < 100) timer.Interval *= 2;
                else if (timer.Interval > 10000) timer.Interval /= 2;
                else
                {
                    timer.Interval = rnd.NextDouble() > 0.5 ? timer.Interval * 2 : timer.Interval / 2;
                }
                begin = Environment.TickCount;
            };
            timer.Start();
        }
    }
}
