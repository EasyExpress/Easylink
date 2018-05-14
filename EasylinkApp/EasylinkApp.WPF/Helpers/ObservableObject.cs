using System;
 

using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace EasylinkApp.WPF
{


    public abstract class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
 
       

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExtractPropertyName(propertyExpression);

            OnPropertyChanged(propertyName);
        }



        private  static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
           
           
            return memberExpression.Member.Name;
        }

        private void OnPropertyChanged(string propertyName)
        {


            this.VerifyPropertyName(propertyName);

            if (PropertyChanged != null)
            {



                var e = new PropertyChangedEventArgs(propertyName);

                PropertyChanged(this, e);

            }

        }
 
        public virtual void VerifyPropertyName(string propertyName)
        {


            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {

                string msg = "Invalid property name: " + propertyName;


                if (this.ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }

                else
                {
                    Debug.Fail(msg);
                }
            }
 
 
        }

 
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
 

    } 

}
