using ApiPeliculas.Models;

namespace ApiPeliculas.Repositories.IRepositories
{
    public interface ICategoryRepositories
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int CategoryId);
        bool ExistsCategory(string name);
        bool ExistsCategory(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
