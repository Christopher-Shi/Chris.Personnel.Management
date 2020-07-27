using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Chris.Personnel.Management.Work.Jobs
{
    //public class EmailReminderJob : IJob
    //{
    //    private readonly AppDbContext _dbContext;
    //    private readonly IEmailSender _emailSender;
    //    public EmailReminderJob(AppDbContext dbContext, IEmailSender emailSender)
    //    {
    //        _dbContext = dbContext;
    //        _emailSender = emailSender;
    //    }
    //    public Task Execute(IJobExecutionContext context)
    //    {
    //        // fetch customers, send email, update DB
    //        return Task.CompletedTask;
    //    }
    //}
}
