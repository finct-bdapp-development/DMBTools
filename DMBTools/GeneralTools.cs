using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTier;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using System.Data;

namespace DMBTools
{
    public class GeneralTools
    {

        /// <summary>
        /// Records details of actions taken by the user. NB this relies on the audit table being in the agreed
        /// DMB format
        /// </summary>
        /// <param name="strID">The unique ID of the user</param>
        /// <param name="strAction">The action they took</param>
        /// <param name="connection">The connection string to the data source</param>
        /// <returns>1 = equals update is a success. 2 equals update failed</returns>
        public Int32 AddToAudit(string strID, string strAction, string connectionString, string connectionType)
        {
            DataConnector cls = new DataConnector();

            Int32 result = 0;

            DateTime dateT;
            string dateOf;
            dateT = DateTime.Parse(DateTime.Now.ToShortDateString());
            dateOf = dateT.ToString("yyyyMMdd");

            DateTime timeT;
            string timeOf;
            timeT = DateTime.Parse(DateTime.Now.ToShortTimeString());
            timeOf = timeT.ToString("HH:mm:ss");

            switch (connectionType)
            {
                case "SQL":
                    try
                    {
                        System.Data.SqlClient.SqlConnection SQLConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                        result = cls.insertTable("tblAudit", "AuditDate,AuditTime,PID,RequestID,AuditAction", "'" + dateOf + "','" + timeOf + "','" + Environment.UserName.ToString() + "','" + strID + "','" + strAction + "'", SQLConnection);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("DMBT-GT01s - unable to store the audit information - " + ex.Message);
                    }
                    break;
                case "ADO":
                    try
                    {
                        System.Data.OleDb.OleDbConnection ADOConnection = new System.Data.OleDb.OleDbConnection(connectionString);
                        result = cls.insertTable("tblAudit", "AuditDate,AuditTime,PID,RequestID,AuditAction", "'" + dateOf + "','" + timeOf + "','" + Environment.UserName.ToString() + "','" + strID + "','" + strAction + "'", ADOConnection);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("DMBT-GT01a - unable to store the audit information - " + ex.Message);
                    }
                    break;
                default:
                    throw new Exception("DMBT-GT02 - The connection type cannot be identified.");
            }
            
            return result;
        }

        /// <summary>
        /// Adds audit information to the data source using the new wrapper functionality. If the update fails
        /// an exception will be passed back to the calling routine
        /// </summary>
        /// <param name="passedUniqueID">The unique ID of the record</param>
        /// <param name="passedAction">The action taken on that record</param>
        /// <param name="connectionString">The string for connecting to the data source</param>
        public void AddToAudit(string passedUniqueID, string passedAction, string connectionString)
        {
            New_Wrapper.DataHandler MyHandler = new New_Wrapper.DataHandler();

            DateTime dateT;
            string dateOf;
            dateT = DateTime.Parse(DateTime.Now.ToShortDateString());
            dateOf = dateT.ToString("yyyyMMdd");

            DateTime timeT;
            string timeOf;
            timeT = DateTime.Parse(DateTime.Now.ToShortTimeString());
            timeOf = timeT.ToString("HH:mm:ss");
            try
            {
                string tempId = Guid.NewGuid().ToString();
                MyHandler.InsertData(connectionString, "INSERT INTO tblAudit (AuditID, AuditDate, AuditTime, PID, RequestID, AuditAction) VALUES ('" + tempId + "', '" + dateOf + "', '" + timeOf + "', '" + Environment.UserName.ToString() + "', '" + passedUniqueID + "', '" + passedAction + "')"); 
            }
            catch (Exception ex)
            {
                throw new Exception("DMBT-GT01a - unable to store the audit information - " + ex.Message);
            }
        }

        /// <summary>
        /// Checks if a character is a numeric
        /// </summary>
        /// <param name="passedCharacter">The character(s) to be checked for a numerical value</param>
        /// <returns></returns>
        public bool checkNumeric(string passedCharacter)
        {
            double dummyValue = new double();
            System.Globalization.CultureInfo myCultureInfo = new CultureInfo("en-US", true);
            return double.TryParse(passedCharacter, NumberStyles.Any, myCultureInfo.NumberFormat, out dummyValue);
        }

        /// <summary>
        /// Takes a string and adds a new string to it
        /// </summary>
        /// <param name="strPassedOriginalMessage">This is the original string</param>
        /// <param name="strpassedNewMessage">This is the string to be added to give the new string</param>
        /// <returns>The result of adding the new string to the original message</returns>
        public string BuildMessage(string strPassedOriginalMessage, string strpassedNewMessage)
        {
            if (strPassedOriginalMessage == "")
            {
                strPassedOriginalMessage = strpassedNewMessage;
            }
            else
            {
                strPassedOriginalMessage = strPassedOriginalMessage + Environment.NewLine + strpassedNewMessage;
            }
            return strPassedOriginalMessage;
        }

        /// <summary>
        /// Takes a string builder and adds a new string to it.
        /// </summary>
        /// <param name="sbPassedOriginalMessage">This is a reference to the original string</param>
        /// <param name="strPassedNewMessage">This is the string to be added to give the new string</param>
        /// <returns></returns>
        public System.Text.StringBuilder BuildMessage(System.Text.StringBuilder sbPassedOriginalMessage, string strPassedNewMessage)
        {
            if (sbPassedOriginalMessage.ToString() == "")
            {
                sbPassedOriginalMessage.Append(strPassedNewMessage);
            }
            else
            {
                sbPassedOriginalMessage.Append(Environment.NewLine);
                sbPassedOriginalMessage.Append(strPassedNewMessage);
            }
            return sbPassedOriginalMessage;
        }

        /// <summary>
        /// Join an array of strings into a single string - the strings will be joined without any characters to
        /// pad between each string
        /// </summary>
        /// <param name="strings">The strings that are to be joined together</param>
        /// <returns>A string containing the entries in the string array</returns>
        public string stringJoiner(string[] strings)
        {
            return joinerRoutine(strings, "");
        }

        /// <summary>
        /// Join an array of strings into a single string with a seperator character
        /// </summary>
        /// <param name="strings">The strings that are to be joined together</param>
        /// <param name="seperator">The character that will be inserted between each string</param>
        /// <returns>A string containing the entries in the string array</returns>
        public string stringJoiner(string[] strings, string seperator)
        {
            return joinerRoutine(strings, seperator);
        }

        /// <summary>
        /// Joins an array of strings into a single string
        /// </summary>
        /// <param name="strings">The strings to be joined together</param>
        /// <param name="seperator">The character that will be inserted between each string</param>
        /// <returns>A string containing the entries in the string array</returns>
        private string joinerRoutine(string[] strings, string seperator)
        {
            StringBuilder myString = new StringBuilder();
            foreach (string addString in strings)
            {
                if (myString.ToString() == "")
                {
                    myString.Append(addString);
                }
                else
                {
                    myString.Append(seperator + addString);
                }
            }
            return myString.ToString();
        }

        /// <summary>
        /// Inserts default values into a field on a form. This is based on the standard DMB form design where
        /// controls will be held within a control panel (name controlPanel) within the form. The routine should
        /// not be used for any application which does not conform to this standard. 
        /// </summary>
        /// <param name="passedForm">The name of the form on which the controls are held</param>
        /// <param name="originatingControlName">The name of the control within the form</param>
        /// <param name="connectionType">The type of data source connection</param>
        /// <param name="originatingValue">The value to be checked for whether a default applies</param>
        /// <param name="connectionString">The connection string</param>
        public void insertDefaults(ref System.Windows.Forms.Form passedForm, string originatingControlName, string connectionType,string originatingValue, string connectionString)
        {
            DataSet defaultsDataset = new DataSet();
            DataConnector myHandler = new DataConnector();
            string SQLQuery = "SELECT DefaultValues.OriginatingForm, DefaultValues.OriginatingField, DefaultValues.OriginatingValue, DefaultValues.AffectedField, DefaultValues.DefaultValue FROM DefaultValues WHERE (((DefaultValues.OriginatingForm)='" + passedForm.Name.ToString() + "') AND ((DefaultValues.OriginatingField)='" + originatingControlName + "') AND ((DefaultValues.OriginatingValue)='" + originatingValue + "'));";
            switch (connectionType)
            {
                case "SQL":
                    System.Data.SqlClient.SqlConnection SQLConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                    defaultsDataset = myHandler.getDataSetViaSQL(SQLQuery, "defaults", SQLConnection);
                    break;
                case "ADO":
                    System.Data.OleDb.OleDbConnection ADOConnection = new System.Data.OleDb.OleDbConnection(connectionString);
                    defaultsDataset = myHandler.getDataSetViaSQL(SQLQuery, "defaults", ADOConnection);
                    break;
                default:
                    throw new Exception("DMBT-GT03 - The connection type cannot be identified.");
            }
            DataTable defaultsTable = defaultsDataset.Tables["defaults"];
            if (defaultsTable.Rows.Count != 0)
            {
                foreach (DataRow myRow in defaultsTable.Rows)
                {
                    //identify the target field type and then activate a switch statement to insert the relevant
                    //info
                    switch (passedForm.Controls["controlPanel"].Controls[myRow["AffectedField"].ToString()].GetType().ToString())
                    {
                        case "System.Windows.Forms.ListBox":
                            System.Windows.Forms.ListBox myListBox = new System.Windows.Forms.ListBox();
                            myListBox = passedForm.Controls["controlPanel"].Controls[myRow["AffectedField"].ToString()] as ListBox;
                            //myListBox.SelectedItem = myRow["DefaultValue"].ToString(); //#####REMOVED 17/09/2015
                            myListBox.Text= myRow["DefaultValue"].ToString();//#####ADDED 17/09/2015
                            //insertDefault(ref myListBox, myRow["DefaultValue"].ToString());
                            break;
                        case "System.Windows.Forms.ComboBox":
                            System.Windows.Forms.ComboBox myCombo = new System.Windows.Forms.ComboBox();
                            myCombo = passedForm.Controls["controlPanel"].Controls[myRow["AffectedField"].ToString()] as ComboBox;
                            //myCombo.SelectedItem = myRow["DefaultValue"].ToString();//#####REMOVED 17/09/2015
                            myCombo.Text = myRow["DefaultValue"].ToString();//#####ADDED 17/09/2015
                            //insertDefault(ref myCombo, myRow["DefaultValue"].ToString());
                            break;
                        case "System.Windows.Forms.TextBox":
                            System.Windows.Forms.TextBox myTextBox = new System.Windows.Forms.TextBox();
                            myTextBox = passedForm.Controls["controlPanel"].Controls[myRow["AffectedField"].ToString()] as TextBox;
                            myTextBox.Text = myRow["DefaultValue"].ToString();
                            //insertDefault(ref myTextBox, myRow["DefaultValue"].ToString());
                            break;
                        default:
                            MessageBox.Show("DMBT-GT04 - The system cannot determine the default values for this selection. Please notify your line manager.");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Allows a list box to be customised so that the user can deselect an item. If the user clicks on any
        /// part of the list that is not a list item then the list will be reset back to its default status. This
        /// method is only appropriate for list boxes where a single item can be selected
        /// </summary>
        /// <param name="affectedList">The list box which has been clicked on</param>
        /// <param name="xCoord">The x coordinate when the mouse was clicked on the control</param>
        /// <param name="yCoord">The y coordinate when the mouse was clicked on the control</param>
        /// <returns>True = the list no longer has any items selected. False - an item was selected from the
        /// list</returns>
        public Boolean deselectListItem(ref ListBox affectedList, int xCoord, int yCoord)
        {
            Boolean whiteSpace = false;
            if (affectedList.IndexFromPoint(xCoord, yCoord) == -1)
            {
                affectedList.SelectedIndex = -1;
                whiteSpace = true;
            }
            return whiteSpace;
        }

        #region "Clear form"

        /// <summary>
        /// Clears the entries in a form. This currently includes textboxes, comboboxes, list boxes and datagrid
        /// views. As the accessibility routine is based on controls being held within a panel on the form this
        /// mirrors this by resetting the controls in the panel.
        /// </summary>
        /// <param name="targetPanel">The panel which contains the controls to be reset</param>
        public void clearControls(ref Panel targetPanel)
        {
            foreach (Control myControl in targetPanel.Controls)
            {
                string controlType = myControl.GetType().ToString();
                switch (controlType)
                {
                    case "System.Windows.Forms.TextBox":
                        myControl.Text = "";
                        break;
                    case "System.Windows.Forms.ComboBox":
                        myControl.Text = null;
                        break;
                    case "System.Windows.Forms.DataGridView":
                        DataGridView myView = (DataGridView)myControl;
                        myView.DataSource = null;
                        myView.Rows.Clear();
                        break;
                    case "System.Windows.Forms.ListBox":
                        ListBox myList = (ListBox)myControl;
                        myList.SelectedIndex = -1;
                        break;
                    default:

                        break;
                }
            }
        }

        /// <summary>
        ///  Clears the entries in a form. This currently includes textboxes, comboboxes, list boxes and datagrid
        /// views.
        /// </summary>
        /// <param name="targetCollection">The form to be cleared</param>
        public void clearControls(System.Windows.Forms.Control.ControlCollection targetCollection)
        {
            foreach (Control myControl in targetCollection)
            {
                string controlType = myControl.GetType().ToString();
                switch (controlType)
                {
                    case "System.Windows.Forms.TextBox":
                        myControl.Text = "";
                        break;
                    case "System.Windows.Forms.ComboBox":
                        myControl.Text = null;
                        break;
                    case "System.Windows.Forms.DataGridView":
                        DataGridView myView = (DataGridView)myControl;
                        myView.DataSource = null;
                        myView.Rows.Clear();
                        break;
                    case "System.Windows.Forms.ListBox":
                        ListBox myList = (ListBox)myControl;
                        myList.SelectedIndex = -1;
                        break;
                    default:

                        break;
                }
            }
        }

        #endregion

        #region "Accessibility settings"

        /// <summary>
        /// Automatically configures a Windows form to meet the DMB development team accessibility standards.
        /// </summary>
        /// <param name="passedForm">The form to be configured</param>
        /// <returns>The configured Form</returns>
        /// <remarks>The current standards are:
        /// Font - matches the user's settings for message boxes</remarks>
        public System.Windows.Forms.Form FormatForm(System.Windows.Forms.Form passedForm)
        {
            passedForm.Font = System.Drawing.SystemFonts.MessageBoxFont;
            passedForm.ForeColor = System.Drawing.SystemColors.WindowText;
            passedForm.BackColor = System.Drawing.SystemColors.Window;
            return passedForm;
        }

        #endregion

        #region "Configure datagrid"

        /// <summary>
        /// Displays only the specified columns in a datagrid control
        /// </summary>
        /// <param name="passedGrid">The datagrid that is to be configured</param>
        /// <param name="fieldsToDisplay">A list of the fields to be displayed</param>
        /// <remarks>The fieldsToDisplay array is a comma seperated value with the name of the field to be displayed and the
        /// caption that is to be applied to it. For instance to display a field named fldReference and display the header text
        /// as SA ref the entry in the array would be fldReference,SA Ref</remarks>
        public void configureDatagrid(ref DataGridView passedGrid, string[] fieldsToDisplay)
        {
            //Loop through the columns in the grid
            foreach (DataGridViewColumn myColumn in passedGrid.Columns)
            {
                myColumn.Visible = false;
                //Check whether the column appears in the list of fields to display
                foreach (string fieldInfo in fieldsToDisplay)
                {
                    string[] tempData = fieldInfo.Split(',');
                    if (tempData[0] == myColumn.Name.ToString())
                    {
                        if (tempData[1] != "")
                        {
                            myColumn.HeaderText = tempData[1];
                        }
                        myColumn.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Automatically resizes the data grid so that the width is based on the largest entry in each column
        /// </summary>
        public void ResizeGrid(ref DataGridView passedView)
        {
            foreach (DataGridViewColumn item in passedView.Columns)
            {
                item.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //CompositesView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        #endregion

    }
}
