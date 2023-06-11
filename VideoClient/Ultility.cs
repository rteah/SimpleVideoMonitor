using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading;
using System.Diagnostics;

namespace VideoClient
{
    internal class Ultility
    { 
        // predefined openfiledialog, enable multiply use of the program.
        public static OpenFileDialog ofd;  
        static Ultility()
        {
            ofd = new OpenFileDialog()
            {

                FileName = "Select a video file",
                Filter = "Video files (*.mp4)|*.mp4",
                Title = "Open Video file"
            };
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            // this create a new form, and enable user input URL
            Form form = new Form()
            {
                Text = title,
                Font = new System.Drawing.Font("Calibri", 10F),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = new Size(400, 200),
                MinimizeBox = false,
                MaximizeBox = false
            };
            Label label = new Label()
            {
                Font = new System.Drawing.Font("Calibri", 10F),
                Text = promptText,
            };
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                Font = new System.Drawing.Font("Calibri", 10F),
            };
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button() 
            {
                Text = "OK",
                Font = new System.Drawing.Font("Calibri", 10F) 
            };
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button()
            {
                Text = "Cancel",
                Font = new System.Drawing.Font("Calibri", 10F),
            };

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(50, 20, 300, 60);
            textBox.SetBounds(50, 80, 300, 60);
            buttonOk.SetBounds(60, 120, 60, 25);
            buttonCancel.SetBounds(240, 120, 80, 25);

            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;
        }

        public static bool IsRTSP(string url)
        {
            Regex regex = new Regex("^rtsp://", RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }

        public static void Debug(string message)
        {
            MessageBox.Show(message);
        }
        public static void LogWrite(string filePath, string message)
        {
            object lck = new object();

            lock (lck)
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine($"{DateTime.Now} - {message}");
                }
            }
        }

        /* Thread Operation */
        public static bool IsStarted(Thread thread)
        {
            return thread.ThreadState != System.Threading.ThreadState.Unstarted;
        }
        public static bool IsRunning(Thread thread)
        {
            return thread.ThreadState == System.Threading.ThreadState.Running;
        }

        public static bool IsSuspended(Thread thread)
        {
            return thread.ThreadState == System.Threading.ThreadState.Stopped;
        }

        internal static bool IsInterrupted(Thread thread)
        {
            return thread.ThreadState == System.Threading.ThreadState.Stopped;
        }
    }
}
