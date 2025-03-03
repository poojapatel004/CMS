using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CMS.Models
{
    public class EmployeeModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Employee;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Dept")]
        public string Department { get; set; }

        [Required(ErrorMessage = "please enter salary")]
        [Range(5000, 50000, ErrorMessage = "Value should be between 5k to 50k")]
        public int Salary { get; set; }

        public List<EmployeeModel> getData()
        {
            List<EmployeeModel> lstEmp = new List<EmployeeModel>();

            SqlDataAdapter apt = new SqlDataAdapter("select * from tbl_emp", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstEmp.Add(new EmployeeModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Name = dr["Name"].ToString(),
                    Department = dr["Dept"].ToString(),
                    Salary = Convert.ToInt32(dr["Salary"].ToString())
                });
            }

            return lstEmp;
        }
        //Retrieve single record from a table 
        public EmployeeModel getData(string Id)
        {
            EmployeeModel emp = new EmployeeModel();
            SqlCommand cmd = new SqlCommand("select * from tbl_emp where id='" + Id +"'", con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    emp.Id = Convert.ToInt32(dr["Id"].ToString());
                    emp.Name = dr["Name"].ToString();
                    emp.Department = dr["Dept"].ToString();
                    emp.Salary = Convert.ToInt32(dr["Salary"].ToString());
                }
            }
            con.Close();
            return emp;
        }
        //Insert a record into a database table 
        public bool insert(EmployeeModel Emp)
        {

            SqlCommand cmd = new SqlCommand("insert into tbl_emp values(@Name, @Dept, @Salary)", con); 
            cmd.Parameters.AddWithValue("@Name", Emp.Name);
            cmd.Parameters.AddWithValue("@Dept", Emp.Department);
            cmd.Parameters.AddWithValue("@Salary", Emp.Salary);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }


            return false;
        }
        //Update a record into a database table 
        public bool update(EmployeeModel Emp)
        {

            SqlCommand cmd = new SqlCommand("update tbl_emp set Name=@Name,Dept = @Dept, Salary = @Salary where Id = @id", con); 
            cmd.Parameters.AddWithValue("@Name", Emp.Name);
            cmd.Parameters.AddWithValue("@Dept", Emp.Department);
            cmd.Parameters.AddWithValue("@Salary", Emp.Salary);
            cmd.Parameters.AddWithValue("@id", Emp.Id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }


            return false;
        }
        //delete a record from a database table 
        public bool delete(EmployeeModel Emp)
        {

            SqlCommand cmd = new SqlCommand("delete tbl_emp where Id = @id", con);
            cmd.Parameters.AddWithValue("@id", Emp.Id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
