namespace alesBlockChain
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startMiner = new System.Windows.Forms.Button();
            this.numericPort = new System.Windows.Forms.NumericUpDown();
            this.connectButton = new System.Windows.Forms.Button();
            this.blockChainDisplay = new System.Windows.Forms.RichTextBox();
            this.listeningLabel = new System.Windows.Forms.Label();
            this.clientsConnectedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericPort)).BeginInit();
            this.SuspendLayout();
            // 
            // startMiner
            // 
            this.startMiner.Location = new System.Drawing.Point(12, 399);
            this.startMiner.Name = "startMiner";
            this.startMiner.Size = new System.Drawing.Size(103, 39);
            this.startMiner.TabIndex = 0;
            this.startMiner.Text = "Start Miner";
            this.startMiner.UseVisualStyleBackColor = true;
            this.startMiner.Click += new System.EventHandler(this.startMiner_Click);
            // 
            // numericPort
            // 
            this.numericPort.Enabled = false;
            this.numericPort.Location = new System.Drawing.Point(528, 415);
            this.numericPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericPort.Name = "numericPort";
            this.numericPort.Size = new System.Drawing.Size(120, 23);
            this.numericPort.TabIndex = 2;
            this.numericPort.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(654, 415);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(134, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // blockChainDisplay
            // 
            this.blockChainDisplay.Location = new System.Drawing.Point(12, 32);
            this.blockChainDisplay.Name = "blockChainDisplay";
            this.blockChainDisplay.ReadOnly = true;
            this.blockChainDisplay.Size = new System.Drawing.Size(776, 361);
            this.blockChainDisplay.TabIndex = 4;
            this.blockChainDisplay.Text = "";
            // 
            // listeningLabel
            // 
            this.listeningLabel.AutoSize = true;
            this.listeningLabel.Location = new System.Drawing.Point(121, 411);
            this.listeningLabel.Name = "listeningLabel";
            this.listeningLabel.Size = new System.Drawing.Size(0, 15);
            this.listeningLabel.TabIndex = 5;
            // 
            // clientsConnectedLabel
            // 
            this.clientsConnectedLabel.AutoSize = true;
            this.clientsConnectedLabel.Location = new System.Drawing.Point(12, 9);
            this.clientsConnectedLabel.Name = "clientsConnectedLabel";
            this.clientsConnectedLabel.Size = new System.Drawing.Size(114, 15);
            this.clientsConnectedLabel.TabIndex = 6;
            this.clientsConnectedLabel.Text = "Clients connected: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.clientsConnectedLabel);
            this.Controls.Add(this.listeningLabel);
            this.Controls.Add(this.blockChainDisplay);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.numericPort);
            this.Controls.Add(this.startMiner);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button startMiner;
        private NumericUpDown numericPort;
        private Button connectButton;
        private RichTextBox blockChainDisplay;
        private Label listeningLabel;
        private Label clientsConnectedLabel;
    }
}