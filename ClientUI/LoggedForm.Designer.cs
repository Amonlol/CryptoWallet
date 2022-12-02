
namespace ClientUI
{
	partial class LoggedForm
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
			this.BalanceBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.DepositButton = new System.Windows.Forms.Button();
			this.TransferButton = new System.Windows.Forms.Button();
			this.WithdrawButton = new System.Windows.Forms.Button();
			this.DepositBox = new System.Windows.Forms.TextBox();
			this.AddressBox = new System.Windows.Forms.TextBox();
			this.TransferBox = new System.Windows.Forms.TextBox();
			this.WithdrawBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BalanceBox
			// 
			this.BalanceBox.Location = new System.Drawing.Point(200, 24);
			this.BalanceBox.Multiline = true;
			this.BalanceBox.Name = "BalanceBox";
			this.BalanceBox.ReadOnly = true;
			this.BalanceBox.Size = new System.Drawing.Size(320, 24);
			this.BalanceBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Средства на вашем счету:";
			// 
			// DepositButton
			// 
			this.DepositButton.Location = new System.Drawing.Point(48, 80);
			this.DepositButton.Name = "DepositButton";
			this.DepositButton.Size = new System.Drawing.Size(120, 23);
			this.DepositButton.TabIndex = 3;
			this.DepositButton.Text = "Внести средства";
			this.DepositButton.UseVisualStyleBackColor = true;
			this.DepositButton.Click += new System.EventHandler(this.Button_Click);
			// 
			// TransferButton
			// 
			this.TransferButton.Location = new System.Drawing.Point(48, 248);
			this.TransferButton.Name = "TransferButton";
			this.TransferButton.Size = new System.Drawing.Size(120, 23);
			this.TransferButton.TabIndex = 4;
			this.TransferButton.Text = "Перевести средства";
			this.TransferButton.UseVisualStyleBackColor = true;
			this.TransferButton.Click += new System.EventHandler(this.Button_Click);
			// 
			// WithdrawButton
			// 
			this.WithdrawButton.Location = new System.Drawing.Point(48, 144);
			this.WithdrawButton.Name = "WithdrawButton";
			this.WithdrawButton.Size = new System.Drawing.Size(120, 23);
			this.WithdrawButton.TabIndex = 5;
			this.WithdrawButton.Text = "Вывести средства";
			this.WithdrawButton.UseVisualStyleBackColor = true;
			this.WithdrawButton.Click += new System.EventHandler(this.Button_Click);
			// 
			// DepositBox
			// 
			this.DepositBox.Location = new System.Drawing.Point(200, 80);
			this.DepositBox.Multiline = true;
			this.DepositBox.Name = "DepositBox";
			this.DepositBox.Size = new System.Drawing.Size(320, 24);
			this.DepositBox.TabIndex = 6;
			this.DepositBox.Text = "Укажите сумму внесения";
			this.DepositBox.TextChanged += new System.EventHandler(this.MaskInput);
			this.DepositBox.Enter += new System.EventHandler(this.BoxEnter);
			this.DepositBox.Leave += new System.EventHandler(this.BoxExit);
			// 
			// AddressBox
			// 
			this.AddressBox.Location = new System.Drawing.Point(48, 208);
			this.AddressBox.Multiline = true;
			this.AddressBox.Name = "AddressBox";
			this.AddressBox.Size = new System.Drawing.Size(472, 24);
			this.AddressBox.TabIndex = 7;
			this.AddressBox.Text = "Укажите адрес получателя";
			this.AddressBox.TextChanged += new System.EventHandler(this.MaskInput);
			this.AddressBox.Enter += new System.EventHandler(this.BoxEnter);
			this.AddressBox.Leave += new System.EventHandler(this.BoxExit);
			// 
			// TransferBox
			// 
			this.TransferBox.Location = new System.Drawing.Point(200, 248);
			this.TransferBox.Multiline = true;
			this.TransferBox.Name = "TransferBox";
			this.TransferBox.Size = new System.Drawing.Size(320, 24);
			this.TransferBox.TabIndex = 8;
			this.TransferBox.Text = "Укажите сумму перевода";
			this.TransferBox.TextChanged += new System.EventHandler(this.MaskInput);
			this.TransferBox.Enter += new System.EventHandler(this.BoxEnter);
			this.TransferBox.Leave += new System.EventHandler(this.BoxExit);
			// 
			// WithdrawBox
			// 
			this.WithdrawBox.Location = new System.Drawing.Point(200, 144);
			this.WithdrawBox.Multiline = true;
			this.WithdrawBox.Name = "WithdrawBox";
			this.WithdrawBox.Size = new System.Drawing.Size(320, 24);
			this.WithdrawBox.TabIndex = 9;
			this.WithdrawBox.Text = "Укажите сумму для вывода";
			this.WithdrawBox.TextChanged += new System.EventHandler(this.MaskInput);
			this.WithdrawBox.Enter += new System.EventHandler(this.BoxEnter);
			this.WithdrawBox.Leave += new System.EventHandler(this.BoxExit);
			// 
			// LoggedForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 290);
			this.Controls.Add(this.WithdrawBox);
			this.Controls.Add(this.TransferBox);
			this.Controls.Add(this.AddressBox);
			this.Controls.Add(this.DepositBox);
			this.Controls.Add(this.WithdrawButton);
			this.Controls.Add(this.TransferButton);
			this.Controls.Add(this.DepositButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BalanceBox);
			this.Name = "LoggedForm";
			this.Text = "LoggedForm";
			this.Click += new System.EventHandler(this.LoggedForm_Click);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox BalanceBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button DepositButton;
		private System.Windows.Forms.Button TransferButton;
		private System.Windows.Forms.Button WithdrawButton;
		private System.Windows.Forms.TextBox DepositBox;
		private System.Windows.Forms.TextBox AddressBox;
		private System.Windows.Forms.TextBox TransferBox;
		private System.Windows.Forms.TextBox WithdrawBox;
	}
}