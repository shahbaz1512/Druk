using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MIIPL.Common
{
    // Custom Exception class to catch all "unhandled exceptions"
    public class CustomExceptionHandler
    {
        // Event handler that will be called when an unhandled
        // exception is caught
        public void OnThreadException(object sender,
                                      ThreadExceptionEventArgs t)
        {
            // Log the exception to a file
            LogException(t.Exception);

            // Tell the user that the app will restart
            //     MessageBox.Show(@"A Fatal Error was detected and logged.Click the OK button to restart the application","Fatal Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            // Shut down the current app instance
            Application.Exit();

            // Restart the app
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        // *Very* simple logging function to write exception details
        // to disk
        protected void LogException(Exception e)
        {
            DateTime now = System.DateTime.Now;
            string error = e.Message + "\n\nStack Trace:\n"
                                        + e.StackTrace;
            string filename = String.Format(@"Log-{0}{1}{2}-{3}{4}{5}.txt",
                                            now.Year.ToString(),
                                            now.Month.ToString(),
                                            now.Day.ToString(),
                                            now.Hour, now.Minute,
                                            now.Second);

            StreamWriter stream = null;
            try
            {
                stream = new StreamWriter(filename, false);
                stream.Write(error);
            }
            catch //(Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }

        public CustomExceptionHandler()
        {
        }
     };
            //    CustomExceptionHandler eh = new CustomExceptionHandler();
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(eh.OnThreadException);

}
