using SysLogger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskRunner.Service;

namespace TaskRunner
{
    public class TaskFactory
    {

        
        private async Task<List<int>> GenerateCalcTasks(List<string> workflowMethods, List<int> Input)
        {
            List<int> CalcResults = new List<int>();

            try
            {
                UpdateStatus("Preparing Test", "MARQUEE");

                int total = Input.Count;
                if (total > 0)
                {
                    var startTime = DateTime.Now;
                    
                    int counter = 0;

                    UpdateStatus("Response Received for Input(s): " + counter.ToString() + " of " + total.ToString(), "STEPINIT", total);

                    // Create a list of Tasks to be run asynchronously
                    var TaskList = new List<Task<int>>();

                    // # Enable staggering of tasks

                    // get delays for generating tasks
                    List<int> lstDelays = new List<int>();

                    var calcService = new Calculator();

                    // loop to create all tasks
                    for (int i = 0; i <= total - 1; i++)
                    {
                            // generate the tasks without staggering as staggering is not selected
                            TaskList.Add(calcService.Plus(Input[i], Input[i]));
                    }

                    // # end generating tasks

                    // Run all tasks, check if any task has completed to do post processing
                    while ((TaskList.Count > 0))
                    {
                        Task<int> TaskResult = await Task.WhenAny(TaskList.ToArray());
                        CalcResults.Add(TaskResult.Result);
                        TaskList.Remove(TaskResult);

                        counter = counter + 1;
                        UpdateStatus("Response Received for Input(s): " + counter.ToString() + " of " + total.ToString(), "STEP");

                    }

                    var endTime = DateTime.Now;
                                     
                }
                else
                    // No input
                    UpdateStatus("Invalid input", "ERROR");
            }

            catch (Exception ex)
            {
                UpdateStatus("An error occured", "ERROR");

                SysLog oSysLog = new SysLog
                {
                    FileName = "TaskFactory",
                    FunctionName = "GenerateCalcTasks",
                    Description = "Error in GenerateCalcTasks",
                    Detail = " Error: " + ex.ToString(),
                    Severity = SyslogSeverity.Critical.ToString()
                };

                SysLoggerFactory.SaveSyslogEvent(oSysLog);
            }

            return CalcResults;
        }

        private void UpdateStatus(string text, string type = "", int max = 0)
        {
            //lblStatus.Text = text;

            switch (type.ToUpper())
            {
                case "ERROR":
                    {
                        /*progressBarStatus.Style = ProgressBarStyle.Blocks;
                        progressBarStatus.Value = 0;
                        picLoading.Visible = false;
                        btnClose.Enabled = true;*/
                        break;
                    }

                case "MARQUEE":
                    {
                        /*progressBarStatus.Style = ProgressBarStyle.Marquee;
                        picLoading.Visible = false;*/
                        break;
                    }

                case "STEPINIT":
                    {
                        /*progressBarStatus.Style = ProgressBarStyle.Continuous;
                        progressBarStatus.Value = 0;
                        progressBarStatus.Maximum = max;
                        picLoading.Visible = true;*/
                        break;
                    }

                case "STEP":
                    {
                        /*progressBarStatus.PerformStep();
                        picLoading.Visible = true;*/
                        break;
                    }

                case "COMPLETED":
                    {
                        /*progressBarStatus.Style = ProgressBarStyle.Blocks;
                        progressBarStatus.Value = progressBarStatus.Maximum;
                        picLoading.Visible = false;*/
                        break;
                    }

                default:
                    {
                        /*progressBarStatus.Style = ProgressBarStyle.Blocks;
                        progressBarStatus.Value = 0;
                        picLoading.Visible = false;*/
                        break;
                    }
            }
        }



    }
}
