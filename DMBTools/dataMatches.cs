using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DTier;

namespace DMBTools
{
    public class dataMatches
    {

        private DataSet m_filteredData;
        /// <summary>
        /// Holds the result of any filtered data - this makes the data available to the calling code
        /// </summary>
        public DataSet filteredData
        {
            get
            {
                return m_filteredData;
            }
        }

        /// <summary>
        /// Checks a string to try to find a match.
        /// </summary>
        /// <param name="checkString">The string which will be searched for a match</param>
        /// <param name="forString">The string which the routine will attempt to find a match for</param>
        /// <param name="exactMatch">Whether the two strings have to be identical or whether the string to be
        /// matched can be part of the string being searched</param>
        /// <returns>True if a match has been found and false if it has not</returns>
        public Boolean matchFound(string checkString, string forString, Boolean exactMatch)
        {
            Boolean match = false;

            return match;
        }

        /// <summary>
        /// Checks a field in a data table to see if any of the rows contain a matching record
        /// </summary>
        /// <param name="checkTable">The table that will be searched for a match</param>
        /// <param name="fieldName">The column name to be checked</param>
        /// <param name="whereClause">The string which the routine will attempt to find a match for</param>
        /// <param name="exactMatch">Whether the string should be identical to the content of the field or whether
        /// it can be part of the field</param>
        /// <returns>True if a match has been found and false if it has not</returns>
        public Boolean matchFound(DataTable checkTable, string fieldNames, string whereClause, Boolean exactMatch)
        {
            Boolean match = false;
            
            return match;
        }

        public Boolean matchFound(string checkTable, string fieldNames, string whereClause, System.Data.OleDb.OleDbConnection myConn)
        {
            Boolean match = false;
            if (string.IsNullOrEmpty(checkTable) || string.IsNullOrEmpty(fieldNames) || string.IsNullOrEmpty(whereClause))
            {
                throw new Exception("The method dataMatches.MatchFound was not passed the correct information.");
            }
            DataConnector myHandler = new DataConnector();
            DataSet matchingRecords = myHandler.getDataSetValues(checkTable, fieldNames, whereClause, "", "", "Results", myConn);
            if (matchingRecords.Tables["Results"].Rows.Count > 0)
            {
                m_filteredData = matchingRecords;
                match = true;
            }
            else
            {
                match = false;
            }            
            return match;
        }

        /// <summary>
        /// Check whether there are matching records that match the specified filter
        /// </summary>
        /// <param name="provider">The connection string for the data source</param>
        /// <param name="password">The encrypted data source password</param>
        /// <param name="query">The SQL query for filtering the data</param>
        /// <param name="connectionType">The type of data connection (i.e. ADO or SQL)</param>
        /// <returns></returns>
        public DataSet matchFound(string provider, string password, string query, string connectionType)
        {
            New_Wrapper.DataHandler myHandler = new New_Wrapper.DataHandler();
            switch (connectionType)
            {
              //  case "ADO":
              //      myHandler = new New_Wrapper.DataHandler(provider, password, query); // password to connect ADO to Access no longer required.
              //      break;
                case "SQL":
                    myHandler = new New_Wrapper.DataHandler(provider, query);
                    break;
                default:
                    throw new Exception("Unrecognised connection type.");
            }
            DataSet temp = myHandler.CreateDataset();
            myHandler.CloseConnection();
            myHandler = null;
            return temp;
        }

        /// <summary>
        /// Checks fields within a table to locate records which 
        /// </summary>
        /// <param name="checkTable"></param>
        /// <param name="fieldNames"></param>
        /// <param name="fieldValues"></param>
        /// <param name="exactMatch"></param>
        /// <param name="myConn"></param>
        /// <returns></returns>
        public Boolean matchFound(string checkTable, string[] fieldNames, string[] fieldValues, Boolean exactMatch, System.Data.OleDb.OleDbConnection myConn)
        {
            Boolean match = false;

            return match;
        }

        /// <summary>
        /// Checks fields within a table to locate records which 
        /// </summary>
        /// <param name="checkTable"></param>
        /// <param name="fieldNames"></param>
        /// <param name="fieldValues"></param>
        /// <param name="exactMatch"></param>
        /// <param name="myConn"></param>
        /// <returns></returns>
        public Boolean matchFound(string checkTable, string[] fieldNames, string[] fieldValues, Boolean exactMatch, System.Data.SqlClient.SqlConnection myConn)
        {
            Boolean match = false;

            return match;
        }

        public Boolean matchFound(DataTable checkTable, DataTable forTable, Boolean exactMatch)
        {
            Boolean match = false;

            return match;
        }

        public Boolean matchFound(string query, string connectionType)
        {
            Boolean match = false;

            return match;
        }

    }
}
