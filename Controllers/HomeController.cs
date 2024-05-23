using GoodFoodProjectMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using System;



namespace GoodFoodProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=goodfood;Integrated Security=True;TrustServerCertificate=True;";

        [HttpPost]
        public IActionResult AddRecipe(Recipes recipe)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Recipes (Name, Description) VALUES (@Name, @Description)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", recipe.Name);
                        command.Parameters.AddWithValue("@Description", recipe.Description);

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Recipes");
            }
            return View(recipe);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Users (First_Name, Last_Name, Email, Password, Created_At) VALUES (@First_Name, @Last_Name, @Email, @Password, GETDATE())";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@First_Name", user.First_Name);
                            command.Parameters.AddWithValue("@Last_Name", user.Last_Name);
                            command.Parameters.AddWithValue("@Email", user.Email);
                            command.Parameters.AddWithValue("@Password", user.Password);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Pomyślnie dodano użytkownika, przekierowanie do strony głównej
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                // W przypadku braku zmian w bazie danych, możemy ustalić, co dalej
                                ViewBag.ErrorMessage = "Nie udało się dodać użytkownika.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Obsługa błędu - przekierowanie do widoku z komunikatem
                    ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania użytkownika.";
                    return View(user);
                }
            }

            return View(user);
        }
        

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Recipes()
        {
            List<Recipes> recipes = new List<Recipes>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Name, Description FROM Recipes";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recipes.Add(new Recipes
                            {
                                Name = reader.GetString(0),
                                Description = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return View(recipes);
        }

       
        public IActionResult AddRecipe()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }
   

        public IActionResult Contact()
        {
            return View();
        }
       private List<User> GetUsersFromDatabase()
        {
   
            List<User> users = new List<User>();
    
   
            string query = "SELECT First_Name, Last_Name, Email, Password, Created_At FROM Users";

    
            using (SqlConnection connection = new SqlConnection(ConnectionString))
    
            {
       
                connection.Open();

        
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User()
                            {
                                First_Name = reader.GetString(0),
                                Last_Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                Created_At = reader.GetDateTime(4),
                            };


                            users.Add(user);
                        }
                    }
        }
    }
    return users;
}

        public IActionResult DataBase()
        {
            var users = GetUsersFromDatabase();
            
            return View(users);
        }
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Przekierowanie do akcji wyświetlającej listę użytkowników
                        return RedirectToAction("DataBase");
                    }
                }
            }
            return NotFound();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult EditUser()
        {
            return View("~/Views/Admin/EditUser.cshtml");
        }

    }


}

