using Employee.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Employee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Action to display the main menu
        public ActionResult Index()
        {
            var employees = GetEmployees();
            return View(employees);
        }

        // Action to add new employees
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(string name, string role, decimal basicPay, decimal allowances, decimal deductions)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (Name, Role, BasicPay, Allowances, Deductions) VALUES (@Name, @Role, @BasicPay, @Allowances, @Deductions)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@BasicPay", basicPay);
                    command.Parameters.AddWithValue("@Allowances", allowances);
                    command.Parameters.AddWithValue("@Deductions", deductions);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Action to display employee list
        public ActionResult ViewEmployees()
        {
            var employees = GetEmployees();
            return View(employees);
        }

        // Action to display individual employee salary
        public ActionResult CalculateSalary(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                employee.BasicPay = employee.BasicPay + employee.Allowances - employee.Deductions; // Calculate salary
                return View(employee);
            }
            return HttpNotFound();
        }

        // Action to calculate total payroll
        public ActionResult TotalPayroll()
        {
            decimal totalPayroll = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(BasicPay + Allowances - Deductions) AS TotalPayroll FROM Employees";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    totalPayroll = Convert.ToDecimal(command.ExecuteScalar());
                }
            }
            ViewBag.TotalPayroll = totalPayroll;
            return View();
        }

        public ActionResult ViewPayroll(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                return View(employee);
            }
            return HttpNotFound();
        }

        // Action to edit an employee's details
        public ActionResult Edit(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                return View(employee);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(int id, string name, string role, decimal basicPay, decimal allowances, decimal deductions)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employees SET Name = @Name, Role = @Role, BasicPay = @BasicPay, Allowances = @Allowances, Deductions = @Deductions WHERE EmployeeID = @EmployeeID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@BasicPay", basicPay);
                    command.Parameters.AddWithValue("@Allowances", allowances);
                    command.Parameters.AddWithValue("@Deductions", deductions);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("ViewEmployees");
        }

        // Action to confirm employee deletion
        public ActionResult Delete(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                return View(employee);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int? EmployeeID)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("ViewEmployees");
        }

        
        private List<BaseEmployee> GetEmployees()
        {
            var employees = new List<BaseEmployee>();
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new BaseEmployee
                            {
                                EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                Name = reader["Name"].ToString(),
                                BasicPay = Convert.ToDecimal(reader["BasicPay"]),
                                Allowances = Convert.ToDecimal(reader["Allowances"]),
                                Deductions = Convert.ToDecimal(reader["Deductions"])
                            });
                        }
                    }
                }
            }
            return employees;
        }

        private BaseEmployee GetEmployeeById(int id)
        {
            BaseEmployee employee = null;
            using (var connection = new SqlConnection(connectionString))
            {
                
                string query = "GetEmployeeSP";
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new BaseEmployee
                            {
                                EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                Name = reader["Name"].ToString(),
                                BasicPay = Convert.ToDecimal(reader["BasicPay"]),
                                Allowances = Convert.ToDecimal(reader["Allowances"]),
                                Deductions = Convert.ToDecimal(reader["Deductions"])
                            };
                        }
                    }
                }
            }
            return employee;
        }
    }
}
