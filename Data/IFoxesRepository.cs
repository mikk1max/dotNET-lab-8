namespace dotNET_lab_8.Data;
using dotNET_lab_8.Models;

public interface IFoxesRepository
{
    void Add(Fox f);
    Fox Get(int id);
    IEnumerable<Fox> GetAll();
    void Update(int id, Fox f);
}