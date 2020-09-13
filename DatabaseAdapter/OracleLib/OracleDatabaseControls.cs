using System;
using System.Collections.Generic;
using System.Data;
using DatabaseAdapter.OracleLib.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DatabaseAdapter.OracleLib
{
    public class OracleDatabaseControls
    {
        private readonly string _connectionString;

        public OracleDatabaseControls(string databaseConnectionString)
        {
            _connectionString = databaseConnectionString;
        }

        public int InsertClassroom(Classrooms classroom)
        {
            //FUNCTION NEW(p_name T_NAME, p_capacity T_CAPACITY) RETURN T_ID
            const string commandText = "PKG_CLASSROOM.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("T_ID", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":T_NAME", OracleDbType.NVarchar2,
                classroom.Name, ParameterDirection.Input);
            command.Parameters.Add(":p_capacity", OracleDbType.Int32, classroom.ClassroomId.ToString(),
                ParameterDirection.Input);
            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public int InsertComment(Comments comment, int messageId)
        {
            //FUNCTION NEW(p_user PKG_USER.T_ID, p_message PKG_GMSG.T_ID, p_content T_CONTENT) RETURN T_ID AS v_id T_ID;
            const string commandText = "PKG_COMMENT.NEW";
            var courseId = -1;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p1 = command.Parameters.Add(":v_id", OracleDbType.Int32, courseId,
                ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user", OracleDbType.Int32, comment.UserId.ToString(),
                ParameterDirection.Input);
            command.Parameters.Add(":p_message", OracleDbType.Int32, messageId.ToString(),
                ParameterDirection.Input);
            //cmd.Parameters.Add(new SqlParameter("@TaxRate", string.IsNullOrEmpty(taxRateTxt.Text) ? (object)DBNull.Value : taxRateTxt.Text));
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2,
                string.IsNullOrEmpty(comment.Content) ? "Unavailable" : comment.Content,
                ParameterDirection.Input);
            command.Connection.Open();
            command.ExecuteNonQuery();
            courseId = int.Parse(p1.Value.ToString()!);

            command.Connection.Close();

            return courseId;
        }

        public int InsertCourse(Courses course)
        {
            //NEW(p_full_name T_FULL_NAME, p_short_name T_SHORT_NAME, p_description T_DESCRIPTION) RETURN T_ID AS v_id T_ID;
            const string commandText = "PKG_COURSE.NEW";
            var courseId = -1;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p1 = command.Parameters.Add(":v_id", OracleDbType.Int32, courseId,
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
            courseId = int.Parse(p1.Value.ToString()!);

            command.Connection.Close();

            return courseId;
        }

        public void InsertGrade(string studentId, string teacherId, string classId, string gradeValue,
            string description)
        {
            const string commandText =
                "INSERT INTO GRADES(STUDENT_ID, TEACHER_ID, COURSE_ID, VALUE, DESCRIPTION) VALUES (:student_id,:teacher_id,:course_id,:grade,:description)";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.Parameters.Add(":student_id", OracleDbType.Int32, studentId, ParameterDirection.Input);
            command.Parameters.Add(":teacher_id", OracleDbType.Int32, teacherId, ParameterDirection.Input);
            command.Parameters.Add(":course_id", OracleDbType.Int32, classId, ParameterDirection.Input);
            command.Parameters.Add(":grade", OracleDbType.Int32, gradeValue, ParameterDirection.Input);
            command.Parameters.Add(":description", OracleDbType.NVarchar2, description, ParameterDirection.Input);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

//        public void InsertStudent(string studentId, string teacherId)
//        {
//            const string commandText = "INSERT INTO STUDENTS(USER_ID, YEAR) VALUES (:user_id,:year_joined)";
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
        public int InsertStudent(User user)
        {
            //FUNCTION NEW(p_user_id PKG_USER.T_ID, p_year T_YEAR DEFAULT 1) RETURN T_ID
            const string commandText = "PKG_STUDENT.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("T_ID", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public int InsertStudent(User user, int year)
        {
            //FUNCTION NEW(p_user_id PKG_USER.T_ID, p_year T_YEAR DEFAULT 1) RETURN T_ID
            const string commandText = "PKG_STUDENT.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("T_ID", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_year", OracleDbType.Int32,
                year.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public void InsertGrade(string groupId, string userId, string content)
        {
            const string commandText =
                "INSERT INTO GROUP_MESSAGES(GROUP_ID, USER_ID, CONTENT) VALUES (:group_id,:user_id,:message_content)";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.Parameters.Add(":group_id", OracleDbType.Int32, groupId, ParameterDirection.Input);
            command.Parameters.Add(":user_id", OracleDbType.Int32, userId, ParameterDirection.Input);
            command.Parameters.Add(":message_content", OracleDbType.NVarchar2, content, ParameterDirection.Input);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public int InsertGroup(Group group)
        {
            //FUNCTION NEW(p_teacher_id PKG_TEACHER.T_ID, p_name T_NAME, p_max_capacity T_MAX_CAPACITY) RETURN T_ID AS v_id T_ID;
            const string commandText = "PKG_GROUP.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("T_ID", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                @group.TeacherId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_name", OracleDbType.NVarchar2,
                @group.Name, ParameterDirection.Input);
            command.Parameters.Add(":p_max_capacity", OracleDbType.Int32,
                @group.MaxCapacity.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public void InsertStudentIntoGroup(Students student, Group group)
        {
            //PROCEDURE ADD_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID) 
            const string commandText = "PKG_GROUP.ADD_STUDENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(":p_group_id", OracleDbType.Int32, @group.GroupId.ToString(),
                ParameterDirection.Input);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32, student.StudentId.ToString(),
                ParameterDirection.Input);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public void InsertStudentIntoGroup(Students student, int groupId)
        {
            //PROCEDURE ADD_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID) 
            const string commandText = "PKG_GROUP.ADD_STUDENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(":p_group_id", OracleDbType.Int32, groupId.ToString(),
                ParameterDirection.Input);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32, student.StudentId.ToString(),
                ParameterDirection.Input);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }


        public int InsertUser(User user)
        {
            const string commandText = "PKG_USER.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
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


            connection.Open();
            command.ExecuteNonQuery();
            var returnedId = int.Parse(command.Parameters[":v_id"].Value.ToString()!);


            command.Connection.Close();
            return returnedId;
        }

        public User GetUserById(int userId)
        {
            const string commandText = "PKG_USER.GET_USER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // Bind the parameters
            // p1 is the RETURN REF CURSOR bound to SELECT * FROM EMP;
            var p1 =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor);
            p1.Direction = ParameterDirection.ReturnValue;

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                userId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            var reader1 = ((OracleRefCursor) p1.Value).GetDataReader();
            var userid = -1;
            var username = "";
            var fname = "";
            var mname = "";
            var lname = "";
            var email = "";
            // Prints "reader1.GetName(0) = EMPNO"
            while (reader1.Read())
            {
                userid = reader1.GetInt32(0);
                username = reader1.GetString(1);
                fname = reader1.GetString(2);

                var check = reader1.IsDBNull(3);
                if (check)
                    mname = "";
                else
                    mname = reader1.GetString(3);

                lname = reader1.GetString(4);
                email = reader1.GetString(5);

                check = reader1.IsDBNull("STATUS_ID");
                if (check)
                {
                }
                else
                    reader1.GetInt32("STATUS_ID");
            }

            connection.Close();
            User usr;
            if (userid != -1)
            {
                //TODO FINISH THE BIO
                usr = new User(userid, username, fname, mname, lname, email, "Bio");
                return usr;
            }

            return null;
        }

        public void DeleteUser(User user)
        {
            const string commandText = "PKG_USER.UPDATE_STATUS";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            //UPDATE USERS SET STATUS_ID = p_status WHERE USER_ID = p_user_id;
            command.CommandType = CommandType.StoredProcedure;

            // Bind the parameters
            // p1 is the RETURN REF CURSOR bound to SELECT * FROM EMP;

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            command.Parameters.Add(":p_status", OracleDbType.Int32, "0", ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR


            connection.Close();
        }

        public User Login(string logonLogin, string logonPassword)
        {
            var userid = -1;
            //FUNCTION LOGIN(p_username USERS.USERNAME%TYPE, p_password USERS.PASSWORD%TYPE) RETURN USERS.USER_ID%TYPE IS
            //v_id USERS.USER_ID%TYPE;
            //v_status USERS.STATUS_ID%TYPE;
            const string commandText = "PKG_USER.LOGIN";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var p0 =
                command.Parameters.Add("v_id", OracleDbType.Int32);
            p0.Direction = ParameterDirection.ReturnValue;

            command.Parameters.Add(":p_username", OracleDbType.NVarchar2,
                logonLogin, ParameterDirection.Input);

            command.Parameters.Add(":p_password", OracleDbType.NVarchar2,
                logonPassword, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            try
            {
                userid = Int32.Parse(p0.Value.ToString()!);
            }
            catch (Exception)
            {
                return null;
            }

            connection.Close();
            if (userid != -1)
            {
                var user = GetUserById(userid);
                return user;
            }

            return null;
        }

        public int SendMessage(PrivateMessages msg)
        {
            const string commandText = "PKG_PMSG.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            //FUNCTION NEW(p_from_user PKG_USER.T_ID, p_to_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID AS
            command.CommandType = CommandType.StoredProcedure;

            var rval =
                command.Parameters.Add("v_id", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_from_user", OracleDbType.Int32,
                msg.FromUser.UserId.ToString(), ParameterDirection.Input);

            command.Parameters.Add(":p_to_user", OracleDbType.Int32, msg.ToUser.UserId.ToString(),
                ParameterDirection.Input);
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2, msg.Content,
                ParameterDirection.Input);
            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnval = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnval;
        }

        public PrivateMessages GetMessage(int id)
        {
            const string commandText = "PKG_PMSG.GET_BY_ID";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            var p1 =
                command.Parameters.Add("p_id", OracleDbType.RefCursor);
            p1.Direction = ParameterDirection.ReturnValue;

            command.Parameters.Add(":p_id", OracleDbType.Int32,
                id.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            var reader1 = ((OracleRefCursor) p1.Value).GetDataReader();
            var retMessage = new PrivateMessages();
            var messageId = -1;
            var content = "";
            var toUserId = -1;
            var fromUserId = -1;
            var time = DateTime.Now;

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
            retMessage.ToUser = GetUserById(toUserId);
            retMessage.FromUser = GetUserById(fromUserId);
            retMessage.PmsgId = messageId;

            connection.Close();
            if (messageId != -1)
            {
                //TODO FINISH THE BIO
                return retMessage;
            }

            return null;
        }

        public void RemoveMessage(PrivateMessages testMessage)
        {
            const string commandText = "PKG_PMSG.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_id", OracleDbType.Int32,
                testMessage.PmsgId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void RemoveMessage(int messageId)
        {
            const string commandText = "PKG_PMSG.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_id", OracleDbType.Int32,
                messageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }


        public void RemoveCourse(Courses course)
        {
            // PROCEDURE REMOVE(p_group_id T_ID) AS
            const string commandText = "PKG_GROUP.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCourseDescription(Courses course)
        {
            //PROCEDURE UPDATE_DESCRIPTION(p_course_id T_ID, p_description T_DESCRIPTION)
            const string commandText = "PKG_GROUP.UPDATE_DESCRIPTION";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_description", OracleDbType.NVarchar2,
                course.Description, ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCourseFullName(Courses course)
        {
            // PROCEDURE UPDATE_FULL_NAME(p_course_id T_ID, p_full_name T_FULL_NAME) 
            const string commandText = "PKG_GROUP.UPDATE_FULL_NAME";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_full_name", OracleDbType.NVarchar2,
                course.FullName, ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        /// <summary>
        /// Updates course
        /// </summary>
        /// <param name="course"></param>
        public void UpdateCourseShortcut(Courses course)
        {
            //PROCEDURE UPDATE_SHORT_NAME(p_course_id T_ID, p_short_name T_SHORT_NAME)
            const string commandText = "PKG_GROUP.UPDATE_FULL_NAME";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_short_name", OracleDbType.NVarchar2,
                course.ShortName.ToUpper(), ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void RemoveClassroom(Classrooms classroom)
        {
            // REMOVE(p_classroom_id T_ID)
            const string commandText = "PKG_CLASSROOM.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                classroom.ClassroomId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateClassroomCapacity(Classrooms classroom, int updatedCapacity)
        {
            //UPDATE_CAPACITY(p_classroom_id T_ID, p_capacity T_CAPACITY) 
            const string commandText = "PKG_CLASSROOM.UPDATE_CAPACITY";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                classroom.ClassroomId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_capacity", OracleDbType.Int32,
                updatedCapacity.ToString(), ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateClassroomName(Classrooms classroom, string newClassroomName)
        {
            //UPDATE_NAME(p_classroom_id T_ID, p_name T_NAME)
            const string commandText = "PKG_CLASSROOM.UPDATE_NAME";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                classroom.ClassroomId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_name", OracleDbType.NVarchar2,
                newClassroomName, ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public Classrooms GetClassroomById(int classroomId)
        {
            //GET_BY_ID(p_classroom_id T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_CLASSROOM.GET_BY_ID";
            var classroom = new Classrooms();

            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS

            command.Parameters.Add(":p_id", OracleDbType.Int32,
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

            return classroom;
        }

        public List<Classrooms> GetClassroomByCapacity(int roomCapacity)
        {
            //GET_BY_CAPACITY(p_capacity T_CAPACITY) RETURN SYS_REFCURSOR
            const string commandText = "PKG_CLASSROOM.GET_BY_CAPACITY";
            var classrooms = new List<Classrooms>();


            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_capacity", OracleDbType.Int32,
                roomCapacity.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var classroom = new Classrooms();
                classroom.ClassroomId = reader.GetInt32("CLASSROOM_ID");
                classroom.Name = reader.GetString("NAME");
                classroom.Capacity = reader.GetInt32("CAPACITY");
                classrooms.Add(classroom);
            }

            connection.Close();

            return classrooms;
        }

        public List<Classrooms> GetClassroomAll()
        {
            //GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_CLASSROOMS";
            var classrooms = new List<Classrooms>();


            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var classroom = new Classrooms();
                classroom.ClassroomId = reader.GetInt32("CLASSROOM_ID");
                classroom.Name = reader.GetString("NAME");
                classroom.Capacity = reader.GetInt32("CAPACITY");
                classrooms.Add(classroom);
            }

            connection.Close();

            return classrooms;
        }

        public List<Courses> GetCourseAll()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_COURSES";
            var classrooms = new List<Courses>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var course = new Courses();
                course.CourseId = reader.GetInt32("COURSE_ID");
                course.FullName = reader.GetString("FULL_NAME");
                course.ShortName = reader.GetString("SHORT_NAME");
                course.Description = reader.GetString("DESCRIPTION");
                classrooms.Add(course);
            }

            connection.Close();

            return classrooms;
        }

        public Courses GetCourseById(int courseId)
        {
            //FUNCTION GET_BY_ID(p_course_id T_ID) RETURN SYS_REFCURSOR 
            const string commandText = "PKG_COURSE.GET_BY_ID";
            Courses course = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p1 =
                command.Parameters.Add("ret", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                courseId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) p1.Value).GetDataReader();
            while (reader.Read())
            {
                course = new Courses
                {
                    CourseId = reader.GetInt32("COURSE_ID"),
                    FullName = reader.GetString("FULL_NAME"),
                    ShortName = reader.GetString("SHORT_NAME"),
                    Description = reader.GetString("DESCRIPTION")
                };
            }

            connection.Close();

            return course;
        }

        public List<PrivateMessages> GetMessageAll()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_PRIVATE_MESSAGES";
            var pMessages = new List<PrivateMessages>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.TableDirect};
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var message = new PrivateMessages();
                message.PmsgId = reader.GetInt32("MESSAGE_ID");
                message.FromUser = GetUserById(reader.GetInt32("FROM_USER_ID"));
                message.ToUser = GetUserById(reader.GetInt32("TO_USER_ID"));
                message.Content = reader.GetString("MESSAGE_CONTENT");
                message.Created = reader.GetDateTime("MESSAGE_CREATED");
                pMessages.Add(message);
            }

            connection.Close();

            return pMessages;
        }

        public PrivateMessages GetMessageById(int messageId)
        {
            //FUNCTION GET_BY_ID(p_course_id T_ID) RETURN SYS_REFCURSOR 
            const string commandText = "PKG_PMSG.GET_BY_ID";
            PrivateMessages pMessage = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var ret =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                messageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) ret.Value).GetDataReader();
            while (reader.Read())
            {
                pMessage = new PrivateMessages
                {
                    PmsgId = reader.GetInt32("MESSAGE_ID"),
                    FromUser = GetUserById(reader.GetInt32("FROM_USER_ID")),
                    ToUser = GetUserById(reader.GetInt32("TO_USER_ID")),
                    Content = reader.GetString("MESSAGE_CONTENT"),
                    Created = reader.GetDateTime("MESSAGE_CREATED")
                };
            }

            connection.Close();

            return pMessage;
        }

        public List<IPrivateMessages> GetMessageByUser(User us)
        {
            //FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
            const string commandText = "PKG_PMSG.GET_BY_USER";
            var pMessages = new List<IPrivateMessages>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var ret =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);

            command.Parameters.Add(":p_user", OracleDbType.Int32,
                us.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) ret.Value).GetDataReader();
            while (reader.Read())
            {
                var pMessage = new PrivateMessages();
                pMessage.PmsgId = reader.GetInt32("MESSAGE_ID");
                pMessage.FromUser = GetUserById(reader.GetInt32("FROM_USER_ID"));
                pMessage.ToUser = GetUserById(reader.GetInt32("TO_USER_ID"));
                pMessage.Content = reader.GetString("MESSAGE_CONTENT");
                pMessage.Created = reader.GetDateTime("MESSAGE_CREATED");
                pMessages.Add(pMessage);
            }

            connection.Close();

            return pMessages;
        }

        public List<Students> GetStudentAll()
        {
            // FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_STUDENTS";
            var students = new List<Students>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = GetUserById(reader.GetInt32("USER_ID"));
                var serializedParent = JsonConvert.SerializeObject(user);
                var student = JsonConvert.DeserializeObject<Students>(serializedParent);
                student.Year = reader.GetInt32("YEAR");
                student.StudentId = reader.GetInt32("STUDENT_ID");
                students.Add(student);
            }

            connection.Close();

            return students;
        }

        public Students GetStudentById(int studentId)
        {
            // FUNCTION GET_BY_ID(p_student_id T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_STUDENT.GET_BY_ID";
            Students student = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p0 = new OracleParameter
            {
                Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.RefCursor
            };
            command.Parameters.Add(p0);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                studentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = GetUserById(reader.GetInt32("USER_ID"));
                var serializedParent = JsonConvert.SerializeObject(user);
                student = JsonConvert.DeserializeObject<Students>(serializedParent);
                student.Year = reader.GetInt32("YEAR");
                student.StudentId = reader.GetInt32("STUDENT_ID");
            }

            connection.Close();

            return student;
        }

        public void UpdateStudentYear(Students student, int updatedYear)
        {
            //PROCEDURE UPDATE_YEAR(p_student_id T_ID, p_year T_YEAR)
            const string commandText = "PKG_STUDENT.UPDATE_YEAR";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                student.StudentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_year", OracleDbType.NVarchar2,
                updatedYear.ToString(), ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateStudentYear(int studentId, int updatedYear)
        {
            //PROCEDURE UPDATE_YEAR(p_student_id T_ID, p_year T_YEAR)
            const string commandText = "PKG_STUDENT.UPDATE_YEAR";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                studentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_year", OracleDbType.NVarchar2,
                updatedYear.ToString(), ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public Students GetStudentByUserId(int userId)
        {
            //FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_STUDENT.GET_BY_ID";
            Students student = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            OracleParameter retVal = new OracleParameter("retVal", OracleDbType.RefCursor);
            retVal.Direction = ParameterDirection.ReturnValue;

            command.Parameters.Add(retVal);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                userId.ToString(), ParameterDirection.Input);


            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = GetUserById(reader.GetInt32("USER_ID"));
                var serializedParent = JsonConvert.SerializeObject(user);
                student = JsonConvert.DeserializeObject<Students>(serializedParent);
                student.Year = reader.GetInt32("YEAR");
                student.StudentId = reader.GetInt32("STUDENT_ID");
            }

            connection.Close();

            return student;
        }

        public Students GetStudentByUserId(User user)
        {
            //FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_STUDENT.GET_BY_USER";
            Students student = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var serializedParent = JsonConvert.SerializeObject(user);
                student = JsonConvert.DeserializeObject<Students>(serializedParent);
                student.Year = reader.GetInt32("YEAR");
                student.StudentId = reader.GetInt32("STUDENT_ID");
            }

            connection.Close();

            return student;
        }

        public bool IsStudent(User user)
        {
            //FUNCTION IS_STUDENT(p_user_id PKG_USER.T_ID) RETURN BOOLEAN AS v_count NUMBER;
            const string commandText = "PKG_STUDENT.IS_STUDENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p0 =
                command.Parameters.Add("v_count", OracleDbType.Boolean, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            connection.Close();
            var boolean = (OracleBoolean) p0.Value;
            return boolean.Value;
        }

        public Teachers InsertTeacher(User user)
        {
            // FUNCTION NEW(p_user_id PKG_USER.T_ID) RETURN T_ID
            const string commandText = "PKG_TEACHER.NEW";
            Teachers professor;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var p0 = command.Parameters.Add("T_ID", OracleDbType.Int32, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            var serializedParent = JsonConvert.SerializeObject(user);
            professor = JsonConvert.DeserializeObject<Teachers>(serializedParent);
            professor.TeacherId = int.Parse(p0.Value.ToString()!);


            connection.Close();

            return professor;
        }

        public bool IsTeacher(Teachers teacher)
        {
            //FUNCTION IS_TEACHER(p_user_id PKG_USER.T_ID) RETURN BOOLEAN 
            const string commandText = "PKG_TEACHER.IS_TEACHER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p0 = command.Parameters.Add(":ret", OracleDbType.Boolean);
            p0.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                teacher.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            connection.Close();
            var b = (OracleBoolean) p0.Value;
            return b.Value;
        }

        public Teachers GetTeacherById(int teacherId)
        {
            //FUNCTION GET_BY_ID(p_teacher_id T_ID) RETURN SYS_REFCURSOR AS
            const string commandText = "PKG_TEACHER.GET_BY_ID";
            Teachers teacher = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p1 =
                command.Parameters.Add("result", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                teacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) p1.Value).GetDataReader();

            while (reader.Read())
            {
                var user = GetUserById(reader.GetInt32("USER_ID"));
                var serializedParent = JsonConvert.SerializeObject(user);
                teacher = JsonConvert.DeserializeObject<Teachers>(serializedParent);
                teacher.TeacherId = reader.GetInt32("TEACHER_ID");
            }

            connection.Close();

            return teacher;
        }

        public List<Teachers> GetTeacherAll()
        {
            // FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_TEACHERS";
            var teachers = new List<Teachers>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = GetUserById(reader.GetInt32("USER_ID"));
                var serializedParent = JsonConvert.SerializeObject(user);
                var teacher = JsonConvert.DeserializeObject<Teachers>(serializedParent);
                teacher.TeacherId = reader.GetInt32("TEACHER_ID");
                teachers.Add(teacher);
            }

            connection.Close();

            return teachers;
        }

        public Teachers GetTeacherByUser(User user)
        {
            //FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_TEACHER.GET_BY_USER";
            Teachers teacher = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var p1 =
                command.Parameters.Add("return", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) p1.Value).GetDataReader();
            while (reader.Read())
            {
                var serializedParent = JsonConvert.SerializeObject(user);
                teacher = JsonConvert.DeserializeObject<Teachers>(serializedParent);
                teacher.TeacherId = reader.GetInt32("TEACHER_ID");
            }

            connection.Close();

            return teacher;
        }

        public Teachers GetTeacherByUser(int userId)
        {
            //FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_TEACHER.GET_BY_USER";
            Teachers teacher = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                userId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = GetUserById(userId);
                var serializedParent = JsonConvert.SerializeObject(user);
                teacher = JsonConvert.DeserializeObject<Teachers>(serializedParent);
                teacher.TeacherId = reader.GetInt32("TEACHER_ID");
            }

            connection.Close();

            return teacher;
        }

        public List<User> GetUserAll()
        {
            // FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_USERS";
            var users = new List<User>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = new User();
                user.UserId = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.FirstName = reader.GetString(2);

                var check = reader.IsDBNull(3);
                if (check)
                    user.MiddleName = "";
                else
                    user.MiddleName = reader.GetString(3);

                user.LastName = reader.GetString(4);
                user.Email = reader.GetString(5);

                check = reader.IsDBNull("STATUS_ID");
                if (check)
                {
                }
                else
                    reader.GetInt32("STATUS_ID");

                user.Admin = reader.GetBoolean("ADMIN");
                users.Add(user);
            }

            connection.Close();

            return users;
        }

        public void SetUserAdmin(User user, bool value)
        {
            //PROCEDURE UPDATE_ADMIN(p_user_id USERS.USER_ID%TYPE, p_admin USERS.ADMIN%TYPE) IS
            const string commandText = "PKG_USER.UPDATE_ADMIN";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_admin", OracleDbType.Int32,
                value ? 1.ToString() : 0.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            connection.Close();
            user.Admin = value;
        }

        public void UpdateUser(User user)
        {
            //PROCEDURE UPDATE_DETAILS(p_user_id USERS.USER_ID%TYPE,
            // p_firstname USERS.FIRST_NAME%TYPE, p_middlename USERS.MIDDLE_NAME%TYPE, p_lastname USERS.LAST_NAME%TYPE,
            // p_email USERS.EMAIL%TYPE)
            const string commandText = "PKG_USER.UPDATE_DETAILS";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_firstname", OracleDbType.NVarchar2,
                user.FirstName, ParameterDirection.Input);
            command.Parameters.Add(":p_middlename", OracleDbType.NVarchar2,
                user.MiddleName, ParameterDirection.Input);
            command.Parameters.Add(":p_lastname", OracleDbType.NVarchar2,
                user.LastName, ParameterDirection.Input);
            command.Parameters.Add(":p_email", OracleDbType.NVarchar2,
                user.Email, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateLogin(int userId, string newLogin, string newPassword)
        {
            //PROCEDURE UPDATE_LOGIN(p_user_id USERS.USER_ID%TYPE, p_username USERS.USERNAME%TYPE, p_password USERS.PASSWORD%TYPE)
            const string commandText = "PKG_USER.UPDATE_LOGIN";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                userId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_username", OracleDbType.NVarchar2,
                newLogin, ParameterDirection.Input);
            command.Parameters.Add(":p_password", OracleDbType.NVarchar2,
                newPassword, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateUserStatus(int userId, int value)
        {
            //PROCEDURE UPDATE_STATUS(p_user_id USERS.USER_ID%TYPE, p_status USERS_STATUS.STATUS_ID%TYPE)

            const string commandText = "PKG_USER.UPDATE_STATUS";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                userId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_status", OracleDbType.NVarchar2,
                value.ToString()!, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            connection.Close();
        }

        public Group GetGroupById(int groupId)
        {
            // FUNCTION GET_BY_ID(p_group_id T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_GROUP.GET_BY_ID";
            Group group = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            while (reader.Read())
            {
                @group = new Group
                {
                    Name = reader.GetString("GROUP_NAME"),
                    GroupId = reader.GetInt32("GROUP_ID"),
                    TeacherId = reader.GetInt32("TEACHER_ID"),
                    ActualCapacity = reader.GetInt32("ACTUAL_CAPACITY"),
                    MaxCapacity = reader.GetInt32("MAX_CAPACITY")
                };
            }

            connection.Close();

            return group;
        }

        public List<Group> GetGroupAll()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            const string commandText = "VW_GROUPS";
            var groups = new List<Group>();


            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var group = new Group
                {
                    Name = reader.GetString("GROUP_NAME"), GroupId = reader.GetInt32("GROUP_ID"),
                    MaxCapacity = reader.GetInt32("MAX_CAPACITY"),
                    ActualCapacity = reader.GetInt32("ACTUAL_CAPACITY"), TeacherId = reader.GetInt32("TEACHER_ID")
                };
                groups.Add(@group);
            }

            connection.Close();

            return groups;
        }

        public Group GetGroupByStudent(Students student)
        {
            //FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_BY_STUDENT";
            Group group = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                student.StudentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                @group = GetGroupById(reader.GetInt32("GROUP_ID"));
            }

            connection.Close();

            return group;
        }

        public Group GetGroupByStudentId(int studentId)
        {
            //FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_BY_STUDENT";
            Group group = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                studentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                @group = GetGroupById(reader.GetInt32("GROUP_ID"));
            }

            connection.Close();

            return group;
        }

        public Group GetGroupByTeacher(Teachers teacher)
        {
            //  FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_BY_TEACHER";
            Group group = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                teacher.TeacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                @group = GetGroupById(reader.GetInt32("GROUP_ID"));
            }

            connection.Close();

            return group;
        }

        public Group GetGroupByTeacherId(int teacherId)
        {
            //  FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_BY_TEACHER";
            Group group = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                teacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                @group = GetGroupById(reader.GetInt32("GROUP_ID"));
            }

            connection.Close();

            return group;
        }

        public List<Students> GetGroupUsers(int groupId)
        {
            //FUNCTION GET_STUDENTS(p_group_id T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_STUDENTS";
            var students = new List<Students>();


            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var ret =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) ret.Value).GetDataReader();
            while (reader.Read())
            {
                var student = GetStudentById(reader.GetInt32("STUDENT_ID"));
                students.Add(student);
            }

            connection.Close();

            return students;
        }

        public void RemoveGroup(int groupId)
        {
            //PROCEDURE REMOVE(p_group_id T_ID)
            const string commandText = "PKG_GROUP.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void RemoveStudentFromGroup(int studentId, Group grp)
        {
            // PROCEDURE REMOVE_STUDENT(p_group_id T_ID, p_student_id PKG_STUDENT.T_ID)
            const string commandText = "PKG_GROUP.REMOVE_STUDENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                grp.GroupId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                studentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateGroupName(int groupId, string updatedName)
        {
            //PROCEDURE UPDATE_NAME(p_group_id T_ID, p_name T_NAME)
            const string commandText = "PKG_GROUP.UPDATE_NAME";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_name", OracleDbType.NVarchar2,
                updatedName, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateGroupTeacher(int groupId, int newTeacherId)
        {
            //PROCEDURE UPDATE_TEACHER(p_group_id T_ID, p_teacher_id PKG_TEACHER.T_ID)
            const string commandText = "PKG_GROUP.UPDATE_TEACHER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                newTeacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void InsertCourseIntoGroup(int groupId, Courses course)
        {
            // PROCEDURE ADD_COURSE(p_group_id T_ID, p_course_id PKG_COURSE.T_ID)
            const string commandText = "PKG_GROUP.ADD_COURSE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Courses> GetGroupCourses(int groupId)
        {
            //FUNCTION GET_COURSES(p_group_id T_ID) RETURN SYS_REFCURSOR
            const string commandText = "PKG_GROUP.GET_COURSES";
            var courses = new List<Courses>();

            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var ret =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                groupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) ret.Value).GetDataReader();
            while (reader.Read())
            {
                var course = GetCourseById(reader.GetInt32("COURSE_ID"));
                courses.Add(course);
            }

            connection.Close();

            return courses;
        }

        public int SendGroupMessage(GroupMessages groupMessage)
        {
            var returnVal = -1;
            // FUNCTION NEW(p_group PKG_GROUP.T_ID, p_user PKG_USER.T_ID, p_content T_CONTENT) RETURN T_ID 
            const string commandText = "PKG_GMSG.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("v_id", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_group", OracleDbType.Int32,
                groupMessage.GroupId.ToString(), ParameterDirection.Input);

            command.Parameters.Add(":p_user", OracleDbType.Int32, groupMessage.UserId.ToString(),
                ParameterDirection.Input);
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2, groupMessage.Content,
                ParameterDirection.Input);
            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public List<GroupMessages> GetGroupMessageAll()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            var groupMessages = new List<GroupMessages>();
            const string commandText = "VW_GROUP_MESSAGES";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var gMessage = new GroupMessages
                {
                    Created = reader.GetDateTime("CREATED"), Content = reader.GetString("CONTENT"),
                    GmsgId = reader.GetInt32("MESSAGE_ID"), GroupId = reader.GetInt32("GROUP_ID"),
                    UserId = reader.GetInt32("AUTHOR_ID")
                };
                groupMessages.Add(gMessage);
            }

            connection.Close();

            return groupMessages;
        }

        public List<GroupMessages> GetGroupMessageByGroup(Group group)
        {
            //FUNCTION GET_BY_GROUP(p_group PKG_GROUP.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            var groupMessages = new List<GroupMessages>();
            const string commandText = "PKG_GMSG.GET_BY_GROUP";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var retVal =
                command.Parameters.Add("v_id", OracleDbType.RefCursor);
            retVal.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_group", OracleDbType.Int32, @group.GroupId.ToString(),
                ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) retVal.Value).GetDataReader();
            while (reader.Read())
            {
                var gMessage = new GroupMessages
                {
                    Created = reader.GetDateTime("CREATED"), Content = reader.GetString("CONTENT"),
                    GmsgId = reader.GetInt32("MESSAGE_ID"), GroupId = reader.GetInt32("GROUP_ID"),
                    UserId = reader.GetInt32("AUTHOR_ID")
                };
                groupMessages.Add(gMessage);
            }

            connection.Close();

            return groupMessages;
        }

        public GroupMessages GetGroupMessageById(int messageId)
        {
            // FUNCTION GET_BY_ID(p_message T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GMSG.GET_BY_ID";
            GroupMessages groupMessage = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_message", OracleDbType.Int32,
                messageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                groupMessage = new GroupMessages
                {
                    Created = reader.GetDateTime("CREATED"), Content = reader.GetString("CONTENT"),
                    GmsgId = reader.GetInt32("MESSAGE_ID"), GroupId = reader.GetInt32("GROUP_ID"),
                    UserId = reader.GetInt32("AUTHOR_ID")
                };
            }

            connection.Close();

            return groupMessage;
        }

        public List<GroupMessages> GetGroupMessagesByUser(int userId)
        {
            //FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            var groupMessages = new List<GroupMessages>();
            const string commandText = "PKG_GMSG.GET_BY_USER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var retVal =
                command.Parameters.Add("v_id", OracleDbType.RefCursor);
            retVal.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_user", OracleDbType.Int32, userId.ToString(),
                ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) retVal.Value).GetDataReader();
            while (reader.Read())
            {
                var gMessage = new GroupMessages
                {
                    Created = reader.GetDateTime("CREATED"), Content = reader.GetString("CONTENT"),
                    GmsgId = reader.GetInt32("MESSAGE_ID"), GroupId = reader.GetInt32("GROUP_ID"),
                    UserId = reader.GetInt32("AUTHOR_ID")
                };
                groupMessages.Add(gMessage);
            }

            connection.Close();

            return groupMessages;
        }

        public void UpdateGroupMessageContent(GroupMessages groupMessage, string updatedContent)
        {
            //    PROCEDURE UPDATE_CONTENT(p_message T_ID, p_content T_CONTENT) AS
            const string commandText = "PKG_GMSG.UPDATE_CONTENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_message", OracleDbType.Int32,
                groupMessage.GmsgId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2,
                updatedContent, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void RemoveGroupMessage(int groupMessageId)
        {
            //    PROCEDURE REMOVE(p_message T_ID)
            const string commandText = "PKG_GMSG.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_message", OracleDbType.Int32,
                groupMessageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateGroupMessageGroup(int groupMessageId, int updatedGroupId)
        {
            //PROCEDURE UPDATE_GROUP(p_message T_ID, p_group PKG_GROUP.T_ID) 
            const string commandText = "PKG_GMSG.UPDATE_GROUP";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_message", OracleDbType.Int32,
                groupMessageId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_group", OracleDbType.Int32,
                updatedGroupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateGroupMessageOwner(int groupMessageId, int newMessageOwnerId)
        {
            //TODO COTO KURVA JIRKO?!
            //PROCEDURE UPDATE_USER(p_message T_ID, p_user PKG_USER.T_ID)
            const string commandText = "PKG_GMSG.UPDATE_GROUP";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_message", OracleDbType.Int32,
                groupMessageId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_user", OracleDbType.Int32,
                newMessageOwnerId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public List<Comments> GetComments()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            var comments = new List<Comments>();
            const string commandText = "VW_COMMENTS";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var usrId = reader.GetInt32("COMMENT_AUTHOR_ID");
                var comment = new Comments
                {
                    UserId = usrId,
                    CommentId = reader.GetInt32("COMMENT_ID"),
                    MessageId = reader.GetInt32("MESSAGE_ID"),
                    Created = reader.GetDateTime("MESSAGE_CREATED"),
                    Content = reader.GetString("MESSAGE_CONTENT"),
                    ContentOwner = GetUserById(usrId)
                };
                comments.Add(comment);
            }

            connection.Close();

            return comments;
        }

        public Comments GetCommentById(int commentId)
        {
            //  FUNCTION GET_BY_ID(p_comment T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_COMMENT.GET_BY_ID";
            Comments comment = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                commentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            if (reader.Read())
            {
                var usrId = reader.GetInt32("COMMENT_AUTHOR_ID");
                comment = new Comments
                {
                    UserId = usrId,
                    CommentId = reader.GetInt32("COMMENT_ID"),
                    MessageId = reader.GetInt32("MESSAGE_ID"),
                    Created = reader.GetDateTime("MESSAGE_CREATED"),
                    Content = reader.GetString("COMMENT_CONTENT"),
                    ContentOwner = GetUserById(usrId)
                };
            }

            connection.Close();

            return comment;
        }

        public List<Comments> GetCommentsByMessage(int messageId)
        {
            //FUNCTION GET_BY_MESSAGE(p_message PKG_GMSG.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_COMMENT.GET_BY_MESSAGE";
            var comments = new List<Comments>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_message", OracleDbType.Int32,
                messageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            while (reader.Read())
            {
                var usrId = reader.GetInt32("COMMENT_AUTHOR_ID");
                comments.Add(new Comments
                    {
                        UserId = usrId,
                        CommentId = reader.GetInt32("COMMENT_ID"),
                        MessageId = reader.GetInt32("MESSAGE_ID"),
                        Created = reader.GetDateTime("MESSAGE_CREATED"),
                        Content = reader.GetString("COMMENT_CONTENT"),
                        ContentOwner = GetUserById(usrId)
                    }
                );
            }

            connection.Close();

            return comments;
        }

        public List<Comments> GetCommentsByUser(User user)
        {
            //UNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_COMMENT.GET_BY_USER";
            var comments = new List<Comments>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rval =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rval.Value).GetDataReader();

            while (reader.Read())
            {
                var usrId = reader.GetInt32("COMMENT_AUTHOR_ID");
                comments.Add(new Comments
                    {
                        UserId = usrId,
                        CommentId = reader.GetInt32("COMMENT_ID"),
                        MessageId = reader.GetInt32("MESSAGE_ID"),
                        Created = reader.GetDateTime("MESSAGE_CREATED"),
                        Content = reader.GetString("MESSAGE_CONTENT"),
                        ContentOwner = GetUserById(usrId)
                    }
                );
            }

            connection.Close();

            return comments;
        }

        public void RemoveCommentById(int gMessageId)
        {
            //PROCEDURE REMOVE(p_comment T_ID)
            const string commandText = "PKG_GMSG.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                gMessageId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void RemoveComment(Comments commentToBeRemoved)
        {
            //PROCEDURE REMOVE(p_comment T_ID)
            const string commandText = "PKG_COMMENT.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                commentToBeRemoved.CommentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCommentContent(Comments comment, string updatedContent)
        {
            // PROCEDURE UPDATE_CONTENT(p_comment T_ID, p_content T_CONTENT
            const string commandText = "PKG_COMMENT.UPDATE_CONTENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                comment.CommentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2,
                updatedContent, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCommentContent(int commentId, string updatedContent)
        {
            // PROCEDURE UPDATE_CONTENT(p_comment T_ID, p_content T_CONTENT
            const string commandText = "PKG_GMSG.UPDATE_CONTENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                commentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_content", OracleDbType.NVarchar2,
                updatedContent, ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCommentPost(int commentId, int newPostId)
        {
            //PROCEDURE UPDATE_MESSAGE(p_comment T_ID, p_message PKG_GMSG.T_ID) 
            const string commandText = "PKG_COMMENT.UPDATE_MESSAGE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                commentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_message", OracleDbType.Int32,
                newPostId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateCommentOwner(int commentId, int newUserId)
        {
            //PROCEDURE UPDATE_USER(p_comment T_ID, p_user PKG_USER.T_ID) 
            const string commandText = "PKG_COMMENT.UPDATE_USER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                commentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_user", OracleDbType.Int32,
                newUserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public int InsertTimetable(Timetables timetable)
        {
            //  FUNCTION NEW(p_group_id PKG_GROUP.T_ID, p_classroom_id PKG_CLASSROOM.T_ID, p_begin T_BEGIN,
            //p_end T_END) RETURN T_ID 
            const string commandText = "PKG_TIMETABLE.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var p0 = command.Parameters.Add("T_ID", OracleDbType.Int32, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                timetable.Group.GroupId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_classroom_id", OracleDbType.Int32,
                timetable.Classroom.ClassroomId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_begin", OracleDbType.TimeStamp,
                timetable.Begin, ParameterDirection.Input);
            command.Parameters.Add(":p_end", OracleDbType.TimeStamp,
                timetable.End, ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            timetable.TimetableId = int.Parse(p0.Value.ToString()!);


            connection.Close();

            return timetable.TimetableId;
        }

        public int InsertGrade(Grades grade)
        {
            // FUNCTION NEW(p_student_id PKG_STUDENT.T_ID, p_teacher_id PKG_TEACHER.T_ID, p_course_id PKG_COURSE.T_ID,
            // p_value T_VALUE, p_description T_DESCRIPTION) RETURN T_ID
            const string commandText = "PKG_GRADES.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var p0 = command.Parameters.Add("T_ID", OracleDbType.Int32, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                grade.StudentId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                grade.TeacherId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                grade.CourseId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_value", OracleDbType.Int32,
                grade.Value.ToString()!, ParameterDirection.Input);
            command.Parameters.Add(":p_description", OracleDbType.NVarchar2,
                grade.Description, ParameterDirection.Input);


            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            grade.GradeId = int.Parse(p0.Value.ToString()!);


            connection.Close();

            return grade.GradeId;
        }

        public List<Grades> GetGrades()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR
            var grades = new List<Grades>();
            const string commandText = "VW_GRADES";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.TableDirect};

            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var grade = new Grades
                {
                    Created = reader.GetDateTime("GRADE_CREATED"),
                    Description = reader.GetString("GRADE_DESCRIPTION"),
                    Value = reader.GetInt32("GRADE_VALUE"),
                    CourseId = reader.GetInt32("COURSE_ID"),
                    StudentId = reader.GetInt32("STUDENT_ID"),
                    TeacherId = reader.GetInt32("TEACHER_ID"),
                    GradeId = reader.GetInt32("GRADE_ID")
                };
                grades.Add(grade);
            }

            connection.Close();

            return grades;
        }

        public List<Grades> GetGrades(Courses course)
        {
            //FUNCTION GET_BY_COURSE(p_course_id PKG_COURSE.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GRADES.GET_BY_COURSE";
            var grades = new List<Grades>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                course.CourseId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            while (reader.Read())
            {
                grades.Add(new Grades
                    {
                        Created = reader.GetDateTime("CREATED"),
                        Description = reader.GetString("DESCRIPTION"),
                        Value = reader.GetInt32("VALUE"),
                        CourseId = reader.GetInt32("COURSE_ID"),
                        StudentId = reader.GetInt32("STUDENT_ID"),
                        TeacherId = reader.GetInt32("TEACHER_ID"),
                        GradeId = reader.GetInt32("GRADE_ID")
                    }
                );
            }

            connection.Close();

            return grades;
        }

        public Grades GetGrade(int gradeId)
        {
            //FUNCTION GET_BY_ID(p_grade_id T_ID) RETURN SYS_REFCURSOR AS
            // v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GRADES.GET_BY_ID";
            Grades grade = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                gradeId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            if (reader.Read())
            {
                grade = new Grades
                {
                    Created = reader.GetDateTime("GRADE_CREATED"),
                    Description = reader.GetString("GRADE_DESCRIPTION"),
                    Value = reader.GetInt32("GRADE_VALUE"),
                    CourseId = reader.GetInt32("COURSE_ID"),
                    StudentId = reader.GetInt32("STUDENT_ID"),
                    TeacherId = reader.GetInt32("TEACHER_ID"),
                    GradeId = reader.GetInt32("GRADE_ID")
                };
            }

            connection.Close();

            return grade;
        }

        public List<Grades> GetGrades(Students student)
        {
            //FUNCTION GET_BY_STUDENT(p_student_id PKG_STUDENT.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GRADES.GET_BY_STUDENT";
            var grades = new List<Grades>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_student_id", OracleDbType.Int32,
                student.StudentId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            while (reader.Read())
            {
                grades.Add(new Grades
                    {
                        Created = reader.GetDateTime("GRADE_CREATED"),
                        Description = reader.GetString("GRADE_DESCRIPTION"),
                        Value = reader.GetInt32("GRADE_VALUE"),
                        CourseId = reader.GetInt32("COURSE_ID"),
                        StudentId = reader.GetInt32("STUDENT_ID"),
                        TeacherId = reader.GetInt32("TEACHER_ID"),
                        GradeId = reader.GetInt32("GRADE_ID")
                    }
                );
            }

            connection.Close();

            return grades;
        }

        public List<Grades> GetGrades(Teachers teacher)
        {
            //FUNCTION GET_BY_TEACHER(p_teacher_id PKG_TEACHER.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GRADES.GET_BY_TEACHER";
            var grades = new List<Grades>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_teacher_id", OracleDbType.Int32,
                teacher.TeacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            while (reader.Read())
            {
                grades.Add(new Grades
                    {
                        Created = reader.GetDateTime("GRADE_CREATED"),
                        Description = reader.GetString("GRADE_DESCRIPTION"),
                        Value = reader.GetInt32("GRADE_VALUE"),
                        CourseId = reader.GetInt32("COURSE_ID"),
                        StudentId = reader.GetInt32("STUDENT_ID"),
                        TeacherId = reader.GetInt32("TEACHER_ID"),
                        GradeId = reader.GetInt32("GRADE_ID")
                    }
                );
            }

            connection.Close();

            return grades;
        }

        public void RemoveGrade(int gradeId)
        {
            //PROCEDURE REMOVE(p_grade_id T_ID)
            const string commandText = "PKG_GRADES.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                gradeId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            // Construct an OracleDataReader from the REF CURSOR
            connection.Close();
        }

        public void UpdateGradeCourse(Grades grade, int courseId)
        {
            // PROCEDURE UPDATE_COURSE(p_grade_id T_ID, p_course_id PKG_COURSE.T_ID)
            const string commandText = "PKG_GRADES.UPDATE_COURSE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                grade.GradeId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_course_id", OracleDbType.Int32,
                courseId.ToString(), ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateGradeDescription(Grades grade, string description)
        {
            // PROCEDURE UPDATE_DESCRIPTION(p_grade_id T_ID, p_description T_DESCRIPTION)
            const string commandText = "PKG_GRADES.UPDATE_DESCRIPTION";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                grade.GradeId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_description", OracleDbType.NVarchar2,
                description, ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateGradeStudent(Grades grade, int studentId)
        {
            // PROCEDURE UPDATE_STUDENT(p_grade_id T_ID, p_student_id PKG_STUDENT.T_ID) 
            const string commandText = "PKG_GRADES.UPDATE_STUDENT";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                grade.GradeId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_student_id", OracleDbType.NVarchar2,
                studentId.ToString(), ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateGradeTeacher(Grades grade, int teacherId)
        {
            // PROCEDURE UPDATE_TEACHER(p_grade_id T_ID, p_teacher_id PKG_TEACHER.T_ID)
            const string commandText = "PKG_GRADES.UPDATE_TEACHER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                grade.GradeId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_teacher_id", OracleDbType.NVarchar2,
                teacherId.ToString(), ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateGradeValue(Grades grade, int value)
        {
            // PROCEDURE UPDATE_VALUE(p_grade_id T_ID, p_value T_VALUE)
            const string commandText = "PKG_GRADES.UPDATE_VALUE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            // FUNCTION GET_BY_ID(p_id T_ID) RETURN SYS_REFCURSOR AS
            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                grade.GradeId.ToString(), ParameterDirection.Input);
            command.Parameters.Add(":p_value", OracleDbType.NVarchar2,
                value.ToString()!, ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }


        public int InsertImage(Files image)
        {
            //FUNCTION NEW(p_user_id PKG_USER.T_ID, p_file_name T_NAME, p_file_type T_TYPE, p_file_data T_DATA) RETURN T_ID 
            const string commandText = "PKG_FILE.NEW";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rval =
                command.Parameters.Add("T_ID", OracleDbType.Int32);
            rval.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                image.UserId.ToString(), ParameterDirection.Input);

            command.Parameters.Add(":p_file_name", OracleDbType.NVarchar2, image.FileName,
                ParameterDirection.Input);

            command.Parameters.Add(":p_file_type", OracleDbType.NVarchar2, image.FileType,
                ParameterDirection.Input);

            command.Parameters.Add(":p_file_data", OracleDbType.Blob, image.FileData,
                ParameterDirection.Input);
            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();

            // Construct an OracleDataReader from the REF CURSOR

            var returnVal = int.Parse(rval.Value.ToString()!);
            connection.Close();
            return returnVal;
        }

        public void RemoveFile(int pictureId)
        {
            // PROCEDURE REMOVE(p_file_id T_ID)
            const string commandText = "PKG_FILE.REMOVE";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(":p_grade_id", OracleDbType.Int32,
                pictureId.ToString(), ParameterDirection.Input);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Files> GetFileAll()
        {
            //FUNCTION GET_ALL RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            var files = new List<Files>();
            const string commandText = "FILES";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection);
            command.CommandType = CommandType.TableDirect;
            connection.Open();
            // Execute the command
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var file = new Files
                {
                    Created = reader.GetDateTime("CREATED"),
                    UserId = reader.GetInt32("USER_ID"),
                    FileName = reader.GetString("FILE_NAME"),
                    FileType = reader.GetString("FILE_TYPE"),
                    FileId = reader.GetInt32("FILE_ID"),
                    FileData = reader.GetOracleBlob(4).Value
                };
                files.Add(file);
            }

            connection.Close();

            return files;
        }

        public Files GetFile(int fileId)
        {
            //FUNCTION GET_BY_ID(p_file_id T_ID) RETURN SYS_REFCURSOR
            Files file = null;
            const string commandText = "PKG_FILE.GET_BY_ID";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_comment", OracleDbType.Int32,
                fileId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            if (reader.Read())
            {
                file = new Files
                {
                    Created = reader.GetDateTime("CREATED"),
                    FileData = reader.GetOracleBlob(5).Value,
                    FileId = reader.GetInt32("FILE_ID"),
                    FileType = reader.GetString("FILE_TYPE"),
                    UserId = reader.GetInt32("USER_ID"),
                    FileName = "RETURNED_FILE"
                };
            }

            connection.Close();

            return file;
        }

        public List<Files> GetFile(User user)
        {
            //FUNCTION GET_BY_USER(p_user_id PKG_USER.T_ID) RETURN SYS_REFCURSOR
            var files = new List<Files>();
            const string commandText = "PKG_FILE.GET_BY_USER";
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user_id", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            if (reader.Read())
            {
                files.Add(new Files
                {
                    Created = reader.GetDateTime("CREATED"),
                    FileData = reader.GetOracleBlob(5).Value,
                    FileId = reader.GetInt32("FILE_ID"),
                    FileType = reader.GetString("FILE_TYPE"),
                    UserId = reader.GetInt32("USER_ID"),
                    FileName = "RETURNED_FILE"
                });
            }

            connection.Close();

            return files;
        }

        public List<Group> GetUserGroups(User user)
        {
            // FUNCTION GET_BY_USER(p_user PKG_USER.T_ID) RETURN SYS_REFCURSOR AS
            //v_cursor SYS_REFCURSOR;
            const string commandText = "PKG_GROUP.GET_BY_USER";
            var groups = new List<Group>();
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_user", OracleDbType.Int32,
                user.UserId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            while (reader.Read())
            {
                groups.Add(new Group
                    {
                        Name = reader.GetString("GROUP_NAME"), GroupId = reader.GetInt32("GROUP_ID"),
                        MaxCapacity = reader.GetInt32("MAX_CAPACITY"),
                        ActualCapacity = reader.GetInt32("ACTUAL_CAPACITY"),
                        TeacherId = reader.GetInt32("TEACHER_ID")
                    }
                );
            }

            connection.Close();

            return groups;
        }

        public Timetables GetTimetablesByGroup(Group group)
        {
            //FUNCTION GET_BY_GROUP(p_group_id PKG_GROUP.T_ID) RETURN SYS_REFCURSOR;
            const string commandText = "PKG_TIMETABLE.GET_BY_GROUP";
            Timetables timetables = null;
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};

            var rVal =
                command.Parameters.Add("v_cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(":p_group_id", OracleDbType.Int32,
                group.GroupId.ToString(), ParameterDirection.Input);

            connection.Open();
            // Execute the command
            command.ExecuteNonQuery();
            var reader = ((OracleRefCursor) rVal.Value).GetDataReader();

            while (reader.Read())
            {
                timetables = new Timetables
                    {
                        TimetableId = reader.GetInt32("TIMETABLE_ID"),
                        Begin = reader.GetDateTime("BEGIN"),
                        End = reader.GetDateTime("END"),
                        Classroom = GetClassroomById(reader.GetInt32("GROUP_ID")),
                        Group = group
                    };
            }

            connection.Close();
            return timetables;
        }
    }
}