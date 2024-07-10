
using System.Data.SqlClient;
using System.Data;
using Zencareservice.Data;
using System.Xml.Linq;
using Zencareservice.Models;
using Zencareservice.Controllers;
using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.CodeDom;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection.Emit;

using System.Text;
using System.Security.Cryptography;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Zencareservice.Repository
{
    public class DataAccess
    {
        SqlDataAccess Obj_SqlDataAccess = new SqlDataAccess();

		public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPassword = Convert.ToBase64String(hashedBytes);
                return hashedPassword;
            }
        }

		
	   public DataSet SaveRegister(Signup Obj)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "SaveRegister_SP";

                string hashedPassword = HashPassword(Obj.Password);

                // Convert hashed password to byte array
                byte[] hashedBytes = Encoding.UTF8.GetBytes(hashedPassword);

                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@Firstname", SqlDbType.NVarChar);
                param[0].Value = Obj.Firstname;
                param[1] = new SqlParameter("@Lastname", SqlDbType.NVarChar);
                param[1].Value = Obj.Lastname;
                param[2] = new SqlParameter("@Email", SqlDbType.NVarChar);
                param[2].Value = Obj.Email;        
                param[3] = new SqlParameter("@Password", SqlDbType.VarBinary);
                param[3].Value = hashedBytes;
                param[4] = new SqlParameter("@Confirmpassword", SqlDbType.VarBinary);
                param[4].Value = hashedBytes;
                param[5] = new SqlParameter("@Dob", SqlDbType.DateTime);
                param[5].Value = Obj.Dob;
                param[6] = new SqlParameter("@Phone", SqlDbType.VarChar);
                param[6].Value = Obj.Phonenumber;
                param[7] = new SqlParameter("@Status", SqlDbType.VarChar);
                param[7].Value = Obj.Status;
                param[8] = new SqlParameter("@Role", SqlDbType.VarChar);
                param[8].Value = Obj.RoleId;
                param[9] = new SqlParameter("@agreeterm", SqlDbType.VarChar);
                param[9].Value = Convert.ToInt32(Obj.agreeterm);
                param[10] = new SqlParameter("@Age", SqlDbType.Int);
                param[10].Value = Convert.ToInt32(Obj.Age);
                param[11] = new SqlParameter("@RCategory", SqlDbType.VarChar);
                param[11].Value = Obj.RCategory;
                param[12] = new SqlParameter("@Username", SqlDbType.NVarChar);
                param[12].Value = Obj.Username;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;

            }


        }

        //public DataSet SaveDRegister(Signup Obj)
        //{

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        string StrSPName = "SaveRegister_SP";

        //        string hashedPassword = HashPassword(Obj.Password);

        //        // Convert hashed password to byte array
        //        byte[] hashedBytes = Encoding.UTF8.GetBytes(hashedPassword);

        //        SqlParameter[] param = new SqlParameter[12];
        //        param[0] = new SqlParameter("@Firstname", SqlDbType.NVarChar);
        //        param[0].Value = Obj.Firstname;
        //        param[1] = new SqlParameter("@Lastname", SqlDbType.NVarChar);
        //        param[1].Value = Obj.Lastname;
        //        param[2] = new SqlParameter("@Email", SqlDbType.NVarChar);
        //        param[2].Value = Obj.Email;
        //        param[3] = new SqlParameter("@Password", SqlDbType.VarBinary);
        //        param[3].Value = hashedBytes;
        //        param[4] = new SqlParameter("@Confirmpassword", SqlDbType.VarBinary);
        //        param[4].Value = hashedBytes;
        //        param[5] = new SqlParameter("@Dob", SqlDbType.DateTime);
        //        param[5].Value = Obj.Dob;
        //        param[6] = new SqlParameter("@Phone", SqlDbType.VarChar);
        //        param[6].Value = Obj.Phonenumber;
        //        param[7] = new SqlParameter("@Status", SqlDbType.VarChar);
        //        param[7].Value = Obj.Status;
        //        param[8] = new SqlParameter("@Role", SqlDbType.VarChar);
        //        param[8].Value = Obj.RoleId;
        //        param[9] = new SqlParameter("@agreeterm", SqlDbType.VarChar);
        //        param[9].Value = Convert.ToInt32(Obj.agreeterm);
        //        param[10] = new SqlParameter("@Age", SqlDbType.Int);
        //        param[10].Value = Convert.ToInt32(Obj.Age);
        //        param[11] = new SqlParameter("@RCategory", SqlDbType.VarChar);
        //        param[11].Value = Obj.RCategory;


        //        ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

        //        return ds;
        //    }

        //    catch (SqlException ex)
        //    {
        //        throw ex;

        //    }


        //}


        public DataSet FetchData()
        {
            try
            {
                DataSet ds = new DataSet();
                
                 
                
                string StrSPName = "GetAllPatientDetails";

                //string StrSPName = "GetAllDoctorDetails";

              


                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            
            }

        }


        public DataSet SaveGLogin(string email)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "SaveGLogin_SP";

				SqlParameter[] param = new SqlParameter[1];

				param[0] = new SqlParameter("@Email", SqlDbType.Int);
                param[0].Value =email;

                return ds;
			}
            catch(Exception ex)
            {
                throw ex;
            }
        }
		public DataSet GetDashboardvalues(Dashboard Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "DashboardValues_SP";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = Obj.UsrId;
                param[1] = new SqlParameter("@Role", SqlDbType.NVarChar);
                param[1].Value = Obj.Role;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName,param);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CheckAppointmentList(Aptcrt Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllAppointmentdetails";

                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@AptBookingdate", SqlDbType.Date);
                param[0].Value = Obj.AptBookingDate;
                param[1] = new SqlParameter("@AptBokingtime", SqlDbType.Time);
                param[1].Value = Obj.AptBookingTime; ;
                param[2] = new SqlParameter("@PFname", SqlDbType.NVarChar);
                param[2].Value = Obj.PatientFirstName;
                param[3] = new SqlParameter("@DFname", SqlDbType.NVarChar);
                param[3].Value = Obj.DoctorFirstName;
                



                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds ;
            }

            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet ReCheckAppointmentList(Appts Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllAppointmentdetails";

                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@AptBookingdate", SqlDbType.Date);
                param[0].Value = Obj.AptBookingDate;
                param[1] = new SqlParameter("@AptBokingtime", SqlDbType.Time);
                param[1].Value = Obj.AptBookingTime; ;
                param[2] = new SqlParameter("@PFname", SqlDbType.NVarChar);
                param[2].Value = Obj.PatientFirstName;
                param[3] = new SqlParameter("@DFname", SqlDbType.NVarChar);
                param[3].Value = Obj.DoctorFirstName;




                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet SaveAppointment(Aptcrt Obj)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "AppointmentBooking_SP";

                SqlParameter[] param = new SqlParameter[13];

                param[0] = new SqlParameter("@PatientFirstname", SqlDbType.NVarChar);
                param[0].Value = Obj.PatientFirstName;
                param[1] = new SqlParameter("@PatientLastname", SqlDbType.NVarChar);
                param[1].Value = Obj.PatientLastName;
                param[2] = new SqlParameter("@PatientEmail", SqlDbType.NVarChar);
                param[2].Value = Obj.PatientEmail;
                param[3] = new SqlParameter("@DoctorFirstname", SqlDbType.NVarChar);
                param[3].Value = Obj.DoctorFirstName;
                param[4] = new SqlParameter("@AptCategory", SqlDbType.NVarChar);
                param[4].Value = Obj.AptCategory;
                param[5] = new SqlParameter("@Patgender", SqlDbType.NVarChar);
                param[5].Value = Obj.PatientGender;
                param[6] = new SqlParameter("@AptBookdate", SqlDbType.Date);
                param[6].Value = Obj.AptBookingDate;
                param[7] = new SqlParameter("@AptTime", SqlDbType.Time);
                param[7].Value = Obj.AptBookingTime;
                param[8] = new SqlParameter("@RCode", SqlDbType.VarChar);
                param[8].Value = Obj.RCode;
                param[9] = new SqlParameter("@ReasonType", SqlDbType.VarChar);
                param[9].Value = Obj.ReasonType;
                param[10] = new SqlParameter("@PatientContact", SqlDbType.VarChar);
                param[10].Value = Obj.Patientphoneno;
                param[11] = new SqlParameter("@Patage", SqlDbType.VarChar);
                param[11].Value = Obj.PatientAge;
                param[12] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[12].Value = Obj.UsrId;



                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;

            }


        }

        public DataSet UpdateAppointment(Appts Obj)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "UpdateAppointment_SP";

                SqlParameter[] param = new SqlParameter[10];


                param[1] = new SqlParameter("@AptBookdate", SqlDbType.Date);
                param[1].Value = Obj.AptBookingDate;
                param[2] = new SqlParameter("@AptTime", SqlDbType.Time);
                param[2].Value = Obj.AptBookingTime;
                param[3] = new SqlParameter("@RCode", SqlDbType.VarChar);
                param[3].Value = Obj.RCode;
                param[4] = new SqlParameter("@ReschduleAppointmentDate", SqlDbType.Date);
                param[4].Value = Obj.ReasonType;
                param[5] = new SqlParameter("@RescheduleAppointmentTime", SqlDbType.Time);
                param[5].Value = Obj.ReasonType;
                param[6] = new SqlParameter("@ReasonType", SqlDbType.VarChar);
                param[6].Value = Obj.ReasonType;
                

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;

            }


        }


        public DataSet CheckEmail(Signup Obj )
        {

            try
            {
                DataSet dse = new DataSet();
                string StrSPName = "GetAllEmaildetails";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@Email", SqlDbType.NVarChar);
                param[0].Value = Obj.Email;

                dse = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return dse;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
		public DataSet CheckLoginEmail(Login Obj)
		{

			try
			{
				DataSet dse = new DataSet();
				string StrSPName = "GetAllEmaildetails";

				SqlParameter[] param = new SqlParameter[1];

				param[0] = new SqlParameter("@Email", SqlDbType.NVarChar);
				param[0].Value = Obj.OTPEmail;

				dse = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

				return dse;
			}
			catch (SqlException ex)
			{
				throw ex;
			}
		}

		public DataSet GetProfile( int Id)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllPersonalDetails";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@Id", SqlDbType.Int);
                param[0].Value = Id;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }


        public DataSet GetEditAppointments(string UsrId, int Id)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetEditAppointments";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
                param[1] = new SqlParameter("@Id", SqlDbType.Int);
                param[1].Value = Id;




                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetEditPrescriptions(string UsrId, string decodedId)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetEditPrescriptions";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
                param[1] = new SqlParameter("@Id", SqlDbType.VarChar);
                param[1].Value = decodedId;




                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }

        public DataSet GetAppointments(string UsrId)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllAppoinments";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
               
                


                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }
        public DataSet GetAppointmentList(string UsrId, string Role)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAppointmentList";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
                param[1] = new SqlParameter("@Role", SqlDbType.VarChar);
                param[1].Value = Role;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }

        public DataSet SetDetails(string type)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetDetails";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                param[0].Value = type;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }


        public DataSet GetPrescriptionList(string UsrId, string Role)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetPrescriptionList";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
                param[1] = new SqlParameter("@Role", SqlDbType.VarChar);
                param[1].Value = Role;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }

        public DataSet SetSelfAppointment(string UsrId)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetSelfAppointment";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@UsrId", SqlDbType.VarChar);
                param[0].Value = UsrId;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SetPrescription(string type)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetPrescription";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                param[0].Value = type;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SaveItemPrescription(int slno, string prescription, int dosage, int noofdays,string aptcode)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "SaveItemPrescription_SP";

                SqlParameter[] param = new SqlParameter[5];



                param[0] = new SqlParameter("@Prscitem", SqlDbType.VarChar);
                param[0].Value = prescription;
                param[1] = new SqlParameter("@Prscdosage", SqlDbType.Int);
                param[1].Value = Convert.ToInt32(dosage);
                param[2] = new SqlParameter("@Prscdays", SqlDbType.Int);
                param[2].Value = Convert.ToInt32(noofdays);
                param[3] = new SqlParameter("@Slno", SqlDbType.Int);
                param[3].Value = slno;
                param[4] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                param[4].Value = aptcode;


                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }

      

        public DataSet SaveUpdateItemPrescription(List<Prescs> medications)
        {
            try
            {
                DataSet ds = new DataSet();
                string processSPName = "ProcessPrscData";
                string deleteSPName = "DeleteProcessPrscData";

                foreach (var medication in medications)
                {
                    
                    SqlParameter[] param = new SqlParameter[6];
                    param[0] = new SqlParameter("@Prscitem", SqlDbType.VarChar);
                    param[0].Value = medication.Prescription;
                    param[1] = new SqlParameter("@Prscdosage", SqlDbType.Int);
                    param[1].Value = medication.Dosage;
                    param[2] = new SqlParameter("@Prscdays", SqlDbType.Int);
                    param[2].Value = medication.NoOfDays;
                    param[3] = new SqlParameter("@Slno", SqlDbType.Int);
                    param[3].Value = medication.SlNo;
                    param[4] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                    param[4].Value = medication.AptCode;
                    param[5] = new SqlParameter("@Prscode", SqlDbType.VarChar);
                    param[5].Value = medication.PrsCode;

                    
                    DataSet tempDs = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(processSPName, param);
                   
                    ds.Merge(tempDs);
                }

                
                SqlParameter[] deleteParam = new SqlParameter[0];
             
                DataSet deleteDs = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(deleteSPName, deleteParam);
                ds.Merge(deleteDs);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataSet SaveHeadPrescription(Prescs Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "SaveHeadPrescription_SP";

                SqlParameter[] param = new SqlParameter[9];


                param[0] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                param[0].Value = Obj.AppointmentCode;
                param[1] = new SqlParameter("@PatFirstName", SqlDbType.VarChar);
                param[1].Value = Obj.PatientFirstName;
                param[2] = new SqlParameter("@PatLastName", SqlDbType.VarChar);
                param[2].Value = Obj.PatientLastName;
                param[3] = new SqlParameter("@PatPhoneno", SqlDbType.VarChar);
                param[3].Value = Obj.Patientphoneno;
                param[4] = new SqlParameter("@PatEmail", SqlDbType.VarChar);
                param[4].Value = Obj.PatientEmail;
                param[5] = new SqlParameter("@PatAge", SqlDbType.VarChar);
                param[5].Value = Obj.PatientAge;
                param[6] = new SqlParameter("@PatBloodgroup", SqlDbType.NVarChar);
                param[6].Value = Obj.PatBloodgroup;
                param[7] = new SqlParameter("@PatWeight", SqlDbType.VarChar);
                param[7].Value = Obj.PatWeight;
                param[8] = new SqlParameter("@Dfname", SqlDbType.VarChar);
                param[8].Value = Obj.DoctorFirstName;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch(SqlException ex)
            {
                throw ex;

            }
        }

        public DataSet UpdateHeadPrescription(Prescs Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "UpdateHeadPrescription_SP";

                SqlParameter[] param = new SqlParameter[10];


                param[0] = new SqlParameter("@AptCode", SqlDbType.VarChar);
                param[0].Value = Obj.AptCode;
                param[1] = new SqlParameter("@PatFirstName", SqlDbType.VarChar);
                param[1].Value = Obj.PatientFirstName;
                param[2] = new SqlParameter("@PatLastName", SqlDbType.VarChar);
                param[2].Value = Obj.PatientLastName;
                param[3] = new SqlParameter("@PatPhoneno", SqlDbType.VarChar);
                param[3].Value = Obj.Patientphoneno;
                param[4] = new SqlParameter("@PatEmail", SqlDbType.VarChar);
                param[4].Value = Obj.PatientEmail;
                param[5] = new SqlParameter("@PatAge", SqlDbType.VarChar);
                param[5].Value = Obj.PatientAge;
                param[6] = new SqlParameter("@PatBloodgroup", SqlDbType.NVarChar);
                param[6].Value = Obj.PatBloodgroup;
                param[7] = new SqlParameter("@PatWeight", SqlDbType.VarChar);
                param[7].Value = Obj.PatWeight;
                param[8] = new SqlParameter("@Dfname", SqlDbType.VarChar);
                param[8].Value = Obj.DoctorFirstName;
                param[9] = new SqlParameter("@Prscode", SqlDbType.VarChar);
                param[9].Value = Obj.PrsCode;


                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }

        public DataSet GetUserList(string Role)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllUserList";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@Role", SqlDbType.VarChar);
                param[0].Value = Role;


                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }
        public DataSet GetPrescriptions(string UsrId)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllPrescriptions";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[0].Value = UsrId;
              

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }

        public DataSet SetCity ( int stateId)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllCitites";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@StateId", SqlDbType.Int);
                param[0].Value = stateId;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet SetConfirmAppointment(Appts Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "ConfirmAppointment_SP";

                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@AptId", SqlDbType.Int);
                param[0].Value = Obj.AptId;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataSet GetStates()
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetAllStates";

                ds = Obj_SqlDataAccess.GetDataWithStoredprocedure(StrSPName);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }

		public DataSet SaveOTPLogin(string Username,  string Password)
		{

			try
			{
				DataSet ds = new DataSet();
				string StrSPName = "CheckOTPLogin_SP";

				SqlParameter[] param = new SqlParameter[2];

				param[0] = new SqlParameter("@Uname", SqlDbType.NVarChar);
				param[0].Value = Username;
				param[1] = new SqlParameter("@OTP", SqlDbType.NVarChar);
				param[1].Value = Password;


				ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

				return ds;
			}

			catch (SqlException ex)
			{
				throw ex;
			}
		}

		public DataSet SaveLogin(Login Obj)
        {
            string hashedPassword = HashPassword(Obj.Password);

            // Convert hashed password to byte array
            byte[] hashedBytes = Encoding.UTF8.GetBytes(hashedPassword);
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "CheckLogin_SP";

                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Uname", SqlDbType.NVarChar);
                param[0].Value = Obj.Username;
                param[1] = new SqlParameter("@Pass", SqlDbType.VarBinary);
                param[1].Value = hashedBytes; ;


                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet SetRescheduleAppointment(Appts Obj)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "RescheduleAppointment_SP";
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Rescheduledate", SqlDbType.Date);
                param[0].Value = Obj.RescheduleAppointmentDate;
                param[1] = new SqlParameter("@Rescheduletime", SqlDbType.Time);
                param[1].Value = Obj.RescheduleAppointmentTime;
                param[2] = new SqlParameter("@AptId", SqlDbType.Int);
                param[2].Value = Obj.AptId;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;

            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet UpdateProfile(Personaldetails Obj)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "UpdatePersonaldetails_SP";

                SqlParameter[] param = new SqlParameter[22];
                param[0] = new SqlParameter("@Gender", SqlDbType.NVarChar);
                param[0].Value = Obj.Gender;
                param[1] = new SqlParameter("@APhoneno", SqlDbType.NVarChar);
                param[1].Value = Obj.AltPhoneno;
                param[2] = new SqlParameter("@Addressline1", SqlDbType.NVarChar);
                param[2].Value = Obj.Address1;
                param[3] = new SqlParameter("@Addressline2", SqlDbType.NVarChar);
                param[3].Value = Obj.Address2;
                param[4] = new SqlParameter("@AltAddress", SqlDbType.NVarChar);
                param[4].Value = Obj.AltAddress;
                param[5] = new SqlParameter("@stateid", SqlDbType.NVarChar);
                param[5].Value = Obj.State;
                param[6] = new SqlParameter("@cityid", SqlDbType.NVarChar);
                param[6].Value = Obj.City;
                param[7] = new SqlParameter("@Country", SqlDbType.NVarChar);
                param[7].Value = Obj.Country;
                param[8] = new SqlParameter("@UniqueId", SqlDbType.NVarChar);
                param[8].Value = Obj.Uniqueid;
                param[9] = new SqlParameter("@UsrId", SqlDbType.Int);
                param[9].Value = Obj.UsrId;
                param[10] = new SqlParameter("@Zipcode", SqlDbType.NVarChar);
                param[10].Value = Obj.Zipcode;
                param[11] = new SqlParameter("@Email", SqlDbType.NVarChar);
                param[11].Value = Obj.Email;
                param[12] = new SqlParameter("@Role", SqlDbType.NVarChar);
                param[12].Value = Obj.Role;

               if(Obj.Role == "Doctor")
               {
                       param[13] = new SqlParameter("@Dqualify", SqlDbType.NVarChar);
                       param[13].Value = Obj.DQualification;
                       param[14] = new SqlParameter("@specialistid", SqlDbType.NVarChar);
                       param[14].Value = Obj.specialistid;
                       param[15] = new SqlParameter("@Dexp", SqlDbType.NVarChar);
                       param[15].Value = Obj.DExp;
                        param[16] = new SqlParameter("@Empqualify", SqlDbType.NVarChar);
                        param[16].Value = "0";
                        param[17] = new SqlParameter("@Emphistory", SqlDbType.NVarChar);
                        param[17].Value = "0";
                        param[18] = new SqlParameter("@Empexp", SqlDbType.Int);
                        param[18].Value = "0";
                        param[19] = new SqlParameter("@pathistory", SqlDbType.NVarChar);
                        param[19].Value = "0";
                        param[20] = new SqlParameter("@patdiagnosis", SqlDbType.NVarChar);
                        param[20].Value = "0";
                        param[21] = new SqlParameter("@other", SqlDbType.Int);
                        param[21].Value = "0";

                }                
               else if(Obj.Role == "Patient")
                {
                   param[13] = new SqlParameter("@pathistory", SqlDbType.NVarChar);
                   param[13].Value = Obj.PatientHistory;
                   param[14] = new SqlParameter("@patdiagnosis", SqlDbType.NVarChar);
                   param[14].Value = Obj.Diagnosisdetails;
                   param[15] = new SqlParameter("@other", SqlDbType.NVarChar);
                   param[15].Value = Obj.Otherdetails;
                    param[16] = new SqlParameter("@Empqualify", SqlDbType.NVarChar);
                    param[16].Value = "0";
                    param[17] = new SqlParameter("@Emphistory", SqlDbType.NVarChar);
                    param[17].Value = "0";
                    param[18] = new SqlParameter("@Empexp", SqlDbType.Int);
                    param[18].Value = "0";
                    param[19] = new SqlParameter("@Dqualify", SqlDbType.NVarChar);
                    param[19].Value = "0";
                    param[20] = new SqlParameter("@specialistid", SqlDbType.NVarChar);
                    param[20].Value = "0";
                    param[21] = new SqlParameter("@Dexp", SqlDbType.NVarChar);
                    param[21].Value = "0";

                }
               else
               {
                   param[13] = new SqlParameter("@Empqualify", SqlDbType.NVarChar);
                   param[13].Value = Obj.EmpQualification;
                   param[14] = new SqlParameter("@Emphistory", SqlDbType.NVarChar);
                   param[14].Value = Obj.Emphistory;
                   param[15] = new SqlParameter("@Empexp", SqlDbType.Int);
                   param[15].Value = Obj.EmpExp;
                    param[16] = new SqlParameter("@pathistory", SqlDbType.NVarChar);
                    param[16].Value = "0";
                    param[17] = new SqlParameter("@patdiagnosis", SqlDbType.NVarChar);
                    param[17].Value = "0";
                    param[18] = new SqlParameter("@other", SqlDbType.Int);
                    param[18].Value = "0";
                    param[19] = new SqlParameter("@Dqualify", SqlDbType.NVarChar);
                    param[19].Value = "0";
                    param[20] = new SqlParameter("@specialistid", SqlDbType.NVarChar);
                    param[20].Value = "0";
                    param[21] = new SqlParameter("@Dexp", SqlDbType.NVarChar);
                    param[21].Value = "0";

                }
                // param[22] = new SqlParameter("@Photo", SqlDbType.VarBinary);
                // param[22].Value = Obj.ImageData;
                // param[23] = new SqlParameter("@Proof", SqlDbType.VarBinary);
                // param[23].Value = Obj.ProfData;
            

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet ResetPassword(Signup Obj, String mail)
        {

            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "UpdatePassword_SP";

                string hashedPassword = HashPassword(Obj.RPassword);

                byte[] hashedBytes = Encoding.UTF8.GetBytes(hashedPassword);

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Email", SqlDbType.NVarChar);
                param[0].Value = mail;
                param[1] = new SqlParameter("@RPassword", SqlDbType.VarBinary);
                param[1].Value = hashedBytes;
                param[2] = new SqlParameter("@CRPassword", SqlDbType.VarBinary);
                param[2].Value = hashedBytes;

                ds = Obj_SqlDataAccess.GetDataWithParamStoredprocedure(StrSPName, param);

                return ds;
            }

            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet RolesList()
        {
            try
            {
                DataSet ds = new DataSet();
                string StrSPName = "GetRoles";

                ds = Obj_SqlDataAccess.GetDataWithStoredprocedure(StrSPName);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

    }
   
}
