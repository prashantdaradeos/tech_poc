using EcoTech.Domain.RepositoryContracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTech.Application.BackGroundServices;

public class DeleteOTPService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(2);
    
    public DeleteOTPService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            using var scope = _serviceProvider.CreateScope();
            var otpRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            await otpRepository.ManageOtp(string.Empty,string.Empty,SpConstants.Delete);
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
