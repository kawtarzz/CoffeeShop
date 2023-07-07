﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Title, 
                                        bv.Id AS BeanVarietyId, bv.[Name], bv.Region, bv.Notes FROM Coffee c JOIN BeanVariety bv ON c.BeanVarietyId = bv.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var coffeeBeverages = new List<Coffee>();
                        while (reader.Read())
                        {
                            var coffeeBev = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Region = reader.GetString(reader.GetOrdinal("Region"))
                                }
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                coffeeBev.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            coffeeBeverages.Add(coffeeBev);
                        }
                        return coffeeBeverages;
                    }
                }
            }
        }

        public Coffee Get(int id)
        {
            using ( var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT c.Id, c.Title, 
                                    bv.Id AS BeanVarietyId, 
                                    bv.[Name],bv.Region, bv.Notes 
                                    FROM Coffee c 
                                    JOIN BeanVariety bv 
                                    ON bv.Id = c.BeanVarietyId
                                    WHERE c.Id =@id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Coffee coffeeBev = null;
                        if (reader.Read())
                        {
                            coffeeBev = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Region = reader.GetString(reader.GetOrdinal("Region"))
                                }
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                coffeeBev.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                        }
                        return coffeeBev;
                    }
                }
            }
        }
        public void Add(Coffee coffeeBev)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee c (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@title, @beanVarietyId)
                        ";
                    cmd.Parameters.AddWithValue("@title", coffeeBev.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffeeBev.BeanVarietyId);
                    coffeeBev.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coffeeBev)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Coffee c
                        SET Title = @title,
                        BeanVarietyId = @beanVarietyId
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", coffeeBev.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffeeBev.BeanVarietyId);
                    cmd.Parameters.AddWithValue("@id", coffeeBev.Id);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coffee WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

