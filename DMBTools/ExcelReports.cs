using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTier;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace DMBTools
{
    public class ExcelReports
    {

        #region "Properties"

        string m_conType;
        /// <summary>
        /// The type of data connection being made - the current version only supports ADO and SQL
        /// </summary>
        public string connType
        {
            set
            {
                if (value != "ADO" && value != "SQL")
                {
                    throw new System.Exception("The connection type can only be ADO or SQL.");
                }
                m_conType = value;
            }
        }

        string m_connString;
        /// <summary>
        /// The string to allow a connection to be made to the data source.
        /// </summary>
        public string connString
        {
            set
            {
                if (value == "")
                {
                    throw new System.Exception("A database connection string has not been supplied to the reporting system.");
                }
                m_connString = value;
            }
        }

        string m_targetPath;

        public string targetPath
        {
            set
            {
                if (value == "")
                {
                    throw new System.Exception("A path has not been supplied - this is essential to ensure that the report is saved.");
                }
                m_targetPath = value;
            }
        }

        #endregion

        #region "Initialise class"

        /// <summary>
        /// Initialises the class and ensures that essential properties are set
        /// </summary>
        /// <param name="passedConnType">The type of connection being made. The system currently supports ADO and
        /// SQL</param>
        /// <param name="passedConnString">The connection string for the data source</param>
        /// <param name="passedPath">The path where the report will be saved.</param>
        public ExcelReports(string passedConnType, string passedConnString, string passedPath)
        {
            m_conType = passedConnType;
            m_connString = passedConnString;
            m_targetPath = passedPath;
        }

        /// <summary>
        /// Initialised the class without setting the essential properties. The properties must be set via code
        /// prior to using the class or error messages will be thrown
        /// </summary>
        public ExcelReports()
        {
        }

        #endregion

        #region "Generate report"

        /// <summary>
        /// Extracts data from a data source and copies it into Excel. The information starts at cell A1 and 
        /// is not formatted
        /// </summary>
        /// <param name="SQLQuery">The filter to be applied</param>
        public void generateReport(string SQLQuery)
        {
            DataConnector myHandler = new DataConnector();
            DataSet myRecords = new DataSet();
            switch (m_conType)
            {
                case "ADO":
                    System.Data.OleDb.OleDbConnection dataConn = new System.Data.OleDb.OleDbConnection(m_connString);
                    myRecords = myHandler.getDataSetViaSQL(SQLQuery, "ReportRecords", dataConn);
                    break;
                case "SQL":

                    break;
                default:
                    throw new System.Exception("The reporting system cannot connect to a data source via " + m_conType + ".");
            }
            if (myRecords.Tables.Count != 1)
            {
                throw new System.Exception("The connection details supplied has not returned a valid connection.");
            }
            myRecords.WriteXml(m_targetPath);
        }

        #endregion

    }

}
