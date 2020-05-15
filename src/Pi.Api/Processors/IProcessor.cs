using Pi.Api.Model;
using System.Threading.Tasks;

namespace Pi.Api.Processors
{
    public interface IProcessor
    {
        Task<Job> SubmitJob(Job job);
    }
}
