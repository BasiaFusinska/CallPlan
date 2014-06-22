using System;
using System.Threading.Tasks;

namespace CallPlan
{
    public class ServiceHandler : IServiceHandler
    {
        private readonly ICategoryService _categoryService;

        public ServiceHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ServiceResponse> HandleService(string originator)
        {
            ServiceResponse response;
            try
            {
                var serviceResponse = await _categoryService.GetResponse(originator);
                response = (ServiceResponse) int.Parse(serviceResponse);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (TimeoutException)
            {
                response = ServiceResponse.Timeout;
            }
            catch (Exception)
            {
                response = ServiceResponse.Exception;
            }

            return response;
        }
    }
}
