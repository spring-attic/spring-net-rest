using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Spring.Rest.Client;
using Spring.Http.Converters.Json;

using Spring.RestSilverlightQuickStart.Commands;
using Spring.RestSilverlightQuickStart.Models;

namespace Spring.RestSilverlightQuickStart.ViewModels
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        private RestTemplate template;

        public MovieViewModel()
        {
            CreateMovieCommand = new DelegateCommand(CreateMovie, CanCreateMovie);
            DeleteMovieCommand = new DelegateCommand(DeleteMovie, CanDeleteMovie);

            template = new RestTemplate("http://localhost:12345/Services/MovieService.svc/");
            template.MessageConverters.Add(new JsonHttpMessageConverter());

            RefreshMovies();
        }

        #region Properties

        private ObservableCollection<MovieModel> _movies;
        public ObservableCollection<MovieModel> Movies
        {
            get { return _movies; }
            set
            {
                _movies = value;
                RaisePropertyChanged("Movies");
            }
        }

        private MovieModel _movieToCreate;
        public MovieModel MovieToCreate
        {
            get { return _movieToCreate; }
            set
            {
                _movieToCreate = value;
                RaisePropertyChanged("MovieToCreate");
            }
        }

        #endregion

        #region Commands

        #region CreateMovie command

        public ICommand CreateMovieCommand { get; set; }

        private void CreateMovie(object parameter)
        {
            MovieModel movie = parameter as MovieModel;
            CreateMovie(movie);
        }

        private bool CanCreateMovie(object parameter)
        {
            return true;
        }

        #endregion

        #region DeleteMovie command

        public ICommand DeleteMovieCommand { get; set; }

        private void DeleteMovie(object parameter)
        {
            int movieId = Convert.ToInt32(parameter);
            DeleteMovie(movieId);
        }

        private bool CanDeleteMovie(object parameter)
        {
            return true;
        }

        #endregion

        #endregion


        private void RefreshMovies()
        {
            MovieToCreate = new MovieModel();

            template.GetForObjectAsync<IEnumerable<MovieModel>>("movies",
                r =>
                {
                    if (r.Error == null)
                    {
                        Movies = new ObservableCollection<MovieModel>(r.Response);                        
                    }
                });
        }

        private void CreateMovie(MovieModel movie)
        {
            template.PostForLocationAsync("movie", movie, 
                r =>
                {
                    if (r.Error == null)
                    {
                        RefreshMovies();
                    }
                });
        }

        private void DeleteMovie(int movieId)
        {
            template.DeleteAsync("movie/{id}",
                r =>
                {
                    if (r.Error == null)
                    {
                        RefreshMovies();
                    }
                }, movieId);
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
