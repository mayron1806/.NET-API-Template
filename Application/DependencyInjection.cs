using Application.Jobs;
using Application.Services.PlanService;
using Application.UseCases.ActiveAccount;
using Application.UseCases.ConfirmFilesUpload;
using Application.UseCases.CreateAccount;
using Application.UseCases.CreateOrganization;
using Application.UseCases.ForgetPassword;
using Application.UseCases.Login;
using Application.UseCases.PrepareFilesUpload;
using Application.UseCases.ResetPassword;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services
        services.AddScoped<IPlanService, PlanService>();
        // validations
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<CreateAccountInputDto>, CreateAccountValidator>();
        services.AddScoped<IValidator<LoginInputDto>, LoginValidator>();
        services.AddScoped<IValidator<ResetPasswordInputDto>, ResetPasswordValidator>();
        services.AddScoped<IValidator<ForgetPasswordInputDto>, ForgetPasswordValidator>();
        services.AddScoped<IValidator<ActiveAccountInputDto>, ActiveAccountValidator>();
        services.AddScoped<IValidator<PrepareFilesUploadInputDto>, PrepareFilesUploadValidator>();
        services.AddScoped<IValidator<ConfirmFilesUploadInputDto>, ConfirmFilesUploadValidator>();
        
        // use cases
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IActiveAccountUseCase, ActiveAccountUseCase>();
        services.AddScoped<IForgetPasswordUseCase, ForgetPasswordUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
        services.AddScoped<ICreateOrganizationUseCase, CreateOrganizationUseCase>();
        services.AddScoped<IPrepareFilesUpload, PrepareFilesUploadUseCase>();
        services.AddScoped<IConfirmFilesUpload, ConfirmFilesUploadUseCase>();

        // jobs
        services.AddQuartz(q =>
        {
            // Just use the name of your job that you created in the Jobs folder.
            var jobKey = new JobKey("ResetUploadDayCount");
            q.AddJob<ResetUploadDayCount>(opts => opts.WithIdentity(jobKey));
            
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("ResetUploadDayCount-trigger")
                .WithCronSchedule("0 0 0 * * ? *")
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
