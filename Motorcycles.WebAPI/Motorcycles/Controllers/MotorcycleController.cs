using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using Motorcycles.Tables;

namespace Motorcycles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MotorcycleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AddMotorcycle")]
        public IActionResult AddMotorcycle([FromBody] Motorcycle motorcycle)
        {
            if (motorcycle == null)
            {
                return BadRequest("Motorcycle data is null.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            INSERT INTO ""Motorcycle"" (""Make"", ""Model"", ""Year"", ""IsDeleted"", ""CreatedByUserId"", ""UpdatedByUserId"", ""DateCreated"", ""DateUpdated"")
                            VALUES (@Make, @Model, @Year, @IsDeleted, @CreatedByUserId, @UpdatedByUserId, @DateCreated, @DateUpdated)";

                        command.Parameters.AddWithValue("Make", (object?)motorcycle.Make ?? DBNull.Value);
                        command.Parameters.AddWithValue("Model", (object?)motorcycle.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("Year", motorcycle.Year);
                        command.Parameters.AddWithValue("IsDeleted", motorcycle.IsDeleted);
                        command.Parameters.AddWithValue("CreatedByUserId", motorcycle.CreatedByUserId);
                        command.Parameters.AddWithValue("UpdatedByUserId", motorcycle.UpdatedByUserId);
                        command.Parameters.AddWithValue("DateCreated", motorcycle.DateCreated);
                        command.Parameters.AddWithValue("DateUpdated", motorcycle.DateUpdated);

                        command.ExecuteNonQuery();
                    }
                }

                return Ok("Motorcycle added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateMotorcycle")]
        public IActionResult UpdateMotorcycle([FromBody] Motorcycle motorcycle)
        {
            if (motorcycle == null || motorcycle.Id == 0)
            {
                return BadRequest("Invalid motorcycle data.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            UPDATE ""Motorcycle""
                            SET ""Make"" = @Make, ""Model"" = @Model, ""Year"" = @Year, ""IsDeleted"" = @IsDeleted,
                                ""CreatedByUserId"" = @CreatedByUserId, ""UpdatedByUserId"" = @UpdatedByUserId,
                                ""DateCreated"" = @DateCreated, ""DateUpdated"" = @DateUpdated
                            WHERE ""Id"" = @Id";

                        command.Parameters.AddWithValue("Make", (object?)motorcycle.Make ?? DBNull.Value);
                        command.Parameters.AddWithValue("Model", (object?)motorcycle.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("Year", motorcycle.Year);
                        command.Parameters.AddWithValue("IsDeleted", motorcycle.IsDeleted);
                        command.Parameters.AddWithValue("CreatedByUserId", motorcycle.CreatedByUserId);
                        command.Parameters.AddWithValue("UpdatedByUserId", motorcycle.UpdatedByUserId);
                        command.Parameters.AddWithValue("DateCreated", motorcycle.DateCreated);
                        command.Parameters.AddWithValue("DateUpdated", motorcycle.DateUpdated);
                        command.Parameters.AddWithValue("Id", motorcycle.Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Motorcycle updated successfully.");
                        }
                        else
                        {
                            return NotFound("Motorcycle not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteMotorcycle")]
        public IActionResult DeleteMotorcycle(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid motorcycle ID.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"DELETE FROM ""Motorcycle"" WHERE ""Id"" = @Id";
                        command.Parameters.AddWithValue("Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok($"Motorcycle with Id = {id} deleted successfully.");
                        }
                        else
                        {
                            return NotFound($"Motorcycle with Id = {id} not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetMotorcycle")]
        public IActionResult GetMotorcycle(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid motorcycle ID.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"SELECT * FROM ""Motorcycle"" WHERE ""Id"" = @Id";
                        command.Parameters.AddWithValue("Id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var motorcycle = new Motorcycle
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Make = reader.GetString(reader.GetOrdinal("Make")),
                                    Model = reader.GetString(reader.GetOrdinal("Model")),
                                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    CreatedByUserId = reader.GetInt32(reader.GetOrdinal("CreatedByUserId")),
                                    UpdatedByUserId = reader.GetInt32(reader.GetOrdinal("UpdatedByUserId")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"))
                                };
                                return Ok(motorcycle);
                            }
                            else
                            {
                                return NotFound($"Motorcycle with Id = {id} not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
