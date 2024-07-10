using Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using Quartz;

namespace Application.Jobs;

public class ResetUploadDayCount(IUnitOfWork unitOfWork) : IJob
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task Execute(IJobExecutionContext context)
    {
        await _unitOfWork.Organization.ResetUploadDayCount();
    }
}