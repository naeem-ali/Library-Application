using Library_Application.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Application.Controllers
{
    public class UserController : Controller
    {

        string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

      

        public ActionResult Index(string CurrentSort)
        {
            List<BookModel> bList = new List<BookModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Books";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (reader.Read())
                    {
                        BookModel b = new BookModel();
                        b.ID = reader.GetInt32(reader.GetOrdinal("Id"));
                        b.Title = reader.GetString(reader.GetOrdinal("Title"));
                        b.Available = reader.GetInt32(reader.GetOrdinal("Available"));
                        b.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                        bList.Add(b);
                    }
                }
                //SqlDataAdapter sda = new SqlDataAdapter(query, con);
                //sda.Fill(dt);

                if (CurrentSort == "Desc" || CurrentSort == null)
                {
                    ViewBag.CurrentSort = "Asc";
                    //CurrentSort = "Asc";
                    bList = bList.OrderBy(t => t.Title).ToList();
                }
                else
                {
                    ViewBag.CurrentSort = "Desc";
                    //CurrentSort = "Asc";
                    bList = bList.OrderByDescending(t => t.Title).ToList();
                }

            }
            return View(bList);
        }

        //// POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserController/Rent/5
        public ActionResult Rent(int id, int available)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Insert Into Rentabook Values (@B_ID, @U_ID, @BookRetrun) SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@B_ID", id);
                cmd.Parameters.AddWithValue("@U_ID", Convert.ToInt32(TempData["User"]));
                cmd.Parameters.AddWithValue("@BookRetrun", false);


                int i = Convert.ToInt32(cmd.ExecuteNonQuery());
                TempData["Message"] = "You have successfully rented a book";

                 query = @"UPDATE [dbo].[Books] SET [Available]=@availability WHERE Id=@id";

                cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@availability", available-1);

                var result = cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index"); ;
        }

        // GET: User/Rent
        [HttpGet]
        public ActionResult Rented()
        {
          // RentBook  rentbook = new RentBook();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT Rentabook.[ID]
                  ,Books.Title as Title
                    ,[B_ID], Books.Available
                 FROM [dbo].[Rentabook] 
               Join Books ON Books.ID=Rentabook.B_ID
                 where U_ID=@userId and Rentabook.BookReturn=0";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@userId", Convert.ToInt32(TempData["User"]));
                sda.Fill(dt);
            }

           
                return View(dt);
            
        }

        public ActionResult BookReturn(int id, int bookId, int availability) {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Rentabook SET [BookReturn]=@BookReturn WHERE ID=@id SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@BookReturn", true);


                int i = Convert.ToInt32(cmd.ExecuteNonQuery());
                TempData["Message"] = "You have successfully rented a book";

                query = @"UPDATE [dbo].[Books] SET [Available]=@availability WHERE Id=@id";

                cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", bookId);
                cmd.Parameters.AddWithValue("@availability", availability + 1);


                var result = cmd.ExecuteNonQuery();

                TempData["BRMessage"] = "Your book is Returned";

                return RedirectToAction("Rented");

            }

        }
        // GET: UserController/RentedBook/
        //      using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            String query = @"SELECT Rentabook.[ID]
        //              ,[U_ID]
        //              ,[B_ID]
        //           ,Books.Title as Title
        //              ,[BookRetrun]
        //          FROM [dbo].[Rentabook] 
        //          Join Books ON Books.ID=Rentabook.B_ID
        //          where U_ID=@userId";
        //SqlCommand cmd = new SqlCommand(query, con);


        //// POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}



        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
