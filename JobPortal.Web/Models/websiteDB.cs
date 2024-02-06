
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class websiteDB
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(constr);
        }


        // 2. ********** Get All Item List **********
        public List<WebScrapModel> GetItemListAsif()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Asif' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListPrudhvi()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Prudhvi' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListRaghu()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Raghu' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListManisha()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Manisha' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListPrapurna()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Prapurna' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListKavya()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Kavyav' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListNirmal()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Nirmal' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListMadhava()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Madhava' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListLakshmi()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Lakshmi' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListPrasanna()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Prasanna' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListAhmed()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Ahmed' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListRamya()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Ramya' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListSandhya()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Sandhya' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListYagandhar()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Yagandhar' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListPoojitha()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();

            string query = "SELECT * FROM websitelist where CreatedBy='Poojitha' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListReshma()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Reshma' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListvinay()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='vinay' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListSurekha()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Surekha' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }

        public List<WebScrapModel> GetItemListrajurajmanthena123()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='prudhvi' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListlakshmi()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='lakshmi' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        public List<WebScrapModel> GetItemListahamed()
        {
            connection();
            List<WebScrapModel> iList = new List<WebScrapModel>();


            string query = "SELECT * FROM websitelist where CreatedBy='Ahmed' ORDER BY DateCreated DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            adapter.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                iList.Add(new WebScrapModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    Website = Convert.ToString(dr["Website"]),
                    //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                    //DateUpdated = (DateTime)dr["DateUpdated"],
                    //Category = (Category1)dr["Category"],
                    Name = Convert.ToString(dr["Name"]),
                    companylogoslink = Convert.ToString(dr["companylogoslink"]),
                    Logo = Convert.ToString(dr["Logo"])
                    //Email = Convert.ToString(dr["Email"])
                });
            }
            return iList;
        }
        //public List<WebScrapModel> GetItemListRaghu()
        //{
        //    connection();
        //    List<WebScrapModel> iList = new List<WebScrapModel>();


        //    string query = "SELECT * FROM websitelist where CreatedBy='Raghu'";
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();

        //    con.Open();
        //    adapter.Fill(dt);
        //    con.Close();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        iList.Add(new WebScrapModel
        //        {
        //            Id = Convert.ToInt32(dr["Id"]),
        //            CountryName = Convert.ToString(dr["CountryName"]),
        //            CompanyName = Convert.ToString(dr["CompanyName"]),
        //            Website = Convert.ToString(dr["Website"]),
        //            //CreatedBy = Convert.ToString(dr["CreatedBy"]),
        //            DateCreated = Convert.ToDateTime(dr["DateCreated"]),
        //            //DateUpdated = (DateTime)dr["DateUpdated"],
        //            //Category = (Category1)dr["Category"],
        //            Name = Convert.ToString(dr["Name"]),
        //            companylogoslink = Convert.ToString(dr["companylogoslink"]),
        //            Logo = Convert.ToString(dr["Logo"])
        //            //Email = Convert.ToString(dr["Email"])
        //        });
        //    }
        //    return iList;
        //}
        // 3. ********** Update Item Details **********
        public bool UpdateItem(WebScrapModel iList)
        {

            connection();
            string query = "UPDATE ItemList SET Category = '" + iList.Category + "', CountryName = " + iList.CountryName + ",CompanyName = " + iList.CountryName + ",companylogolink = " + iList.companylogoslink + ",Website = " + iList.Website + ",DateUpdated = " + DateTime.Now + ",UpdateBy= " + iList.Name + ",Email = " + iList.Email + "  WHERE ID = " + iList.Id;
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // 4. ********** Delete Item **********
        public bool DeleteItem(int id)
        {
            connection();
            string query = "DELETE FROM ItemList WHERE ID = " + id;
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}