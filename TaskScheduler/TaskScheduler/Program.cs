// See https://aka.ms/new-console-template for more information
using TaskScheduler;

var oneTimeTask = new OneTimeTask(new ExecutionContextImplementation(1) , 213, 1);
var taskDriver = new TaskDriver();
taskDriver.Start();
taskDriver.AddTask(oneTimeTask);
var recurringTask = new RecurringTask(new ExecutionContextImplementation(2), 456, 121, 2);
taskDriver.AddTask(recurringTask);

