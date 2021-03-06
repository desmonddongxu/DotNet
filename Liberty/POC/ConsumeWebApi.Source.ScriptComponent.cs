#region Help:  Introduction to the Script Component
/* The Script Component allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services data flow.
 *
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script component. */
#endregion

#region Namespaces
using System;
using System.Data;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

#endregion

/// <summary>
/// This is the class to which to add your code.  Do not change the name, attributes, or parent
/// of this class.
/// </summary>
[Microsoft.SqlServer.Dts.Pipeline.SSISScriptComponentEntryPointAttribute]
public class ScriptMain : UserComponent
{
    #region Help:  Using Integration Services variables and parameters
    /* To use a variable in this script, first ensure that the variable has been added to
     * either the list contained in the ReadOnlyVariables property or the list contained in
     * the ReadWriteVariables property of this script component, according to whether or not your
     * code needs to write into the variable.  To do so, save this script, close this instance of
     * Visual Studio, and update the ReadOnlyVariables and ReadWriteVariables properties in the
     * Script Transformation Editor window.
     * To use a parameter in this script, follow the same steps. Parameters are always read-only.
     *
     * Example of reading from a variable or parameter:
     *  DateTime startTime = Variables.MyStartTime;
     *
     * Example of writing to a variable:
     *  Variables.myStringVariable = "new value";
     */
    #endregion

    #region Help:  Using Integration Services Connnection Managers
    /* Some types of connection managers can be used in this script component.  See the help topic
     * "Working with Connection Managers Programatically" for details.
     *
     * To use a connection manager in this script, first ensure that the connection manager has
     * been added to either the list of connection managers on the Connection Managers page of the
     * script component editor.  To add the connection manager, save this script, close this instance of
     * Visual Studio, and add the Connection Manager to the list.
     *
     * If the component needs to hold a connection open while processing rows, override the
     * AcquireConnections and ReleaseConnections methods.
     * 
     * Example of using an ADO.Net connection manager to acquire a SqlConnection:
     *  object rawConnection = Connections.SalesDB.AcquireConnection(transaction);
     *  SqlConnection salesDBConn = (SqlConnection)rawConnection;
     *
     * Example of using a File connection manager to acquire a file path:
     *  object rawConnection = Connections.Prices_zip.AcquireConnection(transaction);
     *  string filePath = (string)rawConnection;
     *
     * Example of releasing a connection manager:
     *  Connections.SalesDB.ReleaseConnection(rawConnection);
     */
    #endregion

    #region Help:  Firing Integration Services Events
    /* This script component can fire events.
     *
     * Example of firing an error event:
     *  ComponentMetaData.FireError(10, "Process Values", "Bad value", "", 0, out cancel);
     *
     * Example of firing an information event:
     *  ComponentMetaData.FireInformation(10, "Process Values", "Processing has started", "", 0, fireAgain);
     *
     * Example of firing a warning event:
     *  ComponentMetaData.FireWarning(10, "Process Values", "No rows were received", "", 0);
     */
    #endregion
    private static HttpClient _client = new HttpClient();

    public class Coordinates
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
    public class Address
    {
        public string PropertyTypeCode { get; set; }
        public float Score { get; set; }
        public string UAID { get; set; }
        public string UUAID { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetSubAddress { get; set; }
        public string PostalCode { get; set; }
        public string Municipality { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public Coordinates Coordinates { get; set; }
    }


    /// <summary>
    /// This method is called once, before rows begin to be processed in the data flow.
    ///
    /// You can remove this method if you don't need to do anything here.
    /// </summary>
    public override void PreExecute()
    {
        base.PreExecute();
        /*
         * Add your code here
         */
    }

    /// <summary>
    /// This method is called after all the rows have passed through this component.
    ///
    /// You can delete this method if you don't need to do anything here.
    /// </summary>
    public override void PostExecute()
    {
        base.PostExecute();
        /*
         * Add your code here
         */
    }

    public override void CreateNewOutputRows()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12
            | SecurityProtocolType.Ssl3;

        string serviceUrl = Variables.ServiceUrl.ToString();
        string endpoint = Variables.Endpoint.ToString();

        _client.BaseAddress = new Uri(serviceUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var result = GetDataAsync(endpoint, this).GetAwaiter().GetResult();
        
        foreach (Address theAddress in result)
        {
            Output0Buffer.AddRow();
            Output0Buffer.StreetNumber = theAddress.StreetNumber;
            Output0Buffer.StreetName = theAddress.StreetName;
            Output0Buffer.StreetSubAddress = theAddress.StreetSubAddress;
            Output0Buffer.PropertyTypeCode = theAddress.PropertyTypeCode;
            Output0Buffer.Municipality = theAddress.Municipality;
            Output0Buffer.UAID = theAddress.UAID;
            Output0Buffer.UUAID = theAddress.UUAID;
            Output0Buffer.Province = theAddress.Province;
            Output0Buffer.PostalCode = theAddress.PostalCode;
            Output0Buffer.Country = theAddress.Country;
            Output0Buffer.Lat = theAddress.Coordinates.Lat;
            Output0Buffer.Lon = theAddress.Coordinates.Lon;
        }

    }

    public static async System.Threading.Tasks.Task<List<Address>> GetDataAsync(string endpoint, ScriptMain scriptMain)
    {
        bool cancelled;
        var addressList = new List<Address>();
        
        try
        {
            //var response = await _client.GetAsync(endpoint);
            var response = await _client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            var result = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return DeserialzeJsonStream<List<Address>>(result);
            }
            else
            {
                string errorContent = string.Empty;
                if (result != null)
                {
                    using (var streamReader = new StreamReader(result))
                    {
                        errorContent = await streamReader.ReadToEndAsync();
                    }
                }
                scriptMain.ComponentMetaData.FireWarning(0, "WebApi", $"Request {endpoint} return status: {(int)response.StatusCode} - {response.ReasonPhrase} \n {errorContent}" , string.Empty, 0);
            }
        }
        catch (Exception ex)
        {
            scriptMain.ComponentMetaData.FireError(0, "WebApi", $"{endpoint} in error {ex.Message} \n {ex.InnerException}", string.Empty, 0, out cancelled);
        }
        return addressList;

    }

    public static T DeserialzeJsonStream<T>(Stream stream)
    {
        if(stream == null || stream.CanRead == false)
        {
            return default(T);
        }

        using (var streamReader = new StreamReader(stream))
        using (var jsonTextReader = new JsonTextReader(streamReader))
        {

            var jsonSerializer = new JsonSerializer();
            var result = jsonSerializer.Deserialize<T>(jsonTextReader);
            return result;
        }
    }

}
