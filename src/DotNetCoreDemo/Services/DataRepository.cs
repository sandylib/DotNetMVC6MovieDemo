using System;
using System.Linq;
using DotNetCoreDemo.Helper;
using MoviesLibrary;

namespace DotNetCoreDemo.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly MovieDataSource _dataSrc;
        private readonly CacheHelper _cacheHelper;
        private const string ALL_MOVIES = "allMovies";
        
        public DataRepository(MovieDataSource dataSrc, CacheHelper cacheHelper)
        {
            _dataSrc = dataSrc;
            _cacheHelper = cacheHelper;
        }

        public bool CreateMovie(MovieData newMovie)
        {
            try
            {
                if (_dataSrc.Create(newMovie) > 0)
                {
                    _cacheHelper.RemoveFromCache(ALL_MOVIES);
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DoesTitleAlreadyExist(string title)
        {
            return GetAllMovies().Any(r => r.Title == title);
        }

        public IQueryable<MovieData> GetAllMovies()
        {
            var allMovies = _cacheHelper.GetFromCache<IQueryable<MovieData>>(ALL_MOVIES);

            if (allMovies == null)
            {

                allMovies = _dataSrc.GetAllData().AsQueryable();
                _cacheHelper.SaveTocache(ALL_MOVIES, allMovies, DateTime.Now.AddHours(24));
            }

            return allMovies;
        }

        public MovieData GetMovieById(int id)
        {
            try
            {
                var allMovies = GetAllMovies();
                var movie = allMovies.FirstOrDefault(r => r.MovieId == id);
                if (movie == null)
                {
                    movie = _dataSrc.GetDataById(id);
                    if (movie != null)
                        return movie;
                    return null;
                }
                return movie;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateMovie(MovieData movie)
        {
            try
            {
                _dataSrc.Update(movie);
                _cacheHelper.RemoveFromCache(ALL_MOVIES);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
