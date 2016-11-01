using System;
using AutoMapper;
using DotNetCoreDemo.Services;
using DotNetCoreDemo.ViewModal;
using Microsoft.AspNet.Mvc;
using MoviesLibrary;

namespace DotNetCoreDemo.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IDataRepository _repo;
       

        public MoviesController(IDataRepository dataRepository)
        {
            _repo = dataRepository;
            
        }

        //[EnableQuery()]// this is for suport url query working with oData//PageSize = 5
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var movies = _repo.GetAllMovies();
                return Ok(movies);
            }
            catch (Exception ex)
            {

                return new ObjectResult(this.Response)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(Exception)
                };
            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
       {
            try
            {
                if (id > 0)
                {
                    var movie = _repo.GetMovieById(id);
                    var result = Mapper.Map<MovieDataViewModal>(movie);
                    if (result != null) return Ok(result);

                    return HttpNotFound();
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                return new ObjectResult(Response)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(Exception)
                };
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]MovieDataViewModal newMovie)
        {
            try
            {

                if (newMovie == null)
                    return HttpBadRequest("Movie can not be null");

                if (!ModelState.IsValid)
                {
                    return HttpBadRequest(ModelState);
                }

                var movie = Mapper.Map<MovieData>(newMovie);

                if (_repo.CreateMovie(movie)) return Ok(newMovie);

                return HttpBadRequest("Movie already existing");

            }
            catch (Exception ex)
            {
                return  new ObjectResult(Response)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(Exception)
                };
            }
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]MovieDataViewModal movieModified)
        {
            try
            {

                if (movieModified == null)
                    return HttpBadRequest("Movie can not be null");

                if (movieModified.MovieId <= 0 && movieModified.MovieId != id)
                    return HttpBadRequest();

                if (!ModelState.IsValid)
                {
                    return HttpBadRequest(ModelState);
                }

                var movieData = _repo.GetMovieById(id);
                if (movieData != null)
                {
                    var movie = Mapper.Map<MovieData>(movieModified);
                    _repo.UpdateMovie(movie);
                    return Ok();
                }

                return HttpNotFound();

            }
            catch (Exception ex)
            {
                return  new ObjectResult(Response)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(Exception)
                };
            }

        }

    }
}
