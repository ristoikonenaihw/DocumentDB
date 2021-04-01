using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CosmosGettingStartedTutorial
{
    public static class Extensions
    {

        public static IEnumerable<T> Select<T>(
            this SqlDataReader reader, Func<SqlDataReader, T> projection)
        {

            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }

    public class Data
    {

        const string cs = @"Data Source=dev-risto01;Initial Catalog=meteor;Integrated Security=True";

        public class Content
        {
            public string data { get; set; }
            public int itemId { get; set; }
            public int versionId { get; set; }

            public string contenttypename { get; set; }

        }


        public class Relation
        {
            public string rType { get; set; }
            public int? fromItemId { get; set; }
            public int? fromVersionId { get; set; }
            public int? toItemId { get; set; }
            public int? toVersionId { get; set; }


        }

        public static async Task<IEnumerable<Content>> GetContents()
        {
            //SqlConnection conn, 
            //    SqlTransaction trans, 
            //    CommandType commandType, 
            //    string commandText, 
            //    IEnumerable< SqlParameter > parameters, 
            //    CancellationToken token, int commandTimout)
            
            List<Content> l = new List<Content>();

            CancellationToken token = new CancellationToken(false);

            const string query = @"SELECT * FROM published_content";

            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;

                    await conn.OpenAsync();

                    using (var r = await cmd.ExecuteReaderAsync(token))
                    {

                        while (await r.ReadAsync())
                        {

                            l.Add(ContentBuilder(r));
                        }


                        //var x = r.Select(rx => ContentBuilder(rx)).ToList();

                        //var dc = new System.Data.Linq.DataContext(conn);
                        //return dc.Translate<T>(sr).ToList();
                    }
                }

                return l;

                Content ContentBuilder(SqlDataReader reader)
                {

                    return new Content
                    {

                        itemId = (int)reader["itemId"],
                        data = (string)reader["data"],
                        versionId = (int)reader["versionId"],
                        contenttypename = (string)reader["contenttypename"],
                        //Year = int.Parse(reader["Year"].ToString()), // is DBNull ? null : reader["Make"].ToString(),
                        //Price = float.Parse(reader["Price"].ToString()), is DBNull ? null : reader["Model"].ToString(),
                    };
                }
            }
        }
        //cast object of type 'System.DBNull' to type 'System.Int32'

        public static async Task<IEnumerable<Relation>> GetRelations()
        {

            List<Relation> l = new List<Relation>();

            CancellationToken token = new CancellationToken(false);

            const string query = @"SELECT * FROM published_relations";

            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;

                    await conn.OpenAsync();

                    using (var r = await cmd.ExecuteReaderAsync(token))
                    {

                        while (await r.ReadAsync())
                        {

                            l.Add(RelationBuilder(r));
                        }


                        //var x = r.Select(rx => ContentBuilder(rx)).ToList();

                        //var dc = new System.Data.Linq.DataContext(conn);
                        //return dc.Translate<T>(sr).ToList();
                    }
                }

                return l;

                Relation RelationBuilder(SqlDataReader reader)
                {

                    return new Relation
                    {
                        //(accountNumber == DBNull.Value) ? string.Empty : accountNumber.ToString()
                        rType = (string)reader["rType"],
                        fromItemId = (reader["fromItemId"] == DBNull.Value) ? null : (int)reader["fromItemId"],
                        fromVersionId = (reader["fromVersionId"] == DBNull.Value) ? null : (int)reader["fromVersionId"],
                        toItemId = (reader["toItemId"] == DBNull.Value) ? null : (int)reader["toItemId"],
                        toVersionId = (reader["toVersionId"] == DBNull.Value) ? null : (int)reader["toVersionId"],

                    };
                }
            }
        }


        public async Task<IEnumerable<Content>> GetContent()
        {
            List<Content> l = null;

            CancellationToken token = new CancellationToken(false);

            const string query = @"SELECT * FROM published_content";

            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = new SqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;

                    await conn.OpenAsync();

                    //CommandBehavior.SingleResult))

                    using (var r = await cmd.ExecuteReaderAsync(token))
                    {
                        var x = r.Select(rx => ContentBuilder(rx)).ToList();
                        //while (await r.ReadAsync())
                        //{

                        //    l.Add(ContentBuilder(r));
                        //}
                        //l.Select(r => ContentBuilder(r)).ToList();
                    }
                    return l;
                }
            }


            Content ContentBuilder(SqlDataReader reader)
            {

                return new Content
                {

                    itemId = (int)reader["itemId"],
                    data = (string)reader["data"],
                    versionId = (int)reader["versionId"]
                    //Year = int.Parse(reader["Year"].ToString()), // is DBNull ? null : reader["Make"].ToString(),
                    //Price = float.Parse(reader["Price"].ToString()), is DBNull ? null : reader["Model"].ToString(),
                };
            }

            //using (var conn = new SqlConnection(cs)) //_connectionString.Value))
            //{
            //    try
            //    {
            //        var result = await conn.QueryAsync<UserRoleModel>(query, parameters);

            //        if (result != null)
            //        {
            //            l = result.ToList<UserRoleModel>();
            //            var l2 = l.Where(ic => !string.IsNullOrWhiteSpace(ic.Role));

            //            foreach (var icm in l2)
            //            {
            //                cii.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Role, icm.Role));
            //            }
            //        }
            //    }

            //    catch (Exception ex)
            //    {
            //        Utils.WriteLn(ex.Message);

            //    }
            //}
            //return cii;

        }

    }
}
