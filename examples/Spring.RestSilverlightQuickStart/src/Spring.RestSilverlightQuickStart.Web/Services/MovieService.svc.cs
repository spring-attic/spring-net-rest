using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace Spring.RestSilverlightQuickStart.Services
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
    }

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MovieService
    {
        private IDictionary<int, Movie> movies;
        private int index;

        public MovieService()
        {
            movies = new Dictionary<int, Movie>();
            movies.Add(0, new Movie() { ID = 0, Title = "La vita e bella", Director = "Roberto Benigni" });
            movies.Add(1, new Movie() { ID = 1, Title = "Pulp Fiction", Director = "Quentin Tarantino" });
            index = 1;
        }

        [OperationContract]
        [WebGet(UriTemplate = "movies", ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<Movie> GetMovies()
        {
            return movies.Values;
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "movie", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        public void Create(Movie movie)
        {
            WebOperationContext context = WebOperationContext.Current;

            UriTemplateMatch match = context.IncomingRequest.UriTemplateMatch;
            UriTemplate template = new UriTemplate("/movie/{id}");

            index++; // generate new ID
            movie.ID = index;
            movies.Add(index, movie);

            Uri uri = template.BindByPosition(match.BaseUri, movie.ID.ToString());
         
            context.OutgoingResponse.SetStatusAsCreated(uri);
            context.OutgoingResponse.StatusDescription = String.Format("Movie id '{0}' created", movie.ID);
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "movie/{id}", Method = "DELETE")]
        public void Delete(string id)
        {
            WebOperationContext context = WebOperationContext.Current;

            int movieId = Convert.ToInt32(id);
            if (movies.ContainsKey(movieId))
            {
                movies.Remove(movieId);
            }
            else
            {
                context.OutgoingResponse.SetStatusAsNotFound(String.Format("Movie with id '{0}' not found", movieId));
            }
        }
    }
}
