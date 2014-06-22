using System.Threading.Tasks;

namespace CallPlan
{
    public interface IServiceHandler
    {
        Task<ServiceResponse> HandleService(string originator);
    }
}
