namespace JobScheduling
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.inFutureButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.loopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.loopEveryButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.inFutureReturnButton = new System.Windows.Forms.Button();
            this.retryReturnButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inFutureButton
            // 
            this.inFutureButton.Location = new System.Drawing.Point(14, 133);
            this.inFutureButton.Name = "inFutureButton";
            this.inFutureButton.Size = new System.Drawing.Size(151, 27);
            this.inFutureButton.TabIndex = 0;
            this.inFutureButton.Text = "In future do...";
            this.inFutureButton.UseVisualStyleBackColor = true;
            this.inFutureButton.Click += new System.EventHandler(this.inFutureButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTextBox.Location = new System.Drawing.Point(172, 13);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(950, 665);
            this.logTextBox.TabIndex = 1;
            // 
            // loopButton
            // 
            this.loopButton.Location = new System.Drawing.Point(15, 166);
            this.loopButton.Name = "loopButton";
            this.loopButton.Size = new System.Drawing.Size(151, 27);
            this.loopButton.TabIndex = 2;
            this.loopButton.Text = "Loop";
            this.loopButton.UseVisualStyleBackColor = true;
            this.loopButton.Click += new System.EventHandler(this.loopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(15, 13);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(151, 27);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(15, 46);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(151, 27);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Flush";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // loopEveryButton
            // 
            this.loopEveryButton.Location = new System.Drawing.Point(15, 199);
            this.loopEveryButton.Name = "loopEveryButton";
            this.loopEveryButton.Size = new System.Drawing.Size(151, 27);
            this.loopEveryButton.TabIndex = 6;
            this.loopEveryButton.Text = "Loop every...";
            this.loopEveryButton.UseVisualStyleBackColor = true;
            this.loopEveryButton.Click += new System.EventHandler(this.loopEveryButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 27);
            this.button2.TabIndex = 7;
            this.button2.Text = "Retry...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // inFutureReturnButton
            // 
            this.inFutureReturnButton.Location = new System.Drawing.Point(15, 278);
            this.inFutureReturnButton.Name = "inFutureReturnButton";
            this.inFutureReturnButton.Size = new System.Drawing.Size(151, 27);
            this.inFutureReturnButton.TabIndex = 8;
            this.inFutureReturnButton.Text = "In future return...";
            this.inFutureReturnButton.UseVisualStyleBackColor = true;
            this.inFutureReturnButton.Click += new System.EventHandler(this.inFutureReturnButton_Click);
            // 
            // retryReturnButton
            // 
            this.retryReturnButton.Location = new System.Drawing.Point(15, 311);
            this.retryReturnButton.Name = "retryReturnButton";
            this.retryReturnButton.Size = new System.Drawing.Size(151, 27);
            this.retryReturnButton.TabIndex = 9;
            this.retryReturnButton.Text = "Retry return...";
            this.retryReturnButton.UseVisualStyleBackColor = true;
            this.retryReturnButton.Click += new System.EventHandler(this.retryReturnButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(15, 651);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(151, 27);
            this.clearButton.TabIndex = 10;
            this.clearButton.Text = "Clear log";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 354);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(151, 27);
            this.button3.TabIndex = 11;
            this.button3.Text = "Multiple lasting retries";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(15, 387);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(151, 27);
            this.button4.TabIndex = 12;
            this.button4.Text = "Test retry promise resolve";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(15, 420);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(151, 27);
            this.button5.TabIndex = 13;
            this.button5.Text = "Multiple lasting retries (All)";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(15, 453);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(151, 27);
            this.button6.TabIndex = 14;
            this.button6.Text = "Test Promise.All()";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(15, 495);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(151, 27);
            this.button7.TabIndex = 15;
            this.button7.Text = "Test Timer.Interval";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 690);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.retryReturnButton);
            this.Controls.Add(this.inFutureReturnButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.loopEveryButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.loopButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.inFutureButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button inFutureButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button loopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button loopEveryButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button inFutureReturnButton;
        private System.Windows.Forms.Button retryReturnButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

