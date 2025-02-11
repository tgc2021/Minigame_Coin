using Minigame_coin.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Minigame_coin.Controllers
{
    [EnableCorsAttribute(origins: "*", headers: "*", methods: "*")]
    public class coinsGameLogController : ApiController
    {
        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type"); // Specify allowed headers here
            return response;
        }


        [Route("~/api/CoinsGameLog")]
        [AllowAnonymous]
        [HttpPost]
        [HttpOptions]
        public IHttpActionResult InsertGameLog([FromBody] CoinsGameLog gameLog)
        {
            if (gameLog == null)
            {
                return BadRequest("Invalid data.");
            }

            string query = @"
    INSERT INTO tbl_coins_game_log 
    (org_id, id_game, xps, time, id_user, status, updated_datetime) 
    VALUES (@OrgId, @IdGame, @Xps, @Time, @IdUser, @Status, @UpdatedDateTime);";

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                return InternalServerError(new Exception("Database connection string is not configured."));
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@OrgId", gameLog.org_id);
                    command.Parameters.AddWithValue("@IdGame", gameLog.id_game);
                    command.Parameters.AddWithValue("@Xps", gameLog.Xps);
                    command.Parameters.AddWithValue("@Time", gameLog.Time);
                    command.Parameters.AddWithValue("@IdUser", gameLog.id_user);
                    command.Parameters.AddWithValue("@Status", gameLog.Status);
                    command.Parameters.AddWithValue("@UpdatedDateTime", DateTime.Now); // Insert current timestamp

                    try
                    {
                        connection.Open(); // Open the connection
                        command.ExecuteNonQuery(); // Execute the query
                        return Ok("Data inserted successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception if needed
                        return InternalServerError(new Exception("An error occurred while inserting data.", ex));
                    }
                }
                // Connection is closed and disposed of here
            }
        }




    }
}
