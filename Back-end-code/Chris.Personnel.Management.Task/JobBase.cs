using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Quartz;

namespace Chris.Personnel.Management.Work
{
    public class JobBase
    {
        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="func"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<System.Threading.Tasks.Task> func)
        {
            var jobHistory = $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】";
            try
            {
                var s = context.Trigger.Key.Name;
                //记录Job时间
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await func();//执行任务
                stopwatch.Stop();
                jobHistory += $"，【执行成功】，完成时间：{stopwatch.Elapsed.TotalMilliseconds:00}毫秒";
            }
            catch (Exception ex)
            {
                var jobExecutionException = new JobExecutionException(ex)
                {
                    RefireImmediately = true //true  是立即重新执行任务 
                };

                jobHistory += $"，【执行失败】，异常日志：{ex.Message}";
            }

            Console.Out.WriteLine(jobHistory);
            return jobHistory;
        }
    }
}