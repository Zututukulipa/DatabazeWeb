using System.Collections.Generic;
using System.Linq;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;

namespace Databaze_API.Controllers
{
    public class UserController
    {
        private string ConnectionString { get; }

        private readonly OracleDatabaseControls _controls;


        public User Login(string logonLogin, string logonPassword)
        {
            return _controls.Login(logonLogin, logonPassword);
        }

        public UserController()
        {
            ConnectionString = "DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;";
            _controls = new OracleDatabaseControls(ConnectionString);
        }


        public List<IPrivateMessages> GetUserPrivateMessages(User user)
        {
            return _controls.GetMessageByUser(user);
        }

        public List<WallPost> GetWallPosts(User activeUser)
        {
            var gMsgs = _controls.GetGroupMessagesByUser(activeUser.UserId);

            return gMsgs.Select(message => new WallPost
                {
                    Owner = _controls.GetUserById(message.UserId),
                    Created = message.Created,
                    GmsgId = message.GmsgId,
                    GroupId = message.GroupId,
                    MessageContent = message.Content,
                    Comments = _controls.GetCommentsByMessage(message.GmsgId)
                })
                .ToList();
        }

        public void AddWallPost(WallPost addedPost)
        {
            var message = new GroupMessages
            {
                Content = addedPost.MessageContent, Created = addedPost.Created, GroupId = addedPost.GroupId,
                UserId = addedPost.Owner.UserId
            };

            _controls.SendGroupMessage(message);
        }

        public int AddComment(Comments addedComment)
        {
            return _controls.InsertComment(addedComment, addedComment.MessageId);
        }

        public void RemoveComment(Comments comment)
        {
            _controls.RemoveComment(comment);
        }

        public void RemoveWallPost(WallPost removedPost)
        {
            var message = new GroupMessages
            {
                Content = removedPost.MessageContent, Created = removedPost.Created, GroupId = removedPost.GroupId,
                UserId = removedPost.Owner.UserId, GmsgId = removedPost.GmsgId
            };
            _controls.RemoveGroupMessage(message.GmsgId);
        }

        public void EditWallPost(WallPost post)
        {
            var message = new GroupMessages
            {
                Content = post.MessageContent, Created = post.Created, GroupId = post.GroupId,
                UserId = post.Owner.UserId, GmsgId = post.GmsgId
            };
            _controls.UpdateGroupMessageContent(message, message.Content);
        }

        public List<Group> GetUserGroups(User activeUser)
        {
            return _controls.GetUserGroups(activeUser);
        }

        public List<Group> GetGroupUsers(User activeUser)
        {
            return _controls.GetUserGroups(activeUser);
        }

        public List<Students> GetGroupUsers(int selectedGroupId)
        {
            return _controls.GetGroupUsers(selectedGroupId);
        }

        public void EditUser(User user)
        {
            _controls.UpdateUser(user);
        }

        public void RemoveStudentFromGroup(Students student, int groupId)
        {
            _controls.RemoveStudentFromGroup(student.StudentId, _controls.GetGroupById(groupId));
        }


        public void SendPrivateMessage(PrivateMessages newMessage)
        {
            _controls.SendMessage(newMessage);
        }

        public List<Teachers> GetProfessors()
        {
            return _controls.GetTeacherAll();
        }

        public List<Students> GetStudents()
        {
            return _controls.GetStudentAll();
        }

        public List<Classrooms> GetClassrooms()
        {
            return _controls.GetClassroomAll();
        }

        public List<Group> GetGroups()
        {
            return _controls.GetGroupAll();
        }

        public int AddGroup(Group group)
        {
            return _controls.InsertGroup(group);
        }

        public void RemoveGroup(int selectedGroupId)
        {
            _controls.RemoveGroup(selectedGroupId);
        }

        public void AddStudentsToGroup(List<Students> addedStudents, Group designatedGroup)
        {
            foreach (var student in addedStudents)
            {
                _controls.InsertStudentIntoGroup(student, designatedGroup);
            }
        }

        public void AddStudentToGroup(Students student, int designatedGroupId)
        {
            _controls.InsertStudentIntoGroup(student, designatedGroupId);
        }

        public Group GetGroupById(int selectedGroupId)
        {
            return _controls.GetGroupById(selectedGroupId);
        }

        public Teachers GetTeacherByTeacherId(int teacherId)
        {
            return _controls.GetTeacherById(teacherId);
        }

        public User GetUserById(int uid)
        {
            return _controls.GetUserById(uid);
        }

        public List<User> GetUsers()
        {
            return _controls.GetUserAll();
        }

        public Dictionary<int, List<Grades>> GetGrades(User activeUser)
        {
            var grades = _controls.GetGrades(_controls.GetStudentByUserId(activeUser.UserId));
            var g = grades.GroupBy(c => c.CourseId).ToDictionary(g => g.Key, m => m.Select(a => a).ToList());
            return g;
        }

        public Courses GetCourse(int courseId)
        {
            return _controls.GetCourseById(courseId);
        }

        public List<Timetables> GetUserSchedule(User activeUser)
        {
            var timetables = new List<Timetables>();
            var groups = _controls.GetUserGroups(activeUser);
            foreach (var group in groups)
            {
                var groupTimetable = _controls.GetTimetablesByGroup(group);
                if(groupTimetable != null)
                  timetables.Add(groupTimetable);
            }
            return timetables;
        }
    }
}