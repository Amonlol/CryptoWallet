
namespace ClientUI
{
	partial class RegisterForm
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
			this.codeWordBox = new System.Windows.Forms.TextBox();
			this.GenerateButton = new System.Windows.Forms.Button();
			this.ProceedButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.PhoneNumberBox = new System.Windows.Forms.MaskedTextBox();
			this.PasswordBox1 = new System.Windows.Forms.MaskedTextBox();
			this.PasswordBox2 = new System.Windows.Forms.MaskedTextBox();
			this.EmailBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// codeWordBox
			// 
			this.codeWordBox.Location = new System.Drawing.Point(48, 32);
			this.codeWordBox.Multiline = true;
			this.codeWordBox.Name = "codeWordBox";
			this.codeWordBox.ReadOnly = true;
			this.codeWordBox.Size = new System.Drawing.Size(488, 24);
			this.codeWordBox.TabIndex = 0;
			this.codeWordBox.Text = "Ваш уникальный ключ";
			// 
			// GenerateButton
			// 
			this.GenerateButton.Location = new System.Drawing.Point(48, 368);
			this.GenerateButton.Name = "GenerateButton";
			this.GenerateButton.Size = new System.Drawing.Size(144, 56);
			this.GenerateButton.TabIndex = 5;
			this.GenerateButton.Text = "Генерировать";
			this.GenerateButton.UseVisualStyleBackColor = true;
			this.GenerateButton.Click += new System.EventHandler(this.GenerateButtonClick);
			// 
			// ProceedButton
			// 
			this.ProceedButton.Location = new System.Drawing.Point(392, 368);
			this.ProceedButton.Name = "ProceedButton";
			this.ProceedButton.Size = new System.Drawing.Size(144, 56);
			this.ProceedButton.TabIndex = 6;
			this.ProceedButton.Text = "Далее";
			this.ProceedButton.UseVisualStyleBackColor = true;
			this.ProceedButton.Click += new System.EventHandler(this.ProceedButtonClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(163, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Введите Ваш номер телефона:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(48, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(215, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Введите Ваш электронный адрес (почту):";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(48, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Введите пароль:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(48, 272);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(103, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Повторите пароль:";
			// 
			// PhoneNumberBox
			// 
			this.PhoneNumberBox.Location = new System.Drawing.Point(48, 96);
			this.PhoneNumberBox.Mask = "+7 (999) 000-00-00";
			this.PhoneNumberBox.Name = "PhoneNumberBox";
			this.PhoneNumberBox.Size = new System.Drawing.Size(488, 20);
			this.PhoneNumberBox.TabIndex = 1;
			// 
			// PasswordBox1
			// 
			this.PasswordBox1.Location = new System.Drawing.Point(48, 224);
			this.PasswordBox1.Name = "PasswordBox1";
			this.PasswordBox1.PasswordChar = '*';
			this.PasswordBox1.Size = new System.Drawing.Size(488, 20);
			this.PasswordBox1.TabIndex = 3;
			this.PasswordBox1.UseSystemPasswordChar = true;
			// 
			// PasswordBox2
			// 
			this.PasswordBox2.Location = new System.Drawing.Point(48, 288);
			this.PasswordBox2.Name = "PasswordBox2";
			this.PasswordBox2.PasswordChar = '*';
			this.PasswordBox2.Size = new System.Drawing.Size(488, 20);
			this.PasswordBox2.TabIndex = 4;
			this.PasswordBox2.UseSystemPasswordChar = true;
			// 
			// EmailBox
			// 
			this.EmailBox.Location = new System.Drawing.Point(48, 160);
			this.EmailBox.Multiline = true;
			this.EmailBox.Name = "EmailBox";
			this.EmailBox.Size = new System.Drawing.Size(488, 24);
			this.EmailBox.TabIndex = 2;
			// 
			// RegisterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(573, 452);
			this.Controls.Add(this.EmailBox);
			this.Controls.Add(this.PasswordBox2);
			this.Controls.Add(this.PasswordBox1);
			this.Controls.Add(this.PhoneNumberBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ProceedButton);
			this.Controls.Add(this.GenerateButton);
			this.Controls.Add(this.codeWordBox);
			this.Name = "RegisterForm";
			this.Text = "RegisterForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox codeWordBox;
		private System.Windows.Forms.Button GenerateButton;
		private System.Windows.Forms.Button ProceedButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.MaskedTextBox PhoneNumberBox;
		private System.Windows.Forms.MaskedTextBox PasswordBox1;
		private System.Windows.Forms.MaskedTextBox PasswordBox2;
		private System.Windows.Forms.TextBox EmailBox;
	}
}