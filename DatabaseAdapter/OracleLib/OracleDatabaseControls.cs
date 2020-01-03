using System;
using System.Collections.Generic;
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

        public int InsertClassroom(Classrooms classroom)
        {
            //FUNCTION NEW(p_name T_NAME, p_capacity T_CAPACITY) RETURN T_ID
            var commandText = "PKG_CLASSROOM.NEW";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                OracleParameter rval =
                    command.Parameters.Add("T_ID", OracleDbType.Int32);
                rval.Direction = ParameterDirection.ReturnValue;
                OracleParameter p0 = command.Parameters.Add(":T_NAME", OracleDbType.NVarchar2,
                    classroom.Name, ParameterDirection.Input);
                OracleParameter p1 =
                    command.Parameters.Add(":p_capacity", OracleDbType.Int32, classroom.ClassroomId.ToString(),
                        ParameterDirection.Input);
                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();

                // Construct an OracleDataReader from the REF CURSOR

                int returnVal = int.Parse(rval.Value.ToString());
                connection.Close();
                return returnVal;
            }
        }

        public void InsertComment(string userId, string postId, string content)
        {
            var commandText =
                "INSERT INTO COMMENTS(USER_ID, MESSAGE_ID, CONTENT) VALUES (:user_id,:post_id,:comment_content)";
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

        public int InsertCourse(Courses course)
        {
            //NEW(p_full_name T_FULL_NAME, p_short_name T_SHORT_NAME, p_description T_DESCRIPTION) RETURN T_ID AS v_id T_ID;
            var commandText = "PKG_COURSE.NEW";
            int courseId = -1;
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                OracleParameter p1 = command.Parameters.Add(":v_id", OracleDbType.Int32, courseId,
                    ParameterDirection.ReturnValue);
                command.Parameters.Add(":p_full_name", OracleDbType.NVarchar2, course.FullName,
                    ParameterDirection.Input);
                command.Parameters.Add(":p_shortcut", OracleDbType.NVarchar2, course.ShortName,
                    ParameterDirection.Input);
                //cmd.Parameters.Add(new SqlParameter("@TaxRate", string.IsNullOrEmpty(taxRateTxt.Text) ? (object)DBNull.Value : taxRateTxt.Text));
                command.Parameters.Add(":p_description", OracleDbType.NVarchar2,
                    string.IsNullOrEmpty(course.Description) ? "Unavailable" : course.Description,
                    ParameterDirection.Input);
                command.Connection.Open();
                command.ExecuteNonQuery();
                courseId = int.Parse(p1.Value.ToString());

                command.Connection.Close();
            }

            return courseId;
        }

        public void InsertGrade(string studentId, string teacherId, string classId, string gradeValue,
            string description)
        {
            var commandText =
                "INSERT INTO GRADES(STUDENT_ID, TEACHER_ID, COURSE_ID, VALUE, DESCRIPTION) VALUES (:student_id,:teacher_id,:course_id,:grade,:description)";
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
            var commandText =
                "INSERT INTO GROUP_MESSAGES(GROUP_ID, USER_ID, CONTENT) VALUES (:group_id,:user_id,:message_content)";
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
            var commandText =
                "INSERT INTO GROUPS(TEACHER_ID, COURSE_ID, NAME, MAX_CAPACITY) VALUES (:leader_id,:course_id,:course_name,:course_capacity)";
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
            var commandText =
                "INSERT INTO PRIVATE_MESSAGES(FROM_USER, TO_USER, CONTENT) VALUES (:sender_id,:receiver_id,:message_content)";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //ASSIGN PARAMETERS TO BE PASSED
                command.Parameters.Add("studentId", OracleDbType.Int32).Value = studentId;
                command.Parameters.Add("groupId", OracleDbType.Int32).Value = groupId;

                //CALL PROCEDURE
                command.ExecuteNonQuery();
            }
        }


        public int InsertUser(User user)
        {
            var commandText = "PKG_USER.NEW";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure})
            {
                command.Parameters.Add(":v_id", OracleDbType.Int32, ParameterDirection.ReturnValue);
                command.Parameters.Add(":p_username", OracleDbType.NVarchar2, user.Username, ParameterDirection.Input);
                command.Parameters.Add(":p_password", OracleDbType.NVarchar2, user.Password, ParameterDirection.Input);
                command.Parameters.Add(":p_firstname", OracleDbType.NVarchar2, user.FirstName,
                    ParameterDirection.Input);
                command.Parameters.Add(":p_middleName", OracleDbType.NVarchar2, user.MiddleName,
                    ParameterDirection.Input);
                command.Parameters.Add(":p_lastname", OracleDbType.NVarchar2, user.LastName, ParameterDirection.Input);
                command.Parameters.Add(":p_email", OracleDbType.NVarchar2, user.Email, ParameterDirection.Input);
                command.Parameters.Add(":p_status", OracleDbType.Int32, "1", ParameterDirection.Input);


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
                OracleDataReader reader1 = ((OracleRefCursor) p1.Value).GetDataReader();
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

        public int SendMessage(PrivateMessages msg)
        {
            var commandText = "PKG_PMSG.NEW";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                //FUNCTION NEW(p_from_user PKG_USER.T_ID, p_to_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID AS
                command.CommandType = CommandType.StoredProcedure;

                OracleParameter rval =
                    command.Parameters.Add("v_id", OracleDbType.Int32);
                rval.Direction = ParameterDirection.ReturnValue;
                OracleParameter p2 = command.Parameters.Add(":p_from_user", OracleDbType.Int32,
                    msg.FromUser.UserId.ToString(), ParameterDirection.Input);
                OracleParameter p1 =
                    command.Parameters.Add(":p_to_user", OracleDbType.Int32, msg.ToUser.UserId.ToString(),
                        ParameterDirection.Input);
                OracleParameter p3 = command.Parameters.Add(":p_content", OracleDbType.NVarchar2, msg.Content,
                    ParameterDirection.Input);
                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();

                // Construct an OracleDataReader from the REF CURSOR

                var returnval = int.Parse(rval.Value.ToString());
                connection.Close();
                return returnval;
            }
        }

        public PrivateMessages GetMessage(int id)
        {
            var commandText = "PKG_PMSG.GET_BY_ID";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p1 =
                    command.Parameters.Add("p_id", OracleDbType.RefCursor);
                p1.Direction = ParameterDirection.ReturnValue;

                OracleParameter p2 = command.Parameters.Add(":p_id", OracleDbType.Int32,
                    id.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                OracleDataReader reader1 = ((OracleRefCursor) p1.Value).GetDataReader();
                PrivateMessages retMessage = new PrivateMessages();
                int messageId = -1;
                string content = "";
                int toUserId = -1;
                int fromUserId = -1;
                DateTime time = DateTime.Now;

                // Prints "reader1.GetName(0) = EMPNO"
                while (reader1.Read())
                {
                    messageId = reader1.GetInt32(0);
                    fromUserId = reader1.GetInt32(1);
                    toUserId = reader1.GetInt32(3);
                    content = reader1.GetString(5);
                    time = reader1.GetDateTime(6);
                }

                retMessage.Content = content;
                retMessage.Created = time;
                retMessage.ToUser = SelectUser(toUserId);
                retMessage.FromUser = SelectUser(fromUserId);
                retMessage.PmsgId = messageId;

                connection.Close();
                if (messageId != -1)
                {
                    //TODO FINISH THE BIO
                    return retMessage;
                }

                return null;
            }
        }

        public void RemoveMessage(PrivateMessages testMessage)
        {
            var commandText = "PKG_PMSG.REMOVE";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p2 = command.Parameters.Add(":p_id", OracleDbType.Int32,
                    testMessage.PmsgId.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void AssignAsStudent(in int i, int i1)
        {
            throw new NotImplementedException();
        }

        public void RemoveCourse(Courses course)
        {
            // PROCEDURE REMOVE(p_group_id T_ID) AS
            var commandText = "PKG_GROUP.REMOVE";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                    course.CourseId.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void UpdateCourseDescription(Courses course, string newUpdatedDescription)
        {
            //PROCEDURE UPDATE_DESCRIPTION(p_course_id T_ID, p_description T_DESCRIPTION)
            var commandText = "PKG_GROUP.UPDATE_DESCRIPTION";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                    course.CourseId.ToString(), ParameterDirection.Input);
                OracleParameter p1 = command.Parameters.Add(":p_description", OracleDbType.NVarchar2,
                    newUpdatedDescription, ParameterDirection.Input);


                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void UpdateCourseFullName(Courses course, string newUpdatedFullname)
        {
            // PROCEDURE UPDATE_FULL_NAME(p_course_id T_ID, p_full_name T_FULL_NAME) 
            var commandText = "PKG_GROUP.UPDATE_FULL_NAME";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                    course.CourseId.ToString(), ParameterDirection.Input);
                OracleParameter p1 = command.Parameters.Add(":p_full_name", OracleDbType.NVarchar2,
                    newUpdatedFullname, ParameterDirection.Input);


                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        /// <summary>
        /// Updates Shortcut of passed course.
        /// Make sure shortcut has length of 5.
        /// </summary>
        /// <param name="course"></param>
        /// <param name="newUpdatedShortcut"></param>
        public void UpdateCourseShortcut(Courses course, string newUpdatedShortcut)
        {
            //PROCEDURE UPDATE_SHORT_NAME(p_course_id T_ID, p_short_name T_SHORT_NAME)
            var commandText = "PKG_GROUP.UPDATE_FULL_NAME";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                    course.CourseId.ToString(), ParameterDirection.Input);
                OracleParameter p1 = command.Parameters.Add(":p_short_name", OracleDbType.NVarchar2,
                    newUpdatedShortcut.ToUpper(), ParameterDirection.Input);


                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void RemoveClassroom(Classrooms classroom)
        {
            // REMOVE(p_classroom_id T_ID)
            var commandText = "PKG_CLASSROOM.REMOVE";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                    classroom.ClassroomId.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void UpdateClassroomCapacity(Classrooms classroom, int updatedCapacity)
        {
            //UPDATE_CAPACITY(p_classroom_id T_ID, p_capacity T_CAPACITY) 
            var commandText = "PKG_CLASSROOM.UPDATE_CAPACITY";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                    classroom.ClassroomId.ToString(), ParameterDirection.Input);
                OracleParameter p1 = command.Parameters.Add(":p_capacity", OracleDbType.Int32,
                    updatedCapacity.ToString(), ParameterDirection.Input);


                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public void UpdateClassroomName(Classrooms classroom, string newClassroomName)
        {
            //UPDATE_NAME(p_classroom_id T_ID, p_name T_NAME)
            var commandText = "PKG_CLASSROOM.UPDATE_NAME";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                OracleParameter p0 = command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                    classroom.ClassroomId.ToString(), ParameterDirection.Input);
                OracleParameter p1 = command.Parameters.Add(":p_name", OracleDbType.NVarchar2,
                    newClassroomName, ParameterDirection.Input);


                connection.Open();
                // Execute the command
                command.ExecuteNonQuery();
                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }
        }

        public Classrooms GetClassroomById(int classroomId)
        {
            //GET_BY_ID(p_classroom_id T_ID) RETURN SYS_REFCURSOR
            var commandText = "PKG_CLASSROOM.GET_BY_ID";
            Classrooms classroom = new Classrooms();

            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
                OracleParameter p0 = command.Parameters.Add(":p_id", OracleDbType.Int32,
                    classroomId.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    classroom.ClassroomId = reader.GetInt32("CLASSROOM_ID");
                    classroom.Name = reader.GetString("NAME");
                    classroom.Capacity = reader.GetInt32("CAPACITY");
                }

                // Construct an OracleDataReader from the REF CURSOR
                connection.Close();
            }

            return classroom;
        }

        public List<Classrooms> GetClassroomByCapacity(int roomCapacity)
        {
            //GET_BY_CAPACITY(p_capacity T_CAPACITY) RETURN SYS_REFCURSOR
            var commandText = "PKG_CLASSROOM.GET_BY_CAPACITY";
            List<Classrooms> classrooms = new List<Classrooms>();


            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                OracleParameter p0 = command.Parameters.Add(":p_capacity", OracleDbType.Int32,
                    roomCapacity.ToString(), ParameterDirection.Input);

                connection.Open();
                // Execute the command
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Classrooms classroom = new Classrooms();
                    classroom.ClassroomId = reader.GetInt32("CLASSROOM_ID");
                    classroom.Name = reader.GetString("NAME");
                    classroom.Capacity = reader.GetInt32("CAPACITY");
                    classrooms.Add(classroom);
                }

                connection.Close();
            }

            return classrooms;
        }

        public List<Classrooms> GetClassroomAll()
        {
            //GET_ALL RETURN SYS_REFCURSOR
            var commandText = "PKG_CLASSROOM.GET_BY_CAPACITY";
            List<Classrooms> classrooms = new List<Classrooms>();


            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                // Execute the command
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Classrooms classroom = new Classrooms();
                    classroom.ClassroomId = reader.GetInt32("CLASSROOM_ID");
                    classroom.Name = reader.GetString("NAME");
                    classroom.Capacity = reader.GetInt32("CAPACITY");
                    classrooms.Add(classroom);
                }

                connection.Close();
            }

            return classrooms;
        }

        public List<Courses> getCourseAll()
        {
        
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            var commandText = "PKG_COURSE.GET_ALL";
            List<Courses> classrooms = new List<Courses>();
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            using (OracleCommand command = new OracleCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                // Execute the command
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Courses course = new Courses();
                    course.CourseId = reader.GetInt32("COURSE_ID");
                    course.FullName = reader.GetString("FULL_NAME");
                    course.ShortName = reader.GetString("SHORT_NAME");
                    course.Description = reader.GetString("DESCRIPTION");
                    classrooms.Add(course);
                }

                connection.Close();
            }

            return classrooms;
        }
    }
}