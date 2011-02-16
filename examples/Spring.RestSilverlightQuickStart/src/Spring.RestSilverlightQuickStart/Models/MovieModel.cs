using System.ComponentModel;

namespace Spring.RestSilverlightQuickStart.Models
{
    public class MovieModel : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _director;
        public string Director
        {
            get { return _director; }
            set
            {
                _director = value;
                RaisePropertyChanged("Director");
            }
        }


        #region INotifyPropertyChanged

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
