using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Core.Models;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.BlazorWa.Service
{
    public sealed class BlazorLoggedUserService : ILoggedUserService
    {
        public async Task<BaseResult<T>> GetLoggedUserAsync<T>()
            where T : class, IUser
        {
            return await Task.FromResult(new BaseResult<T>());
        }
    }
}
