using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using Ex2.Models;


namespace Ex2.Models.DAL
{
    public class SeriesDBServices
    {
      
        public SeriesDBServices()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //--------------------------------------------------------------------------------------------------
        // This method inserts a car to the cars table 
        //--------------------------------------------------------------------------------------------------
        public int Insert(Series series)
        {
            SqlConnection con;
            //SqlCommand cmd;
            int numEffected;
            
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception exe)
            {
               throw (exe);
            }

            //String cStr = BuildInsertCommand(obj);
            //cmd = CreateCommand(cStr, con);

            String seriesQuery = "INSERT INTO Series_2021 (id,first_air_date,name,origin_country,original_language,overview,popularity,poster_path, preferencesCount) VALUES (@id,@first_air_date,@name,@origin_country,@original_language,@overview,@popularity,@poster_path, @preferencesCount)";
            
            using (SqlCommand command = new SqlCommand(seriesQuery, con))
            {
                command.Parameters.AddWithValue("@id", series.Id);
                command.Parameters.AddWithValue("@first_air_date", series.First_air_date);
                command.Parameters.AddWithValue("@name", series.Name);
                command.Parameters.AddWithValue("@origin_country", series.Origin_country);
                command.Parameters.AddWithValue("@original_language", series.Original_language);
                command.Parameters.AddWithValue("@overview", series.Overview);
                command.Parameters.AddWithValue("@popularity", series.Popularity);
                command.Parameters.AddWithValue("@poster_path", series.Poster_path);
                command.Parameters.AddWithValue("@preferencesCount", series.PreferencesCount);
                try
                {
                    numEffected = command.ExecuteNonQuery(); // execute the command
                    return numEffected;
                }
                catch (SqlException exe)
                {
                    if (!(exe.Number == 2627))  //if the row doesnt exists so what is your problem? (throw it)
                        throw (exe);//so throw what was the problem
                    //else
                    //{
                    //    int preferencesCount = UpdatePreferencesCount(series.Id);
                    //}
                    return -1; //the row exists allready
                }
                finally
                {
                    if (con != null)
                    {
                        // close the db connection
                        con.Close();
                    }
                }
            }
        }
  
        //public int UpdatePreferencesCount(int id)
        //{
        //    SqlConnection con;
        //    SqlCommand cmd;
        //    {
        //        try
        //        {
        //            con = connect("DBConnectionString"); // create the connection
        //        }
        //        catch (Exception ex)
        //        {
        //            // write to log
        //            throw (ex);
        //        }
        //        String cStr = BuildUpdateCommand(id);      // helper method to build the insert string

        //        cmd = CreateCommand(cStr, con);             // create the command

        //        try
        //        {
        //            int rowEffected = cmd.ExecuteNonQuery(); // execute the command
        //            return rowEffected;
        //        }
        //        catch (Exception ex)
        //        {
        //            // write to log
        //            throw (ex);
        //        }

        //        finally
        //        {
        //            if (con != null)
        //            {
        //                // close the db connection
        //                con.Close();
        //            }
        //        }
        //    }
        //}

        ////--------------------------------------------------------------------
        //// Build the Update command String
        ////--------------------------------------------------------------------
        //private String BuildUpdateCommand(int id)
        //{
        //    String command;

        //    StringBuilder sb = new StringBuilder();
        //    // use a string builder to create the dynamic string
        //    sb.AppendFormat(" SET [preferencesCount]=preferencesCount+1 ");
        //    String prefix = "UPDATE Series_2021";
        //    String end = "WHERE id= " + id;
        //    command = prefix + sb.ToString() + end;
        //    return command;
        //}
        //private int GetPreferencesCount(int seriesId)
        //{
        //    SqlConnection con = null;
        //    int preferencesCount = 0;
        //    try
        //    {
        //        con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

        //        String selectSTR = "SELECT preferencesCount FROM Series_2021 WHERE id=" + seriesId;
        //        SqlCommand cmd = new SqlCommand(selectSTR, con);

        //        // get a reader
        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

        //        if (dr.Read())
        //        {   // Read till the end of the data into a row
        //            preferencesCount = Convert.ToInt32(dr["preferencesCount"]);
        //        }
        //        return preferencesCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }
        //    }
        //}
        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }


        //GetSeries
        public List<Series> GetSeries()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
                List<Series> seriesList = new List<Series>();
                String selectSTR = "SELECT * FROM Series_2021";//WHERE isAdmin=false
                cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {
                    Series u = new Series();
                    u.Id = Convert.ToInt32(dr["id"]);
                    u.First_air_date = (string)dr["first_air_date"];
                    u.Name = (string)dr["name"];
                    u.Origin_country = (string)dr["origin_country"];
                    u.Original_language = (string)dr["original_language"];
                    u.Overview = (string)dr["overview"];
                    u.Popularity = Convert.ToInt32(dr["popularity"]);
                    u.Poster_path = (string)(dr["poster_path"]);
                    // u.PreferencesCount = Convert.ToInt32(dr["preferencesCount"]);
                    u.PreferencesCount = dr["preferencesCount"] == DBNull.Value ? 1 : Convert.ToInt32(dr["preferencesCount"]);

                    seriesList.Add(u);
                }
                return seriesList;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

    }
}