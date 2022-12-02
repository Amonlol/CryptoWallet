
namespace ClientUI
{
	partial class MainMenu
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.codeWordBox = new System.Windows.Forms.TextBox();
			this.Login_Button = new System.Windows.Forms.Button();
			this.Register_Button = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.PhoneNumberBox = new System.Windows.Forms.MaskedTextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(96, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(294, 52);
			this.label1.TabIndex = 1;
			this.label1.Text = "Добро пожаловать в систему криптовалюты dotNetCoin!\r\n\r\nДля продолжения войдите в " +
    "систему!\r\n\r\n";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// codeWordBox
			// 
			this.codeWordBox.Location = new System.Drawing.Point(8, 80);
			this.codeWordBox.Multiline = true;
			this.codeWordBox.Name = "codeWordBox";
			this.codeWordBox.Size = new System.Drawing.Size(480, 48);
			this.codeWordBox.TabIndex = 2;
			this.codeWordBox.Text = "Ваша кодовая фраза из 12 слов";
			// 
			// Login_Button
			// 
			this.Login_Button.Location = new System.Drawing.Point(8, 256);
			this.Login_Button.Name = "Login_Button";
			this.Login_Button.Size = new System.Drawing.Size(160, 56);
			this.Login_Button.TabIndex = 3;
			this.Login_Button.Text = "Войти";
			this.Login_Button.UseVisualStyleBackColor = true;
			this.Login_Button.Click += new System.EventHandler(this.LoginButtonClick);
			// 
			// Register_Button
			// 
			this.Register_Button.Location = new System.Drawing.Point(328, 256);
			this.Register_Button.Name = "Register_Button";
			this.Register_Button.Size = new System.Drawing.Size(160, 56);
			this.Register_Button.TabIndex = 4;
			this.Register_Button.Text = "Создать новый кошелек";
			this.Register_Button.UseVisualStyleBackColor = true;
			this.Register_Button.Click += new System.EventHandler(this.RegisterButtonClick);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(8, 168);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(480, 24);
			this.textBox2.TabIndex = 2;
			this.textBox2.Text = "Ваша электронная почта";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(8, 200);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(480, 24);
			this.textBox3.TabIndex = 2;
			this.textBox3.Text = "Ваш пароль";
			// 
			// PhoneNumberBox
			// 
			this.PhoneNumberBox.Location = new System.Drawing.Point(8, 136);
			this.PhoneNumberBox.Mask = "+7 (999) 000-00-00";
			this.PhoneNumberBox.Name = "PhoneNumberBox";
			this.PhoneNumberBox.Size = new System.Drawing.Size(488, 20);
			this.PhoneNumberBox.TabIndex = 5;
			// 
			// MainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 327);
			this.Controls.Add(this.PhoneNumberBox);
			this.Controls.Add(this.Register_Button);
			this.Controls.Add(this.Login_Button);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.codeWordBox);
			this.Controls.Add(this.label1);
			this.Name = "MainMenu";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox codeWordBox;
		private System.Windows.Forms.Button Login_Button;
		private System.Windows.Forms.Button Register_Button;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.MaskedTextBox PhoneNumberBox;
	}
}

