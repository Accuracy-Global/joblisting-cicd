using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IDataService
    {
        Task<List<Output>> ReadAsync<Output>(string queryString);
        List<Output> Read<Output>(string queryString);
        List<Output> Read<Output>(string procedure, List<Parameter> parameters);
        List<Output> Read<Output, Input>(string procedure, Input model);
        Output Single<Output>(string queryString);
        Output Single<Output>(string procedure, List<Parameter> parameters);
        Output Single<Output, Input>(string procedure, Input model);

        Output Scaler<Output>(string queryString);
        Task<Output> ScalerAsync<Output>(string queryString);
        Output Scaler<Output>(string procedure, List<Parameter> parameters);
        Task<Output> ScalerAsync<Output>(string procedure, List<Parameter> parameters);
        Output Scaler<Output, Input>(string procedure, Input model);
        Task<Output> ScalerAsync<Output, Input>(string procedure, Input model);
        Task<int> HandleDataAsync(string storedProcedure, List<Parameter> parameters);
    }
}
