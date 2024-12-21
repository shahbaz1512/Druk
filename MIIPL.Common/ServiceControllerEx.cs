using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace MIIPL.Common
{
    public static class ServiceControllerEx
    {
        public static bool IsExists(string MachineName, string ServiceName)
        {
            bool flag = false;
            try
            {
                ServiceController[] AvailableServices = ServiceController.GetServices(MachineName);
                foreach (ServiceController AvailableService in AvailableServices)
                {
                    if (AvailableService.ServiceName == ServiceName)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            catch { 
            }
            return flag;
        }
        public static bool IsExists(string ServiceName)
        {
            return IsExists(".", ServiceName);
        }
        public static bool StartEx(this ServiceController WSController, bool flag)
        {
            //check the status of the service
            if (WSController.Status.ToString() == "Paused")
            {
                WSController.Continue();
            }
            else if (WSController.Status.ToString() == "Stopped")
            {
                //get an array of services this service depends upon, loop through 
                //the array and prompt the user to start all required services.
                ServiceController[] ParentServices = WSController.ServicesDependedOn;

                //if the length of the array is greater than or equal to 1.
                if (ParentServices.Length >= 1)
                {
                    foreach (ServiceController ParentService in ParentServices)
                    {
                        //make sure the parent service is running or at least paused.
                        if (ParentService.Status.ToString() != "Running" || ParentService.Status.ToString() != "Paused")
                        {
                            //if (MessageBox.Show("This service is required. Would you like to also start this service?\n" + ParentService.DisplayName, "Required Service", MessageBoxButtons.YesNo).ToString() == "Yes")
                            if(flag)
                            {
                                //if the user chooses to start the service

                                ParentService.Start();
                                ParentService.WaitForStatus(ServiceControllerStatus.Running);
                            }
                            else
                            {
                                //otherwise just return.
                                //return;
                                return false;
                            }
                        }
                    }
                }
                WSController.Start();
            }

            WSController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
            return true;
        }
        public static bool StopEx(this ServiceController WSController, bool flag)
        {
            //check to see if the service can be stopped.
            if (WSController.CanStop == true)
            {

                //get an array of dependent services, loop through the array and 
                //prompt the user to stop all dependent services.
                ServiceController[] DependentServices = WSController.DependentServices;

                //if the length of the array is greater than or equal to 1.
                if (DependentServices.Length >= 1)
                {
                    foreach (ServiceController DependentService in DependentServices)
                    {
                        //make sure the dependent service is not already stopped.
                        if (DependentService.Status.ToString() != "Stopped")
                        {
                            //if (MessageBox.Show("Would you like to also stop this dependent service?\n" + DependentService.DisplayName, "Dependent Service", MessageBoxButtons.YesNo).ToString() == "Yes")
                            if(flag)
                            {
                                // not checking at this point whether the dependent service can be stopped.
                                // developer may want to include this check to avoid exception.
                                DependentService.Stop();
                                DependentService.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                //check the status of the service
                if (WSController.Status.ToString() == "Running" || WSController.Status.ToString() == "Paused")
                {
                    WSController.Stop();
                }
                WSController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                return true;
            }
            return false;
        }
        public static bool PauseEx(this ServiceController WSController)
        {
            //check to see if the service can be paused and continue
            if (WSController.CanPauseAndContinue == true)
            {
                //check the status of the service
                if (WSController.Status.ToString() == "Running")
                {
                    WSController.Pause();
                }

                WSController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Paused);
                return true;
            }
            return false;
        }
    }
}
