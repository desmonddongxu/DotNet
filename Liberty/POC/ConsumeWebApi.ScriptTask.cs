#region Help:  Introduction to the script task
/* The Script Task allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services control flow. 
 * 
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script task. */
#endregion


#region Namespaces
using System;
using System.Data;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.OleDb;
#endregion

namespace ST_b373524a6d9a4c36995e2caac83f63ba
{
    /// <summary>
    /// ScriptMain is the entry point class of the script.  Do not change the name, attributes,
    /// or parent of this class.
    /// </summary>
	[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
	public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
	{
        #region Help:  Using Integration Services variables and parameters in a script
        /* To use a variable in this script, first ensure that the variable has been added to 
         * either the list contained in the ReadOnlyVariables property or the list contained in 
         * the ReadWriteVariables property of this script task, according to whether or not your
         * code needs to write to the variable.  To add the variable, save this script, close this instance of
         * Visual Studio, and update the ReadOnlyVariables and 
         * ReadWriteVariables properties in the Script Transformation Editor window.
         * To use a parameter in this script, follow the same steps. Parameters are always read-only.
         * 
         * Example of reading from a variable:
         *  DateTime startTime = (DateTime) Dts.Variables["System::StartTime"].Value;
         * 
         * Example of writing to a variable:
         *  Dts.Variables["User::myStringVariable"].Value = "new value";
         * 
         * Example of reading from a package parameter:
         *  int batchId = (int) Dts.Variables["$Package::batchId"].Value;
         *  
         * Example of reading from a project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].Value;
         * 
         * Example of reading from a sensitive project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].GetSensitiveValue();
         * */

        #endregion

        #region Help:  Firing Integration Services events from a script
        /* This script task can fire events for logging purposes.
         * 
         * Example of firing an error event:
         *  Dts.Events.FireError(18, "Process Values", "Bad value", "", 0);
         * 
         * Example of firing an information event:
         *  Dts.Events.FireInformation(3, "Process Values", "Processing has started", "", 0, ref fireAgain)
         * 
         * Example of firing a warning event:
         *  Dts.Events.FireWarning(14, "Process Values", "No values received for input", "", 0);
         * */
        #endregion

        #region Help:  Using Integration Services connection managers in a script
        /* Some types of connection managers can be used in this script task.  See the topic 
         * "Working with Connection Managers Programatically" for details.
         * 
         * Example of using an ADO.Net connection manager:
         *  object rawConnection = Dts.Connections["Sales DB"].AcquireConnection(Dts.Transaction);
         *  SqlConnection myADONETConnection = (SqlConnection)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Sales DB"].ReleaseConnection(rawConnection);
         *
         * Example of using a File connection manager
         *  object rawConnection = Dts.Connections["Prices.zip"].AcquireConnection(Dts.Transaction);
         *  string filePath = (string)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Prices.zip"].ReleaseConnection(rawConnection);
         * */
        #endregion

        private static HttpClient _client = new HttpClient();
        private static string _outboundFolder;
        private static int _batch = 20;

        public class ExtractEndpoint
        {
            public string Name { get; set; }
            public string Endpoint { get; set; }
        } 

        
		/// <summary>
        /// This method is called when this script task executes in the control flow.
        /// Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
        /// To open Help, press F1.
        /// </summary>
		public void Main()
		{
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;

            _outboundFolder = Dts.Variables["$Package::OutboundFolder"].Value.ToString();
            string serviceUrl = Dts.Variables["$Package::ServiceUrl"].Value.ToString();

            _client.BaseAddress = new Uri(serviceUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            DataTable theTable = new DataTable();
            OleDbDataAdapter theAdapter = new OleDbDataAdapter();
            theAdapter.Fill(theTable, Dts.Variables["EndpointList"].Value); 

            var extractEndpointList = new List<ExtractEndpoint>();
            for (var i = 0; i < theTable.Rows.Count; i++)
            {
                extractEndpointList.Add(new ExtractEndpoint { Name = theTable.Rows[i]["Name"].ToString(), Endpoint = theTable.Rows[i]["Endpoint"].ToString()});
            }


            RunGetAllDataAsync(Dts, extractEndpointList).GetAwaiter().GetResult();

            Dts.TaskResult = (int)ScriptResults.Success;
		}

        public static async System.Threading.Tasks.Task RunGetAllDataAsync(Microsoft.SqlServer.Dts.Tasks.ScriptTask.ScriptObjectModel dts, List<ExtractEndpoint> extractRequests)
        {
            var pendingRequests = new List<System.Threading.Tasks.Task>();
            bool fireAgain = true;

            try
            {
                for (var i = 0; i < _batch && extractRequests.Count > 0; i++)
                {
                    SendRequest(pendingRequests, extractRequests, dts);
                }

                while (pendingRequests.Count > 0)
                {
                    System.Threading.Tasks.Task finishedTask = await System.Threading.Tasks.Task.WhenAny(pendingRequests);
                    pendingRequests.Remove(finishedTask);
                    dts.Events.FireInformation(0, "Consume WebAPI", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")} Task {finishedTask.Id.ToString()} is {finishedTask.Status}", string.Empty, 0, ref fireAgain);
                    if (extractRequests.Count > 0)
                    {
                        SendRequest(pendingRequests, extractRequests, dts);
                    }
                }
            }
            catch (Exception ex)
            {
                dts.Events.FireError(0, "Consume WebAPI", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")} {ex.Message.ToString()}", string.Empty, 0);
                dts.TaskResult = (int)ScriptResults.Failure;
            }
        }

        public static void SendRequest (List<System.Threading.Tasks.Task> getDataTasks, List<ExtractEndpoint> endpointList, Microsoft.SqlServer.Dts.Tasks.ScriptTask.ScriptObjectModel dts)
        {
            bool fireAgain = true;
            var responseTask = GetDataAsync(endpointList[0].Name, endpointList[0].Endpoint, dts);
            getDataTasks.Add(responseTask);
            dts.Events.FireInformation(0, "Consume WebAPI", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")} Task {responseTask.Id.ToString()} for request {endpointList[0].Name} is {responseTask.Status}", string.Empty, 0, ref fireAgain);
            endpointList.RemoveAt(0);
        }

        public static async System.Threading.Tasks.Task<HttpStatusCode> GetDataAsync(string name, string endpoint, Microsoft.SqlServer.Dts.Tasks.ScriptTask.ScriptObjectModel dts)
        {
            bool fireAgain = true;
            var response = await _client.GetAsync(endpoint);
            string result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                dts.Events.FireInformation(0, "Consume WebAPI", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")} Request {name} on {endpoint} has status: {(int)response.StatusCode} - {response.ReasonPhrase}", string.Empty, 0, ref fireAgain);
                var fileName = Path.Combine(_outboundFolder, name + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfffff") + ".json");
                File.AppendAllText(fileName, result);
            }
            else
            {
                dts.Events.FireWarning(0, "Consume WebAPI", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")} Request {name} on {endpoint} has status: {(int)response.StatusCode} - {response.ReasonPhrase} \n{result}", string.Empty, 0);
            }
            return response.StatusCode;
        }

        #region ScriptResults declaration
        /// <summary>
        /// This enum provides a convenient shorthand within the scope of this class for setting the
        /// result of the script.
        /// 
        /// This code was generated automatically.
        /// </summary>
        enum ScriptResults
        {
            Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
            Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
        };
        #endregion

	}
}