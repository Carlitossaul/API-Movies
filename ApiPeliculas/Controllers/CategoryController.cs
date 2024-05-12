using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepositories _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepositories categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategories()
        {
            var listCategories = _categoryRepo.GetCategories();
            var listCategoriesDto = new List<CategoryDto>();

            foreach (var list in listCategories)
            {
                listCategoriesDto.Add(_mapper.Map<CategoryDto>(list));
            }

            return Ok(listCategoriesDto);
        }

        [HttpGet("{CategoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCategory(int CategoryId)
        {
            var itemCategory = _categoryRepo.GetCategory(CategoryId);

            if (itemCategory == null)
            {
                return NotFound();
            }
            var itemCategoryDto = _mapper.Map<CategoryDto>(itemCategory);
            return Ok(itemCategoryDto);
        }

        [HttpPost]

        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateCategory([FromBody]CreateCategoryDto createCategoryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_categoryRepo.ExistsCategory(createCategoryDto.Name))
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(404,ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            if(!_categoryRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something was wrong {category.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId = category.Id },category);
        }


        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]

        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteCategory(int categoryId)
        {
            
            if (!_categoryRepo.ExistsCategory(categoryId))
            {
                return NotFound();
            }

            var category = _categoryRepo.GetCategory(categoryId); 
            if (!_categoryRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Something was wrong deleting {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpPatch("{categoryId:int}", Name= "UpdateCategory")]

        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto CategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (CategoryDto == null || categoryId != CategoryDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepo.ExistsCategory(CategoryDto.Name))
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(CategoryDto);
            if (!_categoryRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Something was wrong updating {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


    }
}
