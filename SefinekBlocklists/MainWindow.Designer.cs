namespace SefinekBlocklists
{
	sealed partial class MainWindow
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
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
				webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
				((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
				SuspendLayout();
				// 
				// webView21
				// 
				webView21.AllowExternalDrop = true;
				webView21.BackColor = Color.Black;
				webView21.CreationProperties = null;
				webView21.DefaultBackgroundColor = Color.White;
				resources.ApplyResources(webView21, "webView21");
				webView21.ForeColor = Color.White;
				webView21.Name = "webView21";
				webView21.ZoomFactor = 1D;
				// 
				// MainWindow
				// 
				resources.ApplyResources(this, "$this");
				AutoScaleMode = AutoScaleMode.Dpi;
				BackColor = Color.Black;
				Controls.Add(webView21);
				DoubleBuffered = true;
				Name = "MainWindow";
				Load += MainWindow_Load;
				((System.ComponentModel.ISupportInitialize)webView21).EndInit();
				ResumeLayout(false);
		  }

		  #endregion

		  private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}
