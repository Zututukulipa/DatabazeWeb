using System;
using System.Collections.Specialized;
using System.Data;
using DatabaseAdapter.OracleLib.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DatabaseAdapter.OracleLib
{
    public class OracleDatabaseControls
    {
        public readonly string ConnectionString;

        public OracleDatabaseControls(string databaseConnectionString)
        {
            ConnectionString = databaseConnectionString;
        }
        
        public void InsertClassroom(string roomName, string roomCapacity)
        {
            var commandText = "INSERT INTO CLASSROOMS( NAME, CAPACITY) VALUES (:room_name,:room_capacity)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":room_name", OracleDbType.NVarchar2, roomName, ParameterDirection.Input);
                command.Parameters.Add(":room_capacity", OracleDbType.Int32, roomCapacity, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertComment(string userId, string postId, string content)
        {
            var commandText = "INSERT INTO COMMENTS(USER_ID, MESSAGE_ID, CONTENT) VALUES (:user_id,:post_id,:comment_content)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":user_id", OracleDbType.Int32, userId, ParameterDirection.Input);
                command.Parameters.Add(":post_id", OracleDbType.Int32, postId, ParameterDirection.Input);
                command.Parameters.Add(":comment_content", OracleDbType.NVarchar2, content, ParameterDirection.Input);
                command.Parameters.Add(":comment_content", OracleDbType.NVarchar2).Value = "dsa";
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertCourse(string fullCourseName, string shortcut, string courseDescription)
        {
            var commandText = "INSERT INTO COURSES(FULL_NAME, SHORT_NAME, DESCRIPTION) VALUES (:full_name,:shortcut,:description)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":full_name", OracleDbType.NVarchar2, fullCourseName, ParameterDirection.Input);
                command.Parameters.Add(":shortcut", OracleDbType.NVarchar2, shortcut, ParameterDirection.Input);
                command.Parameters.Add(":description", OracleDbType.NVarchar2, courseDescription, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertGrade(string studentId, string teacherId, string classId ,string gradeValue, string description)
        {
            var commandText = "INSERT INTO GRADES(STUDENT_ID, TEACHER_ID, COURSE_ID, VALUE, DESCRIPTION) VALUES (:student_id,:teacher_id,:course_id,:grade,:description)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":student_id", OracleDbType.Int32, studentId, ParameterDirection.Input);
                command.Parameters.Add(":teacher_id", OracleDbType.Int32, teacherId, ParameterDirection.Input);
                command.Parameters.Add(":course_id", OracleDbType.Int32, classId, ParameterDirection.Input);
                command.Parameters.Add(":grade", OracleDbType.Int32, gradeValue, ParameterDirection.Input);
                command.Parameters.Add(":description", OracleDbType.NVarchar2, description, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
//        public void InsertStudent(string studentId, string teacherId)
//        {
//            var commandText = "INSERT INTO STUDENTS(USER_ID, YEAR) VALUES (:user_id,:year_joined)";
//            using (OracleConnection connection = new OracleConnection(ConnectionString))
//            using (OracleCommand command = new OracleCommand(commandText, connection))
//            {
//                
//                command.Parameters.Add(":user_id", OracleDbType.Int32, studentId, ParameterDirection.Input);
//                command.Parameters.Add(":year_joined", OracleDbType.Int32, teacherId, ParameterDirection.Input);
//                command.Connection.Open();
//                command.ExecuteNonQuery();
//                command.Connection.Close();
//            }
//        }
//        
        public void InsertStudent(string studentId, string yearJoined)
        {
            var commandText = "INSERT INTO STUDENTS(USER_ID, YEAR) VALUES (:user_id,:year_joined)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":user_id", OracleDbType.Int32, studentId, ParameterDirection.Input);
                command.Parameters.Add(":year_joined", OracleDbType.Int32, yearJoined, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertGrade(string groupId, string userId, string content)
        {
            var commandText = "INSERT INTO GROUP_MESSAGES(GROUP_ID, USER_ID, CONTENT) VALUES (:group_id,:user_id,:message_content)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":group_id", OracleDbType.Int32, groupId, ParameterDirection.Input);
                command.Parameters.Add(":user_id", OracleDbType.Int32, userId, ParameterDirection.Input);
                command.Parameters.Add(":message_content", OracleDbType.NVarchar2, content, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertGroup(string leaderId, string courseId, string courseName, string maxCapacity)
        {
            var commandText = "INSERT INTO GROUPS(TEACHER_ID, COURSE_ID, NAME, MAX_CAPACITY) VALUES (:leader_id,:course_id,:course_name,:course_capacity)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                
                command.Parameters.Add(":leader_id", OracleDbType.Int32, leaderId, ParameterDirection.Input);
                command.Parameters.Add(":course_id", OracleDbType.Int32, courseId, ParameterDirection.Input);
                command.Parameters.Add(":course_name", OracleDbType.NVarchar2, courseName, ParameterDirection.Input);
                command.Parameters.Add(":course_capacity", OracleDbType.Int32, maxCapacity, ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        
        public void InsertStudentIntoGroup(string studentId, string groupId)
        {
            var commandText = "INSERT INTO PRIVATE_MESSAGES(FROM_USER, TO_USER, CONTENT) VALUES (:sender_id,:receiver_id,:message_content)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //ASSIGN PARAMETERS TO BE PASSED
                command.Parameters.Add("studentId",OracleDbType.Int32).Value = studentId;
                command.Parameters.Add("groupId",OracleDbType.Int32).Value = groupId;

                //CALL PROCEDURE
                command.ExecuteNonQuery();

            }
        }


        public int InsertUser(User user)
        {
            
            var commandText = "PKG_USER.NEW";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using(var command = new OracleCommand(commandText, connection) { CommandType = CommandType.StoredProcedure})
            {

                command.Parameters.Add(":v_id", OracleDbType.Int32, ParameterDirection.ReturnValue);
                command.Parameters.Add(":p_username", OracleDbType.NVarchar2, user.Username, ParameterDirection.Input);
                command.Parameters.Add(":p_password", OracleDbType.NVarchar2, user.Password, ParameterDirection.Input);
                command.Parameters.Add(":p_firstname", OracleDbType.NVarchar2, user.FirstName, ParameterDirection.Input);
                command.Parameters.Add(":p_middleName", OracleDbType.NVarchar2, user.MiddleName, ParameterDirection.Input);
                command.Parameters.Add(":p_lastname", OracleDbType.NVarchar2, user.LastName, ParameterDirection.Input);
                command.Parameters.Add(":p_email", OracleDbType.NVarchar2, user.Email, ParameterDirection.Input);
                command.Parameters.Add(":p_status", OracleDbType.Int32,  "1", ParameterDirection.Input);


                int returnedId;
                command.Connection.Open();
                using (var dr = command.ExecuteReader())
                {
                    returnedId = int.Parse(command.Parameters[":v_id"].Value.ToString());
                  
                    dr.Close();
                }
                command.Connection.Close();
                return returnedId;
            } 
        }

        public User SelectUser(int userId)
        {
            
            var commandText = "PKG_USER.GET_USER";
                        using (OracleConnection connection = new OracleConnection(ConnectionString))
                        using (OracleCommand command = new OracleCommand(commandText, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
      
                            // Bind the parameters
                            // p1 is the RETURN REF CURSOR bound to SELECT * FROM EMP;
                            OracleParameter p1 = 
                                command.Parameters.Add("v_cursor", OracleDbType.RefCursor);
                            p1.Direction = ParameterDirection.ReturnValue;

                            OracleParameter p2 = command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                                userId.ToString(), ParameterDirection.Input);
                            
                            connection.Open();
                            // Execute the command
                            command.ExecuteNonQuery();
                            // Construct an OracleDataReader from the REF CURSOR
                            OracleDataReader reader1 = ((OracleRefCursor)p1.Value).GetDataReader();
                            int userid = -1;
                            string username = "";
                            string fname = "";
                            string mname = "";
                            string lname = "";
                            string email = "";
                            int status = -1;
                            bool check = false;
                            // Prints "reader1.GetName(0) = EMPNO"
                            while (reader1.Read())
                            {
                                userid = reader1.GetInt32(0);
                                username = reader1.GetString(1);
                                fname = reader1.GetString(2);
                                
                                check = reader1.IsDBNull(3);
                                if (check)
                                    mname = "";
                                else
                                    mname = reader1.GetString(3);
                                
                                lname = reader1.GetString(4);
                                email = reader1.GetString(5);
                                
                                check = reader1.IsDBNull("STATUS_ID");
                                if (check)
                                    status = 0;
                                else
                                    status = reader1.GetInt32("STATUS_ID");



                            }
                            
                            connection.Close();
                            User usr;
                            if (userid != -1)
                            {
                                //TODO FINISH THE BIO
                                usr = new User(userid, username, fname, mname, lname, email, "Bio");
                                usr.StatusId = status;
                                return usr;
                            }

                            return null;
                        } 
        }

        public void DeleteUser(User user)
        {
            var commandText = "PKG_USER.UPDATE_STATUS";
                        using (OracleConnection connection = new OracleConnection(ConnectionString))
                        using (OracleCommand command = new OracleCommand(commandText, connection))
                        {
                            //UPDATE USERS SET STATUS_ID = p_status WHERE USER_ID = p_user_id;
                            command.CommandType = CommandType.StoredProcedure;
      
                            // Bind the parameters
                            // p1 is the RETURN REF CURSOR bound to SELECT * FROM EMP;

                            OracleParameter p2 = command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                                user.UserId.ToString(), ParameterDirection.Input);
                            OracleParameter p1 = 
                                command.Parameters.Add(":p_status", OracleDbType.Int32, "0", ParameterDirection.Input);
                            
                            connection.Open();
                            // Execute the command
                            command.ExecuteNonQuery();
 
                            // Construct an OracleDataReader from the REF CURSOR
                           
                            
                            connection.Close();
                           
                        } 
        }

        public User Login(string logonLogin, string logonPassword)
        {
            int userid = -1;

             var commandText = "PKG_USER.LOGIN";
                        using (OracleConnection connection = new OracleConnection(ConnectionString))
                        using (OracleCommand command = new OracleCommand(commandText, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            OracleParameter p1 = 
                                command.Parameters.Add("v_id", OracleDbType.Int32);
                            p1.Direction = ParameterDirection.ReturnValue;

                            OracleParameter p2 = command.Parameters.Add(":p_username", OracleDbType.NVarchar2,
                                logonLogin, ParameterDirection.Input);
                            
                            OracleParameter p3 = command.Parameters.Add(":p_password", OracleDbType.NVarchar2,
                                logonPassword, ParameterDirection.Input);
                            
                            connection.Open();
                            // Execute the command
                            command.ExecuteNonQuery();
 
                            // Construct an OracleDataReader from the REF CURSOR

                            try
                            {
                                userid = Int32.Parse(p1.Value.ToString());

                            }
                            catch (Exception e)
                            {
                                return null;
                            }
                                
                            
                            
                            connection.Close();
                                //TODO FINISH THE BIO
                            if (userid != -1) 
                                return SelectUser(userid);
                            return null;
                        } 
        }
    }
}