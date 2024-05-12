using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositories.IRepositories;

namespace ApiPeliculas.Repositories
{
    public class CategoryRepositories : ICategoryRepositories
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepositories(ApplicationDbContext db)
        {
                _db = db;
        }
        public bool CreateCategory(Category category)
        {
            category.CreateData = DateTime.Now;
            _db.Category.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            var valor = _db.Category.Remove(category);
            return Save();
        }

        public bool ExistsCategory(string name)
        {
            var valor = _db.Category.Any(Category => Category.Name.ToLower().Trim() == name.ToLower().Trim());
            return valor;
        }

        public bool ExistsCategory(int id)
        {
            var valor = _db.Category.Any(Category => Category.Id == id);
            return valor;
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(Category => Category.Name).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _db.Category.FirstOrDefault(Category => Category.Id == CategoryId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreateData = DateTime.Now;
            _db.Category.Update(category);
            return Save();
        }


    }
}
