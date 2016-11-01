using System.Linq;
using MoviesLibrary;

namespace DotNetCoreDemo.Services
{
    public interface IDataRepository
    {

        IQueryable<MovieData> GetAllMovies();
        MovieData GetMovieById(int id);
        bool CreateMovie(MovieData newMovie);
        bool UpdateMovie(MovieData movie);
        bool DoesTitleAlreadyExist(string title);
    }
}