using MusicDB.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MusicDB.Helpers
{

    /// <summary>
    /// Helper to access database
    /// </summary>
    public class DBHelper
    {
        //private string _connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=MusicDB;Data Source=DESKTOP-C7DRG20";
        private string _connectionString = "Password=a6Intruder;Persist Security Info=True;User ID = pedrolopes; Initial Catalog = MusicDBStagging; Data Source = musicdb.c0d9wnivbgn8.eu-central-1.rds.amazonaws.com";

        /// <summary>
        /// Retrieves a connection to the MusicDB
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Gets a scalarv value from the MusicDB
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public string GetScalarStringValue(string sqlCommand)
        {
            string result = null;

            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                result = (string)cmd.ExecuteScalar();
                if (string.IsNullOrEmpty(result))
                {
                    result = null;
                }
                con.Close();
            }

            return result;
        }

        public int? GetScalarIntValue(string sqlCommand)
        {
            return GetScalarIntValue(sqlCommand, null);
        }

        /// <summary>
        /// Gets a scalarv value from the MusicDB
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public int? GetScalarIntValue(string sqlCommand, Dictionary<string, object> parameters)
        {
            int result = 0;
            object resultTmp = null;

            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, con);

                if (parameters !=null)
                {
                    foreach (string parameter in parameters.Keys)
                    {
                        if (parameters[parameter] != null)
                        {
                            if (parameters[parameter].GetType().Name == "Byte[]")
                            {
                                SqlParameter param = cmd.Parameters.Add(parameter, SqlDbType.VarBinary);
                                param.Value = parameters[parameter];
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                        }
                    }
                }

                resultTmp = cmd.ExecuteScalar();
                con.Close();

                if (resultTmp != null)
                    result = (int)int.Parse(resultTmp.ToString());
            }

            return result;
        }

        public Int32 InsertUpdateRow(string cmdString, Dictionary<string, object> parameters)
        {
            return InsertUpdateRow(cmdString, parameters, null);
        }

        /// <summary>
        /// Inserts Row in DB
        /// </summary>
        /// <param name="cmdString"></param>
        /// <param name="parameters"></param>
        public Int32 InsertUpdateRow(string cmdString, Dictionary<string, object> parameters, string outParameter)
        {
            int result = -1;
            SqlParameter IDParameter = null;

            using (SqlConnection conn = GetConnection())
            {
                 using (SqlCommand comm = new SqlCommand())
                 {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    foreach(string parameter in parameters.Keys)
                    {
                        if (parameters[parameter] != null)
                        {
                            if (parameters[parameter].GetType().Name == "Byte[]")
                            {
                                SqlParameter param = comm.Parameters.Add(parameter, SqlDbType.VarBinary);
                                param.Value = parameters[parameter];
                            }
                            else
                            {
                                comm.Parameters.AddWithValue(parameter, parameters[parameter]);
                            }
                        }
                        else
                        {
                            comm.Parameters.AddWithValue(parameter, DBNull.Value);
                        }
                    }

                    if (outParameter != null)
                    {
                        IDParameter = new SqlParameter(outParameter, SqlDbType.Int);
                        IDParameter.Direction = ParameterDirection.Output;
                        comm.Parameters.Add(IDParameter);
                    }

                    try
                    {
                        conn.Open();

                        if (outParameter != null)
                        {
                            object resultObj = comm.ExecuteScalar();

                            if (resultObj != null)
                                result = (int)int.Parse(resultObj.ToString());
                        }
                        else
                            comm.ExecuteNonQuery();
                    }
                    catch(SqlException ex)
                    {
                        throw ex;
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Inserts Row in DB
        /// </summary>
        /// <param name="cmdString"></param>
        /// <param name="parameters"></param>
        public Int32 InitRecord(string cmdString, Dictionary<string, object> parameters, Func<SqlDataReader, BaseEntity, BaseEntity> fillRecordFunc, BaseEntity recordToFill)
        {
            int result = -1;

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    foreach (string parameter in parameters.Keys)
                    {
                        if (parameters[parameter] != null)
                        {
                            if (parameters[parameter].GetType().Name == "Byte[]")
                            {
                                SqlParameter param = comm.Parameters.Add(parameter, SqlDbType.VarBinary);
                                param.Value = parameters[parameter];
                            }
                            else
                            {
                                comm.Parameters.AddWithValue(parameter, parameters[parameter]);
                            }
                        }
                        else
                        {
                            comm.Parameters.AddWithValue(parameter, DBNull.Value);
                        }
                    }

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                                recordToFill = fillRecordFunc(reader, recordToFill);
                        }

                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    return result;
                }
            }
        }



        /// <summary>
        /// Inserts Row in DB
        /// </summary>
        /// <param name="cmdString"></param>
        /// <param name="parameters"></param>
        public Int32 GetRecordList(string cmdString, Dictionary<string, object> parameters, Func<SqlDataReader, BaseEntity, BaseEntity> fillRecordFunc, Func<BaseEntity> credateNewEntity, out List<BaseEntity> recordList)
        {
            int result = -1;

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    if (parameters != null)
                    {
                        foreach (string parameter in parameters.Keys)
                        {
                            if (parameters[parameter] != null)
                            {
                                if (parameters[parameter].GetType().Name == "Byte[]")
                                {
                                    SqlParameter param = comm.Parameters.Add(parameter, SqlDbType.VarBinary);
                                    param.Value = parameters[parameter];
                                }
                                else
                                {
                                    comm.Parameters.AddWithValue(parameter, parameters[parameter]);
                                }
                            }
                            else
                            {
                                comm.Parameters.AddWithValue(parameter, DBNull.Value);
                            }
                        }
                    }

                    try
                    {
                        conn.Open();

                        recordList = new List<BaseEntity>();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            result = 0;
                            while (reader.Read())
                            {
                                BaseEntity baseEntity = credateNewEntity();
                                baseEntity = fillRecordFunc(reader, baseEntity);
                                recordList.Add(baseEntity);
                                result++;
                            }
                        }

                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    return result;
                }
            }
        }
    }
}

