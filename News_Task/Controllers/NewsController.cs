using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Application.DTOs;
using News.Domain.Entities;
using News.Domain.Repositories;
using System.Text.Json;

namespace News_Task.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            
            var newsList = _unitOfWork.News.GetAll(query => query
                .Include(n => n.Image)
                .Include(n => n.Translations));

            if (newsList == null || !newsList.Any())
                return NotFound();

           
            var response = newsList.Select(news => new
            {
                news.NewId,
                news.Title,
                news.Content,
                news.CreatedDate,
                news.IsFeatured,
                Translations = news.Translations.Select(t => new
                {
                    t.Language1,
                    t.Language1Title,
                    t.Language1Content,
                    t.Language2,
                    t.Language2Title,
                    t.Language2Content
                }),
                
                Images = news.Image.Select(image => new
                {
                    ImagePath = image.ImagePath 
                }).ToList() 
            }).ToList(); 

            return Ok(response);
        }



        [HttpGet("{id}")]
        public IActionResult GetNewsById(int id)
        {
            var news = _unitOfWork.News.Get(id, query => query
                .Include(n => n.Image)
                .Include(n => n.Translations));

            if (news == null)
                return NotFound();

            
            var imagePaths = news.Image.Select(image =>
            {
                var filePath = image.ImagePath;
               
                return new
                {
                    ImagePath = filePath
                };
            }).ToList();

            var response = new
            {
                news.NewId,
                news.Title,
                news.Content,
                news.CreatedDate,
                news.IsFeatured,
                Translations = news.Translations.Select(t => new
                {
                    t.Language1,
                    t.Language1Title,
                    t.Language1Content,
                    t.Language2,
                    t.Language2Title,
                    t.Language2Content
                }),
                Images = imagePaths 
            };

            return Ok(response);
        }



        [HttpPost]
        public IActionResult AddNews([FromForm] NewsDTO newsDTO)
        {
            if (newsDTO == null)
                return BadRequest("News data cannot be null.");

            var news = new New
            {
                Title = newsDTO.Title,
                Content = newsDTO.Content,
                CreatedDate = newsDTO.CreatedDate,
                IsFeatured = newsDTO.IsFeatured,
                Image = new List<Images>(),
                Translations = new List<NewTranslation>()
            };

            var rootPath = Directory.GetCurrentDirectory();
            var uploadPath = Path.Combine(rootPath, "Uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var imageFile in newsDTO.Image ?? Enumerable.Empty<IFormFile>())
            {
                if (imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    news.Image.Add(new Images { ImagePath = filePath });
                }
            }

            news.Translations.Add(new NewTranslation
            {
                Language1=newsDTO.Language1,
                Language1Title = newsDTO.Translation1Title,
                Language1Content = newsDTO.Translation1Content,
                Language2= newsDTO.Language2,
                Language2Title = newsDTO.Translation2Title,
                Language2Content = newsDTO.Translation2Content
            });

            try
            {
                _unitOfWork.News.Add(news);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving news to the database: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetNewsById), new { id = news.NewId }, news);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateNews(int id, [FromForm] NewsDTO newsDTO)
        {
            if (newsDTO == null)
                return BadRequest("News data cannot be null.");

            
            var existingNews = _unitOfWork.News.Get(id, query => query
                .Include(n => n.Image)
                .Include(n => n.Translations));

            if (existingNews == null)
                return NotFound("News not found.");

           
            existingNews.Title = newsDTO.Title;
            existingNews.Content = newsDTO.Content;
            existingNews.CreatedDate = newsDTO.CreatedDate;
            existingNews.IsFeatured = newsDTO.IsFeatured;

           
            var rootPath = Directory.GetCurrentDirectory();
            var uploadPath = Path.Combine(rootPath, "Uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

           
            if (newsDTO.Image != null && newsDTO.Image.Any())
            {

                
                existingNews.Image.Clear();

               
                foreach (var imageFile in newsDTO.Image)
                {
                    if (imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        existingNews.Image.Add(new Images { ImagePath = filePath });
                    }
                }
            }

           
            var existingTranslation = existingNews.Translations.FirstOrDefault();
            if (existingTranslation != null)
            {
                existingTranslation.Language1 = existingTranslation.Language1;
                existingTranslation.Language1Title = newsDTO.Translation1Title;
                existingTranslation.Language1Content = newsDTO.Translation1Content;
                existingTranslation.Language2 = existingTranslation.Language2;
                existingTranslation.Language2Title = newsDTO.Translation2Title;
                existingTranslation.Language2Content = newsDTO.Translation2Content;
            }
            else
            {
               
                existingNews.Translations.Add(new NewTranslation
                {
                    Language1 = newsDTO.Language1,
                    Language1Title = newsDTO.Translation1Title,
                    Language1Content = newsDTO.Translation1Content,
                    Language2 = newsDTO.Language2,
                    Language2Title = newsDTO.Translation2Title,
                    Language2Content = newsDTO.Translation2Content
                });
            }

            
            try
            {
                _unitOfWork.News.Update(existingNews);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating news in the database: {ex.Message}");
            }

            return Content("Updated");
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           
            var newsItem = _unitOfWork.News.Get(id);

            
            if (newsItem == null)
            {
                return NotFound(); 
            }

            try
            {
                _unitOfWork.News.Remove(newsItem);
                _unitOfWork.Complete(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting news item: {ex.Message}");
            }

            return Content("Deleted");
        }



    }
}
