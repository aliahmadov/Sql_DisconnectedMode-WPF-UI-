using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disconnected_SQL.Services
{
    internal class DMLOperations
    {

        private string _connectionString { get; set; }

        private SqlDataAdapter _adapter { get; set; }

        private DataSet _dataSet { get; set; }

        private SqlConnection _connection { get; set; }

        public DMLOperations()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _dataSet = new DataSet();
            _connection = new SqlConnection();
            _connection.ConnectionString = _connectionString;

        }
        public DataTable DisplayData()
        {
            _connection.Open();
            string query = "SELECT* FROM Authors";
            _adapter = new SqlDataAdapter(query, _connection);
            _adapter.Fill(_dataSet, "Authors");
            return _dataSet.Tables[0];
        }

        public DataTable InsertData(int id, string firstName, string lastName)
        {
            _connection.Open();

            FillDataSet();


            string query = @"INSERT INTO Authors VALUES(@id,@firstName,@lastName)";

            var insert_command = new SqlCommand(query, _connection);
            insert_command.Parameters.Add("@id",SqlDbType.Int).Value=id;
            insert_command.Parameters.Add("@firstName",SqlDbType.NVarChar).Value=firstName;
            insert_command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = lastName;
           
            _adapter.InsertCommand = insert_command;
            _adapter.InsertCommand.ExecuteNonQuery();
            _adapter.Update(_dataSet, "Authors");
            _dataSet.Clear();
            _adapter.Fill(_dataSet, "Authors");
            return (_dataSet.Tables[0]);
        }

        public void FillDataSet()
        {
            string select_query = "SELECT* FROM Authors";
            var select_command = new SqlCommand(select_query, _connection);
            _adapter = new SqlDataAdapter();
            _adapter.SelectCommand = select_command;
            _adapter.Fill(_dataSet, "Authors");
        }

        public DataTable DeleteData(int id)
        {
            _connection.Open();

            FillDataSet();

            string delete_query = "DELETE FROM Authors WHERE Id=@id";
            var delete_command = new SqlCommand(delete_query, _connection);
            delete_command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            });
            _adapter.DeleteCommand = delete_command;
            _adapter.DeleteCommand.ExecuteNonQuery();
            _adapter.Update(_dataSet, "Authors");
            _dataSet.Clear();
            _adapter.Fill(_dataSet, "Authors");
            return _dataSet.Tables[0];
        }

        public DataTable UpdateData(string firstName, string LastName, int id)
        {
            _connection.Open();

            FillDataSet();

            string query = $@"UPDATE Authors 
                            SET FirstName=@firstName, LastName=@lastName
                            WHERE Id=@id";

            var update_command = new SqlCommand(query, _connection);

            update_command.Parameters.Add(new SqlParameter { ParameterName = "@firstName", SqlDbType = SqlDbType.NVarChar, Value = firstName });
            update_command.Parameters.Add(new SqlParameter { ParameterName = "@lastName", SqlDbType = SqlDbType.NVarChar, Value = LastName });
            update_command.Parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = id });

            _adapter.UpdateCommand = update_command;
            _adapter.UpdateCommand.ExecuteNonQuery();
            _adapter.Update(_dataSet, "Authors");
            _dataSet.Clear();
            _adapter.Fill(_dataSet, "Authors");
            //_connection.Close();

            return _dataSet.Tables[0];
        }
    }
}
