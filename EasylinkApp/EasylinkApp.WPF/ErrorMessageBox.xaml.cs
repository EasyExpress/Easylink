 
using System;
using System.Windows;
 

namespace EasylinkApp.WPF
{

  
    public partial class ErrorMessageBox  
    {

        private double  originalHeight;
    
        private bool showDetail = false; 

      
        public ErrorMessageBox(Exception ex)
        {

                InitializeComponent();

           
                lblUserInfo.Text = ex.Message;

                if (ex.InnerException != null)
                {
                    lblDebugInfo.Text = "Inner Exception: " + Environment.NewLine + ex.InnerException.Message;

                    lblDebugInfo.Text += Environment.NewLine + "StackTrace: " + Environment.NewLine + ex.StackTrace;

                }
                else
                {
                    lblDebugInfo.Text = "StackTrace: " + Environment.NewLine + ex.StackTrace;

                }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
  
            originalHeight =  Height;

            panelBottom.Visibility = Visibility.Hidden;

            Height = panelTop.Height +35;  
        }

       

        private void btnShowDetail_Click(object sender, RoutedEventArgs e)
        {
            if (showDetail == false)
            {
                panelBottom.Visibility = Visibility.Visible;

                Height = originalHeight;

                showDetail = true;

                btnShowDetail.Content = "Hide Detail"; 
            }
            else
            {
                showDetail = false;

                panelBottom.Visibility = Visibility.Hidden;

                Height = panelTop.Height + 35;  

                btnShowDetail.Content = "Show Detail"; 

            }


        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            Close();

        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
     
            string copyText = "User info:\n" + lblUserInfo.Text + "\n\n" + lblDebugInfo.Text;


            Clipboard.SetText(copyText.Replace("\n", Environment.NewLine)); 

       
        }

      
        public static void  ShowException(Exception ex)
        {

            var  messageBox = new ErrorMessageBox(ex);

            messageBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            messageBox.ShowDialog();

        }







    }
}
