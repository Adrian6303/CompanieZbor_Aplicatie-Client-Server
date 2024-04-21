namespace CZbor.client
{
    partial class Search
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
            this.destinationComboBox = new System.Windows.Forms.ComboBox();
            this.dataPlecareDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.logoutButton = new System.Windows.Forms.Button();
            this.buyButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.zboruriDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.zboruriDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // destinationComboBox
            // 
            this.destinationComboBox.FormattingEnabled = true;
            this.destinationComboBox.Location = new System.Drawing.Point(106, 58);
            this.destinationComboBox.Name = "destinationComboBox";
            this.destinationComboBox.Size = new System.Drawing.Size(163, 24);
            this.destinationComboBox.TabIndex = 0;
            // 
            // dataPlecareDateTimePicker
            // 
            this.dataPlecareDateTimePicker.Location = new System.Drawing.Point(333, 58);
            this.dataPlecareDateTimePicker.Name = "dataPlecareDateTimePicker";
            this.dataPlecareDateTimePicker.Size = new System.Drawing.Size(239, 22);
            this.dataPlecareDateTimePicker.TabIndex = 1;
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(42, 484);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(93, 29);
            this.logoutButton.TabIndex = 2;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // buyButton
            // 
            this.buyButton.Location = new System.Drawing.Point(400, 484);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(93, 29);
            this.buyButton.TabIndex = 3;
            this.buyButton.Text = "Buy";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.buyButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(645, 53);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(90, 31);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // zboruriDataGridView
            // 
            this.zboruriDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.zboruriDataGridView.Location = new System.Drawing.Point(75, 121);
            this.zboruriDataGridView.Name = "zboruriDataGridView";
            this.zboruriDataGridView.RowHeadersWidth = 51;
            this.zboruriDataGridView.RowTemplate.Height = 24;
            this.zboruriDataGridView.Size = new System.Drawing.Size(686, 340);
            this.zboruriDataGridView.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Choose Destination";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Choose Depature Date";
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 548);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zboruriDataGridView);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.buyButton);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.dataPlecareDateTimePicker);
            this.Controls.Add(this.destinationComboBox);
            this.Name = "Search";
            this.Text = "Search";
            ((System.ComponentModel.ISupportInitialize)(this.zboruriDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox destinationComboBox;
        private System.Windows.Forms.DateTimePicker dataPlecareDateTimePicker;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button buyButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.DataGridView zboruriDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}