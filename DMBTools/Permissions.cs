using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DTier;

namespace DMBTools
{
    public class Permissions
    {

        #region "retrieveUserRole"

        /// <summary>
        /// Checks whether a user has access to an application. This is based on a data table being passed to
        /// the routine. The table must be in the agreed format for a DMB user table
        /// </summary>
        /// <param name="userTable">The table which includes the user details</param>
        /// <param name="userID">The user to be located</param>
        /// <returns>A string containing the user's role. A blank string denotes that they do not have an 
        /// assigned role in the system</returns>
        public string retrieveUserRole(DataTable userTable, string userID)
        {
            string userRole = "";
            DataView myView = new DataView(userTable);
            myView.RowFilter = "userID = '" + userID + "'";
            myView.Sort = "userID";
            userRole = myView[0]["userRole"].ToString();
            return userRole;
        }

        /// <summary>
        /// Checks whether a user has access to an application. This is based on the connection string being
        /// sent to the routine to allow it to connect to the user data. The table must be in the agreed format
        /// for a DMB user table
        /// </summary>
        /// <param name="connection">A string to allow a connection to be made to the data source</param>
        /// <param name="query">A SQL query which can be applied as the filter.</param>
        /// <param name="roleColumn">The name of the data column that holds the user role</param>
        /// <returns>A string containing the user's role. A blank string denotes that they do not have an
        /// assigned user role in the system</returns>
        public string retrieveUserRole(string connection, string query,string roleColumn)
        {
            string userRole = "";
            DTier.DataConnector myUsers = new DataConnector();
            try
            {
                userRole = myUsers.getDataSingleValue(connection, query, roleColumn);
            }
            catch
            {
                throw new Exception("The connection details passed to retrieveUserRole method are invalid.");
            }
            finally
            {
                myUsers = null;
            }
            return userRole;
        }

        /// <summary>
        /// Checks whether a user has access to an application. This is based on the connection string being
        /// sent to the routine to allow it to connect to the user data. The table must be in the agreed format
        /// for a DMB user table. This version will only accept OLEDb connections
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="roleColumn"></param>
        /// <param name="accessDatabase"></param>
        /// <returns></returns>
        public string retrieveUserRole(string connection, string query, string roleColumn, Boolean accessDatabase)
        {
            string userRole = "";
            if (accessDatabase == true)
            { 
                DTier.DataConnector myUsers = new DataConnector();
                try
                {
                    userRole = myUsers.getDataSingleValueOLE(connection, query, roleColumn);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());//"The connection details passed to retrieveUserRole method are invalid.");
                }
                finally
                {
                    myUsers = null;
                }
            }
            return userRole;
        }

        #endregion

        #region "Create user"

        /// <summary>
        /// Adds a user to the system based on settings sent to the routine. NB if the update fails then an
        /// exception will be thrown to the calling code - specific details of the reason for failure will not
        /// be available
        /// </summary>
        /// <param name="tableName">A comma seperated list of the affected tables</param>
        /// <param name="fieldNames">A comma seperated list of the fields within the table. The elements in the
        /// array must be in the same order as the associated tableName element</param>
        /// <param name="fieldValues">A comma seperated list of the values to be stored. The elements in the
        /// array must be in the same order as the associated tableName element</param>
        /// <param name="dataConnection">An OLEdb data connection</param>
        public void createUser(string[] tableName, string[] fieldNames, string[] fieldValues, System.Data.OleDb.OleDbConnection dataConnection)
        {
            int currentElement = 0;
            if (tableName.GetLength(0) == 0 || fieldNames.GetLength(0) == 0 || fieldValues.GetLength(0) == 0)
            {
                foreach (string tableString in tableName)
                {
                    DataConnector myHandler = new DataConnector();
                    if (myHandler.insertTable(tableString, fieldNames[currentElement], fieldValues[currentElement], dataConnection) == 0)
                    {
                        throw new Exception("A fatal error occurred when trying to update the user details.");
                    }
                    currentElement++;
                }
            }
        }

        /// <summary>
        /// Adds a user to the system based on settings sent to the routine. NB if the update fails then an
        /// exception will be thrown to the calling code - specific details of the reason for failure will not
        /// be available
        /// </summary>
        /// <param name="tableName">A comma seperated list of the affected tables</param>
        /// <param name="fieldNames">A comma seperated list of the fields within the table. The elements in the
        /// array must be in the same order as the associated tableName element</param>
        /// <param name="fieldValues">A comma seperated list of the values to be stored. The elements in the
        /// array must be in the same order as the associated tableName element</param>
        /// <param name="dataConnection">A SQL data connection</param>
        public void createUser(string[] tableName, string[] fieldNames, string[] fieldValues, System.Data.SqlClient.SqlConnection dataConnection)
        {
            int currentElement = 0;
            if (tableName.GetLength(0) == 0 || fieldNames.GetLength(0) == 0 || fieldValues.GetLength(0) == 0)
            {
                foreach (string tableString in tableName)
                {
                    DataConnector myHandler = new DataConnector();
                    if (myHandler.insertTable(tableString, fieldNames[currentElement], fieldValues[currentElement], dataConnection) == 0)
                    {
                        throw new Exception("A fatal error occurred when trying to update the user details.");
                    }
                    currentElement++;
                }
            }
        }

        #endregion

    }
}
