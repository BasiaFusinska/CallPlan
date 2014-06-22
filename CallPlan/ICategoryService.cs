using System.Threading.Tasks;

namespace CallPlan
{
    public interface ICategoryService
    {
        Task<string> GetResponse(string originator);
    }
}
