CREATE OR REPLACE VIEW VW_USERS_STATUS AS
SELECT *
FROM USERS_STATUS;

CREATE OR REPLACE VIEW VW_USERS AS
SELECT USERS.USER_ID,
       USERNAME,
       FIRST_NAME,
       MIDDLE_NAME,
       LAST_NAME,
       EMAIL,
       PROFILE_IMAGE     PROFILE_IMAGE_ID,
       FILES.FILE_DATA   PROFILE_IMAGE_DATA,
       USERS.STATUS_ID,
       USERS_STATUS.NAME STATUS_NAME,
       ADMIN
FROM USERS
         JOIN USERS_STATUS ON USERS.STATUS_ID = USERS_STATUS.STATUS_ID
         LEFT JOIN FILES ON USERS.PROFILE_IMAGE = FILES.FILE_ID;

CREATE OR REPLACE VIEW VW_STUDENTS AS
SELECT STUDENT_ID,
       STUDENTS.USER_ID,
       LAST_NAME || ', ' || FIRST_NAME || (CASE WHEN MIDDLE_NAME IS NOT NULL THEN ' ' || MIDDLE_NAME END) NAME,
       EMAIL,
       YEAR,
       STATUS_ID,
       STATUS_NAME
FROM STUDENTS
         JOIN VW_USERS ON VW_USERS.USER_ID = STUDENTS.USER_ID;

CREATE OR REPLACE VIEW VW_FILES AS
SELECT FILE_ID, FILES.USER_ID USER_ID, USERS.USERNAME USER_USERNAME, FIRST_NAME, FILE_TYPE, FILE_DATA, CREATED
FROM FILES
         JOIN USERS ON FILES.USER_ID = USERS.USER_ID;

CREATE OR REPLACE VIEW VW_TEACHERS AS
SELECT TEACHER_ID,
       TEACHERS.USER_ID,
       LAST_NAME || ', ' || FIRST_NAME || (CASE WHEN MIDDLE_NAME IS NOT NULL THEN ' ' || MIDDLE_NAME END) NAME,
       EMAIL,
       STATUS_ID,
       STATUS_NAME
FROM TEACHERS
         JOIN VW_USERS ON TEACHERS.USER_ID = VW_USERS.USER_ID;

CREATE OR REPLACE VIEW VW_CLASSROOMS AS
SELECT *
FROM CLASSROOMS;

CREATE OR REPLACE VIEW VW_COURSES AS
SELECT *
FROM COURSES;

CREATE OR REPLACE VIEW VW_GRADES AS
SELECT GRADE_ID,
       GRADES.STUDENT_ID,
       VW_STUDENTS.NAME      STUDENT_NAME,
       GRADES.TEACHER_ID,
       VW_TEACHERS.NAME      TEACHER_NAME,
       GRADES.COURSE_ID,
       VW_COURSES.SHORT_NAME COURSE_SHORT_NAME,
       VW_COURSES.FULL_NAME  COURSE_FULL_NAME,
       VALUE                 GRADE_VALUE,
       GRADES.DESCRIPTION    GRADE_DESCRIPTION,
       CREATED               GRADE_CREATED
FROM GRADES
         JOIN VW_STUDENTS ON GRADES.STUDENT_ID = VW_STUDENTS.STUDENT_ID
         JOIN VW_TEACHERS ON GRADES.TEACHER_ID = VW_TEACHERS.TEACHER_ID
         JOIN VW_COURSES ON GRADES.COURSE_ID = VW_COURSES.COURSE_ID;

CREATE OR REPLACE VIEW VW_GROUPS AS
SELECT GROUP_ID,
       GROUPS.TEACHER_ID,
       VW_TEACHERS.NAME                 TEACHER_NAME,
       GROUPS.NAME                      GROUP_NAME,
       MAX_CAPACITY,
       ACTUAL_CAPACITY,
       (MAX_CAPACITY - ACTUAL_CAPACITY) FREE_CAPACITY
FROM GROUPS
         JOIN VW_TEACHERS ON GROUPS.TEACHER_ID = VW_TEACHERS.TEACHER_ID;

CREATE OR REPLACE VIEW VW_GROUPS_COURSES AS
SELECT CG.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       VW_GROUPS.MAX_CAPACITY,
       VW_GROUPS.ACTUAL_CAPACITY,
       VW_GROUPS.FREE_CAPACITY,
       VW_GROUPS.TEACHER_ID,
       VW_GROUPS.TEACHER_NAME,
       CG.COURSE_ID,
       VW_COURSES.FULL_NAME   COURSE_FULL_NAME,
       VW_COURSES.SHORT_NAME  COURSE_SHORT_NAME,
       VW_COURSES.DESCRIPTION COURSE_DESCRIPTION
FROM COURSES_GROUPS CG
         JOIN VW_GROUPS ON CG.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_COURSES ON CG.COURSE_ID = VW_COURSES.COURSE_ID;

CREATE OR REPLACE VIEW VW_STUDENTS_GROUPS AS
SELECT SG.STUDENT_ID,
       VW_STUDENTS.NAME  STUDENT_NAME,
       VW_STUDENTS.EMAIL STUDENT_EMAIL,
       SG.GROUP_ID,
       VW_GROUPS.GROUP_NAME
FROM STUDENTS_GROUPS SG
         JOIN VW_STUDENTS ON SG.STUDENT_ID = VW_STUDENTS.STUDENT_ID
         JOIN VW_GROUPS ON SG.GROUP_ID = VW_GROUPS.GROUP_ID;

CREATE OR REPLACE VIEW VW_TIMETABLES AS
SELECT TIMETABLE_ID,
       TIMETABLES.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       ACTUAL_CAPACITY        GROUP_CAPACITY,
       TIMETABLES.CLASSROOM_ID,
       VW_CLASSROOMS.NAME     CLASSROOM_NAME,
       VW_CLASSROOMS.CAPACITY CLASSROOM_CAPACITY,
       TIMETABLES."BEGIN",
       TIMETABLES."END"
FROM TIMETABLES
         JOIN VW_GROUPS ON TIMETABLES.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_CLASSROOMS ON TIMETABLES.CLASSROOM_ID = VW_CLASSROOMS.CLASSROOM_ID;

CREATE OR REPLACE VIEW VW_GROUP_MESSAGES AS
SELECT GMSG_ID           MESSAGE_ID,
       GM.GROUP_ID,
       VW_GROUPS.GROUP_NAME,
       GM.USER_ID        AUTHOR_ID,
       VW_USERS.USERNAME AUTHOR_USERNAME,
       CONTENT,
       CREATED
FROM GROUP_MESSAGES GM
         JOIN VW_GROUPS ON GM.GROUP_ID = VW_GROUPS.GROUP_ID
         JOIN VW_USERS ON GM.USER_ID = VW_USERS.USER_ID;

CREATE OR REPLACE VIEW VW_COMMENTS AS
SELECT COMMENT_ID,
       COMMENTS.CONTENT                  COMMENT_CONTENT,
       COMMENTS.CREATED                  COMMENT_CREATED,
       COMMENTS.USER_ID                  COMMENT_AUTHOR_ID,
       VW_USERS.USERNAME                 COMMENT_AUTHOR_USERNAME,
       COMMENTS.MESSAGE_ID,
       VW_GROUP_MESSAGES.CONTENT         MESSAGE_CONTENT,
       VW_GROUP_MESSAGES.CREATED         MESSAGE_CREATED,
       VW_GROUP_MESSAGES.AUTHOR_ID       MESSAGE_AUTHOR_ID,
       VW_GROUP_MESSAGES.AUTHOR_USERNAME MESSAGE_AUTHOR_USERNAME
FROM COMMENTS
         JOIN VW_USERS ON COMMENTS.USER_ID = VW_USERS.USER_ID
         JOIN VW_GROUP_MESSAGES ON COMMENTS.MESSAGE_ID = VW_GROUP_MESSAGES.MESSAGE_ID;

CREATE OR REPLACE VIEW VW_PRIVATE_MESSAGES AS
SELECT PMSG_ID             MESSAGE_ID,
       FROM_USER           FROM_USER_ID,
       USERS_FROM.USERNAME FROM_USER_USERNAME,
       TO_USER             TO_USER_ID,
       USERS_TO.USERNAME   TO_USER_USERNAME,
       CONTENT             MESSAGE_CONTENT,
       CREATED             MESSAGE_CREATED
FROM PRIVATE_MESSAGES
         JOIN VW_USERS USERS_FROM ON FROM_USER = USERS_FROM.USER_ID
         JOIN VW_USERS USERS_TO ON TO_USER = USERS_TO.USER_ID

